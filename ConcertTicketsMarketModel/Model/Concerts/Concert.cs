// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Performers;

namespace ConcertTicketsMarketModel.Model.Concerts;

public class Concert
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ConcertType Type { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public AgeRating Rating { get; set; }
    public Location Place { get; set; } = null!;
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public Performer Performer { get; set; } = null!;
    public List<Discount> Promocodes { get; set; } = new List<Discount>();
}