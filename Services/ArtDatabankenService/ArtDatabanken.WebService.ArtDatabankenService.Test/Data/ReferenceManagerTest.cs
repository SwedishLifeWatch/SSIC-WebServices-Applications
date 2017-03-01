using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class ReferenceManagerTest : TestBase 
    {
        public ReferenceManagerTest()
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
        public void AddUserSelectedReferences()
        {
            foreach (UserSelectedReferenceUsage referenceUsage in Enum.GetValues(typeof(UserSelectedReferenceUsage)))
            {
                ReferenceManager.AddUserSelectedReferences(GetContext(), ReferenceManagerTest.GetSomeReferenceIds(GetContext()), referenceUsage);
                ReferenceManager.DeleteUserSelectedReferences(GetContext());
            }
        }

        [TestMethod]
        public void CreateReference()
        {
            List<WebReference> references;
            WebReference reference;
            Int32 Year;
            String Text;
            String Name;
            Int32 Id;

            reference = ReferenceManager.GetReferences(GetContext())[1];
            Id = 0;
            Year = 2008;
            Text = "Testtext";
            Name = "TestName InsertTest";
            reference.Id = Id;
            reference.Year = Year;
            reference.Text = Text;
            reference.Name = Name;

            ReferenceManager.CreateReference(GetContext(), reference);
            references = ReferenceManager.GetReferences(GetContext());
            reference = references[references.Count - 1];

            Assert.AreEqual(Year, reference.Year);
            Assert.AreEqual(Name, reference.Name);
            Assert.AreEqual(Text, reference.Text);
        }

        [TestMethod]
        public void DeleteUserSelectedReferences()
        {
            foreach (UserSelectedReferenceUsage referenceUsage in Enum.GetValues(typeof(UserSelectedReferenceUsage)))
            {
                ReferenceManager.AddUserSelectedReferences(GetContext(), ReferenceManagerTest.GetSomeReferenceIds(GetContext()), referenceUsage);
                ReferenceManager.DeleteUserSelectedReferences(GetContext());
            }
        }

        public static WebReference GetOneReference(WebServiceContext context)
        {
            return ReferenceManager.GetReferences(context)[0];
        }

        [TestMethod]
        public void GetReferences()
        {
            List<WebReference> references;

            references = ReferenceManager.GetReferences(GetContext());
            Assert.IsNotNull(references);
            Assert.IsTrue(references.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferencesBySearchString()
        {
            List<WebReference> references;
            String searchString = "Gärdenfors";
            references = ReferenceManager.GetReferencesBySearchString(GetContext(), searchString);
            Assert.IsNotNull(references);
            Assert.IsTrue(references.Count > 4);
        }

        public static List<WebReference> GetSomeReferences(WebServiceContext context)
        {
            List<WebReference> allReferences, references;

            references = new List<WebReference>();
            allReferences = ReferenceManager.GetReferences(context);
            references.Add(allReferences[0]);
            references.Add(allReferences[1]);
            return references;
        }

        public static List<Int32> GetSomeReferenceIds(WebServiceContext context)
        {
            List<Int32> referenceIds;

            referenceIds = new List<Int32>();
            foreach (WebReference reference in GetSomeReferences(context))
            {
                referenceIds.Add(reference.Id);
            }
            return referenceIds;
        }

        public static DataTable GetUserSelectedReferences(WebServiceContext context)
        {
            return GetUserSelectedReferences(context, GetSomeReferences(context));
        }

        public static DataTable GetUserSelectedReferences(WebServiceContext context,
                                                          List<WebReference> referenceIds)
        {
            DataColumn column;
            DataRow row;
            DataTable referenceTable;

            referenceTable = new DataTable(UserSelectedReferenceData.TABLE_NAME);
            column = new DataColumn(UserSelectedReferenceData.REQUEST_ID, typeof(Int32));
            referenceTable.Columns.Add(column);
            column = new DataColumn(UserSelectedReferenceData.REFERENCE_ID, typeof(Int32));
            referenceTable.Columns.Add(column);
            column = new DataColumn(UserSelectedReferenceData.REFERENCE_USAGE, typeof(String));
            referenceTable.Columns.Add(column);
            foreach (WebReference reference in referenceIds)
            {
                row = referenceTable.NewRow();
                row[0] = context.RequestId;
                row[1] = reference.Id;
                row[2] = UserSelectedReferenceUsage.Output.ToString();
                referenceTable.Rows.Add(row);
            }
            return referenceTable;
        }

        [TestMethod]
        public void UpdateReference()
        {
            //Following test might not work, problem could be timeouts

            WebReference oldReference;
            WebReference reference;
            Int32 oldYear;
            String oldName;
            String oldText;

            oldReference = ReferenceManager.GetReferences(GetContext())[1];
            Assert.IsTrue(oldReference.IsNotNull());

            oldYear = oldReference.Year;
            oldName = oldReference.Name;
            oldText = oldReference.Text;

            oldReference.Year = 1912;
            oldReference.Text = "Testtext";
            oldReference.Name = "TestName Test";

            ReferenceManager.UpdateReference(GetContext(), oldReference);
            reference = ReferenceManager.GetReferences(GetContext())[1];

            Assert.AreNotEqual(oldYear, reference.Year);
            Assert.AreNotEqual(oldName, reference.Name);
            Assert.AreNotEqual(oldText, reference.Text);
        }
    }
}
