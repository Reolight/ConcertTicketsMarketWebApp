using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SorterByCriteria.DI;
using SorterByCriteria.QueryFeature;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria;

public class FilterSorterPaginatorService<TContext> : IFspDeveloper
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

    private IQueryable<T> FilterAndSortIQueryable<T>(IQueryable<T> queryable, in AdvancedQueryBuilt<T> queryBuilt) => 
        ApplyFilters(queryable, queryBuilt).SortByCriteria(queryBuilt.Sorting);

    public IQueryable<T> GetFilteredSortedData<T>(IQueryable<T> queryable, string jsonQuery)
    {
        var queryBuilt = ParseQuery<T>(jsonQuery).BuildQuery<T>();
        return FilterAndSortIQueryable(queryable, queryBuilt);
    }

    public (IEnumerable<T> ResultData, int MaxPages) GetFilteredSortedPaginatedData<T>(IQueryable<T> queryable, string jsonQuery)
    {
        var queryBuilt = ParseQuery<T>(jsonQuery).BuildQuery<T>();
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

    public AdvancedQuery ParseQuery<T>(string jsonQuery)
    {
        QueryDeserializer deserializer = new(_typeResolver);
        return deserializer.Deserialize<T>(jsonQuery);
    }

    public AdvancedQueryBuilt<T> BuildQuery<T>(AdvancedQuery query) => 
        query.BuildQuery<T>();

    public IQueryable<T> ApplyFilters<T>(IQueryable<T> queryable, AdvancedQueryBuilt<T> queryBuilt) => 
        queryBuilt.Filters != null ? queryable.Where(queryBuilt.Filters) : queryable;

    public IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, AdvancedQuery query) => 
        queryable.SortByCriteria(query.Sorting);

    public IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, AdvancedQueryBuilt<T> queryBuilt) =>
        queryable.SortByCriteria(queryBuilt.Sorting);

    public (IEnumerable<T> ResultData, int MaxPages) ApplyPagination<T>(IQueryable<T> queryable, int page, int count)
    {
        if (count == 0)
        {
            _logger.LogWarning("Count of elements on page is 0. Default count ({DefaultCount}) will be used",
                _sorterPaginatorConfigurations.DefaultCountOfElementsOnPage);
            count = _sorterPaginatorConfigurations.DefaultCountOfElementsOnPage;
        }

        return (queryable.Skip(page * count).Take(count).AsEnumerable(),
            (int)Math.Ceiling(queryable.Count() / (double)count));
    }

    public (IEnumerable<T> ResultData, int MaxPages) ApplyPagination<T>(IQueryable<T> queryable, AdvancedQuery query)
        => ApplyPagination(queryable, query.Page, query.Count);

    public (IEnumerable<T> ResultData, int MaxPages) ApplyPagination<T>(IQueryable<T> queryable,
        AdvancedQueryBuilt<T> queryBuilt)
        => ApplyPagination(queryable, queryBuilt.Page, queryBuilt.Count);
}