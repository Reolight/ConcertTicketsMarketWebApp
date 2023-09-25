// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Performers;

namespace ViewModels.PostingModels;

public class PerformerPostingModel
{
    public Guid? Id { get; set; }
    public string Type { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;

    public string? Genre { get; set; }
    public List<Guid>? Performers { get; set; } = new();
    public string? VoiceType { get; set; }
}