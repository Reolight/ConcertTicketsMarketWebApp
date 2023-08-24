using System.Collections.Concurrent;
using System.Reflection;
using SorterByCriteria.Attributes;

namespace SorterByCriteria.TypeResolver;

public class Resolver
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Type>?> _objectDescriptors = new();

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

    public void GetObjectDescriptors(object toInspect)
    {
        var objectName = toInspect.GetType().Name;
        _objectDescriptors.GetOrAdd(
            objectName,
            objName =>
            {
                var propsDescriptors = toInspect
                    .GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.GetProperty)
                    .Where(info => info.GetCustomAttribute<IgnoreDescriptionAttribute>() is null)
                    .Select(info => new KeyValuePair<string, Type>(info.Name, info.FieldType));
                return new ConcurrentDictionary<string, Type>(propsDescriptors);
            });
    }
    
    public void GetDescriptorsWithAttributes(object toInspect)
    {
        var objectName = toInspect.GetType().Name;
        _objectDescriptors.GetOrAdd(
            objectName,
            objName =>
                new ConcurrentDictionary<string, Type>(
                    toInspect
                        .GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.GetProperty)
                        .Where(info => info.GetCustomAttribute<UseDescriptionAttribute>() is not null)
                        .Select(info => new KeyValuePair<string, Type>(info.Name, info.FieldType))
                )
            );
    }
}