using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Persistence.Decorators
{
    public class RepositoryLogDecorator<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IRepository<TEntity> _decorated;
        private readonly ILogger<IRepository<TEntity>> _logger;

        public RepositoryLogDecorator(IRepository<TEntity> decorated, ILogger<IRepository<TEntity>> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }
        
        
        public async Task<bool> Delete(Guid id)
        {
            _logger.LogInformation("Called delete");
            return await _decorated.Delete(id);
        }

        public Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResultPage<TEntity>> GetPaged(string pagingToken, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Upsert(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}