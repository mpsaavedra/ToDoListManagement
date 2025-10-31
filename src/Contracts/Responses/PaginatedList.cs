using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Responses;

/// <summary>
/// result of a given query, that include some effective information about the pagination
/// of the result.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class PaginatedList<TEntity>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IList<TEntity> Items { get; set; }
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex + 1 < TotalPages;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{TEntity}" /> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="pageIndex">The index of the page.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="indexFrom">The index from.</param>
    internal PaginatedList(IEnumerable<TEntity> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        if (source is IQueryable<TEntity> queryable)
        {
            PageIndex = pageIndex - 1;
            PageSize = pageSize;
            TotalCount = queryable.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize) - 1;
            //var skip = queryable.Skip(PageIndex * PageSize);
            //var take = skip.Take(PageSize).ToList();
            Items = queryable.Skip(PageIndex * PageSize).Take(PageSize).ToList();
            //Items = take;
        }
        else
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Items = source.Skip((PageIndex) * PageSize).Take(PageSize).ToList();
        }
    }

    public static async Task<PaginatedList<TEntity>> CreateAsync(IQueryable<TEntity> source, int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(new PaginatedList<TEntity>(source, pageIndex, pageSize, cancellationToken));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}" /> class.
    /// </summary>
    internal PaginatedList() => Items = new TEntity[0];
}