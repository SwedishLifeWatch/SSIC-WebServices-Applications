using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using Resources;
using Dyntaxa.Controllers;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UrlHelper = System.Web.Mvc.UrlHelper;

// ReSharper disable once CheckNamespace
namespace Dyntaxa.Test
{
    using System.Collections.Generic;
    using System.Web.Fakes;

    using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
    using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

    using Dyntaxa.Test;
    using Dyntaxa.Test.TestModels;

    /// <summary>
    /// The controller test base.
    /// </summary>
    public class ControllerTestBase
    {

        private static UserDataSourceTestRepository userDataSourceTestRepository = new UserDataSourceTestRepository();
        private static TaxonDataSourceTestRepository taxonDataSourceTestRepository = new TaxonDataSourceTestRepository();
        private static SpeciesFactModelManagerTestRepository speciesFactModelManagerTestRepository = new SpeciesFactModelManagerTestRepository();
        private static PesiNameDataSourceTestRepository _pEsiNameDataSourceTestRepository = new PesiNameDataSourceTestRepository();
        
        public static UserDataSourceTestRepository UserDataSourceTestRepositoryData
        {
            get
            {
                return userDataSourceTestRepository;
            }
        }

        public static TaxonDataSourceTestRepository TaxonDataSourceTestRepositoryData
        {
            get
            {
                return taxonDataSourceTestRepository;
            }
        }

        public static SpeciesFactModelManagerTestRepository SpeciesFactModelManagerTestRepositoryData
        {
            get
            {
                return speciesFactModelManagerTestRepository;
            }
        }

        public static PesiNameDataSourceTestRepository PesiNameDataSourceTestRepositoryData
        {
            get
            {
                return _pEsiNameDataSourceTestRepository;
            }
        }

        
        
        /// <summary>
        /// The session helper.
        /// </summary>
        private static ISessionHelper sessionHelper;

       
        /// <summary>
        /// The user context for logged in user.
        /// </summary>
        private static IUserContext userContextData;

        /// <summary>
        /// The application user context.
        /// </summary>
        private static IUserContext applicationUserContext;

        /// <summary>
        /// The application user context for swedish.
        /// </summary>
        private static IUserContext applicationUserContextSV;

        /// <summary>
        /// The taxon id tuple.
        /// </summary>
        private static TaxonIdTuple taxonIdTuple;

        /// <summary>
        /// The revision.
        /// </summary>
        private static ITaxonRevision taxonRevision;
            

        /// <summary>
        /// Gets or sets the session helper.
        /// </summary>
        public ISessionHelper SessionHelper
        {
            get { return sessionHelper; }
            set { sessionHelper = value; }
        }

        /// <summary>
        /// Gets or sets the cache helper.
        /// </summary>
        public ISessionHelper CacheHelper
        {
            get { return sessionHelper; }
            set { sessionHelper = value; }

        }

        /// <summary>
        /// Gets or sets the user context.
        /// </summary>
        public static IUserContext UserContextData
        {
            get { return userContextData; }
            set { userContextData = value; }
        }

        /// <summary>
        /// Gets or sets the application user context.
        /// </summary>
        public static IUserContext ApplicationUserContext
        {
            get { return applicationUserContext; }
            set { applicationUserContext = value; }
        }

        /// <summary>
        /// Gets or sets the application user context.
        /// </summary>
        public static IUserContext ApplicationUserContextSV
        {
            get { return applicationUserContextSV; }
            set { applicationUserContextSV = value; }
        }

        /// <summary>
        /// Gets or sets the revision id.
        /// </summary>
        public static int SessionRevisionId {get; set;}

        /// <summary>
        /// Gets or sets the revision id.
        /// </summary>
        public static ITaxonRevision SessionRevision
        {
            get
            {
                return taxonRevision;
            }
            set
            {
                taxonRevision = value;
            }
        }

        /// <summary>
        /// Gets or sets the session taxon id.
        /// </summary>
        public static TaxonIdTuple SessionTaxonId
        {
            get
            {
                return taxonIdTuple;
            }
            set
            {
                taxonIdTuple = value;
            }
        }

        /// <summary>
        /// Transaction property
        /// </summary>
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the selected path.
        /// </summary>
        public static string SelectedPath
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTestBase"/> class.
        /// </summary>
        public ControllerTestBase()
        {
           
        }

        /// <summary>
        /// Logout test user.
        /// </summary>
        /// <param name="accountController">
        /// The account Controller.
        /// </param>
        public void LogoutTestUser(AccountController accountController)
        {
            accountController.LogOut(null);
        }

        /// <summary>
        /// Login application user.
        /// </summary>
        /// <param name="stubbedSwedishUserContext">
        /// The stubbed swedish user context, if required.
        /// </param>
        protected void LoginApplicationUser(IUserContext stubbedSwedishApplicationUserContext, IUserContext stubbedApplicationUserContext)
        {

            // Set session helper for handling HttpContext data.
            HttpContext contextBase = GetShimHttpContext();
            sessionHelper = new HttpContextSessionHelper(contextBase);


            // Login application user
            try
            {
                CoreData.UserManager.LoginApplicationUser();
            }
            catch (TimeoutException)
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                CoreData.UserManager.LoginApplicationUser();
            }

            //if (stubbedApplicationUserContext.IsNotNull() && stubbedSwedishApplicationUserContext.IsNotNull())
            //{
            //    // Set session settings.
            //    SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey, stubbedApplicationUserContext);
            //    SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, stubbedApplicationUserContext);
            //    SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, stubbedSwedishApplicationUserContext);
            //    SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextString, DyntaxaTestSettings.Default.EnglishLocale);
            //    SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextIdString, DyntaxaTestSettings.Default.EnglishLocaleId);

            //}
            // Get cash settings.
            //applicationUserContext = CacheHelper.GetFromCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey);
            //IUserContext applicationUserContextEN = CacheHelper.GetFromCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale);
            //IUserContext applicationUserContextSV = CacheHelper.GetFromCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale);            

            // If we use stubbed object we have to set application context for swedish also, if we use login to DB this is not needed.
            if (applicationUserContextSV.IsNull() && stubbedSwedishApplicationUserContext != null)
            {
                applicationUserContextSV = stubbedSwedishApplicationUserContext;
            }
            else if (stubbedSwedishApplicationUserContext == null)
            {
                applicationUserContextSV = CoreData.UserManager.GetApplicationContext("sv-SE");
                applicationUserContext = CoreData.UserManager.GetApplicationContext();
            }

            // Set session settings.
            //SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey, applicationUserContext);
            //SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, applicationUserContextEN);
            //SessionHelper.SetInSession(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, applicationUserContextSV);
            //CacheHelper.SetInCache(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, applicationUserContextSV);

            //// Set Session start parameters that will be set in Global.asax.cs-Session_Start(object sender, EventArgs e)
            //SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextString, SpeciesIdentificationTestSettings.Default.EnglishLocale);

            //// Set language that will be set in Global.asax.cs-Application_BeginRequest(object sender, EventArgs e)
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHelper.GetFromSession<string>(SpeciesIdentificationTestSettings.Default.LanguageContextString));
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHelper.GetFromSession<string>(SpeciesIdentificationTestSettings.Default.LanguageContextString));
            
            // Set Session start parameters that will be set in Global.asax.cs-Session_Start(object sender, EventArgs e)
            //SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextString, DyntaxaTestSettings.Default.EnglishLocale);

            // Set language that will be set in Global.asax.cs-Application_BeginRequest(object sender, EventArgs e)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(DyntaxaTestSettings.Default.EnglishLocale);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DyntaxaTestSettings.Default.EnglishLocale);
            SessionTaxonId = TaxonIdTuple.Create(100047.ToString(), 100047);
        }

        /// <summary>
        /// Logout application user.
        /// </summary>
        protected void LogoutApplicationUser()
        {
            IUserContext appUserContext = CacheHelper.GetFromCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey);
            if (appUserContext != null)
            {
                CoreData.UserManager.Logout(appUserContext);
                SessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, null);
                SessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, null);
                SessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey, null);
                CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey, null);
                CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, null);
                CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, null);
            }
        }

        /// <summary>
        /// Login test user. Must be encapsulated in 
        /// using (ShimsContext.Create()).
        /// </summary>
        /// <param name="stubbedLoginUserContext">
        /// Stubbed user context.
        /// </param>
        protected void LoginTestUser(IUserContext stubbedLoginUserContext = null)
        {
            string userName = DyntaxaTestSettings.Default.TestUserName;
            string password = DyntaxaTestSettings.Default.TestUserPassword;

            // Log in test user
            if (stubbedLoginUserContext.IsNotNull())
            {
                // Set stubbed userContext for login
                userContextData = stubbedLoginUserContext;
                SessionHelper.SetInSession<IUserContext>("userContext", userContextData);
            }

            
            CoreData.UserManager.Login(userName, password, DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
        }

        /// <summary>
        /// Set swedish language.
        /// </summary>
        public void SetSwedishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(DyntaxaTestSettings.Default.SwedishLocale);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DyntaxaTestSettings.Default.SwedishLocale);
           // SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextString, DyntaxaTestSettings.Default.SwedishLocale);
        }

        /// <summary>
        /// Set english language.
        /// </summary>
        protected void SetEnglishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(DyntaxaTestSettings.Default.SwedishLocale);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(DyntaxaTestSettings.Default.SwedishLocale);
          //  SessionHelper.SetInSession(DyntaxaTestSettings.Default.LanguageContextString, DyntaxaTestSettings.Default.SwedishLocale);
        }

        /// <summary>
        /// Make sure that the next time when GetCurrentUser() is executed
        /// an Exception is thrown.        
        /// </summary>
        protected void MakeGetCurrentUserFunctionCallThrowException()
        {
            CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey, null);
            CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, null);
            CacheHelper.SetInCache<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, null);

            CoreData.UserManager = null;
            RemoveUserContextSetOnTestStart();
        }

        /// <summary>
        /// The remove user context set on test start.
        /// </summary>
        protected void RemoveUserContextSetOnTestStart()
        {
            sessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale, null);
            sessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey, null);
            sessionHelper.SetInSession<IUserContext>(DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale, null);

        }

        #region Controller context set up using shim methods

        /// <summary>
        /// Gets a shim controller context, used for tests. Must be encapsulated in 
        /// using (ShimsContext.Create())
        /// {} 
        /// statement.
        /// </summary>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <param name="useCookie">
        /// The used cookie.
        /// </param>
        /// <returns>
        /// A shimmed controller context. <see cref="ControllerContext"/>.
        /// </returns>
        public static ControllerContext GetShimControllerContext(string actionName, string controllerName, bool useCookie = true)
        {
            HttpContext shimHttpContext = GetShimHttpContext(useCookie);
            HttpContextBase shimHttpContextBase = new HttpContextWrapper(shimHttpContext);

            ControllerBase baseStub = new DyntaxaBaseController();//new BaseController();
            var routeData = new RouteData();
            routeData.Values["controller"] = controllerName;
            routeData.Values["action"] = actionName;
            return new ControllerContext(shimHttpContextBase, routeData, baseStub);
        }

        /// <summary>
     

        //public static HttpControllerContext GetShimHttpControllerContext(string apiUri, string actionName, string controllerName)
        //{
        //    HttpConfiguration configuration = new HttpConfiguration();
        //    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/" + apiUri);
        //    IHttpRoute route = configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
        //    HttpRouteData routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", controllerName }, { "action", actionName } });

        //    return new HttpControllerContext(configuration, routeData, requestMessage);
        //}
        
        /// <summary>
        /// Gets a shim account controller context, used for tests. Must be encapsulated in 
        /// using (ShimsContext.Create())
        /// </summary>
        /// <param name="controller">
        /// Account controller to be shimmed.
        /// </param>
        /// <param name="actionName">
        /// The action Name.
        /// </param>
        /// <param name="useCookie">
        /// Set cookie if required.
        /// </param>
        protected void GetShimAccountControllerContext(AccountController controller, string actionName, bool useCookie = true)
        {
            HttpContext shimHttpContext = GetShimHttpContext(useCookie);
            HttpContextBase shimHttpContextBase = new HttpContextWrapper(shimHttpContext);
            ControllerBase baseStub = new DyntaxaBaseController();
          
            FormsAuthenticationService formsAuthenticationServiceMock = new FormsAuthenticationService();//new ShimFormsAuthenticationService() { };
            controller.FormsService = formsAuthenticationServiceMock;
            var requestContext = new RequestContext(shimHttpContextBase, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            var routeData = new RouteData();
            routeData.Values["controller"] = "Account";
            routeData.Values["action"] = actionName;

            controller.ControllerContext = new ControllerContext(shimHttpContextBase, routeData, baseStub);
        }

        /// <summary>
        /// The get shim http context base, used for tests. Must be encapsulated in 
        /// using (ShimsContext.Create())
        /// {} 
        /// statement.
        /// </summary>
        /// <param name="useCookie">
        /// Set to true if cookies are used.
        /// </param>
        /// <returns>
        /// Shimmed httpContextBase. <see cref="HttpContextBase"/>.
        /// </returns>
        private static HttpContext GetShimHttpContext(bool useCookie = true)
        {
            string[] allFactorEnumFieldValueKeys = new string[] { "factorEnumFieldValue_1091_0_0_1097_2" };
            string[] allFactorEnumFieldValueKeys2 = new string[] { "factorEnumFieldValue2_1091_0_0_1097_2" };
            string[] allFactorEnumFieldValueKeys3 = new string[] { "factorEnumFieldValue3_1091_0_0_1097_2" };
            string key1 = "1";
            string key2 = "2";
            string key3 = "3";

            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("factorEnumFieldValue_1091_0_0_1097_2", key1);
            nameValueCollection.Add("factorEnumFieldValue2_1091_0_0_1097_2", key2);
            nameValueCollection.Add("factorEnumFieldValue3_1091_0_0_1097_2", key3);

            //var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
            //mockHttpContext.Stub(c => c.Request.Form.AllKeys).Return(allFactorEnumFieldValueKeys).Repeat.Any();
            ////mockHttpContext.Stub(c => c.Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue_"))).Return(allFactorEnumFieldValueKeys).Repeat.Any();
            ////mockHttpContext.Stub(c => c.Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue2_"))).Return(allFactorEnumFieldValueKeys2).Repeat.Any();
            ////mockHttpContext.Stub(c => c.Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue3_"))).Return(allFactorEnumFieldValueKeys3).Repeat.Any();
            //
            //mockHttpContext.Stub(c => c.Request.Form).Return(value).Repeat.Any();

        
            
            var cookie = new HttpCookieCollection();
            cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            var queryString = new NameValueCollection();
            queryString.Add("error", "false");
            queryString.Add("handler", "true");
            var cultureInfo = new HttpCookie("CultureInfo", "en-GB");
            if (useCookie)
            {
                cookie.Add(cultureInfo);
            }

            HttpRequest stubHttpRequestBase = new System.Web.Fakes.ShimHttpRequest()
            {
                CookiesGet = () => { return cookie; },
                FormGet = () =>
                {
                    if (true)
                    {
                        return nameValueCollection;
                    }
                    else
                    {
                        return nameValueCollection;
                    }
                },
                QueryStringGet = () => { return queryString; },

            };
         

            HttpResponse response = new System.Web.Fakes.ShimHttpResponse()
            {
                CookiesGet = () => { return cookie; }
            };
            HttpServerUtilityBase untilityBase = new System.Web.Fakes.StubHttpServerUtilityBase()
            {
                UrlEncodeString = (info) => { return cultureInfo.ToString(); },
                MapPathString = (path) => { return SelectedPath; }
            };
            HttpServerUtility untility = new System.Web.Fakes.ShimHttpServerUtility()
            {
                UrlEncodeString = (info) => { return cultureInfo.ToString(); },
                MapPathString = (path) => { return SelectedPath; },
                
            };

            

              HttpApplicationState state = new System.Web.Fakes.ShimHttpApplicationState()
            {
                AddStringObject = (cacheKey, userContext) =>
                    {
                        IUserContext tempContext = userContext as IUserContext;

                        if (tempContext.Locale.Id == DyntaxaTestSettings.Default.SwedishLocaleId)
                        {
                            ApplicationUserContextSV = tempContext;
                        }
                        else
                        {
                            ApplicationUserContext = tempContext;
                        }
                    }
            };
            
            var context = new ShimHttpContext
            {
                ApplicationGet = () => { return state; },
                RequestGet = () => { return stubHttpRequestBase; },
                ResponseGet = () => { return response; },
                ServerGet = () => { return untility; }
                

            };
            ShimHttpContext.CurrentGet = () =>
            {
                return context;
            };
           
           
            // Create session varables
            var session = new System.Web.SessionState.Fakes.ShimHttpSessionState()
                              {
                                  ItemGetString = (key) =>
                                    {
                                        if (key == DyntaxaSettings.Default.ApplicationContextCacheKey) return ApplicationUserContext;
                                        else if (key == DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale) return ApplicationUserContext;
                                        else if (key == DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale) return ApplicationUserContextSV;
                                        else if (key == "userContext")
                                        {
                                            if (UserContextData.IsNotNull())
                                            {
                                                return UserContextData;
                                            }
                                            else
                                            {
                                                return ApplicationUserContext;
                                            }
                                        }
                                        else if (key == "RevisionId") return SessionRevisionId;
                                        else if (key == "TaxonId") return SessionTaxonId;
                                        else if (key == "Revision") return SessionRevision;
                                        else if (key == "SpeciesFactHostTaxonIdList") return SessionSpeciesFactHostTaxonIdList;
                                        return null;
                                    },
                                    ItemSetStringObject = (key, sessionObject) =>
                                        {
                                            if (key == "TaxonId")
                                            {
                                                SessionTaxonId = sessionObject as TaxonIdTuple;
                                            }
                                        },
                                   
                                    
                                    
                              };
          

              System.Web.Fakes.ShimHttpContext.AllInstances.SessionGet =
                (o) =>
                {
                    return session;
                };
              
              // Creat cash varables
              var cache = new System.Web.Caching.Fakes.ShimCache()
              {
                  ItemGetString = (key) =>
                  {
                      if (key == DyntaxaSettings.Default.ApplicationContextCacheKey) return ApplicationUserContext;
                      else if (key == DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.EnglishLocale) return ApplicationUserContext;
                      else if (key == DyntaxaSettings.Default.ApplicationContextCacheKey + DyntaxaTestSettings.Default.SwedishLocale) return ApplicationUserContextSV;
                      else if (key == "userContext")
                      {
                          if (UserContextData.IsNotNull())
                          {
                              return UserContextData;
                          }
                          else
                          {
                              return ApplicationUserContext;
                          }
                      }
                      else if (key == "RevisionId") return SessionRevisionId;
                      else if (key == "TaxonId") return SessionTaxonId;
                      else if (key == "Revision") return SessionRevision;
                      else if (key == "SpeciesFactHostTaxonIdList") return SessionSpeciesFactHostTaxonIdList;
                      return null;
                  },


              };


              System.Web.Fakes.ShimHttpContext.AllInstances.CacheGet =
                (o) =>
                {
                    return cache;
                };

            return context;
        }

        /// <summary>
        /// The get shim http context base, used for tests. Must be encapsulated in 
        /// using (ShimsContext.Create())
        /// {} 
        /// statement.
        /// </summary>
        /// <param name="useCookie">
        /// Set to true if cookies are used.
        /// </param>
        /// <returns>
        /// Shimmed httpContextBase. <see cref="HttpContextBase"/>.
        /// </returns>
        private static HttpContextBase GetStubHttpContextBase(bool useCookie = true)
        {
            var cookie = new HttpCookieCollection();
            cookie.Add(new HttpCookie("CultureInfo", "en-GB"));
            var queryString = new NameValueCollection();
            queryString.Add("error", "false");
            queryString.Add("handler", "true");
            var cultureInfo = new HttpCookie("CultureInfo", "en-GB");
            if (useCookie)
            {
                cookie.Add(cultureInfo);
            }

            HttpRequestBase stubHttpRequestBase = new System.Web.Fakes.StubHttpRequestBase()
            {
                CookiesGet = () => { return cookie; },
                FormGet = () => { return new NameValueCollection(); },
                QueryStringGet = () => { return queryString; }
            };
            // Might be needed? instead of stubHttpRequestBase: HttpRequestWrapper httpRequestWrapper = new System.Web.Fakes.ShimHttpRequestWrapper()
            // {
            //    CookiesGet = () => { return cookie; },
            //    FormGet = () => { return new NameValueCollection(); },
            //    QueryStringGet = () => { return queryString; }
            // };

            HttpResponseBase response = new System.Web.Fakes.StubHttpResponseBase()
            {
                CookiesGet = () => { return cookie; }
            };
            HttpServerUtilityBase untilityBase = new System.Web.Fakes.StubHttpServerUtilityBase()
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

        #endregion

        #region File handling unsing shim methods
        /// <summary>
        /// Shims a file path.
        /// </summary>
        public static void ShimFilePath(string filePath, string fileName)
        {

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = filePath + fileName;

            System.IO.Fakes.ShimFile.CreateString = (path) => { return new FileStream(fullPath, FileMode.Create); };
        }



        #endregion    
 
    
    
        public static object SessionSpeciesFactHostTaxonIdList { get; set; }
    }
}