// ReSharper disable NullableWarningSuppressionIsUsed

using System.Linq.Expressions;
using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class ComplexFilter : FilterBase
{
    public ConjunctionType Conjunction { get; set; }
    public List<FilterBase> Filters { get; set; } = new();
    
    internal override Expression BuildFilter<TObject>(ParameterExpression parameter)
    {
        Filters.Aggregate(
            Expression,
            (expression, filter) =>
                Expression = Expression == null
                    ? filter.BuildFilter<TObject>(parameter)
                    : Conjunction.ToConjunctionExpression(expression!, filter.BuildFilter<TObject>(parameter))
        );

        return Expression!;
    }
}