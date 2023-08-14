using ConcertTicketsMarketModel.Concerts;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts;

public class GetConcertByIdRequest : IRequest<Concert?>
{
    public Guid ConcertId { get; set; }
}