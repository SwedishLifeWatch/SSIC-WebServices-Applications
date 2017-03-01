using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebClientTokenTest : TestBase
    {
        private static WebClientToken mClientToken;

        public WebClientTokenTest()
        {
            mClientToken = null;
        }

        [TestMethod]
        public void CheckData()
        {
            WebClientToken clientToken;

            clientToken = new WebClientToken(Settings.Default.TestUserName, Settings.Default.TestClientApplicationName, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
            clientToken = new WebClientToken(Settings.Default.TestUserName, Settings.Default.TestClientApplicationName, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataClientIpAddressToLongError()
        {
            CipherString cipherString;
            String token;
            WebClientToken clientToken;

            token = WebClientToken.GetRandomText() + WebService.Settings.Default.ClientTokenDelimiter +
                    DateTime.Now.WebToString() + WebService.Settings.Default.ClientTokenDelimiter +
                    Settings.Default.TestUserName + WebService.Settings.Default.ClientTokenDelimiter +
                    Settings.Default.TestPassword + WebService.Settings.Default.ClientTokenDelimiter +
                    42 + WebService.Settings.Default.ClientTokenDelimiter +
                    GetString(WebService.Settings.Default.IPv6MaxStringLength + 1) + WebService.Settings.Default.ClientTokenDelimiter +
                    WebServiceData.WebServiceManager.Name + WebService.Settings.Default.ClientTokenDelimiter +
                    WebClientToken.GetRandomText();
            cipherString = new CipherString();
            token = cipherString.EncryptText(token);
            clientToken = new WebClientToken(token, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CheckDataSessionIdFormatError()
        {
            CipherString cipherString;
            String token;
            WebClientToken clientToken;

            token = WebClientToken.GetRandomText() + WebService.Settings.Default.ClientTokenDelimiter +
                    DateTime.Now.WebToString() + WebService.Settings.Default.ClientTokenDelimiter +
                    Settings.Default.TestUserName + WebService.Settings.Default.ClientTokenDelimiter +
                    Settings.Default.TestClientApplicationName + WebService.Settings.Default.ClientTokenDelimiter +
                    "Hej" + WebService.Settings.Default.ClientTokenDelimiter +
                    WebServiceContext.GetClientIpAddress() + WebService.Settings.Default.ClientTokenDelimiter +
                    WebServiceData.WebServiceManager.Name + WebService.Settings.Default.ClientTokenDelimiter +
                    WebClientToken.GetRandomText();
            cipherString = new CipherString();
            token = cipherString.EncryptText(token);
            clientToken = new WebClientToken(token, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        public void ClientIpAddress()
        {
            Assert.IsTrue(GetWebClientToken(true).ClientIpAddress.IsNotEmpty());
        }

        [TestMethod]
        public void Constructor()
        {
            WebClientToken clientToken;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             Settings.Default.TestClientApplicationName,
                                             WebServiceData.WebServiceManager.Key);
            Assert.IsNotNull(clientToken);

            clientToken = new WebClientToken(GetWebClientToken(true).Token, WebServiceData.WebServiceManager.Key);
            Assert.IsNotNull(clientToken);
        }

        [TestMethod]
        public void CreatedDate()
        {
            DateTime createdDate;

            createdDate = GetWebClientToken(true).CreatedDate;
            Assert.IsTrue(createdDate <= DateTime.Now);
        }

        [TestMethod]
        public void GetRandomText()
        {
            Int32 index;
            String randomText;

            for (index = 0; index < 20; index++)
            {
                randomText = WebClientToken.GetRandomText();
                Assert.IsTrue(randomText.IsNotEmpty());
                Assert.IsFalse(randomText.Contains(ArtDatabanken.WebService.Settings.Default.ClientTokenDelimiter.ToString()));
            }
        }

        private static WebClientToken GetWebClientToken(Boolean refresh = false)
        {
            if (mClientToken.IsNull() || refresh)
            {
                mClientToken = new WebClientToken(Settings.Default.TestUserName, Settings.Default.TestClientApplicationName, WebServiceData.WebServiceManager.Key);
            }

            return mClientToken;
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
            CipherString cipherString;
            WebClientToken clientToken; 

            token = "Hej hopp i lingon skogen!";
            cipherString = new CipherString();
            token = cipherString.EncryptText(token);
            clientToken = new WebClientToken(token, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TokenNullError()
        {
            String token;
            CipherString cipherString;
            WebClientToken clientToken;

            cipherString = new CipherString();
            token = cipherString.EncryptText(null);
            clientToken = new WebClientToken(token, WebServiceData.WebServiceManager.Key);
            clientToken.CheckData();
        }

        [TestMethod]
        public void UserName()
        {
            Assert.IsTrue(GetWebClientToken(true).UserName.IsNotEmpty());
        }
    }
}
