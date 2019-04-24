using FluentAssertions;
using IntegrationTestDemo.Test.Infrastructure;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTestDemo.Test
{
    public class SimpleScenario
    {
        /// <summary>
        /// Test to illustrate basic use of web app factory, including sending headers
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetValuesShouldBeSuccessfull()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            var message = new HttpRequestMessage(HttpMethod.Get, "api/values");
            message.Headers.Add("x-test", "It works!");

            var result = await client.SendAsync(message);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetValueByIdShouldThrowException()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            var result = await client.GetAsync("api/values/1");

            result.StatusCode.Should().Be(HttpStatusCode.NotModified);
        }

        /// <summary>
        /// Illustrating model binder support + anonymous types to create completely decoupled, refactor-safe tests.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPersonShouldInvokeModelBinder()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient();

            var result = await client.GetAsync("persons/test@testesen.com");
            var body = await result.Content.ReadAsStringAsync();

            var deserializeAs = new
            {
                Name = string.Empty,
                Email = string.Empty,
                UniqueId = Guid.Empty
            };

            var personId = JsonConvert.DeserializeAnonymousType(body, deserializeAs);
            personId.Name.Should().Be("Test Testesen");
            personId.Email.Should().Be("test@testesen.com");
            personId.UniqueId.Should().NotBeEmpty();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Illustrating file upload support
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadFileShouldNotFail()
        {
            var factory = new IntegrationTestWebAppFactory();
            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

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
