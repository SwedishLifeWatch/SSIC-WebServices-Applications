#if (NO_INFRAGISTICS == false)
using ArtDatabanken.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace ArtDatabanken.IO.Test
{
    
    
    /// <summary>
    ///This is a test class for ExcelFileTest and is intended
    ///to contain all ExcelFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExcelFileTest
    {


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


        /// <summary>
        ///A test for ExcelFile Constructor
        ///</summary>
        [TestMethod()]
        public void ExcelFileConstructorTest()
        {
            DataTable dataTable = new DataTable();
            DataColumn column1 = new DataColumn("Column1");
            column1.Caption = "TestColumn1";
            dataTable.Columns.Add(column1);
            DataColumn column2 = new DataColumn("Column2");
            column2.Caption = "TestColumn2";
            dataTable.Columns.Add(column2);
            for (int row = 0; row < 6; row++)
            {
                dataTable.Rows.Add("Rad " + row.ToString(), "Data");
            }

            TempFile file = new TempFile("xls");
            
            ExcelFile excelfile = new ExcelFile(dataTable, file.FileName);

            string fileNeme = file.FileName;

        }
    }
}
#endif