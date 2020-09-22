using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NUnitAPITestProject2.ConfigClasses
{
    class ApiServices
    {
        [JsonProperty("pivotal")] public ApiConfig Pivotal { get; set; }
        [JsonProperty("trello")] public ApiConfig Trello { get; set; }
    }
}
