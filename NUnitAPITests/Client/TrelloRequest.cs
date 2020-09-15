using RestSharp;

namespace NUnitAPITests.Client
{
    public class TrelloRequest : IRequest
    {
        private RestRequest request;
        public TrelloRequest(string resource)
        {
            request = new RestRequest();
            request.Resource = resource;
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
