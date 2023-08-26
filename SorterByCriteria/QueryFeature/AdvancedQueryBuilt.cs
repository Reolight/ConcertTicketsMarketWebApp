using System.Linq.Expressions;

namespace SorterByCriteria.QueryFeature;

public class AdvancedQueryBuilt<T> : QueryBase
{
    public Expression<Func<T, bool>>? Filters = null;
}