// ReSharper disable NullableWarningSuppressionIsUsed
using ConcertTicketsMarketModel.Concerts;

namespace ConcertTicketsMarketModel
{
    /// <summary>
    /// <para>States are declared via nullability of BookingDate and Owner fields in the next ways: </para>
    /// <para>
    /// Available: <c>booked AND owner == null</c>
    /// Booked: <c>booked AND owner != null</c>
    /// Bought: <c>booked == null AND owner != null</c>
    /// </para>
    /// </summary>
    public class Ticket
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Describes detailed ticket info.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Not null, if booked and not bought yet
        /// </summary>
        public DateTime? BookingTime { get; set; }
        public Guid ConcertId { get; set; }
        public virtual Concert Concert { get; set; } = null!;
        
        /// <summary>
        /// Not null, if booked and bought.
        /// </summary>
        public Guid? OwnerId { get; set; }
    }
}
