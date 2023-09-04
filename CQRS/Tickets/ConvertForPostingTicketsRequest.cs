// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;
using ViewModels.PostingModels;

namespace CQRS.Tickets
{
    public class ConvertForPostingTicketsRequest : IRequest<IEnumerable<Ticket>>
    {
        public List<TicketTemplate> TicketTemplates { get; set; } = null!;
        //public Concert Concert { get; set; }
    }
}
