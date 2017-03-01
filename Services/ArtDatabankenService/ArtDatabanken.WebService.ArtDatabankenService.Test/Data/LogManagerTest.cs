using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class LogManagerTest : TestBase
    {
        public LogManagerTest()
        {
            ApplicationIdentifier = Settings.Default.WebAdminstrationApplicationIdentifier;
        }

        [TestMethod]
        public void DeleteTrace()
        {
            List<WebLogRow> logRows;

            // Check that we have no trace items.
            LogManager.DeleteTrace(GetContext());
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsEmpty());

            // Create trace log items.
            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContextCached(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token, "StartTrace"))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsNotEmpty());

            // Delete trace log.
            LogManager.DeleteTrace(GetContext());
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsEmpty());
        }

        [TestMethod]
        public void GetLog()
        {
            List<WebLogRow> logRows;

            logRows = LogManager.GetLog(GetContext(), LogType.None, null, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = LogManager.GetLog(GetContext(), LogType.Error, null, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = LogManager.GetLog(GetContext(), LogType.None, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = LogManager.GetLog(GetContext(), LogType.Security, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            logRows = LogManager.GetLog(GetContext(), LogType.None, null, -1);
            Assert.IsTrue(logRows.IsEmpty());
            logRows = LogManager.GetLog(GetContext(), LogType.None, "NoUserName", 1);
            Assert.IsTrue(logRows.IsEmpty());
            LogManager.DeleteTrace(GetContext());
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, null, 1);
            Assert.IsTrue(logRows.IsEmpty());
        }

        public static WebLogRow GetOneLogRow(WebServiceContext context)
        {
            return LogManager.GetLog(context, LogType.None, null, 1)[0];
        }

        [TestMethod]
        public void Log()
        {
            foreach (LogType type in Enum.GetValues(typeof(LogType)))
            {
                LogManager.Log(GetContext(), "Hej hopp", type, null);
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
                LogManager.LogError(GetContext(), exception);
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
                LogManager.LogSecurityError(GetContext(), exception);
            }
        }
    }
}
