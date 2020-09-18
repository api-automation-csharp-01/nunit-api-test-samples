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

namespace TrelloPostBoardsTests.tests
{
    public class Tests
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
            var request = new TrelloRequest("boards");
            request.GetRequest().AddJsonBody("{\"name\":\"Board test01\"}");
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);
            Assert.AreEqual(200, (int)response.StatusCode);
            var jsonObject = JObject.Parse(response.Content);
            ids.Add(jsonObject.SelectToken("id").ToString());
            var jsonSchemaString = File.ReadAllText("Schemas/PostSchema.json");
            var jsonSchema = JSchema.Parse(jsonSchemaString);
            IList<string> schemaErrors = new List<string>();
            Assert.IsTrue(jsonObject.IsValid(jsonSchema, out schemaErrors));
                     
        }

        [TearDown]
        public void deleteBoards()
        {
           //Delete the board created
            foreach (var id  in ids) 
            {
                var request = new TrelloRequest("boards/"+id);
                RequestManager.Delete(TrelloClient.GetInstance(), request);
            }
           
        }
    }
}