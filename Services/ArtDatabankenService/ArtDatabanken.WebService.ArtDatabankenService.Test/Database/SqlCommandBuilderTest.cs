using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Database
{
    /// <summary>
    /// Summary description for SqlCommandBuilderTest
    /// </summary>
    [TestClass]
    public class SqlCommandBuilderTest
    {
        public SqlCommandBuilderTest()
        {
        }

        #region Additional test attributes
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AddParameterBoolean()
        {
            SqlCommandBuilder command;

            // Test Boolean argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, false);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 0");

            // Test Boolean argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, "%DEV%");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, true);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = '%DEV%', @ScientificName = 1");

            // Test Boolean argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, false);
            command.AddParameter(TaxonData.TAXON_TYPE_ID, "%Hej%");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 0, @TaxonTypeId = '%Hej%'");

            // Test Boolean argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.TAXON_TYPE_ID, "gfdgsdgsg%");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, true);
            command.AddParameter(TaxonData.ID, 123654);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonTypeId = 'gfdgsdgsg%', @ScientificName = 1, @Id = 123654");
        }

        [TestMethod]
        public void AddParameterDateTime()
        {
            SqlCommandBuilder command;

            // Test DateTime argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SORT_ORDER, new DateTime(2008, 11, 11, 15, 12, 14));
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SortOrder = '2008-11-11 15:12:14.000'");

            // Test DateTime argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 52365);
            command.AddParameter(TaxonData.SORT_ORDER, new DateTime(2008, 11, 14, 20, 10, 14));
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 52365, @SortOrder = '2008-11-14 20:10:14.000'");

            // Test DateTime argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SORT_ORDER, new DateTime(2008, 11, 14, 20, 10, 33));
            command.AddParameter(TaxonData.ID, 52300);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SortOrder = '2008-11-14 20:10:33.000', @Id = 52300");

            // Test DateTime argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 52);
            command.AddParameter(TaxonData.SORT_ORDER, new DateTime(1888, 11, 14, 20, 10, 33));
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "En vanlig kommentar");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 52, @SortOrder = '1888-11-14 20:10:33.000', @ScientificName = 'En vanlig kommentar'");
        }

        [TestMethod]
        public void AddParameterDouble()
        {
            SqlCommandBuilder command;

            // Test Double argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SORT_ORDER, 32.43);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SortOrder = 32.43");

            // Test Double argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 52365);
            command.AddParameter(TaxonData.SORT_ORDER, 100.0);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 52365, @SortOrder = 100");

            // Test Double argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SORT_ORDER, 100.3);
            command.AddParameter(TaxonData.ID, 52300);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SortOrder = 100.3, @Id = 52300");

            // Test Double argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 52);
            command.AddParameter(TaxonData.SORT_ORDER, 222.222);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "En vanlig kommentar");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 52, @SortOrder = 222.222, @ScientificName = 'En vanlig kommentar'");
        }

        [TestMethod]
        public void AddParameterInt32()
        {
            SqlCommandBuilder command;

            // Test Int32 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 32);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 32");

            // Test Int32 argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            command.AddParameter(TaxonData.ID, 42);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 'lklgkj gsldfkg slk', @Id = 42");

            // Test Int32 argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 101);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 101, @ScientificName = 'lklgkj gsldfkg slk'");

            // Test Int32 argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.TAXON_TYPE_ID, true);
            command.AddParameter(TaxonData.ID, 10134);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonTypeId = 1, @Id = 10134, @ScientificName = 'lklgkj gsldfkg slk'");
        }

        [TestMethod]
        public void AddParameterInt64()
        {
            SqlCommandBuilder command;

            // Test Int64 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 32123456789123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 32123456789123");

            // Test Int64 argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            command.AddParameter(TaxonData.ID, 3212345345489123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 'lklgkj gsldfkg slk', @Id = 3212345345489123");

            // Test Int64 argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 3214235345489123);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 3214235345489123, @ScientificName = 'lklgkj gsldfkg slk'");

            // Test Int64 argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.TAXON_TYPE_ID, true);
            command.AddParameter(TaxonData.ID, 3214235347654649123);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonTypeId = 1, @Id = 3214235347654649123, @ScientificName = 'lklgkj gsldfkg slk'");
        }

        [TestMethod]
        public void AddParameterString()
        {
            SqlCommandBuilder command;

            // Test String argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "Konstigt taxon det här");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 'Konstigt taxon det här'");

            // Test String argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 234);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "Konstigt taxon det här");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 234, @ScientificName = 'Konstigt taxon det här'");

            // Test String argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "Konstigt taxon det här");
            command.AddParameter(TaxonData.ID, 555234);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @ScientificName = 'Konstigt taxon det här', @Id = 555234");

            // Test String argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter(TaxonData.ID, 234);
            command.AddParameter(TaxonData.SCIENTIFIC_NAME, "Konstigt taxon det här");
            command.AddParameter(TaxonData.TAXON_TYPE_ID, false);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Id = 234, @ScientificName = 'Konstigt taxon det här', @TaxonTypeId = 0");
        }

        [TestMethod]
        public void Constructor()
        {
            SqlCommandBuilder command;

            command = new SqlCommandBuilder("GetTaxon");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon");
        }

        [TestMethod]
        public void GetCommand()
        {
            SqlCommandBuilder command;

            command = new SqlCommandBuilder("GetTaxonTypes");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxonTypes");
        }
    }
}
