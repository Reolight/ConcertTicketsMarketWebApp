using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketModel.Model.Concerts;

namespace ViewModels.PostingModels;

public class ConcertPostingModel
{
    public string Name { get; set; } = null!;
    public ConcertType Type { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public AgeRating Rating { get; set; }
    
    // Location. Decided not to flatten
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public List<TicketTemplate> Tickets { get; set; } = new();
    public Guid Performer { get; set; }
    public List<Discount> Promocodes { get; set; } = new();
}