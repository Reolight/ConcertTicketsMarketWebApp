﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SorterByCriteria.TypeResolver;

namespace SorterByCriteria.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddFiltersSortersPaginator<TContext>(this IServiceCollection services,
        Action<FilterSorterPaginatorConfigurations>? configure)
        where TContext : DbContext
    {
        services.Configure(configure ?? new Action<FilterSorterPaginatorConfigurations>(
            configs => configs.ReflectOver = InspectionType.Properties));
        
        services.AddSingleton<TypeResolver<TContext>>();
        services.AddTransient<FilterSorterPaginatorService<TContext>>();
        
        return services;
    }
}