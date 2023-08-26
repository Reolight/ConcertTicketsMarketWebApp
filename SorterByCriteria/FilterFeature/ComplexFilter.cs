// ReSharper disable NullableWarningSuppressionIsUsed

using System.Linq.Expressions;
using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class ComplexFilter : FilterBase
{
    public ConjunctionType Conjunction { get; set; }
    public List<FilterBase> Filters { get; set; } = null!;
    
    internal override Expression BuildFilter()
    {
        Filters.Aggregate(
            Expression,
            (expression, filter) =>
                Expression = Expression == null
                    ? filter.BuildFilter()
                    : Conjunction.ToConjunctionExpression(expression!, filter.BuildFilter())
        );

        return Expression!;
    }
}