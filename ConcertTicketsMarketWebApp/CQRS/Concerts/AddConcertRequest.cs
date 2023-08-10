using ConcertTicketsMarketModel.Concerts;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class AddConcertRequest : Concert, IRequest<bool> { }
}
