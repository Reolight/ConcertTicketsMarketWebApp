using System.Text.Json;
using SorterByCriteria.FilterFeature;
using SorterByCriteria.FilterFeature.Enums;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria.QueryFeature;

internal class QueryDeserializer
{
    private readonly ITypeResolver _typeResolver;

    internal QueryDeserializer(ITypeResolver typeResolver)
        => _typeResolver = typeResolver;
    
    internal AdvancedQuery Deserialize<T>(string json)
    {
        AdvancedQuery query = new AdvancedQuery();
        using var document = JsonDocument.Parse(json);
        ParseBasicProperties<T>(document, query);
        return query;
    }

    private void ParseBasicProperties<T>(in JsonDocument document, AdvancedQuery query)
    {
        query.Page = document.RootElement.GetProperty("page").GetInt32();
        query.Count = document.RootElement.GetProperty("count").GetInt32();

        if (document.RootElement.TryGetProperty("sorting", out var sortingElement) &&
            sortingElement is { ValueKind: JsonValueKind.Array })
        {
            ParseSortingCriteria(sortingElement, query);
        }

        if (!document.RootElement.TryGetProperty("filters", out var filterElement) ||
            filterElement is not { ValueKind: JsonValueKind.Object }) return;
        // There is only one property can be at the top level!
        var filterProp = filterElement.EnumerateObject().First();
        ParseFilterCriteria<T>(filterProp, query);
    }

    private void ParseFilterCriteria<T>(in JsonProperty filterProperty, AdvancedQuery query, FilterBase? currentFilter = null)
    {
        var createdFilter = IsComplexFilter(filterProperty) ?
            CreateComplexFilter<T>(filterProperty) :
            CreateSimpleFilter<T>(filterProperty);
        query.Filters = createdFilter;
    }
    
    private bool IsComplexFilter(in JsonProperty filterProperty) => 
        filterProperty is { Name: "and" or "or" };

    private FilterBase CreateComplexFilter<T>(in JsonProperty property, ComplexFilter? complexFilter = null)
    {
        complexFilter ??= new ComplexFilter { Conjunction = Enum.Parse<ConjunctionType>(property.Name) };
        foreach (var prop in property.Value.EnumerateArray()
                     .Select(jsonObj => jsonObj.EnumerateObject().First()))
        {
            complexFilter.Filters.Add(IsComplexFilter(prop)?
                CreateComplexFilter<T>(prop, complexFilter) :
                CreateSimpleFilter<T>(prop));
        }

        return complexFilter;
    }

    private FilterBase CreateSimpleFilter<T>(in JsonProperty property)
    {
        var propName = property.Name;
        var propType = _typeResolver.GetTypeOfProperty(typeof(T).Name, propName);
        if (property.Value is not { ValueKind: JsonValueKind.Object } value)
            throw new InvalidOperationException("Couldn't interpret not simple filter, cuz it has unknown format");
        
        var filterPropertyValue = value.EnumerateObject().First();
        Type genericFilter = typeof(SimpleFilter<>);
        Type[] typeArgs = { propType };
        Type constructedGenericFilterType = genericFilter.MakeGenericType(typeArgs);
        var simpleFilter = Activator.CreateInstance(
            constructedGenericFilterType,
            propName,
            filterPropertyValue.Name,
            filterPropertyValue.Value.Deserialize(propType));
        return simpleFilter as FilterBase
               ?? throw new InvalidOperationException($"Something went wrong. Parsed: {property.ToString()}, gotten: ");
    }

    private void ParseSortingCriteria(in JsonElement array, AdvancedQuery query)
    {
        query.Sorting = array.Deserialize<List<SortingCriterion>>()
                        ?? throw new InvalidOperationException();
    }
}

// simple filter: { NAME: { FILTER: VALUE }}
// { "and" : [{},{},{"or": [{"age": {"gt":50}}, {}] }] }