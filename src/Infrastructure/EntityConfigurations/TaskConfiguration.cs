using Bootler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppTask = Bootler.Domain.Entities.Task;

namespace Bootler.Infrastructure.EntityConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<AppTask>
{
    public void Configure(EntityTypeBuilder<AppTask> builder)
    {
        builder.ConfigureEntity();
        builder.Property(x => x.State).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.StateType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.DueDate).IsRequired(false);
        builder.HasMany(x => x.UserTasks).WithOne(X => X.Task).HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.SetNull);
        builder.HasQueryFilter(x => x.SoftDeleted == false);
    }
}
