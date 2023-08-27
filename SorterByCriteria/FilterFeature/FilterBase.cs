using System.Linq.Expressions;

namespace SorterByCriteria.FilterFeature;

public abstract class FilterBase
{
    internal Expression? Expression = null;
    
    internal abstract Expression BuildFilter<TObject>(ParameterExpression parameter);
}