using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estim8.Backend.Persistence.Model;
using Serilog;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Estim8.Backend.Persistence.Repositories
{
    public abstract class RedisRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected IRedisDatabase Redis;
        protected ILogger Logger;
        public RedisRepository(IRedisCacheClient redisCacheClient, ILogger logger)
        {
            Redis = redisCacheClient.GetDbFromConfiguration();
            Logger = logger;
        }
        
        public async Task<bool> Delete(Guid id)
        {
            return await Redis.RemoveAsync(id.ToString());
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
            return await Redis.GetAsync<TEntity>(id.ToString());
        }

        public async Task<bool> Upsert(TEntity entity)
        {
            return await Redis.AddAsync(entity.Id.ToString(), entity);
        }
    }
}