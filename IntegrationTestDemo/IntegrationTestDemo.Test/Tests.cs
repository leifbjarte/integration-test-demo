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
    public class Tests : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IntegrationTestWebAppFactory factory;

        public Tests(IntegrationTestWebAppFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetValueByIdShouldThrowException()
        {
            var client = factory.CreateClient();

            var result = await client.GetAsync("persons/test@testesen.com");

            result.StatusCode.Should().Be(HttpStatusCode.NotModified);
        }

        /// <summary>
        /// Illustrating file upload support, model binding, anonymous deseralization and custom headers
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadFileAnonymouslyForPersonShouldWork()
        {
            var client = factory.CreateClient();

            var message = new HttpRequestMessage(HttpMethod.Post, $"persons/test@testesen.com/documents/{Guid.NewGuid()}");
            message.Headers.Add("x-test", "It works!");

            using (var fileContent = new MultipartFormDataContent())
            {
                var bytes = await File.ReadAllBytesAsync("Files/TestContent.json");
                fileContent.Add(new ByteArrayContent(bytes), "file", "TestContent.json");
                message.Content = fileContent;

                var response = await client.SendAsync(message);
                var body = await response.Content.ReadAsStringAsync();

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

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
        
        /// <summary>
        /// Illustrating file upload support, model binding, anonymous deseralization and custom headers
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadFileWithAuthenticationForPersonShouldWork()
        {
            var client = factory.CreateClient()
                .WithTestUser(TestUserProfile.TestTestesen)
                .AddTestAuthToken();

            var message = new HttpRequestMessage(HttpMethod.Post, $"persons/test@testesen.com/documents/{Guid.NewGuid()}");
            message.Headers.Add("x-test", "It works!");

            using (var fileContent = new MultipartFormDataContent())
            {
                var bytes = await File.ReadAllBytesAsync("Files/TestContent.json");
                fileContent.Add(new ByteArrayContent(bytes), "file", "TestContent.json");
                message.Content = fileContent;

                var response = await client.SendAsync(message);
                var body = await response.Content.ReadAsStringAsync();

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

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
