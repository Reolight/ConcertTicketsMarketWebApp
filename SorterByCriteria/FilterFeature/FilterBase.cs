using System.Linq.Expressions;

namespace SorterByCriteria.FilterFeature;

internal abstract class FilterBase
{
    internal Expression? Expression = null;
    
    internal abstract Expression BuildFilter();
}