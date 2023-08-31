// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model;
using MediatR;

namespace CQRS.Tickets
{
    public class AddTicketsRequest : IRequest<IEnumerable<Ticket>>
    {
        public Ticket TicketTemplate { get; set; } = null!;
        public int Count { get; set; }
    }
}
