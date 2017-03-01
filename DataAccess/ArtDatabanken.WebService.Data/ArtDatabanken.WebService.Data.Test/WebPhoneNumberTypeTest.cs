using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebPhoneNumberType
    /// </summary>
    [TestClass]
    public class WebPhoneNumberTypeTest
    {
        private WebPhoneNumberType _phoneNumberType;

        public WebPhoneNumberTypeTest()
        {
            _phoneNumberType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebPhoneNumberType phoneNumberType;

            phoneNumberType = new WebPhoneNumberType();

            Assert.IsNotNull(phoneNumberType);
        }

        private WebPhoneNumberType GetPhoneNumberType()
        {
            if (_phoneNumberType.IsNull())
            {
                _phoneNumberType = new WebPhoneNumberType();
            }
            return _phoneNumberType;
        }



        [TestMethod]
        public void Id()
        {

            GetPhoneNumberType().Id = 2;
            Assert.AreEqual(GetPhoneNumberType().Id, 2);

        }

        [TestMethod]
        public void NameStringId()
        {

            GetPhoneNumberType().NameStringId = 2;
            Assert.AreEqual(GetPhoneNumberType().NameStringId, 2);

        }

        [TestMethod]
        public void Name()
        {

            GetPhoneNumberType().Name = "Mobile";
            Assert.AreEqual(GetPhoneNumberType().Name, "Mobile");

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
