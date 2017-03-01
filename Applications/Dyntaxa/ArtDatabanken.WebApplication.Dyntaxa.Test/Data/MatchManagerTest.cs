//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;
//using System.Data;
//using System.IO;
//using System.Xml;
//using ArtDatabanken.IO;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
//    /// <summary>
//    /// Summary description for MatchManagerTest
//    /// </summary>
//    [TestClass]
//    public class MatchManagerTest
//    {
//        private MatchManager _manager = null;
//        private List<MatchItem> _matchList = null;
//        private string[] _nameList = null;
//        private DataTable _nameTable = null;
//        private MatchOptionsModel _options = null;

//        public MatchManagerTest()
//        {
//            this._manager = null;
//        }

//        private MatchManager GetManager(Boolean refresh)
//        {
//            if (_manager.IsNull() || refresh)
//            {
//                _manager = new MatchManager();
//            }
//            return _manager;
//        }

//        private string[] GetNameArray(Boolean refresh)
//        {
//            if (_nameList.IsNull() || refresh)
//            {
//                StringBuilder names = new StringBuilder();

//                names.Append(TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);
//                names.Append("\r\n");
//                names.Append("Hej hej");
//                names.Append("\r\n");
//                names.Append("\r\n");
//                names.Append(TestParameters.DEFAULT_TEST_TAXON_COMMON_NAME);
//                names.Append("\r\n");
//                names.Append(TestParameters.DEFAULT_TEST_TAXON_ID);
                

//                _nameList = names.ToString().Split('\n');

//            }
//            return _nameList;
//        }

//        private string[] GetIdArray(Boolean refresh)
//        {
//            if (_nameList.IsNull() || refresh)
//            {
//                StringBuilder names = new StringBuilder();

//                names.Append(TestParameters.DEFAULT_TEST_TAXON_ID);
//                names.Append("\r\n");
//                names.Append(TestParameters.DEFAULT_TEST_TAXON_ID);
//                names.Append("\r\n");
//                names.Append(TestParameters.DEFAULT_TEST_TAXON_ID);
//                _nameList = names.ToString().Split('\n');

//            }
//            return _nameList;
//        }

//        private DataTable GetNameTable(Boolean refresh)
//        {
//            if (_nameTable.IsNull() || refresh)
//            {
//                DataTable table = new DataTable();

//                DataColumn column = new DataColumn();
//                table.Columns.Add(column);
//                int id = 0;
//                table.Rows.Add();
//                table.Rows[id++][0] = TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME;
//                table.Rows.Add();
//                table.Rows[id++][0] = "Hej hej";
//                table.Rows.Add();
//                table.Rows[id++][0] = TestParameters.DEFAULT_TEST_TAXON_COMMON_NAME;
//                table.Rows.Add();
//                table.Rows[id++][0] = TestParameters.DEFAULT_TEST_TAXON_ID;
//                _nameTable = table;
//            }
//            return _nameTable;
//        }

//        private List<MatchItem> GetNameList(Boolean refresh)
//        {
//            if (_matchList.IsNull() || refresh)
//            {
//                _matchList = new List<MatchItem>();
//            }
//            return _matchList;
//        }

//        private MatchOptionsModel GetOptions(Boolean refresh)
//        {
//            if (_options.IsNull() || refresh)
//            {
//                _options = new MatchOptionsModel(TestParameters.CARABIDAE_TAXON_ID);
//            }
//            return _options;
//        }

//        [TestMethod]
//        public void GetMatchesTest()
//        {
//            List<MatchItem> matches = GetManager(true).GetMatches(GetNameArray(true), GetOptions(true));
//            Assert.AreEqual(matches.Count, GetNameArray(true).Length);
//            Assert.AreEqual(matches[0].RowNumber, 1);
//            Assert.AreEqual(matches[0].NameString, TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);

//            Assert.AreEqual(matches[0].TaxonId, TestParameters.DEFAULT_TEST_TAXON_ID);
//            Assert.AreEqual(matches[0].ScientificName, TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);
//            Assert.AreEqual(matches[0].CommonName, TestParameters.DEFAULT_TEST_TAXON_COMMON_NAME);
//            Assert.AreEqual(matches[0].Status, MatchStatus.Exact);
//            Assert.AreEqual(matches[1].Status, MatchStatus.NoMatch);
//        }

//        [TestMethod]
//        public void GetMatchesInExcelListTest()
//        {
//            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\test.xls";
//            ExcelFile file = new ExcelFile(GetNameTable(true), fileName);
//            MatchOptionsModel options = GetOptions(true);
//            options.IsFirstRowColumnName = true;
//            List<MatchItem> matches = GetManager(true).GetMatches(fileName, options);
//            Assert.AreEqual(matches.Count, GetNameTable(false).Rows.Count);
//            Assert.AreEqual(matches[0].RowNumber, 1);
//            Assert.AreEqual(matches[0].NameString, TestParameters.DEFAULT_TEST_TAXON_SCIENTIFIC_NAME);
//            File.Delete(fileName);
//        }

//        [TestMethod]
//        public void GetMatchesFromTaxonIdsTest()
//        {
//            MatchOptionsModel options = new MatchOptionsModel();
//            options.MatchToType = MatchTaxonToType.TaxonId;
//            List<MatchItem> matches = GetManager(true).GetMatches(GetIdArray(true), options);
//            Assert.AreEqual(matches[0].Status, MatchStatus.Exact);
//            Assert.AreEqual(GetIdArray(false).GetLength(0), matches.Count);
//        }

//        [TestMethod]
//        public void GetMatchesFromTaxonIdsWhenTaxonIdIsMissingTest()
//        {
//            MatchOptionsModel options = new MatchOptionsModel();
//            options.MatchToType = MatchTaxonToType.TaxonId;
//            List<int> ids = new List<int>();
//            ids.Add(1);
//            ids.Add(2);
//            ids.Add(TestParameters.MISSING_TAXON_ID);
//            List<MatchItem> matches = GetManager(true).GetMatches(ids);
//            Assert.AreEqual(matches[0].Status, MatchStatus.Exact);
//            Assert.AreEqual(matches[2].Status, MatchStatus.NoMatch);
//            Assert.AreEqual(ids.Count, matches.Count);

//        }


//        [TestMethod]
//        public void GetMatchResultsTest()
//        {

//        }
//    }
//}
