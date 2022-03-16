using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SandboxApi.Dtos;
using SandboxApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SandboxApi.Controllers
{
    [Route("cosmos-db")]
    [ApiController]
    public class CosmosDbController : ControllerBase
    {
        private readonly Database _db;
        private readonly ICarRepository _carRepository;

        public CosmosDbController(Database db, ICarRepository carRepository)
        {
            _db = db;
            _carRepository = carRepository;
        }

        [HttpPost("admin/containers")]
        public async Task<string> CreateContainer(string containerName, string partitionKey)
        {
            await _db.CreateContainerIfNotExistsAsync(containerName, partitionKey);
            return containerName;
        }

        [HttpDelete("admin/containers")]
        public async Task DeleteContainer(string containerName)
        {
            await _db.GetContainer(containerName).DeleteContainerAsync();
        }

        [HttpGet("cars/{carId}")]
        public async Task<CarDto?> GetCar(string carId)
        {
            return await _carRepository.GetItemAsync(carId);
        }

        [HttpPost("cars")]
        public async Task Post([FromBody] CarDto car)
        {
            await _carRepository.AddItemAsync(car);
        }

        [HttpGet("cars")]
        public async Task<IEnumerable<CarDto>> Get()
        {
            return await _carRepository.GetItemsAsync();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
