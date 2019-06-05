using Estim8.Backend.Persistence.Repositories;
using Lamar;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Protobuf;
using ILogger = Serilog.ILogger;

namespace Estim8.Backend.Persistence.Registry
{
    public class PersistenceRegistry : ServiceRegistry
    {
        public PersistenceRegistry()
        {
            ForSingletonOf<RedisConfiguration>().Use(ctx =>
            {
                var opt = ctx.GetInstance<IOptions<PersistenceConfiguration>>().Value;
                return opt.RedisConfiguration;
            });

            ForSingletonOf<ISerializer>().Use<ProtobufSerializer>();
            ForSingletonOf<IRedisCacheClient>().Use<RedisCacheClient>();
            ForSingletonOf<IRedisCacheConnectionPoolManager>().Use<RedisCacheConnectionPoolManager>();
            ForSingletonOf<IRedisDefaultCacheClient>().Use<RedisDefaultCacheClient>();

            For<ILogger>().Use(Log.Logger);
            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                x.RegisterConcreteTypesAgainstTheFirstInterface();
            });
            
            ProtoBuf.ProtoBufConfig.Configure();
        }
    }
}