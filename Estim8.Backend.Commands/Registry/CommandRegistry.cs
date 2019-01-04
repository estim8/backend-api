using Estim8.Backend.Persistence.Registry;
using Lamar;

namespace Estim8.Backend.Commands.Registry
{
    public class CommandRegistry : ServiceRegistry
    {
        public CommandRegistry()
        {
            IncludeRegistry<PersistenceRegistry>();
        }
    }
}