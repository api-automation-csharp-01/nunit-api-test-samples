
using System.Collections.Generic;

using NUnit.Framework;
using NUnitAPITests.Config;
using RestSharp;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnitAPITests.Client;
using System.Dynamic;

namespace NUnitAPITests.Tests.Trello
{
   
    public class PostBoardsTests
    {
        private List<string> ids;
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
        }
        [Test]
        public void PostBoardTest()
        {
            // Create Board
            var request = new TrelloRequest("boards");
            request.GetRequest().AddJsonBody("{\"name\": \"RestApiBoard\"}");
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);

            //validate Response
            Assert.AreEqual(200, (int)response.StatusCode);

            //parse response to json object
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());

            //Instantiate json schema object
            var jsonSchemaString = File.ReadAllText("Schemas/Trello/PostBoardSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();

            //Assertion
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

        }
        [TearDown]
        public void DeleteBoards()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest($"boards/{id}");
                //The board need to be Closed to be Deleted
                request.GetRequest().AddJsonBody("{\"closed\": \"true\"}");
                RequestManager.Put(TrelloClient.GetInstance(), request);

                //Delete the Board
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }

    }
}
