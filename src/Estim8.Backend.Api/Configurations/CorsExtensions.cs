using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Estim8.Backend.Api.Configurations
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(x => x.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        "https://www.estim8.io", 
                        "https://www-qa.estim8.io", 
                        "http://localhost:8080",
                        "https://localhost:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(HttpResponseHeader.Location.ToString());
            }));

            return services;
        }
    }
}