using CachingFramework.Redis;
using CachingFramework.Redis.Contracts;
using Estim8.Backend.Persistence.Decorators;
using Estim8.Backend.Persistence.ProtoBuf;
using Estim8.Backend.Persistence.Repositories;
using Lamar;
using Microsoft.Extensions.Configuration;
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
            ForSingletonOf<IContext>().Use(ctx =>
            {
                var globalConfig = ctx.GetInstance<IConfiguration>();
                var redisConnStr = globalConfig.GetConnectionString("RedisConnection");
                var redisConnection = ConnectionMultiplexer.Connect(redisConnStr);
                return new RedisContext(redisConnection, new ProtoBufSerializer());
            });
            ProtoBufConfig.Configure();

            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                x.RegisterConcreteTypesAgainstTheFirstInterface();
            });
            For<IRoundRepository>().Use<RoundRepository>();
            For<IGameRepository>().Use<GameRepository>();
            
//            For(typeof(IRepository<>)).DecorateAllWith(typeof(RepositoryLogDecorator<>));
            For<ILogger>().Use(Log.Logger);
        }
    }
}