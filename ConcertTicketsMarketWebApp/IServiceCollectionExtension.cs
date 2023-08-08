using ConcertTicketsMarketWebApp.Data;
using ConcertTicketsMarketWebApp.Services;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ConcertTicketsMarketWebApp
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityWithJwtAndRoles<User, Context>(this IServiceCollection services)
                where User : IdentityUser
                where Context : ApiAuthorizationDbContext<User>
        {
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Context>();

            services.AddIdentityServer()
                .AddApiAuthorization<User, Context>()
                .AddProfileService<ProfileService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
            });

            return services;
        }

        public static IServiceCollection AddDbContextForUsersAndData<IdentityDb, AppDb, AppUser>(this IServiceCollection services, ConfigurationManager configuration)
            where AppUser : IdentityUser
            where IdentityDb : ApiAuthorizationDbContext<AppUser>
            where AppDb : DbContext
        {
            var (identityConnectionString, dataContext) = (
                    configuration.GetConnectionString("IdentityContextConnection")
                        ?? throw new InvalidOperationException("There is no identity db connection string"),
                    configuration.GetConnectionString("DataContextConnection")
                        ?? throw new InvalidOperationException("There is no data context connection string")
            );

            // Add services to the container.            
            services.AddDbContext<IdentityContext>
                (options => options.UseInMemoryDatabase("IdentityDb"/*identityConnectionString*/));
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(dataContext));

            return services;
        }
    }
}