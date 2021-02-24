using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.TableStorage
{
    public class TableStorageRepository : ITableStorageRepository
    {
        public Task<string> GetSomeContentAsync(ITableStorageRepository.PartitionRowKey identifier)
        {
            return Task.FromResult(string.Empty);
        }

        public Task UpdateSomeContentAsync(ITableStorageRepository.PartitionRowKey partitionRowKey, string someData)
        {
            return Task.CompletedTask;
        }
    }
}
