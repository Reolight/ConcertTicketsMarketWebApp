// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel.Model.Performers;

namespace ViewModels;

public class PerformerViewModel
{
    public Guid Id { get; set; }
    public PerformerType PerformerType { get; set; }
    
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;
    public List<ConcertSuperficial> Concerts { get; set; } = new();
    
    public string? Genre { get; set; }
    public List<PerformerSuperficial>? Performers { get; set; } = new();
    public VoiceTypes? VoiceType { get; set; }
}