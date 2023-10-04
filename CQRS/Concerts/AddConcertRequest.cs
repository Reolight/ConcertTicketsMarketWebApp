using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;
using ViewModels.PostingModels;

namespace CQRS.Concerts
{
    public class AddConcertRequest : IRequest<Concert?>
    {
        public ConcertPostingModel NewConcert { get; set; }
    }
}
