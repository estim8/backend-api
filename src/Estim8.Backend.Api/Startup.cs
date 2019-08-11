using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Estim8.Backend.Api.Configurations;
using Estim8.Backend.Api.Health;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Persistence;
using Lamar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis.Extensions.Core.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Estim8.Backend.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public Startup(IHostingEnvironment env, IConfiguration config, ILogger<Startup> logger)
        {
            _env = env;
            _config = config;
            _logger = logger;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddOptions();
            services.Configure<PersistenceConfiguration>(_config.GetSection(nameof(PersistenceConfiguration)));

            services.AddHealthChecks()
                .AddCheck<ContainerHealthCheck>("ioc")
                .AddCheck<DatabaseHealthCheck>("database");
            
            services.AddAuthenticationConfiguration(_config);
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddSignalR();
            
            services.AddSwaggerConfiguration();

            services.AddCorsConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {   
            _logger.LogInformation("Starting application in {env}", _env.EnvironmentName);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            
            app.UseAuthentication();
            
            app.UseStaticFiles();
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseSignalR(routes => { routes.MapHub<GameHub>("/hubs/games"); });
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estim8 API");
                c.RoutePrefix = "";
                c.InjectStylesheet("/swagger-ui.css");
            });
        }
    }
}
