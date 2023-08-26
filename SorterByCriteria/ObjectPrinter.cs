using System.Reflection;
using System.Text;

namespace SorterByCriteria;

internal static class ObjectLogger {
    
    internal static string GetPropertyDescriptor(object? @object, int tabs = 0)
    {
        StringBuilder builder = new StringBuilder();
        if (@object is not { } obj)
            return $"{nameof(@object)} is Null";
        var objType = obj.GetType();
        builder.Append($"{objType.Name} {{");
        tabs++;
        foreach (var field in objType.GetFields(BindingFlags.Public))
        {
            builder.Append(new string('\t', tabs) + $"\"{field.Name}\": \"{GetValue(@object, field, tabs)}");
        }

        return builder.ToString();
    }

    private static string GetValue(object obj, FieldInfo fieldInfo, int tabs)
    {
        if (fieldInfo.GetValue(obj) is not { } fieldValue)
            return "null";
        if (fieldValue.GetType() is not { } type ||
            (type != typeof(string) && !type.IsValueType))

            return GetPropertyDescriptor(fieldValue, tabs);
        return fieldValue.ToString() ??
               throw new NullReferenceException($"there is no value in {fieldInfo.Name}...");
    }
}