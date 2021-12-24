using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArch.Core.Interfaces;
using CleanArch.Data.Repositories;
using CleanArch.Domain.Entities;
using System;

namespace CleanArch.Data
{
    public static class ServiceRegistration
    {
        public static void AddDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
                 configuration.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #region Repositories

            services.AddTransient(typeof(IRepository<Customer>), typeof(EfRepository<Customer, Guid>));

            #endregion Repositories
        }
    }
}