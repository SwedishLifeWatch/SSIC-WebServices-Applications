using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.WFS.Filter;
using ArtDatabanken.GIS.WFS.Filter.Formula;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.WFS.Filter.Formula
{
    [TestClass]
    public class FormulaOperationTests
    {

        [TestMethod]
        public void WfsXmlRepresentation_PropertyIsEqualToFormula_CorrectXmlRepresentationIsCreated()
        {
            string strFilter = "<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsXmlRepresentation = formulaOperation.WfsXmlRepresentation();
            string expectedXmlString = "<ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>20</ogc:Literal></ogc:PropertyIsEqualTo>";

            Assert.IsNotNull(formulaOperation);

            Assert.AreEqual(expectedXmlString , strWfsXmlRepresentation);
        }


        [TestMethod]
        public void WfsXmlRepresentation_OrFormula_CorrectXmlRepresentationIsCreated()
        {
            string strFilter = "<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsXmlRepresentation = formulaOperation.WfsXmlRepresentation();
            string expectedXmlString = "<ogc:Or><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>5</ogc:Literal></ogc:PropertyIsEqualTo><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>6</ogc:Literal></ogc:PropertyIsEqualTo></ogc:Or>";

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(expectedXmlString , strWfsXmlRepresentation);
        }

        [TestMethod]
        public void WfsFilterWfsXmlRepresentation_OrFormula_CorrectXmlRepresentationIsCreated()
        {
            string strFilter = "<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            WFSFilter wfsFilter = new WFSFilter {Formula = formulaOperation};
            string strWfsXmlRepresentation =  wfsFilter.WfsXmlRepresentation();
            string expectedXmlString = "<ogc:Filter xmlns:ogc=\"http://www.opengis.net/ogc\"><ogc:Or><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>5</ogc:Literal></ogc:PropertyIsEqualTo><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>6</ogc:Literal></ogc:PropertyIsEqualTo></ogc:Or></ogc:Filter>";

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(expectedXmlString, strWfsXmlRepresentation);
        }


        [TestMethod]
        public void WfsFilterWfsXmlRepresentation_BoundingBox_CorrectXmlRepresentationIsCreated()
        {
            string strFilter = "<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            SpatialBoundingBox spatialBoundingBox = new SpatialBoundingBox(-180, -90, 180, 90, "EPSG:4326");
            var bboxOperation = new SpatialOperation(new SpatialFieldValue("the_geom"), spatialBoundingBox, WFSSpatialOperator.InsideBbox);
            formulaOperation = new BinaryLogicalOperation(formulaOperation, bboxOperation, WFSBinaryLogicalOperator.And);
            WFSFilter wfsFilter = new WFSFilter { Formula = formulaOperation };
            string strWfsXmlRepresentation = wfsFilter.WfsXmlRepresentation();
            string expectedXmlString = "<ogc:Filter xmlns:ogc=\"http://www.opengis.net/ogc\"><ogc:And><ogc:Or><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>5</ogc:Literal></ogc:PropertyIsEqualTo><ogc:PropertyIsEqualTo matchCase=\"true\"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>6</ogc:Literal></ogc:PropertyIsEqualTo></ogc:Or><ogc:BBOX><ogc:PropertyName>the_geom</ogc:PropertyName><gml:Envelope xmlns:gml=\"http://www.opengis.net/gml\" srsName=\"EPSG:4326\"><gml:lowerCorner>-180 -90</gml:lowerCorner><gml:upperCorner>180 90</gml:upperCorner></gml:Envelope></ogc:BBOX></ogc:And></ogc:Filter>";

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(expectedXmlString, strWfsXmlRepresentation);
        }

    }
}
