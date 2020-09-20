using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;
using RestSharp;
using System;
using NUnitAPITests.Config;
using System.Data;

namespace NUnitAPITests.Tests.Trello
{
    public class UpdateBoardTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();

            var expectedName = "BoardByCSharp";
            var expectedDescription = "This is a second description";

            var request = new TrelloRequest("boards");
            var requestBody = $"{{\"name\": \"{expectedName}\", \"desc\": \"{expectedDescription}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
        }

        [Test]
        public void UpdateBoardTest()
        {

            // Build request

            var expectedNameEdited = "BoardByCSharpEdited";
            var expectedDescriptionEdited = "This is a first description edited";

            var request = new TrelloRequest("boards/{id}");
            foreach (var id in ids)
            {
                request.GetRequest().AddParameter("id", id, ParameterType.UrlSegment);
            }

            // Build request
            var requestBody = $"{{\"name\": \"{expectedNameEdited}\", \"desc\": \"{expectedDescriptionEdited}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            //Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            //Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/UpdateBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            //Response body validation
            var actualName = jsonObject.SelectToken("name").ToString();
            Assert.AreEqual(expectedNameEdited, actualName);
            var actualDescription = jsonObject.SelectToken("desc").ToString();
            Assert.AreEqual(expectedDescriptionEdited, actualDescription);

        }


        [TearDown]
        public void DeleteBoards()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest($"boards/{id}");
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
