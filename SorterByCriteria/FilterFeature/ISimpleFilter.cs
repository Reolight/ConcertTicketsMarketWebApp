using System.Text.Json;

namespace SorterByCriteria.FilterFeature;

public interface ISimpleFilter
{
    internal void Initialize(string propName, string compareExpr, JsonElement value);
}