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
            ForSingletonOf<IConnectionMultiplexer>().Use(ctx =>
            {
                var globalConfig = ctx.GetInstance<IConfiguration>();
                var redisConnStr = globalConfig.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(redisConnStr);
            });

            ProtoBufConfig.Configure();
            For<ISerializer>().Use<Estim8.Backend.Persistence.ProtoBuf.ProtoBufSerializer>();
            For<CachingFramework.Redis.Contracts.ISerializer>().Use<Estim8.Backend.Persistence.ProtoBuf.ProtoBufSerializer>();
            
            ForSingletonOf<IContext>().Use<RedisContext>()
                .SelectConstructor(() => new RedisContext((IConnectionMultiplexer)null, (CachingFramework.Redis.Contracts.ISerializer)null));

            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                x.RegisterConcreteTypesAgainstTheFirstInterface();
            });
            For<IRepository>().Use<RedisRepository>();
            For<IRoundRepository>().Use<RoundRepository>();
            For<IGameRepository>().Use<GameRepository>();
            For<IPlayerRepository>().Use<PlayerRepository>();
            For<IPlayCardRepository>().Use<PlayCardRepository>();
            
//            For(typeof(IRepository<>)).DecorateAllWith(typeof(RepositoryLogDecorator<>));
            For<ILogger>().Use(Log.Logger);
        }
    }
}