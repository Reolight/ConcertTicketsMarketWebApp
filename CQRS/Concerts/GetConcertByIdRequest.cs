using MediatR;
using ViewModels;

namespace CQRS.Concerts;

public class GetConcertByIdRequest : IRequest<ConcertSuperficial?>
{
    public Guid ConcertId { get; set; }
}