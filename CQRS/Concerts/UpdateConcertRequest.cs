using MediatR;
using ViewModels;

namespace CQRS.Concerts
{
    public class UpdateConcertRequest : ConcertSuperficial, IRequest<bool> { }
}
