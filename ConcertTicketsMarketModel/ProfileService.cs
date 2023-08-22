using System.Security.Claims;
using ConcertTicketsMarketModel.Data.Identity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace ConcertTicketsMarketModel
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
            List<Claim> claims = (await _userManager.GetRolesAsync(user))
                .Select(role => new Claim(JwtClaimTypes.Role, role))
                .ToList();
            
            // ReSharper disable once NullableWarningSuppressionIsUsed
            claims.Add(new Claim(JwtClaimTypes.Name, user.UserName!));
            
            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
