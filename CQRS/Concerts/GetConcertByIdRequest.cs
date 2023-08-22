using MediatR;
using ViewModels;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts;

public class GetConcertByIdRequest : IRequest<ConcertSuperficial?>
{
    public Guid ConcertId { get; set; }
}