using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    public class RedisRepository : IRepository
    {
        protected IContext Redis;
        protected IDatabase Database;
        protected ISerializer Serializer;

        public RedisRepository(IContext redisContext, ISerializer serializer)
        {
            Redis = redisContext;
            Serializer = serializer;
            Database = Redis.GetConnectionMultiplexer().GetDatabase();

        }
        public TimeSpan PingConnection()
        {
            return Database.Ping(CommandFlags.DemandMaster);
        } 
    }
    
    public abstract class RedisRepository<T> : RedisRepository, IRepository<T>
    {
        protected ILogger Logger;
        
        protected RedisRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer)
        {
            Logger = loggerFactory.CreateLogger<RedisRepository<T>>();
                        
            var redisLatency = PingConnection();
            Logger.LogInformation("Redis {EntityName} repository ready. Connection is {ConnectionStatus}. Latency is {RedisLatency}", typeof(T).Name, redisLatency != TimeSpan.Zero ? "CONNECTED" : "DISCONNECTED", redisLatency);
        }
        
        protected async Task<bool> ModifyObjectInTransaction(string key, Action<T> mutate)
        {
            var tran = Database.CreateTransaction();
            var original = await Redis.Cache.GetObjectAsync<T>(key);
            
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
                ? $"{typeof(T).Name.ToLower()}:id:{id.ToString()}"
                : $"{collectionType}:id:{id.ToString()}";
        }
        
        async Task<bool> IRepository<T>.Delete(Guid id)
        {
            return await Redis.Cache.RemoveAsync(ToKey(id));
        }

        async Task<T> IRepository<T>.GetById(Guid id)
        {
            return await Redis.Cache.GetObjectAsync<T>(ToKey(id));
        }

        async Task IRepository<T>.Upsert(T entity)
        {
            await Redis.Cache.SetObjectAsync(ToKey(ReadKey(entity)), entity);
        }

        private Guid ReadKey(T entity)
        {
            if(!(entity is Entity e))
                throw new ArgumentException("T is not of type Entity", nameof(entity));

            return e.Id;
        }
    }
}