using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITestProject2.ConfigClasses
{
    public class ApiConfig
    {
        [JsonProperty("token")] public string Token { get; set; }
        [JsonProperty("key")] public string Key { get; set; }
        [JsonProperty("baseURL")] public string BaseUrl { get; set; }
    }
}
