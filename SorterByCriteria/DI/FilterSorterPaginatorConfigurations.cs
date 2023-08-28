// ReSharper disable NullableWarningSuppressionIsUsed

namespace SorterByCriteria.DI;

public class FilterSorterPaginatorConfigurations
{
    public static readonly int DefaultCountOfElementsOnPage = 20;
    public static readonly InspectionType DefaultReflectOver = InspectionType.Properties;
    public InspectionType ReflectOver { get; set; } = DefaultReflectOver;

    public int CountOfElementsOnPage { get; set; } = DefaultCountOfElementsOnPage;
}