using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SandboxApi.Dtos
{
    public class CarDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CarColor Color { get; set; }
    }

    public enum CarColor
    {
        Blue = 1,
        Green = 2,
        Red = 3,
        Black = 4,
    }
}
