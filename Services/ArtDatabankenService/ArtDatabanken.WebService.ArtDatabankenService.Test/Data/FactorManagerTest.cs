using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
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

        [TestMethod]
        public void AddUserSelectedFactors()
        {
            foreach (UserSelectedFactorUsage factorUsage in Enum.GetValues(typeof(UserSelectedFactorUsage)))
            {
                FactorManager.AddUserSelectedFactors(GetContext(), GetSomeFactorIds(), factorUsage);
                FactorManager.DeleteUserSelectedFactors(GetContext());

                // Should be ok to add zero factors.
                FactorManager.AddUserSelectedFactors(GetContext(), null, factorUsage);
                FactorManager.AddUserSelectedFactors(GetContext(), new List<Int32>(), factorUsage);
            }
        }

        private void CheckCircularTree(List<WebFactorTreeNode> factorTrees)
        {
            List<Int32> factorIds;

            if (factorTrees.IsNotEmpty())
            {
                foreach (WebFactorTreeNode factorTree in factorTrees)
                {
                    factorIds = new List<Int32>();
                    CheckCircularTree(factorTree, factorIds);
                }
            }
        }

        private void CheckCircularTree(WebFactorTreeNode factorTree,
                                       List<Int32> factorIds)
        {
            List<Int32> childFactorIds;

            if (factorTree.IsNotNull())
            {
                if (factorIds.Contains(factorTree.Id))
                {
                    // Circular factor tree found.
                    throw new Exception("Circular factor tree found! Factor id = " + factorTree.Id);
                }
                else
                {
                    factorIds.Add(factorTree.Id);
                }

                if (factorTree.Children.IsNotEmpty())
                {
                    foreach (WebFactorTreeNode child in factorTree.Children)
                    {
                        childFactorIds = new List<Int32>();
                        childFactorIds.AddRange(factorIds);
                        CheckCircularTree(child, childFactorIds);
                    }
                }
            }
        }

        [TestMethod]
        public void DeleteUserSelectedFactors()
        {
            foreach (UserSelectedFactorUsage factorUsage in Enum.GetValues(typeof(UserSelectedFactorUsage)))
            {
                FactorManager.AddUserSelectedFactors(GetContext(), GetSomeFactorIds(), factorUsage);
                FactorManager.DeleteUserSelectedFactors(GetContext());
            }

            // Should be ok to delete zero factors.
            FactorManager.DeleteUserSelectedFactors(GetContext());
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            List<WebFactorDataType> factorDataTypes;

            factorDataTypes = FactorManager.GetFactorDataTypes(GetContext());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            List<WebFactorFieldEnum> factorFieldEnums;

            factorFieldEnums = FactorManager.GetFactorFieldEnums(GetContext());
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (WebFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            List<WebFactorFieldType> factorFieldTypes;

            factorFieldTypes = FactorManager.GetFactorFieldTypes(GetContext());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            List<WebFactorOrigin> factorOrigins;

            factorOrigins = FactorManager.GetFactorOrigins(GetContext());
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactors()
        {
            List<WebFactor> factors;

            factors = FactorManager.GetFactors(GetContext());
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsById()
        {
            List<WebFactor> factors;
            List<Int32> factorIds;

            factorIds = GetSomeFactorIds();
            factors = FactorManager.GetFactorsById(GetContext(), factorIds);
            Assert.IsTrue(factors.IsNotEmpty());
            Assert.AreEqual(factors.Count, factorIds.Count);
            foreach (WebFactor factor in factors)
            {
                Assert.IsTrue(factorIds.Contains(factor.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorsByIdEmptyFactorIdsError()
        {
            List<WebFactor> factors;
            List<Int32> factorIds;

            factorIds = new List<Int32>();
            factors = FactorManager.GetFactorsById(GetContext(), factorIds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFactorsByIdNullFactorIdsError()
        {
            List<WebFactor> factors;

            factors = FactorManager.GetFactorsById(GetContext(), null);
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            List<WebFactor> factors;
            List<Int32> factorIds;
            Int32 count1, count2;
            WebFactorSearchCriteria searchCriteria;

            searchCriteria = new WebFactorSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(LANDSCAPES_FACTOR_ID);

            searchCriteria.NameSearchString = null;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(factors.IsNull() || factors.IsEmpty());

            searchCriteria.NameSearchString = "Rödli%";
            searchCriteria.NameSearchMethod = SearchStringComparisonMethod.Like;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());

            searchCriteria.NameSearchString = "Rödli";
            searchCriteria.NameSearchMethod = SearchStringComparisonMethod.Iterative;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(factors.IsNotEmpty());

            searchCriteria.NameSearchString = "Landskapstyper";
            searchCriteria.NameSearchMethod = SearchStringComparisonMethod.Exact;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            count1 = factors.Count;
            Assert.IsNotNull(count1);
            Assert.IsTrue(count1 > 0);

            searchCriteria.NameSearchString = LANDSCAPES_FACTOR_ID.ToString();
            searchCriteria.NameSearchMethod = SearchStringComparisonMethod.Like;
            searchCriteria.IsIdInNameSearchString = true;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            count2 = factors.Count;
            Assert.IsNotNull(count2);
            Assert.IsTrue(count2 > 0);
            Assert.AreEqual(count1, count2);

            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            searchCriteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            Assert.AreEqual(factors.Count, 10);

            foreach (FactorSearchScope factorSearchScope in Enum.GetValues(typeof(FactorSearchScope)))
            {
                searchCriteria = new WebFactorSearchCriteria();
                searchCriteria.RestrictSearchToFactorIds = factorIds;
                searchCriteria.RestrictReturnToScope = factorSearchScope;
                factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
                Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFactorsBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebFactor> factors;

            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), null);
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            List<WebFactorTreeNode> factorTrees;

            factorTrees = FactorManager.GetFactorTrees(GetContext());
            Assert.IsTrue(factorTrees.IsNotEmpty());
            CheckCircularTree(factorTrees);
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteria()
        {
            List<Int32> factorIds;
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria;

            factorIds = new List<Int32>();
            factorIds.Add(LANDSCAPES_FACTOR_ID);
            searchCriteria = new WebFactorTreeSearchCriteria();

            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());

            searchCriteria.RestrictSearchToFactorIds = factorIds;

            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.AreEqual(factorTrees[0].Children.Count, 9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFactorTreesBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebFactorTreeNode> factorTrees;

            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), null);
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            List<WebFactorUpdateMode> factorUpdateModes;

            factorUpdateModes = FactorManager.GetFactorUpdateModes(GetContext());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        public static WebFactor GetForestFactor(WebServiceContext context)
        {
            Int32 factorId;
            List<Int32> factorIds;
            WebFactorSearchCriteria searchCriteria;

            factorId = FOREST_LANSCAPE_FACTOR_ID;
            factorIds = new List<Int32>();
            factorIds.Add(factorId);
            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;

            return FactorManager.GetFactorsBySearchCriteria(context, searchCriteria)[0];
        }

        public static WebFactorTreeNode GetForestFactorTreeNode(WebServiceContext context)
        {
            List<Int32> factorIds;
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria;

            searchCriteria = new WebFactorTreeSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(FOREST_LANSCAPE_FACTOR_ID);
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(context, searchCriteria);
            return factorTrees[0];
        }

        public static WebFactor GetOneFactor(WebServiceContext context)
        {
            foreach (WebFactor factor in FactorManager.GetFactors(context))
            {
                if (factor.Id == LANDSCAPE_FOREST_FACTOR_ID)
                {
                    return factor;
                }
            }
            return null;
        }

        public static WebFactorDataType GetOneFactorDataType(WebServiceContext context)
        {
            return FactorManager.GetFactorDataTypes(context)[0];
        }

        public static WebFactorField GetOneFactorField(WebServiceContext context)
        {
            return GetOneFactorDataType(context).Fields[0];
        }

        public static WebFactorField GetOneFactorFieldBoolean(WebServiceContext context)
        {
            List<WebFactorDataType> factorDataTypes;
            WebFactorDataType factorDataType = null;
            WebFactorField factorField = null;

            factorDataTypes = FactorManager.GetFactorDataTypes(context);
            foreach (WebFactorDataType tempFactorDataType in factorDataTypes)
            {
                if (tempFactorDataType.Id == 30)
                {
                    factorDataType = tempFactorDataType;
                }
            }
            foreach (WebFactorField tempFactorField in factorDataType.Fields)
            {
                if (tempFactorField.DatabaseFieldName == "tal2")
                {
                    factorField = tempFactorField;
                }
            }
            return factorField;
        }

        public static WebFactorField GetOneFactorFieldInt32(WebServiceContext context)
        {
            List<WebFactorDataType> factorDataTypes;
            WebFactorDataType factorDataType = null;
            WebFactorField factorField = null;

            factorDataTypes = FactorManager.GetFactorDataTypes(context);
            foreach (WebFactorDataType tempFactorDataType in factorDataTypes)
            {
                if (tempFactorDataType.Id == 30)
                {
                    factorDataType = tempFactorDataType;
                }
            }
            foreach (WebFactorField tempFactorField in factorDataType.Fields)
            {
                if (tempFactorField.DatabaseFieldName == "tal1")
                {
                    factorField = tempFactorField;
                }
            }
            return factorField;
        }

        public static WebFactorField GetOneFactorFieldString(WebServiceContext context)
        {
            List<WebFactorDataType> factorDataTypes;
            WebFactorDataType factorDataType = null;
            WebFactorField factorField = null;

            factorDataTypes = FactorManager.GetFactorDataTypes(context);
            foreach (WebFactorDataType tempFactorDataType in factorDataTypes)
            {
                if (tempFactorDataType.Id == 15)
                {
                    factorDataType = tempFactorDataType;
                }
            }
            foreach (WebFactorField tempFactorField in factorDataType.Fields)
            {
                if (tempFactorField.DatabaseFieldName == "text1")
                {
                    factorField = tempFactorField;
                }
            }
            return factorField;
        }

        public static WebFactorFieldEnum GetOneFactorFieldEnum(WebServiceContext context)
        {
            return FactorManager.GetFactorFieldEnums(context)[0];
        }

        public static WebFactorFieldEnumValue GetOneFactorFieldEnumValue(WebServiceContext context)
        {
            return GetOneFactorFieldEnum(context).Values[0];
        }

        public static WebFactorFieldType GetOneFactorFieldType(WebServiceContext context)
        {
            return FactorManager.GetFactorFieldTypes(context)[0];
        }

        public static WebFactorOrigin GetOneFactorOrigin(WebServiceContext context)
        {
            return FactorManager.GetFactorOrigins(context)[0];
        }

        public static WebFactorUpdateMode GetOneFactorUpdateMode(WebServiceContext context)
        {
            return FactorManager.GetFactorUpdateModes(context)[0];
        }

        public static List<Int32> GetRedlistFactorIds(WebServiceContext context)
        {
            List<Int32> factorIds;
            List<WebFactor> factors;

            factorIds = new List<Int32>();
            factorIds.Add(REDLIST_FACTOR_ID);

            WebFactorSearchCriteria searchCriteria;

            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            searchCriteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;
            factors = FactorManager.GetFactorsBySearchCriteria(context, searchCriteria);
            foreach (WebFactor factor in factors)
            {
                factorIds.Add(factor.Id);
            }
            return factorIds;
        }

        public static WebFactorTreeNode GetRedlistFactorTreeNode(WebServiceContext context)
        {
            List<Int32> factorIds;
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria;

            searchCriteria = new WebFactorTreeSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(REDLIST_FACTOR_ID);
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(context, searchCriteria);
            return factorTrees[0];
        }

        public static List<Int32> GetSomeFactorIds()
        {
            return GetSomeFactorIds(4);
        }

        public static List<Int32> GetSomeFactorIds(Int32 factorIdCount)
        {
            List<Int32> factorIds;

            factorIds = new List<Int32>();
            if (factorIdCount > 0)
            {
                factorIds.Add(AGRICULTURAL_LANDSCAPE_FACTOR_ID);
            }
            if (factorIdCount > 1)
            {
                factorIds.Add(BIUS_FOREST_FACTOR_ID);
            }
            if (factorIdCount > 2)
            {
                factorIds.Add(FOREST_LANSCAPE_FACTOR_ID);
            }
            if (factorIdCount > 3)
            {
                factorIds.Add(LANDSCAPE_FACTOR_ID);
            }
            return factorIds;
        }

        public static WebFactorFieldType[] GetSomeFactorFieldTypes(WebServiceContext context,
                                                                   Int32 factorFieldTypeCount)
        {
            Int32 typeIndex;
            List<WebFactorFieldType> allFactorFieldTypes;
            WebFactorFieldType[] factorFieldTypes;

            allFactorFieldTypes = FactorManager.GetFactorFieldTypes(context);
            if (factorFieldTypeCount > allFactorFieldTypes.Count)
            {
                factorFieldTypeCount = allFactorFieldTypes.Count;
            }
            factorFieldTypes = new WebFactorFieldType[factorFieldTypeCount];
            for (typeIndex = 0; typeIndex < factorFieldTypeCount; typeIndex++)
            {
                factorFieldTypes[typeIndex] = allFactorFieldTypes[typeIndex];
            }
            return factorFieldTypes;
        }

        public static List<WebFactor> GetSomeFactors(WebServiceContext context)
        {
            List<WebFactor> factors;

            factors = new List<WebFactor>();
            foreach (WebFactor factor in FactorManager.GetFactors(context))
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

        [TestMethod]
        public void GetUserSelectedFactorsTable()
        {
            DataTable userSelectedFactorsTable;
            List<Int32> factorIds;

            factorIds = GetSomeFactorIds();
            foreach (UserSelectedFactorUsage factorUsage in Enum.GetValues(typeof(UserSelectedFactorUsage)))
            {
                userSelectedFactorsTable = FactorManager.GetUserSelectedFactorsTable(GetContext(),
                                                                                     factorIds,
                                                                                     factorUsage);
                Assert.IsNotNull(userSelectedFactorsTable);
                Assert.IsTrue(userSelectedFactorsTable.Rows.IsNotEmpty());
                Assert.AreEqual(factorIds.Count, userSelectedFactorsTable.Rows.Count);
            }
        }
    }
}
