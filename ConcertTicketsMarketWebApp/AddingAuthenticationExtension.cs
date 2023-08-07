using ConcertTicketsMarketWebApp.Areas.Identity.Data;
using ConcertTicketsMarketWebApp.Data;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;

namespace ConcertTicketsMarketWebApp
{
    public static class AddingAuthenticationExtension
    {
        public static IServiceCollection AddMyIdentity<User, Context>(this IServiceCollection services)
            where User : IdentityUser
            where Context : ApiAuthorizationDbContext<User>
        {
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Context>();

            services.AddIdentityServer()
                .AddApiAuthorization<User, Context>();

            services.AddAuthentication()
                .AddOpenIdConnect();

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim("admin"));
            });

            return services;
        }
    }
}
