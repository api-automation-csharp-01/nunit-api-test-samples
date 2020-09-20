using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;
using System;

namespace NUnitAPITests.Tests.Trello
{
    public class PostBoardTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
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
        public void PostBoardTest()
        {
            var nameExpected = "FirstBoard";
            var descriptionExpected = "First Board for Automation API task";
            var prefs_permissionLevelExpected = "public";
            var prefs_backgroundExpected = "green";

            // Build request
            var request = new TrelloRequest("boards");
            var requestBody = "{\"name\": \"" + nameExpected + "\", " +
                "\"desc\": \"" + descriptionExpected + "\", " +
                "\"prefs_permissionLevel\": \"" + prefs_permissionLevelExpected + "\", " +
                "\"prefs_background\": \"" + prefs_backgroundExpected +
                "\"}";

            request.GetRequest().AddJsonBody(requestBody);
            
            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);
            
            // Validate status code
            Assert.AreEqual(200, (int) response.StatusCode);
            
            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            //Validate fields
            Assert.AreEqual(nameExpected, jsonObject.SelectToken("name").ToString());
            Assert.AreEqual(descriptionExpected, jsonObject.SelectToken("desc").ToString());
            Assert.AreEqual(prefs_permissionLevelExpected, jsonObject.SelectToken("prefs.permissionLevel").ToString());
            Assert.AreEqual(prefs_backgroundExpected, jsonObject.SelectToken("prefs.background").ToString());

            // Assert json schema
            ValidateJsonSchema(jsonObject, "Schemas/Trello/PostBoardSchema.json");
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
