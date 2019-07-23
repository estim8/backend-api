using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Estim8.Backend.Persistence.Repositories
{
    public abstract class RedisHashRepository<TEntity> : RedisRepository<TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public readonly string HashSetKey;

        protected RedisHashRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
            HashSetKey = $"{typeof(TEntity).Name.ToLowerInvariant()}:hash";
        }

        public string ToKey(Guid id, string collectionType = null)
        {
            return collectionType == null
                ? $"{typeof(TEntity).Name.ToLower()}:id:{id.ToString()}"
                : $"{collectionType}:id:{id.ToString()}";
        }
        
        public virtual async Task<bool> Delete(Guid id)
        {
            return await Redis.Cache.RemoveAsync(ToKey(id));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await Redis.Cache.GetObjectAsync<TEntity>(ToKey(id));
        }

        public virtual async Task Upsert(TEntity entity)
        {
            await Redis.Cache.SetObjectAsync(ToKey(entity.Id), entity);
        }
    }
}