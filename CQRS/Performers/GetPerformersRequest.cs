using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using SorterByCriteria;
#pragma warning disable CS8618

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class GetPerformersRequest : IRequest<(IEnumerable<Performer>, int)>
    {
        public string JsonQuery { get; set; }
    }
}
