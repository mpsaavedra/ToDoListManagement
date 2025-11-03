using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTask = Bootler.Domain.Entities.Task;
using AppRole = Bootler.Domain.Entities.Role;
using AppUser = Bootler.Domain.Entities.User;
using AppUserTask = Bootler.Domain.Entities.UserTask;
using Bootler.Domain.Entities.States;
using Microsoft.AspNetCore.Identity;

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
    public static async Task SeedDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateAsyncScope();
        // var fact = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        // await using var ctx = await fact.CreateDbContextAsync();
        await using var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userMng = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleMng = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        #region Roles

        AppRole roleAdmin;
        AppRole roleUser;

        if (ctx.Roles.All(r => r.Name != "admin"))
        {
            roleAdmin = new AppRole
            {
                Name = "admin",
            };
            //await ctx.Roles.AddAsync(roleAdmin);
            await roleMng.CreateAsync(roleAdmin);
            await ctx.SaveChangesAsync();
        }
        roleAdmin = await ctx.Roles.FirstAsync(r => r.Name == "admin");

        if (ctx.Roles.All(r => r.Name != "user"))
        {
            roleUser = new AppRole
            {
                Name = "user",
            };
            //await ctx.Roles.AddAsync(roleUser);
            await roleMng.CreateAsync(roleUser);
            await ctx.SaveChangesAsync();
        }
        roleUser = await ctx.Roles.FirstAsync(r => r.Name == "user");

        #endregion

        #region Tasks

        var tasks = new List<AppTask>();

        // create 15 tasks so later references tasks[0..14] are valid
        for (int i = 1; i <= 15; i++)
        {
            var title = $"Simple Task {i}";
            if (ctx.Tasks.All(t => t.Title != title))
            {
                var task = new AppTask
                {
                    Title = title,
                    Description = $"This is a simple task, constructed as a simple test of the application, the assigned number is {i}",
                    StateType = nameof(TaskCreated),
                };
                tasks.Add(task);
                await ctx.Tasks.AddAsync(task);
                await ctx.SaveChangesAsync();
            }
            else
            {
                var existing = await ctx.Tasks.FirstAsync(t => t.Title == title);
                tasks.Add(existing);
            }
        }

        #endregion

        #region Users

        AppUser admin;
        AppUser user1;
        AppUser user2;

        if (ctx.Users.All(u => u.UserName != "admin"))
        {
            admin = new AppUser
            {
                UserName = "admin",
                Password = "admin123.*",
                Role = roleAdmin,
            };
            //await ctx.Users.AddAsync(admin);
            await userMng.CreateAsync(admin);
            await ctx.SaveChangesAsync();
        }
        admin = await ctx.Users.FirstAsync(u => u.UserName == "admin");

        if (ctx.Users.All(u => u.UserName != "user1"))
        {
            user1 = new AppUser
            {
                UserName = "user1",
                Password = "user123.*",
                Role = roleUser,
            };

            foreach (var task in tasks.Skip(0).Take(5))
            {
                user1.Tasks.Add(new AppUserTask
                {
                    Task = task
                });
            }

            // indexes assume tasks list has at least 15 items (we created 15)
            user1.Tasks.Add(new AppUserTask { Task = tasks[11] });
            user1.Tasks.Add(new AppUserTask { Task = tasks[12] });

            //await ctx.Users.AddAsync(user1);
            await userMng.CreateAsync(user1);
            await ctx.SaveChangesAsync();
        }
        user1 = await ctx.Users.FirstAsync(u => u.UserName == "user1");

        if (ctx.Users.All(u => u.UserName != "user2"))
        {
            user2 = new AppUser
            {
                UserName = "user2",
                Password = "user123.*",
                Role = roleUser,
            };

            foreach (var task in tasks.Skip(6).Take(10))
            {
                user2.Tasks.Add(new AppUserTask
                {
                    Task = task
                });
            }

            user2.Tasks.Add(new AppUserTask { Task = tasks[13] });
            user2.Tasks.Add(new AppUserTask { Task = tasks[14] });

            //await ctx.Users.AddAsync(user2);
            await userMng.CreateAsync(user2);
            await ctx.SaveChangesAsync();
        }
        user2 = await ctx.Users.FirstAsync(u => u.UserName == "user2");

        #endregion
    }
}
