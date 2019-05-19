using Cosmonaut;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : RedisRepository<Game>
    {
        public GameRepository(IRedisCacheClient redisClient, ILoggerFactory loggerFactory) : base(redisClient, loggerFactory)
        {
        }
    }
}