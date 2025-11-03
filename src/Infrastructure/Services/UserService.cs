using AutoMapper;
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
using Microsoft.AspNetCore.Identity;

namespace Bootler.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly ICurrentUserService _currentUser;
    private readonly SignInManager<User> _signInManager;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
        ICurrentUserService currentUser, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = config;
        _currentUser = currentUser;
        _signInManager = signInManager;
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
                var users = await repo!.FindAsync(predicate: x => x.UserName == request.UserName, include: x => x.Include(y => y.Role));
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
                        //new (ClaimTypes.Role, role),
                        new (ClaimTypes.Name, request.UserName!),
                    };
                

                var token = new JwtSecurityToken(issuer: jwtIssuer, claims: authClaims,
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: creds);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                user.Token = tokenString;
                await repo.UpdateAsync(user.Id, user, cancellationToken);
                var userDto = _mapper.Map<UserDto>(user);

                await _signInManager.SignInAsync(user, request.RememberMe);

                return new SignInResponse(tokenString, userDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        });

    public Task<bool> SignOutAsync(CancellationToken cancellationToken = default) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                var repo = _unitOfWork.Repository<User>();
                var userId = _currentUser.GetUserId();
                if (userId == null) return true;

                var user = await repo.FirstOrDefaultAsync(x => x.Id == userId.Value);
                if(user == null) return true;
                user.Token = null;

                var updated = await repo.UpdateAsync(user.Id, user);
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        });

    public Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            Log.Debug($"Creating new user {request.UserName}");
            try
            {
                var repo = _unitOfWork.Repository<User>();
                var roleRepo = _unitOfWork.Repository<Role>();
                Log.Debug("Maping user request objetc to app user");
                var users = await repo!.FindAsync(predicate: x => x.UserName == request.UserName);
                if (users == null)
                    throw new Exception($"User {request.UserName} already exists");

                var role = await roleRepo.FirstOrDefaultAsync(x => x.Name == request.Role);
                if (role == null)
                    throw new Exception();

                var user = new User(request.UserName, request.Password, role);
                var id = await repo!.CreateAsync(user, cancellationToken);

                if (!id.HasValue)
                    throw new Exception($"An error has occurs Signing Up user {request.UserName}");

                return new SignUpResponse(id.Value, user.UserName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
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
