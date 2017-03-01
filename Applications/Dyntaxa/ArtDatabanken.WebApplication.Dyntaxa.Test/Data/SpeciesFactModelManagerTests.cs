using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
{
    [TestClass]
    public class SpeciesFactModelManagerTests : TestBase
    {
        [TestMethod]
        public void TestSpeciesFactsWithIncludeMissingSpeciesFacts()
        {
            IUserContext userContext = GetUserContext();
            List<FactorId> factorIds = new List<FactorId> {FactorId.SwedishOccurrence, FactorId.SwedishHistory};
            List<int> taxonIds = new List<int>();
            taxonIds.Add(237935);  //Amphora Veneta
            SpeciesFactList speciesFactList = SpeciesFactModelManager.GetSpeciesFactListByTaxaAndFactors(userContext, factorIds, taxonIds, true);
            var speciesFactDictionary = speciesFactList.ToDictionaryGroupedByTaxonIdThenFactorId();
            
            Assert.IsTrue(speciesFactDictionary[237935][FactorId.SwedishOccurrence].GetStatusId().HasValue);
            Assert.IsFalse(speciesFactDictionary[237935][FactorId.SwedishHistory].GetStatusId().HasValue);

            var occurrenceStatusId = speciesFactDictionary[237935][FactorId.SwedishOccurrence].GetStatusId();
            var occurrenceQualityId = speciesFactDictionary[237935][FactorId.SwedishOccurrence].GetQualityId();
            var occurrenceReferenceId = speciesFactDictionary[237935][FactorId.SwedishOccurrence].GetReferenceId();
            var occurrenceComment = speciesFactDictionary[237935][FactorId.SwedishOccurrence].GetDescription();

            var historyStatusId = speciesFactDictionary[237935][FactorId.SwedishHistory].GetStatusId();
            var historyQualityId = speciesFactDictionary[237935][FactorId.SwedishHistory].GetQualityId();
            var historyReferenceId = speciesFactDictionary[237935][FactorId.SwedishHistory].GetReferenceId();
            var historyComment = speciesFactDictionary[237935][FactorId.SwedishHistory].GetDescription();
            
            Assert.IsNotNull(speciesFactList);
        }


        [TestMethod]
        public void TestSpeciesFactsWithExcludeMissingSpeciesFacts()
        {
            IUserContext userContext = GetUserContext();
            List<FactorId> factorIds = new List<FactorId> { FactorId.SwedishOccurrence, FactorId.SwedishHistory };
            List<int> taxonIds = new List<int>();
            taxonIds.Add(237935);  //Amphora Veneta
            SpeciesFactList speciesFactList = SpeciesFactModelManager.GetSpeciesFactListByTaxaAndFactors(userContext, factorIds, taxonIds, false);
            var speciesFactDictionary = speciesFactList.ToDictionaryGroupedByTaxonIdThenFactorId();

            Assert.IsTrue(speciesFactDictionary.ContainsKey(237935));
            Assert.IsTrue(speciesFactDictionary[237935].ContainsKey(FactorId.SwedishOccurrence));
            Assert.IsFalse(speciesFactDictionary[237935].ContainsKey(FactorId.SwedishHistory));            
        }
    }
}
