using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public static class ConjunctionConverter
{
    public static ConjunctionType ToConjunctionType(string parsed)
        => parsed switch
        {
            "and" => ConjunctionType.And,
            "or" => ConjunctionType.Or,
            _ => throw new ArgumentOutOfRangeException(nameof(parsed), parsed, null)
        };
}