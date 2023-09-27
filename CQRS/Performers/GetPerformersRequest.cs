using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using ViewModels;

#pragma warning disable CS8618

namespace CQRS.Performers
{
    public class GetPerformersRequest : IRequest<(IEnumerable<PerformerViewModel>, int)>
    {
        public string? JsonQuery { get; set; }
    }
}
