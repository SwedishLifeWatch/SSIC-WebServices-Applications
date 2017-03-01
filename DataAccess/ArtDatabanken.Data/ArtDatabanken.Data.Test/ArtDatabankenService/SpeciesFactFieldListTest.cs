using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for SpeciesFieldListTest
    /// </summary>
    [TestClass]
    public class SpeciesFactFieldListTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesFactFieldList _speciesFactFields;

        public SpeciesFactFieldListTest()
        {
            _speciesFactFields = null;
        }

        #region Additional test attributes
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
        public void Get()
        {
            foreach (Data.ArtDatabankenService.SpeciesFactField speciesFactField in GetSpeciesFactFields())
            {
                Assert.AreEqual(speciesFactField, GetSpeciesFactFields().Get(speciesFactField.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 speciesFactFieldId;

            speciesFactFieldId = Int32.MinValue;
            GetSpeciesFactFields().Get(speciesFactFieldId);
        }

        private Data.ArtDatabankenService.SpeciesFactFieldList GetSpeciesFactFields()
        {
            if (_speciesFactFields.IsNull())
            {
                _speciesFactFields = SpeciesFactManagerTest.GetASpeciesFact().Fields;
            }
            return _speciesFactFields;
        }
    }
}
