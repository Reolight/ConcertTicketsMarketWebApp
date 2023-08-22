using ConcertTicketsMarketModel.Model.Concerts;
using ConcertTicketsMarketModel.Model.Performers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;

namespace ConcertTicketsMarketWebApp
{
    public static class MapsterConfigurationExtenstion
    {
        public static void AddMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Concert, ConcertSuperficial>
                .NewConfig()
                .Fork(config => config.Default.PreserveReference(true))
                .PreserveReference(true);
            TypeAdapterConfig<Performer, PerformerSuperficial>
                .NewConfig()
                .Map(dest => dest.PerformerType,
                    src => PerformerTypeConverter.GetPerformerType(src))
                .PreserveReference(true);
        }
    }
}