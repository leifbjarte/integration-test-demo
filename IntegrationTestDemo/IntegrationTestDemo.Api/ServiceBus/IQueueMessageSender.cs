using CloudNative.CloudEvents;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.ServiceBus
{
    public interface IQueueMessageSender
    {
        Task SendMessageToQueueAsync(CloudEvent message);
    }
}