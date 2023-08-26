// ReSharper disable NullableWarningSuppressionIsUsed

namespace SorterByCriteria.DI;

public class FilterSorterPaginatorConfigurations
{
    public InspectionType ReflectOver { get; set; }

    public int DefaultCountOfElementsOnPage { get; set; } = 20;
}