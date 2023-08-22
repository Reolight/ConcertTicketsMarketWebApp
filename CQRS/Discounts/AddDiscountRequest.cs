using ConcertTicketsMarketModel;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class AddDiscountRequest : Discount, IRequest<bool> { }
}
