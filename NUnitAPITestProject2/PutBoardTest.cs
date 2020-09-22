using NUnit.Framework;
using NUnitAPITestProject2.ConfigClasses;
using RestSharp;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Collections.Generic;
using NUnitAPITestProject2.Client;


namespace NUnitAPITestProject2
{
    class PutBoardTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();

            // Post request
            var request = new TrelloRequest("boards");
            var requestBody = @"{
                ""name"":""SecondTesting"", 
                ""desc"":""IsAdescription""
            }";

            request.GetRequest().AddJsonBody(requestBody);

            // Send post request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Get the id board
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

        }

        [Test]
        public void PutBoardTest()
        {
            var request = new TrelloRequest("boards/{id}");

            foreach (var id in ids)
            {
                request.GetRequest().AddUrlSegment("id", id);
            }

            // Build the request
            var requestBody = @"{
                ""name"":""SecondTesting"", 
                ""desc"":""IsAdescription""
            }";
            request.GetRequest().AddJsonBody(requestBody);

            // Send put request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/putBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> errorHandled = new List<string>();

            // Assert json schema
            Assert.IsFalse(jsonObject.IsValid(jsonSchema, out errorHandled));

        }

        [TearDown]

            public void DeleteBoards()
            {
            foreach (var id in ids)
                {
                var request = new TrelloRequest("boards/" + id.ToString());
                var delete = RequestManager.Delete(TrelloClient.GetInstance(), request);
                // Assert.AreEqual(200, (int)delete.StatusCode);
                }
            }
    }
}