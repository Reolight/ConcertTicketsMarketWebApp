// ReSharper disable NullableWarningSuppressionIsUsed

using System.Linq.Expressions;
using System.Text.Json;
using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class SimpleFilter<TProperty> : FilterBase, ISimpleFilter
{
    public string PropertyName = String.Empty;
    public CompareExpression<TProperty> CompareExpression = null!;
    
    public SimpleFilter() { }

    internal SimpleFilter(string propertyName, TProperty value, string compareType)
    {
        PropertyName = propertyName;
        CompareExpression = new CompareExpression<TProperty>
        {
            Value = value,
            CompareType = CompareConverter.ToCompareType(compareType)
        };
    }

    internal override Expression BuildFilter<TObject>(ParameterExpression parameter)
    {
        var property = Expression.Property(parameter, PropertyName);
        var constant = Expression.Constant(CompareExpression.Value, typeof(TProperty));
        Expression = CompareExpression.CompareType.ToBinaryExpression(
                property,
                constant
            );
        return Expression;
    }

    void ISimpleFilter.Initialize(string propertyName, string compareExpr, JsonElement value)
    {
        PropertyName = propertyName;
        CompareExpression = new CompareExpression<TProperty>
        {
            Value = (TProperty)value.Deserialize(typeof(TProperty))!,
            CompareType = CompareConverter.ToCompareType(compareExpr)
        };
    }
}