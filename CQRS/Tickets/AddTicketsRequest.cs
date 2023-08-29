// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Model;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class AddTicketsRequest : IRequest<IEnumerable<Ticket>>
    {
        public Ticket TicketTemplate { get; set; } = null!;
        public int Count { get; set; }
    }
}
