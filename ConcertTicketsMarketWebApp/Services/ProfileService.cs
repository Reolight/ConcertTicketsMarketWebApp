using ConcertTicketsMarketWebApp.Areas.Identity.Data;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ConcertTicketsMarketWebApp.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (await _userManager.GetUserAsync(context.Subject) is not { } user)
                return;
            var roleClaims = (await _userManager.GetRolesAsync(user))
                .Select(role => new Claim(ClaimTypes.Role, role));
            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
