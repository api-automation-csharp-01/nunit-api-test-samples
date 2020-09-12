using NUnitAPITests.Config;
using RestSharp;

namespace NUnitAPITests.Client
{
    public sealed class PivotalClient : IClient
    {
        private static PivotalClient instance;
        private RestClient client;

        private PivotalClient()
        {
            client = new RestClient(EnvironmentConfig.GetInstance().GetBaseUrl(ApisEnum.Pivotal));
        }

        public static PivotalClient GetInstance()
        {
            if (instance == null) instance = new PivotalClient();

            return instance;
        }

        public RestClient GetClient()
        {
            return client;
        }
    }
}
