using System.Reflection;
using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public static class FilterSorterPaginatorServiceExtensions
{
    public static (IEnumerable<TObject> ResultData, int MaxPages) ApplyFilterSortingPaginator<TObject>(
        this IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService, string query) =>
        fspService.GetFilteredSortedPaginatedData(queryable, query);
    
    public static (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService)
        WithJsonQuery<TObject>(this (IQueryable<TObject> queryable,
            IFilterSorterPaginatorService fspService) serviceTuple, string jsonQuery)
    {
        serviceTuple.fspService.ConsumeJsonQuery(jsonQuery);
        return serviceTuple;
    }

    public static IQueryable<TObject> ApplyFilterSorting<TObject>(this IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService, string jsonQuery)
        => fspService.GetFilteredSortedData(queryable, jsonQuery);

    public static IQueryable<TObject> ApplySorting<TObject>(this IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService)
        => fspService.ApplySorting(queryable);

    public static (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService)
        ApplySorting<TObject>(this (IQueryable<TObject> queryable,
            IFilterSorterPaginatorService fspService) serviceTuple)
        => (serviceTuple.fspService.ApplySorting(serviceTuple.queryable), serviceTuple.fspService);

    public static IQueryable<TObject> ApplyFilters<TObject>(this IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService)
        => fspService.ApplyFilters(queryable);
    
    public static (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService)
        ApplyFilters<TObject>(this (IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService) serviceTuple)
        => (serviceTuple.fspService.ApplyFilters(serviceTuple.queryable), serviceTuple.fspService);

    public static (IEnumerable<TObject>, int) ApplyPagination<TObject>(this IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService)
        => fspService.ApplyPagination(queryable);

    public static (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService)
        ApplyAction<TObject>(
            this (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService) serviceTuple,
            Func<IQueryable<TObject>, IQueryable<TObject>> action)
        => (action(serviceTuple.queryable), serviceTuple.fspService);
    
    public static (IQueryable<TProjected> queryable, IFilterSorterPaginatorService fspService)
        ApplyAction<TObject, TProjected>(
            this (IQueryable<TObject> queryable, IFilterSorterPaginatorService fspService) serviceTuple,
            Func<IQueryable<TObject>, IQueryable<TProjected>> action)
        => (action(serviceTuple.queryable), serviceTuple.fspService);
    
    public static (IEnumerable<TObject>, int) ApplyPagination<TObject>(this (IQueryable<TObject> queryable,
        IFilterSorterPaginatorService fspService) serviceTuple)
        => serviceTuple.fspService.ApplyPagination(serviceTuple.queryable);
}