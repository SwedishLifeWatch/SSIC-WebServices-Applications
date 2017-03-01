using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.GIS.WFS.Filter.Formula;

namespace ArtDatabanken.GIS.Test.WFS
{
    /// <summary>
    /// Summary description for Filter
    /// </summary>
    [TestClass]
    public class FilterTests
    {
        public FilterTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        #endregion


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Build_WFS_Filters()
        {
            var wfsFilter = new WFSFilter();
            Assert.IsFalse(wfsFilter.CanUndo);
            Assert.IsFalse(wfsFilter.CanAddLogicalOperation);
            Assert.IsTrue(wfsFilter.CanAddComparisionOperation);

            var taxonIdEqualToOperation = new BinaryComparisionOperation(
                new FieldValue("TaxonId"),
                new ConstantValue("4000107"),
                WFSBinaryComparisionOperator.EqualTo);
            wfsFilter.AddComparisionOperation(taxonIdEqualToOperation);
            Assert.AreEqual("'TaxonId' = 4000107", wfsFilter.FormulaRepresentation());
            Assert.IsTrue(wfsFilter.CanAddLogicalOperation);
            Assert.IsFalse(wfsFilter.CanAddComparisionOperation);
            Assert.IsTrue(wfsFilter.CanUndo);


            var observationCountGreaterThanOperation = new BinaryComparisionOperation(
                new FieldValue("ObservationCount"),
                new ConstantValue("100"),
                WFSBinaryComparisionOperator.GreaterThan);
            wfsFilter.AddLogicalOperation(WFSLogicalOperator.And, observationCountGreaterThanOperation);
            Assert.AreEqual("('TaxonId' = 4000107 And 'ObservationCount' > 100)", wfsFilter.FormulaRepresentation());
            Assert.IsTrue(wfsFilter.CanAddLogicalOperation);
            Assert.IsFalse(wfsFilter.CanAddComparisionOperation);
            Assert.IsTrue(wfsFilter.CanUndo);
            
            wfsFilter.AddUnaryLogicalOperation(WFSUnaryLogicalOperator.Not);
            Assert.AreEqual("Not(('TaxonId' = 4000107 And 'ObservationCount' > 100))", wfsFilter.FormulaRepresentation());

            wfsFilter.Undo();
            Assert.AreEqual("('TaxonId' = 4000107 And 'ObservationCount' > 100)", wfsFilter.FormulaRepresentation());
            wfsFilter.Undo();
            Assert.AreEqual("'TaxonId' = 4000107", wfsFilter.FormulaRepresentation());
            Assert.IsTrue(wfsFilter.CanAddLogicalOperation);
            Assert.IsFalse(wfsFilter.CanAddComparisionOperation);
            Assert.IsTrue(wfsFilter.CanUndo);
            wfsFilter.Undo();
            Assert.AreEqual("", wfsFilter.FormulaRepresentation());
            Assert.IsFalse(wfsFilter.CanUndo);
            Assert.IsFalse(wfsFilter.CanAddLogicalOperation);
            Assert.IsTrue(wfsFilter.CanAddComparisionOperation);            
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Test_WFS_Filters()
        {
            WFSFilter wfsFilter;
            string strWFSRepresentation;
            string strFormulaRepresentation;
            
            
            var equalToOperation = new BinaryComparisionOperation(
                new FieldValue("TaxonId"), 
                new ConstantValue("4000107"), 
                WFSBinaryComparisionOperator.EqualTo);
            wfsFilter = new WFSFilter();
            wfsFilter.Formula = equalToOperation;
            strWFSRepresentation = wfsFilter.WFSRepresentation();
            strFormulaRepresentation = wfsFilter.FormulaRepresentation();
            Assert.AreEqual(
                "<Filter><PropertyIsEqualTo><PropertyName>TaxonId</PropertyName><Literal>4000107</Literal></PropertyIsEqualTo></Filter>", 
                strWFSRepresentation);


            var greaterThanOperation = new BinaryComparisionOperation(
                new FieldValue("ObservationCount"),
                new ConstantValue("100"),
                WFSBinaryComparisionOperator.GreaterThan);
            var andOperation = new BinaryLogicalOperation(equalToOperation, greaterThanOperation, WFSBinaryLogicalOperator.And);
            wfsFilter = new WFSFilter();
            wfsFilter.Formula = andOperation;
            strWFSRepresentation = wfsFilter.WFSRepresentation();
            strFormulaRepresentation = wfsFilter.FormulaRepresentation();
            Assert.AreEqual(
                "<Filter><And><PropertyIsEqualTo><PropertyName>TaxonId</PropertyName><Literal>4000107</Literal></PropertyIsEqualTo><PropertyIsGreaterThan><PropertyName>ObservationCount</PropertyName><Literal>100</Literal></PropertyIsGreaterThan></And></Filter>",
                strWFSRepresentation);


            var orOperation = new BinaryLogicalOperation(
                andOperation, 
                new UnaryComparisionOperation(new FieldValue("Author"), WFSUnaryComparisionOperator.IsNull), 
                WFSBinaryLogicalOperator.Or);
            wfsFilter = new WFSFilter();
            wfsFilter.Formula = orOperation;
            strWFSRepresentation = wfsFilter.WFSRepresentation();
            strFormulaRepresentation = wfsFilter.FormulaRepresentation();
           

        }


        [TestMethod]
        public void Test_BBox()
        {
            string str = null;
            str = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedishCounties&OutputFormat=json&srsname=EPSG:900913";
            str = MakeRequest(str);
            str = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedishCounties&OutputFormat=json&srsname=EPSG:900913&filter=<Filter><BBOX><PropertyName>the_geom</PropertyName><Box%20srsName='EPSG:900913'><coordinates>1517579.5325132,7742185.5358054%201675345.5588718,7838801.9395444</coordinates></Box></BBOX></Filter>";
            str = MakeRequest(str);


            str = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedishCounties&OutputFormat=json&srsname=EPSG:900913&filter=<ogc:Filter%20xmlns:ogc=\"http://www.opengis.net/ogc\"%20xmlns:gml=\"http://www.opengis.net/gml\"%20xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"%20xsi:schemaLocation=\"http://www.opengis.net/ogc/filter/1.0.0/filter.xsd%20http://www.opengis.net/gml/2.1/geometry.xsd\"><ogc:BBOX><ogc:PropertyName>the_geom</ogc:PropertyName><gml:Box%20xmlns=\"http://www.opengis.net/cite/spatialTestSuite\"%20srsName=\"EPSG:4326\"><gml:coordinates>-122.087210506228,%2037.208402,%20-121.813389493772,%2037.383473</gml:coordinates></gml:Box></ogc:BBOX></ogc:Filter>";
            str = MakeRequest(str);

        }


        private string MakeRequest(string requestUrl)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string strXml = wc.DownloadString(requestUrl);
                return strXml;
            }
        }


    }
}
