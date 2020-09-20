using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITests.Client;
using System.Collections.Generic;
using System.IO;

namespace NUnitAPITests.Tests.Trello
{
    public class UpdateTrelloBoardTests
    {
        private List<string> ids;
        [SetUp]
        public void Setup()
        {
            ids = new List<string>();

            var expectedName = "newTestBoard0004";
            var request = new TrelloRequest("/boards");
            var requestBody = @"{
                ""name"":""newTestBoard0004""
            }";

            request.GetRequest().AddJsonBody(requestBody);
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);
            Assert.AreEqual(200, (int)response.StatusCode);
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
            var jsonShemaString = File.ReadAllText("Schemas/Trello/PostBoardShema.json");
            var jsonSchema = JSchema.Parse(jsonShemaString);
            IList<string> schemaErrors = new List<string>();
            var isvalid = jsonObject.IsValid(jsonSchema, out schemaErrors);            
            Assert.IsTrue(isvalid);
            var actualName = jsonObject.SelectToken("name").ToString();
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void PostProjectTest()
        {
            var request = new TrelloRequest("/boards/" + ids[0].ToString());            
            var response = RequestManager.Get(TrelloClient.GetInstance(), request);
            Assert.AreEqual(200, (int)response.StatusCode);


            var expectedName = "updateTestAdri0004";
            var expectedDescrip = "updateDescription0004";
            var expectedClosed = "False";
            var expectedPinned = "False";

            //var request = new TrelloRequest("boards");
            var requestBody = @"{
                ""name"":""updateTestAdri0004"",
                ""desc"":""updateDescription0004"",
                ""closed"":""false"",
                ""pinned"":""false""
            }";

            request.GetRequest().AddJsonBody(requestBody);
            var response2 = RequestManager.Put(TrelloClient.GetInstance(), request);
            Assert.AreEqual(200, (int)response2.StatusCode);
            var jsonObject = JObject.Parse(response2.Content);

            //validation schema
            var jsonShemaString = File.ReadAllText("Schemas/Trello/UpdatePostBoardShema.json");
            var jsonSchema = JSchema.Parse(jsonShemaString);

            IList<string> schemaErrors = new List<string>();
            var isvalid = jsonObject.IsValid(jsonSchema, out schemaErrors);
            System.Console.WriteLine("isvalid:" + isvalid);
            Assert.IsTrue(isvalid);
            var actualName = jsonObject.SelectToken("name").ToString();
            Assert.AreEqual(expectedName, actualName);
            var actualDescrip = jsonObject.SelectToken("desc").ToString();
            Assert.AreEqual(expectedDescrip, actualDescrip);
            var actualClosed = jsonObject.SelectToken("closed").ToString();
            Assert.AreEqual(expectedClosed, actualClosed);
            var actualPinned = jsonObject.SelectToken("pinned").ToString();
            Assert.AreEqual(expectedPinned, actualPinned);
        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new TrelloRequest("/boards/" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);               
            }
        }
    }
}