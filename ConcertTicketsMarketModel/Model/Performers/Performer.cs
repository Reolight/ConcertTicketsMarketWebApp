// ReSharper disable NullableWarningSuppressionIsUsed

using ConcertTicketsMarketModel.Model.Concerts;

namespace ConcertTicketsMarketModel.Model.Performers
{
    public class Performer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public List<Concert> Concerts { get; set; }
    }
}
