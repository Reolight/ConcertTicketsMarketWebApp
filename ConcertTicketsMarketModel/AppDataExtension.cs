using System.Security.Claims;
using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertTicketsMarketModel;

public static class AppDataExtension
{
    public static IServiceCollection AddDbContextForUsersAndData(this IServiceCollection services, ConfigurationManager configuration)
    {
        var (identityConnectionString, dataContext) = (
            configuration.GetConnectionString("IdentityContextConnection") as string
            ?? throw new InvalidOperationException("There is no identity db connection string"),
            configuration.GetConnectionString("DataContextConnection") as string
            ?? throw new InvalidOperationException("There is no data context connection string")
        );

        // Add services to the container.            
        services.AddDbContext<IdentityContext>
            (options => options.UseSqlServer(identityConnectionString));
        services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(dataContext));

        return services;
    }
    
    public static IServiceCollection AddIdentityWithJwtAndRoles(this IServiceCollection services)
    {
        services.AddDefaultIdentity<AppUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 5;
                })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<AppUser, IdentityContext>()
            .AddProfileService<ProfileService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Audience = "localhost:44476";
                options.Authority = "https://localhost:7175";
                options.TokenValidationParameters.ValidateIssuer = false;
                options.TokenValidationParameters.ValidateAudience = false;
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.Admin, policy =>
                policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
        });

        return services;
    }
}