using System;
using System.Threading;
using System.Threading.Tasks;
using Estim8.Backend.Commands.Commands;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Estim8.Backend.Api.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IMediator _mediator;

        public DatabaseHealthCheck(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var connCheck = await _mediator.Send(new CheckDatabaseConnection());
            
            if(!connCheck.IsSuccess)
                return HealthCheckResult.Unhealthy(connCheck.ErrorMessage, connCheck.Exception);
            
            return HealthCheckResult.Healthy();
        }
    }
}