namespace SorterByCriteria;

public static class FilterSorterPaginatorServiceExtensions
{
    public static (IEnumerable<TObject> ResultData, int MaxPages) ApplyFilterSortingPaginator<TObject>(
        this IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService, string query) =>
        fspService.GetFilteredSortedPaginatedData(queryable, query);

    public static IQueryable<TObject> GetFilteredSortedData<TObject>(this IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService, string jsonQuery)
        => fspService.GetFilteredSortedData(queryable, jsonQuery);
}