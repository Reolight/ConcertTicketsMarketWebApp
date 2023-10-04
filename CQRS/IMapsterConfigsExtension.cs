using ConcertTicketsMarketModel.Model.Concerts;
using ConcertTicketsMarketModel.Model.Performers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using ViewModels.PostingModels;

namespace CQRS
{
    public static class MapsterConfigurationExtenstion
    {
        public static void AddMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Concert, ConcertSuperficial>
                .NewConfig()
                .Map(dest => dest.Duration, source => source.Duration.TotalMinutes)
                .PreserveReference(true);
            TypeAdapterConfig<Performer, PerformerSuperficial>
                .NewConfig()
                .Map(dest => dest.PerformerType,
                    src => PerformerTypeConverter.GetPerformerType(src))
                .PreserveReference(true);
            TypeAdapterConfig<Performer, PerformerViewModel>
                .NewConfig()
                .Map(dest => dest.Concerts,
                    source => source.Concerts.Select(concert => concert.Adapt<ConcertSuperficial>()))
                .Map(dest => dest.PerformerType,
                    src => PerformerTypeConverter.GetPerformerType(src))
                .Map(dest => dest.Performers,
                    source =>
                        source.GetType() == typeof(Band)
                            ? ((Band)source).Performers.Select(performer => performer.Adapt<PerformerSuperficial>())
                            : null
                );

            
            TypeAdapterConfig<ConcertPostingModel, Concert>
                .NewConfig()
                .Map(dest => dest.Duration,
                    src => TimeSpan.FromMinutes(src.Duration))
                .Ignore(dest => dest.Tickets);
        }
    }
}