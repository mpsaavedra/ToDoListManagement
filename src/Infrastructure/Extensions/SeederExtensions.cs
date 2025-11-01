using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppTask = Bootler.Domain.Entities.Task;
using AppRole = Bootler.Domain.Entities.Role;
using AppUser = Bootler.Domain.Entities.User;
using AppUserTask = Bootler.Domain.Entities.UserTask;
using Bootler.Domain.Entities.States;

namespace Bootler.Infrastructure.Extensions;

/// <summary>
/// Simple dtabase seeder
/// </summary>
public static class SeederExtensions
{
    /// <summary>
    /// Seed database with tests data
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var fact = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        var ctx = await fact.CreateDbContextAsync();

        #region Roles

        AppRole roleAdmin;
        AppRole roleUser;

        if(ctx.Roles.All(x => x.Name != "admin"))
        {
            roleAdmin = new AppRole
            {
                Name = "admin",
            };
            await ctx.Roles.AddAsync(roleAdmin);
            await ctx.SaveChangesAsync();
        }
        else
            roleAdmin = await ctx.Roles.FirstAsync(ctx => ctx.Name == "admin");

        if (ctx.Roles.Any(x => x.Name != "user"))
        {
            roleUser = new AppRole
            {
                Name = "user",
            };
            await ctx.Roles.AddAsync(roleUser);
            await ctx.SaveChangesAsync();
        }
        else
            roleUser = await ctx.Roles.FirstAsync(ctx => ctx.Name == "user");

        #endregion

        #region Tasks

        var tasks = new List<AppTask>();

        for (int i = 1; i < 15; i++)
        {
            if (ctx.Tasks.All(x => x.Title != $"Simple Task {i}"))
            {
                tasks[i] = new AppTask
                {
                    Title = $"Simple Task {i}",
                    Description = $"This is a simple task, constructed as a simple test of the application, the assigned number is {i}",
                    StateType = nameof(TaskCreated),
                };
                await ctx.Tasks.AddAsync(tasks[i]);
                await ctx.SaveChangesAsync();
            }
            else
                tasks[i] = await ctx.Tasks.FirstAsync(x => x.Title == $"Simple Task {i}");
        }

        #endregion

        #region Users

        AppUser admin;
        AppUser user1;
        AppUser user2;

        if(ctx.Users.All(x => x.UserName != "admin"))
        {
            admin = new AppUser
            {
                UserName = "admin",
                Password = "admin123.*",
                Role = roleAdmin,
            };
            await ctx.Users.AddAsync(admin);
            await ctx.SaveChangesAsync();
        } 
        else
            admin = await ctx.Users.FirstAsync( x => x.UserName == "admin");

        if (ctx.Users.All(x => x.UserName != "user1"))
        {
            user1 = new AppUser
            {
                UserName = "user1",
                Password = "user123.*",
                Role = roleAdmin,
            };

            foreach(var task in tasks.Slice(0, 5))
            {
                user1.Tasks.Add(new AppUserTask
                {
                    Task = task
                });
            }

            user1.Tasks.Add(new AppUserTask
            {
                Task = tasks[11]
            });
            user1.Tasks.Add(new AppUserTask
            {
                Task = tasks[12]
            });

            await ctx.Users.AddAsync(user1);
            await ctx.SaveChangesAsync();
        }
        else
            user1 = await ctx.Users.FirstAsync(x => x.UserName == "user1");

        if (ctx.Users.All(x => x.UserName != "user2"))
        {
            user2 = new AppUser
            {
                UserName = "user2",
                Password = "user123.*",
                Role = roleAdmin,
            };

            foreach (var task in tasks.Slice(6, 10))
            {
                user2.Tasks.Add(new AppUserTask
                {
                    Task = task
                });
            }

            user2.Tasks.Add(new AppUserTask
            {
                Task = tasks[13]
            });
            user2.Tasks.Add(new AppUserTask
            {
                Task = tasks[14]
            });

            await ctx.Users.AddAsync(user1);
            await ctx.SaveChangesAsync();
        }
        else
            user2 = await ctx.Users.FirstAsync(x => x.UserName == "user2");

        #endregion
    }
}
