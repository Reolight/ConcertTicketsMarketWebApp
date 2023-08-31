using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;

namespace CQRS.Concerts
{
    public class AddConcertRequest : Concert, IRequest<Concert?> { }
}
