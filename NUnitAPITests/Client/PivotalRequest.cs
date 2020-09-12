using NUnitAPITests.Config;
using RestSharp;

namespace NUnitAPITests.Client
{
    public class PivotalRequest : IRequest
    {
        private RestRequest request;

        public PivotalRequest(string resource)
        {
            request = new RestRequest();
            request.AddHeader("X-TrackerToken", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Pivotal));
            request.Resource = resource;
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
