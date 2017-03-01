using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for SpeciesFactListTest
    /// </summary>
    [TestClass]
    public class SpeciesFactListTest : TestBase
    {
        Data.ArtDatabankenService.SpeciesFactList _speciesFacts;

        public SpeciesFactListTest()
        {
            _speciesFacts = null;
        }

        [TestMethod]
        public void Get()
        {
            foreach (SpeciesFact speciesFact in GetSpeciesFacts())
            {
                Assert.AreEqual(speciesFact, GetSpeciesFacts().Get(speciesFact.Identifier));
            }
        }

        [TestMethod]
        public void GetSortOrder()
        {
            foreach (SpeciesFact speciesFact in GetSpeciesFacts())
            {
                Assert.AreEqual(speciesFact.Id, GetSpeciesFacts().Get(speciesFact.Id).SortOrder);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 speciesFactId;

            speciesFactId = Int32.MinValue;
            GetSpeciesFacts().Get(speciesFactId);
        }

        private Data.ArtDatabankenService.SpeciesFactList GetSpeciesFacts()
        {
            if (_speciesFacts.IsNull())
            {
                List<Int32> factorIds;
                factorIds = new List<Int32>();
                List<Int32> taxonIds;
                taxonIds = new List<Int32>();

                factorIds.Add((Int32)FactorId.LandscapeFactors);
                factorIds.Add((Int32)FactorId.LandscapeFactor_Agricultural);
                factorIds.Add((Int32)FactorId.LandscapeFactor_Alpin);
                factorIds.Add((Int32)FactorId.LandscapeFactor_Coast);

                taxonIds.Add(1);

                UserParameterSelection userParameterSelection = new UserParameterSelection();
                userParameterSelection.Factors.Merge(Data.ArtDatabankenService.FactorManager.GetFactors(factorIds));
                userParameterSelection.Taxa.Merge(ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(taxonIds, ArtDatabanken.Data.WebService.TaxonInformationType.Basic));

                _speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParameterSelection);
            }
            return _speciesFacts;
        }

        [TestMethod]
        public void GetSpeciesFactsByParameters()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetSpeciesFacts();

            Data.ArtDatabankenService.SpeciesFactList subsetWithTaxon = speciesFacts.GetSpeciesFactsByParameters(speciesFacts[0].Taxon);
            Assert.IsNotNull(subsetWithTaxon);
            Assert.AreEqual(speciesFacts[0].Taxon.ScientificName, subsetWithTaxon[0].Taxon.ScientificName);

            Data.ArtDatabankenService.SpeciesFactList subsetWithFactor = speciesFacts.GetSpeciesFactsByParameters(speciesFacts[0].Factor);
            Assert.IsNotNull(subsetWithFactor);
            Assert.AreEqual(speciesFacts[0].Factor.Label, subsetWithFactor[0].Factor.Label);

            Data.ArtDatabankenService.SpeciesFactList subsetWithIndividualCetegory = speciesFacts.GetSpeciesFactsByParameters(speciesFacts[0].IndividualCategory);
            Assert.IsNotNull(subsetWithIndividualCetegory);
            Assert.AreEqual(speciesFacts[0].IndividualCategory.Name, subsetWithIndividualCetegory[0].IndividualCategory.Name);

            Data.ArtDatabankenService.SpeciesFactList subsetWithPeriod = speciesFacts.GetSpeciesFactsByParameters(speciesFacts[0].Period);
            Assert.IsNotNull(subsetWithPeriod);
            Assert.AreEqual(speciesFacts[0].Period, subsetWithPeriod[0].Period);

            Data.ArtDatabankenService.SpeciesFactList subsetWithCombination = speciesFacts.GetSpeciesFactsByParameters(
                speciesFacts[0].Taxon,
                speciesFacts[0].IndividualCategory,
                speciesFacts[0].Factor,
                speciesFacts[0].Period);
            Assert.IsNotNull(subsetWithCombination);
            Assert.AreEqual(1, subsetWithCombination.Count);

        }

        [TestMethod]
        public void GetSpeciesFactsByQuality()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetSpeciesFacts();

            Data.ArtDatabankenService.SpeciesFactList subset = speciesFacts.GetSpeciesFactsByQuality(speciesFacts[0].Quality);
            Assert.IsNotNull(subset);
            Assert.AreEqual(speciesFacts[0].Quality, subset[0].Quality);
        }

        [TestMethod]
        public void GetSpeciesFactByUniqueParameterCombination()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetSpeciesFacts();
            Data.ArtDatabankenService.SpeciesFact testFact = speciesFacts[0];
            Data.ArtDatabankenService.SpeciesFact speciesFact = speciesFacts.Get(
                testFact.Taxon,
                testFact.IndividualCategory,
                testFact.Factor,
                testFact.Host,
                testFact.Period);
            Assert.IsNotNull(speciesFact);
            Assert.AreEqual(testFact, speciesFact);

        }

        [TestMethod]
        public void CountChanges()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts = GetSpeciesFacts();
            Data.ArtDatabankenService.SpeciesFact testFact = speciesFacts[1];
            Assert.IsFalse(testFact.HasChanged);
            Assert.AreEqual(speciesFacts.CountChanges(), 0);
            testFact.MainField.EnumValue = testFact.Factor.FactorDataType.MainField.FactorFieldEnum.Values[0];
            Assert.IsTrue(testFact.HasChanged);
            Assert.AreEqual(speciesFacts.CountChanges(), 1);
        }


    }
}
