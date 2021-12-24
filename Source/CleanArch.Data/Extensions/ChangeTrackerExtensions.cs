using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CleanArch.Domain.Entities;
using System;
using System.Linq;
using System.Security.Claims;

namespace CleanArch.Data.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(this ChangeTracker changeTracker, IHttpContextAccessor httpContextAccessor)
        {
            changeTracker.DetectChanges();
            var dbContext = (ApplicationDbContext)changeTracker.Context;
            string userId = null;
            var timestamp = DateTime.UtcNow;

            if (httpContextAccessor.HttpContext != null)
            {
                var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
                userId = userIdClaim?.Value;
            }

            foreach (var entry in changeTracker.Entries())
            {
                //Auditable Entity Model
                if (entry.Entity is AuditEntity<Guid> || entry.Entity is AuditEntity<int>)
                {
                    entry.Property("ModifiedAt").CurrentValue = timestamp;
                    entry.Property("ModifiedBy").CurrentValue = userId;

                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedAt").CurrentValue = timestamp;
                        entry.Property("CreatedBy").CurrentValue = userId;
                    }
                }
            }
        }
    }
}