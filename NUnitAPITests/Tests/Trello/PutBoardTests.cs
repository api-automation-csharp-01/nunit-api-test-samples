using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using NUnitAPITests.Client;

namespace NUnitAPITests.Tests.Trello
{
    class PutBoardTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
            //Build request
            var request = new TrelloRequest("boards");
            var requestBody = "{\"name\": \"My new board\", \"desc\": \"new description\"}";
            request.GetRequest().AddJsonBody(requestBody);
            //Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            // Obtain id            
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
        }

        [Test]
        public void PostBoardTest()
        {
            //Build request
            var request = new TrelloRequest("boards/{id}");
            foreach (var id in ids)
            {
                request.GetRequest().AddUrlSegment("id", id);
            }
            var requestBody = "{\"name\": \"Updated test\", \"desc\": \"Updated from framework\"}";
            request.GetRequest().AddJsonBody(requestBody);

            //Send request
            var response = RequestManager.Put(TrelloClient.GetInstance(), request);
            //Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);
            //Validate jsonschema
            var jsonSchemaString = JSchema.Parse(File.ReadAllText("Schemas/Trello/PutBoardSchema.json"));
            var jsonObject = JObject.Parse(response.Content);

            IList<string> schemaErrors = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchemaString, out schemaErrors));
            ids.Add(jsonObject.SelectToken("id").ToString());

        }
        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest("boards/" + id );

                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
