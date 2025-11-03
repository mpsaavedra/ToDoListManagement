using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// checks if is of a given type 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="checkType"></param>
    /// <returns></returns>
    public static bool IsOfType(this Type source, Type checkType) =>
        source.IsInstanceOfType(checkType);

    /// <summary>
    /// returns true if the objects is of provided type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public static bool IsInstanceOfType(this object obj, Type objectType)
    {
        return obj.GetType().IsOfType(objectType);
    }

    /// <summary>
    /// returns true if any of the provided values is Null or Empty
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(params object[] values)
    {
        if (values == null || !values.Any()) return true;

        var fails = values.Where(value =>
        {
            if (value == null)
            {
                return true;
            }

            if (value.IsInstanceOfType(typeof(string)))
            {
                return string.IsNullOrEmpty(((string)value));
            }

            if (value.IsInstanceOfType(typeof(IEnumerable<>)) ||
                value.IsInstanceOfType(typeof(ICollection)) ||
                value.IsInstanceOfType(typeof(ICollection<>)) ||
                value.IsInstanceOfType(typeof(IEnumerable)) ||
                value.IsInstanceOfType(typeof(Array)))
            {
                return NullOrEmpty(value);
            }

            return false;
        });

        return fails.Any();
    }

    /// <summary>
    /// checks if values are null or empty returns true if any it is
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool NullOrEmpty(params object?[] values)
    {
        if (!values.Any()) return true;
        var fails = values.Where(value =>
        {
            if (value == null) return true;

            if (value.IsInstanceOfType(typeof(string)))
                return string.IsNullOrEmpty((string)value);

            switch (value)
            {
                case string s when s.Length < 1:
                case Array a when a.Length < 1:
                case ICollection c when c.Count < 1:
                    //case IEnumerable e when !e.Cast<object>().Any():
                    return true;
                default:
                    return false;
            }
        });
        return fails.Any();
    }

    /// <summary>
    /// if source is null use set the optional value
    /// </summary>
    /// <param name="source"></param>
    /// <param name="optional"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource ThenIfNullOrEmpty<TSource>(this TSource source, TSource optional)
    {
        if (IsNullOrEmpty(source!))
            return optional;
        if (!source.IsNullable())
            return source;
        return source;

        // !NullOrEmpty(source) ? (
        //     source.IsNullable() ? source : source
        // ) : optional;
    }

    /// <summary>
    /// returns true if the source is nullable
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static bool IsNullable<TSource>(this TSource source)
    {
        if (source == null) return true;
        Type type = typeof(TSource);
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
        // return default(TSource) == null;
    }

    /// <summary>
    /// Populate an object with data from another object keeping the data of the original
    /// object if properties does not appear or are null or empty.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mappedData"></param>
    /// <param name="idName"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static TSource PopulateWithMappedData<TSource>(this TSource source, object mappedData,
        string? idName = null)
    {
        foreach (var dbProperty in source!.GetType().GetProperties()
                     .Where(x => x.CanWrite &&
                                 x.PropertyType.IsPublic))
        {
            if (mappedData.GetType().GetProperties().Any(p => p.Name == dbProperty.Name))
            {
                if (!string.IsNullOrEmpty(idName) && dbProperty.Name == idName) continue;

                var mappedValue = mappedData.GetType().GetProperty(dbProperty.Name)?.GetValue(mappedData);
                var sourceValue = source.GetType().GetProperty(dbProperty.Name)?.GetValue(source);
                sourceValue = mappedValue.ThenIfNullOrEmpty(sourceValue);
                if (dbProperty.Name == "RowVersion" || dbProperty.Name == "ConcurrencyStamp")
                    continue;
                source
                    .GetType()
                    .GetProperty(dbProperty.Name)?
                    .SetValue(source, sourceValue);
            }
        }

        return source;
    }
}
