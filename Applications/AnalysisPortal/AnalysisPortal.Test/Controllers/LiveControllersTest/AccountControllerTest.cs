using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using AnalysisPortal.Controllers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Resources;

namespace AnalysisPortal.Tests
{
    // using System.Web.Mvc.Fakes;
    using System.Collections.Specialized;
    using System.IO;
    using System.IO.Fakes;
    using System.Web.Fakes;

    using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    /// This is a test class for AccountControllerTest and is intended
    /// to contain all AccountControllerTest Unit Tests. For private and protected metods AccountController_Accessor is used se
    /// http://msdn.microsoft.com/en-us/library/ms184807.aspx for description.
    /// </summary>
    [TestClass]
    public class AccountControllerTest : AnalysisPortal.Tests.DBTestControllerBaseTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //    CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
        //    CoreData.UserManager = new UserManagerMultiThreadCache();

        //    UserDataSource.SetDataSource();
            
        //}
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        /// A test for AccountController Constructor
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AccountControllerConstructorTest()
        {
            AccountController controller = new AccountController();
            Assert.IsNotNull(controller);
        }

        /// <summary>
        ///A test for AccessIsNotAllowed
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void AccessIsNotAllowedTest()
        {
            //Arrange
            string url = AppSettings.Default.UrlToUserAdminMoneses;
            
            //Act
            AccountController controller = new AccountController(); 
            var result = controller.AccessIsNotAllowed(url) as ViewResult;

            //Assert
            Assert.IsNotNull(result);

            AccessIsNotAllowedViewModel accessIsNotAllowedViewModel = result.ViewData.Model as AccessIsNotAllowedViewModel;
            Assert.IsNotNull(accessIsNotAllowedViewModel);
           
            // Test model value
            Assert.IsTrue(accessIsNotAllowedViewModel.Url.Equals(url)); 
        }


        /// <summary>
        ///A test for LogIn with testuser
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogInGetTest()
        {
            //Arrange
            string userName = AnalysisPortalTestSettings.Default.TestUserLoginName;

            //Act
            // Must remove user context since it is set on test start
            RemoveUserContextSetOnTestStart();
            AccountController controller = new AccountController();
            var result = controller.LogIn(userName) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("LogIn"));

            LogInModel LogInViewModel = result.ViewData.Model as LogInModel;
            Assert.IsNotNull(LogInViewModel);
            // Test model values
            Assert.IsTrue(LogInViewModel.UserName.Equals(AnalysisPortalTestSettings.Default.TestUserLoginName));         
            
        }

        /// <summary>
        ///A test for LogIn when user already has been logged in ie userContext exist
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogInGetUserContextExistTest()
        {
            //Arrange
            string userName = AnalysisPortalTestSettings.Default.TestUserLoginName;
            SessionHandler.UserContext = new UserContext();

            //Act
            AccountController controller = new AccountController();
            var result = controller.LogIn(userName) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("LogOut"));

        }

        /// <summary>
        ///A test for LogIn without cookies
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogInMultipleRolesPostTest()
        {
            
            //Arrange
            string returnUrl;
            using (ShimsContext.Create())
            {
                //Login without cookie
                bool useCookie = false;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);
                // Add another role to user context
                base.AddSightingRoleToUserContext();

                var controller = GetAccountController(useCookie);
               

                //Act
                var redirectResult = controller.LogIn(model, returnUrl) as RedirectToRouteResult;
                //Assert
                Assert.IsNotNull(redirectResult);
                Assert.AreEqual("ChangeUserRole", redirectResult.RouteValues["action"]);
                Assert.AreEqual(returnUrl, redirectResult.RouteValues["url"]);
            }
 
        }

        

        /// <summary>
        /// A test for LogIn verifying that view "ChangeUserRole" is not shown since 
        /// user only has one role.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void LogInNoCookiesPostTest()
        {
            string returnUrl;
               
            using (ShimsContext.Create())
            {
                // Arrange
                // Login without cookie 
                bool useCookie = false;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);

                var controller = GetAccountController(useCookie);

                // Act
                ActionResult redirectResult = controller.LogIn(model, returnUrl);                 

                // Assert
                Assert.IsNotNull(redirectResult);

                // Test that redirect action is returned.
                // Assert.AreEqual(returnUrl, redirectResult.Url);
            }
        }

        /// <summary>
        /// A test for LogIn using cookies.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void LogInWithCookiesPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                // Login with cookie
                bool useCookie = true;
                string returnUrl;

                LogInModel model = CreateLoginModelTestUser(out returnUrl);

                AccountController controller = GetAccountController(useCookie);
                
                // Act
                var redirectResult = controller.LogIn(model, returnUrl);

                // Assert
                Assert.IsNotNull(redirectResult);

                // Test that Edit action is returned.
                // Assert.AreEqual(returnUrl, redirectResult.Url);
            }
        }

        /// <summary>
        /// A test for LogIn with no url set
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void LogInPostNoUrlTest()
        {

            using (ShimsContext.Create())
            {
                // Arrange
                string returnUrl;
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                LogInModel model = CreateLoginModelTestUser(out returnUrl);

                // Act
                RedirectToRouteResult result = controller.LogIn(model, "") as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("ChangeUserRole", result.RouteValues["action"]);
                Assert.AreEqual(null, result.RouteValues["controller"]);
            }
        }


        /// <summary>
        ///A test for LogIn when a modelstate error has occured
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogInPostModelStateErrorTest()
        {

            using (ShimsContext.Create())
            {
                // Arrange
                string returnUrl;

                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                LogInModel model = CreateLoginModelTestUser(out returnUrl);

                // Act
                controller.ModelState.AddModelError(string.Empty, "dummy error message");
                var viewResult = controller.LogIn(model, returnUrl) as ViewResult;

                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsTrue(viewResult.ViewName.Equals("LogIn"));
            }

        }

        /// <summary>
        ///A test for LogIn when login in to user admin fails
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogInFailsPostTest()
        {

            using (ShimsContext.Create())
            {
                //Arrange
                string returnUrl;
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                LogInModel model = CreateLoginModelTestUser(out returnUrl);
                //model.Password = "fel";
                // Rearrange loginStub returning null ie invalidLogin
                UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.ShimUserDataSource()
                {
                    LoginStringStringStringBoolean = (userName, password, applicationIdentifier, isActivationRequired) => { return null; },
                };
                CoreData.UserManager.DataSource = userDataSource;

                //Act
                var viewResult = controller.LogIn(model, returnUrl) as ViewResult;

                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsTrue(viewResult.ViewName.Equals("LogIn"));
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            }


        }


        /// <summary>
        ///A test for LogOut
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogOutTest()
        {
            
            using (ShimsContext.Create())
            {
                //Arrange
                string returnUrl;
                ShimFilePath();
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                LogInModel model = CreateLoginModelTestUser(out returnUrl);

                //Act
                controller.LogIn(model, returnUrl);
                var redirectResult = controller.LogOut(returnUrl) as RedirectResult;

                //Assert
                Assert.IsNotNull(redirectResult);

                // Test that Edit action is returned.
                Assert.AreEqual(returnUrl, redirectResult.Url);
            }
           
        }

        /// <summary>
        ///A test for LogOut with no url set
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void LogOutNoUrlTest()
        {

           
            using (ShimsContext.Create())
            {

                ShimFilePath();
                //Arrange
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);

                //Act
                var result = controller.LogOut(string.Empty) as RedirectToRouteResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
            }
           
        }

#if DEBUG        

        /// <summary>
        ///A test for AutoLogIn
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void AutoLogInTest()
        {

            // Arrange
            using (ShimsContext.Create())
            {

                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);

                //Act
                var redirectResult = controller.AutoLogIn(AppSettings.Default.UrlToUserAdminMoneses.ToLower()) as RedirectResult;

                //Assert
                Assert.IsNotNull(redirectResult);

                // Test that Edit action is returned.
                Assert.AreEqual(AppSettings.Default.UrlToUserAdminMoneses.ToLower(), redirectResult.Url);
            }

        }
       
#endif

        /// <summary>
        /// A test for changing user roles (get) - verifies that PrivatePerson role 
        /// (including sighting authority is set as Current role)
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void ChangeUserRoleGetTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                //string userName = AnalysisPortalTestSettings.Default.TestUserLoginName;
                string url = "/";	// Start page
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                string returnUrl;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);
                // Add another role to user context
                this.AddSightingRoleToUserContext();

                //Act
                controller.LogIn(model, returnUrl);
                var result = controller.ChangeUserRole(url) as ViewResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.ViewName.Equals("ChangeUserRole"));

                UserRoleModel userRoleModel = result.ViewData.Model as UserRoleModel;
                Assert.IsNotNull(userRoleModel);
                // Test model values
                Assert.IsTrue(userRoleModel.UserRoles.Count > 1);
                Assert.IsNotNull(SessionHandler.UserContext.CurrentRole);
                bool sightingExsist = false;
                foreach (Authority autority in SessionHandler.UserContext.CurrentRole.Authorities)
                {
                    if (autority.Identifier.Equals("Sighting"))
                        sightingExsist = true;
                }
                Assert.IsTrue(sightingExsist);
            }
        }


        /// <summary>
        /// A test for changing user roles (get) - verifies that PrivatePerson role 
        /// (including sighting authority is set as Current role), even if current role alredy is set.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void ChangeUserRoleCurrentRoleSetGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                // string userName = AnalysisPortalTestSettings.Default.TestUserLoginName;
                string url = "/"; // Start page
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                // Must add role to application user roles.
                IUserContext userContext = SessionHandler.UserContext;
                string roleName = "AnalysisPortalRole";
                int roleId = 123456789;
                SessionHandler.UserContext.CurrentRoles.Add(base.GetAnalysisPortalRole(roleName,roleId, userContext));
                string returnUrl;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);

                // Act
                controller.LogIn(model, returnUrl);
                SessionHandler.UserContext.CurrentRole = SessionHandler.UserContext.CurrentRoles[2];
                var result = controller.ChangeUserRole(url) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.ViewName.Equals("ChangeUserRole"));

                UserRoleModel userRoleModel = result.ViewData.Model as UserRoleModel;
                Assert.IsNotNull(userRoleModel);

                // Test model values
                Assert.IsTrue(userRoleModel.UserRoles.Count > 1);
                Assert.IsNotNull(SessionHandler.UserContext.CurrentRole);
                bool sightingExsist = false;
                foreach (Authority autority in SessionHandler.UserContext.CurrentRole.Authorities)
                {
                    if (autority.Identifier.Equals("Sighting"))
                    {
                        sightingExsist = true;
                    }
                }

                Assert.IsTrue(sightingExsist);
            }
        }


        /// <summary>
        /// A test for changing user roles (post) - verifies that current role has been changed for logged in user.
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void ChangeUserRolePostNoUrlSetTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                string returnUrl;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);
                UserRoleModel roleModel = new UserRoleModel();
                IUserContext userContext = SessionHandler.UserContext;
                string roleName = "AnalysisPortalRole";
                int roleId = 123456789;
                SessionHandler.UserContext.CurrentRoles.Add(base.GetAnalysisPortalRole(roleName, roleId, userContext));

                //Act
                controller.LogIn(model, returnUrl);

                // Set current role
                SessionHandler.UserContext.CurrentRole = SessionHandler.UserContext.CurrentRoles[0];
                IRole oldRole = SessionHandler.UserContext.CurrentRole;
                var result = controller.ChangeUserRole(Convert.ToString(SessionHandler.UserContext.CurrentRoles[1].Id), roleModel) as RedirectToRouteResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.IsTrue(oldRole.Id != SessionHandler.UserContext.CurrentRole.Id);
            }
       
        }

        /// <summary>
        /// A test for changing user roles (post) - verifies that role hs been changed for logged in user,
        /// and returning to calling url
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void ChangeUserRolePostTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                string url = "/Presentation/Table";	// Start page
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                UserRoleModel roleModel = new UserRoleModel();
                roleModel.ReturnUrl = url;
                string returnUrl;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);
                IUserContext userContext = SessionHandler.UserContext;
                string roleName = "AnalysisPortalRole";
                int roleId = 123456789;
                SessionHandler.UserContext.CurrentRoles.Add(base.GetAnalysisPortalRole(roleName, roleId, userContext));

                //Act
                controller.LogIn(model, returnUrl);

                // Set current role
                SessionHandler.UserContext.CurrentRole = SessionHandler.UserContext.CurrentRoles[0];
                IRole oldRole = SessionHandler.UserContext.CurrentRole;
                var result = controller.ChangeUserRole(Convert.ToString(SessionHandler.UserContext.CurrentRoles[1].Id), roleModel) as RedirectResult;

                //Assert
                Assert.IsNotNull(result);

                // Test that Edit action is returned.
                Assert.AreEqual(url.ToLower(), result.Url);
                Assert.IsTrue(oldRole.Id != SessionHandler.UserContext.CurrentRole.Id);
            }

        }


        /// <summary>
        /// A test for changing user roles (post) - verifies that view is reloaded if invalid new role is set.
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void ChangeUserRoleInvaldRolePostTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                string url = "/Presentation/Table";	// Start page
                // Login without cookie
                bool useCookie = false;
                var controller = GetAccountController(useCookie);
                UserRoleModel roleModel = new UserRoleModel();
                roleModel.ReturnUrl = url;
                string returnUrl;
                LogInModel model = CreateLoginModelMultiRoleUser(out returnUrl);

                //Act
                controller.LogIn(model, returnUrl);

                // Set current role
                SessionHandler.UserContext.CurrentRole = SessionHandler.UserContext.CurrentRoles[0];
                IRole oldRole = SessionHandler.UserContext.CurrentRole;
                var result = controller.ChangeUserRole(null, roleModel) as ViewResult;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("ChangeUserRole", result.ViewName);
                Assert.IsTrue(oldRole.Id == SessionHandler.UserContext.CurrentRole.Id);
            }
        }

        #region helper methods

        /// <summary>
        /// Mocking controller and set modelvalues
        /// </summary>
        /// <returns></returns>
        private AccountController  GetAccountController(bool useCookie)
        {
            base.ShimControllerContextForLogin(useCookie);
            return base.AccountController;
        }

        private static LogInModel CreateLoginModelTestUser(out string returnUrl)
        {
            LogInModel model = new LogInModel();
            model.UserName = AnalysisPortalTestSettings.Default.TestUserLoginName;
            model.Password = AnalysisPortalTestSettings.Default.TestUserPassword;
            returnUrl = AppSettings.Default.UrlToUserAdminMoneses.ToLower();
            return model;
        }

        private static LogInModel CreateLoginModelMultiRoleUser(out string returnUrl)
        {
            LogInModel model = new LogInModel();
            model.UserName = AnalysisPortalTestSettings.Default.TestUserAnalyzerLoginName;
            model.Password = AnalysisPortalTestSettings.Default.TestUserPassword;
            returnUrl = AppSettings.Default.UrlToUserAdminMoneses.ToLower();
            return model;
        }


        /// <summary>
        /// Mock cookies request and response
        /// </summary>
        /// <param name="useCookie"></param>
        /// <returns></returns>
        private static HttpContextBase GetControllerContext(bool useCookie)
        {
            HttpCookieCollection cookie = new HttpCookieCollection();
            if(useCookie)
            {
                cookie.Add(new HttpCookie("CultureInfo", "en-GB")); 
            }
            
          
            HttpRequestBase request = new System.Web.Fakes.ShimHttpRequestBase(new StubHttpRequestBase())
            {
                CookiesGet = () => { return cookie; },
                FormGet = () => { return new NameValueCollection(); },

            };

            HttpResponseBase response = new System.Web.Fakes.ShimHttpResponseBase(new StubHttpResponseBase())
            {
                CookiesGet = () => { return cookie; }
            };

            HttpContextBase context = new System.Web.Fakes.ShimHttpContextBase(new StubHttpContextBase())
            {
                RequestGet = () => { return request; },
                ResponseGet = () => { return response; }
            };

           

            return context;
        }
        #endregion

    }
}
