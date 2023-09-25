using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using ViewModels.PostingModels;

namespace CQRS.Performers;

public class AddPerformerRequest : IRequest<Performer?>
{
    public PerformerPostingModel PostingModel { get; set; }
}