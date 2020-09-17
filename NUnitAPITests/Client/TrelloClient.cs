using NUnitAPITests.Client;
using NUnitAPITests.Config;
using RestSharp;

namespace NUnitAPITests.Tests.Trello
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