using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnitAPITests.Client;
using NUnitAPITests.Models.Pivotal;

namespace NUnitAPITests.Tests.Pivotal
{
    public class PostProjectUsingModelTests
    {
        private List<int> ids;

        [SetUp]
        public void Setup()
        {
            ids = new List<int>();
        }

        [Test]
        public void PostProjectUsingModelTest()
        {
            // Build request
            var request = new PivotalRequest("projects");
            var projectRequest = new ProjectRequestModel();
            projectRequest.Name = "Test Automation";
            projectRequest.Public = false;
            projectRequest.IterationLength = 3;
            projectRequest.WeekStartDay = "Monday";
            var requestJson = JsonConvert.SerializeObject(projectRequest);

            request.GetRequest().AddJsonBody(requestJson);

            // Send request
            var response = RequestManager.Post(PivotalClient.GetInstance(), request);

            // Validate status code
            Assert.AreEqual(200, (int) response.StatusCode);

            // Deserialize
            var projectResponse = JsonConvert.DeserializeObject<ProjectResponseModel>(response.Content);
            ids.Add(projectResponse.Id);

            // Asserts
            Assert.AreEqual(projectRequest.Name, projectResponse.Name);
            Assert.AreEqual(projectRequest.Public, projectResponse.Public);
            Assert.AreEqual(projectRequest.IterationLength, projectResponse.IterationLength);
            Assert.AreEqual(projectRequest.WeekStartDay, projectResponse.WeekStartDay);
        }

        [TearDown]
        public void DeleteProjects()
        {
            foreach (var id in ids)
            {
                var request = new PivotalRequest("projects/" + id);
                RequestManager.Delete(PivotalClient.GetInstance(), request);
            }
        }
    }
}