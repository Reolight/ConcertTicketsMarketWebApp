using ConcertTicketsMarketModel.Concerts;
using SorterByCriteria;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class GetConcertsRequest : IRequest<List<Concert>>
    {
        public List<SortingCriterion> Criteria { get; set; } = new();
    }
}
