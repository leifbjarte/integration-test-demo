using CloudNative.CloudEvents;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.ServiceBus
{
    public class QueueMessageSender : IQueueMessageSender
    {
        public Task SendMessageToQueueAsync(CloudEvent message)
        {
            return Task.CompletedTask;
        }
    }
}
