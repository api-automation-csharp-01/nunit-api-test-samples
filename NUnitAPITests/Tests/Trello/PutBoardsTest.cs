using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITest.Client;
using NUnitAPITests.Client;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NUnitAPITests.Tests.Trello
{
    class PutBoardsTest
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
            var expectedName = "API AUT board 8";
            var expectedDescription = "This is a board created.";
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
        public void PutBoardTest()
        {
            // Build request
            var expectedNameEdited = "API AUT board 8 EDITED";
            var expectedDescriptionEdited = "This is a board created EDITED.";
            var expectedPermissionLevelEdited = "private";
            var expectedVotingEdited = "disabled";
            var expectedBackgroundEdited = "blue";
            var request = new TrelloRequest("boards/{id}");

            foreach (var id in ids)
            {
                request.GetRequest().AddParameter("id", id, ParameterType.UrlSegment);
            }

            // Build request
            var requestBody = $"{{\"name\": \"{expectedNameEdited}\", \"desc\": \"{expectedDescriptionEdited}\", " +
                $"\"prefs_permissionLevel\": \"{expectedPermissionLevelEdited}\", \"prefs_voting\": \"{expectedVotingEdited}\", " +
                $"\"prefs_background\": \"{expectedBackgroundEdited}\"}}";

            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            //Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            //Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PutBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            //Response body validation
            var actualName = jsonObject.SelectToken("name").ToString();
            Assert.AreEqual(expectedNameEdited, actualName);
            var actualDescription = jsonObject.SelectToken("desc").ToString();
            Assert.AreEqual(expectedDescriptionEdited,actualDescription );
            var actualPermissionLevel = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            Assert.AreEqual(expectedPermissionLevelEdited, actualPermissionLevel);
            var actualVoting = jsonObject.SelectToken("prefs.voting").ToString();
            Assert.AreEqual(expectedVotingEdited, actualVoting);
            var actualPrefsBackground = jsonObject.SelectToken("prefs.background").ToString();
            Assert.AreEqual(expectedBackgroundEdited, actualPrefsBackground);
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
