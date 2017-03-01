using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using FactorManager = ArtDatabanken.WebService.TaxonAttributeService.Data.FactorManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonAttributeService.Test.Data
{
    [TestClass]
    public class FactorManagerTest : TestBase
    {
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
        public void GetFactorOrigins()
        {
            List<WebFactorOrigin> factorOrigins;

            UseTransaction = true;
            factorOrigins = FactorManager.GetFactorOrigins(GetContext());
            Assert.IsTrue(factorOrigins.IsNotEmpty());

            UseTransaction = false;
            factorOrigins = FactorManager.GetFactorOrigins(GetContext());
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            List<WebFactorUpdateMode> factorUpdateModes;

            UseTransaction = true;
            factorUpdateModes = FactorManager.GetFactorUpdateModes(GetContext());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());

            UseTransaction = false;
            factorUpdateModes = FactorManager.GetFactorUpdateModes(GetContext());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            List<WebFactorFieldType> factorFieldTypes;

            UseTransaction = true;
            factorFieldTypes = FactorManager.GetFactorFieldTypes(GetContext());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());

            UseTransaction = false;
            factorFieldTypes = FactorManager.GetFactorFieldTypes(GetContext());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            List<WebPeriodType> periodTypes;

            UseTransaction = true;
            periodTypes = FactorManager.GetPeriodTypes(GetContext());
            Assert.IsTrue(periodTypes.IsNotEmpty());

            UseTransaction = false;
            periodTypes = FactorManager.GetPeriodTypes(GetContext());
            Assert.IsTrue(periodTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriods()
        {
            List<WebPeriod> periods;

            UseTransaction = true;
            periods = FactorManager.GetPeriods(GetContext());
            Assert.IsTrue(periods.IsNotEmpty());

            UseTransaction = false;
            periods = FactorManager.GetPeriods(GetContext());
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            List<WebIndividualCategory> individualCategories;

            UseTransaction = true;
            individualCategories = FactorManager.GetIndividualCategories(GetContext());
            Assert.IsTrue(individualCategories.IsNotEmpty());

            UseTransaction = false;
            individualCategories = FactorManager.GetIndividualCategories(GetContext());
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            List<WebFactorFieldEnum> factorFieldEnums;

            UseTransaction = true;
            factorFieldEnums = FactorManager.GetFactorFieldEnums(GetContext());
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (WebFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }

            UseTransaction = false;
            factorFieldEnums = FactorManager.GetFactorFieldEnums(GetContext());
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (WebFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            List<WebFactorDataType> factorDataTypes;

            UseTransaction = true;
            factorDataTypes = FactorManager.GetFactorDataTypes(GetContext());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (WebFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }

            UseTransaction = false;
            factorDataTypes = FactorManager.GetFactorDataTypes(GetContext());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (WebFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactors()
        {
            List<WebFactor> factors;

            UseTransaction = true;
            factors = FactorManager.GetFactors(GetContext());
            Assert.IsTrue(factors.IsNotEmpty());

            UseTransaction = false;
            factors = FactorManager.GetFactors(GetContext());
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            List<WebFactor> factors;
            List<Int32> factorIds;
            Int32 factorCount1, factorCount2;
            WebFactorSearchCriteria searchCriteria;

            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.NameSearchString = null;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(factors.IsNull() || factors.IsEmpty());

            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Rödli%";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());

            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Rödli";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(factors.IsNotEmpty());

            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Landskapstyper";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(factors.IsNull() || factors.IsEmpty());
            factorCount1 = factors.Count;
            Assert.IsNotNull(factorCount1);
            Assert.IsTrue(factorCount1 > 0);

            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "661";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.IsIdInNameSearchString = true;
            factors = FactorManager.GetFactorsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(factors.IsNotEmpty());
            factorCount2 = factors.Count;
            Assert.AreEqual(factorCount1, factorCount2);

            searchCriteria = new WebFactorSearchCriteria();
            factorIds = new List<Int32>();
            factorIds.Add(661);
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
        public void GetFactorTrees()
        {
            List<WebFactorTreeNode> factorTrees;

            UseTransaction = true;
            factorTrees = FactorManager.GetFactorTrees(GetContext());
            Assert.IsTrue(factorTrees.IsNotEmpty());

            UseTransaction = false;
            factorTrees = FactorManager.GetFactorTrees(GetContext());
            Assert.IsTrue(factorTrees.IsNotEmpty());
            CheckCircularTree(factorTrees);
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria()
        {
            List<Int32> factorIds = new List<Int32>();
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria();

            factorIds.Add(661);

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.AreEqual(factorTrees[0].Children.Count, 9);
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria_UserCanReadOnlyPublicFactors_GetPublicFactor_ExpectsFactorTree()
        {
            SetUserAndApplicationIdentifier("testUserPublic", ApplicationIdentifier.EVA.ToString());

            List<Int32> factorIds = new List<Int32>();
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria();

            factorIds.Add(1853);

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Console.WriteLine("factorTrees.Count: " + factorTrees.Count);
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.AreEqual(factorTrees[0].Children.Count, 7);
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria_UserCanReadOnlyPublicFactors_GetNonPublicFactor_ExpectsNoFactorTree()
        {
            SetUserAndApplicationIdentifier("testUserPublic", ApplicationIdentifier.EVA.ToString());

            List<Int32> factorIds = new List<Int32>();
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria();

            factorIds.Add(1872);

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsEmpty());
            Console.WriteLine("factorTrees.Count: " + factorTrees.Count);
            Assert.AreEqual(factorTrees.Count, 0);  
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria_UserCanReadAllFactors_GetPublicFactor_ExpectsFactorTree()
        {
            List<Int32> factorIds = new List<Int32>();
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria();

            factorIds.Add(1853);

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Console.WriteLine("factorTrees.Count: " + factorTrees.Count);
            Assert.AreEqual(factorTrees.Count, 1);
            Console.WriteLine("factorTrees[0].Children.Count: " + factorTrees[0].Children.Count);
            Assert.AreEqual(factorTrees[0].Children.Count, 9);
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria_UserCanReadAllFactors_GetNonPublicFactor_ExpectsFactorTree()
        {
            List<Int32> factorIds = new List<Int32>();
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria();

            factorIds.Add(1872);

            searchCriteria.FactorIds = factorIds;
            factorTrees = FactorManager.GetFactorTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Console.WriteLine("factorTrees.Count: " + factorTrees.Count);
            Assert.AreEqual(factorTrees.Count, 1);
            Console.WriteLine("factorTrees[0].Children.Count: " + factorTrees[0].Children.Count);
            Assert.AreEqual(factorTrees[0].Children.Count, 4);
        }
    }
}
