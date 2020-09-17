using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITests.Client;
using System.Collections.Generic;
using System.IO;

namespace NUnitAPITests.Tests.Trello
{
    public class PostBoardTests
    {
        private List<string> boardsIds;

        [SetUp]
        public void Setup()
        {
            boardsIds = new List<string>();
        }

        [Test]
        public void PostBoardTest()
        {
            // Build request
            var request = new TrelloRequest("boards");
            var expectedName = "Automation Trello Request";
            var expectedDefaultLabels = false;
            var expectedDesc = "Board Description";
            var expectedPrefsBackground = "lime";
            var expectedPrefsCardAging = "pirate";

            //JsonBody
            var jsonRequest = new
            {
                name = expectedName,
                defaultLabels = expectedDefaultLabels,
                desc = expectedDesc,
                prefs_background = expectedPrefsBackground,
                prefs_cardAging = expectedPrefsCardAging
            };

            //Add Body requests
            request.GetRequest().AddJsonBody(jsonRequest);
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            Assert.AreEqual(200, (int)response.StatusCode);

            var jsonObject = JObject.Parse(response.Content);

            //Save Project ID
            boardsIds.Add(jsonObject.SelectToken("id").ToString());

            //Validate actualOutputs Fields            
            Assert.AreEqual(expectedName, jsonObject.SelectToken("name").ToString());
            Assert.AreEqual(expectedDesc, jsonObject.SelectToken("desc").ToString());
            Assert.AreEqual(expectedPrefsBackground, jsonObject.SelectToken("prefs.background").ToString());
            Assert.AreEqual(expectedPrefsCardAging, jsonObject.SelectToken("prefs.cardAging").ToString());

            //Schema Validation
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PostBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);

            IList<string> schemaErrors = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));
        }

        [TearDown]
        public void DeleteBoards()
        {
            foreach (var id in boardsIds)
            {
                var request = new TrelloRequest("boards/" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
