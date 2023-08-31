using MediatR;
using ViewModels;

namespace CQRS.Performers;

public class GetPerformerByIdRequest : IRequest<PerformerViewModel?>
{
    public Guid PerformerId { get; set; }
}