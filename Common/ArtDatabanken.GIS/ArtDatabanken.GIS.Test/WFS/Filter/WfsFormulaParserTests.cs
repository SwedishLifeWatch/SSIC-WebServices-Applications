using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.WFS.Filter;
using ArtDatabanken.GIS.WFS.Filter.Formula;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.WFS.Filter
{
    [TestClass]
    public class WfsFormulaParserTests
    {

        [TestMethod]
        public void Parse_PropertyIsEqualToFormula_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>";
         
            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_PropertyIsGreaterThan_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><PropertyIsGreaterThan><PropertyName>LänSKOD</PropertyName><PropertyName>LänSBOKSTA</PropertyName></PropertyIsGreaterThan></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_PropertyIsGreaterThanOrEqualTo_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<PropertyIsGreaterThanOrEqualTo><PropertyName>LänSBOKSTA</PropertyName><Literal>25</Literal></PropertyIsGreaterThanOrEqualTo>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

     
        [TestMethod]
        public void Parse_PropertyIsLessThanOrEqualTo_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<PropertyIsLessThanOrEqualTo><PropertyName>LänSBOKSTA</PropertyName><Literal>25</Literal></PropertyIsLessThanOrEqualTo>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_PropertyIsLessThan_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<PropertyIsLessThan><PropertyName>LänSBOKSTA</PropertyName><Literal>25</Literal></PropertyIsLessThan>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_PropertyIsLike_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><PropertyIsLike wildcard='*' singleChar='.' escape='!'><PropertyName>NAMN</PropertyName><Literal>*land</Literal></PropertyIsLike></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_PropertyIsNotEqualTo_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><PropertyIsNotEqualTo><PropertyName>NAMN</PropertyName><Literal>dalarna</Literal></PropertyIsNotEqualTo></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }
   
        [TestMethod]
        public void Parse_PropertyIsNull_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><PropertyIsNull><PropertyName>NAMN</PropertyName></PropertyIsNull></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_OrFormula_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }

        [TestMethod]
        public void Parse_AndFormula_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><And><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></And></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }               

        [TestMethod]
        public void Parse_NotFormula_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><Not><PropertyIsLessThan><PropertyName>LänSBOKSTA</PropertyName><Literal>K</Literal></PropertyIsLessThan></Not></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }                       

        [TestMethod]
        public void Parse_ComplexFormula_FormulaIsParsedSuccessfully()
        {
            string strFilter = "<Filter><Or><Or><And><Not><PropertyIsLessThan><PropertyName>LänSBOKSTA</PropertyName><Literal>K</Literal></PropertyIsLessThan></Not><PropertyIsNotEqualTo><PropertyName>NAMN</PropertyName><Literal>Småland</Literal></PropertyIsNotEqualTo></And><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>25</Literal></PropertyIsEqualTo></Or><PropertyIsEqualTo><PropertyName>NAMN</PropertyName><Literal>Dalarna</Literal></PropertyIsEqualTo></Or></Filter>";

            WfsFormulaParser parser = new WfsFormulaParser();
            FormulaOperation formulaOperation = parser.Parse(strFilter);
            string strWfsRepresentation = formulaOperation.WFSRepresentation();
            strFilter = strFilter.Replace("<Filter>", "").Replace("</Filter>", "");

            Assert.IsNotNull(formulaOperation);
            Assert.AreEqual(strFilter, strWfsRepresentation);
        }                       

        
        


    }
}
