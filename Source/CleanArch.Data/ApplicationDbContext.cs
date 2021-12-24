using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CleanArch.Data.Extensions;
using CleanArch.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entities would be configured here
            modelBuilder.Entity<Customer>()
                .Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.SetShadowProperties(_httpContextAccessor);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.SetShadowProperties(_httpContextAccessor);
            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }
}