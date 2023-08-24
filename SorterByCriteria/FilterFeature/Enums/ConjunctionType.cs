using System.Linq.Expressions;

namespace SorterByCriteria.FilterFeature.Enums;

public enum ConjunctionType
{
    And,
    Or
}

public static class ConjunctionTypeExtension
{
    public static Expression ToConjunctionExpression(this ConjunctionType type, Expression left, Expression right)
        => type switch
        {
            ConjunctionType.And => Expression.AndAlso(left, right),
            ConjunctionType.Or => Expression.OrElse(left, right),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}