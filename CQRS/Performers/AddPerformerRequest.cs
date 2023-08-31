using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

namespace CQRS.Performers
{
    public class AddPerformerRequest : IRequest<Performer?>
    {
        public Performer Performer { get; set; }
    }
}
