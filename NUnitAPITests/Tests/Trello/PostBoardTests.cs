using NUnit.Framework;

using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using NUnitAPITests.Client;
using NUnitAPITests.Config;

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
            //Build request
            var request = new TrelloRequest("boards/?name={name}&defaultLabels=true&defaultLists=true&desc={desc}&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&key={apikey}&token={token}");
            request.GetRequest().AddUrlSegment("name", "New Board 2020");
            request.GetRequest().AddUrlSegment("desc", "My Board 2020");
            request.GetRequest().AddUrlSegment("apikey", EnvironmentConfig.GetInstance().GetKey(ApisEnum.Trello));
            request.GetRequest().AddUrlSegment("token", EnvironmentConfig.GetInstance().GetToken(ApisEnum.Trello));
            //Send request
            var response = RequestManager.Post(TrelloClient.GetInstance(), request);
            //Validate status code
            Assert.AreEqual(200, (int)response.StatusCode);
            //Validate jsonschema
            var jsonSchemaString = JSchema.Parse(File.ReadAllText("Schemas/Trello/PostBoardSchema.json"));
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

