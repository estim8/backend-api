using Estim8.Backend.Persistence.Repositories;
using Lamar;

namespace Estim8.Backend.Persistence.Registry
{
    public class PersistenceRegistry : ServiceRegistry
    {
        public PersistenceRegistry()
        {
            Scan(x =>
            {
                x.AssemblyContainingType<PersistenceRegistry>();
                x.AddAllTypesOf(typeof(IRepository<>));
            });
        }
    }
}