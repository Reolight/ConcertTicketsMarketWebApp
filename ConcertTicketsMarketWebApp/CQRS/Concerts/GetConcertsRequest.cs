using ConcertTicketsMarketModel.Concerts;
using SorterByCriteria;
using MediatR;
using ViewModels;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class GetConcertsRequest : IRequest<List<ConcertSuperficial>>
    {
        public List<SortingCriterion> Sorting { get; set; } = new();
    }
}
