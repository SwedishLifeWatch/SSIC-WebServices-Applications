using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebOrganizationCategory
    /// </summary>
    [TestClass]
    public class WebOrganizationCategoryTest
    {
        private WebOrganizationCategory _organizationCategory;

        public WebOrganizationCategoryTest()
        {
            _organizationCategory = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebOrganizationCategory organizationCategory;
            organizationCategory = new WebOrganizationCategory();
            Assert.IsNotNull(organizationCategory);
        }

        private WebOrganizationCategory GetOrganizationCategory()
        {
            if (_organizationCategory.IsNull())
            {
                _organizationCategory = new WebOrganizationCategory();
            }
            return _organizationCategory;
        }
        

        [TestMethod]
        public void Id()
        {
            GetOrganizationCategory().Id = 2;
            Assert.AreEqual(GetOrganizationCategory().Id, 2);
        }

        [TestMethod]
        public void DescriptionStringId()
        {

            GetOrganizationCategory().DescriptionStringId = 2;
            Assert.AreEqual(GetOrganizationCategory().DescriptionStringId, 2);

        }

        [TestMethod]
        public void Name()
        {

            String value = "Test description";
            GetOrganizationCategory().Description = value;
            Assert.AreEqual(GetOrganizationCategory().Description, value);

        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Int32 administrationRoleId = 99;
            GetOrganizationCategory().AdministrationRoleId = administrationRoleId;
            Assert.AreEqual(administrationRoleId, GetOrganizationCategory().AdministrationRoleId);
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

