using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArch.Core.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(object id);

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteByIdAsync(object id);

        IQueryable<TEntity> GetQueryable();
    }
}