using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arineta.Aws.Common.IFC;
using Microsoft.EntityFrameworkCore;

namespace Arineta.Aws.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApiContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<IList<TEntity>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
