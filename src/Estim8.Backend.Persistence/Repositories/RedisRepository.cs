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
    public abstract class RedisRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected IContext Redis;
        protected ILogger Logger;
        protected readonly string HashSetKey;
        public RedisRepository(IContext redisContext, ILoggerFactory loggerFactory)
        {
            Redis = redisContext;
            Logger = loggerFactory.CreateLogger<RedisRepository<TEntity>>();

            HashSetKey = $"{typeof(TEntity).Name.ToLowerInvariant()}:hash";
            
            var redis = (RedisContext) Redis;
            Logger.LogInformation("Redis {EntityName} repository ready. Connection is {ConnectionStatus}.", typeof(TEntity).Name, redis.GetConnectionMultiplexer().IsConnected);
        }

        protected string ToKey(Guid id, string collectionType = null)
        {
            return collectionType == null
                ? $"{typeof(TEntity).Name.ToLower()}:id:{id.ToString()}"
                : $"{collectionType}:id:{id.ToString()}";
        }
        
        public virtual async Task<bool> Delete(Guid id)
        {
            return await Redis.Cache.RemoveHashedAsync(HashSetKey, ToKey(id));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await Redis.Cache.GetHashedAsync<TEntity>(HashSetKey, ToKey(id));
        }

        public virtual async Task Upsert(TEntity entity)
        {
            await Redis.Cache.SetHashedAsync(HashSetKey, ToKey(entity.Id), entity);
        }
    }
}