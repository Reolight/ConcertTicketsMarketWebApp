using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Options;
using SorterByCriteria.Attributes;
using SorterByCriteria.DI;

namespace SorterByCriteria.TypeResolver;

public class TypeResolver<TContext> : ITypeResolver
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Type>?> _objectDescriptors = new();

    public TypeResolver(IOptions<FilterSorterPaginatorConfigurations> configurations)
    {
        switch (configurations.Value.ReflectOver)
        {
            case InspectionType.Properties:
                GetObjectDescriptors(typeof(TContext));
                break;
            case InspectionType.Attributes:
                GetDescriptorsWithAttributes(typeof(TContext));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public Type GetTypeOfProperty(string @object, string property)
    {
        if (!_objectDescriptors.TryGetValue(@object, out var issuedDescriptor)
            || issuedDescriptor is not { } descriptor)
            throw new NullReferenceException($"There is no descriptor for object {@object}");

        if (!descriptor.TryGetValue(property, out var issuedPropertyDescriptor) ||
            issuedPropertyDescriptor is not { } propertyDescriptor)
            throw new NullReferenceException($"There is no descriptor for property {property} of object {@object}");

        return propertyDescriptor;
    }

    private void GetObjectDescriptors(Type dbToInspect)
    {
        var objectName = dbToInspect.Name;
        _objectDescriptors.GetOrAdd(
            objectName,
            objName =>
            {
                var propsDescriptors = dbToInspect
                    .GetFields(BindingFlags.Public | BindingFlags.GetProperty)
                    .Where(info => info.GetCustomAttribute<IgnoreDescriptionAttribute>() is null)
                    .Select(info => new KeyValuePair<string, Type>(info.Name, info.FieldType));
                return new ConcurrentDictionary<string, Type>(propsDescriptors);
            });
    }

    private void GetDescriptorsWithAttributes(Type dbToInspect)
    {
        var objectName = dbToInspect.Name;
        _objectDescriptors.GetOrAdd(
            objectName,
            objName =>
                new ConcurrentDictionary<string, Type>(
                    dbToInspect
                        .GetFields(BindingFlags.Public | BindingFlags.GetProperty)
                        .Where(info => info.GetCustomAttribute<UseDescriptionAttribute>() is not null)
                        .Select(info => new KeyValuePair<string, Type>(info.Name, info.FieldType))
                )
            );
    }
}