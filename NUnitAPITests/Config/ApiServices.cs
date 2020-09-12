using Newtonsoft.Json;

namespace NUnitAPITests.Config
{
    public class ApiServices
    {
        [JsonProperty("pivotal")] public ApiConfig Pivotal { get; set; }
        [JsonProperty("trello")] public ApiConfig Trello { get; set; }
    }
}
