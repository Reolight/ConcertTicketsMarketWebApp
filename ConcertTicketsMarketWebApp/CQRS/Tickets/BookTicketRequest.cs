using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class BookTicketRequest : IRequest<bool>
    {
        public Guid TicketId { get; set; }
        public Guid BookerId { get; set; }
    }
}
