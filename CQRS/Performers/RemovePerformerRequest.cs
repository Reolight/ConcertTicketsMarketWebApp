using MediatR;

namespace CQRS.Performers
{
    public class RemovePerformerRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
