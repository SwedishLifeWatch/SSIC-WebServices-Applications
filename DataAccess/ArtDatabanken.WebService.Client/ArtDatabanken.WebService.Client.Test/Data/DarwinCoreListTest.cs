using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class DarwinCoreListTest : TestBase
    {
        [TestMethod]
        public void GetTaxa()
        {
            DarwinCoreList speciesObservations;
            ICoordinateSystem coordinateSystem;
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationFieldSortOrderList sortOrder;
            TaxonList taxa;

            speciesObservations = new DarwinCoreList();
            taxa = speciesObservations.GetTaxa(GetUserContext());
            Assert.IsTrue(taxa.IsEmpty());

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.AddTaxon((Int32)(TaxonId.Mammals));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 3, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 3, 2);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            sortOrder = null;
            speciesObservations = CoreData.SpeciesObservationManager.GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, sortOrder);
            taxa = speciesObservations.GetTaxa(GetUserContext());
            Assert.IsTrue(taxa.IsNotEmpty());
            foreach (IDarwinCore specieObservation in speciesObservations)
            {
                Assert.IsTrue(taxa.Contains(specieObservation.Taxon.DyntaxaTaxonID));
            }
        }
    }
}
