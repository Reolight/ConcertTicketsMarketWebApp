using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class AddPerformerRequest : IRequest<bool>
    {
        public Performer Performer { get; set; }
    }
}
