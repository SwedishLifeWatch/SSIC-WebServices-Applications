using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebCoordinateSystemExtensionTest
    {
        private WebCoordinateSystem _coordinateSystem;

        public WebCoordinateSystemExtensionTest()
        {
            _coordinateSystem = null;
        }

        [TestMethod]
        public void CheckData()
        {
            GetCoordinateSystem(true);
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                if (coordinateSystemId != CoordinateSystemId.None)
                {
                    GetCoordinateSystem().Id = coordinateSystemId;
                    GetCoordinateSystem().CheckData();
                }
            }
            GetCoordinateSystem().Id = CoordinateSystemId.None;
            GetCoordinateSystem().WKT = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
            GetCoordinateSystem().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataEmptyWktError()
        {
            GetCoordinateSystem().Id = CoordinateSystemId.None;
            GetCoordinateSystem().WKT = " ";
            GetCoordinateSystem().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataFormatWktError()
        {
            GetCoordinateSystem().Id = CoordinateSystemId.None;
            GetCoordinateSystem().WKT = "Hej hopp i lingonskogen";
            GetCoordinateSystem().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckDataNullError()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = null;
            coordinateSystem.CheckData();
        }

        public WebCoordinateSystem GetCoordinateSystem()
        {
            return GetCoordinateSystem(false);
        }

        public WebCoordinateSystem GetCoordinateSystem(Boolean refresh)
        {
            if (_coordinateSystem.IsNull() || refresh)
            {
                _coordinateSystem = new WebCoordinateSystem();
            }
            return _coordinateSystem;
        }

        [TestMethod]
        public void GetWkt()
        {
            String wkt;

            wkt = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
            GetCoordinateSystem(true).Id = CoordinateSystemId.None;
            GetCoordinateSystem().WKT = wkt;
            Assert.AreEqual(wkt, GetCoordinateSystem().GetWkt());

            GetCoordinateSystem().WKT = null;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                GetCoordinateSystem().Id = coordinateSystemId;
                if (coordinateSystemId == CoordinateSystemId.None)
                {
                    Assert.IsTrue(GetCoordinateSystem().GetWkt().IsEmpty());
                }
                else
                {
                    Assert.IsTrue(GetCoordinateSystem().GetWkt().IsNotEmpty());
                }
            }
        }

        [TestMethod]
        public void WebToString()
        {
            String coordinateSystemString;

            GetCoordinateSystem(true);
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                if (coordinateSystemId != CoordinateSystemId.None)
                {
                    GetCoordinateSystem().Id = coordinateSystemId;
                    coordinateSystemString = GetCoordinateSystem().WebToString();
                    Assert.IsTrue(coordinateSystemString.IsNotEmpty());
                }
            }

            GetCoordinateSystem().Id = CoordinateSystemId.None;
            GetCoordinateSystem().WKT = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
            coordinateSystemString = GetCoordinateSystem().WebToString();
            Assert.IsTrue(coordinateSystemString.IsNotEmpty());
        }
    }
}
