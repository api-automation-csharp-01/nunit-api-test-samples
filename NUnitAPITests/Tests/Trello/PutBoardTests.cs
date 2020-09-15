using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using NUnitAPITests.Client;
using NUnitAPITests.Config;

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
            var request = new TrelloRequest("boards/?name={name}&desc={desc}&key={apikey}&token={token}");
            request.GetRequest().AddUrlSegment("name", "Updated by Framework");
            request.GetRequest().AddUrlSegment("desc", "Description updated");
            request.GetRequest().AddUrlSegment("apikey", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello));
            request.GetRequest().AddUrlSegment("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
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
            var request = new TrelloRequest("boards/{id}?name={name}&desc={desc}&key={apikey}&token={token}");
            foreach (var id in ids)
            {
                request.GetRequest().AddUrlSegment("id", id);
            }
            request.GetRequest().AddUrlSegment("name", "Updated by Framework");
            request.GetRequest().AddUrlSegment("desc", "Description updated");
            request.GetRequest().AddUrlSegment("apikey", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello));
            request.GetRequest().AddUrlSegment("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
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
                var request = new TrelloRequest("boards/" + id + "?key={apikey}&token={token}");
                request.GetRequest().AddUrlSegment("apikey", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello));
                request.GetRequest().AddUrlSegment("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
        }
    }
}
