using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;
using ViewModels.PostingModels;

namespace CQRS.Concerts
{
    public class AddConcertRequest : ConcertPostingModel, IRequest<Concert?> { }
}
