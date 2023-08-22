using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Model;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class AddDiscountRequest : Discount, IRequest<bool> { }
}
