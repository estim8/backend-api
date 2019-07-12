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
                Log.LogInformation("Trying to delete entity of {EntityType} with id '{Id}'", typeof(TEntity).Name, id);
                return await Decorated.Delete(id);
            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when deleting entity with id '{Id}'", id);
                throw;
            }
        }

        public async Task<TEntity> GetById(Guid id)
        {
            try
            {
                Log.LogDebug("Trying to fetch entity of {EntityType} with id '{Id}'", typeof(TEntity).Name, id);
                return await Decorated.GetById(id);

            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when fetching entity with id '{Id}'", id);
                throw;
            }
        }

        public async Task Upsert(TEntity entity)
        {
            try
            {
                Log.LogDebug("Trying to upsert {Entity} with id '{Id}'", entity, entity.Id);
                await Decorated.Upsert(entity);

            }
            catch (Exception e)
            {
                Log.LogError(e, "Exception when upserting {Entity} with id '{Id}'", entity, entity.Id);
                throw;
            }
        }
    }
}