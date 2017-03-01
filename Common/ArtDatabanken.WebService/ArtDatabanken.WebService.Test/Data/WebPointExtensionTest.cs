using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebPointExtensionTest : TestBase
    {
        [TestMethod]
        public void GetGeometry()
        {
            SqlGeometry geometryPoint;
            WebPoint point;

            point = new WebPoint();
            point.X = 1000;
            point.Y = 2000;
            geometryPoint = point.GetGeometry();
            Assert.IsNotNull(geometryPoint);
            Assert.AreEqual(SqlGeometryType.Point, geometryPoint.GetGeometryType());
            Assert.AreEqual(point.X, geometryPoint.STX);
            Assert.AreEqual(point.Y, geometryPoint.STY);

            point.IsZSpecified = true;
            point.Z = 3000;
            geometryPoint = point.GetGeometry();
            Assert.IsNotNull(geometryPoint);
            Assert.AreEqual(SqlGeometryType.Point, geometryPoint.GetGeometryType());
            Assert.AreEqual(point.X, geometryPoint.STX);
            Assert.AreEqual(point.Y, geometryPoint.STY);
            Assert.AreEqual(point.Z, (Double)geometryPoint.Z);

            point.IsMSpecified = true;
            point.M = 4000;
            geometryPoint = point.GetGeometry();
            Assert.IsNotNull(geometryPoint);
            Assert.AreEqual(SqlGeometryType.Point, geometryPoint.GetGeometryType());
            Assert.AreEqual(point.X, geometryPoint.STX);
            Assert.AreEqual(point.Y, geometryPoint.STY);
            Assert.AreEqual(point.Z, (Double)geometryPoint.Z);
            Assert.AreEqual(point.M, (Double)geometryPoint.M);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetGeometryNoZValueError()
        {
            SqlGeometry geometryPoint;
            WebPoint point;

            point = new WebPoint();
            point.X = 1000;
            point.Y = 2000;
            point.IsZSpecified = false;
            point.IsMSpecified = true;
            point.M = 4000;
            geometryPoint = point.GetGeometry();
            Assert.IsNotNull(geometryPoint);
            Assert.AreEqual(SqlGeometryType.Point, geometryPoint.GetGeometryType());
            Assert.AreEqual(point.X, geometryPoint.STX);
            Assert.AreEqual(point.Y, geometryPoint.STY);
            Assert.AreEqual(point.M, (Double)geometryPoint.M);
        }
    }
}
