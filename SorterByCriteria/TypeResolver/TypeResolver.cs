using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using SorterByCriteria.Attributes;
using SorterByCriteria.DI;

namespace SorterByCriteria.TypeResolver;

public class TypeResolver<TContext> : ITypeResolver
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Type>?> _objectDescriptors = new();

    public TypeResolver(IOptions<FilterSorterPaginatorConfigurations> configurations)
    {
        ExtractSuitableTypes(typeof(TContext), configurations.Value.ReflectOver)
            .AsParallel()
            .ForAll(suitableType => GetObjectDescriptors(suitableType, configurations.Value.ReflectOver));
    }

    public Type GetTypeOfProperty(string @object, string property)
    {
        @object = @object.ToLowerInvariant();
        property = property.ToLowerInvariant();
        
        if (!_objectDescriptors.TryGetValue(@object, out var issuedDescriptor)
            || issuedDescriptor is not { } descriptor)
            throw new NullReferenceException($"There is no descriptor for object {@object}");

        if (!descriptor.TryGetValue(property, out var issuedPropertyDescriptor) ||
            issuedPropertyDescriptor is not { } propertyDescriptor)
            throw new NullReferenceException($"There is no descriptor for property {property} of object {@object}");

        return propertyDescriptor;
    }

    private IEnumerable<FieldInfo> ExtractSuitableFields(Type typeToExtractFrom, InspectionType inspectionType) =>
        typeToExtractFrom.GetRuntimeFields()
            .Where(info => inspectionType switch
            {
                InspectionType.Properties =>
                    info.GetCustomAttribute<IgnoreDescriptionAttribute>() is null,
                InspectionType.Attributes =>
                    info.GetCustomAttribute<UseDescriptionAttribute>() is not null,
                _ => throw new ArgumentOutOfRangeException(nameof(inspectionType), inspectionType, null)
            });


    private List<Type> ExtractSuitableTypes(Type typeToExtractFrom, InspectionType inspectionType) =>
        ExtractSuitableFields(typeToExtractFrom, inspectionType)
            .Select(info => info.FieldType)
            .Where(type => type.IsGenericType)
            .Select(type => type.GenericTypeArguments.First())
            .ToList();

    private readonly Regex _nameExtractor = new Regex("(?<=<)\\w+(?=>)");
    private void GetObjectDescriptors(Type toInspect, InspectionType inspectionType)
    {
        var objectName = toInspect.Name.ToLowerInvariant();
        _objectDescriptors.GetOrAdd(
            objectName,
            objName =>
            {
                var propsDescriptors = ExtractSuitableFields(toInspect, inspectionType)
                    .Select(info =>
                    {
                        var match = _nameExtractor.Match(info.Name);
                        var name = match.Success ? match.Value : info.Name;
                        return new KeyValuePair<string, Type>(name.ToLowerInvariant(), info.FieldType);
                    });
                return new ConcurrentDictionary<string, Type>(propsDescriptors);
            });
    }
}