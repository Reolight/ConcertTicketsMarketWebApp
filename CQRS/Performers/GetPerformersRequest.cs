using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

#pragma warning disable CS8618

namespace CQRS.Performers
{
    public class GetPerformersRequest : IRequest<(IEnumerable<Performer>, int)>
    {
        public string JsonQuery { get; set; }
    }
}
