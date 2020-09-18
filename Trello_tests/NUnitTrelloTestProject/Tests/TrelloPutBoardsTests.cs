using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitTrelloTestProject.Client;
using NUnitTrelloTestProject.Config;
using RestSharp;

namespace NUnitTrelloTestProject.Tests
{
    public class TrelloPutBoardsTests
    {
        private List<string> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<string>();
        }

        [Test]
        public void PutBoardTest()
        {
            // Create the board
            var request = new TrelloRequest("boards");
            request.GetRequest().AddJsonBody("{\"name\":\"Board test01\"}");
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);
            Assert.AreEqual(200, (int)response.StatusCode);
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
            string id = jsonObject.SelectToken("id").ToString();
            var jsonSchemaString = File.ReadAllText("Schemas/PostSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));

            // Modify the name board
            var request02 = new TrelloRequest("boards/" + id);
            request02.GetRequest().AddJsonBody("{\"name\":\"name modified\"}");
            var response02 = RequestManager.Put(TrelloClient.GetInstance(), request02);
            Assert.AreEqual(200, (int)response02.StatusCode);
            var jsonObject02 = JObject.Parse(response02.Content);
            var jsonSchemaString02 = File.ReadAllText("Schemas/PutSchema.json");
            var jsonSchema02 = JSchema.Parse(jsonSchemaString02);
            IList<string> schemaErrors02 = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchema02, out schemaErrors02));
        }

        [TearDown]
        public void deleteBoards()
        {
            //Delete the board created
            foreach (var id in ids)
            {
                var request = new TrelloRequest("boards/" + id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }

        }
    }
}

