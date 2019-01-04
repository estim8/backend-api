using Estim8.Backend.Persistence.Repositories;
using Lamar;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace Estim8.Backend.Persistence.Registry
{
    public class PersistenceRegistry : ServiceRegistry
    {
        public PersistenceRegistry()
        {
            For<IMongoClient>().Use(ctx =>
            {
                var conn = ctx.GetInstance<IOptions<PersistenceConfiguration>>().Value
                    .ConnectionString;
                return new MongoClient(conn);
            });

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