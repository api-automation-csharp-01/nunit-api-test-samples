using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace NUnitTrelloTestProject.Client
{
    public interface IRequest
    {
        RestRequest GetRequest();

    }
}
