using Microsoft.Extensions.Configuration;
using System.IO;
using NUnitAPITests.Client;

namespace NUnitAPITests.Config
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
            if (instance == null) instance = new EnvironmentConfig();
            return instance;
        }

        public string GetToken(ApisEnum service)
        {
            return GetConfig(service).Token;
        }

        public string GetKey(ApisEnum service)
        {
            return GetConfig(service).Key;
        }

        public string GetBaseUrl(ApisEnum service)
        {
            return GetConfig(service).BaseUrl;
        }

        private ApiConfig GetConfig(ApisEnum service)
        {
            var config = new ApiConfig();
            switch (service)
            {
                case ApisEnum.Pivotal:
                    config = apiServices.Pivotal;
                    break;
                case ApisEnum.Trello:
                    config = apiServices.Trello;
                    break;
            }

            return config;
        }
    }
}
