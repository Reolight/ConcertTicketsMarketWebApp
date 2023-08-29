using SorterByCriteria;
using MediatR;
using ViewModels;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class GetConcertsRequest : IRequest<(IEnumerable<ConcertSuperficial>, int)>
    {
        public string query { get; set; } = string.Empty;
    }
}
