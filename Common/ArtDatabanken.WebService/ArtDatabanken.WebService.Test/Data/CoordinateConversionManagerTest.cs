using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class CoordinateConversionManagerTest
    {
        private CoordinateConversionManager _coordinateConversionManager;

        public CoordinateConversionManagerTest()
        {
            _coordinateConversionManager = null;
        }

        public CoordinateConversionManager GetCoordinateConversionManager()
        {
            return GetCoordinateConversionManager(false);
        }

        public CoordinateConversionManager GetCoordinateConversionManager(Boolean refresh)
        {
            if (_coordinateConversionManager.IsNull() || refresh)
            {
                _coordinateConversionManager = new CoordinateConversionManager();
            }
            return _coordinateConversionManager;
        }

        [TestMethod]
        public void GetConvertedPoint()
        {
            Double fromX, fromY;
            WebCoordinateSystem fromCoordinateSystem, toCoordinateSystem;
            WebPoint fromPoint, toPoint;

            fromX = 1727060.905;// 1644820;
            fromY = 7453389.762; // 6680450;
            fromPoint = new WebPoint(fromX, fromY);
            fromCoordinateSystem = new WebCoordinateSystem();
            fromCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                toCoordinateSystem = new WebCoordinateSystem();
                toCoordinateSystem.Id = coordinateSystemId;
                if (toCoordinateSystem.Id == CoordinateSystemId.None)
                {
                    toCoordinateSystem.WKT = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
                }
                toPoint = GetCoordinateConversionManager(true).GetConvertedPoint(fromPoint, fromCoordinateSystem, toCoordinateSystem);
                Assert.IsNotNull(toPoint);
                if (fromCoordinateSystem.GetWkt() == toCoordinateSystem.GetWkt())
                {
                    Assert.IsTrue(Math.Abs(fromX - toPoint.X) < 2);
                    Assert.IsTrue(Math.Abs(fromY - toPoint.Y) < 2);
                }
                else
                {
                    Assert.IsTrue(Math.Abs(fromX - toPoint.X) > 1);
                    Assert.IsTrue(Math.Abs(fromY - toPoint.Y) > 1);
                }
            }
        }

        [TestMethod]
        public void GetConvertedPoints()
        {
            Double fromX1, fromX2, fromY1, fromY2;
            List<WebPoint> fromPoints, toPoints;
            WebCoordinateSystem fromCoordinateSystem, toCoordinateSystem;

            fromX1 = 1644820;
            fromY1 = 6680450;
            fromX2 = 1634243;
            fromY2 = 6653434;
            fromPoints = new List<WebPoint>();
            fromPoints.Add(new WebPoint(fromX1, fromY1));
            fromPoints.Add(new WebPoint(fromX2, fromY2));
            fromCoordinateSystem = new WebCoordinateSystem();
            fromCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                toCoordinateSystem = new WebCoordinateSystem();
                toCoordinateSystem.Id = coordinateSystemId;
                if (toCoordinateSystem.Id == CoordinateSystemId.None)
                {
                    toCoordinateSystem.WKT = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
                }
                toPoints = GetCoordinateConversionManager(true).GetConvertedPoints(fromPoints, fromCoordinateSystem, toCoordinateSystem);
                Assert.IsTrue(toPoints.IsNotEmpty());
                Assert.AreEqual(fromPoints.Count, toPoints.Count);
                if (fromCoordinateSystem.GetWkt() == toCoordinateSystem.GetWkt())
                {
                    Assert.IsTrue(Math.Abs(fromX1 - toPoints[0].X) < 2);
                    Assert.IsTrue(Math.Abs(fromY1 - toPoints[0].Y) < 2);
                    Assert.IsTrue(Math.Abs(fromX2 - toPoints[1].X) < 2);
                    Assert.IsTrue(Math.Abs(fromY2 - toPoints[1].Y) < 2);
                }
                else
                {
                    Assert.IsTrue(Math.Abs(fromX1 - toPoints[0].X) > 1);
                    Assert.IsTrue(Math.Abs(fromY1 - toPoints[0].Y) > 1);
                    Assert.IsTrue(Math.Abs(fromX2 - toPoints[1].X) > 1);
                    Assert.IsTrue(Math.Abs(fromY2 - toPoints[1].Y) > 1);
                }
            }
        }
    }
}
