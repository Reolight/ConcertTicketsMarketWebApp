using ConcertTicketsMarketModel.Concerts;
using ConcertTicketsMarketModel.Performers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;

namespace ConcertTicketsMarketWebApp
{
    public static class ServiceCollectionExtension
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