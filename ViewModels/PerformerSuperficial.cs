// ReSharper disable NullableWarningSuppressionIsUsed
namespace ViewModels;

public class PerformerSuperficial
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public PerformerType PerformerType { get; set; }
}