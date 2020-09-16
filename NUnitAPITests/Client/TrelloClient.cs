using NUnitAPITests.Config;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Client 
{
    public sealed class TrelloClient : IClient
    {
        private static TrelloClient instance;
        private RestClient client;

        public TrelloClient()
        {
            client = new RestClient(EnvironmentConfig.GetInstance().GetBaseUrl(ApisEnum.Trello));
            //client.Authenticator = new HttpBasicAuthenticator(EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello), 
            //    EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
        }

        public static TrelloClient GetInstance()
        {
            if (instance == null)
            {
                instance = new TrelloClient();
            }
            return instance;
        }

        public RestClient GetClient()
        {
            return client;
        }
    }
}
