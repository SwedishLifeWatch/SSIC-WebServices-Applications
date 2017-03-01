using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;

using Microsoft.VisualStudio.TestTools.UnitTesting;



// ReSharper disable once CheckNamespace
namespace Dyntaxa.Test
{
    using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

    /// <summary>
    /// The controller test base. Used for nightly tests. Only login 
    /// of user is shimmed. 
    /// </summary>
    [TestClass]
    public class ControllerNightlyTestBase : ControllerTestBase
    {
        /// <summary>
        ///  Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            // Kör tester mot produktionsmiljö
            //Configuration.Debug = false;
            //Configuration.InstallationType = InstallationType.Production;

            // Set used datasources    
            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            CoreData.UserManager = new UserManagerMultiThreadCache();
            CoreData.RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            CoreData.ReferenceManager = new ReferenceManagerMultiThreadCache();
            CoreData.SpeciesObservationManager = new SpeciesObservationManagerMultiThreadCache();
            CoreData.MetadataManager = new MetadataManagerMultiThreadCache();
            CoreData.AnalysisManager = new AnalysisManager();
            CoreData.SpeciesFactManager = new SpeciesFactManagerMultiThreadCache();
            CoreData.FactorManager = new FactorManagerMultiThreadCache();            

            UserDataSource.SetDataSource();
            SpeciesObservationDataSource.SetDataSource();
            GeoReferenceDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();

            // Finally login application user
            //LoginApplicationUser(null, null); 
        }

        /// <summary>
        /// The login application user and set session variables.
        /// </summary>
        public void LoginApplicationUserAndSetSessionVariables()
        {
            // Log in application user.
            this.LoginApplicationUser(null, null);
           // SessionRevision = TaxonDataSourceTestRepositoryData.GetTaxonRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestRevisionId);
        }
        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                this.LogoutApplicationUser();

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

        ///// <summary>
        ///// Login test user. User datasource is shimmed. Must be encapsulated in 
        ///// using (ShimsContext.Create()).
        ///// </summary>
        //public void LoginStubTestUser()
        //{

        //    UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.ShimUserDataSource()
        //    {
        //        LoginStringStringStringBoolean
        //            =
        //            (
        //                userName,
        //                password,
        //                applicationIdentifier,
        //                isActivationRequired
        //            )
        //            =>
        //            {
        //                return UserContext;
        //            },
        //    };
        //    CoreData.UserManager.DataSource = userDataSource;
        //    LoginTestUser();
        //}
    }
}
