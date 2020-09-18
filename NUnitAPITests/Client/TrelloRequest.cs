using RestSharp;
using NUnitAPITests.Config;

namespace NUnitAPITests.Client
{
    public class TrelloRequest : IRequest
    {
        private RestRequest request;
        public TrelloRequest(string resource)
        {
            request = new RestRequest();
            request.Resource = resource;
            request.AddParameter("key", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello), ParameterType.QueryString);
            request.AddParameter("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello), ParameterType.QueryString);
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
