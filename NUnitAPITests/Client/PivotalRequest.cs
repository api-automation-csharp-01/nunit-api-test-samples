using NUnitAPITests.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Client
{
    public class PivotalRequest : IRequest
    {
        private RestRequest request;

        public PivotalRequest(string resource)
        {
            request = new RestRequest();
            request.AddHeader(name: "X-TrackerToken", value: EnvironmentConfig.GetInstance().GetToken(service: ApisEnum.Pivotal));
            request.Resource = resource;
        }

        public RestRequest GetRequest()
        {
            return request;
        }
    }
}
