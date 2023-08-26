namespace SorterByCriteria.QueryFeature;

public class QueryBase
{
    public int Page { get; set; }  = 0;
    public int Count { get; set; }= 0;
    public List<SortingCriterion> Sorting { get; set; }= new();
}