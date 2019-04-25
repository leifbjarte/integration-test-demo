using IntegrationTestDemo.Api.Authentication;
using IntegrationTestDemo.Api.ErrorHandling;
using IntegrationTestDemo.Api.ModelBinding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IntegrationTestDemo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.ModelBinderProviders.Insert(0, new PersonModelBinderProvider()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Authentication

            var authBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    if (Environment.GetEnvironmentVariable("INTEGRATION_TEST") == "true")
                    {
                        options.ForwardAuthenticate = "IntegrationTestAuth";
                    }
                });

            if (Environment.GetEnvironmentVariable("INTEGRATION_TEST") == "true")
            {
                authBuilder.AddScheme<IntegrationTestAuthOptions, IntegrationTestAuthHandler>("IntegrationTestAuth", options => { });
            }

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

