using System;
using System.Collections.Generic;
using System.Text;
using NUnitTrelloTestProject.Config;
using RestSharp;

namespace NUnitTrelloTestProject.Client
{
    public class TrelloRequest : IRequest
    {
        private RestRequest request;
        public TrelloRequest(string resource)
        {
            request = new RestRequest();
            request.AddParameter("token", EnvironmentConfig.GetInstance().GetToken("Trello"), ParameterType.QueryString);
            request.AddParameter("key", EnvironmentConfig.GetInstance().GetKey("Trello"), ParameterType.QueryString);
            request.Resource = resource;
        }
        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
