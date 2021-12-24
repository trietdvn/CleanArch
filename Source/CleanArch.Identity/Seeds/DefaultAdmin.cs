using IdentityModel;
using Microsoft.AspNetCore.Identity;
using CleanArch.Core.Constants;
using CleanArch.Identity.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArch.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            ////Seed Default User
            var email = "cleanarch@mailinator.com";
            var password = "Admin@123";
            var defaultAdmin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var existedAdmin = userManager.Users.FirstOrDefault(u => u.UserName == defaultAdmin.Email);
            if (existedAdmin == null)
            {
                var result = await userManager.CreateAsync(defaultAdmin, password);
                if (result != IdentityResult.Success)
                    throw new Exception($"Unable to create '{defaultAdmin.UserName}' account: {result}");

                await userManager.AddClaimsAsync(defaultAdmin, new Claim[]{
                                    new Claim(JwtClaimTypes.Name, email),
                                    //new Claim(JwtClaimTypes.GivenName, firstName),
                                    //new Claim(JwtClaimTypes.FamilyName, lastName),
                                    //new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                                    new Claim(JwtClaimTypes.Email, email),
                                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                });
                await userManager.AddToRoleAsync(defaultAdmin, DefaultConstant.UserRoles.Administrator);
            };
        }
    }
}