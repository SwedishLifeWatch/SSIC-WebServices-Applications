using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class FactorManagerTest : TestBase
    {

        public FactorManagerTest()
        {
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

        public static Data.ArtDatabankenService.FactorUpdateMode GetHeaderFactorUpdateMode()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactorUpdateMode(0);
        }

        public static FactorField GetOneFactorFieldBoolean(out Factor factor)
        {
            factor = null;
            return null;
        }

        [TestMethod]
        public void GetFactorUpdateMode()
        {
            Data.ArtDatabankenService.FactorUpdateMode factorUpdateMode;

            // Get factor update mode type by Int32 id.
            {
                Int32 factorUpdateModeId;

                factorUpdateModeId = 0;
                factorUpdateMode = Data.ArtDatabankenService.FactorManager.GetFactorUpdateMode(factorUpdateModeId);
                Assert.IsNotNull(factorUpdateMode);
                Assert.AreEqual(factorUpdateModeId, factorUpdateMode.Id);


            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorUpdateModeIdError()
        {
            Int32 factorUpdateModeId;

            factorUpdateModeId = Int32.MinValue;
            Data.ArtDatabankenService.FactorManager.GetFactorUpdateMode(factorUpdateModeId);
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            Data.ArtDatabankenService.FactorUpdateModeList factorUpdateModes;

            factorUpdateModes = Data.ArtDatabankenService.FactorManager.GetFactorUpdateModes();
            Assert.IsNotNull(factorUpdateModes);
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }


        ////////////////////////////////////////////////////////////
        public static Data.ArtDatabankenService.FactorFieldEnum GetFirstFactorFieldEnum()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactorFieldEnum(1);
        }

        [TestMethod]
        public void GetFactorFieldEnum()
        {
            Data.ArtDatabankenService.FactorFieldEnum factorFieldEnum;

            // Get factor field enum type by Int32 id.
            {
                Int32 factorFieldEnumId;

                factorFieldEnumId = 1;
                factorFieldEnum = Data.ArtDatabankenService.FactorManager.GetFactorFieldEnum(factorFieldEnumId);
                Assert.IsNotNull(factorFieldEnum);
                Assert.AreEqual(factorFieldEnumId, factorFieldEnum.Id);


            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorFieldEnumIdError()
        {
            Int32 factorFieldEnumId;

            factorFieldEnumId = Int32.MinValue;
            Data.ArtDatabankenService.FactorManager.GetFactorFieldEnum(factorFieldEnumId);
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            Data.ArtDatabankenService.FactorFieldEnumList factorFieldEnums;

            factorFieldEnums = Data.ArtDatabankenService.FactorManager.GetFactorFieldEnums();
            Assert.IsNotNull(factorFieldEnums);
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
        }

        /////////////////
        public static Data.ArtDatabankenService.FactorFieldType GetFirstFactorFieldType()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactorFieldType(0);
        }

        [TestMethod]
        public void GetFactorFieldType()
        {
            Data.ArtDatabankenService.FactorFieldType factorFieldType;

            // Get factor field type by Int32 id.
            {
                Int32 factorFieldTypeId;

                factorFieldTypeId = 0;
                factorFieldType = Data.ArtDatabankenService.FactorManager.GetFactorFieldType(factorFieldTypeId);
                Assert.IsNotNull(factorFieldType);
                Assert.AreEqual(factorFieldTypeId, factorFieldType.Id);


            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorFieldTypeIdError()
        {
            Int32 factorFieldTypeId;

            factorFieldTypeId = Int32.MinValue;
            Data.ArtDatabankenService.FactorManager.GetFactorFieldType(factorFieldTypeId);
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            Data.ArtDatabankenService.FactorFieldTypeList factorFieldTypes;

            factorFieldTypes = Data.ArtDatabankenService.FactorManager.GetFactorFieldTypes();
            Assert.IsNotNull(factorFieldTypes);
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        /////////////////
        public static Data.ArtDatabankenService.FactorField GetOneFactorField()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactorDataTypes()[0].Fields[0];
        }

        /////////////////
        public static Data.ArtDatabankenService.FactorDataType GetFirstFactorDataType()
        {

            return Data.ArtDatabankenService.FactorManager.GetFactorDataType(1);
        }

        [TestMethod]
        public void GetFactorDataType()
        {
            Data.ArtDatabankenService.FactorDataType factorDataType;

            // Get factor Data type by Int32 id.
            {
                Int32 factorDataTypeId;

                factorDataTypeId = 1;
                factorDataType = Data.ArtDatabankenService.FactorManager.GetFactorDataType(factorDataTypeId);
                Assert.IsNotNull(factorDataType);
                Assert.AreEqual(factorDataTypeId, factorDataType.Id);


            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorDataTypeIdError()
        {
            Int32 factorDataTypeId;

            factorDataTypeId = Int32.MinValue;
            Data.ArtDatabankenService.FactorManager.GetFactorDataType(factorDataTypeId);
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            Data.ArtDatabankenService.FactorDataTypeList factorDataTypes;

            factorDataTypes = Data.ArtDatabankenService.FactorManager.GetFactorDataTypes();
            Assert.IsNotNull(factorDataTypes);
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
        }

        /////////////////
        public static Data.ArtDatabankenService.Factor GetFirstFactor()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactor(985);
        }

        [TestMethod]
        public void GetFactor()
        {
            Data.ArtDatabankenService.Factor factor;

            // Get factor by Int32 id.
            {
                Int32 factorId;

                factorId = 1;
                factor = Data.ArtDatabankenService.FactorManager.GetFactor(factorId);
                Assert.IsNotNull(factor);
                Assert.AreEqual(factorId, factor.Id);


            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorIdError()
        {
            Int32 factorId;

            factorId = Int32.MinValue;
            Data.ArtDatabankenService.FactorManager.GetFactor(factorId);
        }

        [TestMethod]
        public void GetFactors()
        {
            Data.ArtDatabankenService.FactorList factors;

            factors = Data.ArtDatabankenService.FactorManager.GetFactors();
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFactorBySearchCriteriaNullError()
        {
            Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(null);
        }


        public static List<Int32> GetFactorIds()
        {
            List<Int32> factorIds;

            factorIds = new List<int>();
            factorIds.Add(LANDSCAPES_FACTOR_ID);
            factorIds.Add(LANDSCAPE_FACTOR_ID);
            factorIds.Add(BIUS_FOREST_FACTOR_ID);

            return factorIds;
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            Data.ArtDatabankenService.FactorList factors;
            Int32 count1, count2;
            List<Int32> factorIds;
            Data.ArtDatabankenService.FactorSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
            searchCriteria.FactorNameSearchString = null;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsTrue(factors.IsNull() || factors.IsEmpty());

            searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
            searchCriteria.FactorNameSearchString = "Rödli%";
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Like;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());

            searchCriteria.FactorNameSearchString = "Landskapstyper";
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Exact;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsTrue(factors.IsNotEmpty());
            count1 = factors.Count;
            Assert.IsNotNull(count1);
            Assert.IsTrue(count1 > 0);

            searchCriteria.FactorNameSearchString = LANDSCAPES_FACTOR_ID.ToString();
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Like;
            searchCriteria.IdInNameSearchString = true;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            count2 = factors.Count;
            Assert.IsNotNull(count2);
            Assert.IsTrue(count2 > 0);
            Assert.AreEqual(count1, count2);

            searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
            searchCriteria.FactorNameSearchString = "Rödli";
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Iterative;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());

            searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = GetFactorIds();
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsNotNull(factors);
            Assert.IsTrue(factors.IsNotEmpty());
            Assert.AreEqual(factors.Count, 3);

            searchCriteria.RestrictReturnToScope = WebService.FactorSearchScope.AllChildFactors;
            factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
            Assert.AreEqual(46, factors.Count);

            factorIds = new List<Int32>();
            factorIds.Add(LANDSCAPES_FACTOR_ID);
            foreach (WebService.FactorSearchScope factorSearchScope in Enum.GetValues(typeof(WebService.FactorSearchScope)))
            {
                searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
                searchCriteria.RestrictSearchToFactorIds = factorIds;
                searchCriteria.RestrictReturnToScope = factorSearchScope;
                factors = Data.ArtDatabankenService.FactorManager.GetFactorsBySearchCriteria(searchCriteria);
                Assert.IsFalse(factors.IsNull() || factors.IsEmpty());

            }

        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteria()
        {
            List<Int32> factorIds;
            Data.ArtDatabankenService.FactorTreeNodeList factorTrees;
            Data.ArtDatabankenService.FactorTreeSearchCriteria searchCriteria;

            factorIds = new List<Int32>();
            factorIds.Add(LANDSCAPES_FACTOR_ID);

            searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;

            factorTrees = Data.ArtDatabankenService.FactorManager.GetFactorTreesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Assert.AreEqual(factorTrees.Count, 1);

            // Test problem with factor 691.
            // The children to factor 691 are not shown.
            factorIds = new List<Int32>();
            factorIds.Add(HISTORIC_DECREASE_FACTOR_ID);
            searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = Data.ArtDatabankenService.FactorManager.GetFactorTreesBySearchCriteria(searchCriteria);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.IsTrue(factorTrees[0].Children.IsNotEmpty());
            Assert.AreEqual(factorTrees[0].Children.Count, 2);

            // Test problem with getting two factor trees 
            // where one is a subpart of the other.
            factorIds = new List<Int32>();
            factorIds.Add(985);
            factorIds.Add(1321);
            searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = Data.ArtDatabankenService.FactorManager.GetFactorTreesBySearchCriteria(searchCriteria);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Assert.AreEqual(2, factorTrees.Count);
            Assert.AreEqual(985, factorTrees[0].Id);
            Assert.AreEqual(1321, factorTrees[1].Id);
        }

        public static Data.ArtDatabankenService.FactorTreeNode GetForestFactorTreeNode()
        {
            List<Int32> factorIds;
            Data.ArtDatabankenService.FactorTreeNodeList factorTrees;
            Data.ArtDatabankenService.FactorTreeSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(BIUS_FOREST_FACTOR_ID);
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = Data.ArtDatabankenService.FactorManager.GetFactorTreesBySearchCriteria(searchCriteria);
            return factorTrees[0];
        }


        public static Data.ArtDatabankenService.FactorTreeNode GetLandscapeFactorTreeNode()
        {
            List<Int32> factorIds;
            Data.ArtDatabankenService.FactorTreeNodeList factorTrees;
            Data.ArtDatabankenService.FactorTreeSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(LANDSCAPE_FACTOR_ID);
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = Data.ArtDatabankenService.FactorManager.GetFactorTreesBySearchCriteria(searchCriteria);
            return factorTrees[0];
        }

        [TestMethod]
        public void GetFactorsById()
        {
            Boolean factorFound;
            Data.ArtDatabankenService.FactorList factors;

            factors = Data.ArtDatabankenService.FactorManager.GetFactors(GetFactorIds());
            Assert.IsNotNull(factors);
            Assert.AreEqual(factors.Count, GetFactorIds().Count);
            foreach (Factor factor in factors)
            {
                Assert.IsNotNull(factor.Label);

                factorFound = false;
                foreach (Int32 factorId in GetFactorIds())
                {
                    if (factorId == factor.Id)
                    {
                        factorFound = true;
                        break;
                    }
                }
                Assert.IsTrue(factorFound);

            }
        }

        public static Data.ArtDatabankenService.FactorOriginList GetFactorOrigins()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactorOrigins();
        }

        [TestMethod]
        public void getFactorOrigins()
        {
            Data.ArtDatabankenService.FactorOriginList factorOriginList;

            factorOriginList = Data.ArtDatabankenService.FactorManager.GetFactorOrigins();
            Assert.IsNotNull(factorOriginList);
            Assert.IsTrue(factorOriginList.IsNotEmpty());

        }

        public static Data.ArtDatabankenService.Factor GetOneFactor()
        {
            foreach (Data.ArtDatabankenService.Factor factor in Data.ArtDatabankenService.FactorManager.GetFactors())
            {
                if (factor.Id == LANDSCAPE_FOREST_FACTOR_ID)
                {
                    return factor;
                }
            }
            return null;
        }

        public static Data.ArtDatabankenService.FactorFieldEnumValue GetOneFactorFieldEnumValue()
        {
            return Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.ContinuingDecline).FactorDataType.Field1.FactorFieldEnum.Values[0];
        }

        public static FactorList GetSomeFactors()
        {
            FactorList factors;

            factors = new FactorList();
            foreach (Factor factor in Data.ArtDatabankenService.FactorManager.GetFactors())
            {
                if (factor.Id == LANDSCAPE_AGRICULTURE_FACTOR_ID)
                {
                    factors.Add(factor);
                }
                if (factor.Id == LANDSCAPE_FOREST_FACTOR_ID)
                {
                    factors.Add(factor);
                }
                if (factor.Id == LANDSCAPE_MOUNTAIN_FACTOR_ID)
                {
                    factors.Add(factor);
                }
                if (factor.Id == LANDSCAPE_FRESH_WATER_FACTOR_ID)
                {
                    factors.Add(factor);
                }
            }
            return factors;
        }

    }
}
