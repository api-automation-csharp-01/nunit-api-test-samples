using RestSharp;

namespace NUnitAPITests.Client
{
    public sealed class RequestManager
    {
        public static IRestResponse Get(IClient client, IRequest request)
        {
            request.GetRequest().Method = Method.GET;
            return client.GetClient().Execute(request.GetRequest());
        }

        public static IRestResponse Post(IClient client, IRequest request)
        {
            request.GetRequest().Method = Method.POST;
            return client.GetClient().Execute(request.GetRequest());
        }

        public static IRestResponse Put(IClient client, IRequest request)
        {
            request.GetRequest().Method = Method.PUT;
            return client.GetClient().Execute(request.GetRequest());
        }

        public static IRestResponse Delete(IClient client, IRequest request)
        {
            request.GetRequest().Method = Method.DELETE;
            return client.GetClient().Execute(request.GetRequest());
        }
    }
}
