﻿using NUnitAPITests.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Client
{
    public sealed class PivotalClient : IClient
    {
        private static PivotalClient instance;
        private RestClient client;
        private PivotalClient()
        {
            client = new RestClient(baseUrl: EnvironmentConfig.GetInstance().GetBaseUrl(service: ApisEnum.Pivotal));
        }

        public static PivotalClient GetInstance()
        {
            if (instance == null)
            {
                instance = new PivotalClient();
            }

            return instance;
        }

        public RestClient GetClient()
        {
            return client;
        }
    }
}
