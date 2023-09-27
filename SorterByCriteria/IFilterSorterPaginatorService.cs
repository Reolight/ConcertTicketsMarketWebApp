namespace SorterByCriteria;

public interface IFilterSorterPaginatorService
{
    /// <summary>
    /// Applies filtering and sorting criteria on Queryable and returns it; 
    /// </summary>
    /// <param name="queryable">Queryable on which criteria apply</param>
    /// <param name="jsonQuery">JSON string with criteria came from another service</param>
    /// <typeparam name="TObject">Type of object</typeparam>
    /// <returns>Queryable with filtering and sorting criteria applied</returns>
    public IQueryable<TObject> GetFilteredSortedData<TObject>(IQueryable<TObject> queryable, string jsonQuery);

    /// <summary>
    /// Applies filtering and sorting criteria on Queryable and returns paginated IEnumerable;
    /// </summary>
    /// <param name="queryable">Queryable on which criteria apply</param>
    /// <param name="jsonQuery">JSON string with criteria came from another service</param>
    /// <typeparam name="TObject">Type of object</typeparam>
    /// <returns>Tuple: first value is IEnumerable with result data, second value defines maximum pages with those filters</returns>
    public (IEnumerable<TObject> ResultData, int MaxPages) GetFilteredSortedPaginatedData<TObject>(IQueryable<TObject> queryable, string jsonQuery);

    public void ConsumeJsonQuery(string? jsonQuery);
    public IQueryable<TObject> ApplyFilters<TObject>(IQueryable<TObject> queryable);
    public IQueryable<TObject> ApplySorting<TObject>(IQueryable<TObject> queryable);
    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable, int page, int count);
    public (IEnumerable<TObject> ResultData, int MaxPages) ApplyPagination<TObject>(IQueryable<TObject> queryable);
}