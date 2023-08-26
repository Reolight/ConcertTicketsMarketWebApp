using System.Text.Json.Serialization;

namespace SorterByCriteria;

public struct SortingCriterion
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("isAsc")]
    public bool IsAsc { get; set; }
}