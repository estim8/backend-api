using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Decorators;
using Estim8.Backend.Persistence.ProtoBuf;
using Estim8.Backend.Persistence.Repositories;
using Lamar;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProtoBuf.Meta;
using Serilog;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Protobuf;
using ILogger = Serilog.ILogger;
using ISerializer = StackExchange.Redis.Extensions.Core.ISerializer;
using JsonSerializer = CachingFramework.Redis.Serializers.JsonSerializer;

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
            
            ForSingletonOf<IContext>().Use(ctx =>
            {
                var connectionPool = ctx.GetInstance<IRedisCacheConnectionPoolManager>();
                return new RedisContext(connectionPool.GetConnection(), new ProtoBufSerializer());
            });
            
            //ForSingletonOf<ISerializer>().Use<ProtobufSerializer>();
            //ForSingletonOf<IRedisCacheClient>().Use<RedisCacheClient>();
            //ForSingletonOf<IRedisCacheConnectionPoolManager>().Use<RedisCacheConnectionPoolManager>();
            //ForSingletonOf<IRedisDefaultCacheClient>().Use<RedisDefaultCacheClient>();

            For<ILogger>().Use(Log.Logger);
            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                x.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            For<IRoundRepository>().Use<RoundRepository>();
            
            ProtoBufConfig.Configure();
            
            For(typeof(IRepository<>)).DecorateAllWith(typeof(RepositoryLogDecorator<>));
        }
    }
}