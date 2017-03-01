using System.Globalization;
using System.Threading;
using System.Web;
using AnalysisPortal.Controllers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.Data;
using System.Web.Mvc;

namespace AnalysisPortal.Tests
{
    using System.Runtime.Remoting.Channels;

    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    ///This is a test class for BaseControllerTest and is intended
    ///to contain all BaseControllerTest Unit Tests.For private and protected metods BaseController_Accessor is used se
    /// http://msdn.microsoft.com/en-us/library/ms184807.aspx for description.
    ///</summary>
    [TestClass()]
    public class BaseControllerTest : DBTestControllerBaseTest
    {

        

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        

        #endregion


       

        /// <summary>
        ///A test for BaseController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void BaseControllerConstructorTest1()
        {
            BaseController target = new BaseController();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetApplicationUser
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("AnalysisPortal.dll")]
        public void GetApplicationUserTest()
        {

            using (ShimsContext.Create())
            {
                this.LoginTestUser();
                BaseController controller = new BaseController();
                IUserContext appUser = controller.GetApplicationUser();

                Assert.IsNotNull(appUser);
                Assert.IsTrue(appUser.User.UserName.Equals("AnalysisPortalUser"));
            }
        }

        /// <summary>
        ///A test for GetCurrentUser
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("AnalysisPortal.dll")]
        public void GetCurrentUserTest()
        {

            using (ShimsContext.Create())
            {
                BaseController controller = new BaseController();
                ShimControllerContextForLogin(false, controller);

                IUserContext user = controller.GetCurrentUser();

                Assert.IsNotNull(user);
                Assert.IsTrue(user.User.UserName.Equals("AnalysisPortalTestUserAppuser"));
            }
        }

        

        /// <summary>
        ///A test for RemoveCookie
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("AnalysisPortal.dll")]
        public void RemoveCookieTest()
        {
            using (ShimsContext.Create())
            {
                // Test that code dont throw exception
                string key = "CultureInfo";
                BaseController controller = new BaseController();
                ShimControllerContextForLogin(false, controller);
                
                controller.RemoveCookie(key);
            }
        }

        /// <summary>
        ///A test for SetLanguage
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("AnalysisPortal.dll")]
        public void SetLanguageTest()
        {
            using (ShimsContext.Create())
            {
                string cultureISOCode = "en-GB";
                BaseController controller = new BaseController();
                SetSwedishLanguage();
                string languageSet = SessionHelper.GetFromSession<string>("language");
                Assert.IsTrue(Thread.CurrentThread.CurrentUICulture.Name.Equals("sv-SE"));
                Assert.IsTrue(Thread.CurrentThread.CurrentCulture.Name.Equals("sv-SE"));
                Assert.IsTrue(languageSet.Equals("sv-SE"));

                ShimControllerContextForLogin(false, controller);
                controller.SetLanguage(cultureISOCode);
                languageSet = SessionHelper.GetFromSession<string>("language");

                Assert.IsTrue(Thread.CurrentThread.CurrentUICulture.Name.Equals("en-GB"));
                Assert.IsTrue(Thread.CurrentThread.CurrentCulture.Name.Equals("en-GB"));
                Assert.IsTrue(languageSet.Equals("en-GB"));
            }
            
        }
    }
}
