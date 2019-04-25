using IntegrationTestDemo.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System;

namespace IntegrationTestDemo.Test
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Startup>
    {
        public IntegrationTestWebAppFactory()
        {
            Environment.SetEnvironmentVariable("INTEGRATION_TEST", "true");
        }
    }
}
