using Duende.IdentityServer.Validation;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertTicketsMarketWebApp
{
    public static class Roles
    {
        public static readonly string Admin = "admin";

        public static void UseDefinedRoles(this WebApplication app)
        {
            // I use here reflection to read available roles. It makes roles addition easier: just writing it as public static field above is enough
            var roles = typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(fieldInfo => fieldInfo.GetValue(null) as string)
                .Where(role => role is not null)
                .OfType<string>().ToList() ?? throw new InvalidOperationException("There is no roles to read from Roles class");
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            roles.ForEach(role =>
            {
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            });
        }
    }
}
