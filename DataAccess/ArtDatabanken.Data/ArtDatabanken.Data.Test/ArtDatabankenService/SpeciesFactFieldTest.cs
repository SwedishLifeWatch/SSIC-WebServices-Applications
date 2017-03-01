using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using FactorFieldDataTypeId = ArtDatabanken.Data.ArtDatabankenService.FactorFieldDataTypeId;

    /// <summary>
    /// Summary description for SpeciesFactFieldTest
    /// </summary>
    [TestClass]
    public class SpeciesFactFieldTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesFactField _speciesFactField;

        public SpeciesFactFieldTest()
        {
            _speciesFactField = null;
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
        public void FactorFieldEnum()
        {
            Assert.IsNotNull(GetSpeciesFactField(true).FactorFieldEnum);
        }

        private Data.ArtDatabankenService.SpeciesFactField GetSpeciesFactField()
        {
            return GetSpeciesFactField(false);
        }

        private Data.ArtDatabankenService.SpeciesFactField GetSpeciesFactField(Boolean refresh)
        {
            if (_speciesFactField.IsNull() || refresh)
            {
                _speciesFactField = SpeciesFactManagerTest.GetASpeciesFactField();
            }
            return _speciesFactField;
        }

        [TestMethod]
        public void HasChanged()
        {
            Assert.IsFalse(GetSpeciesFactField(true).HasChanged);
        }

        [TestMethod]
        public void Information()
        {
            String information;

            information = GetSpeciesFactField(true).Information;
        }

        [TestMethod]
        public void IsMain()
        {
            Assert.IsTrue(GetSpeciesFactField(true).IsMain);
        }

        [TestMethod]
        public void IsSubstantial()
        {
            Assert.IsTrue(GetSpeciesFactField(true).IsSubstantial);
        }

        [TestMethod]
        public void Label()
        {
            Assert.IsTrue(GetSpeciesFactField(true).Label.IsNotEmpty());
        }

        [TestMethod]
        public void MaxSize()
        {
            Assert.IsTrue(GetSpeciesFactField(true).MaxSize > -1);
        }

        [TestMethod]
        public void SetBooleanValue()
        {
            //Check a field of boolean type
            Data.ArtDatabankenService.FactorField factorBooleanField = Data.ArtDatabankenService.FactorManager.GetFactorDataType(1).Field1;
            Assert.AreEqual(factorBooleanField.Type.DataType, FactorFieldDataTypeId.Boolean);
            Data.ArtDatabankenService.SpeciesFactField booleanField = new Data.ArtDatabankenService.SpeciesFactField(SpeciesFactManagerTest.GetASpeciesFact(),
                factorBooleanField, false, null);

            Assert.IsFalse(booleanField.HasValue);

            //Set value to true
            Boolean testValue = true;
            booleanField.Value = testValue;
            Assert.IsTrue(booleanField.HasValue);
            Assert.AreEqual(booleanField.Value, testValue);

            booleanField.Value = null;
            Assert.IsFalse(booleanField.HasValue);

            testValue = false;
            booleanField.Value = testValue;
            Assert.IsTrue(booleanField.HasValue);
            Assert.IsFalse((Boolean)booleanField.Value);
        }

        [TestMethod]
        public void SetStringValue()
        {
            //Check a field of string type
            Data.ArtDatabankenService.FactorField factorStringField = Data.ArtDatabankenService.FactorManager.GetFactorDataType(1).Field5;
            Assert.IsTrue(factorStringField.Type.Id == 2);
            Data.ArtDatabankenService.SpeciesFactField stringField = new Data.ArtDatabankenService.SpeciesFactField(
                SpeciesFactManagerTest.GetASpeciesFact(),
                factorStringField, false, null);

            Assert.IsFalse(stringField.HasValue);

            //Set value to true
            String testValue = "Hej";
            stringField.Value = testValue;
            Assert.IsTrue(stringField.HasValue);
            Assert.AreEqual(stringField.Value, testValue);

            stringField.Value = null;
            Assert.IsFalse(stringField.HasValue);

            testValue = "1";
            stringField.Value = testValue;
            Assert.IsTrue(stringField.HasValue);

            testValue = String.Empty;
            stringField.Value = testValue;
            Assert.IsFalse(stringField.HasValue);

            testValue = "Hej  ";
            stringField.Value = testValue;
            Assert.AreEqual("Hej  ", stringField.Value);
        }

        [TestMethod]
        public void Type()
        {
            Assert.IsNotNull(GetSpeciesFactField(true).Type);
            Assert.IsTrue(GetSpeciesFactField().Type.Name.IsNotEmpty());
        }

        [TestMethod]
        public void UnitLabel()
        {
            Assert.IsTrue(GetSpeciesFactField(true).UnitLabel.IsEmpty());
        }

        [TestMethod]
        public void Value()
        {
            String valueAsString = GetSpeciesFactField(true).Value.ToString();
            Assert.AreEqual(GetSpeciesFactField().Value.GetType(), typeof(FactorFieldEnumValue));
            Assert.IsNotNull(GetSpeciesFactField().Value);
            Assert.IsTrue(valueAsString.IsNotEmpty());
        }
    }
}
