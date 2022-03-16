using System.Net;
using Microsoft.Azure.Cosmos;
using SandboxApi.Dtos;

namespace SandboxApi.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarDto>> GetItemsAsync();
        Task<CarDto?> GetItemAsync(string id);
        Task AddItemAsync(CarDto item);
        Task DeleteItemAsync(string id);
    }

    public class CarRepository : ICarRepository
    {
        private readonly Container _container;

        public CarRepository(CosmosClient client)
        {
            _container = client.GetContainer("test-db", "cars");
        }

        public async Task<IEnumerable<CarDto>> GetItemsAsync()
        {
            var query = this._container.GetItemQueryIterator<CarDto>(new QueryDefinition("SELECT * FROM c"));
            List<CarDto> results = new List<CarDto>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<CarDto?> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CarDto>(id, new PartitionKey(id));
                return response.Resource;

            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddItemAsync(CarDto item)
        {
            await _container.CreateItemAsync(item);
        }

        public async Task DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
