using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class LogManagerTest : TestBase
    {
        public LogManagerTest()
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
        public void DeleteTrace()
        {
            List<WebLogRow> logRows;

            // Check that we have no trace items.
            WebServiceData.LogManager.DeleteTrace(GetContext());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsEmpty());

            // Create trace log items.
            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContext(Settings.Default.TestUserName, Settings.Default.TestApplicationIdentifier))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsNotEmpty());

            // Delete trace log.
            WebServiceData.LogManager.DeleteTrace(GetContext());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsEmpty());
        }

        [TestMethod]
        public void GetLog()
        {
            List<WebLogRow> logRows;

            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.None, null, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Error, null, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.None, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Security, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.None, null, -1);
            Assert.IsTrue(logRows.IsEmpty());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.None, "NoUserName", 1);
            Assert.IsTrue(logRows.IsEmpty());
            WebServiceData.LogManager.DeleteTrace(GetContext());
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, null, 1);
            Assert.IsTrue(logRows.IsEmpty());

            // Test with character '.
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, "Test' user", 1);
            Assert.IsTrue(logRows.IsEmpty());
        }

        public static WebLogRow GetOneLogRow(WebServiceContext context)
        {
            return WebServiceData.LogManager.GetLog(context, LogType.None, null, 1)[0];
        }

        [TestMethod]
        public void Log()
        {
            foreach (LogType type in Enum.GetValues(typeof(LogType)))
            {
                WebServiceData.LogManager.Log(GetContext(), "Hej hopp", type, null);
            }
        }

        [TestMethod]
        public void LogError()
        {
            String text = null;

            try
            {
                text.CheckNotEmpty("text");
            }
            catch (Exception exception)
            {
                WebServiceData.LogManager.LogError(GetContext(), exception);
            }
        }

        [TestMethod]
        public void LogSecurityError()
        {
            String text = null;

            try
            {
                text.CheckNotEmpty("text");
            }
            catch (Exception exception)
            {
                WebServiceData.LogManager.LogSecurityError(GetContext(), exception);
            }
        }
    }
}
