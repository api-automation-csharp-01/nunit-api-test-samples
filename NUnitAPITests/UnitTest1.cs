using NUnit.Framework;
using RestSharp;
using NUnitAPITests.Config;
using System;
using Newtonsoft.Json.Linq;

namespace NUnitAPITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var client = new RestClient("https://www.pivotaltracker.com/services/v5");
            var request = new RestRequest("projects", Method.POST);
            request.AddHeader("X-TrackerToken", EnvironmentConfig.GetInstance().GetToken());
            request.AddJsonBody("{\"name\": \"Executioner1\"}");

            var response = client.Execute(request);
            var responseBody = response.Content;
            var jsonObject = JObject.Parse(responseBody);
            //Assert.AreEqual(200, (int) response.StatusCode);

        }
    }
}