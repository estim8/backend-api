using Estim8.Backend.Commands.Decorators;
using Estim8.Backend.Commands.Handlers;
using Estim8.Backend.Commands.Services;
using Estim8.Backend.Persistence.Registry;
using Lamar;

namespace Estim8.Backend.Commands.Registry
{
    public class CommandRegistry : ServiceRegistry
    {
        public CommandRegistry()
        {
            IncludeRegistry<PersistenceRegistry>();
            
            For<ISecurityTokenService>().Use<SecurityTokenService>();
            
            Scan(x =>
            {
                x.AssemblyContainingType<CommandRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                x.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
            });
            
            For(typeof(ICommandHandler<>)).DecorateAllWith(typeof(ExceptionResponseDecorator<>));
        }
    }
}