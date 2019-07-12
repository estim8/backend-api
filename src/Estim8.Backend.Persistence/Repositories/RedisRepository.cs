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

        protected RedisRepository(IContext redisContext, ILoggerFactory loggerFactory)
        {
            Redis = redisContext;
            Logger = loggerFactory.CreateLogger<RedisRepository<TEntity>>();
            
            var redis = Redis as RedisContext;
            Logger.LogInformation("Redis {EntityName} repository ready. Connection is {ConnectionStatus}.", typeof(TEntity).Name, redis?.GetConnectionMultiplexer().IsConnected);
        } 
    }
}