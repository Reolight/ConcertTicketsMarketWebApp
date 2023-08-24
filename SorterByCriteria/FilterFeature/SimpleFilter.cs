// ReSharper disable NullableWarningSuppressionIsUsed

using System.Linq.Expressions;
using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class SimpleFilter<T> : FilterBase
{
    public string PropertyName = String.Empty;
    public CompareExpression<T> CompareExpression = null!;
    
    internal override Expression BuildFilter()
    {
        var parameter = Expression.Parameter(typeof(T), "obj");
        base.Expression = Expression.Lambda<Func<T, bool>>(
            CompareExpression.CompareType.ToBinaryExpression(
                Expression.Property(parameter, PropertyName),
                Expression.Constant(CompareExpression.Value, typeof(T))
            )
            , parameter);
        return Expression;
    }
}