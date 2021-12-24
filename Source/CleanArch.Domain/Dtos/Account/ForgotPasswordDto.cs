using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Dtos.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
