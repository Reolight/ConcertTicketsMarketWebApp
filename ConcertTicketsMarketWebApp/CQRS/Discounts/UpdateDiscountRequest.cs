using ConcertTicketsMarketModel;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class UpdateDiscountRequest : Discount, IRequest<bool>
    {
    }
}
