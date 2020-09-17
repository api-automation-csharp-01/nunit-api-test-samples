using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITest.Client;
using NUnitAPITests.Client;
using System.Collections.Generic;
using System.IO;

namespace NUnitAPITests.Tests.Trello
{
    class PostBoardsTest
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
            var expectedName = "API AUT board 8";
            var expectedDescription = "This is a board created.";
            var expectedKeepFromSource = "cards";
            var expectedPermissionLevel = "public";
            var expectedVoting = "members";
            var expectedBackground = "green";
            var request = new TrelloRequest("boards");
            var requestBody = $"{{\"name\": \"{expectedName}\", \"desc\": \"{expectedDescription}\", \"keepFromSource\": \"{expectedKeepFromSource}\", " +
                $"\"prefs_permissionLevel\": \"{expectedPermissionLevel}\", \"prefs_voting\": \"{expectedVoting}\", \"prefs_comments\": \"disabled\", " +
                $"\"prefs_cardCovers\": \"false\", \"prefs_background\": \"{expectedBackground}\"}}";
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
            var actualPermissionLevel = jsonObject.SelectToken("prefs.permissionLevel").ToString();
            Assert.AreEqual(expectedPermissionLevel, actualPermissionLevel);
            var actualVoting = jsonObject.SelectToken("prefs.voting").ToString();
            Assert.AreEqual(expectedVoting, actualVoting);
            var actualPrefsBackground = jsonObject.SelectToken("prefs.background").ToString();
            Assert.AreEqual(expectedBackground, actualPrefsBackground);
        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new PivotalRequest("boards/{id}" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
