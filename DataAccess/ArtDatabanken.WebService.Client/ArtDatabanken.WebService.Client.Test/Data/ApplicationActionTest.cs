using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ApplicationActionTest : TestBase
    {
        ApplicationAction _application;

        public ApplicationActionTest()
        {
            _application = null;
        }

        [TestMethod]
        public void Constructor()
        {
            ApplicationAction application;

            application = new ApplicationAction(GetUserContext());
            Assert.IsNotNull(application);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetApplicationAction(true).DataContext);
        }

        public static ApplicationAction GetOneApplicationAction(IUserContext userContext)
        {
            return new ApplicationAction(userContext);
        }

        private ApplicationAction GetApplicationAction()
        {
            return GetApplicationAction(false);
        }

        private ApplicationAction GetApplicationAction(Boolean refresh)
        {
            if (_application.IsNull() || refresh)
            {
                _application = new ApplicationAction(GetUserContext());
            }
            return _application;
        }

        [TestMethod]
        public void ActionIdentity()
        {
            String applicationIdentity;

            applicationIdentity = null;
            GetApplicationAction(true).Identifier = applicationIdentity;
            Assert.IsNull(GetApplicationAction().Identifier);

            applicationIdentity = "";
            GetApplicationAction().Identifier = applicationIdentity;
            Assert.AreEqual(GetApplicationAction().Identifier, applicationIdentity);

            applicationIdentity = @"ActionIdentity";
            GetApplicationAction().Identifier = applicationIdentity;
            Assert.AreEqual(GetApplicationAction().Identifier, applicationIdentity);
        }
    }
}
