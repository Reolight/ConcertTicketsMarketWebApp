using MediatR;
using ViewModels;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class UpdateConcertRequest : ConcertSuperficial, IRequest<bool> { }
}
