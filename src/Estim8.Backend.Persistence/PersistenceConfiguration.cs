using StackExchange.Redis.Extensions.Core.Configuration;

namespace Estim8.Backend.Persistence
{
    public class PersistenceConfiguration
    {
        public RedisConfiguration RedisConfiguration { get; set; }
    }
}