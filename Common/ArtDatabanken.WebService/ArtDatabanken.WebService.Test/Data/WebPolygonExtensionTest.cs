using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebPolygonExtensionTest
    {
        [TestMethod]
        public void GetBoundingBox()
        {
            WebBoundingBox boundingBox;
            WebLinearRing linearRing;
            WebPolygon polygon;

            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(2, 2));
            linearRing.Points.Add(new WebPoint(4, 1));
            linearRing.Points.Add(new WebPoint(5, 4));
            linearRing.Points.Add(new WebPoint(2, 2));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            boundingBox = polygon.GetBoundingBox();
            Assert.IsNotNull(boundingBox);
            Assert.AreEqual(2, boundingBox.Min.X);
            Assert.AreEqual(5, boundingBox.Max.X);
            Assert.AreEqual(1, boundingBox.Min.Y);
            Assert.AreEqual(4, boundingBox.Max.Y);
        }

        [TestMethod]
        public void GetGeometry()
        {
            SqlGeometry polygonGeometry;
            WebLinearRing linearRing;
            WebPolygon polygon;

            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(2, 2));
            linearRing.Points.Add(new WebPoint(4, 1));
            linearRing.Points.Add(new WebPoint(5, 4));
            linearRing.Points.Add(new WebPoint(2, 2));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            polygonGeometry = polygon.GetGeometry();
            Assert.IsNotNull(polygonGeometry);
        }
    }
}
