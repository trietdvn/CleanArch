using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArch.Identity.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; set; }
    }

    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>,
        IAuthorizationRequirement

    {
        private readonly IdentityContext _context;

        public PermissionRequirementHandler(IdentityContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            var idClaim = context.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
                return;

            var userId = idClaim.Value;
            var userGuid = Guid.Parse(userId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userGuid);

            if (user == null)
                return;

            var roleClaims = from ur in _context.UserRoles
                             where ur.UserId == user.Id
                             join r in _context.Roles on ur.RoleId equals r.Id
                             join rc in _context.RoleClaims on r.Id equals rc.RoleId
                             select rc;

            if (await roleClaims.AnyAsync(c => c.ClaimValue == requirement.Permission))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}