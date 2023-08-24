namespace SorterByCriteria.QueryFeature;

public class QueryBase
{
    public int Page = 1;
    public int Count = 10;
    public List<SortingCriterion> Sorting = new();
}