using Estim8.Backend.Commands.Registry;
using Estim8.Backend.Queries.Registry;
using Estim8.Messaging.Registry;
using Lamar;

namespace Estim8.Backend.Api.Registry
{
    public class WebRegistry : ServiceRegistry
    {
        public WebRegistry()
        {
            IncludeRegistry<QueryRegistry>();
            IncludeRegistry<CommandRegistry>();
            
            IncludeRegistry<MessagingRegistry>();
           
        }
    }
}