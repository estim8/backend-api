using System;
using System.Threading.Tasks;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : RedisRepository<Game>, IGameRepository
    {
        public GameRepository(IContext redisContext, ISerializer serializer, ILoggerFactory loggerFactory) : base(redisContext, serializer, loggerFactory)
        {
        }

        public async Task<bool> SetGameState(Guid gameId, GameState newState)
        {
            return await ModifyObjectInTransaction(ToKey(gameId), game => game.State = newState);
        }
    }
}