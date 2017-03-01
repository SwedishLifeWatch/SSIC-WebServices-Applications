using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class LocaleManagerTest : TestBase
    {
        private LocaleManager _localeManager;

        public LocaleManagerTest()
        {
            _localeManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            LocaleManager localeManager;

            localeManager = new LocaleManager();
            Assert.IsNotNull(localeManager);
        }

        [TestMethod]
        public void DataSource()
        {
            ILocaleDataSource dataSource;

            dataSource = null;
            GetLocaleManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetLocaleManager().DataSource);

            dataSource = new LocaleDataSource();
            GetLocaleManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetLocaleManager().DataSource);
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetLocaleManager(true).GetDataSourceInformation());
        }

        [TestMethod]
        public void GetDefaultLocale()
        {
            ILocale locale;

            locale = GetLocaleManager(true).GetDefaultLocale(GetUserContext());
            Assert.IsNotNull(locale);
        }

        [TestMethod]
        public void GetLocale()
        {
            ILocale locale;

            foreach (ILocale testLocale in GetLocaleManager(true).GetLocales(GetUserContext()))
            {
                locale = GetLocaleManager().GetLocale(GetUserContext(), testLocale.ISOCode);
                Assert.AreEqual(testLocale.Id, locale.Id);

                locale = GetLocaleManager().GetLocale(GetUserContext(), testLocale.Id);
                Assert.AreEqual(testLocale.Id, locale.Id);
            }

            locale = GetLocaleManager().GetLocale(GetUserContext(), "en");
            Assert.IsNotNull(locale);
            Assert.IsTrue(locale.ISOCode.ToLower().StartsWith("en"));

            foreach (LocaleId localeId in Enum.GetValues(typeof(LocaleId)))
            {
                locale = GetLocaleManager().GetLocale(GetUserContext(), localeId);
                Assert.IsNotNull(locale);
                Assert.AreEqual((Int32)(localeId), locale.Id);
            }
        }

        private LocaleManager GetLocaleManager()
        {
            return GetLocaleManager(false);
        }

        private LocaleManager GetLocaleManager(Boolean refresh)
        {
            if (_localeManager.IsNull() || refresh)
            {
                _localeManager = new LocaleManager();
                _localeManager.DataSource = new LocaleDataSource();
            }
            return _localeManager;
        }

        [TestMethod]
        public void GetLocales()
        {
            LocaleList locales;

            locales = GetLocaleManager(true).GetLocales(GetUserContext());
            Assert.IsTrue(locales.IsNotEmpty());
        }

        [TestMethod]
        public void GetUsedLocales()
        {
            LocaleList locales;

            locales = GetLocaleManager(true).GetUsedLocales(GetUserContext());
            Assert.IsTrue(locales.IsNotEmpty());
        }
    }
}
