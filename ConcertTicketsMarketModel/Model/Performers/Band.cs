namespace ConcertTicketsMarketModel.Model.Performers
{
    public class Band : Performer
    {
        // ReSharper disable once NullableWarningSuppressionIsUsed
        public string Genre { get; set; } = null!;
        public List<Performer> Performers { get; set; } = new();
    }
}
