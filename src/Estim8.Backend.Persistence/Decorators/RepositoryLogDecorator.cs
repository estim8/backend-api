using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace Estim8.Backend.Persistence.Decorators
{
    
    public class RepositoryLogDecorator<TEntity> : Decorator<IRepository<TEntity>>, IRepository<TEntity> where TEntity : Entity
    {
        public RepositoryLogDecorator(IRepository<TEntity> decorated, ILogger<IRepository<TEntity>> logger) : base(decorated, logger)
        {
        }
        
        public async Task<bool> Delete(Guid id)
        {
            try
            {

                Log.LogInformation("Trying to delete entity of {EntityType} with {Id}", typeof(TEntity).Name, id);
                return await Decorated.Delete(id);
            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when deleting entity with {Id}", id);
                throw;
            }
        }

        public Task<IEnumerable<TEntity>> GetPaged(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResultPage<TEntity>> GetPaged(string pagingToken, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            try
            {
                Log.LogDebug("Trying to fetch entity of {EntityType} with {Id}", typeof(TEntity).Name, id);
                return await Decorated.GetById(id);

            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when fetching entity with {Id}", id);
                throw;
            }
        }

        public async Task<bool> Upsert(TEntity entity)
        {
            try
            {
                Log.LogDebug("Trying to upsert {Entity} with {Id}", entity, entity.Id);
                return await Decorated.Upsert(entity);

            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when upserting {Entity} with {Id}", entity, entity.Id);
                throw;
            }
        }
    }
}