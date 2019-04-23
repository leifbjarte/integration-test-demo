using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTestDemo.Test
{
    public class PlayGround
    {
        [Fact]
        public async Task GetValuesShouldReturnProperValuesAsync()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            var message = new HttpRequestMessage(HttpMethod.Get, "api/values");
            message.Headers.Add("x-test", "It works!");

            var result = await client.SendAsync(message);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPersonShouldInvokeModelBinder()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            var result = await client.GetAsync("persons/test@testesen.com");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UploadFileShouldNotFail()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            //var fileContent = new MultipartFileData(new HttpContentHeaders(), "TestContent.json");

            //var result = await client.PostAsync($"files/{Guid.NewGuid()}", fileContent);
        }
    }
}
