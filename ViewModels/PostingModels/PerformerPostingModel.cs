// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Performers;

namespace ViewModels.PostingModels;

public class PerformerPostingModel
{
    public Guid Id { get; set; }
    public PerformerType PerformerType { get; set; }
    
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;

    public string? Genre { get; set; }
    public List<Guid>? Performers { get; set; } = new();
    public VoiceTypes? VoiceType { get; set; }
}