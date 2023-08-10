using ConcertTicketsMarketModel.Concerts;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class UpdateConcertRequest : Concert, IRequest<bool> { }
}
