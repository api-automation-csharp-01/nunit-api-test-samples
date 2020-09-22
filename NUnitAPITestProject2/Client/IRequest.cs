using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace NUnitAPITestProject2.Client
{
    public interface IRequest
    {
        RestRequest GetRequest();
    }
}
