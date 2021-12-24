using Microsoft.AspNetCore.Identity;
using CleanArch.Core.Constants;
using CleanArch.Identity.Authorization;
using CleanArch.Identity.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            // Ensure administrator role and full permissions
            var adminRoleName = DefaultConstant.UserRoles.Administrator;
            var adminRole = await roleManager.FindByNameAsync(adminRoleName);
            if (adminRole == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(adminRoleName));
                if (result.Succeeded)
                    adminRole = await roleManager.FindByNameAsync(adminRoleName);
            }

            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);

            var currentPermissions = adminRoleClaims.Where(x => x.Type == DefaultConstant.ClaimTypes.Permission).Select(x => x.Value).ToList();
            var allPermissionNames = ApplicationPermissions.GetAllPermissionNames().ToList();

            // Ensure normal user role
            var normalUserRoleName = DefaultConstant.UserRoles.NormalUser;
            var normalUserRole = await roleManager.FindByNameAsync(normalUserRoleName);
            if (normalUserRole == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(normalUserRoleName));
                if (result.Succeeded)
                    normalUserRole = await roleManager.FindByNameAsync(normalUserRoleName);
            }
        }
    }
}