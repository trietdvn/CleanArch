using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CleanArch.Domain.Dtos.Account;
using CleanArch.Domain.Settings;
using CleanArch.Identity.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EndpointSettings _endpointSettings;
        private readonly IAccountService _accountService;

        public AccountController(IOptions<EndpointSettings> endpointSettingsOptions, IAccountService accountService)
        {
            _accountService = accountService;
            _endpointSettings = endpointSettingsOptions.Value;
        }

        // POST: api/Account/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto parameters)
        {
            try
            {
                if (await _accountService.IsRegisteredAsync(parameters.UserName))
                    return BadRequest("Username has already been taken");

                var newUser = await _accountService.RegisterAsync(parameters, $"{ _endpointSettings.ApiEndpoint}/Account/ConfirmEmail");

                if (newUser != null)
                {
                    return Ok("Register User Success");
                }
                else
                {
                    return BadRequest("Register User Failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Register User Failed. Error: {ex.Message}");
            }
        }

        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto parameters)
        {
            var succeeded = await _accountService.ConfirmEmailAsync(parameters);

            return succeeded ? Ok("Success") : BadRequest("User Email Confirmation Failed");
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto parameters)
        {
            var succeeded = await _accountService.ForgotPasswordAsync(parameters, string.Empty);

            return succeeded ? Ok("Success") : BadRequest("Forgot Password Failed");
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto parameters)
        {
            var succeeded = await _accountService.ResetPasswordAsync(parameters);

            return succeeded ? Ok("Success") : BadRequest("Reset Password Failed");
        }

        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto parameters)
        {
            parameters.UserId = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            var succeeded = await _accountService.ChangePasswordAsync(parameters);

            return succeeded ? Ok("Success") : BadRequest("Change Password Failed");
        }
    }
}