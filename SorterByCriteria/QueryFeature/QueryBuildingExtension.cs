using System.Linq.Expressions;

namespace SorterByCriteria.QueryFeature;

internal static class QueryBuildingExtension
{
    internal static AdvancedQueryBuilt<T> BuildQuery<T>(this AdvancedQuery query)
    {
        return new AdvancedQueryBuilt<T>
        {
            Count = query.Count,
            Page = query.Page,
            Sorting = query.Sorting,
            Filters = query.Filters is not null
                ? Expression.Lambda<Func<T, bool>>(query.Filters.BuildFilter())
                : null
        };
    }
}