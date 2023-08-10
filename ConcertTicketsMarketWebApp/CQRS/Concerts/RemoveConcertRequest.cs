using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class RemoveConcertRequest : IRequest<bool>
    {
        public Guid ConcertId { get; set; }
    }
}
