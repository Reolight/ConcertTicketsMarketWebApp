using Microsoft.Extensions.Logging;
using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public partial class FilterSorterPaginatorService<TContext>
{
    public AdvancedQuery<TObject> ParseQuery<TObject>(string? jsonQuery)
    {
        QueryDeserializer deserializer = new(_typeResolver);
        _cachedQuery = deserializer.Deserialize<TObject>(jsonQuery ??
                                                         _cachedJson ??
                                                         throw new NullReferenceException("There is no json to parse"));
        return (AdvancedQuery<TObject>)_cachedQuery;
    }

    public AdvancedQueryBuilt<TObject> BuildQuery<TObject>(AdvancedQuery<TObject>? query)
    {
        _cachedQuery = (query ?? _cachedQuery as AdvancedQuery<TObject> ?? ParseQuery<TObject>(_cachedJson))?.BuildQuery()
               ?? throw new NullReferenceException("Can't build query, because no suitable query found.");
        return (AdvancedQueryBuilt<TObject>)_cachedQuery;
    }

    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable,
        AdvancedQueryBuilt<TObject>? queryBuilt)
    {
        if (_cachedQuery is AdvancedQuery<TObject> advQ)
            BuildQuery(advQ);
        return (queryBuilt ?? _cachedQuery as AdvancedQueryBuilt<TObject>)?.Filters != null
            ? queryable.Where((queryBuilt ?? _cachedQuery as AdvancedQueryBuilt<TObject>)?.Filters
                              ?? throw new NullReferenceException("Can't apply filters because query doesnt exists"))
            : queryable;
    }

    public IQueryable<TObject>
            ApplySorting<TObject>(IQueryable<TObject> queryable, AdvancedQuery<TObject>? query) =>
            queryable.SortByCriteria((query ?? _cachedQuery)?.Sorting ??
                                     throw new NullReferenceException(
                                         "Can't check sorting criteria because no query found"));
        
    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable)
    {
        var (page, count) = (_cachedQuery?.Page ?? 0,
            _cachedQuery?.Count ?? _sorterPaginatorConfigurations.CountOfElementsOnPage);
        return (queryable.Skip(page * count).Take(count).AsEnumerable(),
            (int)Math.Ceiling(queryable.Count() / (double)count));
    }

    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable,
        int page, int count)
    {
        count = count == 0 ? _sorterPaginatorConfigurations.CountOfElementsOnPage : count;
        return (queryable.Skip(page * count).Take(count).AsEnumerable(),
            (int)Math.Ceiling(queryable.Count() / (double)count));
    }
}