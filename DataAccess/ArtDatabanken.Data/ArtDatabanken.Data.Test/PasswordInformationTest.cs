using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class PasswordInformationTest
    {
        PasswordInformation _passwordInformation;

        public PasswordInformationTest()
        {
            _passwordInformation = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PasswordInformation passwordInformation;

            passwordInformation = new PasswordInformation(Settings.Default.TestUserName,
                                                          Settings.Default.TestEmailAddress,
                                                          Settings.Default.TestPassword,
                                                          DataContextTest.GetOneDataContext());
            Assert.IsNotNull(passwordInformation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDataContextNullError()
        {
            PasswordInformation passwordInformation;
            IDataContext dataContext;

            dataContext = null;
            passwordInformation = new PasswordInformation(Settings.Default.TestUserName,
                                                          Settings.Default.TestEmailAddress,
                                                          Settings.Default.TestPassword,
                                                          dataContext);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetPasswordInformation(true).DataContext);
        }

        private PasswordInformation GetPasswordInformation()
        {
            return GetPasswordInformation(false);
        }

        private PasswordInformation GetPasswordInformation(Boolean refresh)
        {
            if (_passwordInformation.IsNull() || refresh)
            {
                _passwordInformation = new PasswordInformation(Settings.Default.TestUserName,
                                                               Settings.Default.TestEmailAddress,
                                                               Settings.Default.TestPassword,
                                                               DataContextTest.GetOneDataContext());
            }
            return _passwordInformation;
        }

        [TestMethod]
        public void EmailAddress()
        {
            Assert.IsTrue(GetPasswordInformation(true).EmailAddress.IsNotEmpty());
        }

        [TestMethod]
        public void Password()
        {
            Assert.IsTrue(GetPasswordInformation(true).Password.IsNotEmpty());
        }

        [TestMethod]
        public void UserName()
        {
            Assert.IsTrue(GetPasswordInformation(true).UserName.IsNotEmpty());
        }
    }
}
