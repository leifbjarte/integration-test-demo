using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.TableStorage
{
    public interface ITableStorageRepository
    {
        Task<string> GetSomeContentAsync(PartitionRowKey identifier);

        Task UpdateSomeContentAsync(PartitionRowKey partitionRowKey, string someData);

        public class PartitionRowKey
        {
            public PartitionRowKey(string partitionKey, string rowKey)
            {
                PartitionKey = partitionKey;
                RowKey = rowKey;
            }

            public string PartitionKey { get; set; }

            public string RowKey { get; set; }
        }
    }
}