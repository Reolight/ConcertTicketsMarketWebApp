// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class AddTicketsRequest : IRequest<bool>
    {
        public Ticket TicketTemplate { get; set; } = null!;
        public int Count { get; set; }
    }
}
