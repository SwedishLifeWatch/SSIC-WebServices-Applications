using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebLoginResponseTest
    {
        private WebLoginResponse _loginResponse;

        public WebLoginResponseTest()
        {
            _loginResponse = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebLoginResponse loginResponse;

            loginResponse = new WebLoginResponse();
            Assert.IsNotNull(loginResponse);
        }

        private WebLoginResponse GetLoginResponse()
        {
            return GetLoginResponse(false);
        }

        private WebLoginResponse GetLoginResponse(Boolean refresh)
        {
            if (_loginResponse.IsNull() || refresh)
            {
                _loginResponse = new WebLoginResponse();
            }
            return _loginResponse;
        }

        [TestMethod]
        public void Locale()
        {
            WebLocale locale = new WebLocale();

            
            GetLoginResponse(true).Locale = locale;
            Assert.AreEqual(locale, GetLoginResponse().Locale);


        }

        [TestMethod]
        public void Token()
        {
            String token;

            token = null;
            GetLoginResponse(true).Token = token;
            Assert.AreEqual(token, GetLoginResponse().Token);

            token = String.Empty;
            GetLoginResponse().Token = token;
            Assert.AreEqual(token, GetLoginResponse().Token);

            token = "ADÖLKFDÖLFKÖSDL";
            GetLoginResponse().Token = token;
            Assert.AreEqual(token, GetLoginResponse().Token);
        }

        [TestMethod]
        public void UserAuthority()
        {
            List<WebRole> userAuthority;

            userAuthority = null;
            GetLoginResponse(true).Roles = userAuthority;
            Assert.AreEqual(userAuthority, GetLoginResponse().Roles);

            userAuthority = new List<WebRole>();
            GetLoginResponse().Roles = userAuthority;
            Assert.AreEqual(userAuthority, GetLoginResponse().Roles);
        }
    }
}
