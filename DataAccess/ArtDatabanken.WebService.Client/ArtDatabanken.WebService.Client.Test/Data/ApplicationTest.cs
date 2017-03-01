using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ApplicationVersionTest : TestBase
    {
        ApplicationVersion _applicationVersion;

        public ApplicationVersionTest()
        {
            _applicationVersion = null;
        }

        [TestMethod]
        public void Constructor()
        {
            ApplicationVersion applicationVersion;

            applicationVersion = new ApplicationVersion(GetUserContext());
            Assert.IsNotNull(applicationVersion);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetApplicationVersion(true).DataContext);
        }

        public static ApplicationVersion GetOneApplicationVersion(IUserContext userContext)
        {
            return new ApplicationVersion(userContext);
        }

        private ApplicationVersion GetApplicationVersion()
        {
            return GetApplicationVersion(false);
        }

        private ApplicationVersion GetApplicationVersion(Boolean refresh)
        {
            if (_applicationVersion.IsNull() || refresh)
            {
                _applicationVersion = new ApplicationVersion(GetUserContext());
            }
            return _applicationVersion;
        }

        [TestMethod]
        public void Version()
        {
            String version;

            version = null;
            GetApplicationVersion(true).Version = version;
            Assert.IsNull(GetApplicationVersion().Version);

            version = "";
            GetApplicationVersion().Version = version;
            Assert.AreEqual(GetApplicationVersion().Version, version);

            version = "2.0";
            GetApplicationVersion().Version = version;
            Assert.AreEqual(GetApplicationVersion().Version, version);
        }
    }
}
