using ConcertTicketsMarketWebApp.Data;
using ConcertTicketsMarketWebApp.Services;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ConcertTicketsMarketModel.Concerts;
using ConcertTicketsMarketModel.Performers;
using Mapster;
using ViewModels;

namespace ConcertTicketsMarketWebApp
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityWithJwtAndRoles<TUser, TContext>(this IServiceCollection services)
                where TUser : IdentityUser
                where TContext : ApiAuthorizationDbContext<TUser>
        {
            services.AddDefaultIdentity<TUser>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.User.RequireUniqueEmail = true;

                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 5;
                    })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<TUser, TContext>()
                .AddProfileService<ProfileService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.Admin, policy =>
                    policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
            });

            return services;
        }

        public static IServiceCollection AddDbContextForUsersAndData<TIdentityContext, TContext, TUser>(this IServiceCollection services, ConfigurationManager configuration)
            where TUser : IdentityUser
            where TIdentityContext : ApiAuthorizationDbContext<TUser>
            where TContext : DbContext
        {
            var (identityConnectionString, dataContext) = (
                    configuration.GetConnectionString("IdentityContextConnection")
                        ?? throw new InvalidOperationException("There is no identity db connection string"),
                    configuration.GetConnectionString("DataContextConnection")
                        ?? throw new InvalidOperationException("There is no data context connection string")
            );

            // Add services to the container.            
            services.AddDbContext<IdentityContext>
                (options => options.UseSqlServer(identityConnectionString));
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(dataContext));

            return services;
        }

        public static void AddMapsterConfigured(this IServiceCollection services)
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
            services.AddMapster();
        }
    }
}