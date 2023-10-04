// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;
using ViewModels.PostingModels;

namespace CQRS.Tickets
{
    public class ConvertForPostingTicketsRequest : IRequest<bool>
    {
        public Guid ConcertId { get; set; }
        public List<TicketTemplate> TicketTemplates { get; set; } = null!;
    }
}
