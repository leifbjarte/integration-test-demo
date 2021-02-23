using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Http
{
    public class ThirdPartyApiMessageHandler : DelegatingHandler
    {
        public ThirdPartyApiMessageHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //do some stuff like add token or resolve URL dynamically

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
