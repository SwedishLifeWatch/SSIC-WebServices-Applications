using System;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test
{
    using System.Threading;

    [TestClass]
    public class TestBase
    {
        private IUserContext _userContext;

        public TestBase()
        {
            _userContext = null;
        }

        protected WebClientInformation GetClientInformation()
        {
            return ((UserDataSource)(CoreData.UserManager.DataSource)).GetClientInformation(_userContext);
        }

        protected IUserContext GetRevisionUserContext(int revisionId = 1)
        {
            // set CurrentRole = Role w/ identifier = "urn:lsid:dyntaxa.se:Revision:id for revision"
            if (_userContext.CurrentRoles.IsNotNull())
            {
                foreach (Role role in _userContext.CurrentRoles)
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

        protected IUserContext GetUserContext()
        {
            return _userContext;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
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
            CoreData.ApplicationManager = new ApplicationManager();
            CoreData.CountryManager = new CountryManagerSingleThreadCache();
            CoreData.LocaleManager = new LocaleManagerSingleThreadCache();
            CoreData.OrganizationManager = new OrganizationManager();
            CoreData.TaxonManager = new TaxonManagerSingleThreadCache();
            CoreData.UserManager = new UserManagerSingleThreadCache();
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
         
            try
            {
                _userContext = CoreData.UserManager.Login(Settings.Default.TestUserName,
                                                      Settings.Default.TestPassword,
                                                      ApplicationIdentifier.Dyntaxa.ToString()); 

            }
            catch (TimeoutException)
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                _userContext = CoreData.UserManager.Login(Settings.Default.TestUserName,
                                                       Settings.Default.TestPassword,
                                                       ApplicationIdentifier.Dyntaxa.ToString());
            }
        }
    }
}
