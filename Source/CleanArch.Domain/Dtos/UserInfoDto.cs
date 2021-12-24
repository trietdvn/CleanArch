using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Dtos
{
    public class UserInfoDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces are not permitted.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Permissions { get; set; }

        public bool IsActive { get; set; }

        public string Nmls { get; set; }

        public string PersonalLink { get; set; }

        public double? Compensation { get; set; }
    }

    public class BaseUserInfoDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PersonalLink { get; set; }
    }
}