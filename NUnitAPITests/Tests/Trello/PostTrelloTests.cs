using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;
using System;
using NUnitAPITests.Config;

namespace NUnitAPITests.Tests.Trello
{
    class PostTrelloTests
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
            // Values 
            var nameBoard = "Miauuuu";
            var desBoard = "12";
            var keepFromSourceBoard = "cards";
            var powerUpsBoard = "cardAging";
            var prefs_permissionLevelBoard = "private";
            var prefs_votingBoard = "members";
            var prefs_invitationsBoard = "admins";
            var prefs_backgroundBoard = "pink";

            // Build request
            var request = new TrelloRequest("boards");
            var requestBody = $"{{\"name\": \"{nameBoard}\", \"desc\": \"{desBoard}\" , \"keepFromSource\" : \"{keepFromSourceBoard}\" , \"powerUps\": \"{powerUpsBoard}\", \"prefs_permissionLevel\":\"{prefs_permissionLevelBoard}\" , \"prefs_voting\":\"{prefs_votingBoard}\", \"prefs_invitations\":\"{prefs_invitationsBoard}\", \"prefs_background\":\"{prefs_backgroundBoard}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PostProjectSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
           IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            //Instance var
            var nameResponse= jsonObject.SelectToken("name").ToString();
            var desResponse = jsonObject.SelectToken("desc").ToString();
            var prefs_permissionLevelResponse = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            var prefs_votingResponse = jsonObject.SelectToken("prefs.voting").ToString();
            var prefs_invitationsResponse = jsonObject.SelectToken("prefs.invitations").ToString();
            var prefs_backgroundResponse = jsonObject.SelectToken("prefs.background").ToString();

            // Assert body
            Assert.AreEqual(nameBoard, nameResponse);
            Assert.AreEqual(desBoard, desResponse);
            Assert.AreEqual(prefs_permissionLevelBoard, prefs_permissionLevelResponse);
            Assert.AreEqual(prefs_votingBoard, prefs_votingResponse);
            Assert.AreEqual(prefs_invitationsBoard, prefs_invitationsResponse);
            Assert.AreEqual(prefs_backgroundBoard, prefs_backgroundResponse);

        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest($"boards/{id}" );
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }


        }
    }
}
