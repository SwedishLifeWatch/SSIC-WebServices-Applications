using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebCityInformation
    /// </summary>
    [TestClass]
    public class WebCityInformationTest
    {
        private WebCityInformation _cityInformation;

        public WebCityInformationTest()
        {
            _cityInformation = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebCityInformation cityInformation;

            cityInformation = new WebCityInformation();

            Assert.IsNotNull(cityInformation);
        }

        private WebCityInformation GetCityInformation()
        {
            if (_cityInformation.IsNull())
            {
                _cityInformation = new WebCityInformation();
            }
            return _cityInformation;
        }

        [TestMethod]
        public void CoordinateX()
        {

            GetCityInformation().CoordinateX = 1.0;
            Assert.AreEqual(GetCityInformation().CoordinateX, 1.0);

        }

        [TestMethod]
        public void CoordinateY()
        {

            GetCityInformation().CoordinateY = 1.0;
            Assert.AreEqual(GetCityInformation().CoordinateY, 1.0);

        }

        [TestMethod]
        public void County()
        {

            GetCityInformation().County = "Name";
            Assert.AreEqual(GetCityInformation().County, "Name");

        }

        [TestMethod]
        public void Municipality()
        {

            GetCityInformation().Municipality = "Name";
            Assert.AreEqual(GetCityInformation().Municipality, "Name");

        }

        [TestMethod]
        public void Name()
        {

            GetCityInformation().Name = "Name";
            Assert.AreEqual(GetCityInformation().Name, "Name");

        }

        [TestMethod]
        public void Parish()
        {

            GetCityInformation().Parish = "Name";
            Assert.AreEqual(GetCityInformation().Parish, "Name");

        }

        [TestMethod]
        public void Province()
        {

            GetCityInformation().Province = "Name";
            Assert.AreEqual(GetCityInformation().Province, "Name");

        }
    }
}
