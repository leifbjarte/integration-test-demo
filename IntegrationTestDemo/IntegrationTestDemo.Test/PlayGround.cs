using FluentAssertions;
using System;
using System.IO;
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

            using (var fileContent = new MultipartFormDataContent())
            {
                var bytes = await File.ReadAllBytesAsync("Files/TestContent.json");
                fileContent.Add(new ByteArrayContent(bytes), "file", "TestContent.json");

                var response = await client.PostAsync($"files/{Guid.NewGuid()}", fileContent);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
