using Bootler.Domain.Entities;
using Bootler.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUser = Bootler.Domain.Entities.User;
using AppTask = Bootler.Domain.Entities.Task;

namespace Bootler.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {        
    }

    public AppDbContext() : base() { }

    public DbSet<Role> Roles { get; set; }
    public DbSet<AppTask> Tasks { get; set; }
    public DbSet<AppUser> Users {  get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
    }
}
