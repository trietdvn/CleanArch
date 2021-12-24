using CleanArch.Core.Interfaces;
using CleanArch.Domain.Entities;
using System.Linq;

namespace CleanArch.Domain.Services
{
    public interface ICustomerService : ICrudService<Customer>
    {
    }

    public class CustomerService : CrudService<Customer>, ICustomerService
    {
        public CustomerService(IRepository<Customer> repository) : base(repository)
        {
        }

        protected override IQueryable<Customer> BuildOrderByQueryable(IQueryable<Customer> query, IQueryStringParameters parameters)
        {
            return base.BuildOrderByQueryable(query, parameters);
        }

        protected override IQueryable<Customer> BuildSearchQueryable(IQueryable<Customer> query, IQueryStringParameters parameters)
        {
            return base.BuildSearchQueryable(query, parameters);
        }
    }
}