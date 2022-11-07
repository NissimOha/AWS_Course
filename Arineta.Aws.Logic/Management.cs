using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arineta.Aws.Common.IFC;

namespace Arineta.Aws.Logic
{
    public class Management<T> : IManagement<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Management(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.CommitAsync();
        }

        public async Task<IList<T>> GetAsync()
        {
            return await _repository.GetAsync();
        }
        
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.CommitAsync();
        }
    }
}
