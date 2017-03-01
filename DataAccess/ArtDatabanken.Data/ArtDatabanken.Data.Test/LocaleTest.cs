using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class LocaleTest
    {
        private Locale _locale;

        public LocaleTest()
        {
            _locale = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Locale locale;

            locale = new Locale(Settings.Default.SwedishLocaleId,
                                Settings.Default.SwedishLocaleISOCode,
                                Settings.Default.SwedishLocaleName,
                                Settings.Default.SwedishLocaleNativeName,
                                new DataContext(new DataSourceInformation(), null));
            Assert.IsNotNull(locale);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDataContextNullError()
        {
            Locale locale;
            IDataContext dataContext;

            dataContext = null;
            locale = new Locale(Settings.Default.SwedishLocaleId,
                                Settings.Default.SwedishLocaleISOCode,
                                Settings.Default.SwedishLocaleName,
                                Settings.Default.SwedishLocaleNativeName,
                                dataContext);
            Assert.IsNotNull(locale);
        }

        [TestMethod]
        public void CultureInfo()
        {
            Assert.IsNotNull(GetLocale(true).CultureInfo);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetLocale(true).DataContext);
        }

        private Locale GetLocale()
        {
            return GetLocale(false);
        }

        private Locale GetLocale(Boolean refresh)
        {
            if (_locale.IsNull() || refresh)
            {
                _locale = new Locale(Settings.Default.SwedishLocaleId,
                                     Settings.Default.SwedishLocaleISOCode,
                                     Settings.Default.SwedishLocaleName,
                                     Settings.Default.SwedishLocaleNativeName,
                                     new DataContext(new DataSourceInformation(), null));
            }
            return _locale;
        }

        public static ILocale GetOneLocale()
        {
            return new Locale(Settings.Default.SwedishLocaleId,
                              Settings.Default.SwedishLocaleISOCode,
                              Settings.Default.SwedishLocaleName,
                              Settings.Default.SwedishLocaleNativeName,
                              new DataContext(new DataSourceInformation(), null));
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetLocale(true).Id;
            Assert.AreEqual(id, GetLocale().Id);
        }

        [TestMethod]
        public void ISOCode()
        {
            String ISOCode;

            ISOCode = GetLocale(true).ISOCode;
            Assert.AreEqual(ISOCode, GetLocale().ISOCode);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = GetLocale(true).Name;
            Assert.AreEqual(name, GetLocale().Name);
        }

        [TestMethod]
        public void NativeName()
        {
            String nativeName;

            nativeName = GetLocale(true).NativeName;
            Assert.AreEqual(nativeName, GetLocale().NativeName);
        }
    }
}
