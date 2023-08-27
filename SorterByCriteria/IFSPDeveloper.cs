using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public interface IFspDeveloper : IFilterSorterPaginatorService
{
    public AdvancedQuery<TObject> ParseQuery<TObject>(string jsonQuery);
    public AdvancedQueryBuilt<TObject> BuildQuery<TObject>(AdvancedQuery<TObject> query);
    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable, AdvancedQueryBuilt<TObject> queryBuilt);
    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable, AdvancedQuery<TObject> query);
    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable, AdvancedQueryBuilt<TObject> queryBuilt);
    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable, AdvancedQuery<TObject> query);
    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable, AdvancedQueryBuilt<TObject> queryBuilt);
}