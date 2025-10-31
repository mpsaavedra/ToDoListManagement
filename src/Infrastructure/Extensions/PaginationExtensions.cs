using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootler.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Extensions;

public static class PaginationExtensions
{
    /// <summary>
    /// returns a <see cref="PaginatedList{T}"/> object from the IQueryable source
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> source, int pageIndex, int pageSize)
        where TDestination : class =>
        PaginatedList<TDestination>.CreateAsync(source.AsNoTracking(), pageIndex, pageSize);
    /// <summary>
    /// Project a Queryable using adefined configuration as a List
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable<TDestination> source, IConfigurationProvider configuration)
        where TDestination : class =>
        source.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
