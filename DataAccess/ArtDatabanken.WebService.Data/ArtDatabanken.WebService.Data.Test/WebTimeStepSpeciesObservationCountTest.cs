using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Tests all properties in WebTimeStepSpeciesObservationCount
    /// </summary>
    [TestClass]
    public class WebTimeStepSpeciesObservationCountTest : WebDomainTestBase<WebTimeStepSpeciesObservationCount>
    {
        [TestMethod]
        public void Constructor()
        {
            WebTimeStepSpeciesObservationCount webTimeStepSpeciesObservationCount = new WebTimeStepSpeciesObservationCount();
            Assert.IsNotNull(webTimeStepSpeciesObservationCount);
        }

        [TestMethod]
        public void IsDateSpecified()
        {
            bool testValue = true;
            GetObject(true).IsDateSpecified = testValue;
            Assert.AreEqual(GetObject().IsDateSpecified, testValue);
        }

        [TestMethod]
        public void Date()
        {
            DateTime testValue = DateTime.Now;
            GetObject(true).Date = testValue;
            Assert.AreEqual(GetObject().Date, testValue);
        }

        [TestMethod]
        public void Name()
        {
            String testValue = "Name";
            GetObject(true).Name = testValue;
            Assert.AreEqual(GetObject().Name, testValue);
        }

        [TestMethod]
        public void Type()
        {
            const ArtDatabanken.Data.Periodicity testValue = ArtDatabanken.Data.Periodicity.Yearly;
            GetObject(true).Periodicity = testValue;
            Assert.AreEqual(GetObject().Periodicity, testValue);
        }

        [TestMethod]
        public void ObservationCount()
        {
            const long testValue = long.MaxValue;
            GetObject(true).Count = testValue;
            Assert.AreEqual(GetObject().Count, testValue);
        }

        [TestMethod]
        public void Id()
        {
            const Int32 testValue = 1;
            GetObject(true).Id = testValue;
            Assert.AreEqual(GetObject().Id, testValue);
        }
    }
}
