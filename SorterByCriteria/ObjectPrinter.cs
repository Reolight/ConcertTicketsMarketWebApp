using System.Reflection;
using System.Text;

namespace SorterByCriteria;

internal static class ObjectLogger {
    
    internal static string GetPropertyDescriptor(object? @object, Tabulation? tabs = null)
    {
        tabs ??= new Tabulation();
        StringBuilder builder = new StringBuilder();
        if (@object is not { } obj)
            return $"{nameof(@object)} is Null";
        var objType = obj.GetType();
        builder.Append($"{objType.Name} {{\n");
        tabs++;
        foreach (var field in objType.GetFields(BindingFlags.Public))
        {
            builder.Append(new string('\t', tabs) + $"\"{field.Name}\": \"{GetValue(@object, field, tabs)};\n");
        }

        builder.Append("}\n");
        // ReSharper disable once RedundantAssignment
        tabs--; // It is used in recursion
        return builder.ToString();
    }

    private static string GetValue(object obj, FieldInfo fieldInfo, Tabulation tabs)
    {
        if (fieldInfo.GetValue(obj) is not { } fieldValue)
            return "null;\n";
        if (fieldValue.GetType() is not { } type ||
            (type != typeof(string) && !type.IsValueType))

            return GetPropertyDescriptor(fieldValue, tabs);
        return fieldValue.ToString() ??
               throw new NullReferenceException($"there is no value in {fieldInfo.Name}...");
    }

    // mostly works as closure.
    internal class Tabulation
    {
        private int _tab = 0;

        public static Tabulation operator ++(Tabulation tabulation)
        {
            tabulation._tab++;
            return tabulation;
        }

        public static Tabulation operator --(Tabulation tabulation)
        {
            tabulation._tab--;
            return tabulation;
        }
        
        public static implicit operator int(Tabulation tabulation) => tabulation._tab;
    }

}