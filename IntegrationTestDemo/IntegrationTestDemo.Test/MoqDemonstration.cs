using FluentAssertions;
using IntegrationTestDemo.Test.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTestDemo.Test
{
    public class MoqDemonstrationTests
    {
        private readonly IntegrationTestWebAppFactory factory;

        public MoqDemonstrationTests()
        {
            factory = new IntegrationTestWebAppFactory();
        }

        [Fact]
        public async Task GetDataFromTableStorage_ShouldMapStuffCorrectly()
        {
            var identifier = Guid.NewGuid().ToString();

            factory.TableStorageMockAction = mock =>
            {
                //do the mock
            };

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var response = await client.GetAsync($"proxy/some-data-from-table-storage/{identifier}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutDataToTableStorage_ShouldSendCorrectData()
        {
            var identifier = Guid.NewGuid().ToString();

            factory.TableStorageMockAction = mock =>
            {
                //do the mock
                //intercept result
            };

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var testInput = new
            {
                SomeData = "Some test data for testing the put endpoint"
            };

            var content = new StringContent(JsonConvert.SerializeObject(testInput), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"proxy/some-data-from-table-storage/{identifier}", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDataFromThirdPartyApi_ShouldMapStuffCorrectly()
        {
            var identifier = Guid.NewGuid().ToString();

            factory.MessageHandlerMockAction = mock =>
            {
                //do the mock
            };

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var response = await client.GetAsync($"proxy/some-third-party-resource/{identifier}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
