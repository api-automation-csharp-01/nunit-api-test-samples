
using NUnitAPITests.Config;
using RestSharp;

namespace NUnitAPITests.Client
{
    public class TrelloClient : IClient
    {
        public static TrelloClient instance;

        private RestClient client;

        private TrelloClient()
        {
            client = new RestClient(EnvironmentConfig.GetInstance().GetBaseUrl(ApisEnum.Trello));  
        }

        public static TrelloClient GetInstance()
        {
            return instance == null ? new TrelloClient() : instance;
        }

        public RestClient GetClient()
        {
            return client;
        }
    }
}
