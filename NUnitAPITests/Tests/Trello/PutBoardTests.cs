using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;
using System;

namespace NUnitAPITests.Tests.Trello
{
    public class PutBoardTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
            var name = "FirstBoard";
            // Build request
            var request = new TrelloRequest("boards");
            var requestBody = "{\"name\": \"" + name + "\"}";

            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

        }

        private void ValidateJsonSchema(JObject jsonObject, string schemaPath) {
            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText(schemaPath);
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));
        }

        [Test]
        public void PutBoardTest()
        {
            var name = "FirstBoard UPDATED";
            var description = "First Board for Automation API task UPDATED";
            var prefs_permissionLevelExpected = "private";

            foreach (var id in ids)
            {
                var request = new TrelloRequest("boards/" + id);
                var requestBody = "{\"name\": \"" + name + "\", " +
                    "\"desc\": \"" + description + "\", " +
                    "\"prefs/permissionLevel\": \"" + prefs_permissionLevelExpected +
                    "\"}";
                request.GetRequest().AddJsonBody(requestBody);


                // Send request
                var response = RequestManager.Put(TrelloClient.GetInstance(), request);

                // Validate status code
                Assert.AreEqual(200, (int)response.StatusCode);

                // Parse response to json object
                var jsonObject = JObject.Parse(response.Content);

                //Validate fields
                Assert.AreEqual(name, jsonObject.SelectToken("name").ToString());
                Assert.AreEqual(description, jsonObject.SelectToken("desc").ToString());
                Assert.AreEqual(prefs_permissionLevelExpected, jsonObject.SelectToken("prefs.permissionLevel").ToString());

                // Assert json schema
                ValidateJsonSchema(jsonObject, "Schemas/Trello/PutBoardSchema.json");
            }
        }

        [TearDown]
        public void DeleteBoard()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest("boards/" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
