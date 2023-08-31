using ConcertTicketsMarketModel.Model;
using MediatR;

namespace CQRS.Discounts
{
    public class AddDiscountRequest : Discount, IRequest<Discount?> { }
}
