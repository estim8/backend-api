using System;
using System.Threading;
using System.Threading.Tasks;
using Lamar;
using Lamar.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Estim8.Backend.Api.Health
{
    public class ContainerHealthCheck : IHealthCheck
    {
        private readonly IContainer _ioc;

        public ContainerHealthCheck(IContainer container)
        {
            _ioc = container;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                _ioc.AssertConfigurationIsValid(AssertMode.ConfigOnly);
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("Container configuration is not valid", e);
            }
            
        }
    }
}