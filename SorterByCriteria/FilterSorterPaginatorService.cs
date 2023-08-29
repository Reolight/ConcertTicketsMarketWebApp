using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SorterByCriteria.DI;
using SorterByCriteria.QueryFeature;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria;

public partial class FilterSorterPaginatorService<TContext> : IFspDeveloper
{
    private readonly TypeResolver<TContext> _typeResolver;
    private readonly ILogger<FilterSorterPaginatorService<TContext>> _logger;
    private readonly FilterSorterPaginatorConfigurations _sorterPaginatorConfigurations;

    private QueryBase? _cachedQuery = null;
    private string? _cachedJson = null;
    public FilterSorterPaginatorService(
        TypeResolver<TContext> typeResolver,
        ILogger<FilterSorterPaginatorService<TContext>> logger,
        IOptions<FilterSorterPaginatorConfigurations> configurations)
    {
        _typeResolver = typeResolver;
        _logger = logger;
        _sorterPaginatorConfigurations = configurations.Value;
    }

    public void ConsumeJsonQuery(string jsonQuery)
        => _cachedJson = jsonQuery;
    
    private IQueryable<TObject> FilterAndSortIQueryable<TObject>(IQueryable<TObject> queryable, in AdvancedQueryBuilt<TObject> queryBuilt) => 
        ApplyFilters(queryable, queryBuilt).SortByCriteria(queryBuilt.Sorting);

    public IQueryable<TObject> GetFilteredSortedData<TObject>(IQueryable<TObject> queryable, string jsonQuery)
    {
        var queryBuilt = ParseQuery<TObject>(jsonQuery).BuildQuery();
        return FilterAndSortIQueryable(queryable, queryBuilt);
    }

    public (IEnumerable<TObject> ResultData, int MaxPages) GetFilteredSortedPaginatedData<TObject>(IQueryable<TObject> queryable, string jsonQuery)
    {
        var queryBuilt = ParseQuery<TObject>(jsonQuery).BuildQuery();
        queryable = FilterAndSortIQueryable(queryable, queryBuilt);
        return ApplyPagination(queryable, queryBuilt.Page, queryBuilt.Count);
    }

    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable)
    {
        return ApplyFilters(queryable, null);
    }

    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable)
    {
        return ApplySorting(queryable, null);
    }
}