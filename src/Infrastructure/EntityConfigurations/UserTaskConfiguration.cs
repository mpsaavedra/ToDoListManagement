using Bootler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.EntityConfigurations;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ConfigureEntity();
        builder.HasOne(x => x.User).WithMany(x => x.Tasks).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Task).WithMany(x => x.UserTasks).HasForeignKey(x => x.TaskId);
        builder.HasOne(x => x.AsignedBy).WithMany(x => x.AssignedTasks).HasForeignKey(x => x.AssignedById);
        builder.HasQueryFilter(x => x.SoftDeleted == false);
    }
}
