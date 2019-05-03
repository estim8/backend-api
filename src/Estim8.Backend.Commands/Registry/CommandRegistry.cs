using Estim8.Backend.Commands.Handlers;
using Estim8.Backend.Persistence.Registry;
using Lamar;

namespace Estim8.Backend.Commands.Registry
{
    public class CommandRegistry : ServiceRegistry
    {
        public CommandRegistry()
        {
            IncludeRegistry<PersistenceRegistry>();
            
            Scan(x =>
            {
                x.AssemblyContainingType<CommandRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
            });
        }
    }
}