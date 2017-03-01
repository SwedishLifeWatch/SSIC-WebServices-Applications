using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class AssemblyExtensionTest
    {
        public AssemblyExtensionTest()
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
        public void GetApplicationName()
        {
            String applicationName;

            applicationName = Assembly.GetExecutingAssembly().GetApplicationName();
            Assert.AreEqual(applicationName, "ArtDatabanken.Test");
        }

        [TestMethod]
        public void GetApplicationVersion()
        {
            String applicationVersion;

            applicationVersion = Assembly.GetExecutingAssembly().GetApplicationVersion();
            Assert.IsTrue(Regex.IsMatch(applicationVersion, @"^[0-9]{1,4}.[0-9]{1,4}.[0-9]{1,4}.[0-9]{1,5}$"));
        }
    }
}
