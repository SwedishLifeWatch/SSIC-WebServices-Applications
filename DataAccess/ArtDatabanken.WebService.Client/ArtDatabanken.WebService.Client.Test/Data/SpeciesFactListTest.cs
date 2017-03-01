using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesFactListTest : TestBase
    {
        [TestMethod]
        public void Merge()
        {
            Int32 count;
            ISpeciesFactSearchCriteria searchCriteria;
            SpeciesFactList speciesFacts;

            // Get species fact list with species facts.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.ActionPlan));
            speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts.IsNotEmpty());

            // Merge species fact that already are in the list.
            count = speciesFacts.Count;
            speciesFacts.Merge(GetUserContext(),
                               speciesFacts[0].Taxon,
                               speciesFacts[0].IndividualCategory,
                               speciesFacts[0].Factor,
                               speciesFacts[0].Host,
                               speciesFacts[0].Period);
            Assert.AreEqual(count, speciesFacts.Count);

            // Merge species fact that is not in the list.
            count = speciesFacts.Count;
            speciesFacts.Merge(GetUserContext(),
                               speciesFacts[0].Taxon,
                               speciesFacts[0].IndividualCategory,
                               CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory),
                               speciesFacts[0].Host,
                               speciesFacts[0].Period);
            Assert.AreEqual(count, speciesFacts.Count - 1);
        }

        [TestMethod]
        public void RemoveSpeciesFactsWithBadQuality()
        {
            Int32 countWithBadQuality, countWithoutBadQuality;
            SpeciesFactList speciesFacts;
            ISpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 779));
            speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            countWithBadQuality = speciesFacts.Count;
            speciesFacts.RemoveSpeciesFactsWithBadQuality();
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            countWithoutBadQuality = speciesFacts.Count;
            Assert.IsTrue(countWithoutBadQuality < countWithBadQuality);
        }
    }
}
