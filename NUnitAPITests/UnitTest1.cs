using NUnit.Framework;
using RestSharp;
using NUnitAPITests.Config;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;

namespace NUnitAPITests
{
    public class Tests
    {
        private List<string> ids;
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
            client = new RestClient(EnvironmentConfig.GetInstance().GetBaseUrl());
        }

        [Test]
        public void Test1()
        {
            var request = new RestRequest("projects", Method.POST);
            request.AddHeader("X-TrackerToken", EnvironmentConfig.GetInstance().GetToken());
            request.AddJsonBody("{\"name\": \"Executioner\"}");

            var response = client.Execute(request);
            var responseBody = response.Content;
            var jsonObject = JObject.Parse(responseBody);
            Assert.AreEqual(200, (int)response.StatusCode);
            ids.Add(jsonObject.SelectToken("id").ToString());
            var jsonSchemaString = File.ReadAllText("Schemas/PostProjectSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));
        }

        [TearDown]
        public void TearDown()
        {
            foreach(var id in ids)
            {
                var request = new RestRequest("projects/" + id, Method.DELETE);
                request.AddHeader("X-TrackerToken", EnvironmentConfig.GetInstance().GetToken());
                client.Execute(request);
            }
        }
    }
}