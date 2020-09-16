using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;
using RestSharp;
using System;
using NUnitAPITests.Config;

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

        [Test]
        public void PostBoardTest()
        {
            // Build request
            var expectedName = "BoardByCSharp";
            var expectedDescription = "This is a first description";
            var expectedPrefsPermissionLevel = "public";
            var expectedPrefsVoting = "observers";
            var expectedPrefsBackground = "orange";

            var request = new TrelloRequest("boards");
            var requestBody = $"{{\"name\": \"{expectedName}\", \"desc\": \"{expectedDescription}\", " +
                $"\"defaulLists\": \"false\", \"prefs_permissionLevel\": \"{expectedPrefsPermissionLevel}\"," +
                $" \"powerUps\": \"calendar\", \"prefs_voting\": \"{expectedPrefsVoting}\"," +
                $" \"prefs_comments\": \"members\", \"prefs_invitations\": \"admins\", " +
                $"\"prefs_background\": \"{expectedPrefsBackground}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PostBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            //Response body validation
            var actualName = jsonObject.SelectToken("name").ToString();
            Assert.AreEqual(expectedName, actualName);
            var actualDescription = jsonObject.SelectToken("desc").ToString();
            Assert.AreEqual(expectedDescription, actualDescription);
            var actualPrefsPermissionLevel = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            Assert.AreEqual(expectedPrefsPermissionLevel, actualPrefsPermissionLevel);
            var actualPrefsVoting = jsonObject.SelectToken("prefs.voting").ToString();
            Assert.AreEqual(expectedPrefsVoting, actualPrefsVoting);
            var actualPrefsBackground = jsonObject.SelectToken("prefs.background").ToString();
            Assert.AreEqual(expectedPrefsBackground, actualPrefsBackground);

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
