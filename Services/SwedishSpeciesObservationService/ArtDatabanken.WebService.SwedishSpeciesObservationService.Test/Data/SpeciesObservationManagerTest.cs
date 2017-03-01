using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesObservationManager = ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesObservationManager;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    [TestClass]
    public class SpeciesObservationManagerTest : TestBase
    {
        private WebCoordinateSystem _coordinateSystem;

        private void CheckDarwinCoreInformation(WebDarwinCoreInformation information)
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

        private void CompareSpeciesObservations(WebSpeciesObservationInformation informationSqlServer,
                                                WebSpeciesObservationInformation informationElasticsearch)
        {
            if (informationSqlServer.SpeciesObservations.IsNotEmpty() &&
                informationElasticsearch.SpeciesObservations.IsNotEmpty())
            {
                CompareSpeciesObservations(informationSqlServer.SpeciesObservations,
                                           informationElasticsearch.SpeciesObservations);
            }

            if (informationSqlServer.SpeciesObservationIds.IsNotEmpty() &&
                informationElasticsearch.SpeciesObservationIds.IsNotEmpty())
            {
                CompareSpeciesObservations(informationSqlServer.SpeciesObservationIds,
                                           informationElasticsearch.SpeciesObservationIds);
            }
        }

        private void CompareSpeciesObservations(List<WebSpeciesObservation> speciesObservationsSqlServer,
                                                List<WebSpeciesObservation> speciesObservationsElasticsearch)
        {
            Dictionary<Int64, WebSpeciesObservation> speciesObservationDictionarySqlServer,
                                                     speciesObservationDictionaryElasticsearch;
            Int64 speciesObservationId;
            List<Int64> missingSpeciesObservationIds;

            speciesObservationDictionarySqlServer = new Dictionary<Int64, WebSpeciesObservation>();
            if (speciesObservationsSqlServer.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in speciesObservationsSqlServer)
                {
                    speciesObservationId = speciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore,
                        SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                    speciesObservationDictionarySqlServer[speciesObservationId] = speciesObservation;
                }
            }

            speciesObservationDictionaryElasticsearch = new Dictionary<Int64, WebSpeciesObservation>();
            if (speciesObservationsElasticsearch.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in speciesObservationsElasticsearch)
                {
                    speciesObservationId = speciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore,
                        SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                    speciesObservationDictionaryElasticsearch[speciesObservationId] = speciesObservation;
                }
            }

            missingSpeciesObservationIds = new List<Int64>();
            foreach (Int64 key in speciesObservationDictionarySqlServer.Keys)
            {
                if (!(speciesObservationDictionaryElasticsearch.ContainsKey(key)))
                {
                    missingSpeciesObservationIds.Add(key);
                    Debug.WriteLine("Elasticsearch does not contain species observation with id = " + key);
                }
            }

            ShowSpeciesObservationsElasticsearch(missingSpeciesObservationIds);
            UpdateSpeciesObservationsElasticsearch(missingSpeciesObservationIds);

            missingSpeciesObservationIds = new List<Int64>();
            foreach (Int64 key in speciesObservationDictionaryElasticsearch.Keys)
            {
                if (!(speciesObservationDictionarySqlServer.ContainsKey(key)))
                {
                    missingSpeciesObservationIds.Add(key);
                    Debug.WriteLine("SQL Server does not contain species observation with id = " + key);
                }
            }
        }

        private void CompareSpeciesObservations(List<Int64> speciesObservationIdsSqlServer,
                                                List<Int64> speciesObservationIdsElasticsearch)
        {
            List<Int64> missingSpeciesObservationIds;

            missingSpeciesObservationIds = new List<Int64>();
            foreach (Int64 speciesObservationId in speciesObservationIdsSqlServer)
            {
                if (!(speciesObservationIdsElasticsearch.Contains(speciesObservationId)))
                {
                    missingSpeciesObservationIds.Add(speciesObservationId);
                    Debug.WriteLine("Elasticsearch does not contain species observation with id = " + speciesObservationId);
                }
            }

            ShowSpeciesObservationsElasticsearch(missingSpeciesObservationIds);
            UpdateSpeciesObservationsElasticsearch(missingSpeciesObservationIds);

            missingSpeciesObservationIds = new List<Int64>();
            foreach (Int64 speciesObservationId in speciesObservationIdsElasticsearch)
            {
                if (!(speciesObservationIdsSqlServer.Contains(speciesObservationId)))
                {
                    missingSpeciesObservationIds.Add(speciesObservationId);
                    Debug.WriteLine("SQL Server does not contain species observation with id = " + speciesObservationId);
                }
            }
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

        private static void SetOwnerFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;

            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            SetOwnerFieldSearchCriteria(fieldSearchCriterias);
            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetUncertainDeterminationSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria, bool value)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;

            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            SetUncertainDeterminationFieldSearchCriterias(fieldSearchCriterias, value);
            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetUncertainDeterminationFieldSearchCriterias(
            List<WebSpeciesObservationFieldSearchCriteria> searchCriterias, bool value)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria =
               new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Identification);
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.UncertainDetermination);
            fieldSearchCriteria.Type = WebDataType.Boolean;
            fieldSearchCriteria.Value = value.ToString();

            searchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetOwnerFieldSearchCriteria(
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria =
                new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.DarwinCore);
            //fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Owner);
            fieldSearchCriteria.Type = WebDataType.String;
            //fieldSearchCriteria.Value = "Länsstyrelsen Östergötland";
            fieldSearchCriteria.Value = "Per Flodin";
            // fieldSearchCriteria.Value = "Flodin";

            fieldSearchCriterias.Add(fieldSearchCriteria);
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

        private static void SetHabitatFieldSearchCriteria(
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria =
                new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Event);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Habitat);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetSubstrateFieldSearchCriteria(
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria =
                new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Occurrence);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Substrate);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetCoordinateUncertaintyInMetersFieldSearchCriteria(
            WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria =
                new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Location);
            fieldSearchCriteria.Property =
                new WebSpeciesObservationProperty(SpeciesObservationPropertyId.CoordinateUncertaintyInMeters);
            fieldSearchCriteria.Type = WebDataType.Float64;
            fieldSearchCriteria.Value = "10000";
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriterias.Add(fieldSearchCriteria);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;

            if (searchCriteria.DataFields.IsNull())
            {
                searchCriteria.DataFields = new List<WebDataField>();
            }

            searchCriteria.DataFields.SetString("FieldLogicalOperator", LogicalOperator.Or.ToString());
        }

        [TestMethod]
        public void GetCountyRegions()
        {
            List<WebRegion> regions;
            regions = SpeciesObservationManager.GetCountyRegions(Context);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetDarwinCoreByIds()
        {
            List<Int64> speciesObservationIds = new List<Int64>();
            WebDarwinCoreInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                speciesObservationIds.Add(speciesObservation.Id);
            }

            information2 = SpeciesObservationManager.GetDarwinCoreByIds(Context, speciesObservationIds,
                GetCoordinateSystem());
            CheckDarwinCoreInformation(information2);
            Assert.AreEqual(information1.SpeciesObservationCount,
                information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreByIdsElasticsearch()
        {
            WebDarwinCoreInformation darwinCoreInformation;
            List<Int64> speciesObservationIds;
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            // Get species observations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Size = 100;
            pageSpecification.Start = 1000;
            speciesObservations =
                SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPageElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, pageSpecification.Size);

            // Get species observation ids.
            speciesObservationIds = new List<Int64>();
            foreach (WebSpeciesObservation speciesObservation in speciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsNotNull(field);
                speciesObservationIds.Add(field.Value.WebParseInt64());
            }

            Assert.AreEqual(speciesObservationIds.Count, pageSpecification.Size);

            // Get species observations by ids.
            darwinCoreInformation = SpeciesObservationManager.GetDarwinCoreByIdsElasticsearch(Context,
                speciesObservationIds, GetCoordinateSystem());
            Assert.IsNotNull(darwinCoreInformation);
            Assert.IsTrue(darwinCoreInformation.SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(darwinCoreInformation.SpeciesObservations.Count,
                speciesObservationIds.Count);
            foreach (WebDarwinCore speciesObservation in darwinCoreInformation.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservationIds.Contains(speciesObservation.Id));
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 60;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue(Double.Parse(darwinCore.Location.CoordinateUncertaintyInMeters) <= searchCriteria.Accuracy);
            }
        }

        [TestMethod]
        // [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaAccuracyArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = -1;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaAccuracyElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 60;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaElasticsearch(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.CoordinateUncertaintyInMeters.WebParseInt32() <=
                              searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaElasticsearch(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Location.CoordinateUncertaintyInMeters.WebParseInt32() <=
                              searchCriteria.Accuracy);
            }

            Assert.IsTrue(information2.SpeciesObservationCount <
                          information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimit()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.BirdNestActivityLimit =
                SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetBirdNestActivities(Context)[3].Id;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IsBirdNestActivityLimitSpecified = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaBirdNestActivityLimitArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.BirdNestActivityLimit = -1;
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1670000, 6670116);
            searchCriteria.BoundingBox.Min = new WebPoint(1300000, 6000000);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new WebPoint(1501658, 6508484);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaBoundingBoxAndObservationDateTime2()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            List<WebDarwinCore> speciesObservations;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationFieldSortOrder sortOrder;
            WebPolygon polygon;
            WebLinearRing linearRing;

            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.Size = 25;
            pageSpecification.Start = 1;
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            sortOrder = new WebSpeciesObservationFieldSortOrder();
            sortOrder.Class = new WebSpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Event;
            sortOrder.Property = new WebSpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            sortOrder.SortOrder = SortOrder.Descending;
            pageSpecification.SortOrder.Add(sortOrder);

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            polygon = new WebPolygon {LinearRings = new List<WebLinearRing>()};
            linearRing = new WebLinearRing
            {
                Points =
                    new List<WebPoint>
                    {
                        new WebPoint(1964062.5791284, 8358432.8520649),
                        new WebPoint(1964062.5791284, 8358693.2156924),
                        new WebPoint(1963768.774301, 8358693.2156924),
                        new WebPoint(1963768.774301, 8358432.8520649),
                        new WebPoint(1964062.5791284, 8358432.8520649)
                    }
            };
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon> {polygon};
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(new WebDateTimeInterval());
            searchCriteria.ObservationDateTime.PartOfYear[0].Begin = new DateTime(2013, 10, 01);
            searchCriteria.ObservationDateTime.PartOfYear[0].End = new DateTime(2013, 10, 31);
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;

            speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                coordinateSystem, pageSpecification);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Min = new WebPoint(1562902, 6618355);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 8, 1);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 10, 1);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= speciesObservation.Modified);
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= speciesObservation.Modified);
            }
            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            // Find changed observations with interval.
            // Test changed observations PartOfYear with a excluding interval over a newyearsday.
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 12, 31);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2000, 12, 20);
            dateTimeInterval.End = new DateTime(2001, 1, 10);
            dateTimeInterval.IsDayOfYearSpecified = true;

            searchCriteria.ChangeDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Modified.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Modified.DayOfYear));
            }

            // Test changed observations PartOfYear with including interval over a newyearsday
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
        //  [TestCategory("NightlyTest")]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuids()
        {
            List<WebSpeciesObservationDataProvider> speciesObservationDataProviders;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information1, information2;

            speciesObservationDataProviders = SpeciesObservationManager.GetSpeciesObservationDataProviders(Context);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataProviderGuids.Add(speciesObservationDataProviders[0].Guid);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.AreEqual(speciesObservationDataProviders[0].Name, darwinCore.DatasetName);
            }

            searchCriteria.DataProviderGuids.Add(speciesObservationDataProviders[1].Guid);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount >= information1.SpeciesObservationCount);
            foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
            {
                Assert.IsTrue((speciesObservationDataProviders[0].Name == darwinCore.DatasetName) ||
                              (speciesObservationDataProviders[1].Name == darwinCore.DatasetName));
            }

            searchCriteria.DataProviderGuids = new List<String>();
            foreach (WebSpeciesObservationDataProvider dataProvider in speciesObservationDataProviders)
            {
                searchCriteria.DataProviderGuids.Add(dataProvider.Guid);
            }
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaDataProviderGuidsUnknownGuidError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataProviderGuids.Add("None data provider GUID");
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        /// <summary>
        /// This test should be run on production data.
        /// The test fails if SQl Server is used but returnes correct result if Elasticsearch is used.
        /// There is a bug in the SQL Server implementation. A coordinate system (Google mercator)
        /// that is not linear is used when species observation outside specified polygon are
        /// considered.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaForestAgency()
        {
            Boolean isSpeciesObservationFound;
            WebCoordinateSystem coordinateSystem;
            WebDarwinCoreInformation information;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            // Test problem reported from swedish forest agency.
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1000;
            searchCriteria.BirdNestActivityLimit = 18;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracyConsidered = true;
            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(516652.9341705670000000, 6441514.2864947500000000));
            linearRing.Points.Add(new WebPoint(516800.5794939930000000, 6441511.1451048900000000));
            linearRing.Points.Add(new WebPoint(516800.5794939930000000, 6441341.5100524500000000));
            linearRing.Points.Add(new WebPoint(516662.3583401470000000, 6441376.0653409100000000));
            linearRing.Points.Add(new WebPoint(516652.9341705670000000, 6441514.2864947500000000));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.TaxonIds = SpeciesObservationManager.GetSwedishForestAgencyTaxonIds(Context).GetInt32List();
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem, null);
            CheckDarwinCoreInformation(information);
            isSpeciesObservationFound = false;
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                if (speciesObservation.Occurrence.OccurrenceID == "urn:lsid:observationsdatabasen.se:Sighting:159584")
                {
                    isSpeciesObservationFound = true;
                    break;
                }
            }

            Assert.IsTrue(isSpeciesObservationFound);
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludeNeverFoundObservations = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludeNotRediscoveredObservations = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.IncludePositiveObservations = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.IncludePositiveObservations = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
                information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                    GetCoordinateSystem(), null);
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
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaIncludeRedListCategoriesArgumentError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
            searchCriteria.IncludeRedListCategories.Add(RedListCategory.LC);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIncludeRedlistedTaxa()
        {
            Boolean hasProtectedSpeciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1993, 07, 12);
            searchCriteria.ObservationDateTime.End = new DateTime(1993, 07, 14);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222489); // Kvarngröe.
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            if (information1.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
                {
                    Assert.IsTrue(darwinCore.Conservation.RedlistCategory.IsNotEmpty() ||
                                  darwinCore.Taxon.DyntaxaTaxonID == 222489);
                }
            }

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(222489); // Kvarngröe.
            searchCriteria.IncludeRedlistedTaxa = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
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
            Int64 observationsInRegion;
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPoint point;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test with bounding box.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1612506, 6653581);
            searchCriteria.BoundingBox.Min = new WebPoint(1501658, 6508484);
            searchCriteria.IsAccuracyConsidered = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            observationsInRegion = 0;
            foreach (WebDarwinCore observation in information1.SpeciesObservations)
            {
                point = new WebPoint(observation.Location.CoordinateX,
                    observation.Location.CoordinateY);
                if (searchCriteria.BoundingBox.IsPointInside(point))
                {
                    observationsInRegion++;
                }
            }
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            Assert.AreEqual(observationsInRegion, information2.SpeciesObservationCount);
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            observationsInRegion = 0;
            foreach (WebDarwinCore observation in information1.SpeciesObservations)
            {
                point = new WebPoint(observation.Location.CoordinateX,
                    observation.Location.CoordinateY);
                if (polygon.IsPointInsideGeometry(point))
                {
                    observationsInRegion++;
                }
            }
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            Assert.AreEqual(observationsInRegion, information2.SpeciesObservationCount);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.IsAccuracyConsidered = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsAccuracyConsidered = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaIsAccuracyConsideredNoRegionError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IsAccuracyConsidered = true;
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsDisturbanceSensitivityConsidered()
        {
            Int64 observationsInRegion;
            WebDarwinCoreInformation information1, information2;
            WebLinearRing linearRing;
            WebPoint point;
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            observationsInRegion = 0;
            foreach (WebDarwinCore observation in information1.SpeciesObservations)
            {
                point = new WebPoint(observation.Location.CoordinateX,
                    observation.Location.CoordinateY);
                if (searchCriteria.BoundingBox.IsPointInside(point))
                {
                    observationsInRegion++;
                }
            }

            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            Assert.AreEqual(observationsInRegion, information2.SpeciesObservationCount);
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            observationsInRegion = 0;
            foreach (WebDarwinCore observation in information1.SpeciesObservations)
            {
                point = new WebPoint(observation.Location.CoordinateX,
                    observation.Location.CoordinateY);
                if (polygon.IsPointInsideGeometry(point))
                {
                    observationsInRegion++;
                }
            }

            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            Assert.AreEqual(observationsInRegion, information2.SpeciesObservationCount);
            searchCriteria.Polygons = null;

            // Test with region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Skane);
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);

            // Test with accuracy.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1700000, 6700000);
            searchCriteria.BoundingBox.Min = new WebPoint(1360000, 6000000);
            searchCriteria.IsAccuracyConsidered = true;

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            searchCriteria.Polygons = null;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            searchCriteria.BoundingBox = null;
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaIsDisturbanceSensitivityConsideredNoRegionError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaIsNaturalOccurrence()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(233818); // Mandarinand.

            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
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
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebDarwinCore darwinCore in information2.SpeciesObservations)
                {
                    Assert.IsFalse(darwinCore.Occurrence.IsNaturalOccurrence);
                }
            }

            searchCriteria.IsIsNaturalOccurrenceSpecified = false;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.MaxProtectionLevel = 5;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel <= searchCriteria.MaxProtectionLevel);
            }

            searchCriteria.MaxProtectionLevel = 1;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore darwinCore in information1.SpeciesObservations)
            {
                Assert.IsTrue(darwinCore.Conservation.ProtectionLevel >= searchCriteria.MinProtectionLevel);
            }
            searchCriteria.IsMaxProtectionLevelSpecified = false;

            searchCriteria.MinProtectionLevel = 5;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= speciesObservation.Event.End);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.Start);
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= speciesObservation.Event.End);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1930, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(214537); // 	gullinjerad gullrissäckmal
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(speciesObservation.Event.Start <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(speciesObservation.Event.End >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 4000;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2016, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >=
                              (speciesObservation.Event.Start - speciesObservation.Event.End));
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 4;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1900, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2016, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            foreach (WebDarwinCore speciesObservation in information2.SpeciesObservations)
            {
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >=
                              (speciesObservation.Event.Start - speciesObservation.Event.End));
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mammals));

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.Begin = new DateTime(2012, 1, 1);
            dateTimeInterval.End = new DateTime(2012, 3, 1);

            // Day of year - excluding.
            dateTimeInterval.IsDayOfYearSpecified = true;
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >=
                              (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // Day of year - including.
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.Start.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.Event.Start.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End.DayOfYear >= speciesObservation.Event.End.DayOfYear) ||
                              (dateTimeInterval.Begin.DayOfYear <= speciesObservation.Event.End.DayOfYear));

                Assert.IsTrue((dateTimeInterval.End - dateTimeInterval.Begin).Days >=
                              (speciesObservation.Event.End - speciesObservation.Event.End).Days);
            }

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            // find observations with interval

            // TEST WITHOUT INTERVAL FOR COMPARING REASON
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 12, 29);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 1, 5);

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            // DAY OF YEAR - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            excludeSize = information1.SpeciesObservations.Count;

            // DATE - INCLUDING
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            includeSize = information1.SpeciesObservations.Count;

            //DATE INCLUDING-EXCLUDING INTERVAL OVER NYE
            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Location.Locality.StartsWith(
                        searchCriteria.LocalityNameSearchString.SearchString, true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Location.Locality.ToLower()
                        .Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));

            }

            searchCriteria.LocalityNameSearchString.SearchString = "backar";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Location.Locality.EndsWith(searchCriteria.LocalityNameSearchString.SearchString,
                        true, ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "%Full%";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Location.Locality.Contains(
                        searchCriteria.LocalityNameSearchString.SearchString.Substring(1, 4)));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsFalse(speciesObservation.Location.Locality.Equals(searchCriteria.LocalityNameSearchString.SearchString));
            }

            // Test with character '.
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö ' backar";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            Assert.IsNotNull(information);
            Assert.AreEqual(0, information.SpeciesObservationCount);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringNoOperatorError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaLocalityNameSearchStringOperatorsError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Full";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaObserverSearchString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "oskar";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);

            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Occurrence.RecordedBy.StartsWith(
                        searchCriteria.ObserverSearchString.SearchString, true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Occurrence.RecordedBy.ToLower()
                        .Contains(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            searchCriteria.ObserverSearchString.SearchString = "Kindvall";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Occurrence.RecordedBy.EndsWith(searchCriteria.ObserverSearchString.SearchString,
                        true, ci));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.ObserverSearchString.SearchString = "";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.ObserverSearchString.SearchString = "Oskar Kindva%";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsTrue(
                    speciesObservation.Occurrence.RecordedBy.StartsWith(
                        searchCriteria.ObserverSearchString.SearchString.Substring(0, 10)));
            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.ObserverSearchString.SearchString = "";
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
            foreach (WebDarwinCore speciesObservation in information.SpeciesObservations)
            {
                Assert.IsFalse(
                    speciesObservation.Occurrence.RecordedBy.Equals(searchCriteria.ObserverSearchString.SearchString));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringNoOperatorError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaObserverSearchStringOperatorsError()
        {
            WebDarwinCoreInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Full";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygon);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test two regions using (the default) ByCoordinate.
            searchCriteria.DataFields = new List<WebDataField>();
            WebDataField dataFieldByCoordinate = new WebDataField()
            {
                Name = typeof(CountyProvinceRegionSearchType).ToString(),
                Type = WebDataType.String,
                Value = CountyProvinceRegionSearchType.ByCoordinate.ToString()
            };

            searchCriteria.DataFields.Add(dataFieldByCoordinate);
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            Assert.IsTrue(information1.SpeciesObservations.IsNotEmpty());

            // Same search By Name
            searchCriteria.DataFields = new List<WebDataField>();
            WebDataField dataFieldByName = new WebDataField()
            {
                Name = typeof(CountyProvinceRegionSearchType).ToString(),
                Value = CountyProvinceRegionSearchType.ByName.ToString(),
                Type = WebDataType.String
            };
            searchCriteria.DataFields.Add(dataFieldByName);
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // Test adding the same regions twice.
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields = new DataList<WebDataField>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.AreEqual(information1.SpeciesObservationCount, information2.SpeciesObservationCount);

            // Test adding another region.
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);
            searchCriteria.DataFields.Clear();
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // Test a normal county region
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields.Clear();
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
               GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // Test a custom county region (i.e, Kalmar fastland / Öland)
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields.Clear();
            searchCriteria.RegionGuids.Add(CountyGuid.KalmarFastland);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
               GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // test a custom province region (Lappland)
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.LuleLappmark);
            // Change taxon id to moose
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) TaxonId.Moose);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
               GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // test having both ordinary and custom counties and provinces
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.LuleLappmark);
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.KalmarFastland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
               GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());

            // test having both ordinary counties and provinces and some other region but no "Custom" county or province
            // This should use the standard way to filter region data.
            searchCriteria.RegionGuids.Clear();
            searchCriteria.DataFields.Clear();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(CountyGuid.Uppsala);
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet13Feature4");
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
               GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information2.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsInvalidRegionCombinationError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
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
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, 
                                                                                  searchCriteria,
                                                                                  GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownCountyError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet21Feature1000");
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, 
                                                                                    searchCriteria,
                                                                                    GetCoordinateSystem(), null);
                CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownProvinceError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet16Feature1000");
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, 
                                                                                  searchCriteria,
                                                                                  GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaRegionGuidsUnknownRegionGuidError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add("Unknown region guid");
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2015, 8, 1);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= speciesObservation.ReportedDate);
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= speciesObservation.ReportedDate);
            }

            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2016, 10, 1);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

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
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int excludeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.DayOfYear));
            }

            // Test reported observations PartOfYear with including interval over a newyearsday
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Including;

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null);
            CheckDarwinCoreInformation(information1);
            int includeSize = information1.SpeciesObservations.Count;
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= speciesObservation.ReportedDate.DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= speciesObservation.ReportedDate.DayOfYear));
                // Debug.WriteLine(speciesObservation.ReportedDate + " - "+ speciesObservation.ReportedDate.DayOfYear + " - " + speciesObservation.Id);
            }

            Assert.IsTrue(includeSize >= excludeSize);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaSortOrder()
        {
            List<WebSpeciesObservationFieldSortOrder> sortOrder;
            WebDarwinCoreInformation information;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            sortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Descending;
            sortOrder.Add(startSortOrder);

            // Test without polygon.
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), sortOrder);
            CheckDarwinCoreInformation(information);

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
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), sortOrder);
            CheckDarwinCoreInformation(information);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(
                SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetSpeciesActivities(Context)[1].Id);
            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);

            searchCriteria.SpeciesActivityIds.Add(
                SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetSpeciesActivities(Context)[4].Id);
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof (ArgumentException))]
        public void GetDarwinCoreBySearchCriteriaSpeciesActivityIdsUnknownIdError()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebDarwinCoreInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.SpeciesActivityIds = new List<Int32>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            searchCriteria.SpeciesActivityIds.Add(1000000);
            information = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
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
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 6, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            information1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information1);
            foreach (WebDarwinCore speciesObservation in information1.SpeciesObservations)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            information2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageSizeOutOfRange()
        {
            WebSpeciesObservationPageSpecification pageSpecification = new WebSpeciesObservationPageSpecification();

            pageSpecification.Size = ArtDatabanken.Settings.Default.SpeciesObservationPageMaxSize;
            pageSpecification.Start = 1;
            pageSpecification.CheckData();
            Assert.AreEqual(pageSpecification.Size, ArtDatabanken.Settings.Default.SpeciesObservationPageMaxSize);

            try
            {
                pageSpecification.Size = ArtDatabanken.Settings.Default.SpeciesObservationPageMaxSize + 1;
                pageSpecification.CheckData();
                Assert.Fail("Should have been an exception here!");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException);
            }

            try
            {
                pageSpecification.Size = -1;
                pageSpecification.CheckData();
                Assert.Fail("Should have been an exception here!");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageDataProviderGuids()
        {
            List<WebSpeciesObservationDataProvider> speciesObservationDataProviders;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            List<WebDarwinCore> speciesObservation;
            WebSpeciesObservationPageSpecification pageSpecification;

            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.Size = 1000;
            pageSpecification.Start = 1;
            speciesObservationDataProviders = SpeciesObservationManager.GetSpeciesObservationDataProviders(Context);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();

            searchCriteria.DataProviderGuids.Add(speciesObservationDataProviders[9].Guid);
            speciesObservation = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                coordinateSystem, pageSpecification);
            Assert.IsTrue(speciesObservation.IsNotEmpty());
            foreach (WebDarwinCore darwinCore in speciesObservation)
            {
                Assert.AreEqual(speciesObservationDataProviders[9].Name, darwinCore.DatasetName);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageOrderByStartEnd()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            WebSpeciesObservationPageSpecification pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            WebSpeciesObservationFieldSortOrder startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Descending;
            pageSpecification.SortOrder.Add(startSortOrder);

            WebSpeciesObservationFieldSortOrder endSortOrder = new WebSpeciesObservationFieldSortOrder();
            endSortOrder.Class = new WebSpeciesObservationClass();
            endSortOrder.Class.Id = SpeciesObservationClassId.Event;
            endSortOrder.Property = new WebSpeciesObservationProperty();
            endSortOrder.Property.Identifier = "End";
            endSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(endSortOrder);

            pageSpecification.Size = 20;

            for (int i = 1; i < 100; i = i + (int) pageSpecification.Size)
            {
                pageSpecification.Start = i;
                pageSpecification.CheckData();
                speciesObservations1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification);
                //CheckDarwinCoreInformation(speciesObservations1);

                //to break the loop if no records are returned
                if (speciesObservations1.Count == 0)
                {
                    break;
                }

                foreach (WebDarwinCore speciesObservation in speciesObservations1)
                {
                    Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
                }

                Assert.IsTrue(speciesObservations1.Count <= pageSpecification.Size);
            }
            WebSpeciesObservationPageSpecification pageSpecification2 = new WebSpeciesObservationPageSpecification();
            pageSpecification2.Start = 10;
            pageSpecification2.Size = 20;
            speciesObservations1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification2);
            foreach (WebDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            Assert.IsTrue(speciesObservations1.Count <= pageSpecification2.Size);
        }

        [TestMethod]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaPageAccessRights()
        {
            // Add extra parameter to method GetDarwinCoreBySearchCriteriaPage.
            // Boolean isTestAccessRights = false
            // Replace line in metod GetDarwinCoreBySearchCriteriaPage.
            // if (context.CurrentRoles.IsSimpleSpeciesObservationAccessRights())
            // if (isTestAccessRights)
            // Add parameter in call to method
            // GetDarwinCoreBySearchCriteriaPage in this method.

            Int32 index;
            Int64 speciesObservationCount;
            List<WebDarwinCore> speciesObservations1, speciesObservations2;
            WebDarwinCoreInformation darwinCoreInformation;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            darwinCoreInformation = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null);
            CheckDarwinCoreInformation(darwinCoreInformation);
            speciesObservationCount = darwinCoreInformation.SpeciesObservationCount;
            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.Size = 100;
            pageSpecification.Start = 1;

            while (pageSpecification.Start < speciesObservationCount)
            {
                speciesObservations1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification);
                speciesObservations2 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification); //, true);
                Assert.IsTrue(speciesObservations1.IsEmpty() == speciesObservations2.IsEmpty());
                if (speciesObservations1.IsNotEmpty() &&
                    speciesObservations2.IsNotEmpty())
                {
                    Assert.AreEqual(speciesObservations1.Count, speciesObservations2.Count);
                    for (index = 0; index < speciesObservations1.Count; index++)
                    {
                        Assert.AreEqual(speciesObservations1[index].Id, speciesObservations2[index].Id);
                    }
                }
                pageSpecification.Start += pageSpecification.Size;
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageAll()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            //  Int32 speciesObservations = WebServiceProxy.AnalysisService.GetSpeciesObservationCountBySearchCriteria(, searchCriteria, GetCoordinateSystem());


            WebSpeciesObservationPageSpecification pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            WebSpeciesObservationFieldSortOrder startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);

            WebSpeciesObservationFieldSortOrder endSortOrder = new WebSpeciesObservationFieldSortOrder();
            endSortOrder.Class = new WebSpeciesObservationClass();
            endSortOrder.Class.Id = SpeciesObservationClassId.Event;
            endSortOrder.Property = new WebSpeciesObservationProperty();
            endSortOrder.Property.Identifier = "End";
            endSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(endSortOrder);

            pageSpecification.Size = 5;

            for (int i = 1; i < 100; i = i + (int) pageSpecification.Size)
            {
                pageSpecification.Start = i;
                pageSpecification.CheckData();

                speciesObservations1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification);
                //CheckDarwinCoreInformation(speciesObservations1);

                //to break the loop if no records are returned
                if (speciesObservations1.Count == 0)
                {
                    break;
                }

                foreach (WebDarwinCore speciesObservation in speciesObservations1)
                {
                    Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
                }

                Assert.IsTrue(speciesObservations1.Count <= pageSpecification.Size);
            }
            WebSpeciesObservationPageSpecification pageSpecification2 = new WebSpeciesObservationPageSpecification();
            pageSpecification2.Start = 10;
            pageSpecification2.Size = 20;
            speciesObservations1 = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification2);
            foreach (WebDarwinCore speciesObservation in speciesObservations1)
            {
                Assert.AreEqual(searchCriteria.TaxonIds[0], speciesObservation.Taxon.DyntaxaTaxonID);
            }

            Assert.IsTrue(speciesObservations1.Count <= pageSpecification2.Size);
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageTaxonIdsElasticsearch()
        {
            Int64 index, speciesObservationCount;
            List<WebDarwinCore> speciesObservations;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            speciesObservationCount =
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem());

            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Size = 1000;

            for (index = 0; index < speciesObservationCount; index += pageSpecification.Size)
            {
                pageSpecification.Start = index + 1;
                speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPageElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification);
                Assert.IsTrue(speciesObservations.IsNotEmpty());
                Assert.IsTrue(speciesObservations.Count <= pageSpecification.Size);
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaPageTaxonRegion()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));

            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);

            WebSpeciesObservationPageSpecification pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            WebSpeciesObservationFieldSortOrder startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);

            //WebSpeciesObservationFieldSortOrder endSortOrder = new WebSpeciesObservationFieldSortOrder();
            //endSortOrder.Class = new WebSpeciesObservationClass();
            //endSortOrder.Class.Id = SpeciesObservationClassId.Event;
            //endSortOrder.Property = new WebSpeciesObservationProperty();
            //endSortOrder.Property.Identifier = "End";
            //endSortOrder.SortOrder = SortOrder.Ascending;
            //pageSpecification.SortOrder.Add(endSortOrder);
            pageSpecification.Start = 1;
            pageSpecification.Size = 1000;
            pageSpecification.CheckData();

            speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification);
            Assert.IsTrue(speciesObservations.Count <= pageSpecification.Size);
        }

        [TestMethod]
        [Ignore]
        public void GetDarwinCoreBySearchCriteriaPageBigData()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebDarwinCore> speciesObservations;

            WebSpeciesObservationPageSpecification pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();

            WebSpeciesObservationFieldSortOrder startSortOrder = new WebSpeciesObservationFieldSortOrder();
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Debug.WriteLine("1 - 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            pageSpecification.Start = 10000;
            pageSpecification.Size = 100;
            stopwatch.Restart();
            speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Debug.WriteLine("10 000 - 10 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            pageSpecification.Start = 100000;
            pageSpecification.Size = 100;
            stopwatch.Restart();
            speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria,
                GetCoordinateSystem(), pageSpecification);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Debug.WriteLine("100 000 - 100 100: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            //pageSpecification.Start = 1000000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("1 000 000 - 1 000 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1500000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("1 500 000 - 1 500 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1400000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("1 400 000 - 1 400 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 11000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("11 000 - 11 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1400000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("1 400 000 - 1 400 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();

            //pageSpecification.Start = 1900000;
            //pageSpecification.Size = 100;
            //stopwatch.Restart();
            //speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification);
            //Assert.IsTrue(speciesObservations.IsNotEmpty());
            //Debug.WriteLine("1 900 000 - 1 900 100: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Stop();
        }

        [TestMethod]
        // [Ignore]
        public void GetDarwinCoreChangeAll()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = null;
            WebDarwinCoreChange webDarwinCoreChange;

            long changeId = 94760000; // 24814680; // 67018357; 
            DateTime changedFrom = new DateTime(2013, 6, 26);
            DateTime changedTo = new DateTime(2013, 6, 27);
            Boolean isChangedFromSpecified = false;
            Boolean isChangedToSpecified = false;
            Boolean isChangedIdSpecified = true;
            long maxReturnedChanges = 100;
            Boolean moreData = true;
            int i = 0;
            while (moreData)
            {
                webDarwinCoreChange = SpeciesObservationManager.GetDarwinCoreChange(Context,
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
        public void GetDarwinCoreChange()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = null;
            WebDarwinCoreChange webDarwinCoreChange;

            long changeId = 0; // 27091302; //16561900; // 11109018; //11189018;
            DateTime changedFrom = new DateTime(2013, 10, 04);
            DateTime changedTo = new DateTime(2013, 10, 05);
            Boolean isChangedFromSpecified = true;
            Boolean isChangedToSpecified = true;
            Boolean isChangedIdSpecified = false;
            long maxReturnedChanges = 4000;
            Boolean moreData = true;
            int i = 0;
            long currentMaxChangeId = 0;
            while (moreData)
            {
                webDarwinCoreChange = SpeciesObservationManager.GetDarwinCoreChange(Context, changedFrom,
                    isChangedFromSpecified,
                    changedTo, isChangedToSpecified, changeId,
                    isChangedIdSpecified, maxReturnedChanges,
                    searchCriteria, GetCoordinateSystem());
                moreData = webDarwinCoreChange.IsMoreSpeciesObservationsAvailable;

                changeId = webDarwinCoreChange.MaxChangeId + 1; //ta inte med sista changeID igen, därför +1

                isChangedIdSpecified = true;
                isChangedFromSpecified = false;
                i++;

                Assert.IsTrue(currentMaxChangeId != webDarwinCoreChange.MaxChangeId);
                currentMaxChangeId = webDarwinCoreChange.MaxChangeId;



                Debug.WriteLine("ReadNo: " + i
                                + " Created:" + webDarwinCoreChange.CreatedSpeciesObservations.Count
                                + " Updated:" + webDarwinCoreChange.UpdatedSpeciesObservations.Count
                                + " Deleted:" + webDarwinCoreChange.DeletedSpeciesObservationGuids.Count
                                + " MaxChangeId: " + webDarwinCoreChange.MaxChangeId
                                + " More data: " + webDarwinCoreChange.IsMoreSpeciesObservationsAvailable);
            }
        }

        [TestMethod]
        public void GetDarwinCoreChangeSearchCriteriaObservationDateTime()
        {
            WebDarwinCoreChange webDarwinCoreChange;

            const long changeId = 16551900; //16561900; // 11109018; //11189018;
            DateTime changedFrom = new DateTime(2013, 6, 26);
            DateTime changedTo = new DateTime(2013, 6, 27);
            const Boolean isChangedFromSpecified = true;
            const Boolean isChangedToSpecified = true;
            const Boolean isChangedIdSpecified = false;
            const long maxReturnedChanges = 25000;

            WebSpeciesObservationSearchCriteria searchCriteria;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 5, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(221917);

            webDarwinCoreChange = SpeciesObservationManager.GetDarwinCoreChange(Context, changedFrom,
                isChangedFromSpecified,
                changedTo, isChangedToSpecified, changeId,
                isChangedIdSpecified, maxReturnedChanges,
                searchCriteria, GetCoordinateSystem());

            Assert.IsTrue(webDarwinCoreChange.IsNotNull());
        }

        [TestMethod]
        public void GetDarwinCoreChangeSearchCriteriaRegionGuids()
        {
            WebDarwinCoreChange webDarwinCoreChange;

            const long changeId = 72098000;
            DateTime changedFrom = new DateTime(2016, 8, 26);
            DateTime changedTo = new DateTime(2016, 8, 28);
            const Boolean isChangedFromSpecified = true;
            const Boolean isChangedToSpecified = true;
            const Boolean isChangedIdSpecified = false;
            const long maxReturnedChanges = 5000;

            WebSpeciesObservationSearchCriteria searchCriteria;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);

            // Stockholms kommun.
            // searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet1Feature180");

            // Göteborgs kommun.
            // searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet1Feature1480");

            webDarwinCoreChange = SpeciesObservationManager.GetDarwinCoreChange(Context, changedFrom,
                isChangedFromSpecified,
                changedTo, isChangedToSpecified, changeId,
                isChangedIdSpecified, maxReturnedChanges,
                searchCriteria, GetCoordinateSystem());

            Assert.IsTrue(webDarwinCoreChange.IsNotNull());
            Assert.IsTrue(webDarwinCoreChange.CreatedSpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationAccuracy()
        {
            Boolean hasProtectedSpeciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 1000;
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1580000, 6640000);
            hasProtectedSpeciesObservations = SpeciesObservationManager.GetProtectedSpeciesObservationIndication(
                Context, searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;

            searchCriteria.Accuracy = 10;
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1580000, 6640000);
            hasProtectedSpeciesObservations = SpeciesObservationManager.GetProtectedSpeciesObservationIndication(
                Context, searchCriteria, GetCoordinateSystem());
            Assert.IsFalse(hasProtectedSpeciesObservations);
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationAccuracyElasticsearch()
        {
            Boolean hasProtectedSpeciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1590000, 6650000);
            searchCriteria.BoundingBox.Min = new WebPoint(1570000, 6600000);
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 1000;
            hasProtectedSpeciesObservations =
                SpeciesObservationManager.GetProtectedSpeciesObservationIndicationElasticsearch(Context, searchCriteria,
                    GetCoordinateSystem());
            Assert.IsTrue(hasProtectedSpeciesObservations);
            searchCriteria.Polygons = null;

            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1581000, 6650000);
            searchCriteria.BoundingBox.Min = new WebPoint(1579000, 6649000);
            searchCriteria.Accuracy = 1;
            hasProtectedSpeciesObservations =
                SpeciesObservationManager.GetProtectedSpeciesObservationIndicationElasticsearch(Context, searchCriteria,
                    GetCoordinateSystem());
            Assert.IsFalse(hasProtectedSpeciesObservations);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void GetProtectedSpeciesObservationIndicationNoGeometryError()
        {
            Boolean hasProtectedSpeciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.Accuracy = 1000;
            hasProtectedSpeciesObservations = SpeciesObservationManager.GetProtectedSpeciesObservationIndication(
                Context, searchCriteria, GetCoordinateSystem());
            Assert.IsTrue(hasProtectedSpeciesObservations);
        }

        [TestMethod]
        public void GetProtectedSpeciesObservationIndicationObservationDateTime()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // Test that Begin can't be to close to today.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1584300, 6647900);
            searchCriteria.ObservationDateTime.Begin = DateTime.Now;
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            SpeciesObservationManager.GetProtectedSpeciesObservationIndication(Context, searchCriteria,
                GetCoordinateSystem());
            Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <
                          DateTime.Now - new TimeSpan(300, 0, 0, 0));
            searchCriteria.Polygons = null;

            // Test that End can't be in the past.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1584300, 6647900);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2005, 1, 1);
            SpeciesObservationManager.GetProtectedSpeciesObservationIndication(Context, searchCriteria,
                GetCoordinateSystem());
            Assert.IsTrue(DateTime.Now <= searchCriteria.ObservationDateTime.End);
            searchCriteria.Polygons = null;

            // Test that PartOfYear is not used.
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1584351, 6647917);
            searchCriteria.BoundingBox.Min = new WebPoint(1584300, 6647900);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();
            searchCriteria.ObservationDateTime.PartOfYear.Add(new WebDateTimeInterval());
            searchCriteria.ObservationDateTime.PartOfYear[0].Begin = DateTime.Now;
            searchCriteria.ObservationDateTime.PartOfYear[0].End = DateTime.Now + new TimeSpan(10, 0, 0, 0);
            SpeciesObservationManager.GetProtectedSpeciesObservationIndication(Context, searchCriteria,
                GetCoordinateSystem());
            Assert.IsNull(searchCriteria.ObservationDateTime.PartOfYear);
        }

        [TestMethod]
        public void GetProvinceRegions()
        {
            List<WebRegion> regions;
            regions = SpeciesObservationManager.GetProvinceRegions(Context);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviders()
        {
            List<WebSpeciesObservationDataProvider> speciesObservationDataProviders;

            speciesObservationDataProviders = SpeciesObservationManager.GetSpeciesObservationDataProviders(Context);
            Assert.IsTrue(speciesObservationDataProviders.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsByIdsFieldSpecification()
        {
            List<Int64> speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(5747105);
            speciesObservationIds.Add(5747106);
            speciesObservationIds.Add(5747107);
            speciesObservationIds.Add(5747108);
            speciesObservationIds.Add(5747109);
            speciesObservationIds.Add(5747110);
            speciesObservationIds.Add(5747111);
            speciesObservationIds.Add(5747112);

            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            WebSpeciesObservationSpecification speciesObservationSpecification =
                new WebSpeciesObservationSpecification();
            speciesObservationSpecification.Fields = new List<WebSpeciesObservationFieldSpecification>();

            WebSpeciesObservationFieldSpecification speciesObservationFieldSpecification;

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "DarwinCore";
            speciesObservationFieldSpecification.Property.Identifier = "Id";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "Country";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "Continent";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "DarwinCore";
            speciesObservationFieldSpecification.Property.Identifier = "BibliographicCitation";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

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
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "DecimalLongitude";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "CoordinateX";
            speciesObservationSpecification.Fields.Add(speciesObservationFieldSpecification);

            speciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            speciesObservationFieldSpecification.Class = new WebSpeciesObservationClass();
            speciesObservationFieldSpecification.Property = new WebSpeciesObservationProperty();
            speciesObservationFieldSpecification.Class.Identifier = "Location";
            speciesObservationFieldSpecification.Property.Identifier = "CoordinateY";
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

            WebSpeciesObservationInformation webSpeciesObservationInformation =
                SpeciesObservationManager.GetSpeciesObservationsByIds(Context, speciesObservationIds, coordinateSystem,
                    speciesObservationSpecification);
            Assert.IsNotNull(webSpeciesObservationInformation);
            Assert.IsTrue(webSpeciesObservationInformation.SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservationIds.Count, webSpeciesObservationInformation.SpeciesObservationCount);
            Assert.AreEqual(speciesObservationIds.Count, webSpeciesObservationInformation.SpeciesObservations.Count);
            foreach (WebSpeciesObservation webSpeciesObservation in webSpeciesObservationInformation.SpeciesObservations
                )
            {
                Debug.WriteLine("---");
                foreach (WebSpeciesObservationField webSpeciesObservationField in webSpeciesObservation.Fields)
                {
                    Debug.WriteLine(webSpeciesObservationField.ClassIdentifier + ": " +
                                    webSpeciesObservationField.PropertyIdentifier + ": "
                                    + webSpeciesObservationField.Value + ": " + webSpeciesObservationField.Type);
                }
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
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
            pageSpecification.CheckData();

            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPage(Context,
                searchCriteria, GetCoordinateSystem(), pageSpecification, null);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaTaxonIdsCompareDatabases()
        {
            Dictionary<Int32, TaxonInformation> taxonInformationCache;
            Int32 taxonId;
            List<Int32> taxonIds;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(Context);
            taxonIds = new List<Int32>();
            foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
            {
                if (taxonInformation.TaxonCategoryId == (Int32) (TaxonCategoryId.Species))
                {
                    taxonIds.Add(taxonInformation.DyntaxaTaxonId);
                }
            }

            for (Int32 index = 0; index < taxonIds.Count && index < 10; index++)
            {
                taxonId = taxonIds[index];
                searchCriteria = new WebSpeciesObservationSearchCriteria();
                searchCriteria.TaxonIds = new List<Int32>();
                searchCriteria.TaxonIds.Add(taxonId);
                searchCriteria.IncludePositiveObservations = true;
                information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
                information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
                Debug.WriteLine("Taxon id = " + taxonId);
                if (information1.SpeciesObservations.IsNotEmpty() &&
                    information2.SpeciesObservations.IsNotEmpty() &&
                    (information1.SpeciesObservations.Count != information2.SpeciesObservations.Count))
                {
                    break;
                }

                if (information1.SpeciesObservationIds.IsNotEmpty() &&
                    information2.SpeciesObservationIds.IsNotEmpty() &&
                    (information1.SpeciesObservationIds.Count != information2.SpeciesObservationIds.Count))
                {
                    break;
                }

                CompareSpeciesObservations(information1, information2);
            }
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
            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaUsesOwnerFieldSearchCriteriaExpectedSpeciesObservations()
        {
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            SetOwnerFieldSearchCriterias(searchCriteria);

            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaUsesFieldSearchCriteriaUncertainDetermination()
        {
            WebSpeciesObservationInformation speciesObservations1, speciesObservations2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            // Add FieldSearchCriteria for UncertainDetermination with false (Do not include uncertain determination)
            SetUncertainDeterminationSearchCriterias(searchCriteria, false);
            var uncertainCriteria = searchCriteria.FieldSearchCriteria;

            // Add another type of FieldSearchCriteria
            SetOwnerFieldSearchCriterias(searchCriteria);
            searchCriteria.FieldSearchCriteria.AddRange(uncertainCriteria);

            speciesObservations1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations1.SpeciesObservations.IsNotEmpty());

            // Add FieldSearchCriteria for UncertainDetermination with true (Do only include uncertain determination)
            SetUncertainDeterminationSearchCriterias(searchCriteria, true);
            
            searchCriteria.TaxonIds.Clear();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.AppleTrees));
            speciesObservations2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations2.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaUsesOrCombinedFieldSearchCriteriasExpectedSpeciesObservations()
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

            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations.SpeciesObservations.IsNotEmpty());
        }

        [Ignore]
        [TestMethod]
        public void
            GetSpeciesObservationsBySearchCriteriaUsesCoordinateUncertaintyInMetersFieldSearchCriteriaExpectedSpeciesObservations
            ()
        {
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.DataProviderGuids = new List<string>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:8");

            SetCoordinateUncertaintyInMetersFieldSearchCriteria(searchCriteria);

            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(speciesObservations.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void
            GetSpeciesObservationsBySearchCriteriaStenskvattaInGoetalandFrom2007To2013ReturnObservationsSuccessfully()
        {
            WebSpeciesObservationInformation speciesObservations;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102996);
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet13Feature4");
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2007, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 6, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(speciesObservations);
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
                speciesObservationChange = SpeciesObservationManager.GetSpeciesObservationChange(Context,
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
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCriteriasSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = null;
            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument null exception occurred.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCoordinateSystemSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = null;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument null exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaAccurrancyTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 30;
            searchCriteria.IsAccuracySpecified = true;


            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            // Increase Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;


            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurancyFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IncludePositiveObservations = true;

            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument null exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Don't use accurancy, all positiv observations should be collected
            searchCriteria.IsAccuracySpecified = false;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            // Enable Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IsAccuracySpecified = true;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 < noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;
            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaIsNaturalOccuranceTest()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxGoogleMercatorTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxWgs84Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxSweref99Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new WebPoint(820000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxRt9025GonVTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxRt90Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNoneTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(90, 90);
            searchCriteria.BoundingBox.Min = new WebPoint(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxInvalidMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                    coordinateSystem);

            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria = new WebSpeciesObservationSearchCriteria();
                    SetDefaultSearchCriteria(searchCriteria);
                    searchCriteria.BoundingBox = new WebBoundingBox();
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new WebPoint(1113195, 31118890);
                    SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                        coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail(
                        "No argument exception thrown that YMin value is larger that YMax value for bounding box.");
                }
                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");
            }
            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNullMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria = new WebSpeciesObservationSearchCriteria();
                SetDefaultSearchCriteria(searchCriteria);
                searchCriteria.BoundingBox = new WebBoundingBox();
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                    coordinateSystem);

            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria = new WebSpeciesObservationSearchCriteria();
                    SetDefaultSearchCriteria(searchCriteria);
                    searchCriteria.BoundingBox = new WebBoundingBox();
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                        coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");
                }
                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");
            }
            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaChangeDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 07, 25);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 08, 01);

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaChangePartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            // Get complete years data
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2012, 02, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2012, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2015, 05, 01);
            interval2.End = new DateTime(2015, 05, 10);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2012, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2012, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2015, 04, 01);
            interval3.End = new DateTime(2015, 05, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations >= noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 >= noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 >= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaDataProvidersTest()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:19");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaDataProviderInvalidTest()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;

            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityAllConditionsTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<int>();
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            // operatorString.SearchString = "Per Lundkvist";
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringAllConditionsTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 2;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations7 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations8 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

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
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
            Assert.IsTrue(noOfObservations3 >= noOfObservations2);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Greater;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateInvalidDatesTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            // Get complete years data
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 06, 01);
            interval.End = new DateTime(2001, 09, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 06, 01);
            interval.End = new DateTime(2001, 06, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 09, 01);
            interval2.End = new DateTime(2002, 06, 01);
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 07, 01);
            interval.End = new DateTime(2001, 07, 31);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 09, 01);
            interval2.End = new DateTime(2002, 06, 30);
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2001, 07, 01);
            interval3.End = new DateTime(2002, 06, 30);
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

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
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearIsDayOfYearSpecifiedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;


            // Get less amount of data since only two mounth within a year
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2000, 12, 31);
            interval2.End = new DateTime(2001, 07, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Adding one more time interval to the first one from nov to jan
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2000, 12, 31);
            interval2.End = new DateTime(2001, 07, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2000, 08, 01);
            interval3.End = new DateTime(2001, 07, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Not using day of year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals3 = new List<WebDateTimeInterval>();
            interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2000, 08, 01);
            interval3.End = new DateTime(2001, 07, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations4 >= noOfObservations3);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof (ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearFailedTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2008, 06, 01);
            interval.End = new DateTime(2008, 03, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria,
                coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test search criteria Polygons.
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239)); //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141)); //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178)); //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239)); //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141)); //Örebro
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsDifferentCoordinateSystemsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            CoordinateConversionManager coordinateConversionManager = new CoordinateConversionManager();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Create polygon
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239)); //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141)); //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178)); //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            //GoogleMercator
            WebCoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new WebCoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonMercator = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem,
                coordinateSystemMercator);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonMercator);
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystemMercator);
            //RT90
            WebCoordinateSystem coordinateSystemRt90;
            coordinateSystemRt90 = new WebCoordinateSystem();
            coordinateSystemRt90.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonRt90 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem,
                coordinateSystemRt90);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRt90);
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystemRt90);

            //Rt90_25_gon_v
            WebCoordinateSystem coordinateSystemRt9025GonV;

            coordinateSystemRt9025GonV = new WebCoordinateSystem();
            coordinateSystemRt9025GonV.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonRt9025GonV = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem,
                coordinateSystemRt9025GonV);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRt9025GonV);
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystemRt9025GonV);

            //SWEREF99
            WebCoordinateSystem coordinateSystemSweref99;
            coordinateSystemSweref99 = new WebCoordinateSystem();
            coordinateSystemSweref99.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonSweref99 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem,
                coordinateSystemSweref99);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonSweref99);
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystemSweref99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 0.2 % difference in result
            double delta = noOfObservations*0.002;
            Assert.IsTrue(noOfObservations > 0);
            Assert.AreEqual(noOfObservations, noOfObservations2, delta);
            Assert.AreEqual(noOfObservations, noOfObservations3, delta);
            Assert.AreEqual(noOfObservations, noOfObservations4, delta);
            Assert.AreEqual(noOfObservations, noOfObservations5, delta);


        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2015, 02, 01);
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2016, 01, 01);

            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 03, 31);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 02, 28);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2012, 04, 15);
            intervals2.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 02, 28);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2010, 04, 15);
            intervals.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 02, 01);
            interval3.End = new DateTime(2010, 04, 15);
            intervals3.Add(interval3);
            searchCriteria.ReportedDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

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
            // TODO something is wrong with timeintervals and its result..
            // Assert.IsTrue(noOfObservations5 <= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListdCategoriesTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088); //Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916); //Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088); //Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916); //Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListTaxaTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088); //Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916); //Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088); //Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916); //Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;
            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;
            Int64 noOfObservations2 = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaTaxaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088); // Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916); //Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Debug.Write("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaUsedAllCriteriasTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            SetDefaultSearchCriteria(searchCriteria);

            List<int> taxa = new List<int>();
            taxa.Add((Int32) (TaxonId.DrumGrasshopper));
            taxa.Add((Int32) (TaxonId.Carnivore));


            searchCriteria.TaxonIds = taxa;

            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);

            // Create polygon in WGS84
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239)); //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141)); //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178)); //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            searchCriteria.DataProviderGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.


            Int64 noOfObservations = SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(Context,
                searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        private static void SetDefaultSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            searchCriteria.TaxonIds = new List<int>();
            //            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
            //            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IsAccuracySpecified = false;
            searchCriteria.Accuracy = 50;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.IncludePositiveObservations = true;
        }


        [TestMethod]
        public void GetSpeciesObservationsByIds()
        {
            List<Int64> speciesObservationIds;
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservationInformation speciesObservationInformation;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            // Get species observations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Size = 100;
            pageSpecification.Start = 1000;
            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPage(Context, searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, pageSpecification.Size);

            // Get species observation ids.
            speciesObservationIds = new List<Int64>();
            foreach (WebSpeciesObservation speciesObservation in speciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsNotNull(field);
                speciesObservationIds.Add(field.Value.WebParseInt64());
            }

            Assert.AreEqual(speciesObservationIds.Count, pageSpecification.Size);

            // Get species observations by ids.
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsByIds(Context,
                speciesObservationIds, GetCoordinateSystem(), null);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservationInformation.SpeciesObservations.Count,
                speciesObservationIds.Count);
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(speciesObservationIds.Contains(field.Value.WebParseInt64()));
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsByIdsElasticsearch()
        {
            List<Int64> speciesObservationIds;
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservationInformation speciesObservationInformation;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            // Get species observations.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Size = 100;
            pageSpecification.Start = 1000;
            speciesObservations =
                SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPageElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem(), pageSpecification, null);
            Assert.IsTrue(speciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, pageSpecification.Size);

            // Get species observation ids.
            speciesObservationIds = new List<Int64>();
            foreach (WebSpeciesObservation speciesObservation in speciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsNotNull(field);
                speciesObservationIds.Add(field.Value.WebParseInt64());
            }

            Assert.AreEqual(speciesObservationIds.Count, pageSpecification.Size);

            // Get species observations by ids.
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsByIdsElasticsearch(Context,
                speciesObservationIds, GetCoordinateSystem(), null);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservationInformation.SpeciesObservations.Count,
                speciesObservationIds.Count);
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(speciesObservationIds.Contains(field.Value.WebParseInt64()));
            }
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteriaAccurrancyElasticsearch()
        {
            Int64 speciesObservationCount1, speciesObservationCount2;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.Accuracy = 30;
            searchCriteria.IsAccuracySpecified = true;
            speciesObservationCount1 =
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context,
                    searchCriteria, coordinateSystem);
            Assert.IsTrue(speciesObservationCount1 > 0);

            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            speciesObservationCount2 =
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context,
                    searchCriteria, coordinateSystem);
            Assert.IsTrue(speciesObservationCount2 > speciesObservationCount1);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaAccuracyElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 60;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.CoordinateUncertaintyInMeters.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(field.Value.WebParseInt32() <= searchCriteria.Accuracy);
            }

            searchCriteria.Accuracy = 30;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.CoordinateUncertaintyInMeters.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(field.Value.WebParseInt32() <= searchCriteria.Accuracy);
            }

            Assert.IsTrue(information2.SpeciesObservationCount <
                          information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaAccuracyCompareDatabases()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            searchCriteria.Accuracy = 60;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null, null);
            CompareSpeciesObservations(information1, information2);

            searchCriteria.Accuracy = 30;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria,
                coordinateSystem, null, null);
            CompareSpeciesObservations(information1, information2);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBirdNestActivityLimitElasticsearch()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 5, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 5, 10);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));

            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.BirdNestActivityLimit =
                SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetBirdNestActivities(Context)[3].Id;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);

            searchCriteria.IsBirdNestActivityLimitSpecified = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaBoundingBoxElasticsearch()
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
            speciesObservations = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaChangeDateTimeElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 7, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 8, 1);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= field.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= field.Value.WebParseDateTime());
            }

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2015, 6, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 9, 1);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(searchCriteria.ChangeDateTime.Begin <= field.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ChangeDateTime.End >= field.Value.WebParseDateTime());
            }

            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaChangeDateTimeIntervalElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 12, 31);
            searchCriteria.ChangeDateTime.PartOfYear = new List<WebDateTimeInterval>();

            // Test day of year with Begin < End.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 5);
            searchCriteria.ChangeDateTime.PartOfYear.Clear();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            //// Test day of year with End < Begin.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 12, 29);
            dateTimeInterval.End = new DateTime(2001, 1, 2);
            searchCriteria.ChangeDateTime.PartOfYear.Clear();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            // Test exact date with Begin < End.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ChangeDateTime.PartOfYear.Clear();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.Month < field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= field.Value.WebParseDateTime().Day)));
                Assert.IsTrue((dateTimeInterval.End.Month > field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= field.Value.WebParseDateTime().Day)));
            }

            // Test exact date with End < Begin.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 12, 29);
            dateTimeInterval.End = new DateTime(2001, 1, 2);
            searchCriteria.ChangeDateTime.PartOfYear.Clear();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.Month < field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= field.Value.WebParseDateTime().Day)) ||
                              (dateTimeInterval.End.Month > field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= field.Value.WebParseDateTime().Day)));
            }

            // Test with more than one interval.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ChangeDateTime.PartOfYear.Clear();
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 1, 6);
            dateTimeInterval.End = new DateTime(2000, 1, 7);
            searchCriteria.ChangeDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaDataProviderGuidsElasticsearch()
        {
            List<WebSpeciesObservationDataProvider> speciesObservationDataProviders;
            WebSpeciesObservationField field;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation information1, information2;

            speciesObservationDataProviders = SpeciesObservationManager.GetSpeciesObservationDataProviders(Context);
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.DataProviderGuids.Add(speciesObservationDataProviders[0].Guid);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.DatasetName.ToString());
                Assert.AreEqual(speciesObservationDataProviders[0].Name, field.Value);
            }

            searchCriteria.DataProviderGuids.Add(speciesObservationDataProviders[1].Guid);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount >= information1.SpeciesObservationCount);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.DatasetName.ToString());
                Assert.IsTrue((speciesObservationDataProviders[0].Name == field.Value) ||
                              (speciesObservationDataProviders[1].Name == field.Value));
            }

            searchCriteria.DataProviderGuids = new List<String>();
            foreach (WebSpeciesObservationDataProvider dataProvider in speciesObservationDataProviders)
            {
                searchCriteria.DataProviderGuids.Add(dataProvider.Guid);
            }

            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaDataProviderGuidsCompareDatabases()
        {
            WebSpeciesObservationDataProvider dataProvider;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            dataProvider =
                WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationDataProvider(Context,
                    SpeciesObservationDataProviderId.Mvm);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add(dataProvider.Guid);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2016, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(4000147);
            searchCriteria.IncludePositiveObservations = true;

            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria,
                GetCoordinateSystem(), null, null);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CompareSpeciesObservations(information1, information2);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaBoolean()
        {
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Identification;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.UncertainDetermination;
            fieldSearchCriteria.Type = WebDataType.Boolean;

            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = Boolean.TrueString;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Identification,
                                                           SpeciesObservationPropertyId.UncertainDetermination);
                Assert.IsTrue(field.Value.WebParseBoolean());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaBooleanElasticsearch()
        {
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mammals));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Occurrence;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.IsNotRediscoveredObservation;
            fieldSearchCriteria.Type = WebDataType.Boolean;

            // Test with equal condition and true.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = Boolean.TrueString;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString());
                Assert.IsTrue(field.Value.WebParseBoolean());
            }

            // Test with not equal condition and false.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = Boolean.FalseString;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString());
                Assert.IsTrue(field.Value.WebParseBoolean());
            }

            Assert.AreEqual(information1.SpeciesObservationCount,
                information2.SpeciesObservationCount);

            searchCriteria.TaxonIds.Clear();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test with equal condition and false.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = Boolean.FalseString;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString());
                Assert.IsFalse(field.Value.WebParseBoolean());
            }

            // Test with not equal condition and true.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = Boolean.TrueString;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString());
                Assert.IsFalse(field.Value.WebParseBoolean());
            }

            Assert.AreEqual(information1.SpeciesObservationCount,
                information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaDateTimeElasticsearch()
        {
            DateTime modified;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Get used date time from existing observation.
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            field = ListWeb.GetField(information1.SpeciesObservations[3].Fields,
                SpeciesObservationClassId.DarwinCore.ToString(),
                SpeciesObservationPropertyId.Modified.ToString());
            Assert.IsNotNull(field);
            modified = field.Value.WebParseDateTime();

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.DarwinCore;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.Modified;
            fieldSearchCriteria.Type = WebDataType.DateTime;

            // Test compare operator equal.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = modified.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.AreEqual(modified, field.Value.WebParseDateTime());
            }

            // Test compare operator greater.
            fieldSearchCriteria.Operator = CompareOperator.Greater;
            fieldSearchCriteria.Value = modified.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(modified < field.Value.WebParseDateTime());
            }

            // Test compare operator greater or equal.
            fieldSearchCriteria.Operator = CompareOperator.GreaterOrEqual;
            fieldSearchCriteria.Value = modified.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(modified <= field.Value.WebParseDateTime());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator less.
            fieldSearchCriteria.Operator = CompareOperator.Less;
            fieldSearchCriteria.Value = modified.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(modified > field.Value.WebParseDateTime());
            }

            // Test compare operator less or equal.
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Value = modified.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(modified >= field.Value.WebParseDateTime());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator not equal.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = modified.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.AreNotEqual(modified, field.Value.WebParseDateTime());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaFloat64Elasticsearch()
        {
            Double longitude;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Get used date time from existing observation.
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            field = ListWeb.GetField(information1.SpeciesObservations[3].Fields,
                SpeciesObservationClassId.Location.ToString(),
                SpeciesObservationPropertyId.DecimalLongitude.ToString());
            Assert.IsNotNull(field);
            longitude = field.Value.WebParseDouble();

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.DecimalLongitude;
            fieldSearchCriteria.Type = WebDataType.Float64;

            // Test compare operator equal.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = longitude.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.AreEqual(longitude, field.Value.WebParseDouble());
            }

            // Test compare operator greater.
            fieldSearchCriteria.Operator = CompareOperator.Greater;
            fieldSearchCriteria.Value = longitude.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.IsTrue(longitude < field.Value.WebParseDouble());
            }

            // Test compare operator greater or equal.
            fieldSearchCriteria.Operator = CompareOperator.GreaterOrEqual;
            fieldSearchCriteria.Value = longitude.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.IsTrue(longitude <= field.Value.WebParseDouble());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator less.
            fieldSearchCriteria.Operator = CompareOperator.Less;
            fieldSearchCriteria.Value = longitude.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.IsTrue(longitude > field.Value.WebParseDouble());
            }

            // Test compare operator less or equal.
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Value = longitude.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.IsTrue(longitude >= field.Value.WebParseDouble());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator not equal.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = longitude.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.DecimalLongitude.ToString());
                Assert.AreNotEqual(longitude, field.Value.WebParseDouble());
            }

            // Test species observation project parameter search criteria.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Project;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.None;
            fieldSearchCriteria.Property.Identifier = "ProjectParameterSpeciesGateway_ProjectParameter174";
            fieldSearchCriteria.Type = WebDataType.Float64;
            fieldSearchCriteria.Value = "1";
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaInt32Elasticsearch()
        {
            Int32 dyntaxaTaxonId;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mammals));

            // Get used date time from existing observation.
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            field = ListWeb.GetField(information1.SpeciesObservations[3].Fields,
                SpeciesObservationClassId.Taxon.ToString(),
                SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
            Assert.IsNotNull(field);
            dyntaxaTaxonId = field.Value.WebParseInt32();

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Taxon;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.DyntaxaTaxonID;
            fieldSearchCriteria.Type = WebDataType.Int32;

            // Test compare operator equal.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.AreEqual(dyntaxaTaxonId, field.Value.WebParseInt32());
            }

            // Test compare operator greater.
            fieldSearchCriteria.Operator = CompareOperator.Greater;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.IsTrue(dyntaxaTaxonId < field.Value.WebParseInt32());
            }

            // Test compare operator greater or equal.
            fieldSearchCriteria.Operator = CompareOperator.GreaterOrEqual;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.IsTrue(dyntaxaTaxonId <= field.Value.WebParseInt32());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator less.
            fieldSearchCriteria.Operator = CompareOperator.Less;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.IsTrue(dyntaxaTaxonId > field.Value.WebParseInt32());
            }

            // Test compare operator less or equal.
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.IsTrue(dyntaxaTaxonId >= field.Value.WebParseInt32());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator not equal.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = dyntaxaTaxonId.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.AreNotEqual(dyntaxaTaxonId, field.Value.WebParseInt32());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaInt64Elasticsearch()
        {
            Int64 id;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 5;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mammals));

            // Get used date time from existing observation.
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            field = ListWeb.GetField(information1.SpeciesObservations[3].Fields,
                SpeciesObservationClassId.DarwinCore.ToString(),
                SpeciesObservationPropertyId.Id.ToString());
            Assert.IsNotNull(field);
            id = field.Value.WebParseInt64();

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.DarwinCore;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.Id;
            fieldSearchCriteria.Type = WebDataType.Int64;

            // Test compare operator equal.
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = id.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.AreEqual(id, field.Value.WebParseInt64());
            }

            // Test compare operator greater.
            fieldSearchCriteria.Operator = CompareOperator.Greater;
            fieldSearchCriteria.Value = id.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsTrue(id < field.Value.WebParseInt64());
            }

            // Test compare operator greater or equal.
            fieldSearchCriteria.Operator = CompareOperator.GreaterOrEqual;
            fieldSearchCriteria.Value = id.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsTrue(id <= field.Value.WebParseInt64());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator less.
            fieldSearchCriteria.Operator = CompareOperator.Less;
            fieldSearchCriteria.Value = id.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsTrue(id > field.Value.WebParseInt64());
            }

            // Test compare operator less or equal.
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Value = id.WebToString();
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.IsTrue(id >= field.Value.WebParseInt64());
            }

            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);

            // Test compare operator not equal.
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = id.WebToString();
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Id.ToString());
                Assert.AreNotEqual(id, field.Value.WebParseInt64());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaString()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            Int64 speciesObservationCount1, speciesObservationCount2;
            String locality;
            WebSpeciesObservationField localityField;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102978); // större hackspett

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.County;
            fieldSearchCriteria.Type = WebDataType.String;

            // Test compare operator begins with.
            locality = "Uppsal";
            fieldSearchCriteria.Operator = CompareOperator.BeginsWith;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.IsTrue(localityField.Value.StartsWith(locality, true, ci));

            }

            // Test compare operator contains.
            locality = "ppsal";
            fieldSearchCriteria.Operator = CompareOperator.Contains;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.IsTrue(localityField.Value.ToLower().Contains(locality.ToLower()));
            }

            // Test compare operator ends with.
            locality = "ppsala";
            fieldSearchCriteria.Operator = CompareOperator.EndsWith;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.IsTrue(localityField.Value.EndsWith(locality, true, ci));
            }

            // Test compare operator equal.
            locality = "Uppsala";
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.IsTrue(localityField.Value.ToLower().Equals(locality.ToLower()));
            }

            // Test compare operator like.
            locality = "%p%a%l%";
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("p") > -1);
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("a") > -1);
            }

            // Test compare operator not equal.
            locality = "Fullerö backar";
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = locality;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.County.ToString());
                Assert.AreNotEqual(localityField.Value.ToLower(), locality.ToLower());
            }

            // Test with character ' in search value.
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Value = "*a*b'*c*";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(information);
            Assert.AreEqual(0, information.SpeciesObservationCount);

            // Test with two species observation field search criterias.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.County;
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "Jönköping";
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.StateProvince;
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "Småland";
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.Municipality;
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "Vetlanda";
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

            searchCriteria.DataFields = new List<WebDataField>();
            searchCriteria.DataFields.Add(new WebDataField());
            searchCriteria.DataFields[0].Name = "FieldLogicalOperator";
            searchCriteria.DataFields[0].Type = WebDataType.String;
            searchCriteria.DataFields[0].Value = LogicalOperator.And.ToString();
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            speciesObservationCount1 = information.SpeciesObservationCount;
            searchCriteria.DataFields[0].Value = LogicalOperator.Or.ToString();
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            speciesObservationCount2 = information.SpeciesObservationCount;
            Assert.IsTrue(speciesObservationCount1 < speciesObservationCount2);

            // Test species observation project parameter search criteria.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Project;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.None;
            fieldSearchCriteria.Property.Identifier = "ProjectParameterSpeciesGateway_ProjectParameter175";
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "Fågelskär";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaFieldSearchCriteriaStringElasticsearch()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            Int64 speciesObservationCount1, speciesObservationCount2;
            String locality;
            WebSpeciesObservationField localityField;
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102978); // större hackspett

            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.Locality;
            fieldSearchCriteria.Type = WebDataType.String;

            // Test compare operator begins with.
            locality = "Näsna";
            fieldSearchCriteria.Operator = CompareOperator.BeginsWith;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.StartsWith(locality, true, ci));

            }

            // Test compare operator contains.
            locality = "snare";
            fieldSearchCriteria.Operator = CompareOperator.Contains;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.ToLower().Contains(locality.ToLower()));
            }

            // Test compare operator ends with.
            locality = "snaren";
            fieldSearchCriteria.Operator = CompareOperator.EndsWith;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.EndsWith(locality, true, ci));
            }

            // Test compare operator equal.
            locality = "Näsnaren";
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.ToLower().Equals(locality.ToLower()));
            }

            // Test compare operator like.
            locality = "*a*b*c*";
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Value = locality;
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("a") > -1);
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("b") > -1);
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("c") > -1);
            }

            // Test compare operator not equal.
            locality = "Fullerö backar";
            fieldSearchCriteria.Operator = CompareOperator.NotEqual;
            fieldSearchCriteria.Value = locality;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.AreNotEqual(localityField.Value.ToLower(), locality.ToLower());
            }

            // Test with three species observation field search criterias.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Location;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.Locality;
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "Näsnaren";
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Taxon;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.VernacularName;
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Value = "kråka";
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

            searchCriteria.DataFields = new List<WebDataField>();
            searchCriteria.DataFields.Add(new WebDataField());
            searchCriteria.DataFields[0].Name = "FieldLogicalOperator";
            searchCriteria.DataFields[0].Type = WebDataType.String;
            searchCriteria.DataFields[0].Value = LogicalOperator.And.ToString();
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            speciesObservationCount1 = information.SpeciesObservationCount;
            searchCriteria.DataFields[0].Value = LogicalOperator.Or.ToString();
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            speciesObservationCount2 = information.SpeciesObservationCount;
            Assert.IsTrue(speciesObservationCount1 < speciesObservationCount2);

            // Test species observation project parameter search criteria.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesObservationFieldSearchCriteria>();
            fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
            fieldSearchCriteria.Class = new WebSpeciesObservationClass();
            fieldSearchCriteria.Class.Id = SpeciesObservationClassId.Project;
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty();
            fieldSearchCriteria.Property.Id = SpeciesObservationPropertyId.None;
            fieldSearchCriteria.Property.Identifier = "ProjectParameterSpeciesGateway_ProjectParameter175";
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "Fågelskär";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);

            // Test with character ' in search value.
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Value = "*a*b'*c*";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(information);
            Assert.AreEqual(0, information.SpeciesObservationCount);
        }

        /// <summary>
        /// Test problem reported by Forest Agency.
        /// The search with SqlServer should result in the same answer 
        /// as the search with Elasticsearch. Currently there is a bug in the
        /// SqlServer implementation.
        /// </summary>
        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaForestAgency()
        {
            WebCoordinateSystem coordinateSystem;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebDarwinCoreInformation informationElasticsearch, informationSqlServer;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1000;
            searchCriteria.BirdNestActivityLimit = 19;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracyConsidered = true;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IsBirdNestActivityLimitSpecified = true;
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = DateTime.Now;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(573762.302, 6283201.274));
            linearRing.Points.Add(new WebPoint(573770.249, 6283210.281));
            linearRing.Points.Add(new WebPoint(573775.698, 6283203.733));
            linearRing.Points.Add(new WebPoint(573784.584, 6283178.086));
            linearRing.Points.Add(new WebPoint(573783.099, 6283158.326));
            linearRing.Points.Add(new WebPoint(573826.778, 6283014.302));
            linearRing.Points.Add(new WebPoint(573816.242, 6283013.603));
            linearRing.Points.Add(new WebPoint(573735.551, 6283013.688));
            linearRing.Points.Add(new WebPoint(573679.456, 6282998.651));
            linearRing.Points.Add(new WebPoint(573666.348, 6283092.529));
            linearRing.Points.Add(new WebPoint(573745.611, 6283182.358));
            linearRing.Points.Add(new WebPoint(573759.232, 6283199.139));
            linearRing.Points.Add(new WebPoint(573762.302, 6283201.274));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            searchCriteria.TaxonIds = SpeciesObservationManager.GetSwedishForestAgencyTaxonIds(Context).GetInt32List();
            informationSqlServer = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(Context, searchCriteria, coordinateSystem, null);
            informationElasticsearch = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem, null);
            Assert.IsTrue(informationSqlServer.IsNotNull());
            Assert.IsTrue(informationElasticsearch.IsNotNull());
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
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(Context, searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                Assert.IsNotNull(speciesObservation);
                Assert.IsNotNull(speciesObservation.Fields);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaGetProjectParametersElasticsearch()
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
            speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
            foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
            {
                Assert.IsNotNull(speciesObservation);
                Assert.IsNotNull(speciesObservation.Fields);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIdsOnlyElasticsearch()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            if (Configuration.InstallationType == InstallationType.Production)
            {
                searchCriteria.TaxonIds.Add(1000503); // pärlemorfjärilar
            }
            else
            {
                searchCriteria.TaxonIds.Add(2002976); // äkta dagfjärilar
            }

            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsNotNull(information);
            Assert.IsTrue(information.SpeciesObservations.IsEmpty());
            Assert.IsTrue(information.SpeciesObservationIds.IsNotEmpty());
            Assert.AreEqual(information.SpeciesObservationCount,
                information.SpeciesObservationIds.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeNeverFoundObservationsElasticsearch()
        {
            WebSpeciesObservationField field;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(1633); // Rökpipsvamp.

            searchCriteria.IncludeNeverFoundObservations = true;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);

            searchCriteria.IncludeNeverFoundObservations = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount > information2.SpeciesObservationCount);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNeverFoundObservation.ToString());
                Assert.IsFalse(field.Value.WebParseBoolean());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeNotRediscoveredObservationsElasticsearch()
        {
            WebSpeciesObservationField field;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100119); // Gölgroda.

            searchCriteria.IncludeNotRediscoveredObservations = true;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);

            searchCriteria.IncludeNotRediscoveredObservations = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString());
                Assert.IsFalse(field.Value.WebParseBoolean());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludePositiveObservationsElasticsearch()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(1445); // bombmurkla

            searchCriteria.IncludePositiveObservations = true;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.IsPositiveObservation.ToString());
                Assert.IsFalse(field.Value.WebParseBoolean());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedListCategoriesElasticsearch()
        {
            RedListCategory redListCategory;
            WebSpeciesObservationField redlistCategoryField, dyntaxaTaxonIdField;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNotRediscoveredObservations = true;
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                searchCriteria.IncludeRedListCategories = new List<RedListCategory>();
                searchCriteria.IncludeRedListCategories.Add(redListCategory);
                searchCriteria.TaxonIds = null;
                information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem(), null, null);
                CheckSpeciesObservationInformation(information1);
                foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
                {
                    // The check for not empty red list category should not be
                    // necessary but data on test server is not up to date.
                    // This test has a problem with taxon 232265 that
                    // is redlisted as VU since its parent taxon is
                    // red listed as NT.
                    redlistCategoryField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Conservation.ToString(),
                        SpeciesObservationPropertyId.RedlistCategory.ToString());
                    dyntaxaTaxonIdField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Taxon.ToString(),
                        SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                    if (redlistCategoryField.Value.IsNotEmpty() &&
                        (dyntaxaTaxonIdField.Value.WebParseInt32() != 232265))
                    {
                        Assert.AreEqual(redListCategory.ToString(),
                            redlistCategoryField.Value.Substring(0, 2).ToUpper());
                    }
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIncludeRedlistedTaxaElasticsearch()
        {
            WebSpeciesObservationField redlistCategoryField, dyntaxaTaxonIdField;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 90;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(224932); // vanlig backsmörblomma.

            searchCriteria.IncludeRedlistedTaxa = true;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            if (information1.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
                {
                    redlistCategoryField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Conservation.ToString(),
                        SpeciesObservationPropertyId.RedlistCategory.ToString());
                    dyntaxaTaxonIdField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Taxon.ToString(),
                        SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                    Assert.IsTrue(dyntaxaTaxonIdField.Value.WebParseInt32() == 224932 ||
                                  redlistCategoryField.Value.IsNotEmpty());
                }
            }

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(224932); // vanlig backsmörblomma.
            searchCriteria.IncludeRedlistedTaxa = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
                {
                    dyntaxaTaxonIdField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Taxon.ToString(),
                        SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                    Assert.AreEqual(224932, dyntaxaTaxonIdField.Value.WebParseInt32());
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsAccuracyConsideredElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation speciesObservations1, speciesObservations2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1443618, 6365688);
            searchCriteria.BoundingBox.Min = new WebPoint(1442707, 6364005);
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.IsAccuracyConsidered = false;
            speciesObservations1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations1);

            searchCriteria.IsAccuracyConsidered = true;
            searchCriteria.Polygons = null;
            speciesObservations2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations2);

            Assert.IsTrue(speciesObservations1.SpeciesObservationCount <
                          speciesObservations2.SpeciesObservationCount);
        }

        [TestMethod]
        public void
            GetSpeciesObservationsBySearchCriteriaIsAccuracyConsideredAndIsDisturbanceSensitivityConsideredElasticsearch
            ()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation speciesObservations1, speciesObservations2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1443618, 6365688);
            searchCriteria.BoundingBox.Min = new WebPoint(1442707, 6364005);
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.IsAccuracyConsidered = false;
            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservations1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations1);

            searchCriteria.IsAccuracyConsidered = true;
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            searchCriteria.Polygons = null;
            speciesObservations2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations2);

            Assert.IsTrue(speciesObservations1.SpeciesObservationCount <
                          speciesObservations2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsDisturbanceSensitivityConsideredElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation speciesObservations1, speciesObservations2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(1443618, 6365688);
            searchCriteria.BoundingBox.Min = new WebPoint(1442707, 6364005);
            searchCriteria.IncludeRedlistedTaxa = true;
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.IsDisturbanceSensitivityConsidered = false;
            speciesObservations1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations1);

            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            searchCriteria.Polygons = null;
            speciesObservations2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                Context, searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(speciesObservations2);

            Assert.IsTrue(speciesObservations1.SpeciesObservationCount <= speciesObservations2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaIsNaturalOccurrenceElasticsearch()
        {
            WebSpeciesObservationField isNaturalOccurrenceField;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(233813); // stripgås.

            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            if (information1.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
                {
                    isNaturalOccurrenceField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Occurrence.ToString(),
                        SpeciesObservationPropertyId.IsNaturalOccurrence.ToString());
                    Assert.IsTrue(isNaturalOccurrenceField.Value.WebParseBoolean());
                }
            }

            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            if (information2.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
                {
                    isNaturalOccurrenceField = ListWeb.GetField(speciesObservation.Fields,
                        SpeciesObservationClassId.Occurrence.ToString(),
                        SpeciesObservationPropertyId.IsNaturalOccurrence.ToString());
                    Assert.IsFalse(isNaturalOccurrenceField.Value.WebParseBoolean());
                }
            }

            searchCriteria.IsIsNaturalOccurrenceSpecified = false;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaLocalityNameSearchStringElasticsearch()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            WebSpeciesObservationField localityField;
            WebSpeciesObservationInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102978); // större hackspett

            searchCriteria.LocalityNameSearchString = new WebStringSearchCriteria();
            searchCriteria.LocalityNameSearchString.SearchString = "Näsna";
            searchCriteria.LocalityNameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.StartsWith(searchCriteria.LocalityNameSearchString.SearchString, true,
                    ci));

            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.LocalityNameSearchString.SearchString = "snare";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(
                    localityField.Value.ToLower()
                        .Contains(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));
            }

            searchCriteria.LocalityNameSearchString.SearchString = "snaren";
            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.EndsWith(searchCriteria.LocalityNameSearchString.SearchString, true,
                    ci));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            //            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            //            searchCriteria.LocalityNameSearchString.SearchString = "Vattentäktsvägen, Strandskogen";
            searchCriteria.LocalityNameSearchString.SearchString = "Näsnaren";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(
                    localityField.Value.ToLower().Equals(searchCriteria.LocalityNameSearchString.SearchString.ToLower()));
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.LocalityNameSearchString.SearchString = "*a*b*c*";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("a", StringComparison.CurrentCulture) > -1);
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("b", StringComparison.CurrentCulture) > -1);
                Assert.IsTrue(localityField.Value.ToLower().IndexOf("c", StringComparison.CurrentCulture) > -1);
            }

            searchCriteria.LocalityNameSearchString.CompareOperators.Clear();
            searchCriteria.LocalityNameSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.LocalityNameSearchString.SearchString = "Fullerö backar";
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                localityField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.AreNotEqual(localityField.Value.ToLower(),
                    searchCriteria.LocalityNameSearchString.SearchString.ToLower());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaMaxProtectionLevelElasticsearch()
        {
            WebSpeciesObservationField protectionLevelField;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsMaxProtectionLevelSpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            searchCriteria.MaxProtectionLevel = 5;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                protectionLevelField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Conservation.ToString(),
                    SpeciesObservationPropertyId.ProtectionLevel.ToString());
                Assert.IsTrue(protectionLevelField.Value.WebParseInt32() <= searchCriteria.MaxProtectionLevel);
            }

            searchCriteria.MaxProtectionLevel = 1;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information2.SpeciesObservationCount <= information1.SpeciesObservationCount);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                protectionLevelField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Conservation.ToString(),
                    SpeciesObservationPropertyId.ProtectionLevel.ToString());
                Assert.IsTrue(protectionLevelField.Value.WebParseInt32() <= searchCriteria.MaxProtectionLevel);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaMinProtectionLevelElasticsearch()
        {
            WebSpeciesObservationField protectionLevelField;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(626); // Ljungögontröst
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsMinProtectionLevelSpecified = true;

            searchCriteria.MinProtectionLevel = 1;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                protectionLevelField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Conservation.ToString(),
                    SpeciesObservationPropertyId.ProtectionLevel.ToString());
                Assert.IsTrue(protectionLevelField.Value.WebParseInt32() >= searchCriteria.MinProtectionLevel);
            }
            searchCriteria.IsMaxProtectionLevelSpecified = false;

            searchCriteria.MinProtectionLevel = 5;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            Assert.IsTrue(information2.SpeciesObservations.IsEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTimeElasticsearch()
        {
            WebSpeciesObservationField endField, startField;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));

            // Test Begin and End.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1996, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= endField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= endField.Value.WebParseDateTime());
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1985, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1990, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= endField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= endField.Value.WebParseDateTime());
            }

            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);

            // Test Operator on Begin and End.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1990, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(startField.Value.WebParseDateTime() <= searchCriteria.ObservationDateTime.End);
                Assert.IsTrue(endField.Value.WebParseDateTime() >= searchCriteria.ObservationDateTime.Begin);
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(1990, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.Begin <= endField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= startField.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ObservationDateTime.End >= endField.Value.WebParseDateTime());
            }

            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);

            // Test Accuracy.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 400;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1997, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >=
                              (startField.Value.WebParseDateTime() - endField.Value.WebParseDateTime()));
            }

            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 40;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1950, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(1997, 1, 1);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                Assert.IsTrue(new TimeSpan(searchCriteria.ObservationDateTime.Accuracy.Days, 0, 0, 0) >=
                              (startField.Value.WebParseDateTime() - endField.Value.WebParseDateTime()));
            }

            Assert.IsTrue(information2.SpeciesObservationCount < information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObservationDateTimeIntervalElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationField endField, startField;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ObservationDateTime.PartOfYear = new List<WebDateTimeInterval>();

            // Test day of year with Begin < End and operator Excluding.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue(dateTimeInterval.Begin.DayOfYear <= startField.Value.WebParseDateTime().DayOfYear);
                Assert.IsTrue(dateTimeInterval.Begin.DayOfYear <= endField.Value.WebParseDateTime().DayOfYear);
                Assert.IsTrue(dateTimeInterval.End.DayOfYear >= startField.Value.WebParseDateTime().DayOfYear);
                Assert.IsTrue(dateTimeInterval.End.DayOfYear >= endField.Value.WebParseDateTime().DayOfYear);
            }

            // Test day of year with Begin < End and operator Including.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue(((dateTimeInterval.Begin.DayOfYear <= startField.Value.WebParseDateTime().DayOfYear) &&
                               (dateTimeInterval.End.DayOfYear >= startField.Value.WebParseDateTime().DayOfYear)) ||
                              ((dateTimeInterval.Begin.DayOfYear <= endField.Value.WebParseDateTime().DayOfYear) &&
                               (dateTimeInterval.End.DayOfYear >= endField.Value.WebParseDateTime().DayOfYear)) ||
                              ((dateTimeInterval.Begin.DayOfYear > startField.Value.WebParseDateTime().DayOfYear) &&
                               (dateTimeInterval.End.DayOfYear < endField.Value.WebParseDateTime().DayOfYear)) ||
                              ((endField.Value.WebParseDateTime() - startField.Value.WebParseDateTime()).Days > 365));
            }

            Assert.IsTrue(information1.SpeciesObservationCount <=
                          information2.SpeciesObservationCount);

            //// Test day of year with End < Begin and operator Excluding.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 12, 28);
            dateTimeInterval.End = new DateTime(2001, 1, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= startField.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= startField.Value.WebParseDateTime().DayOfYear));
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= endField.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= endField.Value.WebParseDateTime().DayOfYear));
            }

            // Test day of year with End < Begin and operator Including.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 12, 28);
            dateTimeInterval.End = new DateTime(2001, 1, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue(((dateTimeInterval.Begin.DayOfYear <= startField.Value.WebParseDateTime().DayOfYear) ||
                               (dateTimeInterval.End.DayOfYear >= startField.Value.WebParseDateTime().DayOfYear)) ||
                              ((dateTimeInterval.Begin.DayOfYear <= endField.Value.WebParseDateTime().DayOfYear) ||
                               (dateTimeInterval.End.DayOfYear >= endField.Value.WebParseDateTime().DayOfYear)) ||
                              (startField.Value.WebParseDateTime().DayOfYear > dateTimeInterval.End.DayOfYear) ||
                              ((endField.Value.WebParseDateTime() - startField.Value.WebParseDateTime()).Days > 365));
            }

            Assert.IsTrue(information1.SpeciesObservationCount <=
                          information2.SpeciesObservationCount);

            // Test exact date with Begin < End and operator Excluding.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.Month < startField.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == startField.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= startField.Value.WebParseDateTime().Day)));
                Assert.IsTrue((dateTimeInterval.End.Month > startField.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == startField.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= startField.Value.WebParseDateTime().Day)));
                Assert.IsTrue((dateTimeInterval.Begin.Month < endField.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == endField.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= endField.Value.WebParseDateTime().Day)));
                Assert.IsTrue((dateTimeInterval.End.Month > endField.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == endField.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= endField.Value.WebParseDateTime().Day)));
            }

            // Test exact date with Begin < End and operator Including.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue((((dateTimeInterval.Begin.Month < startField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.Begin.Month == startField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.Begin.Day <= startField.Value.WebParseDateTime().Day))) &&

                               ((dateTimeInterval.End.Month > startField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.End.Month == startField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.End.Day >= startField.Value.WebParseDateTime().Day)))) ||

                              (((dateTimeInterval.Begin.Month < endField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.Begin.Month == endField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.Begin.Day <= endField.Value.WebParseDateTime().Day))) &&

                               ((dateTimeInterval.End.Month > endField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.End.Month == endField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.End.Day >= endField.Value.WebParseDateTime().Day)))) ||

                              (((dateTimeInterval.Begin.Month > startField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.Begin.Month == startField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.Begin.Day > startField.Value.WebParseDateTime().Day))) &&

                               ((dateTimeInterval.End.Month < endField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.End.Month == endField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.End.Day < endField.Value.WebParseDateTime().Day)))) ||

                              ((endField.Value.WebParseDateTime() - startField.Value.WebParseDateTime()).TotalDays > 365));
            }

            Assert.IsTrue(information1.SpeciesObservationCount <=
                          information2.SpeciesObservationCount);

            //// Test exact date with End < Begin and operator Excluding.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 12, 28);
            dateTimeInterval.End = new DateTime(2001, 1, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue(((dateTimeInterval.Begin.Month < startField.Value.WebParseDateTime().Month) ||
                               ((dateTimeInterval.Begin.Month == startField.Value.WebParseDateTime().Month) &&
                                (dateTimeInterval.Begin.Day <= startField.Value.WebParseDateTime().Day))) ||
                              ((dateTimeInterval.End.Month > startField.Value.WebParseDateTime().Month) ||
                               ((dateTimeInterval.End.Month == startField.Value.WebParseDateTime().Month) &&
                                (dateTimeInterval.End.Day >= startField.Value.WebParseDateTime().Day))));
                Assert.IsTrue(((dateTimeInterval.Begin.Month < endField.Value.WebParseDateTime().Month) ||
                               ((dateTimeInterval.Begin.Month == endField.Value.WebParseDateTime().Month) &&
                                (dateTimeInterval.Begin.Day <= endField.Value.WebParseDateTime().Day))) ||
                              ((dateTimeInterval.End.Month > endField.Value.WebParseDateTime().Month) ||
                               ((dateTimeInterval.End.Month == endField.Value.WebParseDateTime().Month) &&
                                (dateTimeInterval.End.Day >= endField.Value.WebParseDateTime().Day))));
            }

            // Test exact date with End < Begin and operator Including.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 12, 28);
            dateTimeInterval.End = new DateTime(2001, 1, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                endField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.End.ToString());
                startField = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsTrue((((dateTimeInterval.Begin.Month < startField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.Begin.Month == startField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.Begin.Day <= startField.Value.WebParseDateTime().Day))) ||

                               ((dateTimeInterval.End.Month > startField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.End.Month == startField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.End.Day >= startField.Value.WebParseDateTime().Day)))) ||

                              (((dateTimeInterval.Begin.Month < endField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.Begin.Month == endField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.Begin.Day <= endField.Value.WebParseDateTime().Day))) ||

                               ((dateTimeInterval.End.Month > endField.Value.WebParseDateTime().Month) ||
                                ((dateTimeInterval.End.Month == endField.Value.WebParseDateTime().Month) &&
                                 (dateTimeInterval.End.Day >= endField.Value.WebParseDateTime().Day)))) ||

                              ((endField.Value.WebParseDateTime().Month < startField.Value.WebParseDateTime().Month) ||
                               ((endField.Value.WebParseDateTime().Month == startField.Value.WebParseDateTime().Month) &&
                                (endField.Value.WebParseDateTime().Day < startField.Value.WebParseDateTime().Day))) ||

                              ((endField.Value.WebParseDateTime() - startField.Value.WebParseDateTime()).TotalDays > 365));
            }

            Assert.IsTrue(information1.SpeciesObservationCount <=
                          information2.SpeciesObservationCount);

            // Test with more than one interval.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Clear();
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 4, 1);
            dateTimeInterval.End = new DateTime(2000, 4, 2);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount <
                          information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaObserverSearchStringElasticsearch()
        {
            CultureInfo ci = new CultureInfo("sv-SE");
            WebSpeciesObservationField recordedByField;
            WebSpeciesObservationInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(102978); // större hackspett

            searchCriteria.ObserverSearchString = new WebStringSearchCriteria();
            searchCriteria.ObserverSearchString.SearchString = "Björn";
            searchCriteria.ObserverSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                Assert.IsTrue(recordedByField.Value.StartsWith(searchCriteria.ObserverSearchString.SearchString, true,
                    ci));

            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.ObserverSearchString.SearchString = "jörn";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                Assert.IsTrue(
                    recordedByField.Value.ToLower().Contains(searchCriteria.ObserverSearchString.SearchString.ToLower()));

            }

            searchCriteria.ObserverSearchString.SearchString = "Karlsson";
            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.EndsWith);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                Assert.IsTrue(recordedByField.Value.EndsWith(searchCriteria.ObserverSearchString.SearchString, true, ci));

            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.ObserverSearchString.SearchString = "Johan Södercrantz";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                Assert.IsTrue(
                    recordedByField.Value.ToLower().Equals(searchCriteria.ObserverSearchString.SearchString.ToLower()));

            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.Like);
            searchCriteria.ObserverSearchString.SearchString = "*a*b*c*";
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                Assert.IsTrue(recordedByField.Value.ToLower().IndexOf("a") > -1);
                Assert.IsTrue(recordedByField.Value.ToLower().IndexOf("b") > -1);
                Assert.IsTrue(recordedByField.Value.ToLower().IndexOf("c") > -1);

            }

            searchCriteria.ObserverSearchString.CompareOperators.Clear();
            searchCriteria.ObserverSearchString.CompareOperators.Add(StringCompareOperator.NotEqual);
            searchCriteria.ObserverSearchString.SearchString = "";
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                recordedByField = ListWeb.GetField(speciesObservation.Fields,
                    SpeciesObservationClassId.Occurrence.ToString(),
                    SpeciesObservationPropertyId.RecordedBy.ToString());
                if (recordedByField.IsNotNull())
                {
                    Assert.AreNotEqual(recordedByField.Value.ToLower(),
                        searchCriteria.ObserverSearchString.SearchString.ToLower());
                }
                // else Null is not equal to "".
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPageTaxonIdsElasticsearch()
        {
            Int64 index, speciesObservationCount;
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservationFieldSortOrder startSortOrder;
            WebSpeciesObservationPageSpecification pageSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            speciesObservationCount =
                SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context,
                    searchCriteria, GetCoordinateSystem());

            pageSpecification = new WebSpeciesObservationPageSpecification();
            pageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            startSortOrder = new WebSpeciesObservationFieldSortOrder();
            startSortOrder.Class = new WebSpeciesObservationClass();
            startSortOrder.Class.Id = SpeciesObservationClassId.Event;
            startSortOrder.Property = new WebSpeciesObservationProperty();
            startSortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            startSortOrder.SortOrder = SortOrder.Ascending;
            pageSpecification.SortOrder.Add(startSortOrder);
            pageSpecification.Size = 1000;

            for (index = 0; index < speciesObservationCount; index += pageSpecification.Size)
            {
                pageSpecification.Start = index + 1;
                speciesObservations =
                    SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPageElasticsearch(Context,
                        searchCriteria, GetCoordinateSystem(), pageSpecification, null);
                Assert.IsTrue(speciesObservations.IsNotEmpty());
                Assert.IsTrue(speciesObservations.Count <= pageSpecification.Size);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaPolygonsElasticsearch()
        {
            WebSpeciesObservationInformation information1, information2;
            WebLinearRing linearRing;
            WebPolygon polygon;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

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
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);

            // Test adding same polygon twice.
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygon);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
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
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
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
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaRegionGuidsElasticsearch()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);

            // Test adding another region.
            searchCriteria.DataFields = null;
            searchCriteria.Polygons = null;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.RegionGuids.Add(ProvinceGuid.Skane);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information1.SpeciesObservations.Count < information2.SpeciesObservations.Count);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaReportedDateTimeElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            // Test Begin and End.
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2015, 7, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2015, 8, 1);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= field.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= field.Value.WebParseDateTime());
            }

            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2015, 6, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2015, 9, 1);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.Modified.ToString());
                Assert.IsTrue(searchCriteria.ReportedDateTime.Begin <= field.Value.WebParseDateTime());
                Assert.IsTrue(searchCriteria.ReportedDateTime.End >= field.Value.WebParseDateTime());
            }

            Assert.IsTrue(information2.SpeciesObservationCount > information1.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaReportedDateTimeIntervalElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;
            WebDateTimeInterval dateTimeInterval;
            WebSpeciesObservationField field;
            WebSpeciesObservationInformation information1, information2;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Mallard));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2000, 1, 1);
            searchCriteria.ReportedDateTime.End = new DateTime(2012, 12, 31);
            searchCriteria.ReportedDateTime.PartOfYear = new List<WebDateTimeInterval>();

            // Test day of year with Begin < End.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ReportedDateTime.PartOfYear.Clear();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            //// Test day of year with End < Begin.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 12, 30);
            dateTimeInterval.End = new DateTime(2001, 1, 1);
            searchCriteria.ReportedDateTime.PartOfYear.Clear();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            // Test exact date with Begin < End.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ReportedDateTime.PartOfYear.Clear();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.Month < field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= field.Value.WebParseDateTime().Day)));
                Assert.IsTrue((dateTimeInterval.End.Month > field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= field.Value.WebParseDateTime().Day)));
            }

            // Test exact date with End < Begin.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = false;
            dateTimeInterval.Begin = new DateTime(2000, 12, 30);
            dateTimeInterval.End = new DateTime(2001, 1, 1);
            searchCriteria.ReportedDateTime.PartOfYear.Clear();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.Month < field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.Begin.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.Begin.Day <= field.Value.WebParseDateTime().Day)) ||
                              (dateTimeInterval.End.Month > field.Value.WebParseDateTime().Month) ||
                              ((dateTimeInterval.End.Month == field.Value.WebParseDateTime().Month) &&
                               (dateTimeInterval.End.Day >= field.Value.WebParseDateTime().Day)));
            }

            // Test with more than one interval.
            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 2, 25);
            dateTimeInterval.End = new DateTime(2000, 3, 3);
            searchCriteria.ReportedDateTime.PartOfYear.Clear();
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information1);
            foreach (WebSpeciesObservation speciesObservation in information1.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            dateTimeInterval = new WebDateTimeInterval();
            dateTimeInterval.IsDayOfYearSpecified = true;
            dateTimeInterval.Begin = new DateTime(2000, 1, 6);
            dateTimeInterval.End = new DateTime(2000, 1, 7);
            searchCriteria.ReportedDateTime.PartOfYear.Add(dateTimeInterval);
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, coordinateSystem, null, null);
            CheckSpeciesObservationInformation(information2);
            foreach (WebSpeciesObservation speciesObservation in information2.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.DarwinCore.ToString(),
                    SpeciesObservationPropertyId.ReportedDate.ToString());
                Assert.IsTrue((dateTimeInterval.Begin.DayOfYear <= field.Value.WebParseDateTime().DayOfYear) ||
                              (dateTimeInterval.End.DayOfYear >= field.Value.WebParseDateTime().DayOfYear));
            }

            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaSortOrderElasticsearch()
        {
            DateTime previouseStart;
            Int32 dyntaxaTaxonId;
            List<WebSpeciesObservationFieldSortOrder> sortOrders;
            String locality;
            WebSpeciesObservationField field;
            WebSpeciesObservationFieldSortOrder sortOrder;
            WebSpeciesObservationInformation information;
            WebSpeciesObservationSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));
//            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
            searchCriteria.IncludePositiveObservations = true;
            sortOrders = new List<WebSpeciesObservationFieldSortOrder>();

            // Test default sort order.
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, sortOrders);
            CheckSpeciesObservationInformation(information);
            previouseStart = DateTime.Now;
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(previouseStart >= field.Value.WebParseDateTime());
                previouseStart = field.Value.WebParseDateTime();
            }

            // Sort on observation start ascending.
            sortOrder = new WebSpeciesObservationFieldSortOrder();
            sortOrder.Class = new WebSpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Event;
            sortOrder.Property = new WebSpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.Start;
            sortOrder.SortOrder = SortOrder.Ascending;
            sortOrders.Clear();
            sortOrders.Add(sortOrder);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, sortOrders);
            CheckSpeciesObservationInformation(information);
            previouseStart = new DateTime(1755, 1, 1);
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Event.ToString(),
                    SpeciesObservationPropertyId.Start.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(previouseStart <= field.Value.WebParseDateTime());
                previouseStart = field.Value.WebParseDateTime();
            }

            // Sort on observation Dyntaxa taxon id ascending.
            sortOrder = new WebSpeciesObservationFieldSortOrder();
            sortOrder.Class = new WebSpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Taxon;
            sortOrder.Property = new WebSpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.DyntaxaTaxonID;
            sortOrder.SortOrder = SortOrder.Ascending;
            sortOrders.Clear();
            sortOrders.Add(sortOrder);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, sortOrders);
            CheckSpeciesObservationInformation(information);
            dyntaxaTaxonId = -1;
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Taxon.ToString(),
                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                Assert.IsNotNull(field);
                Assert.IsTrue(dyntaxaTaxonId <= field.Value.WebParseInt32());
                dyntaxaTaxonId = field.Value.WebParseInt32();
            }

            // Sort on more than one field.
            sortOrder = new WebSpeciesObservationFieldSortOrder();
            sortOrder.Class = new WebSpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Location;
            sortOrder.Property = new WebSpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.Locality;
            sortOrder.SortOrder = SortOrder.Ascending;
            sortOrders.Clear();
            sortOrders.Add(sortOrder);
            sortOrder = new WebSpeciesObservationFieldSortOrder();
            sortOrder.Class = new WebSpeciesObservationClass();
            sortOrder.Class.Id = SpeciesObservationClassId.Event;
            sortOrder.Property = new WebSpeciesObservationProperty();
            sortOrder.Property.Id = SpeciesObservationPropertyId.End;
            sortOrder.SortOrder = SortOrder.Descending;
            sortOrders.Add(sortOrder);
            information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, sortOrders);
            CheckSpeciesObservationInformation(information);
            locality = null;
            foreach (WebSpeciesObservation speciesObservation in information.SpeciesObservations)
            {
                field = ListWeb.GetField(speciesObservation.Fields, SpeciesObservationClassId.Location.ToString(),
                    SpeciesObservationPropertyId.Locality.ToString());
                Assert.IsNotNull(field);
                if (locality.IsNull())
                {
                    locality = field.Value;
                }

//                Assert.IsTrue(String.Compare(locality, field.Value, false, new CultureInfo("sv-SE")) <= 0);
                locality = field.Value;
                Debug.WriteLine(locality);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaSpeciesObservationSpecificationElasticsearch()
        {
            List<WebSpeciesObservationFieldSpecification> fieldSpecifications;
            WebSpeciesObservationInformation speciesObservationInformation;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationSpecification speciesObservationSpecification;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 60;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            speciesObservationSpecification = new WebSpeciesObservationSpecification();
            foreach (
                SpeciesObservationSpecificationId speciesObservationSpecificationId in
                    Enum.GetValues(typeof (SpeciesObservationSpecificationId)))
            {
                speciesObservationSpecification.Specification = speciesObservationSpecificationId;
                fieldSpecifications = speciesObservationSpecification.GetFields();
                speciesObservationInformation =
                    SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                        searchCriteria, GetCoordinateSystem(), speciesObservationSpecification, null);
                CheckSpeciesObservationInformation(speciesObservationInformation);
                if (fieldSpecifications.IsNotEmpty())
                {
                    foreach (WebSpeciesObservationFieldSpecification fieldSpecification in fieldSpecifications)
                    {
                        foreach (
                            WebSpeciesObservation speciesObservation in
                                speciesObservationInformation.SpeciesObservations)
                        {
                            Assert.IsTrue(speciesObservation.Fields.Count <= fieldSpecifications.Count);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaTaxonIdsElasticsearch()
        {
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information1, information2;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 6, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 8, 30);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32) (TaxonId.DrumGrasshopper));

            information1 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information1);

            searchCriteria.TaxonIds.Add((Int32) (TaxonId.Grasshoppers));
            information2 = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context,
                searchCriteria, GetCoordinateSystem(), null, null);
            CheckSpeciesObservationInformation(information2);
            Assert.IsTrue(information1.SpeciesObservationCount < information2.SpeciesObservationCount);
        }

        [TestMethod]
        public void GetSpeciesObservationsBySearchCriteriaToManyObservationsErrorElasticsearch()
        {
            Boolean isExecptionThrown;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebSpeciesObservationInformation information;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            isExecptionThrown = false;
            try
            {
                information = SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(Context, searchCriteria, GetCoordinateSystem(), null, null);
                CheckSpeciesObservationInformation(information);
            }
            catch (Exception)
            {
                isExecptionThrown = true;
            }

            Assert.IsTrue(isExecptionThrown);
        }

        private void ShowSpeciesObservationsElasticsearch(List<Int64> speciesObservationIds)
        {
            Int32 index;
            Int64 id;
            StringBuilder filter;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationInformation speciesObservationInformation;
            WebSpeciesObservationSpecification speciesObservationSpecification;

            if (speciesObservationIds.IsNotEmpty())
            {
                // Check that data is valid.
                coordinateSystem = new WebCoordinateSystem();
                coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
                speciesObservationSpecification = null;
                if (speciesObservationIds.Count >
                    SwedishSpeciesObservationService.Settings.Default.MaxSpeciesObservationWithInformation)
                {
                    throw new ArgumentException("Too many species observations was retrieved!, Limit is set to " +
                                                SwedishSpeciesObservationService.Settings.Default
                                                    .MaxSpeciesObservationWithInformation +
                                                " observations with information.");
                }

                // Get species observation filter.
                filter = new StringBuilder();
                filter.Append("{");
                filter.Append(" \"size\": " + speciesObservationIds.Count);
                filter.Append(", " + SpeciesObservationManager.GetFields(Context, speciesObservationSpecification));
                filter.Append(", \"filter\": {\"bool\":{ \"must\" : [");
                filter.Append("{ \"terms\": {");
                filter.Append(" \"");
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    filter.Append(elastisearch.GetFieldName(SpeciesObservationClassId.DarwinCore,
                                                            SpeciesObservationPropertyId.Id));
                }
                filter.Append("\":[");
                filter.Append(speciesObservationIds[0].WebToString());
                for (index = 1; index < speciesObservationIds.Count; index++)
                {
                    filter.Append(", " + speciesObservationIds[index].WebToString());
                }

                filter.Append("]}}]}}}");

                speciesObservationInformation =
                    SpeciesObservationManager.GetSpeciesObservationInformationElasticsearch(Context,
                        null,
                        coordinateSystem,
                        filter.ToString(),
                        speciesObservationSpecification,
                        "ShowSpeciesObservationsElasticsearch");
                if (speciesObservationInformation.IsNotNull() &&
                    speciesObservationInformation.SpeciesObservations.IsNotEmpty())
                {
                    foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
                    {
                        if (speciesObservation.Fields.IsNotEmpty())
                        {
                            id =
                                speciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore,
                                    SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                            speciesObservationIds.Remove(id);
                            Debug.WriteLine("");
                            foreach (WebSpeciesObservationField field in speciesObservation.Fields)
                            {
                                Debug.WriteLine(field.ClassIdentifier + " " + field.PropertyIdentifier + " " + field.Value);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateSpeciesObservationsElasticsearch(List<Int64> speciesObservationIds)
        {
        }
    }
}
