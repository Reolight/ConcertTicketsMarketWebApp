using System.Linq.Expressions;

namespace SorterByCriteria.FilterFeature.Enums;

public static class CompareConverter
{
    public static CompareType ToCompareType(string parsed)
        => parsed switch
        {
            "lt" => CompareType.LessThen,
            "le" => CompareType.LessEqual,
            "eq" => CompareType.Equal,
            "nq" => CompareType.NotEqual,
            "ge" => CompareType.GreaterEqual,
            "gt" => CompareType.GreaterThen,
            "li" => CompareType.Like,
            "nl" => CompareType.NotLike,
            _ => throw new InvalidCastException($"Can not cast {parsed} to CompareType")
        };

    public static string CompareTypeToString(CompareType compare)
        => compare switch
        {
            CompareType.LessThen => "lt",
            CompareType.LessEqual => "le",
            CompareType.Equal => "eq",
            CompareType.NotEqual => "nq",
            CompareType.GreaterEqual => "ge",
            CompareType.GreaterThen => "gt",
            CompareType.Like => "li",
            CompareType.NotLike => "nl",
            _ => throw new ArgumentOutOfRangeException(nameof(compare), compare, "Unknown CompareType value detected")
        };

    public static Expression ToBinaryExpression(this CompareType type, Expression left,
        Expression right)
        => type switch
        {
            CompareType.LessThen => Expression.LessThan(left, right),
            CompareType.LessEqual => Expression.LessThanOrEqual(left, right),
            CompareType.Equal => Expression.Equal(left, right),
            CompareType.NotEqual => Expression.NotEqual(left, right),
            CompareType.GreaterEqual => Expression.GreaterThanOrEqual(left, right),
            CompareType.GreaterThen => Expression.GreaterThan(left, right),
            CompareType.Like => throw new NotImplementedException(),
            CompareType.NotLike => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}