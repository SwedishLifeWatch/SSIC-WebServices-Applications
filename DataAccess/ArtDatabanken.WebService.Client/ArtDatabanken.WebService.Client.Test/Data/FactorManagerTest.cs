using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class FactorManagerTest : TestBase
    {
        private FactorManager _factorManager;

        public FactorManagerTest()
        {
            _factorManager = null;
        }

        [TestMethod]
        public void GetCurrentPublicPeriod()
        {
            IPeriod currentPublicPeriod;

            currentPublicPeriod = GetFactorManager().GetCurrentPublicPeriod(GetUserContext());
            Assert.IsTrue(currentPublicPeriod.IsNotNull());
        }

        [TestMethod]
        public void GetDefaultIndividualCategory()
        {
            IIndividualCategory individualCategory;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                individualCategory = GetFactorManager(true).GetDefaultIndividualCategory(GetUserContext());
                Assert.IsTrue(individualCategory.IsNotNull());
                Assert.AreEqual((Int32)(IndividualCategoryId.Default), individualCategory.Id);
            }

            individualCategory = GetFactorManager().GetDefaultIndividualCategory(GetUserContext());
            Assert.IsTrue(individualCategory.IsNotNull());
            Assert.AreEqual((Int32)(IndividualCategoryId.Default), individualCategory.Id);
        }

        [TestMethod]
        public void GetDefaultOrganismGroups()
        {
            OrganismGroupList organismGroups;

            organismGroups = GetFactorManager(true).GetDefaultOrganismGroups(GetUserContext());
            Assert.IsTrue(organismGroups.IsNotEmpty());
            Assert.AreEqual(OrganismGroupType.Standard, organismGroups[0].Type);
        }

        private FactorManager GetFactorManager(Boolean refresh = false)
        {
            if (_factorManager.IsNull() || refresh)
            {
                _factorManager = new FactorManager();
                _factorManager.DataSource = new FactorDataSource();
            }

            return _factorManager;
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            FactorOriginList factorOrigins;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorOrigins = GetFactorManager(true).GetFactorOrigins(GetUserContext());
                Assert.IsTrue(factorOrigins.IsNotEmpty());
            }

            factorOrigins = GetFactorManager().GetFactorOrigins(GetUserContext());
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorOrigin()
        {
            int factorOriginId = 1;
            IFactorOrigin factorOrigin;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorOrigin = GetFactorManager(true).GetFactorOrigin(GetUserContext(), factorOriginId);
                Assert.IsNotNull(factorOrigin);
                Assert.AreEqual(factorOriginId, factorOrigin.Id);
            }

            factorOrigin = GetFactorManager().GetFactorOrigin(GetUserContext(), factorOriginId);
            Assert.IsNotNull(factorOrigin);
            Assert.AreEqual(factorOriginId, factorOrigin.Id);
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            FactorUpdateModeList factorUpdateModes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorUpdateModes = GetFactorManager(true).GetFactorUpdateModes(GetUserContext());
                Assert.IsTrue(factorUpdateModes.IsNotEmpty());
            }

            factorUpdateModes = GetFactorManager().GetFactorUpdateModes(GetUserContext());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorUpdateMode()
        {
            int factorUpdateModeId = 2;
            IFactorUpdateMode factorUpdateMode;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorUpdateMode = GetFactorManager(true).GetFactorUpdateMode(GetUserContext(), factorUpdateModeId);
                Assert.IsNotNull(factorUpdateMode);
                Assert.AreEqual(factorUpdateModeId, factorUpdateMode.Id);
            }

            factorUpdateMode = GetFactorManager().GetFactorUpdateMode(GetUserContext(), factorUpdateModeId);
            Assert.IsNotNull(factorUpdateMode);
            Assert.AreEqual(factorUpdateModeId, factorUpdateMode.Id);
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            FactorFieldTypeList factorFieldTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorFieldTypes = GetFactorManager(true).GetFactorFieldTypes(GetUserContext());
                Assert.IsTrue(factorFieldTypes.IsNotEmpty());
            }

            factorFieldTypes = GetFactorManager().GetFactorFieldTypes(GetUserContext());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetOrganismGroups()
        {
            OrganismGroupList organismGroups;

            foreach (OrganismGroupType organismGroupType in Enum.GetValues(typeof(OrganismGroupType)))
            {
                organismGroups = GetFactorManager().GetOrganismGroups(GetUserContext(), organismGroupType);
                Assert.IsTrue(organismGroups.IsNotEmpty());
                Assert.AreEqual(organismGroupType, organismGroups[0].Type);
            }
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            PeriodTypeList periodTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                periodTypes = GetFactorManager(true).GetPeriodTypes(GetUserContext());
                Assert.IsTrue(periodTypes.IsNotEmpty());
            }

            periodTypes = GetFactorManager().GetPeriodTypes(GetUserContext());
            Assert.IsTrue(periodTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriods()
        {
            PeriodList periods;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                periods = GetFactorManager(true).GetPeriods(GetUserContext());
                Assert.IsTrue(periods.IsNotEmpty());
            }

            periods = GetFactorManager().GetPeriods(GetUserContext());
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriod()
        {
            int periodId = 1;
            IPeriod period;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                period = GetFactorManager(true).GetPeriod(GetUserContext(), periodId);
                Assert.IsTrue(period.IsNotNull());
                Assert.AreEqual(periodId, period.Id);
            }

            period = GetFactorManager().GetPeriod(GetUserContext(), periodId);
            Assert.IsTrue(period.IsNotNull());
            Assert.AreEqual(periodId, period.Id);
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            IndividualCategoryList individualCategories;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                individualCategories = GetFactorManager(true).GetIndividualCategories(GetUserContext());
                Assert.IsTrue(individualCategories.IsNotEmpty());
            }

            individualCategories = GetFactorManager().GetIndividualCategories(GetUserContext());
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetIndividualCategory()
        {
            int individualCategoryId = 1;
            IIndividualCategory individualCategory;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                individualCategory = GetFactorManager(true).GetIndividualCategory(GetUserContext(), individualCategoryId);
                Assert.IsTrue(individualCategory.IsNotNull());
                Assert.AreEqual(individualCategoryId, individualCategory.Id);
            }

            individualCategory = GetFactorManager().GetIndividualCategory(GetUserContext(), individualCategoryId);
            Assert.IsTrue(individualCategory.IsNotNull());
            Assert.AreEqual(individualCategoryId, individualCategory.Id);
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            FactorFieldEnumList factorFieldEnums;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorFieldEnums = GetFactorManager(true).GetFactorFieldEnums(GetUserContext());
                Assert.IsTrue(factorFieldEnums.IsNotEmpty());
                foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
                {
                    Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
                }
            }

            factorFieldEnums = GetFactorManager().GetFactorFieldEnums(GetUserContext());
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            FactorDataTypeList factorDataTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorDataTypes = GetFactorManager(true).GetFactorDataTypes(GetUserContext());
                Assert.IsTrue(factorDataTypes.IsNotEmpty());
                foreach (IFactorDataType factorDataType in factorDataTypes)
                {
                    Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
                }
            }

            factorDataTypes = GetFactorManager().GetFactorDataTypes(GetUserContext());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (IFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactorDataType()
        {
            int factorDataTypeId = 4;
            IFactorDataType factorDataType;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorDataType = GetFactorManager(true).GetFactorDataType(GetUserContext(), factorDataTypeId);
                Assert.IsNotNull(factorDataType);
                Assert.AreEqual(factorDataTypeId, factorDataType.Id);
            }

            factorDataType = GetFactorManager().GetFactorDataType(GetUserContext(), factorDataTypeId);
            Assert.IsNotNull(factorDataType);
            Assert.AreEqual(factorDataTypeId, factorDataType.Id);
        }

        [TestMethod]
        public void GetFactors()
        {
            FactorList factors;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factors = GetFactorManager(true).GetFactors(GetUserContext());
                Assert.IsTrue(factors.IsNotEmpty());
            }

            factors = GetFactorManager().GetFactors(GetUserContext());
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsByIds()
        {
            FactorList factors;
            List<Int32> factorIds;

            factorIds = new List<Int32>();
            factorIds.Add((Int32)(FactorId.RedlistCategory));
            factorIds.Add((Int32)(FactorId.RedlistCriteriaString));
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factors = GetFactorManager(true).GetFactors(GetUserContext(), factorIds);
                Assert.IsTrue(factors.IsNotEmpty());
                Assert.AreEqual(factorIds.Count, factors.Count);
            }

            factors = GetFactorManager().GetFactors(GetUserContext(), factorIds);
            Assert.IsTrue(factors.IsNotEmpty());
            Assert.AreEqual(factorIds.Count, factors.Count);
        }

        [TestMethod]
        public void GetFactorsByDataType()
        {
            FactorList factors;
            int factorDataTypeId = 109; // datatype SA_NyckelRot.
            IFactorDataType factorDataType;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorDataType = GetFactorManager(true).GetFactorDataType(GetUserContext(), factorDataTypeId);
                Assert.IsNotNull(factorDataType);
                factors = GetFactorManager().GetFactors(GetUserContext(), factorDataType);
                Assert.IsTrue(factors.IsNotEmpty());
            }

            factorDataType = GetFactorManager().GetFactorDataType(GetUserContext(), factorDataTypeId);
            Assert.IsNotNull(factorDataType);
            factors = GetFactorManager().GetFactors(GetUserContext(), factorDataType);
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactor()
        {
            int factorId = 985;
            IFactor factor;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factor = GetFactorManager(true).GetFactor(GetUserContext(), factorId);
                Assert.IsTrue(factor.IsNotNull());
                Assert.AreEqual(factorId, factor.Id);
            }

            factor = GetFactorManager().GetFactor(GetUserContext(), factorId);
            Assert.IsTrue(factor.IsNotNull());
            Assert.AreEqual(factorId, factor.Id);
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            FactorSearchCriteria searchCriteria = new FactorSearchCriteria();
            List<int> factorIds = new List<int> { 1, 2, 3 };
            FactorList factors;

            searchCriteria.NameSearchString = null;
            searchCriteria.RestrictSearchToFactorIds = factorIds;
            factors = GetFactorManager().GetFactors(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factors);
            Assert.AreEqual(factorIds.Count, factors.Count);

            searchCriteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;
            factors = GetFactorManager().GetFactors(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factors);
            Assert.IsTrue(factors.Count > 30);
        }

        [TestMethod]
        public void GetFactorTree()
        {
            IFactorTreeNode factorTree;
            Int32 factorId;

            factorId = (Int32)(FactorId.RedlistCategory);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorTree = GetFactorManager(true).GetFactorTree(GetUserContext(), factorId);
                Assert.IsTrue(factorTree.IsNotNull());
                Assert.AreEqual(factorId, factorTree.Id);
            }

            factorTree = GetFactorManager().GetFactorTree(GetUserContext(), factorId);
            Assert.IsTrue(factorTree.IsNotNull());
            Assert.AreEqual(factorId, factorTree.Id);

            factorId = 652; // Rödlistning enligt IUCN.
            factorTree = GetFactorManager().GetFactorTree(GetUserContext(), factorId);
            Assert.IsTrue(factorTree.IsNotNull());
            Assert.AreEqual(factorId, factorTree.Id);
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            FactorTreeNodeList factorTrees;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorTrees = GetFactorManager(true).GetFactorTrees(GetUserContext());
                Assert.IsTrue(factorTrees.IsNotEmpty());
            }

            factorTrees = GetFactorManager().GetFactorTrees(GetUserContext());
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteria()
        {
            List<int> factorIds = new List<int> { 985 };
            FactorTreeSearchCriteria searchCriteria = new FactorTreeSearchCriteria { FactorIds = factorIds };
            FactorTreeNodeList factorTrees;

            factorTrees = GetFactorManager().GetFactorTrees(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.AreEqual(factorTrees[0].Id, 985);
        }

        [TestMethod]
        public void GetPublicPeriods()
        {
            PeriodList periods;

            periods = GetFactorManager(true).GetPublicPeriods(GetUserContext());
            Assert.IsTrue(periods.IsNotEmpty());

            foreach (PeriodTypeId periodTypeId in Enum.GetValues(typeof(PeriodTypeId)))
            {
                periods = GetFactorManager().GetPublicPeriods(GetUserContext(), (Int32)periodTypeId);
                Assert.IsTrue(periods.IsNotEmpty());

                periods = GetFactorManager().GetPublicPeriods(GetUserContext(), periodTypeId);
                Assert.IsTrue(periods.IsNotEmpty());
            }
        }
    }
}
