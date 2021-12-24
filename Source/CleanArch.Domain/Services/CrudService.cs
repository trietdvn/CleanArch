using CleanArch.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Domain.Services
{
    public abstract class CrudService<TEntity> : ICrudService<TEntity>
    {
        private readonly IRepository<TEntity> _repository;

        public CrudService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            return _repository.CreateAsync(entity);
        }

        public Task<bool> DeleteByIdAsync(object id)
        {
            return _repository.DeleteByIdAsync(id);
        }

        public Task<(List<TEntity> dataList, int total)> GetAsync(IQueryStringParameters parameters)
        {
            var query = _repository.GetQueryable();

            // search
            query = BuildSearchQueryable(query, parameters);

            // count total
            var totalItems = query.Count();

            // order by
            query = BuildOrderByQueryable(query, parameters);

            // paging
            var pagedListQuery = parameters.PageIndex == null || parameters.PageIndex < 0
                ? query 
                : query.Skip(parameters.PageIndex.Value  * parameters.PageSize.Value).Take(parameters.PageSize.Value);

            // TODO: ToListAsync
            var pagedList = pagedListQuery.ToList();
            return Task.FromResult<(List<TEntity> dataList, int total)>((pagedList, totalItems));
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return _repository.UpdateAsync(entity);
        }

        protected virtual IQueryable<TEntity> BuildSearchQueryable(IQueryable<TEntity> query, IQueryStringParameters parameters)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> BuildOrderByQueryable(IQueryable<TEntity> query, IQueryStringParameters parameters)
        {
            return query;
        }
    }
}