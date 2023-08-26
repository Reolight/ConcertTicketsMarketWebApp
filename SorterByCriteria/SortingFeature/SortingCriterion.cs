namespace SorterByCriteria;

public struct SortingCriterion
{
    public readonly string FieldName { get; init; }
    public readonly bool IsAscending { get; init; }
}