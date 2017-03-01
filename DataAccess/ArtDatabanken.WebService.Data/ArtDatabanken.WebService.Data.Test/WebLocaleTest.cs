using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebLocale
    /// </summary>
    [TestClass]
    public class WebLocaleTest
    {
        private WebLocale _locale;

        public WebLocaleTest()
        {
            _locale = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebLocale locale;

            locale = new WebLocale();
            Assert.IsNotNull(locale);
        }

        private WebLocale GetLocale()
        {
            return GetLocale(false);
        }

        private WebLocale GetLocale(Boolean refresh)
        {
            if (_locale.IsNull() || refresh)
            {
                _locale = new WebLocale();
            }
            return _locale;
        }




        [TestMethod]
        public void Id()
        {
            WebLocale locale = GetLocale();
            locale.Id = 1;
            Assert.AreEqual(locale.Id, 1);
        }

        [TestMethod]
        public void ISOCode()
        {
            WebLocale locale = GetLocale();
            locale.ISOCode = "Namn";
            Assert.AreEqual(locale.ISOCode, "Namn");
        }

        [TestMethod]
        public void Name()
        {
            WebLocale locale = GetLocale();
            locale.Name = "Namn";
            Assert.AreEqual(locale.Name, "Namn");
        }

        [TestMethod]
        public void NativeName()
        {
            WebLocale locale = GetLocale();
            locale.NativeName = "Namn";
            Assert.AreEqual(locale.NativeName, "Namn");
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


    }
}
