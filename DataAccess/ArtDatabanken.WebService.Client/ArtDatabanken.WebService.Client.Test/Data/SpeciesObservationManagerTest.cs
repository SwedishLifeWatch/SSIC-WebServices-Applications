using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using SpeciesObservationDataSource = ArtDatabanken.WebService.Client.SpeciesObservationService.SpeciesObservationDataSource;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesObservationManagerTest : TestBase
    {
        private SpeciesObservationManager _speciesObservationManager;

        public SpeciesObservationManagerTest()
        {
            _speciesObservationManager = null;
        }

        private void CheckSpeciesObservationExist(SpeciesObservationList speciesObservations)
        {
            Assert.IsNotNull(speciesObservations);
            Assert.IsTrue(speciesObservations.Count > 0);
        }

        [TestMethod]
        public void GetDarwinCoreChange()
        {
            IDarwinCoreChange darwinCoreChange;

            CoordinateSystem coordinateSystem;

            DateTime? changedFrom = new DateTime(2013, 6, 26);
            DateTime? changedTo = new DateTime(2013, 6, 27);
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            const long maxReturnedChanges = 1000;
            Int64? changeId = null; //16551900; //16561900; // 11109018; //11189018;

            SpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            darwinCoreChange = GetSpeciesObservationManager(true).GetDarwinCoreChange(GetUserContext(),
                                                                    changedFrom,
                                                                    changedTo,
                                                                    changeId,
                                                                    maxReturnedChanges,
                                                                    searchCriteria, coordinateSystem);
            Assert.IsTrue(darwinCoreChange.UpdatedSpeciesObservations.Count > 0);
        }

        [TestMethod]
        public void GetDarwinCoreChangeStockholmCity()
        {
            ICoordinateSystem coordinateSystem;
            IDarwinCoreChange darwinCoreChange;
            Int64 maxReturnedChanges;
            Int64? changeId;
            ISpeciesObservationSearchCriteria searchCriteria;

            changeId = 0;
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            maxReturnedChanges = 25000;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.IsNaturalOccurrence = true;
            //searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            //searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 1, 12);
            //searchCriteria.ObservationDateTime.End = new DateTime(2015, 1, 18);
            //searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet1Feature180"); // Stockholms kommun.
            //searchCriteria.TaxonIds = new List<Int32>();
            //searchCriteria.TaxonIds.Add(102107); // Brunand

            //do
            //{
                darwinCoreChange = GetSpeciesObservationManager(true).GetDarwinCoreChange(GetUserContext(),
                                                                                          null,
                                                                                          null,
                                                                                          changeId,
                                                                                          maxReturnedChanges,
                                                                                          searchCriteria,
                                                                                          coordinateSystem);
                changeId = darwinCoreChange.MaxChangeId;
            //}
            //while (darwinCoreChange.CreatedSpeciesObservations.IsEmpty() &&
            //       darwinCoreChange.UpdatedSpeciesObservations.IsEmpty());

            Assert.IsNotNull(darwinCoreChange);
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            SpeciesActivityList birdNestActivities;

            birdNestActivities = GetSpeciesObservationManager(true).GetBirdNestActivities(GetUserContext());
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        [TestMethod]
        public void GetCountyRegions()
        {
            RegionList regions;

            regions = GetSpeciesObservationManager(true).GetCountyRegions(GetUserContext());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetProvinceRegions()
        {
            RegionList regions;

            regions = GetSpeciesObservationManager(true).GetProvinceRegions(GetUserContext());
            Assert.IsTrue(regions.IsNotEmpty());
        }

        private SpeciesObservationManager GetSpeciesObservationManager(Boolean refresh = false)
        {
            if (_speciesObservationManager.IsNull() || refresh)
            {
                _speciesObservationManager = new SpeciesObservationManager();
                _speciesObservationManager.DataSource = new SpeciesObservationDataSource();
            }
            return _speciesObservationManager;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesObservationManager speciesObservationManager;

            speciesObservationManager = new SpeciesObservationManager();
            Assert.IsNotNull(speciesObservationManager);
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
            hasProtectedSpeciesObservations = GetSpeciesObservationManager(true).GetProtectedSpeciesObservationIndication(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;

            searchCriteria.Accuracy = 10;
            hasProtectedSpeciesObservations = GetSpeciesObservationManager().GetProtectedSpeciesObservationIndication(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            Assert.IsFalse(hasProtectedSpeciesObservations);
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationCountyAdministration()
        {
            Boolean hasProtectedSpeciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.BirdNestActivityLimit = CoreData.SpeciesObservationManager.GetBirdNestActivities(GetUserContext()).Get(18);
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new Point(1580000, 6640000);

            hasProtectedSpeciesObservations = GetSpeciesObservationManager(true).GetProtectedSpeciesObservationIndication(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;
        }

        private TaxonList GetRedlistedTaxa()
        {
            IFactor factor;
            RedListCategory redListCategory;
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria;
            TaxonList taxa;

            speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            speciesFactSearchCriteria.Factors = new FactorList();
            speciesFactSearchCriteria.Factors.Add(factor);
            speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            speciesFactSearchCriteria.Periods = new PeriodList();
            speciesFactSearchCriteria.Periods.Add(CoreData.FactorManager.GetCurrentPublicPeriod(GetUserContext()));

            speciesFactFieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
            speciesFactFieldSearchCriteria.FactorField = factor.DataType.Field1;
            speciesFactFieldSearchCriteria.Operator = CompareOperator.Equal;
            speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            speciesFactSearchCriteria.FieldSearchCriteria.Add(speciesFactFieldSearchCriteria);
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                speciesFactFieldSearchCriteria.AddValue((Int32)redListCategory);
            }

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), speciesFactSearchCriteria));
            return taxa;
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivities()
        {
            SpeciesActivityList speciesActivities = GetSpeciesObservationManager(true).GetSpeciesActivities(GetUserContext());

            foreach (SpeciesActivity speciesActivity in speciesActivities)
            {
                Assert.IsTrue(speciesActivity.IsNotNull());
            }
        }

        [Ignore]
        [TestMethod]
        public void GetSpeciesActivityCategories()
        {
            SpeciesActivityCategoryList speciesActivityCategories = GetSpeciesObservationManager(true).GetSpeciesActivityCategories(GetUserContext());
            foreach (SpeciesActivityCategory speciesActivityCategory in speciesActivityCategories)
            {
                Assert.IsTrue(speciesActivityCategory.IsNotNull());
            }
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivityCategory()
        {
            ISpeciesActivityCategory speciesActivityCategory = GetSpeciesObservationManager(true).GetSpeciesActivityCategory(GetUserContext(), 7);
        
            Assert.IsTrue(speciesActivityCategory.Identifier.IsNotNull());
            Assert.IsTrue(speciesActivityCategory.Name == "Död");
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
                speciesObservationChange = GetSpeciesObservationManager(true).GetSpeciesObservationChange(GetUserContext(),
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
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsNotNull(speciesObservation);
                Assert.IsNotNull(speciesObservation.Project);
                Assert.IsTrue(speciesObservation.Project.ProjectParameters.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsByIds()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            List<Int64> speciesObservationIds;
            SpeciesObservationList speciesObservations;

            // Get some species observations.
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.AddTaxon((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.SetObservationDataTime(new DateTime(2015, 07, 01),
                                                  new DateTime(2015, 07, 30),
                                                  CompareOperator.Excluding);
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(speciesObservations.IsNotEmpty());

            // Get species observations by ids.
            speciesObservationIds = new List<Int64>();
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                speciesObservationIds.Add(speciesObservation.Id);
            }

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), speciesObservationIds, GetCoordinateSystem(), null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, speciesObservationIds.Count);
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            searchCriteria.Accuracy = 60;
            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);
            foreach (ISpeciesObservation speciesObservation in observations1)
            {
                Assert.IsTrue(Double.Parse(speciesObservation.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations2);
            Assert.IsTrue(observations2.Count < observations1.Count);
            foreach (ISpeciesObservation speciesObservation in observations2)
            {
                Assert.IsTrue(Double.Parse(speciesObservation.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaAccuracyArgumentError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;


            searchCriteria.Accuracy = -1;

            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            observations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations);
            foreach (ISpeciesObservation speciesObservation in observations)
            {
                Assert.IsTrue(Double.Parse(speciesObservation.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBirdNestActivityLimit()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservation2;

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

            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);

            searchCriteria.BirdNestActivityLimit = null;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservation2.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaBirdNestActivityLimitArgumentError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BirdNestActivityLimit = new SpeciesActivity();
            searchCriteria.BirdNestActivityLimit.Id = -1000;

            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBox()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1632635, 6670116);
            searchCriteria.BoundingBox.Min = new Point(1300000, 6000000);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new Point(1501658, 6508484);
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations2);
            Assert.IsTrue(observations2.Count < observations1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBoxAndObservationDateTime()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationFieldList fieldList;
            SpeciesObservationList speciesObservations;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            fieldList = new SpeciesObservationFieldList();
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new Point(1580000, 6640000);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(new DateTimeInterval());
            searchCriteria.ObservationDateTime.PartOfYear[0].Begin = new DateTime(2013, 10, 01);
            searchCriteria.ObservationDateTime.PartOfYear[0].End = new DateTime(2013, 10, 31);
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBoxAndObservationDateTime2()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;            
            SpeciesObservationList speciesObservations;
            SpeciesObservationPageSpecification pageSpecification;
            SpeciesObservationFieldSortOrder sortOrder;
            Polygon polygon;
            LinearRing linearRing;

            pageSpecification = new SpeciesObservationPageSpecification();
            pageSpecification.Size = 25;
            pageSpecification.Start = 1;
            pageSpecification.SortOrder = new SpeciesObservationFieldSortOrderList();            
            sortOrder = new SpeciesObservationFieldSortOrder();
            sortOrder.Class = new SpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Event;
            sortOrder.Property = new SpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            sortOrder.SortOrder = SortOrder.Descending;
            pageSpecification.SortOrder.Add(sortOrder);

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new SpeciesObservationSearchCriteria();                        
            polygon = new Polygon {LinearRings = new List<ILinearRing>()};            
            linearRing = new LinearRing
                {
                    Points =
                        new List<IPoint>
                            {
                                new Point(1964062.5791284, 8358432.8520649),
                                new Point(1964062.5791284, 8358693.2156924),
                                new Point(1963768.774301, 8358693.2156924),
                                new Point(1963768.774301, 8358432.8520649),
                                new Point(1964062.5791284, 8358432.8520649)
                            }
                };
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon> {polygon};
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(new DateTimeInterval());
            searchCriteria.ObservationDateTime.PartOfYear[0].Begin = new DateTime(2013, 10, 01);
            searchCriteria.ObservationDateTime.PartOfYear[0].End = new DateTime(2013, 10, 31);
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, pageSpecification);
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBoxNullMaxError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Min = new Point(1562902, 6618355);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaChangeDateTime()
        {
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);
            foreach (ISpeciesObservation speciesObservation in observations1)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified.Value);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified.Value);
            }

            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 10, 1);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations2);
            foreach (ISpeciesObservation speciesObservation in observations2)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified.Value);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified.Value);
            }
            Assert.IsTrue(observations2.Count > observations1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaChangeDateTimeInterval()
        {
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
//            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations);
            int excludeSize = observations.Count;
            foreach (ISpeciesObservation speciesObservation in observations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.Value.DayOfYear));

            }

            // Test changed observations PartOfYear with including interval over a newyearsday
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Including;
            observations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations);
            int includeSize = observations.Count;
            foreach (ISpeciesObservation speciesObservation in observations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.Value.DayOfYear));
            }

            Assert.IsTrue(includeSize >= excludeSize);

            // Test with interval and day of year on leap year.
            //searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            //searchCriteria.ChangeDateTime.Begin = new DateTime(2012, 01, 01);
            //searchCriteria.ChangeDateTime.End = new DateTime(2013, 01, 01);
            //searchCriteria.ChangeDateTime.PartOfYear = new List<IDateTimeInterval>();
            //searchCriteria.ChangeDateTime.PartOfYear.Add(new DateTimeInterval());
            //searchCriteria.ChangeDateTime.PartOfYear[0].Begin = new DateTime(2014, 07, 01);
            //searchCriteria.ChangeDateTime.PartOfYear[0].End = new DateTime(2014, 07, 02);
            //searchCriteria.ChangeDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            //observations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            //CheckSpeciesObservationExist(observations);
            //foreach (ISpeciesObservation speciesObservation in observations)
            //{
            //    Assert.IsTrue((searchCriteria.ChangeDateTime.PartOfYear[0].Begin.Day <= speciesObservation.Modified.Value.Day) &&
            //                  (searchCriteria.ChangeDateTime.PartOfYear[0].End.Day >= speciesObservation.Modified.Value.Day));
            //}
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaCoordinateConversion()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1;
            CoordinateSystem coordinateSystem;
            DataContext dataContext;
            Point max, min;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            dataContext = new DataContext(GetUserContext());

            max = new Point(null, 17.7, 60.0, null, dataContext);
            min = new Point(null, 17.6, 59.9, null, dataContext);
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add((int)TaxonId.Butterflies);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = new BoundingBox(max, min, dataContext);
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations1);
            Assert.IsTrue(speciesObservations1.Count > 0);

        }

        // TODO Test search GetSpeciesObservationsBySearchCriteriaChangeDateTime not implemented.

        // TODO Test search GetSpeciesObservationsBySearchCriteriaDataFields not implemented.

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaDataSourceGuids()
        {
            ArtDatabanken.Data.SpeciesObservationDataProviderList speciesObservationDataProviders;
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            speciesObservationDataProviders = GetSpeciesObservationManager(true).GetSpeciesObservationDataProviders(GetUserContext());
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataSourceGuids.Add(speciesObservationDataProviders[3].Guid);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);
            foreach (ISpeciesObservation speciesObservation in observations1)
            {
                Assert.AreEqual(speciesObservationDataProviders[3].Name, speciesObservation.DatasetName);
            }

            searchCriteria.DataSourceGuids.Add(speciesObservationDataProviders[1].Guid);
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations2);
            Assert.IsTrue(observations2.Count >= observations1.Count);
            foreach (ISpeciesObservation speciesObservation in observations2)
            {
                Assert.IsTrue((speciesObservationDataProviders[3].Name == speciesObservation.DatasetName) ||
                              (speciesObservationDataProviders[1].Name == speciesObservation.DatasetName));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaDataProviderGuidsUnknownGuidError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataSourceGuids.Add("None data provider GUID");
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);

            searchCriteria.IncludeNeverFoundObservations = false;
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100119); // Gölgroda.

            searchCriteria.IncludeNotRediscoveredObservations = true;
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);

            searchCriteria.IncludeNotRediscoveredObservations = false;
            observations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations2);
            foreach (ISpeciesObservation speciesObservation in observations2)
            {
                Assert.IsFalse(speciesObservation.Occurrence.IsNotRediscoveredObservation.Value);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludePositiveObservations()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList observations1, observations2;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.IncludePositiveObservations = true;
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(observations1);

            searchCriteria.IncludePositiveObservations = false;
            observations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            if (observations2.IsNotEmpty())
            {
                foreach (SpeciesObservation speciesObservation in observations2)
                {
                    Assert.IsFalse(speciesObservation.Occurrence.IsPositiveObservation.Value);
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedListCategories()
        {
            RedListCategory redListCategory;
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            GetSpeciesObservationManager(true);
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
                searchCriteria.IncludeRedListCategories.Add(redListCategory);
                SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
                speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
                CheckSpeciesObservationExist(speciesObservations);
                foreach (ISpeciesObservation speciesObservation in speciesObservations)
                {
                    // The check for not empty red list category should not be
                    // necessary but data on test server is not up to date.
                    // This test has a problem with taxon 232265 that
                    // is redlisted as VU since its parent taxon is
                    // red listed as NT.
                    if (speciesObservation.Conservation.RedlistCategory.IsNotEmpty() &&
                        (speciesObservation.Taxon.DyntaxaTaxonID.Value != 232265))
                    {
                        Assert.AreEqual(redListCategory.ToString(),
                                        speciesObservation.Conservation.RedlistCategory.Substring(0, 2).ToUpper());
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedListCategoriesArgumentError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList observations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.LC);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            observations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(observations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedlistedTaxa()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 9;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221940); // Mellanlummer.

            searchCriteria.IncludeRedlistedTaxa = true;
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);
            if (speciesObservation1.IsNotEmpty())
            {
                foreach (ISpeciesObservation speciesObservation in speciesObservation1)
                {
                    Assert.IsTrue(speciesObservation.Conservation.RedlistCategory.IsNotEmpty() ||
                                  speciesObservation.Taxon.DyntaxaTaxonID == 222489);
                }
            }

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221940); // Mellanlummer.
            searchCriteria.IncludeRedlistedTaxa = false;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);
            if (speciesObservation2.IsNotEmpty())
            {
                foreach (ISpeciesObservation speciesObservation in speciesObservation2)
                {
                    Assert.AreEqual(221940, speciesObservation.Taxon.DyntaxaTaxonID);
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsAccuracyConsidered()
        {
            SpeciesObservationList speciesObservations1, speciesObservations2;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations2);
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
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count <= speciesObservations1.Count);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(Settings.Default.UpplandGUID);
            searchCriteria.IsAccuracyConsidered = true;
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count <= speciesObservations1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsNaturalOccurrence()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100017); // Klockgroda.

            searchCriteria.IsNaturalOccurrence = true;
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);
            if (speciesObservations1.IsNotEmpty())
            {
                foreach (ISpeciesObservation speciesObservation in speciesObservations1)
                {
                    Assert.IsTrue(speciesObservation.Occurrence.IsNaturalOccurrence.Value);
                }
            }

            searchCriteria.IsNaturalOccurrence = false;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            if (speciesObservations2.IsNotEmpty())
            {
                foreach (SpeciesObservation speciesObservation in speciesObservations2)
                {
                    Assert.IsFalse(speciesObservation.Occurrence.IsNaturalOccurrence.Value);
                }
            }

            searchCriteria.IsNaturalOccurrence = null;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.Count > speciesObservations1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsDisturbanceSensitivityConsidered()
        {
            SpeciesObservationList speciesObservation1, speciesObservation2;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation2);
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
            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Skane);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation1);

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.RegionGuids = null;

            // Test with accuracy.
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new Point(1360000, 6000000);
            searchCriteria.IsAccuracyConsidered = true;

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation1);
            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            searchCriteria.BoundingBox = null;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.StartsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.ToLower().Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));

            }

            searchCriteria.LocalityNameSearchString.SearchString = "backar";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.EndsWith(searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "%Full%";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString.Substring(1, 4)));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsFalse(speciesObservation.Location.Locality.Contains(searchCriteria.LocalityNameSearchString.SearchString));

            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaLocalityNameSearchStringOperatorsError()
        {
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaLocalityNameSearchStringNoOperatorError()
        {
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new StringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;
            Int64 noOfObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            searchCriteria = new SpeciesObservationSearchCriteria { TaxonIds = new List<Int32> { (Int32)(TaxonId.DrumGrasshopper) } };
            noOfObservations = GetSpeciesObservationManager().GetSpeciesObservationCount(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObservationDateTime()
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
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
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }
            Assert.IsTrue(speciesObservation2.Count < speciesObservation1.Count);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteria()
        {
            DarwinCoreList darwinCores;
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            SpeciesObservationFieldSortOrderList fieldSortOrderList = new SpeciesObservationFieldSortOrderList();

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32> {(Int32) TaxonId.DrumGrasshopper};
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            darwinCores = GetSpeciesObservationManager()
                .GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, fieldSortOrderList);
            Assert.IsNotNull(darwinCores);
            Assert.IsTrue(darwinCores.Count > 0);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministration1()
        {
            DarwinCoreList speciesObservations;
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ISpeciesObservationSearchCriteria searchCriteria;
            ITaxon lichens;
            SpeciesObservationFieldSortOrderList sortOrder = null;
            TaxonList redlListedTaxa, taxa;

            // Test problem with to many lichens observations
            // in new Artportalskopplingen.

            lichens = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Lichens);
            taxa = lichens.GetChildTaxonTree(GetUserContext(), true).GetChildTaxa();
            redlListedTaxa = GetRedlistedTaxa();
            taxa.Subset(redlListedTaxa);

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.BirdNestActivityLimit = CoreData.SpeciesObservationManager.GetBirdNestActivities(GetUserContext()).Get(18);
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2005, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(@"URN:LSID:Artportalen.se:Area:DataSet21Feature18"); // Örebro län.
            searchCriteria.TaxonIds = taxa.GetIds();
            speciesObservations = GetSpeciesObservationManager().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, sortOrder);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministration2()
        {
            DarwinCoreList speciesObservations;
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ISpeciesObservationSearchCriteria searchCriteria;
            ITaxon taxon;
            SpeciesObservationFieldSortOrderList sortOrder = null;

            // Test problem with missing observations in
            // Artportalskoppling compared to Analysis portal.

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 102609); // Stensimpa.
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.DataSourceGuids.Add(CoreData.SpeciesObservationManager.GetSpeciesObservationDataProviders(GetUserContext()).Get(7).Guid); // Sers.
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 12, 10, 10, 59, 59);
            searchCriteria.ObservationDateTime.End = new DateTime(2014, 12, 10, 10, 59, 59);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(@"URN:LSID:Artportalen.se:Area:DataSet21Feature1"); // Stockholms län.
            searchCriteria.RegionLogicalOperator = LogicalOperator.Or;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(taxon.Id);
            speciesObservations = CoreData.SpeciesObservationManager.GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, sortOrder);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaCountyAdministration3()
        {
            DarwinCoreList speciesObservations;
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ISpeciesObservationSearchCriteria searchCriteria;
            ITaxon taxon;
            SpeciesObservationFieldSortOrderList sortOrder = null;

            // Test problem with wrong handling of bird nest activity.

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 102609); // Stensimpa.
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.BirdNestActivityLimit = CoreData.SpeciesObservationManager.GetBirdNestActivities(GetUserContext()).Get(18);
            searchCriteria.DataSourceGuids = new List<String>();
            searchCriteria.DataSourceGuids.Add(CoreData.SpeciesObservationManager.GetSpeciesObservationDataProviders(GetUserContext()).Get(7).Guid); // Sers.
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 12, 10, 10, 59, 59);
            searchCriteria.ObservationDateTime.End = new DateTime(2014, 12, 10, 10, 59, 59);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(@"URN:LSID:Artportalen.se:Area:DataSet21Feature1"); // Stockholms län.
            searchCriteria.RegionLogicalOperator = LogicalOperator.Or;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(taxon.Id);
            speciesObservations = CoreData.SpeciesObservationManager.GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, sortOrder);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPage()
        {
            DarwinCoreList darwinCores;
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            ICoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ISpeciesObservationPageSpecification pageSpecification = new SpeciesObservationPageSpecification
            {
                Size = 3,
                SortOrder = new SpeciesObservationFieldSortOrderList(),
                Start = 1
            };

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32> { (Int32)TaxonId.DrumGrasshopper };
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            darwinCores = GetSpeciesObservationManager().GetDarwinCore(GetUserContext(), searchCriteria, coordinateSystem, pageSpecification);
            Assert.IsNotNull(darwinCores);
            Assert.IsTrue(darwinCores.Count > 0 && darwinCores.Count == pageSpecification.Size);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaMaxProtectionLevel()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1, speciesObservation2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.MaxProtectionLevel = 5;
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(speciesObservation.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel.Value);
            }

            searchCriteria.MaxProtectionLevel = 1;
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservation2.Count <= speciesObservation1.Count);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(speciesObservation.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel.Value);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaMinProtectionLevel()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations1, speciesObservations2;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(626); // Ljungögontröst
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.MinProtectionLevel = 1;
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(speciesObservation.Conservation.ProtectionLevel >= searchCriteria.MinProtectionLevel);
            }

            searchCriteria.MinProtectionLevel = 4;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(speciesObservations2.IsEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTime()
        {
            SpeciesObservationList speciesObservations1, speciesObservations2;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
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
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            foreach (ISpeciesObservation speciesObservation in speciesObservations2)
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
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(speciesObservation.Event.Start <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(speciesObservation.Event.End >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            foreach (ISpeciesObservation speciesObservation in speciesObservations2)
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
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }

            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(400, 0, 0, 0);
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            foreach (ISpeciesObservation speciesObservation in speciesObservations2)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Value.Days, 0, 0, 0) >= (speciesObservation.Event.Start - speciesObservation.Event.End));
            }
            Assert.IsTrue(speciesObservations2.Count < speciesObservations1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTimeInterval()
        {
            // Tests when the interval is within a year
            // (not over new year's eve)
            // Tests day of year and date (ie Month-Day) queries
            // Tests Excluding and Including

            Int32 excludeSize, includeSize;
            SpeciesObservationList speciesObservations;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            excludeSize = speciesObservations.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.Value.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.Value.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.Value.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Value.Days);
            }

            // Day of year - including.
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            includeSize = speciesObservations.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.Value.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.Value.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.Value.DayOfYear));
            }

            //DAY INCLUDING-EXCLUDING INTERVAL WITHIN A YEAR
            Assert.IsTrue(includeSize >= excludeSize);

            // DATE - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = false;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            excludeSize = speciesObservations.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.Value.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.Value.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.Value.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >= (speciesObservation.Event.End - speciesObservation.Event.End).Value.Days);
            }

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            includeSize = speciesObservations.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin <= speciesObservation.Event.Start.Value) ||
                              (dateTimeInterval.End >= speciesObservation.Event.Start.Value));

                Assert.IsTrue((dateTimeInterval.End >= speciesObservation.Event.End.Value) ||
                              (dateTimeInterval.Begin <= speciesObservation.Event.End.Value));
            }

            //DATE INCLUDING-EXCLUDING INTERVAL WITHIN A YEAR
            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTimeIntervalNewYearsEve()
        {
            //Tests when the interval is over new year's eve.
            //Tests day of year and date (ie Month-Day) queries
            //Tests Excluding and Including

            Int32 excludeSize, includeSize;
            SpeciesObservationList speciesObservations;
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            excludeSize = speciesObservations.Count;

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
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

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            excludeSize = speciesObservations.Count;

            // DAY OF YEAR - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            includeSize = speciesObservations.Count;
            //DAY INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);

            // DATE - EXCLUDING
            dateTimeInterval.IsDayOfYearSpecified = false;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear = new List<IDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            excludeSize = speciesObservations.Count;

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            includeSize = speciesObservations.Count;

            //DATE INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObserverSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "oskar";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.ToLower().Contains(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            searchCriteria.ObserverSearchString.SearchString = "Kindvall";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.EndsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.ObserverSearchString.SearchString = "";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.ObserverSearchString.SearchString = "Oskar Kindva%";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsTrue(speciesObservation.Occurrence.RecordedBy.StartsWith(searchCriteria.ObserverSearchString.SearchString.Substring(0, 10)));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.ObserverSearchString.SearchString = "";
            speciesObservations = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
            foreach (ISpeciesObservation speciesObservation in speciesObservations)
            {
                Assert.IsFalse(speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringNoOperatorError()
        {
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringOperatorsError()
        {
            SpeciesObservationList speciesObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new StringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPagedTaxonIds()
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
            for (int i = 1; i < 100; i = i + (int)pageSpecification.Size)
            {
                pageSpecification.Start = i;

                speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem, pageSpecification);
                if (speciesObservations1.IsNotEmpty())
                {
                    CheckSpeciesObservationExist(speciesObservations1);

                    Assert.AreEqual(speciesObservations1.Count, speciesObservations1.GetIds().Count);
                    foreach (ISpeciesObservation speciesObservation in speciesObservations1)
                    {
                        Assert.AreEqual(Settings.Default.DrumGrasshopperId, speciesObservation.Taxon.DyntaxaTaxonID);
                    }
                }
            }
        }

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

            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), pageSpecification, null);
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservations1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Add(polygon);
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
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
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
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
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(CoordinateSystemId.Rt90_25_gon_v));
            CheckSpeciesObservationExist(speciesObservations1);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaRegionGuids()
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);

            // Test adding the same region twice.
            searchCriteria.CountyProvinceRegionSearchType = CountyProvinceRegionSearchType.ByCoordinate; //This is the default
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.AreEqual(speciesObservation1.Count, speciesObservations2.Count);

            // Test adding the same region using name search.
            searchCriteria.CountyProvinceRegionSearchType = CountyProvinceRegionSearchType.ByName;
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.AreEqual(speciesObservation1.Count, speciesObservations2.Count);

            // Test adding another region.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservation1.Count < speciesObservations2.Count);

            // Test a normal county region
            searchCriteria.RegionGuids.Clear();
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            speciesObservations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.IsNotEmpty());

            // Test a custom county region (i.e, Kalmar fastland / Öland)
            searchCriteria.RegionGuids.Clear();
            searchCriteria.RegionGuids.Add(CountyGuid.KalmarFastland);
            speciesObservations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.IsNotEmpty());

            // test a custom province region (Lappland)
            searchCriteria.RegionGuids.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.LuleLappmark);
            // Change taxon id to moose
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)TaxonId.Moose);
            speciesObservations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.IsNotEmpty());

            // test having both ordinary and custom counties and provinces
            searchCriteria.RegionGuids.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.LuleLappmark);
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.KalmarFastland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            speciesObservations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.IsNotEmpty());

            // test having both ordinary counties and provinces and some other region but no "Custom" county or province
            // This should use the standard way to filter region data.
            searchCriteria.RegionGuids.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet13Feature4");
            speciesObservations2 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations2.IsNotEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsInvalidRegionCombinationError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            // test having both ordinary and custom counties and provinces AND other regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.LuleLappmark);
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.KalmarFastland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet13Feature4");
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownCountyError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet21Feature1000");
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownProvinceError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservation1;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet16Feature1000");
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation1);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationsBySearchCriteriaRegionGuidsUnknownRegionGuidError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("Unknown region guid");
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservation1);
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }

            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2010, 10, 1);
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservation2);
            foreach (ISpeciesObservation speciesObservation in speciesObservation2)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }
            Assert.IsTrue(speciesObservation2.Count > speciesObservation1.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaRegistrationDateTimeInterval()
        {
            ICoordinateSystem coordinateSystem;
            SpeciesObservationList speciesObservation1;
            IDateTimeInterval dateTimeInterval;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));

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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservation1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservation1);
            int excludeSize = speciesObservation1.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.Value.DayOfYear));
            }

            // Test reported observations PartOfYear with including interval over a newyearsday
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Including;

            speciesObservation1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, coordinateSystem);
            CheckSpeciesObservationExist(speciesObservation1);
            int includeSize = speciesObservation1.Count;
            foreach (ISpeciesObservation speciesObservation in speciesObservation1)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.Value.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.Value.DayOfYear));
            }

            Assert.IsTrue(includeSize >= excludeSize);
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
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationManager(true).GetSpeciesActivities(GetUserContext())[1].Id);
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations1);

            searchCriteria.SpeciesActivityIds.Add(GetSpeciesObservationManager().GetSpeciesActivities(GetUserContext())[4].Id);
            speciesObservation2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservation2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservation2.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        [Ignore]
        public void GetSpeciesObservationsBySearchCriteriaSpeciesActivityIdsUnknownIdError()
        {
            ISpeciesObservationSearchCriteria searchCriteria;
            SpeciesObservationList speciesObservations;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(1000000);
            SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
            speciesObservations = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem());
            CheckSpeciesObservationExist(speciesObservations);
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

            speciesObservations1 = GetSpeciesObservationManager(true).GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations1);
            foreach (ISpeciesObservation speciesObservation in speciesObservations1)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            speciesObservations2 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations2);
            Assert.IsTrue(speciesObservations1.Count < speciesObservations2.Count);

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222226); // Knylhavre
            speciesObservations1 = GetSpeciesObservationManager().GetSpeciesObservations(GetUserContext(), searchCriteria, GetCoordinateSystem(), null, sortOrder);
            CheckSpeciesObservationExist(speciesObservations1);
        }

        protected override String GetTestApplicationName()
        {
            return ApplicationIdentifier.PrintObs.ToString();
        }
    }
}
