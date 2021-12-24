using IdentityModel;
using Microsoft.AspNetCore.Identity;
using CleanArch.Application;
using CleanArch.Core.Constants;
using CleanArch.Domain.Dtos;
using CleanArch.Domain.Dtos.Account;
using CleanArch.Domain.Extensions;
using CleanArch.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CleanArch.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IEmailService _emailSenderService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEmailService emailSenderService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSenderService = emailSenderService;
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto input)
        {
            if (input.UserId == null || input.Token == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user == null)
            {
                return false;
            }

            var token = input.Token;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return false;
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimAsync(user, userClaims.FirstOrDefault(p => p.Type == JwtClaimTypes.EmailVerified));
            await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean));
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto input, string confirmUrl)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || (!string.IsNullOrEmpty(confirmUrl) && !await _userManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }

            #region Forgot Password Email

            try
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string callbackUrl = $"{confirmUrl}?userId={user.Id}&token={HttpUtility.UrlEncode(token)}"; //token must be a query string parameter as it is very long

                var email = new EmailMessageDto();
                email.ToAddresses.Add(new EmailAddressDto(user.Email, user.Email));
                email.BuildForgotPasswordEmail(user.UserName, callbackUrl, user.Id.ToString(), token); //Replace First UserName with Name if you want to add name to Registration Form

                await _emailSenderService.SendAsync(email);
                return true;
            }
            catch (Exception)
            {
            }

            #endregion Forgot Password Email

            return false;
        }

        // returns registered username
        public async Task<string> RegisterAsync(RegisterDto input, string confirmUrl)
        {
            var user = new ApplicationUser
            {
                UserName = input.UserName,
                Email = input.Email
            };

            var createUserResult = input.Password == null ?
                await _userManager.CreateAsync(user) :
                await _userManager.CreateAsync(user, input.Password);

            //if (!createUserResult.Succeeded)
            //    throw new DomainException(string.Join(",", createUserResult.Errors.Select(i => i.Description)));

            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            userClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            userClaims.Add(new Claim(JwtClaimTypes.EmailVerified, string.IsNullOrEmpty(confirmUrl) ? "false" : "true", ClaimValueTypes.Boolean));
            userClaims.Add(new Claim(DefaultConstant.ClaimTypes.IsActive, "true", ClaimValueTypes.Boolean));

            // add additional user info
            if (!string.IsNullOrEmpty(input.FirstName))
                userClaims.Add(new Claim(JwtClaimTypes.GivenName, input.FirstName));
            if (!string.IsNullOrEmpty(input.LastName))
                userClaims.Add(new Claim(JwtClaimTypes.FamilyName, input.LastName));
            if (!string.IsNullOrEmpty(input.PhoneNumber))
                userClaims.Add(new Claim(JwtClaimTypes.PhoneNumber, input.PhoneNumber));

            await _userManager.AddClaimsAsync(user, userClaims);

            await _userManager.AddToRoleAsync(user, DefaultConstant.UserRoles.NormalUser);

            var emailMessage = new EmailMessageDto();

            if (!string.IsNullOrEmpty(confirmUrl))
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"{confirmUrl}?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";

                emailMessage.BuildNewUserConfirmationEmail(user.UserName, user.Email, callbackUrl, user.Id.ToString(), token); //Replace First UserName with Name if you want to add name to Registration Form
            }
            else
            {
                emailMessage.BuildNewUserEmail(user.UserName, user.UserName, user.Email, input.Password);
            }

            emailMessage.ToAddresses.Add(new EmailAddressDto(user.Email, user.Email));
            try
            {
                await _emailSenderService.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
            }

            // retrieve new user has been registered
            var newUser = await _userManager.FindByNameAsync(input.UserName);
            return newUser != null ? newUser.UserName : string.Empty;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user == null)
            {
                return false;
            }

            try
            {
                var result = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);
                if (result.Succeeded)
                {
                    #region Email Successful Password change

                    var email = new EmailMessageDto();
                    email.ToAddresses.Add(new EmailAddressDto(user.Email, user.Email));
                    email.BuildPasswordResetEmail(user.UserName);

                    await _emailSenderService.SendAsync(email);

                    return true;

                    #endregion Email Successful Password change
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<bool> IsRegisteredAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }
    }
}