using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class RemoveDiscountRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
