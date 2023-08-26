namespace SorterByCriteria.TypeResolver;

public interface ITypeResolver
{
    Type GetTypeOfProperty(string @object, string property);
}