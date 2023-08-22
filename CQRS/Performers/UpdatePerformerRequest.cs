// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class UpdatePerformerRequest : IRequest<bool>
    {
        public Performer Performer { get; set; } = null!;
    }
}
