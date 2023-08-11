using ConcertTicketsMarketModel;
using MediatR;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class GetTicketsRequest : IRequest<List<Ticket>>
    {
        public List<SortingCriterion> SortingCriteria { get; set; } = new();
        
        // Concert id
        public Guid ConcertId { get; set; }
        
        // Null, if anonymous
        public Guid? UserId { get; set; }
        
        // if Admin, returns really all tickets;
        public bool IsAdmin { get; set; }
    }
}
