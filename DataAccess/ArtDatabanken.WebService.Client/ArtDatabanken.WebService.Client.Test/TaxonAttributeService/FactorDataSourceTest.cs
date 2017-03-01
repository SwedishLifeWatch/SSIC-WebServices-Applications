using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.TaxonAttributeService
{
    [TestClass]
    public class FactorDataSourceTest : TestBase
    {
        private FactorDataSource _factorDataSource;

        public FactorDataSourceTest()
        {
            _factorDataSource = null;
        }

        private FactorDataSource GetFactorDataSource(Boolean refresh = false)
        {
            if (_factorDataSource.IsNull() || refresh)
            {
                _factorDataSource = new FactorDataSource();
            }

            return _factorDataSource;
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            FactorOriginList factorOrigins;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorOrigins = GetFactorDataSource(true).GetFactorOrigins(GetUserContext());
                Assert.IsTrue(factorOrigins.IsNotEmpty());
            }

            factorOrigins = GetFactorDataSource().GetFactorOrigins(GetUserContext());
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            FactorUpdateModeList factorUpdateModes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorUpdateModes = GetFactorDataSource(true).GetFactorUpdateModes(GetUserContext());
                Assert.IsTrue(factorUpdateModes.IsNotEmpty());
            }

            factorUpdateModes = GetFactorDataSource().GetFactorUpdateModes(GetUserContext());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            FactorFieldTypeList factorFieldTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorFieldTypes = GetFactorDataSource(true).GetFactorFieldTypes(GetUserContext());
                Assert.IsTrue(factorFieldTypes.IsNotEmpty());
            }

            factorFieldTypes = GetFactorDataSource().GetFactorFieldTypes(GetUserContext());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            PeriodTypeList periodTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                periodTypes = GetFactorDataSource(true).GetPeriodTypes(GetUserContext());
                Assert.IsTrue(periodTypes.IsNotEmpty());
            }

            periodTypes = GetFactorDataSource().GetPeriodTypes(GetUserContext());
            Assert.IsTrue(periodTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriods()
        {
            PeriodList periods;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                periods = GetFactorDataSource(true).GetPeriods(GetUserContext());
                Assert.IsTrue(periods.IsNotEmpty());
            }

            periods = GetFactorDataSource().GetPeriods(GetUserContext());
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            IndividualCategoryList individualCategories;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                individualCategories = GetFactorDataSource(true).GetIndividualCategories(GetUserContext());
                Assert.IsTrue(individualCategories.IsNotEmpty());
            }

            individualCategories = GetFactorDataSource().GetIndividualCategories(GetUserContext());
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            FactorFieldEnumList factorFieldEnums;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorFieldEnums = GetFactorDataSource(true).GetFactorFieldEnums(GetUserContext());
                Assert.IsTrue(factorFieldEnums.IsNotEmpty());
                foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
                {
                    Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
                }
            }

            factorFieldEnums = GetFactorDataSource().GetFactorFieldEnums(GetUserContext());
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
                factorDataTypes = GetFactorDataSource(true).GetFactorDataTypes(GetUserContext());
                Assert.IsTrue(factorDataTypes.IsNotEmpty());
                foreach (IFactorDataType factorDataType in factorDataTypes)
                {
                    Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
                }
            }

            factorDataTypes = GetFactorDataSource().GetFactorDataTypes(GetUserContext());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (IFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactors()
        {
            FactorList factors;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factors = GetFactorDataSource(true).GetFactors(GetUserContext());
                Assert.IsTrue(factors.IsNotEmpty());
            }

            factors = GetFactorDataSource().GetFactors(GetUserContext());
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            List<int> factorIds = new List<int> { 1, 2, 3 };
            IFactorSearchCriteria searchCriteria = new FactorSearchCriteria();
            FactorList factors;

            searchCriteria.RestrictSearchToFactorIds = factorIds;
            searchCriteria.NameSearchString = null;
            factors = GetFactorDataSource().GetFactors(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factors);
            Assert.AreEqual(factorIds.Count, factors.Count);

            searchCriteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;
            factors = GetFactorDataSource().GetFactors(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factors);
            Assert.IsTrue(factors.Count > 30);
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            FactorTreeNodeList factorTrees;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                factorTrees = GetFactorDataSource(true).GetFactorTrees(GetUserContext());
                Assert.IsTrue(factorTrees.IsNotEmpty());
            }

            factorTrees = GetFactorDataSource().GetFactorTrees(GetUserContext());
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria()
        {
            List<int> factorIds = new List<int> { 985 };
            FactorTreeSearchCriteria searchCriteria = new FactorTreeSearchCriteria { FactorIds = factorIds };
            FactorTreeNodeList factorTrees;

            factorTrees = GetFactorDataSource().GetFactorTrees(GetUserContext(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
            Assert.AreEqual(factorTrees.Count, 1);
            Assert.AreEqual(factorTrees[0].Id, 985);
        }
    }
}