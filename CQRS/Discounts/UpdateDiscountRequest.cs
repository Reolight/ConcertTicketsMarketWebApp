using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Model;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class UpdateDiscountRequest : Discount, IRequest<bool>
    {
    }
}
