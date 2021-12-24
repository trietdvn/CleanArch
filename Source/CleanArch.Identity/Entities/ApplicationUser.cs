using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CleanArch.Identity.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    }
}