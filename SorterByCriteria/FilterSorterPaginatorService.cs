using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SorterByCriteria.DI;
using SorterByCriteria.QueryFeature;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria;

public class FilterSorterPaginatorService<TContext> : IFilterSorterPaginatorService
{
    private readonly TypeResolver<TContext> _typeResolver;
    private readonly ILogger<FilterSorterPaginatorService<TContext>> _logger;
    private readonly FilterSorterPaginatorConfigurations _sorterPaginatorConfigurations;

    public FilterSorterPaginatorService(
        TypeResolver<TContext> typeResolver,
        ILogger<FilterSorterPaginatorService<TContext>> logger,
        IOptions<FilterSorterPaginatorConfigurations> configurations)
    {
        _typeResolver = typeResolver;
        _logger = logger;
        _sorterPaginatorConfigurations = configurations.Value;
    }

    // private AdvancedQueryBuilt()
    private AdvancedQueryBuilt<T> BuildQuery<T>(string jsonQuery)
    {
        QueryDeserializer deserializer = new(_typeResolver);
        AdvancedQuery query = deserializer.Deserialize<T>(jsonQuery);
        return query.BuildQuery<T>();
    }

    private IQueryable<T> FilterAndSortIQueryable<T>(IQueryable<T> queryable, in AdvancedQueryBuilt<T> queryBuilt)
    {
        if (queryBuilt.Filters != null)
            queryable = queryable.Where(queryBuilt.Filters);
        return queryable.SortByCriteria(queryBuilt.Sorting);
    }
    
    public IQueryable<T> GetFilteredSortedData<T>(IQueryable<T> queryable, string jsonQuery)
    {
        var queryBuilt = BuildQuery<T>(jsonQuery);
        return FilterAndSortIQueryable(queryable, queryBuilt);
    }

    public (IEnumerable<T> resultData, int maxPages) GetFilteredSortedPaginatedData<T>(IQueryable<T> queryable, string jsonQuery)
    {
        var queryBuilt = BuildQuery<T>(jsonQuery);
        queryable = FilterAndSortIQueryable(queryable, queryBuilt);
        if (queryBuilt.Count == 0)
        {
            _logger.LogWarning("Count of elements on page is 0. Default count ({DefaultCount}) will be used",
                _sorterPaginatorConfigurations.DefaultCountOfElementsOnPage);
            queryBuilt.Count = _sorterPaginatorConfigurations.DefaultCountOfElementsOnPage;
        }

        return (queryable.Skip(queryBuilt.Page * queryBuilt.Count).Take(queryBuilt.Count).AsEnumerable(),
                (int)Math.Ceiling(queryable.Count() / (double)queryBuilt.Count));
    }
}