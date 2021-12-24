using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArch.Core.Interfaces
{
    public interface ICrudService<TEntity>
    {
        Task<(List<TEntity> dataList, int total)> GetAsync(IQueryStringParameters parameters);

        Task<TEntity> GetByIdAsync(object id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteByIdAsync(object id);
    }
}