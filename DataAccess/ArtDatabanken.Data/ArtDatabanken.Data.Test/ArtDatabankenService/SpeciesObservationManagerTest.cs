using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class SpeciesObservationManagerTest : TestBase
    {
        public static BirdNestActivityList GetAllBirdNestActivities()
        {
            return Data.ArtDatabankenService.SpeciesObservationManager.GetBirdNestActivities();
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            BirdNestActivityList birdNestActivities;

            TestInitialize("PrintObs");
            birdNestActivities = Data.ArtDatabankenService.SpeciesObservationManager.GetBirdNestActivities();
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        public static BirdNestActivity GetOneBirdNestActivity()
        {
            return Data.ArtDatabankenService.SpeciesObservationManager.GetBirdNestActivities()[0];
        }

        public static Data.ArtDatabankenService.SpeciesObservationSearchCriteria GetOneSearchCriteria()
        {
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.Accuracy = 1000;
            return searchCriteria;
        }

        public static WebSpeciesObservationSearchCriteria GetOneWebSearchCriteria()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = Data.ArtDatabankenService.UserManager.GetUser().Roles[0].Id;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.UserRoleIdSpecified = true;
#endif
            searchCriteria.IsAccuracySpecified = true;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.IsAccuracySpecifiedSpecified = true;
#endif
            searchCriteria.Accuracy = 1000;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.AccuracySpecified = true;
#endif
            searchCriteria.IsBirdNestActivityLevelSpecified = false;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.IsBirdNestActivityLevelSpecifiedSpecified = true;
#endif
            searchCriteria.IncludePositiveObservations = true;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.IncludePositiveObservationsSpecified = true;
#endif
            searchCriteria.IsRectangleSpecified = false;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.IsRectangleSpecifiedSpecified = true;
#endif
            return searchCriteria;
        }

        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom, changedTo;
            Data.ArtDatabankenService.SpeciesObservationChange change;

            TestInitialize("PrintObs");

            // Get some changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
            Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());

            // Get many changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 2, 0, 0, 0);
            change = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
            Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesObservationChangeDateSwitchError()
        {
            DateTime changedFrom, changedTo;
            Data.ArtDatabankenService.SpeciesObservationChange change;

            TestInitialize("PrintObs");

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesObservationChangeFutureChangedToError()
        {
            DateTime changedFrom, changedTo;
            Data.ArtDatabankenService.SpeciesObservationChange change;

            TestInitialize("PrintObs");

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = DateTime.Now;
            change = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void GetSpeciesObservationCount()
        {
            Int32 speciesObservationCount;
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;

            TestInitialize("PrintObs");
            searchCriteria = GetOneSearchCriteria();
            speciesObservationCount = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservationCount(searchCriteria);
            Assert.IsTrue(speciesObservationCount > 0);
        }

        [TestMethod]
        public void GetSpeciesObservations()
        {
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationInformation speciesObservationInformation;

            TestInitialize("PrintObs");
            searchCriteria = GetOneSearchCriteria();
            speciesObservationInformation = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservations(searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsFalse(speciesObservationInformation.Cancelled);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsIncludeUncertainTaxonDetermination()
        {
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationInformation speciesObservationInformation1,
                                          speciesObservationInformation2;

            TestInitialize("PrintObs");
            searchCriteria = new Data.ArtDatabankenService.SpeciesObservationSearchCriteria();
            searchCriteria.IncludeUncertainTaxonDetermination = false;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(5740); // Gul fingersvamp
            searchCriteria.UserRoleId = Data.ArtDatabankenService.UserManager.GetUser().Roles[0].Id;
            speciesObservationInformation1 = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservations(searchCriteria);
            Assert.IsNotNull(speciesObservationInformation1);
            Assert.IsTrue(speciesObservationInformation1.SpeciesObservations.IsNotEmpty());

            searchCriteria.IncludeUncertainTaxonDetermination = true;
            speciesObservationInformation2 = Data.ArtDatabankenService.SpeciesObservationManager.GetSpeciesObservations(searchCriteria);
            Assert.IsNotNull(speciesObservationInformation2);
            Assert.IsTrue(speciesObservationInformation2.SpeciesObservations.IsNotEmpty());
            Assert.IsTrue(speciesObservationInformation2.SpeciesObservations.Count >
                          speciesObservationInformation1.SpeciesObservations.Count);
        }

        [TestMethod]
        public void GetTaxaBySpeciesObservations()
        {
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;
            Data.ArtDatabankenService.TaxonList taxa;

            TestInitialize("PrintObs");
            searchCriteria = GetOneSearchCriteria();
            taxa = Data.ArtDatabankenService.SpeciesObservationManager.GetTaxaBySpeciesObservations(searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaCountBySpeciesObservations()
        {
            Int32 taxaCount;
            Data.ArtDatabankenService.SpeciesObservationSearchCriteria searchCriteria;

            TestInitialize("PrintObs");
            searchCriteria = GetOneSearchCriteria();
            taxaCount = Data.ArtDatabankenService.SpeciesObservationManager.GetTaxaCountBySpeciesObservations(searchCriteria);
            Assert.IsTrue(0 < taxaCount);
        }
    }
}
