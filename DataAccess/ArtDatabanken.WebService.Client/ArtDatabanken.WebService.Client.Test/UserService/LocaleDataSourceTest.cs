using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.Test.UserService
{
    [TestClass]
    public class LocaleDataSourceTest : TestBase
    {
        private LocaleDataSource _localeDataSource;

        public LocaleDataSourceTest()
        {
            _localeDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            LocaleDataSource localeDataSource;

            localeDataSource = new LocaleDataSource();
            Assert.IsNotNull(localeDataSource);
        }

        private LocaleDataSource GetLocaleDataSource()
        {
            return GetLocaleDataSource(false);
        }

        private LocaleDataSource GetLocaleDataSource(Boolean refresh)
        {
            if (_localeDataSource.IsNull() || refresh)
            {
                _localeDataSource = new LocaleDataSource();
            }
            return _localeDataSource;
        }

        [TestMethod]
        public void GetLocales()
        {
            LocaleList locales;

            locales = GetLocaleDataSource(true).GetLocales(GetUserContext());
            Assert.IsTrue(locales.IsNotEmpty());
        }
    }
}
