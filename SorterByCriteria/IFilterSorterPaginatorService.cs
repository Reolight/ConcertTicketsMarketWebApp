namespace SorterByCriteria;

public interface IFilterSorterPaginatorService
{
    /// <summary>
    /// Applies filtering and sorting criteria on Queryable and returns it; 
    /// </summary>
    /// <param name="queryable">Queryable on which criteria apply</param>
    /// <param name="jsonQuery">JSON string with criteria came from another service</param>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns>Queryable with filtering and sorting criteria applied</returns>
    public IQueryable<T> GetFilteredSortedData<T>(IQueryable<T> queryable, string jsonQuery);

    /// <summary>
    /// Applies filtering and sorting criteria on Queryable and returns paginated IEnumerable;
    /// </summary>
    /// <param name="queryable">Queryable on which criteria apply</param>
    /// <param name="jsonQuery">JSON string with criteria came from another service</param>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns>Tuple: first value is IEnumerable with result data, second value defines maximum pages with those filters</returns>
    public (IEnumerable<T> resultData, int maxPages) GetFilteredSortedPaginatedData<T>(IQueryable<T> queryable, string jsonQuery);
}