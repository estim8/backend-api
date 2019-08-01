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
    public abstract class RedisRepository<TEntity> : IRepository<TEntity> 
        where TEntity : Entity
    {
        protected IContext Redis;
        protected ILogger Logger;
        protected IDatabase Database;
        protected ISerializer Serializer;

        protected RedisRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory)
        {
            Redis = redisContext;
            Serializer = serializer;
            Logger = loggerFactory.CreateLogger<RedisRepository<TEntity>>();
            Database = Redis.GetConnectionMultiplexer().GetDatabase();

            var redisLatency = Database.Ping(CommandFlags.DemandMaster);
            Logger.LogInformation("Redis {EntityName} repository ready. Connection is {ConnectionStatus}. Latency is {RedisLatency}", typeof(TEntity).Name, redisLatency != TimeSpan.Zero ? "CONNECTED" : "DISCONNECTED", redisLatency);
        }

        protected async Task<bool> ModifyObjectInTransaction(string key, Action<TEntity> mutate)
        {
            var tran = Database.CreateTransaction();
            var original = await Redis.Cache.GetObjectAsync<TEntity>(key);
            
            tran.AddCondition(Condition.StringEqual(key, Serializer.Serialize(original)));
            
            mutate(original);
            
#pragma warning disable 4014
            tran.StringSetAsync(key, Serializer.Serialize(original));
#pragma warning restore 4014

            return await tran.ExecuteAsync();        
        }
        
        public string ToKey(Guid id, string collectionType = null)
        {
            return collectionType == null
                ? $"{typeof(TEntity).Name.ToLower()}:id:{id.ToString()}"
                : $"{collectionType}:id:{id.ToString()}";
        }
        
        async Task<bool> IRepository<TEntity>.Delete(Guid id)
        {
            return await Redis.Cache.RemoveAsync(ToKey(id));
        }

        async Task<TEntity> IRepository<TEntity>.GetById(Guid id)
        {
            return await Redis.Cache.GetObjectAsync<TEntity>(ToKey(id));
        }

        async Task IRepository<TEntity>.Upsert(TEntity entity)
        {
            await Redis.Cache.SetObjectAsync(ToKey(entity.Id), entity);
        }
    }
}