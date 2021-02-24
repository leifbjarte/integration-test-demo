using IntegrationTestDemo.Api.Authorization;
using IntegrationTestDemo.Api.Http;
using IntegrationTestDemo.Api.ModelBinding;
using IntegrationTestDemo.Api.ServiceBus;
using IntegrationTestDemo.Api.TableStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc(options => options.ModelBinderProviders.Insert(0, new PersonModelBinderProvider()));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddSingleton<IAuthorizationHandler, AdminAuthHandler>();

            services.AddTransient<ThirdPartyApiMessageHandler>();
            services.AddHttpClient(HttpClientNames.ThirdPartyApi)
                .AddHttpMessageHandler<ThirdPartyApiMessageHandler>();

            services.AddScoped<ITableStorageRepository, TableStorageRepository>();
            services.AddScoped<IQueueMessageSender, QueueMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

