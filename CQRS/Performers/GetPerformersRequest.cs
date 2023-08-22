using ConcertTicketsMarketModel.Performers;
using MediatR;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class GetPerformersRequest : IRequest<List<Performer>>
    {
        public List<SortingCriterion> SortingCriteria { get; set; } = new();
    }
}
