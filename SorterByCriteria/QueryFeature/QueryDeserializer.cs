using System.Text.Json;
using SorterByCriteria.FilterFeature;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria.QueryFeature;

internal class QueryDeserializer
{
    private readonly ITypeResolver _typeResolver;

    internal QueryDeserializer(ITypeResolver typeResolver)
        => _typeResolver = typeResolver;
    
    internal AdvancedQuery<TObject> Deserialize<TObject>(string json)
    {
        AdvancedQuery<TObject> query = new AdvancedQuery<TObject>();
        using var document = JsonDocument.Parse(json);
        ParseBasicProperties(document, query);
        return query;
    }

    private int ParseInt32(in JsonDocument document, in string propertyName) =>
        !document.RootElement.TryGetProperty(propertyName, out var property) ? 
            0 :
            property.GetInt32();

    private void ParseBasicProperties<TObject>(in JsonDocument document, AdvancedQuery<TObject> query)
    {
        query.Page = ParseInt32(document, "page");
        query.Count = ParseInt32(document, "count");

        if (document.RootElement.TryGetProperty("sorting", out var sortingElement) &&
            sortingElement is { ValueKind: JsonValueKind.Array })
        {
            ParseSortingCriteria(sortingElement, query);
        }

        if (!document.RootElement.TryGetProperty("filters", out var filterElement) ||
            filterElement is not { ValueKind: JsonValueKind.Object }) return;
        
        // There is only one property can be at the top level!
        var filterProp = filterElement.EnumerateObject().First();
        ParseFilterCriteria(filterProp, query);
    }

    private void ParseFilterCriteria<TObject>(in JsonProperty filterProperty, AdvancedQuery<TObject> query, FilterBase? currentFilter = null)
    {
        var createdFilter = IsComplexFilter(filterProperty) ?
            CreateComplexFilter<TObject>(filterProperty) :
            CreateSimpleFilter<TObject>(filterProperty);
        query.Filters = createdFilter;
    }
    
    private bool IsComplexFilter(in JsonProperty filterProperty) => 
        filterProperty is { Name: "and" or "or" };

    private FilterBase CreateComplexFilter<TObject>(in JsonProperty property)
    {
        var complexFilter = new ComplexFilter { Conjunction = ConjunctionConverter.ToConjunctionType(property.Name) };
        foreach (var prop in property.Value.EnumerateArray()
                     .Select(jsonObj => jsonObj.EnumerateObject().First()))
        {
            complexFilter.Filters.Add(IsComplexFilter(prop)?
                CreateComplexFilter<TObject>(prop) :
                CreateSimpleFilter<TObject>(prop));
        }

        return complexFilter;
    }

    private FilterBase CreateSimpleFilter<TObject>(in JsonProperty property)
    {
        var propName = property.Name;
        var propType = _typeResolver.GetTypeOfProperty(typeof(TObject).Name, propName);
        if (property.Value is not { ValueKind: JsonValueKind.Object } value)
            throw new InvalidOperationException("Couldn't interpret not simple filter, cuz it has unknown format");
        
        var filterPropertyValue = value.EnumerateObject().First();
        Type genericFilter = typeof(SimpleFilter<>);
        Type[] typeArgs = { propType };
        Type constructedGenericFilterType = genericFilter.MakeGenericType(typeArgs);
        var simpleFilter = Activator.CreateInstance(constructedGenericFilterType) as ISimpleFilter
            ?? throw new InvalidOperationException($"{constructedGenericFilterType} couldn't be constructed");
        simpleFilter.Initialize(propName, filterPropertyValue.Name, filterPropertyValue.Value);

        return simpleFilter as FilterBase
               ?? throw new InvalidOperationException($"Something went wrong. Parsed: {property.ToString()}, gotten: ");
    }

    private void ParseSortingCriteria<TObject>(in JsonElement array, AdvancedQuery<TObject> query)
    {
        query.Sorting = array.Deserialize<List<SortingCriterion>>()
                        ?? throw new InvalidOperationException();
    }
}

// simple filter: { NAME: { FILTER: VALUE }}
// { "and" : [{},{},{"or": [{"age": {"gt":50}}, {}] }] }