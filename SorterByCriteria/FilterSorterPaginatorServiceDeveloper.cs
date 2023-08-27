using Microsoft.Extensions.Logging;
using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public partial class FilterSorterPaginatorService<TContext>
{
    public AdvancedQuery<TObject> ParseQuery<TObject>(string jsonQuery)
    {
        QueryDeserializer deserializer = new(_typeResolver);
        return deserializer.Deserialize<TObject>(jsonQuery);
    }

    public AdvancedQueryBuilt<TObject> BuildQuery<TObject>(AdvancedQuery<TObject> query) =>
        query.BuildQuery<TObject>();

    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable,
        AdvancedQueryBuilt<TObject> queryBuilt) =>
        queryBuilt.Filters != null ? queryable.Where(queryBuilt.Filters) : queryable;

    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable, AdvancedQuery<TObject> query) =>
        queryable.SortByCriteria(query.Sorting);

    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable,
        AdvancedQueryBuilt<TObject> queryBuilt) =>
        queryable.SortByCriteria(queryBuilt.Sorting);

    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable,
        int page, int count)
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

    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable,
        AdvancedQuery<TObject> query)
        => ApplyPagination(queryable, query.Page, query.Count);

    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(
        IQueryable<TObject> queryable,
        AdvancedQueryBuilt<TObject> queryBuilt)
        => ApplyPagination(queryable, queryBuilt.Page, queryBuilt.Count);
}