using System.Linq.Expressions;

namespace SorterByCriteria.QueryFeature;

public class AdvancedQueryBuilt<TObject> : QueryBase
{
    public Expression<Func<TObject, bool>>? Filters = null;
}