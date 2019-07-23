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
    public abstract class RedisRepository<TEntity> where TEntity : Entity
    {
        protected IContext Redis;
        protected ILogger Logger;
        protected IDatabase Database => Redis.GetConnectionMultiplexer().GetDatabase();
        protected ISerializer Serializer;

        protected RedisRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory)
        {
            Redis = redisContext;
            Serializer = serializer;
            Logger = loggerFactory.CreateLogger<RedisRepository<TEntity>>();

            var redis = redisContext.GetConnectionMultiplexer();
            Logger.LogInformation("Redis {EntityName} repository ready. Connection is {ConnectionStatus}.", typeof(TEntity).Name, redis.IsConnected ? "CONNECTED" : "DISCONNECTED");
        }

        protected async Task<bool> ModifyObjectInTransaction(string key, Action<TEntity> mutate)
        {
            var tran = Database.CreateTransaction();
            var original = await Redis.Cache.GetObjectAsync<TEntity>(key);
            
            tran.AddCondition(Condition.StringEqual(key, Serializer.Serialize(original)));
            
            mutate(original);
            tran.StringSetAsync(key, Serializer.Serialize(original));

            return await tran.ExecuteAsync();        
        }
    }
}