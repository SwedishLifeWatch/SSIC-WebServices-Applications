using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class CoreDataTest
    {
        public CoreDataTest()
        {
        }

        [TestMethod]
        public void CountryManager()
        {
            Assert.IsNotNull(CoreData.CountryManager);

            CoreData.CountryManager = new CountryManager();
            Assert.IsNotNull(CoreData.CountryManager);

            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            Assert.IsNotNull(CoreData.CountryManager);

            CoreData.CountryManager = new CountryManagerSingleThreadCache();
            Assert.IsNotNull(CoreData.CountryManager);
        }

        [TestMethod]
        public void LocaleManager()
        {
            Assert.IsNotNull(CoreData.LocaleManager);

            CoreData.LocaleManager = new LocaleManager();
            Assert.IsNotNull(CoreData.LocaleManager);

            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            Assert.IsNotNull(CoreData.LocaleManager);

            CoreData.LocaleManager = new LocaleManagerSingleThreadCache();
            Assert.IsNotNull(CoreData.LocaleManager);
        }

        [TestMethod]
        public void UserManager()
        {
            Assert.IsNotNull(CoreData.UserManager);

            CoreData.UserManager = new UserManager();
            Assert.IsNotNull(CoreData.UserManager);

            CoreData.UserManager = new UserManagerMultiThreadCache();
            Assert.IsNotNull(CoreData.UserManager);

            CoreData.UserManager = new UserManagerSingleThreadCache();
            Assert.IsNotNull(CoreData.UserManager);
        }
    }
}
