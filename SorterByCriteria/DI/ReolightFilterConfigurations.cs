// ReSharper disable NullableWarningSuppressionIsUsed

using Microsoft.EntityFrameworkCore;

namespace SorterByCriteria.DI;

public class ReolightFilterConfigurations
{
    public InspectionType ReflectOver { get; set; }
    public DbContext ReflectOn { get; set; } = null!;
}