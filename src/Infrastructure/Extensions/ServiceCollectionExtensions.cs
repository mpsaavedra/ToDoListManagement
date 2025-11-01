using Bootler.Events;
using Bootler.Infrastructure.Behaviours;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Repositories;
using Bootler.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Bootler.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBootlerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();
        services.AddOptions();
        services.AddAutoMapper(typeof(AutoMapping));
        services.AddValidatorsFromAssembly(typeof(AppDbContext).Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AppDbContext).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services
                .AddPooledDbContextFactory<AppDbContext>(cfg =>
                cfg
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                    cfg =>
                    {
                        cfg.MigrationsAssembly(typeof(AppDbContext).Assembly);
                        cfg.EnableRetryOnFailure(
                            10,
                            TimeSpan.FromSeconds(3, 10, 0),
                            null);
                    })
            );
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserService, UserService>();

        services.AddAuthenticationServices(configuration);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DomainEventDispatcher).Assembly);
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
            IConfiguration configuration)
    {
        // JWT Auth
        var jwtKey = configuration["Jwt:Key"] ?? "800249BD-D2D8-4F43-8818-BF0B5334E2CB";
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "BootlerApi";
        var key = Encoding.ASCII.GetBytes(jwtKey);
        services
            .AddAuthorization(opts =>
            {
                opts.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim("UserId");
                });
            })
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        var msg = "The access token provided is not valid. " +
                                  context.Exception.GetBaseException().Message;
                        var stackTrace = context.Exception.StackTrace;
                        var text = JsonSerializer.Serialize(new ApplicationException(msg));
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", (StringValues)"true");
                            text = JsonSerializer.Serialize(new ApplicationException(
                                "The access token provided has expired. " +
                                context.Exception.GetBaseException().Message));
                        }

                        context.Response.WriteAsync(text);
                        return Task.CompletedTask;
                    }
                };
            });
        return services;
    }

    public static WebApplication UseBootlerServices(this WebApplication app)
    {
        return app;
    }
}
