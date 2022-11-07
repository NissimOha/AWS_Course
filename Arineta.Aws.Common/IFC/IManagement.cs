using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arineta.Aws.Common.IFC
{
    public interface IManagement<T> where T: class
    {
        Task AddAsync(T entity);
        Task<IList<T>> GetAsync();
        Task DeleteAsync(Guid id);
    }
}
