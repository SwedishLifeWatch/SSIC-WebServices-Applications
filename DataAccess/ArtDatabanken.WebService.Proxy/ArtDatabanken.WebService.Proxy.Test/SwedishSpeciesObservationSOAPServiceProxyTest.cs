using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class SwedishSpeciesObservationSOAPServiceProxyTest
    {
#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        private WebClientInformation _clientInformation;
        private WebCoordinateSystem _coordinateSystem;

        public SwedishSpeciesObservationSOAPServiceProxyTest()
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
                    Assert.IsTrue(speciesObservation.CollectionCode.IsNotEmpty());
                }
            }
        }

        [TestMethod]
        public void ClearCache()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SwedishSpeciesObservationSOAPService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        public void DeleteTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);

            // Create some trace information.
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.SwedishSpeciesObservationSOAPService.GetLog(GetClientInformation(), LogType.None, null, 100);
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.SwedishSpeciesObservationSOAPService.DeleteTrace(GetClientInformation());
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            List<WebBirdNestActivity> birdNestActivities;

            birdNestActivities = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetBirdNestActivities(GetClientInformation());
            Assert.IsTrue(birdNestActivities.Count > 1);

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
            webDarwinCoreInformation = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreByIds(GetClientInformation(), speciesObservationIds, coordinateSystem);
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
        public void GetDarwinCoreBySearchCriteriaCountyAdministrationProblem()
        {
            List<WebDarwinCore> speciesObservations;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            // Test problem with missing observations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102918); // citronfläckad kärrtrollslända 
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            information = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information);

            // Test problem with no observations in Heby kommun
            // when searching on Uppsala län.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(200010); // Ljungvårtbitare
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.
            information = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information);
            speciesObservations = new List<WebDarwinCore>();
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                if (speciesObservation.Location.Municipality == "Heby")
                {
                    speciesObservations.Add(speciesObservation);
                }
            }
            Assert.IsTrue(speciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministrationProblem2()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            // Test problem with viewing species observations from Artprotalen 2.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.SpeciesActivityIds.Add(10);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100077); // Utter. 
            searchCriteria.IncludePositiveObservations = true;
            information = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministration()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.BirdNestActivityLimit = 18;
            //searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 11, 27);
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            // searchCriteria.ObservationDateTime.com = CompareOperator.Excluding;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(@"URN:LSID:Artportalen.se:Area:DataSet1Feature1440");
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.SpeciesActivityIds.Add(18);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102118);
            information = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, GetCoordinateSystem());
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministrationProblem3()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;

            // Test problem with viewing species observations from Södermanlands län.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.SodermanlandCounty);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2012, 9, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 9, 5);
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.SpeciesActivityIds.Add(20);
            information = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteria()
        {
            WebBirdNestActivity birdNestActivity;
            WebCoordinateSystem coordinateSystem, wgsCoordinateSystem;
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            // Test search criteria Accuracy.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 111;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 59;
            information2 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            // Test search criteria BoundingBox.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = new WebBoundingBox();

            searchCriteria.BoundingBox.Max = new WebPoint(1645000, 6681000);
            searchCriteria.BoundingBox.Min = new WebPoint(1308000, 6222000);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Location.CoordinateX <= searchCriteria.BoundingBox.Max.X);
                Assert.IsTrue(darwinCore.Location.CoordinateX >= searchCriteria.BoundingBox.Min.X);
                Assert.IsTrue(darwinCore.Location.CoordinateY <= searchCriteria.BoundingBox.Max.Y);
                Assert.IsTrue(darwinCore.Location.CoordinateY >= searchCriteria.BoundingBox.Min.Y);
            }

            searchCriteria.BoundingBox.Max = new WebPoint(1500000, 6500000);
            searchCriteria.BoundingBox.Min = new WebPoint(1400000, 6300000);
            information2 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Location.CoordinateX <= searchCriteria.BoundingBox.Max.X);
                Assert.IsTrue(darwinCore.Location.CoordinateX >= searchCriteria.BoundingBox.Min.X);
                Assert.IsTrue(darwinCore.Location.CoordinateY <= searchCriteria.BoundingBox.Max.Y);
                Assert.IsTrue(darwinCore.Location.CoordinateY >= searchCriteria.BoundingBox.Min.Y);
            }

            // Test error with coordinate conversion.
            searchCriteria.BoundingBox.Max = new WebPoint(17.7, 60.0);
            searchCriteria.BoundingBox.Min = new WebPoint(17.6, 59.9);
            wgsCoordinateSystem = new WebCoordinateSystem();
            wgsCoordinateSystem.Id = CoordinateSystemId.WGS84;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, wgsCoordinateSystem);
            CheckDarwinCoreInformation(information1);

            // Test search criteria ChangeDateTime.

            // Test search criteria DataFieldSearchCriteria.

            // Test search criteria DataSourceIds.

            // Test search criteria IncludeNeverFoundObservations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludeNeverFoundObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Occurrence.IsNeverFoundObservation);
            }

            // Test search criteria IncludeNotRediscoveredObservations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludeNotRediscoveredObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Occurrence.IsNotRediscoveredObservation);
            }

            // Test search criteria IncludePositiveObservations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludePositiveObservations = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Occurrence.IsPositiveObservation);
            }

            // Test search criteria IncludeRedListCategories.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.CR);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                // The check for not empty red list category should not be
                // necessary but data on test server is not up to date.
                // This test has a problem with taxon 232265 that
                // is redlisted as VU since its parent taxon is
                // red listed as NT.
                if (darwinCore.Conservation.RedlistCategory.IsNotEmpty() &&
                    (Int32.Parse(darwinCore.Taxon.TaxonID) != 232265))
                {
                    Assert.AreEqual(searchCriteria.IncludeRedListCategories[0].ToString(),
                                    darwinCore.Conservation.RedlistCategory.Substring(0, 2).ToUpper());
                }
            }

            // Test search criteria IncludeRedlistedTaxa.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 3;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            Assert.IsTrue(information1.SpeciesObservations.IsNotEmpty());

            // Test search criteria LocalityNameSearchString.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö%";
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.AreEqual(searchCriteria.LocalityNameSearchString.SearchString.Substring(0, 7).ToUpper(),
                                darwinCore.Location.Locality.Substring(0, 7).ToUpper());
            }


            // Test search criteria ObservationDateTime.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 8, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2009, 8, 31);

            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= darwinCore.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= darwinCore.Event.Start);
            }

            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(new WebDateTimeInterval());
            searchCriteria.ObservationDateTime.PartOfYear[0].Begin = new DateTime(2009, 8, 1);
            searchCriteria.ObservationDateTime.PartOfYear[0].End = new DateTime(2009, 8, 31);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.AreEqual(searchCriteria.ObservationDateTime.PartOfYear[0].Begin.Month,
                                darwinCore.Event.Start.Month);
                Assert.IsTrue(searchCriteria.ObservationDateTime.PartOfYear[0].Begin.Day <= darwinCore.Event.Start.Day);
                Assert.IsTrue(searchCriteria.ObservationDateTime.PartOfYear[0].End.Day >= darwinCore.Event.Start.Day);
            }

            // Test search criteria ObserverIds.

            // Test search criteria Polygons.
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1500000, 6500000));
            linearRing.Points.Add(new WebPoint(1800000, 6500000));
            linearRing.Points.Add(new WebPoint(1800000, 6800000));
            linearRing.Points.Add(new WebPoint(1500000, 6500000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            Assert.IsTrue(information1.SpeciesObservations.IsNotEmpty());

            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(1300000, 6100000));
            linearRing.Points.Add(new WebPoint(1600000, 6100000));
            linearRing.Points.Add(new WebPoint(1600000, 6300000));
            linearRing.Points.Add(new WebPoint(1300000, 6100000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons.Add(polygon);
            information2 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);

            // Test search criteria ProjectIds.

            // Test search criteria RegionIds.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Hedgehog));
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.UpplandGuid);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            Assert.IsTrue(information1.SpeciesObservations.IsNotEmpty());

            searchCriteria.RegionGuids.Add(Settings.Default.BlekingeGUID);
            information2 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);

            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.VastmanlandCountyGuid);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            Assert.IsTrue(information1.SpeciesObservations.IsNotEmpty());

            // Test search criteria RegionLogicalOperator.

            // Test search criteria RegistrationDateTime.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.RegistrationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.RegistrationDateTime.Begin = new DateTime(2009, 8, 1);
            searchCriteria.RegistrationDateTime.End = new DateTime(2009, 8, 31);

            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);

            searchCriteria.RegistrationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.RegistrationDateTime.PartOfYear.Add(new WebDateTimeInterval());
            searchCriteria.RegistrationDateTime.PartOfYear[0].Begin = new DateTime(2009, 8, 1);
            searchCriteria.RegistrationDateTime.PartOfYear[0].End = new DateTime(2009, 8, 31);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);

            // Test search criteria SpeciesActivityIds.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.TwiteId);
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            birdNestActivity = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetBirdNestActivities(GetClientInformation())[0];
            searchCriteria.SpeciesActivityIds.Add(birdNestActivity.Id);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                if (darwinCore.DatasetName == "Fåglar")
                {
                    Assert.AreEqual(birdNestActivity.Name, darwinCore.Occurrence.Behavior);
                }
            }

            // Test search criteria TaxonIds.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            information1 = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.AreEqual(Settings.Default.DrumGrasshopperId, Int32.Parse(darwinCore.Taxon.TaxonID));
            }

            // Test search criteria TaxonValidationStatusIds.
        }

        [TestMethod]
        public void GetDarwinCoreChange()
        {
            DateTime changedFrom, changedTo;
            List<WebDarwinCore> speciesObservations;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreChange change;

            changedFrom = new DateTime(2007, 07, 16);
            changedTo = new DateTime(2007, 07, 16);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;
            change = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreChange(GetClientInformation(), changedFrom, changedTo, coordinateSystem);
            Assert.IsNotNull(change);
            Assert.IsTrue(0 < change.MaxSpeciesObservationCount);
            if (change.DeletedSpeciesObservationCount > 0)
            {
                Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
                Assert.AreEqual(change.DeletedSpeciesObservationCount, change.DeletedSpeciesObservationGuids.Count);
            }
            if (change.NewSpeciesObservationCount > 0)
            {
                Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
                Assert.AreEqual(change.NewSpeciesObservationCount, change.NewSpeciesObservations.Count);
            }
            if (change.UpdatedSpeciesObservationCount > 0)
            {
                Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
                Assert.AreEqual(change.UpdatedSpeciesObservationCount, change.UpdatedSpeciesObservations.Count);
            }

            // Test problem with GBIF not receiving all public 
            // species observation.
            speciesObservations = new List<WebDarwinCore>();
            foreach (WebDarwinCore webDarwinCore in change.NewSpeciesObservations)
            {
                if (webDarwinCore.Taxon.TaxonID == "101010")
                {
                    speciesObservations.Add(webDarwinCore);
                }
            }
            Assert.IsTrue(0 <= speciesObservations.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreChangeDateSwitchError()
        {
            DateTime changedFrom, changedTo;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreChange change;

            changedFrom = new DateTime(2011, 2, 2);
            changedTo = new DateTime(2011, 2, 1);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;
            change = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreChange(GetClientInformation(), changedFrom, changedTo, coordinateSystem);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreChangeFutureChangedToError()
        {
            DateTime changedFrom, changedTo;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreChange change;

            changedFrom = new DateTime(2011, 2, 1);
            changedTo = DateTime.Now;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;
            change = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetDarwinCoreChange(GetClientInformation(), changedFrom, changedTo, coordinateSystem);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.SwedishSpeciesObservationSOAPService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void HasConservationSpeciesObservation()
        {
            Boolean hasConservationSpeciesObservation;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);
            searchCriteria.IncludePositiveObservations = true;

            hasConservationSpeciesObservation = WebServiceProxy.SwedishSpeciesObservationSOAPService.HasConservationSpeciesObservation(GetClientInformation(),
                                                                                                                                       searchCriteria,
                                                                                                                                       coordinateSystem);
            Assert.IsTrue(hasConservationSpeciesObservation);
        }

        [TestMethod]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SwedishSpeciesObservationSOAPService.Login(Settings.Default.TestUserName,
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

            loginResponse = WebServiceProxy.SwedishSpeciesObservationSOAPService.Login(Settings.Default.TestUserName,
                                                                                       Settings.Default.TestPassword,
                                                                                       Settings.Default.DyntaxaApplicationIdentifier,
                                                                                       false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.SwedishSpeciesObservationSOAPService.Logout(clientInformation);
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.SwedishSpeciesObservationSOAPService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void StartTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        public void StopTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SwedishSpeciesObservationSOAPService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (_clientInformation.IsNotNull())
                {
                    WebServiceProxy.SwedishSpeciesObservationSOAPService.Logout(_clientInformation);
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
            TestInitialize(Settings.Default.PrintObsApplicationIdentifier);
        }

        public void TestInitialize(String applicationIdentifier)
        {
            WebLoginResponse loginResponse;

            TestCleanup();

            Configuration.InstallationType = InstallationType.ServerTest;
            //WebServiceProxy.SwedishSpeciesObservationSOAPService.WebServiceAddress = @"silurus2-1.artdata.slu.se/SwedishSpeciesObservationSOAPService/SwedishSpeciesObservationSOAPService.svc";
            loginResponse = WebServiceProxy.SwedishSpeciesObservationSOAPService.Login(Settings.Default.TestUserName,
                                                                                       Settings.Default.TestPassword,
                                                                                       applicationIdentifier,
                                                                                       false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Role = loginResponse.Roles[0];
            _clientInformation.Token = loginResponse.Token;
        }
#endif
    }
}
