namespace SorterByCriteria.QueryFeature;

public class QueryBase<TObject>
{
    public int Page { get; set; }  = 0;
    public int Count { get; set; }= 0;
    
    // ReSharper disable once NullableWarningSuppressionIsUsed
    public List<SortingCriterion> Sorting { get; set; } = new();
}