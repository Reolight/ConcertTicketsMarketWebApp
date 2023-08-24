using Microsoft.Extensions.DependencyInjection;

namespace SorterByCriteria.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddReolightFilters(this IServiceCollection services,
        Action<ReolightFilterConfigurations> configure)
    {
        return services;
    }
}