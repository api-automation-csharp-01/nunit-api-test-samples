using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITests.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NUnitAPITests.Tests.Trello
{
    class PutTrelloTest
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
            // Values 
            var nameBoard = "Miauuuu";
            var desBoard = "12";

            var request = new TrelloRequest("boards");
            var requestBody = $"{{\"name\": \"{nameBoard}\", \"desc\": \"{desBoard}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
        }

        [Test]
        public void PutProjectTest()
        {
            // Values 
            var nameUpdate = "Update Miua";
            var desUpdate = "Update description";
            var prefs_permissionLevelUpdate = "private";
            var prefs_votingUpdate = "disabled";
            var prefs_invitationsUpdate = "members";
            var prefs_backgroundUpdate = "blue";

            // Build request
            var request = new TrelloRequest(resource:"boards/"+ids.Single());
            var requestBody = $"{{\"name\": \"{nameUpdate}\", \"desc\": \"{desUpdate}\",\"prefs_permissionLevel\":\"{prefs_permissionLevelUpdate}\" , \"prefs_voting\":\"{prefs_votingUpdate}\", \"prefs_invitations\":\"{prefs_invitationsUpdate}\", \"prefs_background\":\"{prefs_backgroundUpdate}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);
            
            //Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            //Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            //Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/UpdateProjectSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            //Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));


            //Instance var
            var nameResponse = jsonObject.SelectToken("name").ToString();
            var desResponse = jsonObject.SelectToken("desc").ToString();
            var prefs_permissionLevelResponse = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            var prefs_votingResponse = jsonObject.SelectToken("prefs.voting").ToString();
            var prefs_invitationsResponse = jsonObject.SelectToken("prefs.invitations").ToString();
            var prefs_backgroundResponse = jsonObject.SelectToken("prefs.background").ToString();

            // Assert body
            Assert.AreEqual(nameUpdate, nameResponse);
            Assert.AreEqual(desUpdate, desResponse);
            Assert.AreEqual(prefs_permissionLevelUpdate, prefs_permissionLevelResponse);
            Assert.AreEqual(prefs_votingUpdate, prefs_votingResponse);
            Assert.AreEqual(prefs_invitationsUpdate, prefs_invitationsResponse);
            Assert.AreEqual(prefs_backgroundUpdate, prefs_backgroundResponse);
        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest($"boards/{id}");
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }

        }

    }
}
