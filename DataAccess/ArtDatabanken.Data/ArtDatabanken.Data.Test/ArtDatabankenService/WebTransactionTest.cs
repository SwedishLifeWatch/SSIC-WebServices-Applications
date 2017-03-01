using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class WebTransactionTest : TestBase
    {
        public WebTransactionTest()
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
        public void Commit()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                transaction.Commit();

                // Should be ok to commit an already
                // commited transaction.
                transaction.Commit();
            }

            // Test problem with a second transaction starting
            // before timeout control of the first transaction has ended.
            using (WebTransaction transaction = new WebTransaction(1))
            {
                transaction.Commit();

                // Should be ok to commit an already
                // commited transaction.
                transaction.Commit();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void CommitTransactionTimeoutError()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                Thread.Sleep(2000);
                transaction.Commit();
            }
        }

        [TestMethod]
        public void Constructor()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                transaction.Commit();
            }
        }

        [TestMethod]
        public void Dispose()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                transaction.Dispose();

                // Should be ok to dispose an already
                // disposed transaction.
                transaction.Dispose();
            }
        }
    }
}
