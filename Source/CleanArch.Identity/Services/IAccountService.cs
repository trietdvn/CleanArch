using CleanArch.Domain.Dtos.Account;
using System.Threading.Tasks;

namespace CleanArch.Identity.Services
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto input, string confirmUrl);

        Task<bool> ConfirmEmailAsync(ConfirmEmailDto input);

        Task<bool> ForgotPasswordAsync(ForgotPasswordDto input, string confirmUrl);

        Task<bool> ResetPasswordAsync(ResetPasswordDto input);

        Task<bool> ChangePasswordAsync(ChangePasswordDto input);

        Task<bool> IsRegisteredAsync(string username);
    }
}