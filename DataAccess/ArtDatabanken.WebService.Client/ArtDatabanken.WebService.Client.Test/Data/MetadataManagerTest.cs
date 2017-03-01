using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IPictureDataSource = ArtDatabanken.Data.DataSource.IPictureDataSource;
using ISpeciesObservationDataSource = ArtDatabanken.Data.DataSource.ISpeciesObservationDataSource;
using PictureDataSource = ArtDatabanken.WebService.Client.PictureService.PictureDataSource;
using SpeciesObservationDataSource = ArtDatabanken.WebService.Client.SpeciesObservationService.SpeciesObservationDataSource;

namespace ArtDatabanken.WebService.Client.Test.Data
{

    [TestClass]
    public class MetadataManagerTest : TestBase
    {
        private MetadataManager _metadataManager;

        public MetadataManagerTest()
        {
            _metadataManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            MetadataManager metadataManager;

            metadataManager = new MetadataManager();
            Assert.IsNotNull(metadataManager);
        }

        [TestMethod]
        public void DataSource()
        {
            ISpeciesObservationDataSource dataSource;

            dataSource = null;
            GetMetadataManager(true).SpeciesObservationDataSource = dataSource;
            Assert.AreEqual(dataSource, GetMetadataManager().SpeciesObservationDataSource);

            dataSource = new SpeciesObservationDataSource();
            GetMetadataManager().SpeciesObservationDataSource = dataSource;
            Assert.AreEqual(dataSource, GetMetadataManager().SpeciesObservationDataSource);
        }

        [TestMethod]
        public void PictureDataSource()
        {
            IPictureDataSource dataSource = null;

            GetMetadataManager(true).PictureDataSource = dataSource;
            Assert.AreEqual(dataSource, GetMetadataManager().PictureDataSource);

            dataSource = new PictureDataSource();
            GetMetadataManager().PictureDataSource = dataSource;
            Assert.AreEqual(dataSource, GetMetadataManager().PictureDataSource);
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetMetadataManager(true).GetDataSourceInformation());
        }

        [TestMethod]
        public void GetPictureDataSourceInformation()
        {
            Assert.IsNotNull(GetMetadataManager(true).GetPictureDataSourceInformation());
        }

        private MetadataManager GetMetadataManager()
        {
            return GetMetadataManager(false);
        }

        private MetadataManager GetMetadataManager(Boolean refresh)
        {
            if (_metadataManager.IsNull() || refresh)
            {
                _metadataManager = new MetadataManager();
                _metadataManager.SpeciesObservationDataSource = new SpeciesObservationDataSource();
                _metadataManager.PictureDataSource = new PictureDataSource();
            }
            return _metadataManager;
        }

        [TestMethod]
        public void GetSpeciesObservationFieldDescriptions()
        {
            SpeciesObservationFieldDescriptionList fieldDescriptionsMappings, list;
            list = GetMetadataManager(true).GetSpeciesObservationFieldDescriptions(GetUserContext());
            Assert.IsTrue(list.Count > 85);

            Int32 previousSortValue = -1;
            foreach (ISpeciesObservationFieldDescription field in list)
            {
                //Test that all mandatory properties has value for every item.
                Assert.IsTrue(field.Class.GetName().Length > 1);
                //Assert.IsTrue(field.Definition.Length > 1);
                //Assert.IsTrue(field.DocumentationUrl.Length > 1);
                Assert.IsTrue(field.Guid.Length > 1);
                Assert.IsNotNull(field.Id);
                Assert.IsNotNull(field.Importance);
                Assert.IsNotNull(field.IsAcceptedByTdwg);
                Assert.IsNotNull(field.IsClass);
                Assert.IsNotNull(field.IsImplemented);
                Assert.IsNotNull(field.IsMandatory);
                Assert.IsNotNull(field.IsMandatoryFromProvider);
                Assert.IsNotNull(field.IsObtainedFromProvider);
                Assert.IsNotNull(field.IsPlanned);
                // Assert.IsTrue(field.Label.Length > 0);
                // Assert.IsTrue(field.Name.Length > 1);
                Assert.IsNotNull(field.Type);
                

                //Test conditional properties
                if (field.IsAcceptedByTdwg)
                {
                    Assert.IsTrue(field.DefinitionUrl.Length > 0);
                }

                //Test that SortOrder is correct. 
                Assert.IsTrue(field.SortOrder > previousSortValue);
                previousSortValue = field.SortOrder;
            }

            fieldDescriptionsMappings = new SpeciesObservationFieldDescriptionList();
            foreach (ISpeciesObservationFieldDescription fieldDescription in list)
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
        public void GetPictureMetaDataDescriptions()
        {
            PictureMetaDataDescriptionList pictureMetaDataDescriptions;

            GetUserContext().Locale = CoreData.LocaleManager.GetLocale(GetUserContext(), LocaleId.en_GB); // English
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext());
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = GetMetadataManager(false).GetPictureMetaDataDescriptions(GetUserContext());
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());

            GetUserContext().Locale = CoreData.LocaleManager.GetLocale(GetUserContext(), LocaleId.sv_SE); // Swedish
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext());
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = GetMetadataManager(false).GetPictureMetaDataDescriptions(GetUserContext());
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataDescriptionsByIds()
        {
            List<Int32> metaDataIds = new List<int> { 2, 3 };
            PictureMetaDataDescriptionList pictureMetaDataDescriptions;

            GetUserContext().Locale = CoreData.LocaleManager.GetLocale(GetUserContext(), LocaleId.en_GB); // English
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext(), metaDataIds);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = GetMetadataManager(false).GetPictureMetaDataDescriptions(GetUserContext(), metaDataIds);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());

            GetUserContext().Locale = CoreData.LocaleManager.GetLocale(GetUserContext(), LocaleId.sv_SE); // Swedish
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext(), metaDataIds);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = GetMetadataManager(false).GetPictureMetaDataDescriptions(GetUserContext(), metaDataIds);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }
    }
}
