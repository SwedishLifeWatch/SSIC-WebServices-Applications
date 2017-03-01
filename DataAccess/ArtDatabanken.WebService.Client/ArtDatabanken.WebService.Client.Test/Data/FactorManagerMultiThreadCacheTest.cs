using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class FactorManagerMultiThreadCacheTest : TestBase
    {
        private FactorManagerMultiThreadCache _factorManager;

        public FactorManagerMultiThreadCacheTest()
        {
            _factorManager = null;
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            long durationFirst, durationSecond;
            FactorOriginList factorOrigins;

            Stopwatch.Start();
            factorOrigins = GetFactorManager(true).GetFactorOrigins(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorOrigins.IsNotEmpty());

            Stopwatch.Start();
            factorOrigins = GetFactorManager().GetFactorOrigins(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorOrigins.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactor()
        {
            int factorId = 2540;
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
        public void GetFactorUpdateModes()
        {
            long durationFirst, durationSecond;
            FactorUpdateModeList factorUpdateModes;

            Stopwatch.Start();
            factorUpdateModes = GetFactorManager(true).GetFactorUpdateModes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());

            Stopwatch.Start();
            factorUpdateModes = GetFactorManager().GetFactorUpdateModes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            long durationFirst, durationSecond;
            FactorFieldTypeList factorFieldTypes;

            Stopwatch.Start();
            factorFieldTypes = GetFactorManager(true).GetFactorFieldTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());

            Stopwatch.Start();
            factorFieldTypes = GetFactorManager().GetFactorFieldTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetOrganismGroups()
        {
            Int64 durationFirst, durationSecond;
            OrganismGroupList organismGroups;

            Stopwatch.Start();
            organismGroups = GetFactorManager(true).GetOrganismGroups(GetUserContext(), OrganismGroupType.Standard);
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(organismGroups.IsNotEmpty());

            Stopwatch.Start();
            organismGroups = GetFactorManager().GetOrganismGroups(GetUserContext(), OrganismGroupType.Standard);
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(organismGroups.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            long durationFirst, durationSecond;
            PeriodTypeList periodTypes;

            Stopwatch.Start();
            periodTypes = GetFactorManager(true).GetPeriodTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(periodTypes.IsNotEmpty());

            Stopwatch.Start();
            periodTypes = GetFactorManager().GetPeriodTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(periodTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetPeriods()
        {
            long durationFirst, durationSecond;
            PeriodList periods;

            Stopwatch.Start();
            periods = GetFactorManager(true).GetPeriods(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(periods.IsNotEmpty());

            Stopwatch.Start();
            periods = GetFactorManager().GetPeriods(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(periods.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            long durationFirst, durationSecond;
            IndividualCategoryList individualCategories;

            Stopwatch.Start();
            individualCategories = GetFactorManager(true).GetIndividualCategories(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(individualCategories.IsNotEmpty());

            Stopwatch.Start();
            individualCategories = GetFactorManager().GetIndividualCategories(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(individualCategories.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            long durationFirst, durationSecond;
            FactorFieldEnumList factorFieldEnums;

            Stopwatch.Start();
            factorFieldEnums = GetFactorManager(true).GetFactorFieldEnums(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }

            Stopwatch.Start();
            factorFieldEnums = GetFactorManager().GetFactorFieldEnums(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            long durationFirst, durationSecond;
            FactorDataTypeList factorDataTypes;

            Stopwatch.Start();
            factorDataTypes = GetFactorManager(true).GetFactorDataTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (IFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }

            Stopwatch.Start();
            factorDataTypes = GetFactorManager().GetFactorDataTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (IFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactors()
        {
            long durationFirst, durationSecond;
            FactorList factors;

            Stopwatch.Start();
            factors = GetFactorManager(true).GetFactors(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factors.IsNotEmpty());

            Stopwatch.Start();
            factors = GetFactorManager().GetFactors(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factors.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            long durationFirst, durationSecond;
            FactorTreeNodeList factorTrees;

            Stopwatch.Start();
            factorTrees = GetFactorManager(true).GetFactorTrees(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorTrees.IsNotEmpty());

            Stopwatch.Start();
            factorTrees = GetFactorManager().GetFactorTrees(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(factorTrees.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetFactorTree()
        {
            long durationFirst, durationSecond;
            IFactorTreeNode factorTreeNode;
            int factorId = 2524;

            Stopwatch.Start();
            factorTreeNode = GetFactorManager(true).GetFactorTree(GetUserContext(), factorId);
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsNotNull(factorTreeNode);

            Stopwatch.Start();
            factorTreeNode = GetFactorManager().GetFactorTree(GetUserContext(), factorId);
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsNotNull(factorTreeNode);

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        private FactorManagerMultiThreadCache GetFactorManager(bool refresh = false)
        {
            if (_factorManager.IsNull() || refresh)
            {
                _factorManager = new FactorManagerMultiThreadCache();
                _factorManager.DataSource = new FactorDataSource();
            }
            return _factorManager;
        }
    }
}