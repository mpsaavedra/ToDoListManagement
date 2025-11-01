using Bootler.Domain.Entities;
using Bootler.Events;
using Bootler.Infrastructure.EntityConfigurations;
using Bootler.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTask = Bootler.Domain.Entities.Task;
using AppUser = Bootler.Domain.Entities.User;

namespace Bootler.Infrastructure;

public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;
    //private readonly ICurrentUserService _currentUser;

    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher
        /*, ICurrentUserService currentUser*/) : base(options)
    {
        _dispatcher = dispatcher;
        //_currentUser = currentUser;
    }

    public AppDbContext() : base() { }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<AppTask> Tasks { get; set; }
    public DbSet<AppUser> Users {  get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    public IDomainEventDispatcher Dispatcher => _dispatcher;
    //public long? CurrentUserId => _currentUser.GetUserId();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=todo_db;Username=todo_user;Password=todo_pwd;Include Error Detail=true",
            opts =>
                opts.MigrationsAssembly("Bootler.Api"));

        return new AppDbContext(optionsBuilder.Options);
    }
}