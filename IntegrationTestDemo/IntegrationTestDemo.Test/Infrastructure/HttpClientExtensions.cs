using System.Net.Http;

namespace IntegrationTestDemo.Test.Infrastructure
{
    public static class HttpClientExtensions
    {
        public static TestUserBuilder WithTestUser(this HttpClient client, TestUserProfile userProfile)
        {
            var builder = new TestUserBuilder(client)
            {
                Profile = userProfile
            };

            return builder;
        }
    }
}
