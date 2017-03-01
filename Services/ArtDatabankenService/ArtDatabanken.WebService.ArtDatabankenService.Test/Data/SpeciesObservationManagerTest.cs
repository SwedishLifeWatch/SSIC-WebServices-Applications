using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class SpeciesObservationManagerTest : TestBase
    {
        public SpeciesObservationManagerTest()
            : base(false, 90)
        {
            ApplicationIdentifier = ArtDatabankenService.Data.ApplicationIdentifier.PrintObs.ToString();
        }

        [TestMethod]
        public void AddUserSelectedSpeciesObservations()
        {
            SpeciesObservationManager.AddUserSelectedSpeciesObservations(GetContext(), GetSomeSpeciesObservationIds(GetContext()));
            SpeciesObservationManager.DeleteUserSelectedSpeciesObservations(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedSpeciesObservations()
        {
            SpeciesObservationManager.AddUserSelectedSpeciesObservations(GetContext(), GetSomeSpeciesObservationIds(GetContext()));
            SpeciesObservationManager.DeleteUserSelectedSpeciesObservations(GetContext());
        }

        public static List<WebBirdNestActivity> GetAllBirdNestActivities(WebServiceContext context)
        {
            return SpeciesObservationManager.GetBirdNestActivities(context);
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            List<WebBirdNestActivity> birdNestActivities;

            birdNestActivities = SpeciesObservationManager.GetBirdNestActivities(GetContext());
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        public static WebBirdNestActivity GetOneBirdNestActivity(WebServiceContext context)
        {
            return SpeciesObservationManager.GetBirdNestActivities(context)[0];
        }

        public static WebSpeciesObservation GetOneSpeciesObservation(WebServiceContext context)
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(context)[0].Id;
            searchCriteria.Accuracy = 1000;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(context, searchCriteria);
            return speciesObservationInformation.SpeciesObservations[0];
        }

        public static WebSpeciesObservationInformation GetOneSpeciesObservationInformation(WebServiceContext context)
        {
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(context)[0].Id;
            searchCriteria.Accuracy = 1000;
            return SpeciesObservationManager.GetSpeciesObservations(context, searchCriteria);
        }

        public static List<Int64> GetSomeSpeciesObservationIds(WebServiceContext context)
        {
            List<Int64> speciesObservationIds;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(629558);
            speciesObservationIds.Add(629559);
            return speciesObservationIds;
        }

        public static List<WebSpeciesObservation> GetSomeSpeciesObservations(WebServiceContext context)
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(4);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(context)[0].Id;
            searchCriteria.Accuracy = 1000;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(context, searchCriteria);
            return speciesObservationInformation.SpeciesObservations;
        }

        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            // Get some changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = SpeciesObservationManager.GetSpeciesObservationChange(GetContext(), changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());

            // Get many changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 20, 0, 0, 0);
            change = SpeciesObservationManager.GetSpeciesObservationChange(GetContext(), changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsNotEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesObservationChangeDateSwitchError()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = SpeciesObservationManager.GetSpeciesObservationChange(GetContext(), changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesObservationChangeFutureChangedToError()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = DateTime.Now;
            change = SpeciesObservationManager.GetSpeciesObservationChange(GetContext(), changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            Int32 speciesObservationCount;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.Accuracy = 1000;
            speciesObservationCount = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesObservationCount > 0);

            // Test observer search string.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.DatabaseIds = new List<Int32>();
            searchCriteria.DatabaseIds.Add(7);
            searchCriteria.ObserverSearchString = "";
            speciesObservationCount = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesObservationCount > 0);
        }

        [TestMethod]
        public void GetSpeciesObservations()
        {
            DateTime startDate;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.Accuracy = 1000;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());

            // Get more species observations than 
            // SpeciesObservationManager.MAX_SPECIES_OBSERVATIONS_WITH_INFORMATION.
            //searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.TaxonIds.Add(BEWICKS_SWAN_ID);
            //searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            //searchCriteria.Accuracy = 10000;
            //speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            //Assert.IsNotNull(speciesObservationInformation);
            //Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsEmpty());
            //Assert.IsTrue(speciesObservationInformation.SpeciesObservationIds.IsNotEmpty());
            //Assert.AreEqual(speciesObservationInformation.SpeciesObservationIds.Count, speciesObservationInformation.SpeciesObservationCount);
            
            // Test problem with getting observations inside an rectangle.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsRectangleSpecified = true;
            searchCriteria.EastCoordinate = 1296797;
            searchCriteria.NorthCoordinate = 6363590;
            searchCriteria.SouthCoordinate = 6350588;
            searchCriteria.WestCoordinate = 1284223;
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());

            // Test problem with getting observations inside an rectangle.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101656);
            searchCriteria.UseOfObservationDate = WebUseOfDate.IgnoreYear;
            searchCriteria.ObservationStartDate = new DateTime(2009, 8, 1);
            searchCriteria.ObservationEndDate = new DateTime(2009, 8, 31);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                startDate = speciesObservation.DataFields[4].Value.WebParseDateTime();
                Assert.AreEqual(searchCriteria.ObservationStartDate.Month,
                                startDate.Month);
                Assert.IsTrue(searchCriteria.ObservationStartDate.Day <= startDate.Day);
                Assert.IsTrue(searchCriteria.ObservationEndDate.Day >= startDate.Day);
            }

            // Test observer search string.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.DatabaseIds = new List<Int32>();
            searchCriteria.DatabaseIds.Add(7);
            searchCriteria.ObserverSearchString = "";
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsIncludeUncertainTaxonDetermination()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation1,
                                             speciesObservationInformation2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeUncertainTaxonDetermination = false;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(5740); // Gul fingersvamp
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            speciesObservationInformation1 = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation1);
            Assert.IsTrue(speciesObservationInformation1.SpeciesObservations.IsNotEmpty());

            searchCriteria.IncludeUncertainTaxonDetermination = true;
            speciesObservationInformation2 = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation2);
            Assert.IsTrue(speciesObservationInformation2.SpeciesObservations.IsNotEmpty());
            Assert.IsTrue(speciesObservationInformation2.SpeciesObservationCount >
                          speciesObservationInformation1.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetSpeciesObservationsToManyError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds.Add(INSECTS_TAXON_ID);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservations(GetContext(), searchCriteria);
            Assert.IsNotNull(speciesObservationInformation);
        }

        [TestMethod]
        public void GetSpeciesObservationsById()
        {
            Int32 userRoleId;
            WebSpeciesObservationInformation speciesObservationInformation;

            userRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsById(GetContext(), GetSomeSpeciesObservationIds(GetContext()), userRoleId);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySpeciesObservations()
        {
            Int32 taxonCount;
            List<WebTaxon> taxa;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.Accuracy = 1000;
            taxa = SpeciesObservationManager.GetTaxaBySpeciesObservations(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test problem where taxon is of a higher level in
            // the taxon tree.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds.Add(MAMMAL_TAXON_ID);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            taxa = SpeciesObservationManager.GetTaxaBySpeciesObservations(GetContext(), searchCriteria);
            taxonCount = SpeciesObservationManager.GetTaxaCountBySpeciesObservations(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxonCount, taxa.Count);

            // Test observer search string.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.DatabaseIds = new List<Int32>();
            searchCriteria.DatabaseIds.Add(7);
            searchCriteria.ObserverSearchString = "";
            taxa = SpeciesObservationManager.GetTaxaBySpeciesObservations(GetContext(), searchCriteria);
            taxonCount = SpeciesObservationManager.GetTaxaCountBySpeciesObservations(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxonCount, taxa.Count);
        }

        [TestMethod]
        public void GetTaxaCountBySpeciesObservations()
        {
            List<WebTaxon> taxa;
            Int32 taxaCount;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = TaxonManagerTest.GetSomeTaxonIds(1);
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.Accuracy = 1000;
            taxaCount = SpeciesObservationManager.GetTaxaCountBySpeciesObservations(GetContext(), searchCriteria);
            Assert.IsTrue(taxaCount > 0);

            // Test observer search string.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.UserRoleId = WebServiceData.UserManager.GetRoles(GetContext())[0].Id;
            searchCriteria.DatabaseIds = new List<Int32>();
            searchCriteria.DatabaseIds.Add(7);
            searchCriteria.ObserverSearchString = "";
            taxa = SpeciesObservationManager.GetTaxaBySpeciesObservations(GetContext(), searchCriteria);
            taxaCount = SpeciesObservationManager.GetTaxaCountBySpeciesObservations(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxaCount, taxa.Count);
        }

        public static DataTable GetUserSelectedSpeciesObservationTable(WebServiceContext context)
        {
            DataRow row;
            DataTable speciesObservationTable;

            speciesObservationTable = SpeciesObservationManager.GetUserSelectedSpeciesObservationsTable(context);
            foreach (Int64 speciesObservationId in GetSomeSpeciesObservationIds(context))
            {
                row = speciesObservationTable.NewRow();
                row[0] = context.RequestId;
                row[1] = speciesObservationId;
                speciesObservationTable.Rows.Add(row);
            }
            return speciesObservationTable;
        }
    }
}
