using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ApplicationTest : TestBase
    {
        Application _application;

        public ApplicationTest()
        {
            _application = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Application application;

            application = new Application(GetUserContext());
            Assert.IsNotNull(application);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetApplication(true).DataContext);
        }

        public static Application GetOneApplication(IUserContext userContext)
        {
            return new Application(userContext);
        }

        private Application GetApplication()
        {
            return GetApplication(false);
        }

        private Application GetApplication(Boolean refresh)
        {
            if (_application.IsNull() || refresh)
            {
                _application = new Application(GetUserContext());
            }
            return _application;
        }

        [TestMethod]
        public void ApplicationIdentity()
        {
            String applicationIdentity;

            applicationIdentity = null;
            GetApplication(true).Identifier = applicationIdentity;
            Assert.IsNull(GetApplication().Identifier);

            applicationIdentity = "";
            GetApplication().Identifier = applicationIdentity;
            Assert.AreEqual(GetApplication().Identifier, applicationIdentity);

            applicationIdentity = Settings.Default.TestApplicationIdentifier;
            GetApplication().Identifier = applicationIdentity;
            Assert.AreEqual(GetApplication().Identifier, applicationIdentity);
        }
    }
}
