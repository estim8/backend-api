using Cosmonaut;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : RedisRepository<Game>
    {
        public GameRepository(IRedisCacheClient redisClient, ILogger logger) : base(redisClient, logger)
        {
        }
    }
}