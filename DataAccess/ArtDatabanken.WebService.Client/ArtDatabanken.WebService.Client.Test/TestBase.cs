using System;
using System.Diagnostics;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.PictureService;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test
{
    [TestClass]
    public class TestBase
    {
        private IUserContext _userContext;
        private String _applicationName;

        public TestBase()
        {
            _userContext = null;
            _applicationName = GetTestApplicationName();
            Stopwatch = new Stopwatch();
        }

        protected Stopwatch Stopwatch { get; set; }

        virtual protected String GetTestApplicationName()
        {
            return Settings.Default.TestApplicationIdentifier;
        }

        protected WebClientInformation GetClientInformation()
        {
            return ((UserDataSource)(CoreData.UserManager.DataSource)).GetClientInformation(_userContext);
        }


        protected CoordinateSystem GetCoordinateSystem(CoordinateSystemId coordinateSystemId = CoordinateSystemId.WGS84)
        {
            CoordinateSystem coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = coordinateSystemId;
            coordinateSystem.WKT = "None";
            return coordinateSystem;
        }

        protected IUserContext GetUserContext()
        {
            return _userContext;
        }

        protected IUserContext GetRevisionUserContext(int revisionId = 1)
        {
            // set CurrentRole = Role w/ identifier = "urn:lsid:dyntaxa.se:Revision:id for revision"
            if (_userContext.CurrentRoles.IsNotNull())
            {
                foreach (IRole role in _userContext.CurrentRoles)
                {
                    var testRoleIdentifier = (Settings.Default.TestRevisionGUIDPrefix + ":" + Convert.ToString(revisionId));
                    if (role.Identifier.IsNotNull() && role.Identifier.EndsWith(testRoleIdentifier))
                    {
                        role.Identifier = Settings.Default.TestRevisionGUIDPrefix + ":" + Convert.ToString(revisionId);
                        _userContext.CurrentRole = role;
                    }
                }
            }
            if (_userContext.CurrentRole.IsNull())
            {
                throw new ArgumentException("Revision context could not be set.");
            }
            return _userContext;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public virtual void TestCleanup()
        {
            try
            {
                CoreData.UserManager.Logout(_userContext);
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            Configuration.SetInstallationType();
            //Configuration.InstallationType = InstallationType.Production;

            CoreData.CountryManager = new CountryManagerSingleThreadCache();
            CoreData.LocaleManager = new LocaleManagerSingleThreadCache();
            CoreData.UserManager = new UserManagerSingleThreadCache();
            CoreData.OrganizationManager = new OrganizationManager();
            CoreData.ApplicationManager = new ApplicationManager();
            CoreData.ReferenceManager = new ReferenceManagerSingleThreadCache();
            CoreData.RegionManager = new RegionManager(GetCoordinateSystem());
            CoreData.TaxonManager = new TaxonManagerSingleThreadCache();
            CoreData.SpeciesObservationManager = new SpeciesObservationManagerSingleThreadCache();
            CoreData.AnalysisManager = new AnalysisManager();

            UserDataSource.SetDataSource();
            GeoReferenceDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();
            SpeciesObservationDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();
            PictureDataSource.SetDataSource();

            bool loginSuccess;
            // Added try catch due to problems with Moneses-Dev loosing connection.
            try
            {
                loginSuccess = Login(Settings.Default.TestUserName, Settings.Default.TestPassword);
            }
            catch (TimeoutException)
            {
                Thread.Sleep(20000);
                loginSuccess = Login(Settings.Default.TestUserName, Settings.Default.TestPassword);
            }

            if (!loginSuccess)
            {
                throw new ArgumentException("UserManager login failed.");
            }
        }

        // UserManager Login - to get UserContext
        protected Boolean Login(String userName, String password)
        {
            _userContext = CoreData.UserManager.Login(userName, password, _applicationName);
            return (_userContext.IsNotNull());
        }      
    }
}
