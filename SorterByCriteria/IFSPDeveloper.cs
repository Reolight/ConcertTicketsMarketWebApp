using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public interface IFspDeveloper : IFilterSorterPaginatorService
{
    public AdvancedQuery ParseQuery<T>(string jsonQuery);
    public AdvancedQueryBuilt<T> BuildQuery<T>(AdvancedQuery query);
    public IQueryable<T> ApplyFilters<T>(IQueryable<T> queryable, AdvancedQueryBuilt<T> queryBuilt);
    public IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, AdvancedQuery query);
    public IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, AdvancedQueryBuilt<T> queryBuilt);
    public (IEnumerable<T> ResultData, int MaxPages) ApplyPagination<T>(IQueryable<T> queryable, AdvancedQuery query);
    public (IEnumerable<T> ResultData, int MaxPages) ApplyPagination<T>(IQueryable<T> queryable, AdvancedQueryBuilt<T> queryBuilt);
}