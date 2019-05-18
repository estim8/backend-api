using StackExchange.Redis.Extensions.Core.Configuration;

namespace Estim8.Backend.Persistence
{
    public class PersistenceConfiguration
    {
        public string DefaultDatabase { get; set; }
        public string CosmosUri { get; set; }
        public string AuthKey { get; set; }

        public RedisConfiguration RedisConfiguration { get; set; }
    }
}