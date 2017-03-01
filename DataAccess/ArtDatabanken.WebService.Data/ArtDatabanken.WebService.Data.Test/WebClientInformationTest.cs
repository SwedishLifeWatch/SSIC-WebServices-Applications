using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebClientInformationTest
    {
        private WebClientInformation _clientInformation;

        public WebClientInformationTest()
        {
            _clientInformation = null;
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void Constructor()
        {
            WebClientInformation clientInformation;

            clientInformation = new WebClientInformation();
            Assert.IsNotNull(clientInformation);
        }

        private WebClientInformation GetClientInformation()
        {
            return GetClientInformation(false);
        }

        private WebClientInformation GetClientInformation(Boolean refresh)
        {
            if (_clientInformation.IsNull() || refresh)
            {
                _clientInformation = new WebClientInformation();
            }
            return _clientInformation;
        }

        [TestMethod]
        public void Locale()
        {
            WebLocale locale = new WebLocale();

           
            GetClientInformation(true).Locale = locale;
            Assert.AreEqual(locale, GetClientInformation().Locale);

        }

        [TestMethod]
        public void Token()
        {
            String token;

            token = null;
            GetClientInformation(true).Token = token;
            Assert.AreEqual(token, GetClientInformation().Token);

            token = "Hej";
            GetClientInformation(true).Token = token;
            Assert.AreEqual(token, GetClientInformation().Token);
        }
    }
}
