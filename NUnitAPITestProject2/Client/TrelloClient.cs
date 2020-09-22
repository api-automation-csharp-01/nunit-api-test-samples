using System;
using System.Collections.Generic;
using System.Text;
using NUnitAPITestProject2.Client;
using NUnitAPITestProject2.ConfigClasses;
using RestSharp;

namespace NUnitAPITestProject2.Client
{
    public sealed class TrelloClient : IClient
    {
        private static TrelloClient instance;
        private RestClient client;

        private TrelloClient()
        {
            client = new RestClient(EnvironmentConfig.GetInstance().GetBaseUrl(ApisEnum.Trello));
        }

        public static TrelloClient GetInstance()
        {
            if (instance == null) instance = new TrelloClient();

            return instance;
        }

        public RestClient GetClient()
        {
            return client;
        }
    }
}
