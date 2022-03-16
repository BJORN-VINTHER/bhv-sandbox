using Microsoft.Azure.Cosmos;
using SandboxApi.Repositories;

namespace SandboxApi.Setup
{
    public static class CosmosDbExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration config)
        {
            var client = new CosmosClient(config["Cosmos:Url"], config["Cosmos:Key"], new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
            var db = client.GetDatabase("test-db");
            services.AddSingleton(db);
            services.AddSingleton<ICarRepository>(new CarRepository(client));

            return services;
        }
    }
}
