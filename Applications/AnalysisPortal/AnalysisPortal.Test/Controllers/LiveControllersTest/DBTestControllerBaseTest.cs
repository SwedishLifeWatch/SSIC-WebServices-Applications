using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Fakes;
using System.Threading;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Routing;
using AnalysisPortal.Controllers;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization.Fakes;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalysisPortal.Tests
{
    [TestClass]
    public class DBTestControllerBaseTest
    {

        private static ISessionHelper sessionHelper;
        //private static TestControllerBuilder builder = new TestControllerBuilder();
        private AccountController accountController = new AccountController();
        private IUserContext userContext;
        private IUserContext applicationUserContext;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        //public TestContext TestContext
        //{
        //    get { return testContextInstance; }
        //    set { testContextInstance = value; }
        //}

        public AccountController AccountController
        {
            get
            {  
                return accountController;
            }
        }

        public ISessionHelper SessionHelper
        {
            get { return sessionHelper; }
            set { sessionHelper = value; }

        }

        public IUserContext UserContext
        {
            get { return userContext; }
            set { userContext = value; }

        }

        public IUserContext ApplicationUserContext
        {
            get { return applicationUserContext; }
            set { applicationUserContext = value; }

        }
        //public TestControllerBuilder Builder
        //{
        //    get { return builder; }
        //    set { builder = value; }

        //}


        #region Additional test attributes


        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {

            //Set session helper for handling HttpContext data.
            sessionHelper = new DictionarySessionHelper();
            SessionHandler.SetSessionHelper(sessionHelper);

            // this.StubDataSourceData();

            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            CoreData.UserManager = new UserManagerMultiThreadCache();
            CoreData.RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            CoreData.ReferenceManager = new ReferenceManagerMultiThreadCache();
            CoreData.SpeciesObservationManager = new SpeciesObservationManagerMultiThreadCache();
            CoreData.MetadataManager = new MetadataManagerMultiThreadCache();
            CoreData.AnalysisManager = new AnalysisManager();

            UserDataSource.SetDataSource();
            SpeciesObservationDataSource.SetDataSource();
            GeoReferenceDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();

            //Finally login application user
            try
            {
                CoreData.UserManager.LoginApplicationUser();
            }
            catch (Exception)
            {
                // Try Once more... 
                Thread.Sleep(20000);
                CoreData.UserManager.LoginApplicationUser();
            }
            IUserContext user = CacheHandler.GetApplicationUserContext("ApplicationContext:");
            SessionHelper.SetInSession("userContext", user);
            SessionHelper.SetInSession("ApplicationContext:en-GB", user);
            SessionHelper.SetInSession("ApplicationContext:", user);
            SessionHelper.SetInSession("mySettings", new MySettings());
            SessionHelper.SetInSession("results", new CalculatedDataItemCollection());
            user.User = new User(user);
            user.User.UserName = AnalysisPortalTestSettings.Default.TestUserName + "Appuser";
            userContext = user;
            applicationUserContext = user;

            //SessionHelper.SetInSession("userContext", userContext);
            //SessionHelper.SetInSession("ApplicationContext:en-GB", applicationUserContext);
            //SessionHelper.SetInSession("ApplicationContext:", applicationUserContext);
           // CacheHandler.SetApplicationUserContext("ApplicationContext:", applicationUserContext);
            
                
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                CoreData.UserManager.Logout(applicationUserContext);
                // Reset to english language
                SetEnglishLanguage();
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }


        public IRole GetTaxonAdministratorRole(string userName, int roleId, IUserContext userContext)
        {
            IRole newRole;

            newRole = new Role(userContext);
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testdescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            newRole.Identifier = Resources.AppSettings.Default.ApplicationIdentifier;
            return newRole;
        }



        public IRole GetSpeciesFactRole(string userName, int roleId, IUserContext userContext)
        {
            IRole newRole;

            newRole = new Role(userContext);
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testSpeciesDescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            newRole.Identifier = Resources.AppSettings.Default.ApplicationIdentifier;
            AuthorityList autorities = new AuthorityList();
            Authority authority = new Authority(userContext);
            authority.UpdatePermission = true;
            authority.Identifier = "SpeciesFact";
            autorities.Add(authority);
            newRole.Authorities = autorities;
            return newRole;
        }

        public IRole GetAnalysisPortalRole(string userName, int roleId, IUserContext userContext)
        {
            IRole newRole;

            newRole = new Role(userContext);
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testAnalysisPortalDescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            newRole.Identifier = Resources.AppSettings.Default.ApplicationIdentifier;
            AuthorityList autorities = new AuthorityList();
            Authority authority = new Authority(userContext);
            authority.UpdatePermission = true;
            authority.Identifier = "Sighting";
            autorities.Add(authority);
            newRole.Authorities = autorities;
            return newRole;
        }
       

     

      

     


        #endregion

        #region Helper methods

        /// <summary>
        /// Make sure that the next time when GetCurrentUser() is executed
        /// an Exception is thrown.        
        /// </summary>
        protected void MakeGetCurrentUserFunctionCallThrowException()
        {
            //HttpContext.Current.Session.Clear();
            //HttpContext.Current.Cache.Remove("ApplicationContext:en-GB");
            //HttpContext.Current.Cache.Remove("ApplicationContext:sv-SE");
            CacheHandler.SetApplicationUserContext("ApplicationContext:", null);
            CacheHandler.SetApplicationUserContext("ApplicationContext:en-GB", null);
            CacheHandler.SetApplicationUserContext("ApplicationContext:sv-SE", null);

            CoreData.UserManager = null;
            RemoveUserContextSetOnTestStart();
            

            // this code clears all cache
            //foreach (DictionaryEntry dicEntry in HttpContext.Current.Cache)
            //{
            //    HttpContext.Current.Cache.Remove((string)dicEntry.Key);
            //}            
        }
        protected  void RemoveUserContextSetOnTestStart()
        {
            sessionHelper.SetInSession<IUserContext>("userContext", null);
            sessionHelper.SetInSession<IUserContext>("ApplicationContext:en-GB", null);
            sessionHelper.SetInSession<IUserContext>("ApplicationContext:", null);
        }

        /// <summary>
        /// Login test user using Rhino mock for handling cookies and authentication.
        /// </summary>
        protected void LoginTestUser()
        {
            //// Mock account controller (Otherwise  accountController.HttpContext is null)
            ////builder.InitializeController(accountController);

            //// To handle login stubs must be created for  IFormsAuthenticationService
            //IFormsAuthenticationService formsAuthenticationServiceMock = MockRepository.GenerateStub<IFormsAuthenticationService>();
            //accountController.FormsService = formsAuthenticationServiceMock;

            //accountController.ControllerContext = GetAccountControllerContext();

            this.ShimControllerContextForLogin();

            //Log in test user
            LogInModel logInModel = new LogInModel();
            logInModel.UserName = AnalysisPortalTestSettings.Default.TestUserLoginName;
            logInModel.Password = AnalysisPortalTestSettings.Default.TestUserPassword;
            accountController.LogIn(logInModel, String.Empty);

            SessionHandler.Language = SessionHandler.UserContext.Locale.CultureInfo.Name;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHandler.Language);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHandler.Language);
           
            

        }

        /// <summary>
        /// Login test user analyzer using Rhino mock for handling cookies and authentication.
        /// This user has two different role regarding specieds observations.
        /// </summary>
        protected void LoginTestUserAnalyser()
        {
            // Mock account controller (Otherwise  accountController.HttpContext is null)
            //builder.InitializeController(accountController);

            // To handle login stubs must be created for  IFormsAuthenticationService
            //IFormsAuthenticationService formsAuthenticationServiceMock = MockRepository.GenerateStub<IFormsAuthenticationService>();
            //accountController.FormsService = formsAuthenticationServiceMock;

            //accountController.ControllerContext = GetAccountControllerContext();


            this.ShimControllerContextForLogin();

            //Log in test user
            LogInModel logInModel = new LogInModel();
            logInModel.UserName = AnalysisPortalTestSettings.Default.TestUserAnalyzerLoginName;
            logInModel.Password = AnalysisPortalTestSettings.Default.TestUserPassword;
            accountController.LogIn(logInModel, String.Empty);

            SessionHandler.Language = SessionHandler.UserContext.Locale.CultureInfo.Name;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHandler.Language);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHandler.Language);  
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShimControllerContextForLogin(bool useCookie = true, ControllerBase controller = null)
        {
            UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.ShimUserDataSource()
                                                {
                                                    LoginStringStringStringBoolean
                                                        =
                                                        (
                                                            userName,
                                                            password,
                                                            applicationIdentifier,
                                                            isActivationRequired)
                                                        =>
                                                            {
                                                                return
                                                                    this.userContext;
                                                            },
                                                };
            CoreData.UserManager.DataSource = userDataSource;

            FormsAuthenticationService formsAuthenticationServiceMock = new ShimFormsAuthenticationService() { };
            this.accountController.FormsService = formsAuthenticationServiceMock;

            HttpContextBase stubHttpContext = GetAccountControllerContext(useCookie);
            var requestContext = new RequestContext(stubHttpContext, new RouteData());
            this.accountController.Url = new UrlHelper(requestContext);

            var routeData = new RouteData();
            ControllerBase baseStub = new BaseController();// new System.Web.Mvc.Fakes.StubControllerBase() { };
            if (controller.IsNull())
            {
                this.accountController.ControllerContext = new ControllerContext(stubHttpContext, routeData, baseStub);
                this.accountController.Url = new UrlHelper(requestContext);
            }
            else
            {
                controller.ControllerContext = new ControllerContext(stubHttpContext, routeData, baseStub);
                // controller.Url = new UrlHelper(requestContext);
            }
        }

        public static void ShimFilePath()
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string dir = Directory.GetDirectoryRoot(appPath);
             string path = dir+ @"Temp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = dir + @"Temp\myTempFile.txt";

            ShimFile.CreateString = (filePath) => { return new FileStream(fullPath, FileMode.Create); };
        }

        /// <summary>
        /// Logout test user.
        /// </summary>
        protected void LogoutTestUser()
        {
            accountController.LogOut(null);
        }

        /// <summary>
        /// Set swedish language
        /// </summary>
        protected static void SetSwedishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            sessionHelper.SetInSession("language", "sv-SE");
        }

        /// <summary>
        /// Set english language
        /// </summary>
        protected static void SetEnglishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            sessionHelper.SetInSession("language", "en-GB");
        }

        /// <summary>
        /// Mock and stub result controller
        /// </summary>
        protected void StubResultController(ResultController resultController)
        {
            // Mock account controller
            resultController.ControllerContext = GetControllerContext();

        }

        public void AddSightingRoleToUserContext()
        {
            IUserContext userContext = SessionHandler.UserContext;
            string roleName = "AnalysisPortalRole";
            int roleId = 123456789;
            SessionHandler.UserContext.CurrentRoles.Add(GetAnalysisPortalRole(roleName, roleId, userContext));
        }

        private static ControllerContext GetControllerContext()
        {
            var cookie = new HttpCookieCollection();
            cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            var queryString = new NameValueCollection();
            queryString.Add("error", "false");
            queryString.Add("handler", "true");
            HttpRequestBase request = new System.Web.Fakes.ShimHttpRequestBase(new StubHttpRequestBase())
            {
                CookiesGet = () => { return cookie; },
                FormGet = () => { return new NameValueCollection(); },
                QueryStringGet = () => { return queryString; }
              
               
            };
            HttpRequestWrapper httpRequestWrapper = new System.Web.Fakes.ShimHttpRequestWrapper()
            {
                CookiesGet = () => { return cookie; },
                FormGet = () => { return new NameValueCollection(); },
                QueryStringGet = () => { return queryString; }
                
            };

            HttpResponseBase response = new System.Web.Fakes.ShimHttpResponseBase(new StubHttpResponseBase())
            {
                CookiesGet = () => { return cookie; },
               
            };

           
          
            HttpContextBase context = new System.Web.Fakes.ShimHttpContextBase(new StubHttpContextBase())
            {
                RequestGet = () => { return httpRequestWrapper; },
                ResponseGet = () => { return response; },
                ServerGet = () => new StubHttpServerUtilityBase
                {
                    UrlEncodeString = (url) => { return "UrlEncodeTestValue"; }
                }
               
               
            };



            ControllerBase baseStub = new BaseController();

            //new System.Web.Mvc.Fakes.ShimControllerBase(new StubControllerBase())
            //{

            //};

            var routeData = new RouteData();

            return new ControllerContext(context, routeData, baseStub);
        }

        public static HttpContextBase GetAccountControllerContext(bool useCookie = true)
        {

            var cookie = new HttpCookieCollection();
            if(useCookie)
                cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            var cultureInfo = new HttpCookie("CultureInfo", "en-GB");
            HttpRequestBase stubHttpRequestBase = new System.Web.Fakes.StubHttpRequestBase()
            {
                CookiesGet = () => { return cookie; },
                
            };
            HttpResponseBase response = new System.Web.Fakes.StubHttpResponseBase()
            {
                CookiesGet = () => { return cookie; }
            };
            HttpServerUtilityBase untilityBase = new System.Web.Fakes.ShimHttpServerUtilityBase(new StubHttpServerUtilityBase())
            {
                UrlEncodeString = (info) => { return cultureInfo.ToString(); }
            };

            HttpContextBase stubHttpContext = new System.Web.Fakes.StubHttpContextBase()
            {
                RequestGet = () => { return stubHttpRequestBase; },
                ResponseGet = () => { return response; },
                ServerGet = () => { return untilityBase; }
                
            };

            return stubHttpContext;
        }

        public static ControllerContext GetBaseControllerContext()
        {
            var cookie = new HttpCookieCollection();
            cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            var cultureInfo = new HttpCookie("CultureInfo", "en-GB");
            HttpRequestBase request = new System.Web.Fakes.ShimHttpRequestBase(new StubHttpRequestBase())
            {
                CookiesGet = () => { return cookie; }
            };
            HttpResponseBase response = new System.Web.Fakes.ShimHttpResponseBase(new StubHttpResponseBase())
            {
                CookiesGet = () => { return cookie; }
            };

            HttpServerUtilityBase untilityBase = new System.Web.Fakes.ShimHttpServerUtilityBase(new StubHttpServerUtilityBase())
            {
                UrlEncodeString = (info) => { return cultureInfo.ToString(); }
            };

            HttpContextBase context = new System.Web.Fakes.ShimHttpContextBase(new StubHttpContextBase())
            {
                RequestGet = () => { return request; },
                ResponseGet = () => { return response; },
                ServerGet = () => { return untilityBase; }
            };

            ControllerBase baseStub = new BaseController();//new System.Web.Mvc.Fakes.ShimControllerBase(new StubControllerBase())
            //{

            //};

            var routeData = new RouteData();



            return new ControllerContext(context, routeData, baseStub);
        }


         public static ControllerContext GetErrorControllerContext(string actionName, string controllerName)
        {

            var cookie = new HttpCookieCollection();
            cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            HttpRequestBase request = new System.Web.Fakes.ShimHttpRequestBase(new StubHttpRequestBase())
            {
                CookiesGet = () => { return cookie; }
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

            ControllerBase baseStub = new BaseController();//
                //System.Web.Mvc.Fakes.ShimControllerBase(new StubControllerBase())
            //{

            //};

            var routeData = new System.Web.Routing.RouteData();
            routeData.Values["controller"] = controllerName;
            routeData.Values["action"] = actionName;


            return new ControllerContext(context, routeData, baseStub);
        }
        

        #endregion
    }

 
}
