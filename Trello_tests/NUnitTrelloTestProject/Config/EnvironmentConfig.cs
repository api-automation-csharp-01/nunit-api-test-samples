using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NUnitTrelloTestProject.Config
{
    public sealed class EnvironmentConfig
    {
        private static EnvironmentConfig instance;
        private ApiServices apiServices;
        private EnvironmentConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestSettings.json")
                .Build();
            apiServices = builder.Get<ApiServices>();
        }

        public static EnvironmentConfig GetInstance()
        {
            if (instance==null)
            {
                instance = new EnvironmentConfig();
            }
            return instance;
        }

        public string GetToken(string service)
        {
            return GetConfig(service).Token;
        }
        public string GetKey(string service)
        {
            return GetConfig(service).Key;
        }

        public string GetBaseUrl(string service)
        {
            return GetConfig(service).baseUrl;
        }

        private ApiConfig GetConfig(string service)
        {
            ApiConfig config = new ApiConfig();
            switch (service)
            {
                case "Trello":
                    config = apiServices.Trello;
                    break;
            }
            return config;
        }
    }
}
