using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.CoordinateConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
{
    [TestClass]
    public class CoordinateConversionManagerTests
    {

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetConvertedPoint_FromWgs84ToGoogleMercator_ReturnConvertedPoint()
        {            
            CoordinateConversionManager conversionManager = new CoordinateConversionManager();

            IPoint point = new Point(10,15);
            ICoordinateSystem fromCoordinateSystem = new CoordinateSystem(CoordinateSystemId.WGS84);
            ICoordinateSystem toCoordinateSystem = new CoordinateSystem(CoordinateSystemId.GoogleMercator);
            IPoint convertedPoint = conversionManager.GetConvertedPoint(point, fromCoordinateSystem, toCoordinateSystem);
            Assert.AreEqual(1113194.9079327357, convertedPoint.X, 0.001);
            Assert.AreEqual(1689200.1396078924, convertedPoint.Y, 0.001);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetConvertedPoint_FromWgs84ToSweref99_ReturnConvertedPoint()
        {            
            CoordinateConversionManager conversionManager = new CoordinateConversionManager();

            IPoint point = new Point(10,15);
            ICoordinateSystem fromCoordinateSystem = new CoordinateSystem(CoordinateSystemId.WGS84);
            ICoordinateSystem toCoordinateSystem = new CoordinateSystem(CoordinateSystemId.SWEREF99_TM);
            IPoint convertedPoint = conversionManager.GetConvertedPoint(point, fromCoordinateSystem, toCoordinateSystem);
            Assert.AreEqual(-38133.05646984861, convertedPoint.X, 0.001);
            Assert.AreEqual(1664414.4300254737, convertedPoint.Y, 0.001);
        }
        

    }
}
