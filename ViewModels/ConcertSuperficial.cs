// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketModel.Model.Concerts;

namespace ViewModels;

public class ConcertSuperficial
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ConcertType Type { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public AgeRating Rating { get; set; }
    public Location Place { get; set; } = null!;
    public PerformerSuperficial Performer { get; set; } = null!;
}