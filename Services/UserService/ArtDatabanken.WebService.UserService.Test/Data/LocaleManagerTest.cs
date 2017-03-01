using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test
{
    [TestClass]
    public class LocaleManagerTest : TestBase
    {
        public LocaleManagerTest()
        {
        }

        [TestMethod]
        public void GetLocale()
        {
            WebLocale testLocale;

            // Test get locale by id.
            foreach (WebLocale locale in LocaleManager.GetLocales(GetContext()))
            {
                testLocale = LocaleManager.GetLocale(GetContext(), locale.Id);
                Assert.AreEqual(testLocale.Id, locale.Id);
                Assert.AreEqual(testLocale.ISOCode, locale.ISOCode);
            }

            // Test get locale by ISO code.
            foreach (WebLocale locale in LocaleManager.GetLocales(GetContext()))
            {
                testLocale = LocaleManager.GetLocale(GetContext(), locale.ISOCode);
                Assert.AreEqual(testLocale.Id, locale.Id);
                Assert.AreEqual(testLocale.ISOCode, locale.ISOCode);
            }
        }

        [TestMethod]
        public void GetLocales()
        {
            List<WebLocale> locales;

            locales = LocaleManager.GetLocales(GetContext());
            Assert.IsTrue(locales.IsNotEmpty());
        }
    }
}
