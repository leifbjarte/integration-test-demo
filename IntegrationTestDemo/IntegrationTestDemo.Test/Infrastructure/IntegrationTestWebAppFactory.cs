using IntegrationTestDemo.Api;
using IntegrationTestDemo.Api.Http;
using IntegrationTestDemo.Api.ServiceBus;
using IntegrationTestDemo.Api.TableStorage;
using IntegrationTestDemo.Test.Infrastructure;
using IntegrationTestDemo.Test.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net.Http;

namespace IntegrationTestDemo.Test
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Startup>
    {
        public IntegrationTestWebAppFactory()
        {
            Environment.SetEnvironmentVariable("INTEGRATION_TEST", "true");
        }

        public Mock<ITableStorageRepository> TableStorageMock { get; set; } = new Mock<ITableStorageRepository>();

        public Mock<ThirdPartyApiMessageHandler> MessageHandlerMock { get; set; } = new Mock<ThirdPartyApiMessageHandler>();

        public Mock<IQueueMessageSender> MessageSenderMock { get; set; } = new Mock<IQueueMessageSender>();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var builder = services.AddAuthentication();
                builder.AddScheme<IntegrationTestAuthOptions, IntegrationTestAuthHandler>(IntegrationTestAuthHandler.AuthenticationScheme, opts => { });
                services.PostConfigureAll<JwtBearerOptions>(o => o.ForwardDefault = IntegrationTestAuthHandler.AuthenticationScheme);

                services.TryRemoveService<ITableStorageRepository>();
                services.AddSingleton(TableStorageMock.Object);

                services.TryRemoveService<IQueueMessageSender>();
                services.AddSingleton(MessageSenderMock.Object);

                services.TryRemoveService<ThirdPartyApiMessageHandler>();
                var client = new HttpClient(MessageHandlerMock.Object)
                {
                    BaseAddress = new Uri("https://bogus-address.com")
                };

                services.TryRemoveService<IHttpClientFactory>();
                var httpClientFactoryMock = new Mock<IHttpClientFactory>();
                httpClientFactoryMock.Setup(f => f.CreateClient(HttpClientNames.ThirdPartyApi)).Returns(client);
                services.AddSingleton(httpClientFactoryMock.Object);
            });
        }
    }
}
