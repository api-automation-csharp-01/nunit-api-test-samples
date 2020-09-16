using Newtonsoft.Json;

namespace NUnitAPITests.Models.Pivotal
{
    public class ProjectRequestModel
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("iteration_length")] public int IterationLength { get; set; }
        [JsonProperty("week_start_day")] public string WeekStartDay { get; set; }
        [JsonProperty("public")] public bool Public { get; set; }
    }
}