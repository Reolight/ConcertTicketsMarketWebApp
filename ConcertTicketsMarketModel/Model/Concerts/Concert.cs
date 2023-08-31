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
    
    // Location. Decided not to flatten
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public Performer Performer { get; set; } = null!;
    public List<Discount> Promocodes { get; set; } = new List<Discount>();
}