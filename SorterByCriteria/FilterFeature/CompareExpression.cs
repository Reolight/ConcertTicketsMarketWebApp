using SorterByCriteria.FilterFeature.Enums;

namespace SorterByCriteria.FilterFeature;

public class CompareExpression<T>
{
    public CompareType CompareType { get; set; }
    public T Value { get; set; }
}