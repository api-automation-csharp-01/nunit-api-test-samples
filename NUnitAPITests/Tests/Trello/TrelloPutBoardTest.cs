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
    public class TrelloPutBoardTest
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();

            // create a board
            var request = new TrelloRequest(resource: "boards");
            var requestBody = $"{{\"name\": \"Board created\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send Request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
        }

        [Test]
        public void PutBoardTest()
        {
            // Variables to compare
            var expectedName = "Board Updated";
            var expectedDesc = "This board has been updated";
            var expectedIdOrg = "5f5801b5776a7a5da2f3561a";
            var expectedPrefsPermissionLevel = "org";
            var expectedPerfsInvitations = "admins";
            var expectedPrefsBackground = "green";

            // Build request
            var request = new TrelloRequest(resource: "boards/" + ids.Single());
            var requestBody = $"{{\"name\": \"{expectedName}\", \"desc\": \"{expectedDesc}\", \"idOrganization\": \"{expectedIdOrg}\", \"prefs/permissionLevel\": \"{expectedPrefsPermissionLevel}\", \"prefs/invitations\": \"{expectedPerfsInvitations}\", \"prefs/background\": \"{expectedPrefsBackground}\"}}";
            request.GetRequest().AddJsonBody(requestBody);

            // Send Request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PutBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            // Assert 
            var actualName = jsonObject.SelectToken("name").ToString();
            var actualDesc = jsonObject.SelectToken("desc").ToString();
            var actualIdOrg = jsonObject.SelectToken("idOrganization").ToString();
            var actualPrefsPermissionLevel = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            var actualPerfsInvitations = jsonObject.SelectToken("prefs.invitations").ToString();
            var actualPrefsBackground = jsonObject.SelectToken("prefs.background").ToString();

            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedDesc, actualDesc);
            Assert.AreEqual(expectedIdOrg, actualIdOrg);
            Assert.AreEqual(expectedPrefsPermissionLevel, actualPrefsPermissionLevel);
            Assert.AreEqual(expectedPerfsInvitations, actualPerfsInvitations);
            Assert.AreEqual(expectedPrefsBackground, actualPrefsBackground);

        }

        [TearDown]
        public void DeleteBoards()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest(resource: "boards/" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
