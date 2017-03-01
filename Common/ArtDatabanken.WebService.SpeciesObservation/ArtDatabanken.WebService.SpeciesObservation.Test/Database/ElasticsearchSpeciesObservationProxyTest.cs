using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Database.ElasticsearchSerializingClasses;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservation.Test.Database
{
    [TestClass]
    public class ElasticsearchSpeciesObservationProxyTest : TestBase
    {
        private ElasticsearchSpeciesObservationProxy mElasticsearch;

        public ElasticsearchSpeciesObservationProxyTest()
        {
            mElasticsearch = null;
        }

        [TestMethod]
        [Ignore]
        public void CreateIndex()
        {
            ElasticsearchSpeciesObservationProxy elasticsearch;

            elasticsearch = new ElasticsearchSpeciesObservationProxy("swedish_species_observation" + "_2016_10_14");
            elasticsearch.CreateIndex();
        }

        [TestMethod]
        [Ignore]
        public void DeleteIndex()
        {
            ElasticsearchSpeciesObservationProxy elasticsearch;

            Configuration.InstallationType = InstallationType.LocalTest;

            elasticsearch = new ElasticsearchSpeciesObservationProxy("swedish_species_observation" + "_2016_01_26");
            elasticsearch.DeleteIndex();
        }

        [TestMethod]
        [Ignore]
        public void DeleteType()
        {
            GetElasticsearch().DeleteType(GetElasticsearch().SpeciesObservationType);
        }

        [TestMethod]
        public void GetSpeciesObservationCount()
        {
            Int64 speciesObservationCount;

            speciesObservationCount = GetElasticsearch().GetSpeciesObservationCount(null).DocumentCount;
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Species observation count = " + speciesObservationCount);
        }

        private ElasticsearchSpeciesObservationProxy GetElasticsearch()
        {
            if (mElasticsearch.IsNull())
            {
                mElasticsearch = new ElasticsearchSpeciesObservationProxy("swedish_species_observation");
            }

            return mElasticsearch;
        }

        [TestMethod]
        public void GetIndexAlias()
        {
            Dictionary<String, String> aliases;

            aliases = GetElasticsearch().GetIndexAliases();
            Assert.IsTrue(aliases.IsNotEmpty());
        }


        [TestMethod]
        public void GetHealth()
        {
            ElasticsearchHealth elasticsearchHealth;

            elasticsearchHealth = GetElasticsearch().GetHealth();
            Assert.IsNotNull(elasticsearchHealth);
            Assert.IsTrue(elasticsearchHealth.IsOk());
        }

        [TestMethod]
        public void GetScroll()
        {
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchScroll scroll;

            scroll = new ElasticsearchScroll();
            scroll.KeepAlive = 1;
            GetElasticsearch().StartScroll(null, scroll);
            Assert.IsTrue(scroll.ScrollId.IsNotEmpty());
            documentFilterResponse = GetElasticsearch().GetScroll(scroll);
            Assert.IsTrue(scroll.ScrollId.IsNotEmpty());
            Assert.IsNotNull(documentFilterResponse);
            Assert.IsTrue(documentFilterResponse.DocumentsJson.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationMapping()
        {
            FieldDefinitionList fieldDefinitions;

            fieldDefinitions = GetElasticsearch().GetSpeciesObservationMapping();
            Assert.IsTrue(fieldDefinitions.IsNotEmpty());
            foreach (FieldDefinition fieldDefinition in fieldDefinitions)
            {
                switch (fieldDefinition.DataType)
                {
                    case "date":
                        Debug.WriteLine(fieldDefinition.Name + ", " +
                                        fieldDefinition.DataType + ", " +
                                        fieldDefinition.Format);
                        break;
                    case "geo_shape":
                        Debug.WriteLine(fieldDefinition.Name + ", " +
                                        fieldDefinition.DataType + ", " +
                                        fieldDefinition.TreeLevel);
                        break;
                    case "string":
                        Debug.WriteLine(fieldDefinition.Name + ", " +
                                        fieldDefinition.DataType + ", " +
                                        fieldDefinition.FieldIndex);
                        break;
                    default:
                        Debug.WriteLine(fieldDefinition.Name + ", " +
                                        fieldDefinition.DataType);
                        break;
                }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationsByScroll()
        {
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchScroll scroll;

            scroll = new ElasticsearchScroll();
            scroll.KeepAlive = 1;
            for (Int32 index = 0; index < 5; index++)
            {
                documentFilterResponse = GetElasticsearch().GetSpeciesObservations(null, scroll);
                Assert.IsTrue(scroll.ScrollId.IsNotEmpty());
                Assert.IsNotNull(documentFilterResponse);
                Assert.IsTrue(documentFilterResponse.DocumentsJson.IsNotEmpty());
            }
        }

        [TestMethod]
        public void IsClusterOk()
        {
            Assert.IsTrue(GetElasticsearch().IsClusterOk());
        }

        [TestMethod]
        public void StartScroll()
        {
            ElasticsearchScroll scroll;

            scroll = new ElasticsearchScroll();
            scroll.KeepAlive = 1;
            GetElasticsearch().StartScroll(null, scroll);
            Assert.IsTrue(scroll.ScrollId.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void UpdateIndexAlias()
        {
            Dictionary<String, String> aliases;

            // swedish_species_observation_2015_12_09
            // swedish_species_observation_2016_01_26
            // swedish_species_observation_2016_10_14

            Configuration.InstallationType = InstallationType.LocalTest;

            GetElasticsearch().UpdateIndexAlias("swedish_species_observation", null, "swedish_species_observation_2016_10_14");
            aliases = GetElasticsearch().GetIndexAliases();
            Assert.IsTrue(aliases.IsNotEmpty());
        }
    }
}
