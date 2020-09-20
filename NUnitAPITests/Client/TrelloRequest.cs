using NUnitAPITests.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Client
{
    public class TrelloRequest: IRequest
    {
        private RestRequest request;

        public TrelloRequest(string resource)
        {
            request = new RestRequest();            
            request.AddParameter("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello), ParameterType.QueryString);
            request.AddParameter("key", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello), ParameterType.QueryString);
            request.Resource = resource;
        }
        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
