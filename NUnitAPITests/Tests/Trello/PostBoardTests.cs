using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using NUnitAPITests.Client;

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
            var request = new TrelloRequest("boards");
            var requestBody = @"{
                ""name"":""myFirstTestBoard"", 
                ""desc"":""thisIsAdescription"",
                ""closed"":""false"",
                ""pinned"":""false""
            }";
            
            request.GetRequest().AddJsonBody(requestBody);

            // Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);

            // Parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            // Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/postBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            // Assert json schema
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));
        }

        [TearDown]
        public void DeleteBoards()
        {
            foreach (var id in ids)
            {                
                var request = new TrelloRequest("boards/" + id.ToString());
                var delete = RequestManager.Delete(TrelloClient.GetInstance(), request);
                Assert.AreEqual(200, (int)delete.StatusCode);
            }

        }
    }
}
