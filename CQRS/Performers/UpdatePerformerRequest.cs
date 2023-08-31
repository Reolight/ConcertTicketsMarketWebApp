// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Performers;
using MediatR;

namespace CQRS.Performers
{
    public class UpdatePerformerRequest : IRequest<bool>
    {
        public Performer Performer { get; set; } = null!;
    }
}
