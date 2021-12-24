using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using CleanArch.Core.Constants;
using CleanArch.Identity.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArch.Identity.IdentityServer
{
    public class IdentityProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;

        public IdentityProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // This override UserClaims role in Is4Config.cs
            var user = await _userManager.GetUserAsync(context.Subject);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            roles.ToList().ForEach(r => roleClaims.Add(new Claim(JwtClaimTypes.Role, r)));

            var isActiveClaim = user.Claims.FirstOrDefault(x => x.ClaimType == DefaultConstant.ClaimTypes.IsActive);
            var isActive = isActiveClaim != null && bool.TryParse(isActiveClaim.ClaimValue, out var isActiveVal) && isActiveVal;

            // use context.RequestedClaimTypes to see which claims are requested.
            context.IssuedClaims.AddRange(roleClaims);
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean));
            context.IssuedClaims.Add(new Claim(DefaultConstant.ClaimTypes.IsActive, isActive.ToString(), ClaimValueTypes.Boolean));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            if (user == null || user.Claims == null)
            {
                context.IsActive = false;
            }
            else
            {
                var isActiveClaim = user.Claims.FirstOrDefault(x => x.ClaimType == DefaultConstant.ClaimTypes.IsActive);
                var isActive = isActiveClaim != null && bool.TryParse(isActiveClaim.ClaimValue, out var isActiveVal) && isActiveVal;

                context.IsActive = user.EmailConfirmed && isActive;
            }
        }
    }
}