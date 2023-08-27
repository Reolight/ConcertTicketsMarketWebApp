using SorterByCriteria.FilterFeature;

namespace SorterByCriteria.QueryFeature;

public class AdvancedQuery<TObject> : QueryBase<TObject>
{
    public FilterBase? Filters = null;
}