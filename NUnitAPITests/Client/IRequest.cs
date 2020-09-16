using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitAPITests.Client
{
    public interface IRequest
    {
        RestRequest GetRequest();

    }
}
