using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Dtos.Account
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}