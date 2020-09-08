using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NUnitAPITests.Config
{
    public sealed class EnvironmentConfig
    {
        private static EnvironmentConfig instance;
        private ApiConfig apiConfig;
        private EnvironmentConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestSettings.json")
                .Build();
            apiConfig = builder.Get<ApiConfig>();
        }

        public static EnvironmentConfig GetInstance()
        {
            if (instance == null)
            {
                instance = new EnvironmentConfig();
            }
            return instance;
        }

        public string GetToken()
        {
            return apiConfig.Token;
        }

        public string GetKey()
        {
            return apiConfig.Key;
        }
    }
}
