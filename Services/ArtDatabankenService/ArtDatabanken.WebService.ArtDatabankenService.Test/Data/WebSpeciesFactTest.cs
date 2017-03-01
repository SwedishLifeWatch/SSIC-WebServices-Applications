using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WbSpeciesFactTest
    /// </summary>
    [TestClass]
    public class WebSpeciesFactTest : TestBase
    {
        private WebSpeciesFact _speciesFact;

        public WebSpeciesFactTest()
        {
            _speciesFact = null;
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void FactorId()
        {
            Int32 factorId;

            factorId = 42;
            GetSpeciesFact(true).FactorId = factorId;
            Assert.AreEqual(GetSpeciesFact().FactorId, factorId);
        }

        [TestMethod]
        public void FieldValue1()
        {
            Double fieldValue;

            fieldValue = 42.42;
            GetSpeciesFact(true).FieldValue1 = fieldValue;
            Assert.AreEqual(GetSpeciesFact().FieldValue1, fieldValue);
        }

        [TestMethod]
        public void FieldValue2()
        {
            Double fieldValue;

            fieldValue = 42.42;
            GetSpeciesFact(true).FieldValue2 = fieldValue;
            Assert.AreEqual(GetSpeciesFact().FieldValue2, fieldValue);
        }

        [TestMethod]
        public void FieldValue3()
        {
            Double fieldValue;

            fieldValue = 42.42;
            GetSpeciesFact(true).FieldValue3 = fieldValue;
            Assert.AreEqual(GetSpeciesFact().FieldValue3, fieldValue);
        }

        [TestMethod]
        public void FieldValue4()
        {
            String fieldValue;

            fieldValue = "42.42";
            GetSpeciesFact(true).FieldValue4 = fieldValue;
            Assert.AreEqual(GetSpeciesFact().FieldValue4, fieldValue);
        }

        [TestMethod]
        public void FieldValue5()
        {
            String fieldValue;

            fieldValue = "42.42";
            GetSpeciesFact(true).FieldValue5 = fieldValue;
            Assert.AreEqual(GetSpeciesFact().FieldValue5, fieldValue);
        }

        [TestMethod]
        public void GetField4MaxLength()
        {
            Int32 maxStringLength;

            maxStringLength = WebSpeciesFact.GetField4MaxLength(GetContext());
            Assert.IsTrue(0 < maxStringLength);
        }

        [TestMethod]
        public void GetField5MaxLength()
        {
            Int32 maxStringLength;

            maxStringLength = WebSpeciesFact.GetField5MaxLength(GetContext());
            Assert.IsTrue(0 < maxStringLength);
        }

        private WebSpeciesFact GetSpeciesFact()
        {
            return GetSpeciesFact(false);
        }

        private WebSpeciesFact GetSpeciesFact(Boolean refresh)
        {
            if (_speciesFact.IsNull() || refresh)
            {
                _speciesFact = SpeciesFactManagerTest.GetOneSpeciesFact(GetContext());

            }
            return _speciesFact;
        }

        [TestMethod]
        public void GetUpdateUserFullNameMaxLength()
        {
            Int32 maxStringLength;

            maxStringLength = WebSpeciesFact.GetUpdateUserFullNameMaxLength(GetContext());
            Assert.IsTrue(0 < maxStringLength);
        }

        [TestMethod]
        public void HasFieldValue1()
        {
            Boolean hasFieldValue;

            hasFieldValue = true;
            GetSpeciesFact(true).IsFieldValue1Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue1Specified, hasFieldValue);

            hasFieldValue = false;
            GetSpeciesFact().IsFieldValue1Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue1Specified, hasFieldValue);
        }

        [TestMethod]
        public void HasFieldValue2()
        {
            Boolean hasFieldValue;

            hasFieldValue = true;
            GetSpeciesFact(true).IsFieldValue2Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue2Specified, hasFieldValue);

            hasFieldValue = false;
            GetSpeciesFact().IsFieldValue2Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue2Specified, hasFieldValue);
        }

        [TestMethod]
        public void HasFieldValue3()
        {
            Boolean hasFieldValue;

            hasFieldValue = true;
            GetSpeciesFact(true).IsFieldValue3Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue3Specified, hasFieldValue);

            hasFieldValue = false;
            GetSpeciesFact().IsFieldValue3Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue3Specified, hasFieldValue);
        }

        [TestMethod]
        public void HasFieldValue4()
        {
            Boolean hasFieldValue;

            hasFieldValue = true;
            GetSpeciesFact(true).IsFieldValue4Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue4Specified, hasFieldValue);

            hasFieldValue = false;
            GetSpeciesFact().IsFieldValue4Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue4Specified, hasFieldValue);
        }

        [TestMethod]
        public void HasFieldValue5()
        {
            Boolean hasFieldValue;

            hasFieldValue = true;
            GetSpeciesFact(true).IsFieldValue5Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue5Specified, hasFieldValue);

            hasFieldValue = false;
            GetSpeciesFact().IsFieldValue5Specified = hasFieldValue;
            Assert.AreEqual(GetSpeciesFact().IsFieldValue5Specified, hasFieldValue);
        }

        [TestMethod]
        public void HasHost()
        {
            Boolean hasHost;

            hasHost = true;
            GetSpeciesFact(true).IsHostSpecified = hasHost;
            Assert.AreEqual(GetSpeciesFact().IsHostSpecified, hasHost);

            hasHost = false;
            GetSpeciesFact().IsHostSpecified = hasHost;
            Assert.AreEqual(GetSpeciesFact().IsHostSpecified, hasHost);
        }

        [TestMethod]
        public void HasPeriod()
        {
            Boolean hasPeriod;

            hasPeriod = true;
            GetSpeciesFact(true).IsPeriodSpecified = hasPeriod;
            Assert.AreEqual(GetSpeciesFact().IsPeriodSpecified, hasPeriod);

            hasPeriod = false;
            GetSpeciesFact().IsPeriodSpecified = hasPeriod;
            Assert.AreEqual(GetSpeciesFact().IsPeriodSpecified, hasPeriod);
        }

        [TestMethod]
        public void HostId()
        {
            Int32 hostId;

            hostId = 42;
            GetSpeciesFact(true).HostId = hostId;
            Assert.AreEqual(GetSpeciesFact().HostId, hostId);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetSpeciesFact(true).Id = id;
            Assert.AreEqual(GetSpeciesFact().Id, id);
        }

        [TestMethod]
        public void IndividualCategoryId()
        {
            Int32 individualCategoryId;

            individualCategoryId = 42;
            GetSpeciesFact(true).IndividualCategoryId = individualCategoryId;
            Assert.AreEqual(GetSpeciesFact().IndividualCategoryId, individualCategoryId);
        }

        [TestMethod]
        public void PeriodId()
        {
            Int32 periodId;

            periodId = 42;
            GetSpeciesFact(true).PeriodId = periodId;
            Assert.AreEqual(GetSpeciesFact().PeriodId, periodId);
        }

        [TestMethod]
        public void QualityId()
        {
            Int32 qualityId;

            qualityId = 42;
            GetSpeciesFact(true).QualityId = qualityId;
            Assert.AreEqual(GetSpeciesFact().QualityId, qualityId);
        }

        [TestMethod]
        public void ReferenceId()
        {
            Int32 referenceId;

            referenceId = 42;
            GetSpeciesFact(true).ReferenceId = referenceId;
            Assert.AreEqual(GetSpeciesFact().ReferenceId, referenceId);
        }

        [TestMethod]
        public void TaxonId()
        {
            Int32 taxonId;

            taxonId = 42;
            GetSpeciesFact(true).TaxonId = taxonId;
            Assert.AreEqual(GetSpeciesFact().TaxonId, taxonId);
        }

        [TestMethod]
        public void UpdateDate()
        {
            DateTime updateDate;

            updateDate = DateTime.Now;
            GetSpeciesFact(true).UpdateDate = updateDate;
            Assert.AreEqual(GetSpeciesFact().UpdateDate, updateDate);
        }

        [TestMethod]
        public void UpdateUserFullName()
        {
            String updateUserFullname;

            updateUserFullname = "Oskar";
            GetSpeciesFact(true).UpdateUserFullName = updateUserFullname;
            Assert.AreEqual(GetSpeciesFact().UpdateUserFullName, updateUserFullname);
        }
    }
}
