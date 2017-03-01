//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;
//using System.Data;
//using System.IO;
//using System.Xml;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
//    [TestClass]
//    public class ListManagerTest : TestBase
//    {

//        public ListManagerTest()
//        {
//        }


//        [TestMethod]
//        public void GetBasicTaxonListTest()
//        {
//            ListOptionsModel options = new ListOptionsModel(TestParameters.CARABIDAE_TAXON_ID);
//            ListManager manager = new ListManager(options);
//            List<ReadTaxonItem> list = manager.GetBasicTaxonList();
//            Assert.IsTrue(list.Count > 10);
//            Assert.AreEqual(TestParameters.CARABIDAE_TAXON_ID, list[0].Id.ToString());
//        }

//        [TestMethod]
//        public void CreateExcelFileHierarchicalListTest()
//        {
//            ListOptionsModel options = new ListOptionsModel("Orthoptera");
//            options.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.xlsx";

//            options.ListType = TaxonListType.Hierarchical;
//            //options.OutputAuthor = true;
//            //options.OutputCommonName = true;
//            options.IncludeAuthorInAllNameCells = true;

//            ListManager manager = new ListManager(options);
//            bool success = manager.CreateExcelFile();
//            Assert.IsTrue(success);

//           // ArtDatabanken.IO.ExcelFile file = new ArtDatabanken.IO.ExcelFile(options.FileName, true);
//          //  DataTable table = file.DataTable;
//         //   Assert.AreEqual("Gammarus", table.Rows[1][1].ToString());

//            //File.Delete(options.FileName);
//        }

//        [TestMethod]
//        public void CreateExcelFileSraightListTest()
//        {
//            ListOptionsModel options = new ListOptionsModel(TestParameters.CARABIDAE_TAXON_ID);
//            options.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.xlsx";
//            options.ListType = TaxonListType.Straight;
//            options.OutputScientificName = true;
//            options.OutputGUID = true;
//            options.OutputTaxonCategory = true;
//            options.OutputAuthor = true;
//            options.OutputCommonName = true;

//            ListManager manager = new ListManager(options);
//            bool success = manager.CreateExcelFile();
//            Assert.IsTrue(success);

//            // ArtDatabanken.IO.ExcelFile file = new ArtDatabanken.IO.ExcelFile(options.FileName, true);
//            //  DataTable table = file.DataTable;
//            //   Assert.AreEqual("Gammarus", table.Rows[1][1].ToString());

//            //File.Delete(options.FileName);
//        }

//        [TestMethod]
//        public void CreateExcelFileNameListTest()
//        {
//            ListOptionsModel options = new ListOptionsModel(TestParameters.GAMMARUS_TAXON_ID);
//            options.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.xlsx";

            
//            options.ListType = TaxonListType.TaxonName;

//            options.OutputScientificName = true;
//            options.OutputTaxonCategory = true;
//            options.OutputAuthor = true;

//            ListManager manager = new ListManager(options);
//            bool success = manager.CreateExcelFile();
//            Assert.IsTrue(success);

//            //File.Delete(options.FileName);
//        }
//    }
//}
