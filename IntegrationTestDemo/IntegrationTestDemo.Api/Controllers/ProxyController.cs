using IntegrationTestDemo.Api.Http;
using IntegrationTestDemo.Api.RequestModels;
using IntegrationTestDemo.Api.TableStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient thirdPartyClient;
        private readonly ITableStorageThingy tableStorageThingy;

        public ProxyController(IHttpClientFactory httpClientFactory, ITableStorageThingy tableStorageThingy)
        {
            thirdPartyClient = httpClientFactory.CreateClient(HttpClientNames.ThirdPartyApi);
            this.tableStorageThingy = tableStorageThingy;
        }

        [HttpGet("proxy/some-third-party-resource/{identifier}")]
        public async Task<ActionResult> GetDataFromThirdPartyApi(string identifier)
        {
            var response = await thirdPartyClient.GetAsync($"api/resource/{identifier}");
            //check response and deserialize

            return new OkResult();
        }

        [HttpGet("proxy/some-data-from-table-storage/{identifier}")]
        public async Task<ActionResult<string>> GetDataFromTableStorage(string identifier)
        {
            var data = await tableStorageThingy.GetSomeContentAsync(new ITableStorageThingy.PartitionRowKey("dataset", identifier));

            return data;
        }

        [HttpPut("proxy/some-data-from-table-storage/{identitier}")]
        public async Task<ActionResult> UpdateDataInTableStorage(string identifier, [FromBody] SomeDataInput requestData)
        {
            await tableStorageThingy.UpdateSomeContentAsync(new ITableStorageThingy.PartitionRowKey("dataset", identifier), requestData.SomeData);

            return Accepted();
        }
    }
}
