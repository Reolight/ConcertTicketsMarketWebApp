using ConcertTicketsMarketModel.Model.Performers;

namespace ViewModels;

public static class PerformerTypeConverter
{
    public static PerformerType GetPerformerType(Performer performer) =>
        performer switch
        {
            Band band => PerformerType.Band,
            Singer singer => PerformerType.Singer,
            { } perf => PerformerType.Performer,
            _ => throw new InvalidCastException()
        };
}