using System;
using System.Globalization;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels
{

    [TestClass]
    public class TestBase
    {
        private static ISessionHelper sessionHelper;

        public ISessionHelper SessionHelper
        {
            get { return sessionHelper; }
            set { sessionHelper = value; }

        }

        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {

            //Set session helper for handling HttpContext data.
            sessionHelper = new DictionarySessionHelper();
            SessionHandler.SetSessionHelper(sessionHelper);

            //Set applicatiobn start parameters that will be set in Global.asax.cs-Application_Start())
            CoreData.ApplicationManager = new ApplicationManager();
            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            CoreData.OrganizationManager = new OrganizationManager();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            CoreData.UserManager = new UserManagerMultiThreadCache();
            CoreData.RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            CoreData.AnalysisManager = new AnalysisManager();

            //Create datasources
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            SpeciesObservationDataSource.SetDataSource();
            GeoReferenceDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();

            //Set Session start parameters that will be set in Global.asax.cs-Session_Start(object sender, EventArgs e)
            SessionHelper.SetInSession("language", "en-GB");
            SessionHelper.SetInSession("mySettings", new AnalysisPortal.MySettings.MySettings());
            SessionHelper.SetInSession("results", new CalculatedDataItemCollection());
            // Set language that will be set in Global.asax.cs-Application_BeginRequest(object sender, EventArgs e)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHelper.GetFromSession<string>("language"));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHelper.GetFromSession<string>("language"));

            LoginApplicationUser(); // to remove this call, you must implement SpeciesObservationDataSourceTestRepository.GetSpeciesObservationFieldDescriptions
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (CacheHandler.GetApplicationUserContext("ApplicationContext:").IsNotNull())
                {
                    CoreData.UserManager.Logout(CacheHandler.GetApplicationUserContext("ApplicationContext:"));
                }

                sessionHelper = null;
                SessionHandler.UserContext = null;
                SessionHelper.SetInSession<IUserContext>("userContext", null);
                SessionHelper.SetInSession<IUserContext>("ApplicationContext:en-GB", null);
                SessionHelper.SetInSession<IUserContext>("ApplicationContext:sv-SE", null);
                SessionHelper.SetInSession<IUserContext>("ApplicationContext:", null);
                CacheHandler.SetApplicationUserContext("ApplicationContext:", null);
                CacheHandler.SetApplicationUserContext("ApplicationContext:en-GB", null);
                CacheHandler.SetApplicationUserContext("ApplicationContext:sv-SE", null);
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        protected void LoginApplicationUser()
        {
            //Finally login application user
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
            IUserContext user = CacheHandler.GetApplicationUserContext("ApplicationContext:");

            SessionHelper.SetInSession("userContext", user);
            //SessionHelper.SetInSession("ApplicationContext:en-GB", user);
            //SessionHelper.SetInSession("ApplicationContext:sv-SE", user);
        }

        /// <summary>
        /// Logout application user.
        /// </summary>
        protected void LogoutApplicationUser()
        {
            IUserContext userContext = CacheHandler.GetApplicationUserContext("ApplicationContext:");
            if (userContext != null)
            {
                CoreData.UserManager.Logout(userContext);
                SessionHandler.UserContext = null;
            }
        }
        /// <summary>
        /// Login test user
        /// </summary>
        protected void LoginTestUser()
        {
            string userName = AnalysisPortalTestSettings.Default.TestUserLoginName;
            string password = AnalysisPortalTestSettings.Default.TestUserPassword;

            IUserContext userContext = CoreData.UserManager.Login(userName, password, AppSettings.Default.ApplicationIdentifier);
            SessionHandler.UserContext = userContext;
            SessionHandler.Language = SessionHandler.UserContext.Locale.CultureInfo.Name;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHandler.Language);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHandler.Language);
        }

        /// <summary>
        /// Logout test user.
        /// </summary>
        protected void LogoutTestUser()
        {
            IUserContext userContext = SessionHandler.UserContext;
            if (userContext != null)
            {
                CoreData.UserManager.Logout(userContext);
                SessionHandler.UserContext = null;                
            }
        }

        /// <summary>
        /// Set swedish language
        /// </summary>
        protected static void SetSwedishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
        }

        /// <summary>
        /// Set english language
        /// </summary>
        protected static void SetEnglishLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        
        protected void MockSpeciesObservationManager()
        {
            CoreData.SpeciesObservationManager.DataSource = new SpeciesObservationDataSourceTestRepository();
        }

        protected void MockUserManager()
        {
            CoreData.UserManager.DataSource = new UserDataSourceTestRepository();
            //IUserContext userContext = CoreData.UserManager.DataSource.Login(
            //    AnalysisPortalTestSettings.Default.TestUserLoginName,
            //    AnalysisPortalTestSettings.Default.TestUserPassword,
            //    AppSettings.Default.ApplicationIdentifier, false);
            //SessionHelper.SetInSession("userContext", userContext);
            //SessionHelper.SetInSession("ApplicationContext:en-GB", userContext);
        }

        protected void MockMetadataManager()
        {
            CoreData.MetadataManager.SpeciesObservationDataSource = new SpeciesObservationDataSourceTestRepository();
        }

        protected void MockDataSources()
        {
            //MockMetadataManager();
            MockSpeciesObservationManager();
            MockUserManager();
        }
    }
}
