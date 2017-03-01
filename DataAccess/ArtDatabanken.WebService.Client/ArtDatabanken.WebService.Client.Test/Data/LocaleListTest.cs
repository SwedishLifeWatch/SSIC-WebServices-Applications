using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class LocaleListTest : TestBase
    {
        private LocaleList _locales;

        public LocaleListTest()
        {
            _locales = null;
        }

        [TestMethod]
        public void Constructor()
        {
            LocaleList locales;

            locales = new LocaleList();
            Assert.IsNotNull(locales);
        }

        [TestMethod]
        public void Get()
        {
            ILocale locale;

            foreach (ILocale tempLocale in GetLocales(true))
            {
                Assert.AreEqual(tempLocale, GetLocales().Get(tempLocale.Id));
                Assert.AreEqual(tempLocale, GetLocales().Get(tempLocale.ISOCode));
            }

            locale = GetLocales().Get("en");
            Assert.IsNotNull(locale);

            locale = GetLocales().Get("");
            Assert.IsNull(locale);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 localeId;

            localeId = Int32.MinValue;
            GetLocales(true).Get(localeId);
        }

        [TestMethod]
        public void GetIndex()
        {
            Int32 index;
            LocaleList locales;

            locales = GetLocales(true);
            for (index = 0; index < locales.Count; index++)
            {
                Assert.AreEqual(index, locales.GetIndex(locales[index]));
            }
        }

        [TestMethod]
        public void GetIsoCodeError()
        {
            ILocale locale;
            String isoCode;

            isoCode = "Hoppsan";
            locale = GetLocales(true).Get(isoCode);
            Assert.IsNull(locale);
        }

        private LocaleList GetLocales()
        {
            return GetLocales(false);
        }

        private LocaleList GetLocales(Boolean refresh)
        {
            if (_locales.IsNull() || refresh)
            {
                _locales = CoreData.LocaleManager.GetLocales(GetUserContext());
            }
            return _locales;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            LocaleList newLocaleList, oldLocaleList;
            Int32 localeIndex;

            oldLocaleList = GetLocales(true);
            newLocaleList = new LocaleList();
            for (localeIndex = 0; localeIndex < oldLocaleList.Count; localeIndex++)
            {
                newLocaleList.Add(oldLocaleList[oldLocaleList.Count - localeIndex - 1]);
            }
            for (localeIndex = 0; localeIndex < oldLocaleList.Count; localeIndex++)
            {
                Assert.AreEqual(newLocaleList[localeIndex], oldLocaleList[oldLocaleList.Count - localeIndex - 1]);
            }
        }
    }
}
