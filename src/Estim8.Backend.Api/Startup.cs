using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Estim8.Backend.Persistence;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Estim8 API", Version = "v1"}); });
            services.AddCors(x => x.AddDefaultPolicy(new CorsPolicy
            {
                Origins = {"*"},
                Headers = {"*"},
                Methods = {"*"}
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {   
            _logger.LogInformation("Starting application in {env}", _env.EnvironmentName);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estim8 API"); });

            app.UseCors();
        }
    }
}
