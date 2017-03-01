using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class AuthorizationManagerTest : TestBase
    {
        private AuthorizationManager _authorizationManager;

        public AuthorizationManagerTest()
        {
            _authorizationManager = null;
        }

        [TestMethod]
        public void CheckAuthorization()
        {
            AuthorityIdentifier authorityIdentifier;

            authorityIdentifier = AuthorityIdentifier.WebServiceAdministrator;
            GetAuthorizationManager(true).CheckAuthorization(GetContext(ApplicationIdentifier.UserAdmin), authorityIdentifier);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CheckAuthorizationError()
        {
            AuthorityIdentifier authorityIdentifier;

            authorityIdentifier = AuthorityIdentifier.WebServiceAdministrator;
            GetAuthorizationManager(true).CheckAuthorization(GetContext(ApplicationIdentifier.PrintObs), authorityIdentifier);
        }

        [TestMethod]
        public void Constructor()
        {
            AuthorizationManager authorizationManager;

            authorizationManager = new AuthorizationManager();
            Assert.IsNotNull(authorizationManager);
        }

        private AuthorizationManager GetAuthorizationManager()
        {
            return GetAuthorizationManager(false);
        }

        private AuthorizationManager GetAuthorizationManager(Boolean refresh)
        {
            if (_authorizationManager.IsNull() || refresh)
            {
                _authorizationManager = new AuthorizationManager();
            }
            return _authorizationManager;
        }
    }
}
