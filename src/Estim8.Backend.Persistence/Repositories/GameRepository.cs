using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Estim8.Backend.Persistence.Repositories
{
    public class GameRepository : RedisHashRepository<Game>
    {
        public GameRepository(IContext redisContext, ILoggerFactory loggerFactory) : base(redisContext, loggerFactory)
        {
        }
    }
}