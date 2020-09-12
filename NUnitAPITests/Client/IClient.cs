using RestSharp;

namespace NUnitAPITests.Client
{
    public interface IClient
    {
        RestClient GetClient();
    }
}
