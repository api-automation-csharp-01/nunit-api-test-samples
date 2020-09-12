using RestSharp;

namespace NUnitAPITests.Client
{
    public interface IRequest
    {
        RestRequest GetRequest();
    }
}
