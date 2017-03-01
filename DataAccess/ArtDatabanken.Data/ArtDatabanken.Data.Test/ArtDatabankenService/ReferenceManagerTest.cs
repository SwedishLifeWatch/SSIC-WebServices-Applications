using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using ReferenceList = ArtDatabanken.Data.ArtDatabankenService.ReferenceList;

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

        public static Data.ArtDatabankenService.Reference GetAReference()
        {
            return GetAReference(false);
        }

        public static Data.ArtDatabankenService.Reference GetAReference(Boolean refresh)
        {
            return Data.ArtDatabankenService.ReferenceManager.GetReferences(refresh)[0];
        }

        [TestMethod]
        public void GetReference()
        {
            Data.ArtDatabankenService.Reference reference2;

            foreach (Data.ArtDatabankenService.Reference reference1 in GetSomeReferences(true))
            {
                Assert.IsNotNull(reference1);
                reference2 = Data.ArtDatabankenService.ReferenceManager.GetReference(reference1.Id);
                Assert.IsNotNull(reference2);
                Assert.IsTrue(AreEqual(reference1, reference2));
            }
        }

        [TestMethod]
        public void GetReferences()
        {
            ReferenceList referenceList;

            referenceList = Data.ArtDatabankenService.ReferenceManager.GetReferences();
            Assert.IsTrue(referenceList.IsNotEmpty());
            referenceList = Data.ArtDatabankenService.ReferenceManager.GetReferences(false);
            Assert.IsTrue(referenceList.IsNotEmpty());
            referenceList = Data.ArtDatabankenService.ReferenceManager.GetReferences(true);
            Assert.IsTrue(referenceList.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferencesBySearchString()
        {
            ReferenceList referenceList;
            String searchString = "Gärdenfors";
            referenceList = Data.ArtDatabankenService.ReferenceManager.GetReferences(searchString);
            Assert.IsTrue(referenceList.IsNotEmpty());
        }

        public static ReferenceList GetSomeReferences()
        {
            return GetSomeReferences(false);
        }

        public static ReferenceList GetSomeReferences(Boolean refresh)
        {
            ReferenceList references;

            references = new ReferenceList();
            references.AddRange(Data.ArtDatabankenService.ReferenceManager.GetReferences(refresh).GetRange(0, 10));
            return references;
        }

        [TestMethod]
        public void UpdateReference()
        {
            Data.ArtDatabankenService.Reference reference;
            Int32 oldYear;
            String oldName;
            String oldText;
            Data.ArtDatabankenService.Reference updatedReference;

            using (WebTransaction transaction = new WebTransaction(10))
            {
                reference = (Data.ArtDatabankenService.Reference)Data.ArtDatabankenService.ReferenceManager.GetReferences().Find(1);
                Assert.IsNotNull(reference);

                oldYear = reference.Year;
                oldName = reference.Name;
                oldText = reference.Text;

                reference.Name = "NameFromReferenceManagerTest";
                reference.Text = "Text Test";
                reference.Year = 1925;

                Data.ArtDatabankenService.ReferenceManager.UpdateReference(reference);

                updatedReference = (Data.ArtDatabankenService.Reference)Data.ArtDatabankenService.ReferenceManager.GetReferences().Find(1);
                Assert.AreNotEqual(oldYear, updatedReference.Year);
                Assert.AreNotEqual(oldName, updatedReference.Name);
                Assert.AreNotEqual(oldText, updatedReference.Text);

                transaction.Dispose(); //I am afraid there is no rollback...
            }

        }

        [TestMethod]
        public void CreateReference()
        {
            Data.ArtDatabankenService.Reference reference = new Data.ArtDatabankenService.Reference(0, "Testinsert", 0, "Testinsert");
            Data.ArtDatabankenService.Reference lastReference;
            Data.ArtDatabankenService.Reference newLastReference;
            ReferenceList references;
            List<Data.ArtDatabankenService.Reference> listOfReferences = new List<Data.ArtDatabankenService.Reference>();
            Int32 lastReferenceId = 0;
            Int32 newLastReferenceId = 0;

            // Den funkar men går aldrig in i rollback??
            using (WebTransaction transaction = new WebTransaction(10))
            {
                //Find the last reference Id
                references = Data.ArtDatabankenService.ReferenceManager.GetReferences(true);
                foreach (Data.ArtDatabankenService.Reference reference1 in references)
                {
                    listOfReferences.Add(reference1);
                }
                lastReference = listOfReferences.Last();
                Assert.IsNotNull(lastReference);

                lastReferenceId = lastReference.Id;
                newLastReferenceId = lastReferenceId + 1;

                //Insert a reference
                Data.ArtDatabankenService.ReferenceManager.CreateReference(reference);

                //Find the new last reference Id
                references = Data.ArtDatabankenService.ReferenceManager.GetReferences(true);
                foreach (Data.ArtDatabankenService.Reference reference1 in references)
                {
                    listOfReferences.Add(reference1);
                }
                newLastReference = listOfReferences.Last();

                Assert.AreNotEqual(lastReferenceId, newLastReferenceId);
                transaction.Dispose(); //I am afraid there is no rollback...

            }
        }
        /*
           [TestMethod]
           public void DeleteReference()
           {
              Int32 referenceId;
              Int32 numberOfReferences;
              Int32 newNumberOfReferences;
              ReferenceList references; 

               using (WebTransaction transaction = new WebTransaction(10))
               {

                   references = ReferenceManager.GetReferences(true);
                   numberOfReferences = references.Count;

                   //Delete a reference
                   ReferenceManager.DeleteReference(referenceId);

                   //Find the new last reference Id
                   references = ReferenceManager.GetReferences(true);
                   newNumberOfReferences = references.Count;

                   Assert.AreNotEqual(numberOfReferences, newNumberOfReferences);
                   transaction.Dispose(); //I am afraid there is no rollback...

               }
           }
        */
    }
}
