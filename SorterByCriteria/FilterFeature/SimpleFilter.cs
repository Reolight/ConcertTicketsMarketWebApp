// ReSharper disable NullableWarningSuppressionIsUsed

using System.Linq.Expressions;
using System.Text.Json;
using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class SimpleFilter<T> : FilterBase, ISimpleFilter
{
    public string PropertyName = String.Empty;
    public CompareExpression<T> CompareExpression = null!;
    
    public SimpleFilter() { }

    internal SimpleFilter(string propertyName, T value, string compareType)
    {
        PropertyName = propertyName;
        CompareExpression = new CompareExpression<T>
        {
            Value = value,
            CompareType = CompareConverter.ToCompareType(compareType)
        };
    }

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

    void ISimpleFilter.Initialize(string propertyName, string compareExpr, JsonElement value)
    {
        PropertyName = propertyName;
        CompareExpression = new CompareExpression<T>
        {
            Value = (T)value.Deserialize(typeof(T))!,
            CompareType = CompareConverter.ToCompareType(compareExpr)
        };
    }
}