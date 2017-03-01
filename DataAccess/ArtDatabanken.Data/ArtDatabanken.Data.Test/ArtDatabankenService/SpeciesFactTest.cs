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
    /// Summary description for SpeciesFactTest
    /// </summary>
    [TestClass]
    public class SpeciesFactTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesFact _specisFact;

        public SpeciesFactTest()
        {
            _specisFact = null;
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
        public void AllowAutomaticUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetSpeciesFact(true).AllowAutomaticUpdate;
        }

        [TestMethod]
        public void AllowManualUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetSpeciesFact(true).AllowManualUpdate;
        }

        [TestMethod]
        public void AllowUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetSpeciesFact(true).AllowUpdate;
        }

        [TestMethod]
        public void Field1()
        {
            if (GetSpeciesFact(true).HasField1)
            {
                Assert.IsNotNull(GetSpeciesFact().Field1);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field1);
            }
        }

        [TestMethod]
        public void Field2()
        {
            if (GetSpeciesFact(true).HasField2)
            {
                Assert.IsNotNull(GetSpeciesFact().Field2);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field2);
            }
        }

        [TestMethod]
        public void Field3()
        {
            if (GetSpeciesFact(true).HasField3)
            {
                Assert.IsNotNull(GetSpeciesFact().Field3);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field3);
            }
        }

        [TestMethod]
        public void Field4()
        {
            if (GetSpeciesFact(true).HasField4)
            {
                Assert.IsNotNull(GetSpeciesFact().Field4);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field4);
            }
        }

        [TestMethod]
        public void Field5()
        {
            if (GetSpeciesFact(true).HasField5)
            {
                Assert.IsNotNull(GetSpeciesFact().Field5);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field5);
            }
        }

        private Data.ArtDatabankenService.SpeciesFact GetSpeciesFact()
        {
            return GetSpeciesFact(false);
        }

        private Data.ArtDatabankenService.SpeciesFact GetSpeciesFact(Boolean refresh)
        {
            if (_specisFact.IsNull() || refresh)
            {
                _specisFact = SpeciesFactManagerTest.GetASpeciesFact();
            }
            return _specisFact;
        }

        [TestMethod]
        public void HasChanged()
        {
            Data.ArtDatabankenService.Reference oldReference;
            Data.ArtDatabankenService.SpeciesFactQuality oldQuality;

            oldQuality = GetSpeciesFact(true).Quality;
            oldReference = GetSpeciesFact().Reference;

            // Test no change.
            Assert.IsFalse(GetSpeciesFact().HasChanged);

            // Test change of quality.
            if (GetSpeciesFact().AllowManualUpdate)
            {
                GetSpeciesFact().Quality = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQualities()[3];
                Assert.IsTrue(GetSpeciesFact().HasChanged);

                // Restore old quality value.
                GetSpeciesFact().Quality = oldQuality;
                Assert.IsFalse(GetSpeciesFact().HasChanged);

                // Test change of reference.
                GetSpeciesFact().Reference = Data.ArtDatabankenService.ReferenceManager.GetReferences()[4];
                Assert.IsTrue(GetSpeciesFact().HasChanged);

                // Restore old reference value.
                GetSpeciesFact().Reference = oldReference;
                Assert.IsFalse(GetSpeciesFact().HasChanged);
            }
        }

        [TestMethod]
        public void HasField1()
        {
            if (GetSpeciesFact(true).HasField1)
            {
                Assert.IsNotNull(GetSpeciesFact().Field1);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field1);
            }
        }

        [TestMethod]
        public void HasField2()
        {
            if (GetSpeciesFact(true).HasField2)
            {
                Assert.IsNotNull(GetSpeciesFact().Field2);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field2);
            }
        }

        [TestMethod]
        public void HasField3()
        {
            if (GetSpeciesFact(true).HasField3)
            {
                Assert.IsNotNull(GetSpeciesFact().Field3);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field3);
            }
        }

        [TestMethod]
        public void HasField4()
        {
            if (GetSpeciesFact(true).HasField4)
            {
                Assert.IsNotNull(GetSpeciesFact().Field4);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field4);
            }
        }

        [TestMethod]
        public void HasField5()
        {
            if (GetSpeciesFact(true).HasField5)
            {
                Assert.IsNotNull(GetSpeciesFact().Field5);
            }
            else
            {
                Assert.IsNull(GetSpeciesFact().Field5);
            }
        }

        [TestMethod]
        public void Id()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsNotNull(speciesFact.Id);
            Assert.IsTrue(speciesFact.HasId);
        }

        [TestMethod]
        public void Identifier()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            String speciesFactIdentifier = String.Empty;

            speciesFactIdentifier =
                Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactIdentifier(
                speciesFact.Taxon,
                speciesFact.IndividualCategory,
                speciesFact.Factor,
                null,
                null);

            Assert.AreEqual(speciesFact.Identifier, speciesFactIdentifier);

            speciesFactIdentifier =
                Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactIdentifier(
                speciesFact.Taxon.Id,
                speciesFact.IndividualCategory.Id,
                speciesFact.Factor.Id,
                true,
                1,
                false,
                0);

            Assert.AreNotEqual(speciesFact.Identifier, speciesFactIdentifier);

        }

        [TestMethod]
        public void Fields()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.Fields.Count > 0);
        }

        [TestMethod]
        public void SubstantialFields()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.SubstantialFields.Count > 0);
        }

        [TestMethod]
        public void MainField()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.MainField.Label.IsNotEmpty());
            Assert.IsTrue(speciesFact.MainField.IsMain);
        }

        [TestMethod]
        public void Taxon()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.Taxon.ScientificName.IsNotEmpty());
        }

        [TestMethod]
        public void IndividualCategory()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.IndividualCategory.Id == 0);
            Assert.IsTrue(speciesFact.IndividualCategory.Name.IsNotEmpty());
        }

        [TestMethod]
        public void Factor()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.Factor.Label.IsNotEmpty());
        }

        [TestMethod]
        public void Host()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.AreEqual(speciesFact.HasHost, speciesFact.Host.IsNotNull());
        }

        [TestMethod]
        public void Period()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.AreEqual(speciesFact.HasPeriod, speciesFact.Period.IsNotNull());
        }

        [TestMethod]
        public void Reference()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.Reference.Name.IsNotEmpty());
        }

        [TestMethod]
        public void ShouldBeDeleted()
        {
            Int32 fieldIndex;

            GetSpeciesFact(true);
            for (fieldIndex = 0; fieldIndex < GetSpeciesFact().Fields.Count; fieldIndex++)
            {
                switch (GetSpeciesFact().Fields[fieldIndex].FactorField.Type.DataType)
                {
                    case FactorFieldDataTypeId.Boolean:
                        GetSpeciesFact().Fields[fieldIndex].BooleanValue = true;
                        break;
                    case FactorFieldDataTypeId.Double:
                        GetSpeciesFact().Fields[fieldIndex].DoubleValue = 5.5;
                        break;
                    case FactorFieldDataTypeId.Enum:
                        GetSpeciesFact().Fields[fieldIndex].EnumValue = GetSpeciesFact().Fields[fieldIndex].FactorField.FactorFieldEnum.Values[1];
                        break;
                    case FactorFieldDataTypeId.Int32:
                        GetSpeciesFact().Fields[fieldIndex].Int32Value = 4;
                        break;
                    case FactorFieldDataTypeId.String:
                        GetSpeciesFact().Fields[fieldIndex].StringValue = "Hej";
                        break;
                }
            }
            Assert.IsFalse(GetSpeciesFact().ShouldBeDeleted);

            for (fieldIndex = 0; fieldIndex < GetSpeciesFact().Fields.Count; fieldIndex++)
            {
                GetSpeciesFact().Fields[fieldIndex].Value = null;
            }
            Assert.IsTrue(GetSpeciesFact().ShouldBeDeleted);
        }

        [TestMethod]
        public void ShouldBeSaved()
        {
            Boolean shouldBeSaved;

            shouldBeSaved = GetSpeciesFact(true).ShouldBeSaved;
        }

        [TestMethod]
        public void UpdateUser()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact = GetSpeciesFact(true);
            Assert.IsTrue(speciesFact.UpdateUserFullName.IsNotEmpty());
            Assert.IsNotNull(speciesFact.UpdateDate);
        }
    }
}
