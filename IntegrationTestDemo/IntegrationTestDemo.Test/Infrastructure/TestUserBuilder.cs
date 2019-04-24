using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace IntegrationTestDemo.Test.Infrastructure
{
    public class TestUserBuilder
    {
        public TestUserBuilder(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; set; }

        public TestUserProfile Profile { get; set; }

        public HttpClient AddTestAuthToken()
        {
            var testToken = new TestToken
            {
                UniqueId = Profile.UniqueId,
                Name = Profile.Name,
                Email = Profile.Email
            };

            var serialized = JsonConvert.SerializeObject(testToken);
            var tokenBytes = Encoding.UTF8.GetBytes(serialized);
            var tokenString = Convert.ToBase64String(tokenBytes);

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenString}");

            return Client;
        }
    }
}
