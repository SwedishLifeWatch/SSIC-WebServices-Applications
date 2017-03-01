using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesObservationDataSource = ArtDatabanken.WebService.Client.SpeciesObservationService.SpeciesObservationDataSource;

namespace ArtDatabanken.WebService.Client.Test.SpeciesObservationService
{
    [TestClass]
    public class SpeciesObservationDataSourceTest : TestBase
    {
        private SpeciesObservationDataSource _speciesObservationDataSource;
        
        private void CheckDarwinCoreInformation(DarwinCoreList darwinCoreObservations)
        {
            Assert.IsNotNull(darwinCoreObservations);
            Assert.IsTrue(darwinCoreObservations.GetIds().IsNotEmpty());
        }

        private void CheckSpeciesObservationInformation(SpeciesObservationList speciesObservations)
        {
            Assert.IsNotNull(speciesObservations);
            Assert.IsTrue(speciesObservations.GetIds().IsNotEmpty());
        }

        private void CheckSpeciesObservationExist(SpeciesObservationList speciesObservations)
        {
            Assert.IsNotNull(speciesObservations);
            Assert.IsTrue(speciesObservations.Count > 0);
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesObservationDataSource speciesObservationDataSource;

            speciesObservationDataSource = new SpeciesObservationDataSource();
            Assert.IsNotNull(speciesObservationDataSource);
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            SpeciesActivityList birdNestActivities;

            birdNestActivities = GetSpeciesObservationDataSource(true).GetBirdNestActivities(GetUserContext());
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        [TestMethod]
        public void GetCountyRegions()
        {
            RegionList regions;
            regions = GetSpeciesObservationDataSource(true).GetCountyRegions(GetUserContext());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetProvinceRegions()
        {
            RegionList regions;
            regions = GetSpeciesObservationDataSource(true).GetProvinceRegions(GetUserContext());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreByIds()
        {
            DarwinCoreList darwinCoreObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;
             
            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);
            speciesObservationIds.Add(671);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), speciesObservationIds, coordinateSystem);
           
            Assert.IsTrue(darwinCoreObservations.IsNotNull());
            //Test for development only, id is not stable
            //Assert.AreEqual(darwinCoreObservations[3].Taxon.VernacularName, "rödbena");
            
            speciesObservationIds.Clear();
            darwinCoreObservations.Clear();

            for (int i=1; i < 20011; i++)
            {
                speciesObservationIds.Add(i);
            }

            darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), speciesObservationIds, coordinateSystem);
            Assert.IsTrue(darwinCoreObservations.IsNotNull());
            Assert.IsTrue(darwinCoreObservations.Count > 15000);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof (NullReferenceException))]
        public void GetDarwinCoreByIdsFailUserContext()
        {
            DarwinCoreList darwinCoreObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCore(null, speciesObservationIds, coordinateSystem);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(ArgumentNullException))]
        public void GetDarwinCoreByIdsFailSpeciesObservationsIds()
        {
            DarwinCoreList darwinCoreObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), null, coordinateSystem);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(ArgumentNullException))]
        public void GetDarwinCoreByIdsFailCoordinateSystem()
        {
            DarwinCoreList darwinCoreObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), speciesObservationIds, null);
            Assert.IsTrue(darwinCoreObservations.IsNotEmpty());

            // Ska man testa id = 0?
            //  speciesObservationIds.Add(0);
            //  darwinCoreObservations = GetSpeciesObservationDataSource(true).GetDarwinCoreByIds(GetUserContext(), speciesObservationIds, coordinateSystem);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteria()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria
            {
                TaxonIds = new List<Int32> { (Int32) TaxonId.DrumGrasshopper },
                IncludePositiveObservations = true
            };
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            DarwinCoreList darwinCores = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);

            CheckDarwinCoreInformation(darwinCores);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPage()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria
            {
                TaxonIds = new List<Int32> { (Int32)TaxonId.DrumGrasshopper },
                IncludePositiveObservations = true
            };
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ISpeciesObservationPageSpecification pageSpecification = new SpeciesObservationPageSpecification
            {
                Size = 10,
                SortOrder = new SpeciesObservationFieldSortOrderList(),
                Start = 1
            };
            DarwinCoreList darwinCores = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, pageSpecification);

            CheckDarwinCoreInformation(darwinCores);
            Assert.IsTrue(darwinCores.Count == pageSpecification.Size);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaAccuracy()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.Accuracy = 60;
            observations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations1);
            foreach (IDarwinCore darwinCore in observations1)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            observations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations2);
            Assert.IsTrue(observations2.Count < observations1.Count);
            foreach (IDarwinCore darwinCore in observations2)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaAccuracyArgumentError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.Accuracy = -1;
            observations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations);
            foreach (IDarwinCore darwinCore in observations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimit()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.BirdNestActivityLimit = CoreData.SpeciesObservationManager.GetBirdNestActivities(GetUserContext())[3];
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            searchCriteria.BirdNestActivityLimit = null;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservation2.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimitArgumentError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BirdNestActivityLimit = new SpeciesActivity();
            searchCriteria.BirdNestActivityLimit.Id = -1000;
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBoundingBox()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1632635, 6670116);
            searchCriteria.BoundingBox.Min = new Point(1300000, 6000000);
            observations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations1);

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new Point(1501658, 6508484);
            observations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations2);
            Assert.IsTrue(observations2.Count < observations1.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetDarwinCoreBySearchCriteriaBoundingBoxNullMaxError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Min = new Point(1562902, 6618355);
            observations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaChangeDateTime()
        {
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations1, observations2;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 8, 1);
            observations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations1);
            foreach (IDarwinCore speciesObservation in observations1)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }

            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 10, 1);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            observations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations2);
            foreach (IDarwinCore speciesObservation in observations2)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }
            Assert.IsTrue(observations2.Count > observations1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaChangeDateTimeInterval()
        {
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // Find changed observations with interval.
            // Test changed observations PartOfYear with a excluding interval over a newyearsday.
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2012, 12, 31);

            dateTimeInterval = new DateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2000, 12, 20);
            dateTimeInterval.End = new DateTime(2001, 1, 10);
            dateTimeInterval.IsDayOfYearSpecified = true;

            searchCriteria.ChangeDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            observations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations);
            int excludeSize = observations.Count;
            foreach (IDarwinCore speciesObservation in observations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.DayOfYear));

            }

            // Test changed observations PartOfYear with including interval over a newyearsday
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Including;

            observations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations);
            int includeSize = observations.Count;
            foreach (IDarwinCore speciesObservation in observations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.DayOfYear));
            }

            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCoordinateConversion()
        {
            SpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList darwinCoreObservations1;
            CoordinateSystem coordinateSystem;
            DataContext dataContext;
            Point max, min;
            
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            dataContext = new DataContext(GetUserContext());
            
            max = new Point(17.9, 65.0);
            min = new Point(17.0, 55.9);
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = new BoundingBox(max, min, dataContext);
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            darwinCoreObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), (ISpeciesObservationSearchCriteria)searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(darwinCoreObservations1);
            Assert.AreEqual(darwinCoreObservations1.Count, darwinCoreObservations1.GetIds().Count);
        }

        // Test search GetDarwinCoreBySearch criteria DataFieldSearchCriteria.

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuids()
        {
            SpeciesObservationDataProviderList speciesObservationDataProviders;
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList observations1, observations2;

            speciesObservationDataProviders = GetSpeciesObservationDataSource(true).GetSpeciesObservationDataProviders(GetUserContext());
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataSourceGuids.Add(speciesObservationDataProviders[3].Guid);
            observations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations1);
            foreach (IDarwinCore darwinCore in observations1)
            {
                Assert.AreEqual(speciesObservationDataProviders[3].Name, darwinCore.DatasetName);
            }

            searchCriteria.DataSourceGuids.Add(speciesObservationDataProviders[1].Guid);
            observations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations2);
            Assert.IsTrue(observations2.Count >= observations1.Count);
            foreach (IDarwinCore darwinCore in observations2)
            {
                Assert.IsTrue((speciesObservationDataProviders[3].Name == darwinCore.DatasetName) ||
                              (speciesObservationDataProviders[1].Name == darwinCore.DatasetName));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuidsUnknownGuidError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataSourceGuids.Add("None data provider GUID");
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeNeverFoundObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList speciesObservations1, speciesObservation2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101213); // Linsräka.

            searchCriteria.IncludeNeverFoundObservations = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            searchCriteria.IncludeNeverFoundObservations = false;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            foreach (IDarwinCore darwinCore in speciesObservation2)
            {
                Assert.IsFalse(darwinCore.Occurrence.IsNeverFoundObservation);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeNotRediscoveredObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList speciesObservations1, speciesObservations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100119); // Gölgroda.

            searchCriteria.IncludeNotRediscoveredObservations = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            searchCriteria.IncludeNotRediscoveredObservations = false;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            foreach (IDarwinCore darwinCore in speciesObservations2)
            {
                Assert.IsFalse(darwinCore.Occurrence.IsNotRediscoveredObservation);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludePositiveObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            DarwinCoreList speciesObservations1, speciesObservations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.IncludePositiveObservations = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            searchCriteria.IncludePositiveObservations = false;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            if (speciesObservations2.IsNotEmpty())
            {
                foreach (IDarwinCore darwinCore in speciesObservations2)
                {
                    Assert.IsFalse(darwinCore.Occurrence.IsPositiveObservation);
                }
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeRedListCategories()
        {
            RedListCategory redListCategory;
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            GetSpeciesObservationDataSource(true);
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
                searchCriteria.IncludeRedListCategories.Add(redListCategory);
                speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
                CheckDarwinCoreInformation(speciesObservations);
                foreach (IDarwinCore darwinCore in speciesObservations)
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
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList observations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.LC);
            observations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(observations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeRedlistedTaxa()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservation1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 9;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221940); // Mellanlummer.

            searchCriteria.IncludeRedlistedTaxa = true;
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            if (speciesObservation1.IsNotEmpty())
            {
                foreach (IDarwinCore darwinCore in speciesObservation1)
                {
                    Assert.IsTrue(darwinCore.Conservation.RedlistCategory.IsNotEmpty() ||
                                  darwinCore.Taxon.DyntaxaTaxonID == 222489);
                }
            }

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221940); // Mellanlummer.
            searchCriteria.IncludeRedlistedTaxa = false;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);
            if (speciesObservation2.IsNotEmpty())
            {
                foreach (IDarwinCore darwinCore in speciesObservation2)
                {
                    Assert.AreEqual(221940, darwinCore.Taxon.DyntaxaTaxonID);
                }
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsAccuracyConsidered()
        {
            DarwinCoreList speciesObservations1, speciesObservations2;
            CoordinateSystem coordinateSystem;
            ILinearRing linearRing;
            IPolygon polygon;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test with bounding box.
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new Point(1501658, 6508484);
            searchCriteria.IsAccuracyConsidered = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count <= speciesObservations1.Count);
            searchCriteria.BoundingBox = null;

            // Test with polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1370000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6460000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.IsAccuracyConsidered = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count <= speciesObservations1.Count);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.UpplandGUID);
            searchCriteria.IsAccuracyConsidered = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count <= speciesObservations1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsDisturbanceSensitivityConsidered()
        {
            DarwinCoreList speciesObservation1, speciesObservation2;
            ILinearRing linearRing;
            IPolygon polygon;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101316); // grön hedvårtbitare.

            // Test with bounding box.
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new Point(1360000, 6000000);

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v),(SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.BoundingBox = null;

            // Test with polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1700000, 6700000));
            linearRing.Points.Add(new Point(1360000, 6700000));
            linearRing.Points.Add(new Point(1360000, 6000000));
            linearRing.Points.Add(new Point(1700000, 6000000));
            linearRing.Points.Add(new Point(1700000, 6700000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Skane);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.RegionGuids = null;

            // Test with accuracy.
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new Point(1360000, 6000000);
            searchCriteria.IsAccuracyConsidered = true;

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.BoundingBox = null;
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsNaturalOccurrence()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100017); // Klockgroda.

            searchCriteria.IsNaturalOccurrence = true;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            if (speciesObservations1.IsNotEmpty())
            {
                foreach (IDarwinCore darwinCore in speciesObservations1)
                {
                    Assert.IsTrue(darwinCore.Occurrence.IsNaturalOccurrence);
                }
            }

            searchCriteria.IsNaturalOccurrence = false;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            if (speciesObservations2.IsNotEmpty())
            {
                foreach (IDarwinCore darwinCore in speciesObservations2)
                {
                    Assert.IsFalse(darwinCore.Occurrence.IsNaturalOccurrence);
                }
            }

            searchCriteria.IsNaturalOccurrence = null;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count > speciesObservations1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.StartsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.ToLower().Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));

            }

            searchCriteria.LocalityNameSearchString.SearchString = "backar";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.EndsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "%Full%";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString.Substring(1, 4)));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsFalse(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString));

            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringOperatorsError()
        {
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringNoOperatorError()
        {
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaMaxProtectionLevel()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservation1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.MaxProtectionLevel = 5;
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            foreach (IDarwinCore darwinCore in speciesObservation1)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel.Value);
            }

            searchCriteria.MaxProtectionLevel = 1;
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            foreach (IDarwinCore darwinCore in speciesObservation2)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel.Value);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaMinProtectionLevel()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(626); // Ljungögontröst
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.MinProtectionLevel = 1;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            foreach (IDarwinCore darwinCore in speciesObservations1)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel >= searchCriteria.MinProtectionLevel);
            }

            searchCriteria.MinProtectionLevel = 4;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            Assert.IsTrue(speciesObservations2.IsEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTime()
        {
            DarwinCoreList speciesObservations1, speciesObservations2;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            foreach (IDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1970, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            foreach (IDarwinCore speciesObservation in speciesObservations2)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(speciesObservations2.Count < speciesObservations1.Count);

            // Test Operator on Begin and End.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            foreach (IDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(speciesObservation.Event.Start <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(speciesObservation.Event.End >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            foreach (IDarwinCore speciesObservation in speciesObservations2)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(speciesObservations2.Count < speciesObservations1.Count);

            // Test Accuracy.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(4000, 0, 0, 0);
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            foreach (IDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(400, 0, 0, 0);
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            foreach (IDarwinCore speciesObservation in speciesObservations2)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }
            Assert.IsTrue(speciesObservations2.Count < speciesObservations1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTimeInterval()
        {
            // Tests when the interval is within a year
            // (not over new year's eve)
            // Tests day of year and date (ie Month-Day) queries
            // Tests Excluding and Including

            Int32 excludeSize, includeSize;
            DarwinCoreList speciesObservations;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add(205479); // Stormfågel

            dateTimeInterval = new DateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2012, 1, 1);
            dateTimeInterval.End = new DateTime(2012, 3, 1);

            // Day of year - excluding.
            dateTimeInterval.IsDayOfYearSpecified = true;
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            excludeSize = speciesObservations.Count;
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // Day of year - including.
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            includeSize = speciesObservations.Count;
            foreach (IDarwinCore speciesObservation in speciesObservations)
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
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            excludeSize = speciesObservations.Count;
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            includeSize = speciesObservations.Count;
            foreach (IDarwinCore speciesObservation in speciesObservations)
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
            DarwinCoreList speciesObservations;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // find observations with interval

            // TEST WITHOUT INTERVAL FOR COMPARING REASON
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 12, 29);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 1, 5);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            excludeSize = speciesObservations.Count;

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            includeSize = speciesObservations.Count;
            //TEST WITHOUT INTERVAL FOR COMPARING REASON
            Assert.IsTrue(includeSize >= excludeSize);

            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            dateTimeInterval = new DateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2009, 12, 29);
            dateTimeInterval.End = new DateTime(2010, 1, 5);

            // DAY OF YEAR - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = true;
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            excludeSize = speciesObservations.Count;

            // DAY OF YEAR - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            includeSize = speciesObservations.Count;
            //DAY INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);

            // DATE - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = false;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            excludeSize = speciesObservations.Count;

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            includeSize = speciesObservations.Count;

            //DATE INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);
        }

        // Test search GetDarwinCoreBySearch criteria ObserverIds.

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObserverSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "oskar";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);

            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.ToLower().Contains(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            searchCriteria.ObserverSearchString.SearchString = "Kindvall";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.EndsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.ObserverSearchString.SearchString = "";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.ObserverSearchString.SearchString = "Oskar Kindva%";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString.Substring(0, 10)));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.ObserverSearchString.SearchString = "";
            speciesObservations = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
            foreach (IDarwinCore speciesObservation in speciesObservations)
            {
                Assert.IsFalse(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringNoOperatorError()
        {
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringOperatorsError()
        {
            DarwinCoreList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPolygons()
        {
            DarwinCoreList speciesObservations1, speciesObservations2;
            ILinearRing linearRing;
            IPolygon polygon;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1000000, 7000000));
            linearRing.Points.Add(new Point(1000000, 5000000));
            linearRing.Points.Add(new Point(2000000, 5000000));
            linearRing.Points.Add(new Point(2000000, 7000000));
            linearRing.Points.Add(new Point(1000000, 7000000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.AreEqual(speciesObservations1.Count, speciesObservations2.Count);

            // Test with smaler polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1370000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6460000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations1.Count > speciesObservations2.Count);

            // Test with points in reverse order.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1000000, 7000000));
            linearRing.Points.Add(new Point(2000000, 7000000));
            linearRing.Points.Add(new Point(2000000, 5000000));
            linearRing.Points.Add(new Point(1000000, 5000000));
            linearRing.Points.Add(new Point(1000000, 7000000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
        }

        // Test search GetDarwinCoreBySearch criteria ProjectIds.

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegionGuids()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservation1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);

            // Test adding the same region twice.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.AreEqual(speciesObservation1.Count, speciesObservations2.Count);

            // Test adding another region.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservation1.Count < speciesObservations2.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownRegionGuidError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("Unknown region guid");
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        // Test search GetDarwinCoreBySearch criteria RegionLogicalOperator.

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegistrationDateTime()
        {
            ICoordinateSystem coordinateSystem;
            DarwinCoreList speciesObservation1, speciesObservation2;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 8, 1);
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            foreach (IDarwinCore speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }

            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 10, 1);
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            foreach (IDarwinCore speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }
            Assert.IsTrue(speciesObservation2.Count > speciesObservation1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaRegistrationDateTimeInterval()
        {
            ICoordinateSystem coordinateSystem;
            DarwinCoreList speciesObservation1;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            // Find reported observations with interval.
            // Test reported observations PartOfYear with a excluding interval over a newyearsday.
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            dateTimeInterval = new DateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2000, 12, 20);
            dateTimeInterval.End = new DateTime(2001, 1, 10);
            dateTimeInterval.IsDayOfYearSpecified = true;

            searchCriteria.ReportedDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            speciesObservation1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            int excludeSize = speciesObservation1.Count;
            foreach (IDarwinCore speciesObservation in speciesObservation1)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.DayOfYear));
            }

            // Test reported observations PartOfYear with including interval over a newyearsday
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Including;

            speciesObservation1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation1);
            int includeSize = speciesObservation1.Count;
            foreach (IDarwinCore speciesObservation in speciesObservation1)
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
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationDataSource(true).GetSpeciesActivities(GetUserContext())[1].Id);
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationDataSource().GetSpeciesActivities(GetUserContext())[4].Id);
            speciesObservation2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservation2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservation2.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaSpeciesActivityIdsUnknownIdError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(1000000);
            speciesObservations = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaTaxonIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 6, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = this.GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
            foreach (IDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            speciesObservations2 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservations2.Count);

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222226); // Knylhavre
            speciesObservations1 = this.GetSpeciesObservationDataSource().GetDarwinCore(GetUserContext(), searchCriteria, (ICoordinateSystem)this.GetCoordinateSystem(), (SpeciesObservationFieldSortOrderList)null);
            CheckDarwinCoreInformation(speciesObservations1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPagedTaxonIds()
        {
            SpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Settings.Default.DrumGrasshopperId);

            ISpeciesObservationPageSpecification pageSpecification = new SpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new SpeciesObservationFieldSortOrderList();

            ISpeciesObservationFieldSortOrder startSortOrder = new SpeciesObservationFieldSortOrder();
            startSortOrder.Class = new SpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new SpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            try
            {
                pageSpecification.Size = ArtDatabanken.Data.Settings.Default.SpeciesObservationPageMaxSize + 1;
                Assert.Fail("Should have been an exception here!");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException);
            }

            pageSpecification.Size = 20;
            for (int i = 1; i < 100; i = i + (int) pageSpecification.Size)
                {
                    pageSpecification.Start = i;

                    speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, pageSpecification, null);
                    if (speciesObservations1.IsNotEmpty())
                    {
                        CheckSpeciesObservationInformation(speciesObservations1);

                        Assert.AreEqual(speciesObservations1.Count, speciesObservations1.GetIds().Count);
                        foreach (ISpeciesObservation speciesObservation in speciesObservations1)
                        {
                            Assert.AreEqual(Settings.Default.DrumGrasshopperId, speciesObservation.Taxon.DyntaxaTaxonID);
                        }
                    }
                }
           
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaSortOrderStart()
        {
            SpeciesObservationSearchCriteria searchCriteria;
            DarwinCoreList speciesObservations1, speciesObservations2;

            SpeciesObservationFieldSortOrderList sortOrderList = new SpeciesObservationFieldSortOrderList();

            SpeciesObservationFieldSortOrder startSortOrder = new SpeciesObservationFieldSortOrder();
            startSortOrder.Class = new SpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new SpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            sortOrderList.Add(startSortOrder);

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, GetCoordinateSystem(), sortOrderList);
            //CheckDarwinCoreInformation(speciesObservations1);
            int i = 0;
            Debug.WriteLine("----- ASC -----");
            foreach (IDarwinCore speciesObservation in speciesObservations1)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            //Change Sort Order and try again
            startSortOrder.SortOrder = SortOrder.Descending;
            sortOrderList.Clear();
            sortOrderList.Add(startSortOrder);
            speciesObservations2 = GetSpeciesObservationDataSource(true).GetDarwinCore(GetUserContext(), searchCriteria, GetCoordinateSystem(), sortOrderList);
            i = 0;
            Debug.WriteLine("----- DESC -----");
            foreach (IDarwinCore speciesObservation in speciesObservations2)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }
            Debug.WriteLine("----------");
            Assert.IsTrue(speciesObservations1.Count <= speciesObservations2.Count);

        }


        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPage_SortOrderStart()
        {
            SpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservations2;

            SpeciesObservationPageSpecification pageSpecification = new SpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new SpeciesObservationFieldSortOrderList();

            SpeciesObservationFieldSortOrder startSortOrder = new SpeciesObservationFieldSortOrder();
            startSortOrder.Class = new SpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new SpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Start = 1;
            pageSpecification.Size = 100;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            //CheckDarwinCoreInformation(speciesObservations1);
            int i = 0;
            Debug.WriteLine("----- ASC -----");
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.ToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            //Change Sort Order and try again
            startSortOrder.SortOrder = SortOrder.Descending;
            pageSpecification.SortOrder.Clear();
            pageSpecification.SortOrder.Add(startSortOrder);
            speciesObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            i = 0;
            Debug.WriteLine("----- DESC -----");
            foreach (ISpeciesObservation speciesObservation in speciesObservations2)
            {
                Debug.WriteLine(i++ + " : " + speciesObservation.Event.Start.ToString() + " : " + speciesObservation.Id);
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }
            Debug.WriteLine("----------");
            Assert.IsTrue(speciesObservations1.Count <= speciesObservations2.Count);
        }      

        // Test search GetDarwinCoreBySearch criteria TaxonValidationStatusIds.

        [TestMethod]
        public void GetDarwinCoreChange()
        {
            CoordinateSystem coordinateSystem;

            DateTime? changedFrom = new DateTime(2013, 6, 26);
            DateTime? changedTo = new DateTime(2013, 6, 27);
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            const long maxReturnedChanges = 10000;
            Int64? changeId = null; //27091302;//16551900; //16561900; // 11109018; //11189018;

            SpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            IDarwinCoreChange change = GetSpeciesObservationDataSource(true).GetDarwinCoreChange(GetUserContext(), 
                                                                    changedFrom,
                                                                    changedTo,
                                                                    changeId, 
                                                                    maxReturnedChanges,
                                                                    searchCriteria, coordinateSystem);
            Assert.IsNotNull(change);
            Assert.IsTrue(0 < change.MaxChangeCount);
            if (change.DeletedSpeciesObservationGuids.Count > 0)
            {
                Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
            }
            if (change.CreatedSpeciesObservations.Count > 0)
            {
                Assert.IsTrue(change.CreatedSpeciesObservations.IsNotEmpty());
            }
            if (change.UpdatedSpeciesObservations.Count > 0)
            {
                Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetDarwinCoreChange_SearchCriteria()
        {  
            CoordinateSystem coordinateSystem= new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;

            SpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 5, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221917);

            Int64? changeId = null; //16551900; //16561900; // 11109018; //11189018;
            DateTime? changedFrom = new DateTime(2013, 6, 26);
            DateTime? changedTo = new DateTime(2013, 6, 27);
            const Int64 maxReturnedChanges = 10000;

            IDarwinCoreChange change = GetSpeciesObservationDataSource(true).GetDarwinCoreChange(GetUserContext(),
                                                                    changedFrom, 
                                                                    changedTo, 
                                                                    changeId, 
                                                                    maxReturnedChanges,
                                                                    searchCriteria, coordinateSystem);
            Assert.IsNotNull(change);
            Assert.IsTrue(0 < change.MaxChangeCount);
            if (change.DeletedSpeciesObservationGuids.Count > 0)
            {
                Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
            }
            if (change.CreatedSpeciesObservations.Count > 0)
            {
                Assert.IsTrue(change.CreatedSpeciesObservations.IsNotEmpty());
            }
            if (change.UpdatedSpeciesObservations.Count > 0)
            {
                Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            }
        }
        private ISpeciesObservationSpecification GetNullSpeciesObservationSpecification()
        {
            return null;
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationAccuracy()
        {
            Boolean hasProtectedSpeciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new Point(1580000, 6640000);
            searchCriteria.MinProtectionLevel = 2;

            searchCriteria.Accuracy = 1000;
            hasProtectedSpeciesObservations = GetSpeciesObservationDataSource(true).GetProtectedSpeciesObservationIndication(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;

            searchCriteria.Accuracy = 10;
            hasProtectedSpeciesObservations = GetSpeciesObservationDataSource().GetProtectedSpeciesObservationIndication(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            Assert.IsFalse(hasProtectedSpeciesObservations);
        }

        private SpeciesObservationDataSource GetSpeciesObservationDataSource(Boolean refresh = false)
        {
            if (_speciesObservationDataSource.IsNull() || refresh)
            {
                _speciesObservationDataSource = new SpeciesObservationDataSource();
            }

            return _speciesObservationDataSource;
        }

        [TestMethod]
        public void GetSpeciesObservationsByIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            List<Int64> speciesObservationIds;
            SpeciesObservationList speciesObservations;

            // Get some species observations.
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.AddTaxon((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.SetObservationDataTime(new DateTime(2015, 07, 01),
                                                  new DateTime(2015, 07, 30),
                                                  CompareOperator.Excluding);
            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), GetNullSpeciesObservationSpecification(), null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());

            // Get species observations by ids.
            speciesObservationIds = new List<Int64>();
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                speciesObservationIds.Add(speciesObservation.Id);
            }

            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), speciesObservationIds, GetCoordinateSystem(), null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, speciesObservationIds.Count);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(NullReferenceException))]
        public void GetSpeciesObservationsByIdsFailUserContext()
        {
            SpeciesObservationList speciesObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(null, speciesObservationIds, coordinateSystem, null);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(ArgumentException))]
        public void GetSpeciesObservationsByIdsFailSpeciesObservationsIds()
        {
            SpeciesObservationList speciesObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), null, coordinateSystem, null);
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(ArgumentNullException))]
        public void GetSpeciesObservationsByIdsFailCoordinateSystem()
        {
            SpeciesObservationList speciesObservations;
            List<Int64> speciesObservationIds;
            ICoordinateSystem coordinateSystem;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(1);
            speciesObservationIds.Add(2);
            speciesObservationIds.Add(3);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            coordinateSystem.WKT = "None";

            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), speciesObservationIds, null, null);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Set smaller interval than default
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 07, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);

            Int64 noOfObservations = this.GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCriteriasSetTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = null;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCoordinateSystemSetTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = null;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaAccurrancyTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 1200;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 > noOfObservations);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurancyFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Don't use accurancy, all positiv observations should be collected
            searchCriteria.Accuracy = null;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            // Enable Accurancy
            searchCriteria.Accuracy = 50;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 < noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaIsNaturalOccurrenceTest()
        {
            ICoordinateSystem coordinateSystem;
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;

            searchCriteria.IncludePositiveObservations = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            searchCriteria.IsNaturalOccurrence = true;
            // searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);


            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_GoogleMercator_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new Point(1113195, 1118890);

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_WGS84_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);



        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_SWEREF99_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new Point(820000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_25_gon_v_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNoneTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 2;
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(90, 90);
            searchCriteria.BoundingBox.Min = new Point(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxInvalidMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new Point(1113195, 31118890);
                    GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }

                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }

            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNullMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }

                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }

            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaChangeDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 07, 25);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2011, 01, 01);

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaChangePartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 01);
            interval.End = new DateTime(2010, 09, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 26);
            interval.End = new DateTime(2012, 08, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data 
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 07, 01);
            interval2.End = new DateTime(2012, 07, 25);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            // There is a bug here!!!! This is not working
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 07, 01);
            interval3.End = new DateTime(2012, 08, 31);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            // Bug Bug Assert.IsTrue(noOfObservations >= noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 >= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaDataProvidersTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            //searchCriteria.Accuracy = 1;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaDataProviderInvalidTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds = null;
            searchCriteria.Accuracy = 100;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityAllConditionsTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            // Can only set one stringCompareOperator 
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria operatorString = new StringSearchCriteria();
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            // Can only set one stringCompareOperator 
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationTypeTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations7 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations8 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > noOfObservations);
            Assert.IsTrue(noOfObservations5 > noOfObservations4);
            // TODO should be 0
            Assert.IsTrue(noOfObservations6 == noOfObservations);
            Assert.IsTrue(noOfObservations7 > noOfObservations);
            Assert.IsTrue(noOfObservations8 > noOfObservations3);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);


            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            // Enlarge the search area regarding time
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
            Assert.IsTrue(noOfObservations3 >= noOfObservations2);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Greater;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateInvalidDatesTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;
            // Get obe and a half year data
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 01);
            interval.End = new DateTime(2010, 09, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 12, 01);
            interval.End = new DateTime(2010, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2011, 01, 01);
            interval2.End = new DateTime(2011, 01, 31);
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 12, 01);
            interval3.End = new DateTime(2011, 01, 31);
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations > noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 <= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearIsDayOfYearSpecifiedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 11, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;


            // Get less amount of data since only two mounth within a year
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);


            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 12, 31);
            interval2.End = new DateTime(2011, 01, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval to the first one from nov to jan
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 11, 01);
            interval3.End = new DateTime(2011, 01, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Not using day of year
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations4 >= noOfObservations3);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearFailedTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2008, 06, 01);
            interval.End = new DateTime(2008, 03, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));

            // Test search criteria Polygons.
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsDifferentCoordinateSystemsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Create polygon
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            //GoogleMercator
            ICoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new CoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839)); //Uppsala E-N
            linearRing.Points.Add(new Point(1444869.9949174046, 8667823.4407080747));  //Tandådalen
            linearRing.Points.Add(new Point(1689906.68069054, 8241460.585692808));   //Örebro
            linearRing.Points.Add(new Point(2041443.6138615266, 7896601.1644738344));   //Visby
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystemMercator);
            //RT90
            ICoordinateSystem coordinateSystemRT90;
            coordinateSystemRT90 = new CoordinateSystem();
            coordinateSystemRT90.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883)); //Uppsala E-N
            linearRing.Points.Add(new Point(1348023.3768757628, 6788474.8526085708));  //Tandådalen
            linearRing.Points.Add(new Point(1464402.0051632109, 6573554.02424856));   //Örebro
            linearRing.Points.Add(new Point(1651196.4277014462, 6395804.2455542833));   //Visby
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystemRT90);

            //Rt90_25_gon_v
            ICoordinateSystem coordinateSystemRT90_25_gon_v;

            coordinateSystemRT90_25_gon_v = new CoordinateSystem();
            coordinateSystemRT90_25_gon_v.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883)); //Uppsala E-N
            linearRing.Points.Add(new Point(1348023.3768757628, 6788474.8526085708));  //Tandådalen
            linearRing.Points.Add(new Point(1464402.0051632109, 6573554.02424856));   //Örebro
            linearRing.Points.Add(new Point(1651196.4277014462, 6395804.2455542833));   //Visby
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystemRT90_25_gon_v);

            //SWEREF99
            ICoordinateSystem coordinateSystemSWEREF99;
            coordinateSystemSWEREF99 = new CoordinateSystem();
            coordinateSystemSWEREF99.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019)); //Uppsala E-N
            linearRing.Points.Add(new Point(391358.12244400941, 6784781.6853031125));  //Tandådalen
            linearRing.Points.Add(new Point(510296.21811902762, 6571401.8266052864));   //Örebro
            linearRing.Points.Add(new Point(699151.04601517064, 6395960.6072938712));   //Visby
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystemSWEREF99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 0.2 % difference in result
            double delta = noOfObservations * 0.002;
            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations == noOfObservations2);
            Assert.AreEqual(noOfObservations, noOfObservations3, delta);
            Assert.AreEqual(noOfObservations, noOfObservations4, delta);
            Assert.AreEqual(noOfObservations, noOfObservations5, delta);


        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 01, 01);

            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 04, 15);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 03, 31);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 02, 28);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data 
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2010, 04, 15);
            intervals2.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 02, 01);
            interval3.End = new DateTime(2010, 04, 15);
            intervals3.Add(interval3);
            searchCriteria.ReportedDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations > noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 <= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListCategoriesTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;



            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            searchCriteria.IncludeRedlistedTaxa = true;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListTaxaTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;
            Int64 noOfObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaTaxaTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088);// Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;



            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaUsedAllCriteriasTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));

            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            // Create polygon in WGS84
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            searchCriteria.DataSourceGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.


            Int64 noOfObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaAccuracy()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.Accuracy = 60;
            observations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(observations1);
            foreach (ISpeciesObservation speciesObservation in observations1)
            {
                Assert.IsTrue(Double.Parse(speciesObservation.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            observations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(observations2);
            Assert.IsTrue(observations2.Count < observations1.Count);
            foreach (ISpeciesObservation speciesObservation in observations2)
            {
                Assert.IsTrue(Double.Parse(speciesObservation.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBox()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservations2;
            CoordinateSystem coordinateSystem;
            DataContext dataContext;
            Point max, min;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            dataContext = new DataContext(GetUserContext());

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add((int)TaxonId.DrumGrasshopper);
            searchCriteria.IncludePositiveObservations = true;

            max = new Point(null, 1645000, 6681000, null, dataContext);
            min = new Point(null, 1308000, 6222000, null, dataContext);
            searchCriteria.BoundingBox = new BoundingBox(max, min, dataContext);

            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (SpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(speciesObservation.Location.CoordinateX <= searchCriteria.BoundingBox.Max.X);
                Assert.IsTrue(speciesObservation.Location.CoordinateX >= searchCriteria.BoundingBox.Min.X);
                Assert.IsTrue(speciesObservation.Location.CoordinateY <= searchCriteria.BoundingBox.Max.Y);
                Assert.IsTrue(speciesObservation.Location.CoordinateY >= searchCriteria.BoundingBox.Min.Y);

            }

            max = new Point(null, 1500000, 6500000, null, dataContext);
            min = new Point(null, 1400000, 6300000, null, dataContext);
            searchCriteria.BoundingBox = new BoundingBox(max, min, dataContext);

            speciesObservations2 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations2.Count < speciesObservations1.Count);
            foreach (SpeciesObservation speciesObservation in speciesObservations2)
            {
                Assert.IsTrue(speciesObservation.Location.CoordinateX <= searchCriteria.BoundingBox.Max.X);
                Assert.IsTrue(speciesObservation.Location.CoordinateX >= searchCriteria.BoundingBox.Min.X);
                Assert.IsTrue(speciesObservation.Location.CoordinateY <= searchCriteria.BoundingBox.Max.Y);
                Assert.IsTrue(speciesObservation.Location.CoordinateY >= searchCriteria.BoundingBox.Min.Y);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaCoordinateConversion()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((int)TaxonId.DrumGrasshopper);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(19, 61);
            searchCriteria.BoundingBox.Min = new Point(17, 59);
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations1.Count > 0);
        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaChangeDateTime not implemented.

        // TODO Test search GetSpeciesObservationsBySearchCriteriaDataFields not implemented.

        // TODO Test search GetSpeciesObservationsBySearchCriteriaDataSourceIds not implemented.

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeNeverFoundObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(101213); // Linsräka.

            searchCriteria.IncludeNeverFoundObservations = true;
            observations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(observations1);

            searchCriteria.IncludeNeverFoundObservations = false;
            observations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(observations2);
            foreach (ISpeciesObservation darwinCore in observations2)
            {
                Assert.IsFalse(darwinCore.Occurrence.IsNeverFoundObservation.Value);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeNotRediscoveredObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((int)TaxonId.DrumGrasshopper);
            searchCriteria.IncludeNotRediscoveredObservations = true;
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations1.Count > 0);
            foreach (SpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.IsNotNull(speciesObservation.Occurrence.IsNotRediscoveredObservation);
                Assert.IsTrue((bool)speciesObservation.Occurrence.IsNotRediscoveredObservation);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludePositiveObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList speciesObservations1, speciesObservations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.IncludePositiveObservations = true;
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);

            searchCriteria.IncludePositiveObservations = false;
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            if (speciesObservations2.IsNotEmpty())
            {
                foreach (ISpeciesObservation speciesObservation in speciesObservations2)
                {
                    Assert.IsTrue(speciesObservation.Occurrence.IsPositiveObservation.HasValue);
                    Assert.IsFalse(speciesObservation.Occurrence.IsPositiveObservation.Value);
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedListCategories()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.CR);
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations1.Count >= 0);
            foreach (SpeciesObservation speciesObservation in speciesObservations1)
            {
                // This check should not be necessary but data
                // on test server is not up to date.
                if (speciesObservation.Conservation.RedlistCategory.IsNotEmpty())
                {
                    Assert.AreEqual(searchCriteria.IncludeRedListCategories[0].ToString(),
                                    speciesObservation.Conservation.RedlistCategory.Substring(0, 2).ToUpper());
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedlistedTaxa()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 3;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations1.Count > 0);
            Assert.IsTrue(speciesObservations1.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaLocalityNameSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.StartsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            speciesObservations = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.ToLower().Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));

            }

            searchCriteria.LocalityNameSearchString.SearchString = "backar";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            speciesObservations = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.EndsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "%Full%";
            speciesObservations = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString.Substring(1, 4)));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsFalse(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString));

            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTime()
        {
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList speciesObservation1, speciesObservation2;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservation1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1970, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservation2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);

            // Test Operator on Begin and End.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            speciesObservation1 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(speciesObservation.Event.Start <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(speciesObservation.Event.End >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservation2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);

            // Test Accuracy.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(4000, 0, 0, 0);
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservation1 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(400, 0, 0, 0);
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservation2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);
        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaObserverIds not implemented.

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPageTaxonIds()
        {
            SpeciesObservationList speciesObservations;
            ISpeciesObservationFieldSortOrder startSortOrder;
            ISpeciesObservationPageSpecification pageSpecification;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            pageSpecification = new SpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new SpeciesObservationFieldSortOrderList();
            startSortOrder = new SpeciesObservationFieldSortOrder();
            startSortOrder.Class = new SpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new SpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Start = 1;
            pageSpecification.Size = 100;

            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.IsTrue(speciesObservations.Count <= pageSpecification.Size);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPolygons()
        {
            SpeciesObservationList speciesObservations1, speciesObservations2;
            ILinearRing linearRing;
            IPolygon polygon;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1000000, 7000000));
            linearRing.Points.Add(new Point(1000000, 5000000));
            linearRing.Points.Add(new Point(2000000, 5000000));
            linearRing.Points.Add(new Point(2000000, 7000000));
            linearRing.Points.Add(new Point(1000000, 7000000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.AreEqual(speciesObservations1.Count, speciesObservations2.Count);

            // Test with smaler polygon.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1370000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6240000));
            linearRing.Points.Add(new Point(1600000, 6460000));
            linearRing.Points.Add(new Point(1370000, 6460000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations1.Count > speciesObservations2.Count);

            // Test with points in reverse order.
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1000000, 7000000));
            linearRing.Points.Add(new Point(2000000, 7000000));
            linearRing.Points.Add(new Point(2000000, 5000000));
            linearRing.Points.Add(new Point(1000000, 5000000));
            linearRing.Points.Add(new Point(1000000, 7000000));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations1 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);
        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaProjectIds() not implemented.

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaGetProjectParameters()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            // Get species observations with project parameters.
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.AddTaxon(106752); // hjärtgrynsnäcka
            searchCriteria.SetObservationDataTime(new DateTime(2013, 10, 06),
                                                  new DateTime(2013, 10, 06),
                                                  CompareOperator.Excluding);
            speciesObservations = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), GetNullSpeciesObservationSpecification(), null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsNotNull(speciesObservation);
                Assert.IsNotNull(speciesObservation.Project);
                Assert.IsTrue(speciesObservation.Project.ProjectParameters.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaRegionIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservation1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation1);

            // Test adding the same region twice.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.AreEqual(speciesObservation1.Count, speciesObservations2.Count);

            // Test adding another region.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservation1.Count < speciesObservations2.Count);
        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaRegionLogicalOperator not implemented.

       

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaRegistrationDateTime()
        {
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList speciesObservation1, speciesObservation2;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 8, 1);
            speciesObservation1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }

            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 10, 1);
            speciesObservation2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }
            Assert.IsTrue(speciesObservation2.Count > speciesObservation1.Count);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationsBySearchCriteriaSpeciesActivityIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationDataSource(true).GetSpeciesActivities(GetUserContext())[1].Id);
            speciesObservations1 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservations1);

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationDataSource().GetSpeciesActivities(GetUserContext())[4].Id);
            speciesObservation2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), (ISpeciesObservationSpecification)null, null);
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservation2.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaTaxonIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationFieldSortOrderList sortOrder;
            SpeciesObservationList speciesObservations1, speciesObservations2;

            sortOrder = null;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 6, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            speciesObservations1 = GetSpeciesObservationDataSource(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            speciesObservations2 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservations2.Count);

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222226); // Knylhavre
            speciesObservations1 = GetSpeciesObservationDataSource().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations1);
        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaTaxonValidationStatusIds not implemented.

        [TestMethod]
        public void GetSpeciesObservationFieldDescriptions()
        {
            SpeciesObservationFieldDescriptionList list;
            IUserContext context = GetUserContext();
            list = GetSpeciesObservationDataSource(true).GetSpeciesObservationFieldDescriptions(context);
            Assert.IsTrue(list.Count > 100);
        }

        protected override String GetTestApplicationName()
        {
            return ApplicationIdentifier.PrintObs.ToString();
        }

        [TestMethod]
        public void SetDataSource()
        {
            CoreData.SpeciesObservationManager.DataSource = null;
            SpeciesObservationDataSource.SetDataSource();
            Assert.IsNotNull(CoreData.SpeciesObservationManager.DataSource);
        }

        public SpeciesObservationDataSourceTest()
        {
            _speciesObservationDataSource = null;
        }

        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            ISpeciesObservationChange speciesObservationChange;

            long? changeId = null;
            DateTime? changedFrom = new DateTime(2013, 10, 04);
            DateTime? changedTo = new DateTime(2013, 10, 05);
            const long MaxReturnedChanges = 4000;
            Boolean moreData = true;
            long currentMaxChangeId = 0;
            while (moreData)
            {
                speciesObservationChange = GetSpeciesObservationDataSource(true).GetSpeciesObservationChange(GetUserContext(),
                                                                                                             changedFrom,
                                                                                                             changedTo,
                                                                                                             changeId,
                                                                                                             MaxReturnedChanges,
                                                                                                             null,
                                                                                                             GetCoordinateSystem(),
                                                                                                             null);
                moreData = speciesObservationChange.IsMoreSpeciesObservationsAvailable;

                // Do not include last change id again. Thats why we add 1.
                changeId = speciesObservationChange.MaxChangeId + 1;

                changedFrom = null;

                Assert.IsTrue(currentMaxChangeId != speciesObservationChange.MaxChangeId);
                currentMaxChangeId = speciesObservationChange.MaxChangeId;
            }
        }

        [TestMethod]
        public void GetSpeciesObservationDataProvider()
        {
            SpeciesObservationDataProviderList speciesObservationDataProviders;
            speciesObservationDataProviders = new SpeciesObservationDataProviderList();
            speciesObservationDataProviders = GetSpeciesObservationDataSource(true).GetSpeciesObservationDataProviders(GetUserContext());
            Assert.IsNotNull(speciesObservationDataProviders);
            Assert.IsTrue(speciesObservationDataProviders[0].Id == 1);
            Assert.IsTrue(speciesObservationDataProviders[1].Id == 2);
            Assert.IsNotNull(speciesObservationDataProviders[0].Name);
            Assert.IsNotNull(speciesObservationDataProviders[1].Name);
            Assert.IsNotNull(speciesObservationDataProviders[2].Name);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivities()
        {
            SpeciesActivityList getSpeciesActivities;
            getSpeciesActivities = GetSpeciesObservationDataSource(true).GetSpeciesActivities(GetUserContext());

            Assert.IsTrue(getSpeciesActivities.IsNotEmpty());
  
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivityCategories()
        {
            SpeciesActivityCategoryList getSpeciesActivityCategories;
            getSpeciesActivityCategories = GetSpeciesObservationDataSource(true).GetSpeciesActivityCategories(GetUserContext());

            Assert.IsTrue(getSpeciesActivityCategories.IsNotEmpty());
        }
        private static void SetDefaultSearchCriteria(ISpeciesObservationSearchCriteria searchCriteria)
        {
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.IncludePositiveObservations = true;
        }
    }
}
