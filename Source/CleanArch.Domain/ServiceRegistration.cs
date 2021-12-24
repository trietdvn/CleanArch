using CleanArch.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Domain
{
    public static class ServiceRegistration
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<ICustomerService, CustomerService>();

            //        services.Scan(scan =>
            //scan.FromAssemblyOf<IDomainService>()
            //    .AddClasses(classes => classes.AssignableTo<IDomainService>())
            //    .AsMatchingInterface()
            //    .WithScopedLifetime());
        }
    }
}