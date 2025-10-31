using Bootler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ConfigureEntity();
        builder.Property(x => x.UserName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Token).IsRequired(false);
        builder.HasMany(x => x.Tasks).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.AssignedTasks).WithOne(x => x.AsignedBy).HasForeignKey(x => x.AsignedById);
        builder.HasIndex(x => x.UserName).IsUnique().HasDatabaseName("IX_User_Username_Id");
    }
}
