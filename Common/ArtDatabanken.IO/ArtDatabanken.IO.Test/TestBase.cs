using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManager = ArtDatabanken.Data.ArtDatabankenService.UserManager;

namespace ArtDatabanken.IO.Test
{
    using System.Threading;

    [TestClass]
    public class TestBase
    {
        private IUserContext _userContext;
        private String _applicationName;

        protected const Int32 BEAR_TAXON_ID = 100145;
        protected const Int32 BLACK_THROATED_LOON_TAXON_ID = 100065; // Storlom.
        protected const Int32 COMMON_BUZZARD_ID = 102942; // Ormvråk.
        protected const Int32 GOLDEN_EAGLE_TAXON_ID = 100011;
        protected const Int32 HAWK_BIRDS_TAXON_ID = 2002066;
        protected const Int32 NORTHERN_HAWK_OWL_TAXON_ID = 102620;
        protected const Int32 RED_FOX_TAXON_ID = 206026;
        protected const Int32 BADGER_TAXON_ID = 206036; // Grävling.
        protected const Int32 BEAVER_TAXON_ID = 102607; // Bäver.
        protected const Int32 HEDGEHOG_TAXON_ID = 100053; // Igelkott.
        protected const Int32 FALLOW_DEER_TAXON_ID = 206044; // Dovhjort.

        protected const Int32 FAMILY_TAXON_TYPE_ID = 11;
        protected const Int32 GENUS_TAXON_TYPE_ID = 14;
        protected const Int32 SPECIES_TAXON_TYPE_ID = 17;

        protected const Int32 LANDSCAPE_FACTOR_ID = 750;
        protected const Int32 LANDSCAPES_FACTOR_ID = 661;
        protected const Int32 LANDSCAPE_FOREST_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_AGRICULTURE_FACTOR_ID = 663;
        protected const Int32 LANDSCAPE_MOUNTAIN_FACTOR_ID = 665;
        protected const Int32 LANDSCAPE_FRESH_WATER_FACTOR_ID = 667;
        protected const Int32 HISTORIC_DECREASE_FACTOR_ID = 691;
        protected const Int32 BIUS_FOREST_FACTOR_ID = 2;

        protected const String TEST_CLIENT_APPLICATION_NAME = "WebServiceTest";
        protected const String TEST_CLIENT_APPLICATION_VERSION = "1.2.3.4";

        private static string _startupPath;

        public TestBase()
        {
            _userContext = null;
            _applicationName = GetTestApplicationName();
        }

        virtual protected String GetTestApplicationName()
        {
            return ApplicationIdentifier.EVA.ToString();
        }


        /// <summary>
        /// Gets the relative resource folder path.
        /// </summary>        
        protected static string RelativeResourceFolderPath
        {
            get { return "Resources"; }
        }

        /// <summary>
        /// Gets the absolute resource folder path.
        /// </summary>        
        protected static string AbsoluteResourceFolderPath
        {
            get
            {                
                return Path.Combine(_startupPath, RelativeResourceFolderPath).TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Gets the relative temporary folder path.
        /// The tests can write data in this folder.
        /// </summary>        
        protected static string RelativeTempFolderPath
        {
            get { return "TempFolder"; }
        }

        /// <summary>
        /// Gets the absolute temporary folder path.
        /// The tests can write data in this folder.
        /// </summary>        
        protected static string AbsoluteTempFolderPath
        {
            get
            {                
                return Path.Combine(_startupPath, RelativeTempFolderPath).TrimEnd('\\') + "\\";
            }
        }

        protected Boolean AreEqual(DataId data1, DataId data2)
        {
            if ((data1.IsNull() && data2.IsNotNull()) ||
                (data1.IsNotNull() && data2.IsNull()))
            {
                return false;
            }

            if (data1.IsNull() && data2.IsNull())
            {
                return true;
            }

            return (data1.Id == data2.Id) &&
                   (data1.GetType().Name == data2.GetType().Name);
        }

        protected Boolean RunAllTests
        {
            get { return false; }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            UserManager.Logout();

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

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            _startupPath = Path.GetFullPath(".\\");
            EnsureFolder(AbsoluteTempFolderPath);
        }

        protected IUserContext GetUserContext()
        {
            return _userContext;
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            UserManager.Login(Settings.Default.TestUserName, Settings.Default.TestPassword, _applicationName, false);

            // Set used datasources            
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();

            bool loginSuccess = false;
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
        public Boolean Login(String userName, String password)
        {
            _userContext = CoreData.UserManager.Login(userName, password, _applicationName);
            return (_userContext.IsNotNull());
        }

        /// <summary>
        /// Login test user.
        /// </summary>
        protected IUserContext LoginTestUser()
        {            
            return _userContext;
        }

        /// <summary>
        /// Ensures that the folder on disk exists.
        /// If it doesn't exist, the folder will be created.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        private static void EnsureFolder(string path)
        {            
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }
}
