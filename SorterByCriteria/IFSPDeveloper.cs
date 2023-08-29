using SorterByCriteria.QueryFeature;

namespace SorterByCriteria;

public interface IFspDeveloper : IFilterSorterPaginatorService
{
    public AdvancedQuery<TObject> ParseQuery<TObject>(string? jsonQuery);
    public AdvancedQueryBuilt<TObject> BuildQuery<TObject>(AdvancedQuery<TObject>? query = null);
    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable, AdvancedQueryBuilt<TObject>? queryBuilt = null);
    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable, AdvancedQuery<TObject>? query = null);
}