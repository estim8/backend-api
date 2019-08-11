using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Persistence.Repositories;

namespace Estim8.Backend.Commands.Handlers
{
    public class HealthHandler : ICommandHandler<CheckDatabaseConnection>
    {
        private readonly IRepository _repo;

        public HealthHandler(IRepository repo)
        {
            _repo = repo;
        }
        
#pragma warning disable 1998
        public async Task<Response> Handle(CheckDatabaseConnection request, CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            try
            {
                _repo.PingConnection();
                return Response.Success;
            }
            catch (Exception e)
            {
                return Response.Failed(e);
            }
        }
    }
}