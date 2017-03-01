using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebClientTokenTest : TestBase
    {
        private static WebClientToken _clientToken;

        public WebClientTokenTest()
        {
            _clientToken = null;
        }

        [TestMethod]
        public void CheckData()
        {
            WebClientToken clientToken;

            clientToken = new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataClientIPAddressToLongError()
        {
            String token;

            token = TEST_USER_NAME + Settings.Default.ClientTokenDelimitor +
                    ApplicationIdentifier + Settings.Default.ClientTokenDelimitor +
                    42 + Settings.Default.ClientTokenDelimitor +
                    GetString(WebClientToken.GetClientIPAddressMaxLength() + 1) + Settings.Default.ClientTokenDelimitor +
                    WebServiceData.WebServiceManager.Name;
            GetWebClientToken(token).CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CheckDataSessionIdFormatError()
        {
            String token;

            token = TEST_USER_NAME + Settings.Default.ClientTokenDelimitor +
                    ApplicationIdentifier + Settings.Default.ClientTokenDelimitor +
                    "Hej" + Settings.Default.ClientTokenDelimitor +
                    GetString(WebClientToken.GetClientIPAddressMaxLength()) + Settings.Default.ClientTokenDelimitor +
                    WebServiceData.WebServiceManager.Name;
            GetWebClientToken(token).CheckData();
        }

        [TestMethod]
        public void ClientIPAddress()
        {
            Assert.IsTrue(GetWebClientToken(true).ClientIPAddress.IsNotEmpty());
        }

        [TestMethod]
        public void Constructor()
        {
            WebClientToken clientToken;

            clientToken = new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key);
            Assert.IsNotNull(clientToken);

            clientToken = new WebClientToken(GetWebClientToken(true).Token, WebServiceData.WebServiceManager.Key);
            Assert.IsNotNull(clientToken);
        }

        [TestMethod]
        public void GetClientIPAddressMaxLength()
        {
            Int32 maxLength = 0;

            maxLength = WebClientToken.GetClientIPAddressMaxLength();
            Assert.IsTrue(0 < maxLength);
        }

        public WebClientToken GetWebClientToken()
        {
            return GetWebClientToken(false, null);
        }

        public WebClientToken GetWebClientToken(Boolean refresh)
        {
            return GetWebClientToken(refresh, null);
        }

        public WebClientToken GetWebClientToken(String token)
        {
            return GetWebClientToken(false, token);
        }

        public WebClientToken GetWebClientToken(Boolean refresh, String token)
        {
            CipherString cipherString;

            if (_clientToken.IsNull() || refresh || token.IsNotEmpty())
            {
                if (token.IsEmpty())
                {
                    _clientToken = new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key);
                }
                else
                {
                    cipherString = new CipherString();
                    token = cipherString.EncryptText(token);
                    _clientToken = new WebClientToken(token, WebServiceData.WebServiceManager.Key);
                }
            }
            return _clientToken;
        }

        [TestMethod]
        public void SessionId()
        {
            Assert.IsTrue(GetWebClientToken(true).SessionId >= 0);
        }

        [TestMethod]
        public void Token()
        {
            Assert.IsTrue(GetWebClientToken(true).Token.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TokenFormatError()
        {
            String token;

            token = "Hej hopp i lingon skogen!";
            GetWebClientToken(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TokenNullError()
        {
            WebClientToken clientToken;

            clientToken = new WebClientToken(null, WebServiceData.WebServiceManager.Key);
        }

        [TestMethod]
        public void UserName()
        {
            Assert.IsTrue(GetWebClientToken(true).UserName.IsNotEmpty());
        } 
    }
}
