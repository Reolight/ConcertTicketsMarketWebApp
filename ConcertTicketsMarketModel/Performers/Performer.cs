// ReSharper disable NullableWarningSuppressionIsUsed

namespace ConcertTicketsMarketModel.Performers
{
    public class Performer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Origin { get; set; } = null!;
    }
}
