using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        protected IRedisDatabase Redis;
        protected ILogger Logger;
        public RedisRepository(IRedisCacheClient redisCacheClient, ILoggerFactory loggerFactory)
        {
            Redis = redisCacheClient.GetDbFromConfiguration();
            Logger = loggerFactory.CreateLogger<RedisRepository<TEntity>>();

            Logger.LogInformation("Redis {EntityName} repository ready. Using database {database}.", typeof(TEntity).Name, Redis.Database.Database);

        }
        
        public async Task<bool> Delete(Guid id)
        {
            return await Redis.RemoveAsync(id.ToString(), CommandFlags.DemandMaster);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await Redis.GetAsync<TEntity>(id.ToString());
        }

        public async Task<bool> Upsert(TEntity entity)
        {
            return await Redis.AddAsync(entity.Id.ToString(), entity,flag: CommandFlags.DemandMaster);
        }
    }
}