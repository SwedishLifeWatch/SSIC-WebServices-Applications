using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.BirdRingingCentre;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.EntomologicalCollections;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.HerbariumOfUmeaUniversity;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.LundBotanicalMuseum;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.SwedishMalaiseTrapProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Gbif
{
    [TestClass]
    public class GbifConnectorTest : TestBase
    {
        [TestMethod]
        [Ignore]
        public void GetPageCountBirdRingingCentre()
        {
            GbifConnector connector = new BirdRingingCentreConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Bird Ringing Centre (GBIF), total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Bird Ringing Centre (GBIF), total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetPageCountEntomologicalCollections()
        {
            GbifConnector connector = new EntomologicalCollectionsConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Entomological Collections (GBIF), total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Entomological Collections (GBIF), total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetPageCountLundBotanicalMuseum()
        {
            GbifConnector connector = new LundBotanicalMuseumConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Lund Botanical Museum (GBIF), total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Lund Botanical Museum (GBIF), total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetPageCountSwedishMalaiseTrapProject()
        {
            GbifConnector connector = new SwedishMalaiseTrapProjectConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Swedish Malaise Trap Project (GBIF), total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Swedish Malaise Trap Project (GBIF), total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetPageCountUme()
        {
            GbifConnector connector = new HerbariumOfUmeaUniversityConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Ume (GBIF), total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Ume (GBIF), total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChangeBirdRingingCentre()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            GbifConnector connector = new BirdRingingCentreConnector();
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            try
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId - 1);
                GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom,
                                                                                            changedTo,
                                                                                            mappings,
                                                                                            GetContext(),
                                                                                            connectorServer);
                Assert.IsFalse(areMoreSpeciesObservationsAvailable);
            }
            catch (Exception)
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId);
                throw;
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChangeEntomologicalCollections()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            GbifConnector connector = new EntomologicalCollectionsConnector();
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            try
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId - 1);
                GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom,
                                                                                            changedTo,
                                                                                            mappings,
                                                                                            GetContext(),
                                                                                            connectorServer);
                Assert.IsFalse(areMoreSpeciesObservationsAvailable);
            }
            catch (Exception)
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId);
                throw;
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChangeLundBotanicalMuseum()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            GbifConnector connector = new LundBotanicalMuseumConnector();
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            try
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId - 1);
                GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom,
                                                                                            changedTo,
                                                                                            mappings,
                                                                                            GetContext(),
                                                                                            connectorServer);
                Assert.IsFalse(areMoreSpeciesObservationsAvailable);
            }
            catch (Exception)
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId);
                throw;
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChangeSwedishMalaiseTrapProject()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            GbifConnector connector = new SwedishMalaiseTrapProjectConnector();
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            try
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId - 1);
                GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom,
                                                                                            changedTo,
                                                                                            mappings,
                                                                                            GetContext(),
                                                                                            connectorServer);
                Assert.IsFalse(areMoreSpeciesObservationsAvailable);
            }
            catch (Exception)
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId);
                throw;
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChangeUme()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            GbifConnector connector = new HerbariumOfUmeaUniversityConnector();
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            try
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId - 1);
                GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom,
                                                                                            changedTo,
                                                                                            mappings,
                                                                                            GetContext(),
                                                                                            connectorServer);
                Assert.IsFalse(areMoreSpeciesObservationsAvailable);
            }
            catch (Exception)
            {
                GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, dataProvider.MaxChangeId);
                throw;
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCountBirdRingingCentre()
        {
            GbifConnector connector = new BirdRingingCentreConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.AreNotEqual(speciesObservationCount, -1);
            Debug.WriteLine("Bird Ringing Centre (GBIF), total number of species observations are " + speciesObservationCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCountEntomologicalCollections()
        {
            GbifConnector connector = new EntomologicalCollectionsConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.AreNotEqual(speciesObservationCount, -1);
            Debug.WriteLine("Entomological Collections (GBIF), total number of species observations are " + speciesObservationCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCountLundBotanicalMuseum()
        {
            GbifConnector connector = new LundBotanicalMuseumConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.AreNotEqual(speciesObservationCount, -1);
            Debug.WriteLine("Lund Botanical Museum (GBIF), total number of species observations are " + speciesObservationCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCountSwedishMalaiseTrapProject()
        {
            GbifConnector connector = new SwedishMalaiseTrapProjectConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.AreNotEqual(speciesObservationCount, -1);
            Debug.WriteLine("Swedish Malaise Trap Project (GBIF), total number of species observations are " + speciesObservationCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCountUme()
        {
            GbifConnector connector = new HerbariumOfUmeaUniversityConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.AreNotEqual(speciesObservationCount, -1);
            Debug.WriteLine("Ume (GBIF), total number of species observations are " + speciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviderBirdRingingCentre()
        {
            GbifConnector connector = new BirdRingingCentreConnector();
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.BirdRingingCentre));
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviderEntomologicalCollections()
        {
            GbifConnector connector = new EntomologicalCollectionsConnector();
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.EntomologicalCollections));
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviderLundBotanicalMuseum()
        {
            GbifConnector connector = new LundBotanicalMuseumConnector();
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.LundBotanicalMuseum));
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviderSwedishMalaiseTrapProject()
        {
            GbifConnector connector = new SwedishMalaiseTrapProjectConnector();
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.SwedishMalaiseTrapProject));
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviderUme()
        {
            GbifConnector connector = new HerbariumOfUmeaUniversityConnector();
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = connector.GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity));
        }
    }
}
