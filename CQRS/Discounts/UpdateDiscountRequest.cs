using ConcertTicketsMarketModel.Model;
using MediatR;

namespace CQRS.Discounts
{
    public class UpdateDiscountRequest : Discount, IRequest<bool>
    {
    }
}
