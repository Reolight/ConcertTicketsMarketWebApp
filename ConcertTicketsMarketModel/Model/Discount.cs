// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Concerts;

namespace ConcertTicketsMarketModel.Model
{
    public class Discount
    {
        public Guid Id { get; set; }
        public string Promocode { get; set; } = null!;
        public double Value { get; set; }
        public bool IsAbsolute { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? EndTime { get; set; }


        public Guid? ConcertId { get; set; }
        public virtual Concert? Concert { get; set; }
    }
}
