using AutoMapper;
using Azure.Core;
using Bootler;
using Bootler.Contracts.DTOs.Users;
using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using Bootler.Domain.Entities;
using Bootler.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._config = config;
    }

    public async Task<bool> IsAdminAsync(string userName, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<User>();
        var user = await repo!.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken: cancellationToken);
        if (user == null) return false;
        return user.Role.Name == "admin";
    }

    public Task<SignInResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                var repo = _unitOfWork.Repository<User>();
                var users = await repo!.FindAsync(predicate: x => x.UserName == request.UserName);
                if (users == null)
                    throw new Exception("");

                var user = await users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
                if (user == null)
                    throw new Exception("SignIn user could not not be null or empty");


                var jwtKey = _config["Jwt:Key"] ?? "ChangeThisSecretForDevOnly_ReplaceInProd";
                var jwtIssuer = _config["Jwt:Issuer"] ?? "BootlerAPI";
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var role = user.Role.Name.ToLower();

                var authClaims = new List<Claim>
                    {
                        new (ClaimTypes.Role, role),
                        new (ClaimTypes.Name, request.UserName!),
                    };
                

                var token = new JwtSecurityToken(issuer: jwtIssuer, claims: authClaims,
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: creds);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                user.Token = tokenString;
                await repo.UpdateAsync(user.Id, user, cancellationToken);
                var userDto = _mapper.Map<UserDto>(user);

                return new SignInResponse(tokenString, userDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        });

    public async Task<bool> SignOutAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            Log.Debug($"Creating new user {request.UserName}");
            try
            {
                var repo = _unitOfWork.Repository<User>();
                Log.Debug("Maping user request objetc to app user");
                var data = _mapper.Map<User>(request);
                var users = await repo!.FindAsync(predicate: x => x.UserName == data.UserName);
                if (users == null)
                    throw new Exception($"User {data.UserName} already exists");

                var id = await repo!.CreateAsync(data, cancellationToken);

                return new SignUpResponse(data.Id, data.UserName);
            }
            catch (Exception ex)
            {
                throw;
            }
        });

    public async Task<bool> VerifyPasswordAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<User>();
        var user = await repo.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken: cancellationToken);
        if (user == null) return false;
        return user.Password != password;
    }
}
