using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arineta.Aws.Common.IFC
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<IList<TEntity>> GetAsync();
        Task DeleteAsync(Guid id);

        //Without UoW - commit in the repository
        Task CommitAsync();
    }
}
