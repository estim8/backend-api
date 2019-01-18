using Cosmonaut;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Lamar;
using Microsoft.Extensions.Options;
using Serilog;

namespace Estim8.Backend.Persistence.Registry
{
    public class PersistenceRegistry : ServiceRegistry
    {
        public PersistenceRegistry()
        {
            ForSingletonOf<CosmosStoreSettings>().Use(ctx =>
            {
                var opt = ctx.GetInstance<IOptions<PersistenceConfiguration>>().Value;
                return new CosmosStoreSettings(opt.DefaultDatabase, opt.CosmosUri, opt.AuthKey);
            });

            //TODO: This should work, but currently it throws an ArgumentException about open generic types.
            //For(typeof(ICosmosStore<>)).Use(typeof(CosmosStore<>));
            
            //For now, register stores individually as below
            For<ICosmosStore<Game>>().Use<CosmosStore<Game>>().Singleton();

            For<ILogger>().Use(Log.Logger);
            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                x.RegisterConcreteTypesAgainstTheFirstInterface();
            });
        }
    }
}