using NUnitAPITests.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NUnitAPITests.Client
{
    public class TrelloRequest : IRequest
    {
        private RestRequest request;

        public TrelloRequest(string resource)
        {
            request = new RestRequest();
            request.AddParameter(name: "key", value: EnvironmentConfig.GetInstance().GetKey(service: ApisEnum.Trello), type: ParameterType.QueryString);
            request.AddParameter(name: "token", value: EnvironmentConfig.GetInstance().GetToken(service: ApisEnum.Trello), type: ParameterType.QueryString);
            request.Resource = resource;
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
