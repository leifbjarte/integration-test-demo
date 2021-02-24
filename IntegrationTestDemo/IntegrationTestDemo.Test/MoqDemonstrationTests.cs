using CloudNative.CloudEvents;
using FluentAssertions;
using IntegrationTestDemo.Api.ApiModels;
using IntegrationTestDemo.Test.Infrastructure;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static IntegrationTestDemo.Api.TableStorage.ITableStorageRepository;

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
            var content = "Some data from table storage";

            //mock setup

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var response = await client.GetAsync($"proxy/some-data-from-table-storage/{identifier}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Be(content);
        }

        [Fact]
        public async Task PutDataToTableStorage_ShouldSendCorrectData()
        {
            var identifier = Guid.NewGuid().ToString();

            //mock setups?

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var testInput = new
            {
                SomeData = "Some test data for testing the put endpoint"
            };

            var content = new StringContent(JsonConvert.SerializeObject(testInput), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"proxy/some-data-from-table-storage/{identifier}", content);
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            //mock verification
        }

        [Fact]
        public async Task SendMessageToQueue_ShouldSendCorrectMessage()
        {
            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            //mock setup, with callback

            var testInput = new
            {
                SomeData = "Some message body content for third party"
            };

            var content = new StringContent(JsonConvert.SerializeObject(testInput), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"proxy/some-message-to-other-system", content);
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            //mock/callback verification
        }

        [Fact]
        public async Task GetDataFromThirdPartyApi_ShouldMapStuffCorrectly()
        {
            var identifier = Guid.NewGuid().ToString();
            var content = "Some content from third party API";

            //protected mock setup

            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var response = await client.GetAsync($"proxy/some-third-party-resource/{identifier}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var bodyRaw = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<ThirdPartyData>(bodyRaw);
            deserialized.SomeData.Should().Be(content);

            //mock verification?
        }
    }
}
