// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel.Concerts;

namespace ConcertTicketsMarketModel
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Describes detailed ticket info.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        public Guid ConcertId { get; set; }
        public virtual Concert Concert { get; set; } = null!;
        public Guid? OwnerId { get; set; }
    }
}
