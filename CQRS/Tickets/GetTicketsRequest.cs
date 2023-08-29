using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Model;
using MediatR;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class GetTicketsRequest : IRequest<List<Ticket>>
    {
        // ReSharper disable once NullableWarningSuppressionIsUsed
        public string JsonQuery { get; set; } = null!;
        
        // Concert id
        public Guid ConcertId { get; set; }
        
        // Null, if anonymous
        public Guid? UserId { get; set; }
        
        // if Admin, returns really all tickets;
        public bool IsAdmin { get; set; }
    }
}
