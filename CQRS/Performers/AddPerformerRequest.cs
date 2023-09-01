using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using ViewModels;
using ViewModels.PostingModels;

namespace CQRS.Performers
{
    public class AddPerformerRequest : IRequest<Performer?>
    {
        public PerformerPostingModel Performer { get; set; }
        public PerformerType PerformerType { get; set; }
    }
}
