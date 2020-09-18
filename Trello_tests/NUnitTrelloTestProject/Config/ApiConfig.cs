using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NUnitTrelloTestProject.Config
{
    public class ApiConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("baseUrl")]
        public string baseUrl { get; set; }
    }
}
