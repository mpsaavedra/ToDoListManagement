using Bootler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler;

/// <summary>
/// Entity class extensions, just to make easier development
/// or jusst because am a little bit lazy ;D
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// extension to easily configure the Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="generatedKeyValue"></param>
    /// <returns></returns>
    public static EntityTypeBuilder<T> ConfigureEntity<T>(
        this EntityTypeBuilder<T> builder, bool generatedKeyValue = true)
        where T : class, IEntity
    {
        if (generatedKeyValue)
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        else
            builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired(false);
        builder.Property(x => x.LastUpdatedAt).IsRequired(false);
        builder.Property(x => x.LastUpdatedBy).IsRequired(false);
        builder.HasKey(x => x.Id);
        return builder;
    }
}
