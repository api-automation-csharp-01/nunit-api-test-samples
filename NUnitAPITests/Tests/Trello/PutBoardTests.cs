using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITests.Client;
using System.Collections.Generic;
using System.IO;

namespace NUnitAPITests.Tests.Trello
{
    public class PutBoardTests
    {
        private List<string> boardsIds;

        [SetUp]
        public void Setup()
        {
            boardsIds = new List<string>();

            //Create Board
            var request = new TrelloRequest("boards");

            //JsonBody
            var jsonRequest = new
            {
                name = "Automation Trello Put Request",                
                desc = "Board Description",
                prefs_background = "blue"            
            };

            //Requests
            request.GetRequest().AddJsonBody(jsonRequest);
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            var jsonObject = JObject.Parse(response.Content);

            //Save Project ID
            boardsIds.Add(jsonObject.SelectToken("id").ToString());
        }

        [Test]
        public void PutBoardTest()
        {
            // Build request
            var request = new TrelloRequest("boards/" + boardsIds[0]);
            var expectedName = "Automation Trello Request Updated";

            //JsonBody
            var jsonRequest = new
            {
                name = expectedName,
            };

            //Add Body requests
            request.GetRequest().AddJsonBody(jsonRequest);
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);

            Assert.AreEqual(200, (int)response.StatusCode);

            var jsonObject = JObject.Parse(response.Content);

            //Save Project ID
            boardsIds.Add(jsonObject.SelectToken("id").ToString());

            //Validate actualOutputs Fields            
            Assert.AreEqual(expectedName, jsonObject.SelectToken("name").ToString());

            //Schema Validation
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PutBoardSchema.json");
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
