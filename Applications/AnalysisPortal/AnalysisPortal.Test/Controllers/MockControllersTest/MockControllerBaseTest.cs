using System;
using System.Globalization;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Dyntaxa.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;

namespace AnalysisPortal.Tests
{
    using ArtDatabanken.WebService.Client.ReferenceService;

    [TestClass]
    public class MockControllerBaseTest
    {
        private static UserDataSourceTestRepository userDataSourceTestRepository = new UserDataSourceTestRepository();

        private static ISessionHelper sessionHelper;
       // private static TestControllerBuilder builder = new TestControllerBuilder();
        private  IUserContext testUserContext;
        private  IUserContext applicationUserContext;
        private IUserContext userContext;
       // private IUserContext applicationUserContext;

        public UserDataSourceTestRepository UserDataSourceTestRepository
        {
            get
            {
                return userDataSourceTestRepository;
            }
            set
            {
                userDataSourceTestRepository = value;
            }

        }
        
        public ISessionHelper SessionHelper
        {
            get
            {
                return sessionHelper;
            }
            set
            {
                sessionHelper = value;
            }

        }

        //public TestControllerBuilder Builder
        //{
        //    get
        //    {
        //        return builder;
        //    }
        //    set
        //    {
        //        builder = value;
        //    }

        //}

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

        public IUserContext ApplicationUserContext
        {
            get
            {
                return applicationUserContext;
            }
            set
            {
                applicationUserContext = value;
            }

        }

        public IUserContext TestUserContext
        {
            get
            {
                return testUserContext;
            }
            set
            {
                testUserContext = value;
            }

        }

        public IUserContext UserContext
        {
            get { return userContext; }
            set { userContext = value; }

        }

      

        #region Additional test attributes
      

        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
           
            //Set applicatiobn start parameters that will be set in Global.asax.cs-Application_Start())
            //CoreData.CountryManager = new CountryManagerMultiThreadCache();
            //CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            //CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            //CoreData.UserManager = new UserManagerMultiThreadCache();
            //CoreData.RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            //Set session helper for handling HttpContext data.
            sessionHelper = new DictionarySessionHelper();
            SessionHandler.SetSessionHelper(sessionHelper);

            //Stub Data including set up of application user context.
            StubDataSourceAndManagerData();
           
            sessionHelper.SetInSession("applicationUserContext", applicationUserContext);
            sessionHelper.SetInSession("userContext", applicationUserContext);
            sessionHelper.SetInSession("ApplicationContext:en-GB", applicationUserContext);
            sessionHelper.SetInSession("mySettings", new MySettings());
            sessionHelper.SetInSession("results", new CalculatedDataItemCollection());
           
            //  Set Session start parameters that will be set in Global.asax.cs-Session_Start(object sender, EventArgs e)
            sessionHelper.SetInSession("language", "en-GB");
            sessionHelper.SetInSession("mySettings", new MySettings());
            sessionHelper.SetInSession("results", new CalculatedDataItemCollection()); 

            // Set language that will be set in Global.asax.cs-Application_BeginRequest(object sender, EventArgs e)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionHelper.GetFromSession<string>("language"));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionHelper.GetFromSession<string>("language"));
        }

        public IUser GetUser(IUserContext testUserContext,  IDataContext dataContext, bool multipleUsers = false)
        {
         
            int id = AnalysisPortalTestSettings.Default.TestUserId;
            if (multipleUsers)
                id = id + 1;

            UpdateInformation updateInformation = new UpdateInformation();
            updateInformation.CreatedBy = AnalysisPortalTestSettings.Default.TestUserId;
            updateInformation.CreatedDate = DateTime.Now;
            updateInformation.ModifiedBy = AnalysisPortalTestSettings.Default.TestUserId;
            updateInformation.ModifiedDate = DateTime.Now;

            IUser testUser = new ArtDatabanken.Data.Fakes.StubIUser()
                                {
                                    UserNameGet = () => { return AnalysisPortalTestSettings.Default.TestUserName; },
                                    ApplicationIdGet = () => { return AnalysisPortalTestSettings.Default.TestnAnalysisPortalApplcationId; },
                                    IsAccountActivatedGet = () => { return true; },
                                    EmailAddressGet = () => { return AnalysisPortalTestSettings.Default.TestUserEmail; },
                                    DataContextGet = () => { return dataContext; },
                                    GUIDGet = () => { return AnalysisPortalTestSettings.Default.TestUserGuid; },
                                    IdGet = () => { return id; },
                                    ShowEmailAddressGet = () => { return true; },
                                    TypeGet = () => { return UserType.Person; },
                                    UpdateInformationGet = () => { return updateInformation; },
                                    ValidFromDateGet = () => { return DateTime.Now; },
                                    ValidToDateGet = () => { return new DateTime(2144, 12, 31); }                                
                                };





            return testUser;
        }

        public IUser GetApplicationUser(IUserContext appUserContext, IDataContext dataContext)
        {
            UpdateInformation updateInformation = new UpdateInformation();
            updateInformation.CreatedBy = AnalysisPortalTestSettings.Default.TestUserId + 1;
            updateInformation.CreatedDate = DateTime.Now;
            updateInformation.ModifiedBy = AnalysisPortalTestSettings.Default.TestUserId + 1;
            updateInformation.ModifiedDate = DateTime.Now;

            IUser appUser = new ArtDatabanken.Data.Fakes.StubIUser()
                                {
                                    UserNameGet = () => { return AnalysisPortalTestSettings.Default.TestUserName + "Appuser"; },
                                    ApplicationIdGet = () => { return AnalysisPortalTestSettings.Default.TestnAnalysisPortalApplcationId; },
                                    IsAccountActivatedGet = () => { return true; },
                                    EmailAddressGet = () => { return AnalysisPortalTestSettings.Default.TestUserEmail + "Appuser"; },
                                    DataContextGet = () => { return dataContext; },
                                    GUIDGet = () => { return AnalysisPortalTestSettings.Default.TestUserGuid + "Appuser"; },
                                    IdGet = () => { return AnalysisPortalTestSettings.Default.TestUserId + 1; },
                                    ShowEmailAddressGet = () => { return true; },
                                    TypeGet = () => { return UserType.Person; },
                                    UpdateInformationGet = () => { return updateInformation; },
                                    ValidFromDateGet = () => { return DateTime.Now; },
                                    ValidToDateGet = () => { return new DateTime(2144, 12, 31); }                                
                                };




            return appUser;
        }

        public DataContext GetDataContext(IUserContext testUserContext)
        {
            // Set data
            ILocale locale = new Locale(AnalysisPortalTestSettings.Default.SwedishLocaleId, AnalysisPortalTestSettings.Default.SwedishLocale, AnalysisPortalTestSettings.Default.SwedishNameString, AnalysisPortalTestSettings.Default.SvenskNameString, new DataContext(testUserContext));

            IDataSourceInformation dataSource = new DataSourceInformation();
            DataContext dataContext = new DataContext(dataSource, locale);

            return dataContext;
        }

        private void StubDataSourceAndManagerData()
        {

            int testRoleId = 3333;
            int testApplicationRoleId = 4444;
          
            UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubUserDataSource() { };
           

            IUserManager testUserManager = new ArtDatabanken.Data.Fakes.StubIUserManager()
            {
                LoginStringStringStringBoolean =
                    (userName,
                     password,
                     applicationIdentifier,
                     isActivationRequired) =>
                    {
                        return this.userContext;
                    },
                LoginStringStringString =
                    (userName,
                     password,
                     applicationIdentifier) =>
                    {
                        return
                            this.applicationUserContext;
                    }
            };

            CoreData.UserManager = testUserManager;
            CoreData.UserManager.DataSource = userDataSource;
            // CoreData.UserManager.DataSource = userDataSource;
            CoreData.OrganizationManager.DataSource = userDataSource;

            CountryDataSource countryDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubCountryDataSource();
            CoreData.CountryManager.DataSource = countryDataSource;

            LocaleDataSource localeDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubLocaleDataSource();
            {
            }

            ApplicationDataSource applicationDataSource =
                new ArtDatabanken.WebService.Client.UserService.Fakes.StubApplicationDataSource();
            CoreData.ApplicationManager.DataSource = applicationDataSource;

            ReferenceDataSource referenceDataSource = new ArtDatabanken.WebService.Client.ReferenceService.Fakes.StubReferenceDataSource();
            CoreData.ReferenceManager.DataSource = referenceDataSource;

            TaxonDataSource taxonDataSource = new ArtDatabanken.WebService.Client.TaxonService.Fakes.StubTaxonDataSource();

             ITaxonManager testTaxonManager = new ArtDatabanken.Data.Fakes.StubITaxonManager()
            {
                   
            };

            CoreData.TaxonManager = testTaxonManager;
            
            CoreData.TaxonManager.DataSource = taxonDataSource;

            SpeciesObservationDataProviderList dataProviders = new SpeciesObservationDataProviderList();

            dataProviders.Add(new SpeciesObservationDataProvider());
            SpeciesObservationDataSource speciesObservationDataSource =
                new ArtDatabanken.WebService.Client.SpeciesObservationService.Fakes.StubSpeciesObservationDataSource()
                {
                    //  public SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext)
                };
            SpeciesObservationManager testSpeciesObservationManager =
                new ArtDatabanken.Data.Fakes.StubSpeciesObservationManager()
                {
                    GetSpeciesObservationDataProvidersIUserContext
                        = (context) => { return dataProviders; }
                };

            CoreData.SpeciesObservationManager = testSpeciesObservationManager;

            CoreData.SpeciesObservationManager.DataSource = speciesObservationDataSource;
            CoreData.MetadataManager.SpeciesObservationDataSource = speciesObservationDataSource;

            RegionDataSource regionDataSource =
                new ArtDatabanken.WebService.Client.GeoReferenceService.Fakes.StubRegionDataSource();
            CoreData.RegionManager.DataSource = regionDataSource;

            AnalysisDataSource analysisDataSource =
                new ArtDatabanken.WebService.Client.AnalysisService.Fakes.StubAnalysisDataSource();
            CoreData.AnalysisManager.DataSource = analysisDataSource;

           

            this.StubApplicationUserContex(testApplicationRoleId);

            this.StubUserContext(testRoleId);

            LocaleList usedLocales = new LocaleList();
            ILocale testSvLocale = new Locale(
                AnalysisPortalTestSettings.Default.SwedishLocaleId,
                AnalysisPortalTestSettings.Default.SwedishLocale,
                AnalysisPortalTestSettings.Default.SwedishNameString,
                AnalysisPortalTestSettings.Default.SvenskNameString,
                new DataContext(this.userContext));

            usedLocales.Add(testSvLocale);

            LocaleManager testLocaleManager = new ArtDatabanken.Data.Fakes.StubLocaleManager()
            {
                GetUsedLocalesIUserContext =
                    (context) =>
                    {
                        return usedLocales;
                    },
                GetDefaultLocaleIUserContext
                    =
                    (context) =>
                    {
                        return
                            testSvLocale;
                    }
            };

            CoreData.LocaleManager = testLocaleManager;
            CoreData.LocaleManager.DataSource = localeDataSource;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testRoleId"></param>
        private void StubUserContext(int testRoleId)
        {
            IDataContext dataContext = new ArtDatabanken.Data.Fakes.StubIDataContext()
            {

            };
            ILocale locale = new Locale(
                AnalysisPortalTestSettings.Default.SwedishLocaleId,
                AnalysisPortalTestSettings.Default.SwedishLocale,
                AnalysisPortalTestSettings.Default.SwedishNameString,
                AnalysisPortalTestSettings.Default.SvenskNameString,
                dataContext);

            IUser user = this.GetUser(this.userContext, dataContext);
            this.userContext = new ArtDatabanken.Data.Fakes.StubIUserContext()
                                   {
                                       LocaleSetILocale = (value) => locale = value,
                                       LocaleGet = () => locale,
                                       UserGet = () => user,
                                       CurrentRoleGet =
                                           () =>
                                               {
                                                   return
                                                       this.GetAnalysisPortalRole(
                                                           "TestUser",
                                                           testRoleId,
                                                           this.userContext);
                                               }
                                   };
        }

        private ILocale StubApplicationUserContex(int testRoleId)
        {

            IDataContext applicationDataContext = new ArtDatabanken.Data.Fakes.StubIDataContext()
                                           {

                                           };
            ILocale applicationLocale = new Locale(
                AnalysisPortalTestSettings.Default.SwedishLocaleId,
                AnalysisPortalTestSettings.Default.SwedishLocale,
                AnalysisPortalTestSettings.Default.SwedishNameString,
                AnalysisPortalTestSettings.Default.SvenskNameString,
                applicationDataContext);
            IUser applicationUser = this.GetApplicationUser(this.applicationUserContext, applicationDataContext);
            this.applicationUserContext = new ArtDatabanken.Data.Fakes.StubIUserContext()
                                              {
                                                  LocaleSetILocale =
                                                      (value) =>
                                                      applicationLocale = value,
                                                  LocaleGet =
                                                      () => applicationLocale,
                                                  UserGet = () => applicationUser,
                                                  CurrentRoleGet =
                                                      () =>
                                                          {
                                                              return
                                                                  this.GetAnalysisPortalRole
                                                                      (
                                                                          AppSettings
                                                                              .Default
                                                                              .ApplicationUserName,
                                                                          testRoleId,
                                                                          this
                                                                              .applicationUserContext);
                                                          }
                                              };
            return applicationLocale;
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
           
        }


        #endregion


        protected void LoginTestUser()
        {
            
            sessionHelper.SetInSession("userContext", testUserContext);

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
    }
}
