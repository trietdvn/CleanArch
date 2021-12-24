using CleanArch.Domain.Dtos;
using System.Threading.Tasks;

namespace CleanArch.Application
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessageDto emailMessage);
    }
}