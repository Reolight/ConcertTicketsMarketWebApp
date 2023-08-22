using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Performers;

public class GetPerformerByIdRequest : IRequest<Performer?>
{
    public Guid PerformerId { get; set; }
}