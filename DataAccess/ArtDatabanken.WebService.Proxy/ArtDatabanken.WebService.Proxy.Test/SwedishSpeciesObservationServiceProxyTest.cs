using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class SwedishSpeciesObservationServiceProxyTest
    {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        private WebClientInformation _clientInformation;
        private WebCoordinateSystem _coordinateSystem;

        public SwedishSpeciesObservationServiceProxyTest()
        {
            _clientInformation = null;
            _coordinateSystem = null;
        }

        public void CheckDarwinCoreInformation(WebDarwinCoreInformation information)
        {
            Assert.IsNotNull(information);
            Assert.IsTrue(0 < information.MaxSpeciesObservationCount);
            if (information.SpeciesObservationCount > information.MaxSpeciesObservationCount)
            {
                Assert.IsTrue(information.SpeciesObservationIds.IsNotEmpty());
                Assert.AreEqual(information.SpeciesObservationCount, information.SpeciesObservationIds.Count);
            }
            else
            {
                Assert.IsTrue(information.SpeciesObservations.IsNotEmpty());
                Assert.AreEqual(information.SpeciesObservationCount, information.SpeciesObservations.Count);
            }
            if (information.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
                {
                    Assert.IsNotNull(speciesObservation);
                }
            }
        }

        private void CheckSpeciesObservationInformation(WebSpeciesObservationInformation information)
        {
            Assert.IsNotNull(information);
            Assert.IsTrue(0 < information.MaxSpeciesObservationCount);
            if (information.SpeciesObservationCount > information.MaxSpeciesObservationCount)
            {
                Assert.IsTrue(information.SpeciesObservationIds.IsNotEmpty());
                Assert.AreEqual(information.SpeciesObservationCount, information.SpeciesObservationIds.Count);
            }
            else
            {
                Assert.IsTrue(information.SpeciesObservations.IsNotEmpty());
                Assert.AreEqual(information.SpeciesObservationCount, information.SpeciesObservations.Count);
            }
        }

        [TestMethod]
        [Ignore]
        public void ClearCache()
        {
            TestInitialize(ApplicationIdentifier.WebAdministration.ToString());
            WebServiceProxy.SwedishSpeciesObservationService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        [Ignore]
        public void DeleteTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);

            // Create some trace information.
            WebServiceProxy.SwedishSpeciesObservationService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.SwedishSpeciesObservationService.GetLog(GetClientInformation(), LogType.None, null, 100);
            WebServiceProxy.SwedishSpeciesObservationService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.SwedishSpeciesObservationService.DeleteTrace(GetClientInformation());
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            List<WebSpeciesActivity> birdNestActivities;

            birdNestActivities = WebServiceProxy.SwedishSpeciesObservationService.GetBirdNestActivities(GetClientInformation());
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void GetCountyRegions()
        {
            List<WebRegion> regions;

            regions = WebServiceProxy.SwedishSpeciesObservationService.GetCountyRegions(GetClientInformation());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        private WebCoordinateSystem GetCoordinateSystem()
        {
            if (_coordinateSystem.IsNull())
            {
                _coordinateSystem = new WebCoordinateSystem();
                _coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            }
            return _coordinateSystem;
        }

        [TestMethod]
        public void GetDarwinCoreByIds()
        {
            Int32 index;
            List<Int64> speciesObservationIds;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation webDarwinCoreInformation;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;
            webDarwinCoreInformation = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreByIds(GetClientInformation(), speciesObservationIds, coordinateSystem);
            Assert.IsNotNull(webDarwinCoreInformation);
            Assert.IsTrue(webDarwinCoreInformation.SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservationIds.Count, webDarwinCoreInformation.SpeciesObservationCount);
            Assert.AreEqual(speciesObservationIds.Count, webDarwinCoreInformation.SpeciesObservations.Count);
            for (index = 0; index < speciesObservationIds.Count; index++)
            {
                Assert.AreEqual(speciesObservationIds[index], webDarwinCoreInformation.SpeciesObservations[index].Id);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaAccuracy()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 60;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaAccuracyArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = -1;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimit()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.BirdNestActivityLimit = WebServiceProxy.SwedishSpeciesObservationService.GetBirdNestActivities(GetClientInformation())[3].Id;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IsBirdNestActivityLimitSpecified = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimitArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.BirdNestActivityLimit = -1;
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBoundingBox()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1632635, 6670116);
            searchCriteria.BoundingBox.Min = new WebPoint(1300000, 6000000);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new WebPoint(1501658, 6508484);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaBoundingBoxNullMaxError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Min = new WebPoint(1562902, 6618355);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaChangeDateTime()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 8, 1);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaChangeDateTimeInterval()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();

            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // Find changed observations with interval.
            // Test changed observations PartOfYear with a excluding interval over a newyearsday.
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 7, 1);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2010, 5, 1);
            dateTimeInterval.End = new DateTime(2010, 7, 1);
            dateTimeInterval.IsDayOfYearSpecified = false;

            searchCriteria.ChangeDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.DayOfYear));
            }

            // Test changed observations PartOfYear with including interval over a newyearsday
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int includeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.DayOfYear));
            }

            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministration1()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            // Test problem with missing bird observations in
            // new Artportalskopplingen.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BirdNestActivityLimit = 18;
            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 11, 27);
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(@"URN:LSID:Artportalen.se:Area:DataSet1Feature1440"); // Ale kommun.
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102118); // Nattskärra.
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuids()
        {
            List<WebSpeciesObservationDataProvider> speciesObservationDataSources;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            speciesObservationDataSources = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationDataProviders(GetClientInformation());
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add(speciesObservationDataSources[3].Guid);

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.AreEqual(speciesObservationDataSources[3].Name, darwinCore.DatasetName);
            }

            searchCriteria.DataProviderGuids.Add(speciesObservationDataSources[1].Guid);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount >= information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue((speciesObservationDataSources[3].Name == darwinCore.DatasetName) ||
                              (speciesObservationDataSources[1].Name == darwinCore.DatasetName));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuidsUnknownGuidError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataProviderGuids.Add("None data provider GUID");
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeNeverFoundObservations()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101213); // Linsräka.

            searchCriteria.IncludeNeverFoundObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludeNeverFoundObservations = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsFalse(darwinCore.Occurrence.IsNeverFoundObservation);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeNotRediscoveredObservations()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100119); // Gölgroda.

            searchCriteria.IncludeNotRediscoveredObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludeNotRediscoveredObservations = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsFalse(darwinCore.Occurrence.IsNotRediscoveredObservation);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludePositiveObservations()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.IncludePositiveObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludePositiveObservations = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
                {
                    Assert.IsFalse(darwinCore.Occurrence.IsPositiveObservation);
                }
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeRedListCategories()
        {
            RedListCategory redListCategory;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
                searchCriteria.IncludeRedListCategories.Add(redListCategory);
                information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
                CheckDarwinCoreInformation(information1);
                foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
                {
                    // The check for not empty red list category should not be
                    // necessary but data on test server is not up to date.
                    // This test has a problem with taxon 232265 that
                    // is redlisted as VU since its parent taxon is
                    // red listed as NT.
                    if (darwinCore.Conservation.RedlistCategory.IsNotEmpty() &&
                        (darwinCore.Taxon.DyntaxaTaxonID != 232265))
                    {
                        Assert.AreEqual(redListCategory.ToString(),
                            darwinCore.Conservation.RedlistCategory.Substring(0, 2).ToUpper());
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaIncludeRedListCategoriesArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.LC);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeRedlistedTaxa()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 90;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222489); // Kvarngröe.

            searchCriteria.IncludeRedlistedTaxa = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            if (information1.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
                {
                    bool em = darwinCore.Conservation.RedlistCategory.IsNotEmpty();
                    int tax = darwinCore.Taxon.DyntaxaTaxonID;

                    Assert.IsTrue(darwinCore.Conservation.RedlistCategory.IsNotEmpty() ||
                                  darwinCore.Taxon.DyntaxaTaxonID == 222489);
                }
            }

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222489); // Kvarngröe.
            searchCriteria.IncludeRedlistedTaxa = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
                {
                    Assert.AreEqual(222489, darwinCore.Taxon.DyntaxaTaxonID);
                }
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsAccuracyConsidered()
        {
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test with bounding box.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new WebPoint(1501658, 6508484);
            searchCriteria.IsAccuracyConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.BoundingBox = null;

            // Test with polygon.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1370000, 6460000));
            linearRing.Points.Add(new WebPoint(1370000, 6240000));
            linearRing.Points.Add(new WebPoint(1600000, 6240000));
            linearRing.Points.Add(new WebPoint(1600000, 6460000));
            linearRing.Points.Add(new WebPoint(1370000, 6460000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.IsAccuracyConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.UpplandGuid);
            searchCriteria.IsAccuracyConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsDisturbanceSensitivityConsidered()
        {
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101316); // grön hedvårtbitare.

            // Test with bounding box.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new WebPoint(1360000, 6000000);

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.BoundingBox = null;

            // Test with polygon.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1700000, 6700000));
            linearRing.Points.Add(new WebPoint(1360000, 6700000));
            linearRing.Points.Add(new WebPoint(1360000, 6000000));
            linearRing.Points.Add(new WebPoint(1700000, 6000000));
            linearRing.Points.Add(new WebPoint(1700000, 6700000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Skane);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.RegionGuids = null;

            // Test with accuracy.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new WebPoint(1360000, 6000000);
            searchCriteria.IsAccuracyConsidered = true;

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.BoundingBox = null;
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsNaturalOccurrence()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100017); // Klockgroda.

            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            if (information1.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
                {
                    Assert.IsTrue(darwinCore.Occurrence.IsNaturalOccurrence);
                }
            }

            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
                {
                    Assert.IsFalse(darwinCore.Occurrence.IsNaturalOccurrence);
                }
            }

            searchCriteria.IsIsNaturalOccurrenceSpecified = false;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchString()
        {
            var ci = new CultureInfo("sv-SE");
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.StartsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.ToLower().Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));
            }

            searchCriteria.LocalityNameSearchString.SearchString = "backar";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.EndsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "%Full%";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString.Substring(1, 4)));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsFalse(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringOperatorsError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringNoOperatorError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaMaxProtectionLevel()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsMaxProtectionLevelSpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.MaxProtectionLevel = 5;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel);
            }

            searchCriteria.MaxProtectionLevel = 1;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaMinProtectionLevel()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(626); // Ljungögontröst
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsMinProtectionLevelSpecified = true;

            searchCriteria.MinProtectionLevel = 1;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel >= searchCriteria.MinProtectionLevel);
            }

            searchCriteria.MinProtectionLevel = 5;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            Assert.IsTrue(information2.SpeciesObservations.IsEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTime()
        {
            WebDarwinCoreInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1970, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);

            // Test Operator on Begin and End.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Event.Start <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(speciesObservation.Event.End >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);

            // Test Accuracy.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 4000;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 400;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTimeInterval()
        {
            // Tests when the interval is within a year
            // (not over new year's eve)
            // Tests day of year and date (ie Month-Day) queries
            // Tests Excluding and Including

            Int32 excludeSize, includeSize;
            WebDarwinCoreInformation information1;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add(205479); // Stormfågel

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2012, 1, 1);
            dateTimeInterval.End = new DateTime(2012, 3, 1);

            // Day of year - excluding.
            dateTimeInterval.IsDayOfYearSpecified = true;
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // Day of year - including.
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));
            }

            //DAY INCLUDING-EXCLUDING INTERVAL WITHIN A YEAR
            Assert.IsTrue(includeSize >= excludeSize);

            // DATE - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = false;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin <= speciesObservation.Event.Start) ||
                              (dateTimeInterval.End >= speciesObservation.Event.Start));

                Assert.IsTrue((dateTimeInterval.End >= speciesObservation.Event.End) ||
                              (dateTimeInterval.Begin <= speciesObservation.Event.End));
            }

            //DATE INCLUDING-EXCLUDING INTERVAL WITHIN A YEAR
            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTimeIntervalNewYearsEve()
        {
            //Tests when the interval is over new year's eve.
            //Tests day of year and date (ie Month-Day) queries
            //Tests Excluding and Including

            Int32 excludeSize, includeSize;
            WebDarwinCoreInformation information1;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // find observations with interval

            // TEST WITHOUT INTERVAL FOR COMPARING REASON
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 12, 29);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 1, 5);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;
            //TEST WITHOUT INTERVAL FOR COMPARING REASON
            Assert.IsTrue(includeSize >= excludeSize);

            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2009, 12, 29);
            dateTimeInterval.End = new DateTime(2010, 1, 5);

            // DAY OF YEAR - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = true;
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            // DAY OF YEAR - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;
            //DAY INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);

            // DATE - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = false;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;

            //DATE INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObserverSearchString()
        {
            var ci = new CultureInfo("sv-SE");
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "oskar";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);

            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.ToLower().Contains(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            searchCriteria.ObserverSearchString.SearchString = "Kindvall";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.EndsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.ObserverSearchString.SearchString = "";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.ObserverSearchString.SearchString = "Oskar Kindva%";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString.Substring(0, 10)));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.ObserverSearchString.SearchString = "";
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsFalse(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringNoOperatorError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringOperatorsError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }


        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPage_SortOrderStart()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations1, speciesObservations2;

            var pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            var startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Start = 1;
            pageSpecification.Size = 100;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //CheckDarwinCoreInformation(speciesObservations1);
            int i = 0;
            Debug.WriteLine("----- ASC -----");
            foreach (WebDarwinCore speciesObservation in speciesObservations1)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.WebToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            //Change Sort Order and try again
            startSortOrder.SortOrder = SortOrder.Descending;
            pageSpecification.SortOrder.Clear();
            pageSpecification.SortOrder.Add(startSortOrder);
            speciesObservations2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            i = 0;
            Debug.WriteLine("----- DESC -----");
            foreach (WebDarwinCore speciesObservation in speciesObservations2)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.WebToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }
            Debug.WriteLine("----------");
            Assert.IsTrue(speciesObservations1.Count <= speciesObservations2.Count);
        }


        [TestMethod]
        public void GetDarwinCoreBySearchCriteria_SortOrderStart()
        {
            WebDarwinCoreInformation speciesObservations1, speciesObservations2;
            WebSpeciesObservationSearchCriteria searchCriteria;
            //List<WebDarwinCore> speciesObservations1, speciesObservations2;

            var sortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            var startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            sortOrder.Add(startSortOrder);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), sortOrder);

            //CheckDarwinCoreInformation(speciesObservations1);
            int i = 0;
            Debug.WriteLine("----- ASC -----");
            foreach (WebDarwinCore speciesObservation in speciesObservations1.SpeciesObservations)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.WebToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
                if (i == 100) break;
            }

            //Change Sort Order and try again
            startSortOrder.SortOrder = SortOrder.Descending;
            sortOrder.Clear();
            sortOrder.Add(startSortOrder);
            speciesObservations2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), sortOrder);
            i = 0;
            Debug.WriteLine("----- DESC -----");
            foreach (WebDarwinCore speciesObservation in speciesObservations2.SpeciesObservations)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.WebToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
                if (i == 100) break;
            }
            Debug.WriteLine("----------");
            Assert.IsTrue(speciesObservations1.SpeciesObservations.Count <= speciesObservations2.SpeciesObservations.Count);
        }


        [TestMethod]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaPage_BigData()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations;

            var pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            var startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;

            pageSpecification.Start = 1;
            pageSpecification.Size = 100;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            Debug.WriteLine("1 - 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            pageSpecification.Start = 10000;
            pageSpecification.Size = 100;
            stopwatch.Restart();
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            Debug.WriteLine("10 000 - 10 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            pageSpecification.Start = 100000;
            pageSpecification.Size = 100;
            stopwatch.Restart();
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            Debug.WriteLine("100 000 - 100 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            //pageSpecification.Start = 1000000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("1 000 000 - 1 000 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1500000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("1 500 000 - 1 500 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1400000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("1 400 000 - 1 400 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 11000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("11 000 - 11 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1400000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("1 400 000 - 1 400 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1900000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Debug.WriteLine("1 900 000 - 1 900 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPage_Taxon_Region()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(4000072);

            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);

            var pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            var startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);


            pageSpecification.Start = 1;
            pageSpecification.Size = 1000;


            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification);


            int x = speciesObservations.Capacity;
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPolygons()
        {
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test polygon.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1000000, 7000000));
            linearRing.Points.Add(new WebPoint(1000000, 5000000));
            linearRing.Points.Add(new WebPoint(2000000, 5000000));
            linearRing.Points.Add(new WebPoint(2000000, 7000000));
            linearRing.Points.Add(new WebPoint(1000000, 7000000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Add(polygon);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.AreEqual(information1.SpeciesObservationCount, information2.SpeciesObservationCount);

            // Test with smaler polygon.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1370000, 6460000));
            linearRing.Points.Add(new WebPoint(1370000, 6240000));
            linearRing.Points.Add(new WebPoint(1600000, 6240000));
            linearRing.Points.Add(new WebPoint(1600000, 6460000));
            linearRing.Points.Add(new WebPoint(1370000, 6460000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount > information2.SpeciesObservationCount);

            // Test with points in reverse order.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1000000, 7000000));
            linearRing.Points.Add(new WebPoint(2000000, 7000000));
            linearRing.Points.Add(new WebPoint(2000000, 5000000));
            linearRing.Points.Add(new WebPoint(1000000, 5000000));
            linearRing.Points.Add(new WebPoint(1000000, 7000000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegionGuids()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            // Test adding the same region twice.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.AreEqual(information1.SpeciesObservationCount, information2.SpeciesObservationCount);

            // Test adding another region.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservations.Count < information2.SpeciesObservations.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownRegionGuidError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("Unknown region guid");
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegistrationDateTime()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 8, 1);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }

            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 10, 1);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }
            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegistrationDateTimeInterval()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // Find reported observations with interval.
            // Test reported observations PartOfYear with a excluding interval over a newyearsday.
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2000, 12, 20);
            dateTimeInterval.End = new DateTime(2001, 1, 10);
            dateTimeInterval.IsDayOfYearSpecified = true;

            searchCriteria.ReportedDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.DayOfYear));
            }

            // Test reported observations PartOfYear with including interval over a newyearsday
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Including;

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int includeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.DayOfYear));
            }

            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaSpeciesActivityIds()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivities(GetClientInformation())[1].Id);
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.SpeciesActivityIds.Add(WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivities(GetClientInformation())[4].Id);
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaSpeciesActivityIdsUnknownIdError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(1000000);
            information = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaTaxonIds()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 6, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            information2 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222226); // Knylhavre
            information1 = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreChange()
        {
            const WebSpeciesObservationSearchCriteria searchCriteria = null;
            WebDarwinCoreChange webDarwinCoreChange;

            long changeId = 94797516; //33240760;// 33240760;// 27091302; //16561900; // 11109018; //11189018;
            var changedFrom = new DateTime(2013, 6, 26);
            var changedTo = new DateTime(2013, 6, 27);
            Boolean isChangedFromSpecified = true;
            Boolean isChangedToSpecified = true;
            Boolean isChangedIdSpecified = false;
            long maxReturnedChanges = 4000;
            Boolean moreData = true;
            int i = 0;
            while (moreData)
            {
                webDarwinCoreChange =
                    WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreChange(GetClientInformation(), changedFrom,
                        isChangedFromSpecified, changedTo,
                        isChangedToSpecified, changeId,
                        isChangedIdSpecified, maxReturnedChanges,
                        searchCriteria, GetCoordinateSystem());
                moreData = webDarwinCoreChange.IsMoreSpeciesObservationsAvailable;

                changeId = webDarwinCoreChange.MaxChangeId + 1; //ta inte med sista changeID igen, därför +1

                isChangedIdSpecified = true;
                i++;
                //moreData = false;
                Debug.WriteLine("ReadNo: " + i
                                + " Created:" + webDarwinCoreChange.CreatedSpeciesObservations.Count
                                + " Updated:" + webDarwinCoreChange.UpdatedSpeciesObservations.Count
                                + " Deleted:" + webDarwinCoreChange.DeletedSpeciesObservationGuids.Count
                                + " MaxChangeId: " + webDarwinCoreChange.MaxChangeId
                                + " More data: " + webDarwinCoreChange.IsMoreSpeciesObservationsAvailable);
            }
        }

        [TestMethod]
        [Ignore]
        public void GetDarwinCoreChangeAll()
        {
            const WebSpeciesObservationSearchCriteria searchCriteria = null;
            WebDarwinCoreChange webDarwinCoreChange;

            long changeId = 0;
            var changedFrom = new DateTime(2013, 6, 26);
            var changedTo = new DateTime(2013, 6, 27);
            Boolean isChangedFromSpecified = false;
            Boolean isChangedToSpecified = false;
            Boolean isChangedIdSpecified = true;
            long maxReturnedChanges = 5000;
            Boolean moreData = true;
            int i = 0;
            while (moreData)
            {
                webDarwinCoreChange = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreChange(GetClientInformation(),
                    changedFrom,
                    isChangedFromSpecified,
                    changedTo,
                    isChangedToSpecified,
                    changeId,
                    isChangedIdSpecified,
                    maxReturnedChanges,
                    searchCriteria,
                    GetCoordinateSystem());
                moreData = webDarwinCoreChange.IsMoreSpeciesObservationsAvailable;

                changeId = webDarwinCoreChange.MaxChangeId + 1; //ta inte med sista changeID igen, därför +1

                i++;
                Debug.WriteLine("ReadNo: " + i
                                + " Created:" + webDarwinCoreChange.CreatedSpeciesObservations.Count
                                + " Updated:" + webDarwinCoreChange.UpdatedSpeciesObservations.Count
                                + " Deleted:" + webDarwinCoreChange.DeletedSpeciesObservationGuids.Count
                                + " MaxChangeId: " + webDarwinCoreChange.MaxChangeId
                                + " More data: " + webDarwinCoreChange.IsMoreSpeciesObservationsAvailable);
                Thread.Sleep(3000);
            }
        }

        [TestMethod]
        public void GetDarwinCoreChange_searchCriteria()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = null;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 5, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221917);
            WebDarwinCoreChange webDarwinCoreChange;

            const long changeId = 16551900; //16561900; // 11109018; //11189018;
            var changedFrom = new DateTime(2013, 6, 26);
            var changedTo = new DateTime(2013, 6, 27);
            const Boolean isChangedFromSpecified = true;
            const Boolean isChangedToSpecified = true;
            const Boolean isChangedIdSpecified = false;
            const long maxReturnedChanges = 10000;

            webDarwinCoreChange = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreChange(GetClientInformation(), changedFrom,
                isChangedFromSpecified, changedTo, isChangedToSpecified,
                changeId, isChangedIdSpecified, maxReturnedChanges,
                searchCriteria, GetCoordinateSystem());

            Assert.IsTrue(webDarwinCoreChange.IsNotNull());
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationAccuracy()
        {
            Boolean hasProtectedSpeciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1580000, 6640000);
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IsMinProtectionLevelSpecified = true;
            searchCriteria.MinProtectionLevel = 2;
            searchCriteria.BirdNestActivityLimit = 20;
            searchCriteria.IsBirdNestActivityLimitSpecified = true;

            searchCriteria.Accuracy = 1000;
            hasProtectedSpeciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetProtectedSpeciesObservationIndication(GetClientInformation(), searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;

            searchCriteria.Accuracy = 1;
            hasProtectedSpeciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetProtectedSpeciesObservationIndication(GetClientInformation(), searchCriteria, GetCoordinateSystem());
            Assert.IsFalse(hasProtectedSpeciesObservations);
        }

        [TestMethod]
        public void GetProvinceRegions()
        {
            List<WebRegion> regions;

            regions = WebServiceProxy.SwedishSpeciesObservationService.GetProvinceRegions(GetClientInformation());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivities()
        {
            List<WebSpeciesActivity> activities;

            activities = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivities(GetClientInformation());
            Assert.IsNotNull(activities);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivityCategories()
        {
            List<WebSpeciesActivityCategory> activityCategories;

            activityCategories = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivityCategories(GetClientInformation());
            Assert.IsNotNull(activityCategories);
        }


        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            WebSpeciesObservationChange speciesObservationChange;

            long changeId = 0;
            DateTime changedFrom = new DateTime(2013, 10, 04);
            DateTime changedTo = new DateTime(2013, 10, 05);
            Boolean isChangedFromSpecified = true;
            const bool IsChangedToSpecified = true;
            Boolean isChangedIdSpecified = false;
            const long MaxReturnedChanges = 4000;
            Boolean moreData = true;
            long currentMaxChangeId = 0;
            while (moreData)
            {
                speciesObservationChange = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationChange(GetClientInformation(),
                                                                                                                        changedFrom,
                                                                                                                        isChangedFromSpecified,
                                                                                                                        changedTo,
                                                                                                                        IsChangedToSpecified,
                                                                                                                        changeId,
                                                                                                                        isChangedIdSpecified,
                                                                                                                        MaxReturnedChanges,
                                                                                                                        null,
                                                                                                                        GetCoordinateSystem(),
                                                                                                                        null);
                moreData = speciesObservationChange.IsMoreSpeciesObservationsAvailable;

                // Do not include last change id again. Thats why we add 1.
                changeId = speciesObservationChange.MaxChangeId + 1;

                isChangedIdSpecified = true;
                isChangedFromSpecified = false;

                Assert.IsTrue(currentMaxChangeId != speciesObservationChange.MaxChangeId);
                currentMaxChangeId = speciesObservationChange.MaxChangeId;
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsByIds()
        {
            List<Int64> speciesObservationIds = new List<Int64>();
            WebSpeciesObservationInformation speciesObservationInformation;

            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(4);
            speciesObservationIds.Add(20);
            speciesObservationIds.Add(300);
            speciesObservationIds.Add(10);
            speciesObservationIds.Add(4000);

            WebSpeciesObservationInformation dataSourceList;
            WebSpeciesObservationSpecification speciesObservationSpecification = new WebSpeciesObservationSpecification();
            speciesObservationSpecification.Fields = new List<WebSpeciesObservationFieldSpecification>();

            WebSpeciesObservationFieldSpecification speciesObservationFieldSpecification;

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "DarwinCore";
            speciesObservationFieldSpecification.Property.Identifier = "AccessRights";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "DarwinCore";
            speciesObservationFieldSpecification.Property.Identifier = "BasisOfRecord";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "DarwinCore";
            speciesObservationFieldSpecification.Property.Identifier = "Owner";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "Parish";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Taxon";
            speciesObservationFieldSpecification.Property.Identifier = "VernacularName";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Conservation";
            speciesObservationFieldSpecification.Property.Identifier = "RedlistCategory";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Taxon";
            speciesObservationFieldSpecification.Property.Identifier = "TaxonSortOrder";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Conservation";
            speciesObservationFieldSpecification.Property.Identifier = "Natura2000";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "DecimalLatitude";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Event";
            speciesObservationFieldSpecification.Property.Identifier = "StartDayOfYear";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Event";
            speciesObservationFieldSpecification.Property.Identifier = "Start";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            dataSourceList = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsByIds(GetClientInformation(),
                                                                                                          speciesObservationIds,
                                                                                                          GetCoordinateSystem(),
                                                                                                          speciesObservationSpecification);
            Assert.IsNotNull(dataSourceList);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBox()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1443618, 6365688);
            searchCriteria.BoundingBox.Min = new WebPoint(1442707, 6364005);
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.IncludePositiveObservations = true;
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaGetProjectParameters()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation speciesObservationInformation;

            // Get species observations with project parameters.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(106752); // hjärtgrynsnäcka
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2013, 10, 06);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 10, 06);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservationInformation = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                Assert.IsNotNull(speciesObservation);
                Assert.IsNotNull(speciesObservation.Fields);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPageTaxonIds()
        {
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Start = 1;
            pageSpecification.Size = 100;

            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteriaPage(GetClientInformation(), searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.IsTrue(speciesObservations.Count <= pageSpecification.Size);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaTaxonIds()
        {
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            Int64 noOfObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            searchCriteria = new WebSpeciesObservationSearchCriteria { DataProviderGuids = new List<string> { "urn:lsid:swedishlifewatch.se:DataProvider:3" } };
            noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria_UsesOwnerFieldSearchCriteria_ExpectsObservations()
        {
            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria { DataProviderGuids = new List<string> { "urn:lsid:swedishlifewatch.se:DataProvider:1" } };

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            SetOwnerFieldSearchCriterias(searchCriteria);
            long noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            Debug.Print("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);
        }

        private static void SetOwnerFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetOwnerFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetOwnerFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.DarwinCore);
            //fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Owner);
            fieldSearchCriteria.Type = WebDataType.String;
            //fieldSearchCriteria.Value = "Länsstyrelsen Östergötland";
            //fieldSearchCriteria.Value = "Per Flodin";
            fieldSearchCriteria.Value = "Mari Friberg";

            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteria_UsesOrCombinedFieldSearchCriterias_ExpectsObservations()
        {
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.DataProviderGuids = new List<string>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            SetOrCombinedFieldSearchCriterias(searchCriteria);

            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria_UsesOrCombinedFieldSearchCriterias_ExpectsObservations()
        {
            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria { DataProviderGuids = new List<string> { "urn:lsid:swedishlifewatch.se:DataProvider:4" } };

            SetOrCombinedFieldSearchCriterias(searchCriteria);
            
            long noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            Debug.Print("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);
        }

        private static void SetOrCombinedFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetHabitatFieldSearchCriteria(fieldSearchCriterias);
            SetSubstrateFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;

            if (searchCriteria.DataFields.IsNull())
            {
                searchCriteria.DataFields = new List<WebDataField>();
            }

            searchCriteria.DataFields.SetString("FieldLogicalOperator", LogicalOperator.Or.ToString());
        }

        private static void SetHabitatFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Event);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Habitat);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetSubstrateFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Occurrence);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Substrate);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria_UsesCoordinateUncertaintyInMetersFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");

            SetCoordinateUncertaintyInMetersFieldSearchCriteria(searchCriteria);

            // Act
            long noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Debug.Print("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);
        }

        [Ignore]
        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteria_UsesCoordinateUncertaintyInMetersFieldSearchCriteria_ExpectsObservations()
        {
            // Arrange
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.DataProviderGuids = new List<string>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            SetCoordinateUncertaintyInMetersFieldSearchCriteria(searchCriteria);

            // Act
            speciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem(), null, null);
            
            // Assert
            Assert.IsTrue(speciesObservations.SpeciesObservations.IsNotEmpty());
        }

        private static void SetCoordinateUncertaintyInMetersFieldSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Location);
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.CoordinateUncertaintyInMeters);
            fieldSearchCriteria.Type = WebDataType.Float64;
            fieldSearchCriteria.Value = "10000";
            fieldSearchCriterias.Add(fieldSearchCriteria);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviders()
        {
            List<WebSpeciesObservationDataProvider> dataSourceList;

            dataSourceList = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationDataProviders(GetClientInformation());
            Assert.IsNotNull(dataSourceList);
        }

        [TestMethod]
        public void GetSpeciesObservationFieldDescriptions()
        {
            List<WebSpeciesObservationFieldDescription> fieldDescriptions, fieldDescriptionsMappings;

            fieldDescriptions = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationFieldDescriptions(GetClientInformation());
            Assert.IsTrue(fieldDescriptions.IsNotEmpty());
            Assert.IsTrue(fieldDescriptions.Count > 90);
            fieldDescriptionsMappings = new List<WebSpeciesObservationFieldDescription>();
            foreach (WebSpeciesObservationFieldDescription fieldDescription in fieldDescriptions)
            {
                if (fieldDescription.Mappings.IsNotEmpty() &&
                    fieldDescription.Mappings.Count < 5)
                {
                    fieldDescriptionsMappings.Add(fieldDescription);
                }
            }

            Assert.IsTrue(fieldDescriptionsMappings.IsNotEmpty());
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.SwedishSpeciesObservationService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.SwedishSpeciesObservationService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(Settings.Default.TestUserName,
                Settings.Default.TestPassword,
                Settings.Default.DyntaxaApplicationIdentifier,
                false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(Settings.Default.TestUserName,
                Settings.Default.TestPassword,
                Settings.Default.DyntaxaApplicationIdentifier,
                false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.SwedishSpeciesObservationService.Logout(clientInformation);
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.GeoReferenceService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        [Ignore]
        public void StartTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SwedishSpeciesObservationService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SwedishSpeciesObservationService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        [Ignore]
        public void StopTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SwedishSpeciesObservationService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SwedishSpeciesObservationService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (_clientInformation.IsNotNull())
                {
                    WebServiceProxy.SwedishSpeciesObservationService.Logout(_clientInformation);
                }
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
            finally
            {
                _clientInformation = null;
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            TestInitialize(ApplicationIdentifier.PrintObs.ToString());
        }

        public void TestInitialize(String applicationIdentifier)
        {
            WebLoginResponse loginResponse;

            TestCleanup();

            Configuration.SetInstallationType();

            loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(Settings.Default.TestUserName,
                                                                                   Settings.Default.TestPassword,
                                                                                   applicationIdentifier,
                                                                                   false);

            // PRODUKTIONSMILJÖ
            //Configuration.InstallationType = InstallationType.Production;
            //WebServiceProxy.SwedishSpeciesObservationService.WebServiceAddress = @"swedishspeciesobservation.artdatabankensoa.se/SwedishSpeciesObservationService.svc";
            //loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(USER_NAME,
            //                                                                       PASSWORD,
            //                                                                       applicationIdentifier,
            //                                                                       false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Token = loginResponse.Token;
        }
#endif
    }
}