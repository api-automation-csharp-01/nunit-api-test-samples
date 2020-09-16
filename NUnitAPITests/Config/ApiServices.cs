using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Config
{
    public class ApiServices
    {
        [JsonProperty("pivotal")]
        public ApiConfig Pivotal { get; set; }
        [JsonProperty("trello")]
        public ApiConfig Trello { get; set; }
    }
}
