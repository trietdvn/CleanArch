using Microsoft.EntityFrameworkCore;
using CleanArch.Core.Interfaces;
using CleanArch.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.Data.Repositories
{
    public class EfRepository<TEntity, TKey> : IRepository<TEntity>
        where TEntity : BaseEntity<TKey>
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _entities;

        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync(CancellationToken.None);
            if (result > 0)
                return entity;

            return default;
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                var result = await _dbContext.SaveChangesAsync(CancellationToken.None);
                return (result > 0);
            }

            return false;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            var entity = await _entities.AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(id));
            return entity;
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return _entities.AsNoTracking().AsQueryable();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updateEntity = GetByIdAsync(entity.Id);
            if (updateEntity != null)
            {
                _entities.Update(entity);
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                return entity;
            }

            return default;
        }
    }
}