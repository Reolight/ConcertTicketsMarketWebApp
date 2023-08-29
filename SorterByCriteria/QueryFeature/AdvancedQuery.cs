using SorterByCriteria.FilterFeature;

namespace SorterByCriteria.QueryFeature;

public class AdvancedQuery<TObject> : QueryBase
{
    public FilterBase? Filters = null;
}