using NUnit.Framework;
using RestSharp;
using NUnitAPITests.Config;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using NUnitAPITests.Client;

namespace NUnitAPITests.Tests
{
    public class PivotalPostProjectTest
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
        }

        [Test]
        public void PostProjectTest()
        {
            // Build request
            var request = new PivotalRequest(resource: "projects");
            var requestBody = "{\"name\": \"Executioner\"}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(PivotalClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText(path: "Schemas/Pivotal/PostProjectSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach(var id in ids)
            {
                var request = new PivotalRequest(resource: "projects/" + id);
                RequestManager.Delete(PivotalClient.GetInstance(), request);
            }
        }
    }
}