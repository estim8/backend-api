using Estim8.Backend.Api.Security;
using Estim8.Backend.Commands.Registry;
using Estim8.Backend.Queries.Registry;
using Estim8.Messaging.Registry;
using Lamar;
using Microsoft.Extensions.Options;

namespace Estim8.Backend.Api.Registry
{
    public class WebRegistry : ServiceRegistry
    {
        public WebRegistry()
        {
            For<ISecurityTokenService>().Use<SecurityTokenService>();
            
            IncludeRegistry<QueryRegistry>();
            IncludeRegistry<CommandRegistry>();
            
            IncludeRegistry<MessagingRegistry>();
            //For(typeof(IOptions<>)).Use(typeof(OptionsManager<>));
        }
    }
}