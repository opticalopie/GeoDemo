using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeocachingExample;
using GeocachingExample.Controllers;
using GeocachingExample.Models;
using System.Web.Http.Hosting;
using GeocachingExample.Filters;

namespace GeocachingExample.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            ValuesController controller = createController();

                
            // Act
            IEnumerable<GeocacheItem> result = controller.Get();

                
            //Assert

            //Not null.
            Assert.IsNotNull(result, "Returned null.");
                
            //Has at least 1 record.
            Assert.IsTrue((0 < result.Count()), "The count should be greater than 0.");
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            ValuesController controller = createController();
            IEnumerable<GeocacheItem> collection = controller.Get();

            // Act
            for (int inx = 0; inx < collection.Count() && inx < 5; inx++)
            {
                GeocacheItem testItem = collection.ElementAt(inx);
                GeocacheItem result = controller.Get(testItem.GeocacheID.Value);

                // Assert
                Assert.IsNotNull(result, "Object was returned null.");

                //Check get signle is working.
                Assert.IsTrue(testItem == result, "Did not return expected value.");
            }

            //Act
            Assert.IsNull(controller.Get(0), "Should have returned null.");
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            ValuesController controller = createController();
                        
            GeocacheItem item = new GeocacheItem()
            {
                Name = DateTime.Now.ToString("yyMMddhhmmss"),
                Latitude = 123.456M,
                Longitude = 123.456M
            };

            // Act
            int geoID;
            HttpResponseMessage result = controller.Post(item);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK, "Returned something other than status OK(200).");
            Assert.IsTrue(result.TryGetContentValue<int>(out geoID), "No ID value returned.");
            Assert.IsTrue(geoID > 0, "The returned ID is not valid.");

            //Add the same item again.
            result = controller.Post(item);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.NotAcceptable, "Added more than 1 item with the same name.");
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            ValuesController controller = createController();
            IEnumerable<GeocacheItem> collection = controller.Get();

            HttpResponseMessage result = null;
            // Act
            for (int inx = 0; inx < collection.Count() && inx < 2; inx++)
            {
                GeocacheItem testItem = collection.ElementAt(inx);
                result = controller.Delete(testItem.GeocacheID.Value);

                // Assert
                Assert.IsNotNull(result, "Object was returned null.");

                //Check get signle is working.
                Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK, "Returned something other than status OK(200).");
            }
        }



        private ValuesController createController()
        {
            ValuesController controller = new ValuesController();
            controller.Request = new HttpRequestMessage() { RequestUri = new Uri("http://localhost/api/") };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            controller.Configuration = new HttpConfiguration();
            controller.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext();
            
            return controller;
        }
    }
}
