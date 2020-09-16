using NUnitAPITests.Config;
using RestSharp;

namespace NUnitAPITests.Client
{
    public class TrelloRequest : IRequest
    {
        private RestRequest request;

        public TrelloRequest(string resource)
        {
            request = new RestRequest();
            request.AddQueryParameter("key", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello));
            request.AddQueryParameter("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
            request.Resource = resource;
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
