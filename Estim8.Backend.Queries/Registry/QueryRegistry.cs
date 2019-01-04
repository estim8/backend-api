using Estim8.Backend.Persistence.Registry;
using Estim8.Backend.Queries.Handlers;
using Estim8.Backend.Queries.Queries;
using Lamar;
using MediatR;

namespace Estim8.Backend.Queries.Registry
{
    public class QueryRegistry : ServiceRegistry
    {
        public QueryRegistry()
        {
            IncludeRegistry<PersistenceRegistry>();
            
            Scan(x =>
            {
                x.AssemblyContainingType<QueryRegistry>();
                x.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                x.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                x.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });
        }
    }
}