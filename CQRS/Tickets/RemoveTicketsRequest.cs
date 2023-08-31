using MediatR;

namespace CQRS.Tickets
{
    public class RemoveTicketsRequest : IRequest<bool>
    {
        public Guid TicketId { get; set; }
        public Guid IssuerId { get; set; }
    }
}
