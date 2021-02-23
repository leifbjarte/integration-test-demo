using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.TableStorage
{
    public class TableStorageThingy : ITableStorageThingy
    {
        public TableStorageThingy()
        {

        }

        public Task<string> GetSomeContentAsync(ITableStorageThingy.PartitionRowKey identifier)
        {
            return Task.FromResult(string.Empty);
        }

        public Task UpdateSomeContentAsync(ITableStorageThingy.PartitionRowKey partitionRowKey, string someData)
        {
            return Task.CompletedTask;
        }
    }
}
