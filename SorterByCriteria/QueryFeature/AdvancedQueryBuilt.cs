using System.Linq.Expressions;

namespace SorterByCriteria.QueryFeature;

internal class AdvancedQueryBuilt<T> : QueryBase
{
    public Expression<Func<T, bool>>? Filters = null;
}