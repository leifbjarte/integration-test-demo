using CloudNative.CloudEvents;
using IntegrationTestDemo.Api.ApiModels;
using IntegrationTestDemo.Api.Http;
using IntegrationTestDemo.Api.RequestModels;
using IntegrationTestDemo.Api.ServiceBus;
using IntegrationTestDemo.Api.TableStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient thirdPartyClient;
        private readonly ITableStorageRepository tableStorageThingy;
        private readonly IQueueMessageSender messageSender;

        public ProxyController(IHttpClientFactory httpClientFactory, ITableStorageRepository tableStorageThingy, IQueueMessageSender messageSender)
        {
            thirdPartyClient = httpClientFactory.CreateClient(HttpClientNames.ThirdPartyApi);
            this.tableStorageThingy = tableStorageThingy;
            this.messageSender = messageSender;
        }

        [HttpGet("proxy/some-third-party-resource/{identifier}")]
        public async Task<ActionResult<ThirdPartyData>> GetDataFromThirdPartyApi(string identifier)
        {
            var response = await thirdPartyClient.GetAsync($"api/resource/{identifier}");

            if (!response.IsSuccessStatusCode)
                return new StatusCodeResult(424); //dependency failed

            var body = await response.Content.ReadAsStringAsync();

            return new ThirdPartyData { SomeData = body };
        }

        [HttpGet("proxy/some-data-from-table-storage/{identifier}")]
        public async Task<ActionResult<string>> GetDataFromTableStorage(string identifier)
        {
            var data = await tableStorageThingy.GetSomeContentAsync(new ITableStorageRepository.PartitionRowKey("dataset", identifier));

            return data;
        }

        [HttpPut("proxy/some-data-from-table-storage/{identitier}")]
        public async Task<ActionResult> UpdateDataInTableStorage(string identifier, [FromBody] SomeDataInput requestData)
        {
            await tableStorageThingy.UpdateSomeContentAsync(new ITableStorageRepository.PartitionRowKey("dataset", identifier), requestData.SomeData);

            return Accepted();
        }

        [HttpPost("proxy/some-message-to-other-system")]
        public async Task<ActionResult> SendMessageToOtherSystem([FromBody] SomeDataInput requestData)
        {
            var message = new CloudEvent("eventType", new Uri("https://proxy-service.com"))
            {
                Data = requestData.SomeData
            };

            await messageSender.SendMessageToQueueAsync(message);

            return Accepted();
        }
    }
}
