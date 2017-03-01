using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.PictureService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    using System.Linq;

    [TestClass]
    public class PictureManagerTest : TestBase
    {
        private PictureManager _pictureManager;
        private MetadataManager _metadataManager;

        public PictureManagerTest()
        {
            _pictureManager = null;
        }

        protected override string GetTestApplicationName()
        {
            return ApplicationIdentifier.PictureAdmin.ToString();
        }

        private PictureManager GetPictureManager(Boolean refresh = false)
        {
            if (_pictureManager.IsNull() || refresh)
            {
                _pictureManager = new PictureManager();
                _pictureManager.DataSource = new PictureDataSource();
            }

            return _pictureManager;
        }

        private MetadataManager GetMetadataManager(Boolean refresh = false)
        {
            if (_metadataManager.IsNull() || refresh)
            {
                _metadataManager = new MetadataManager();
                _metadataManager.PictureDataSource = new PictureDataSource();
            }

            return _metadataManager;
        }

        [TestMethod]
        public void GetPictureRelationDataType()
        {
            IPictureRelationDataType pictureRelationDataType;

            GetPictureManager(true);
            foreach (PictureRelationDataTypeIdentifier pictureRelationDataTypeIdentifier in Enum.GetValues(typeof(PictureRelationDataTypeIdentifier)))
            {
                using (ITransaction transaction = GetUserContext().StartTransaction())
                {
                    pictureRelationDataType = GetPictureManager().GetPictureRelationDataType(GetUserContext(), (Int32)pictureRelationDataTypeIdentifier);
                    Assert.IsTrue(pictureRelationDataType.IsNotNull());
                }
            }

            foreach (PictureRelationDataTypeIdentifier pictureRelationDataTypeIdentifier in Enum.GetValues(typeof(PictureRelationDataTypeIdentifier)))
            {
                pictureRelationDataType = GetPictureManager().GetPictureRelationDataType(GetUserContext(), (Int32)pictureRelationDataTypeIdentifier);
                Assert.IsTrue(pictureRelationDataType.IsNotNull());
            }
        }

        [TestMethod]
        public void GetPictureRelationDataTypes()
        {
            PictureRelationDataTypeList pictureRelationDataTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelationDataTypes = GetPictureManager(true).GetPictureRelationDataTypes(GetUserContext());
                Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());
            }

            pictureRelationDataTypes = GetPictureManager().GetPictureRelationDataTypes(GetUserContext());
            Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelations()
        {
            IFactor factor;
            List<Int32> speciesFactIds;
            IPictureRelationType pictureRelationType;
            ITaxon taxon;
            PictureRelationList pictureRelations;
            SpeciesFactList speciesFacts;

            // TODO: Change test when real data is available in database.
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), 743);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelations = GetPictureManager(true).GetPictureRelations(GetUserContext(), factor);
                Assert.IsTrue(pictureRelations.IsEmpty());
            }

            pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), factor);
            Assert.IsTrue(pictureRelations.IsEmpty());

            speciesFactIds = new List<Int32>();
            speciesFactIds.Add(1);
            speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(GetUserContext(), speciesFactIds);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), speciesFacts[0]);
                Assert.IsTrue(pictureRelations.IsEmpty());
            }

            pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), speciesFacts[0]);
            Assert.IsTrue(pictureRelations.IsEmpty());

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 233621);
            pictureRelationType = CoreData.PictureManager.GetPictureRelationType(GetUserContext(), 3);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), taxon, pictureRelationType);
                Assert.IsTrue(pictureRelations.IsNotEmpty());
            }

            pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), taxon, pictureRelationType);
            Assert.IsTrue(pictureRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelationsByPictureId()
        {
            Int64 pictureId;
            PictureRelationList pictureRelations;

            pictureId = 2;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelations = GetPictureManager(true).GetPictureRelations(GetUserContext(), pictureId);
                Assert.IsTrue(pictureRelations.IsNotEmpty());
            }

            pictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), pictureId);
            Assert.IsTrue(pictureRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelationTypeById()
        {
            IPictureRelationType pictureRelationType;
            PictureRelationTypeList pictureRelationTypes;

            pictureRelationTypes = GetPictureManager(true).GetPictureRelationTypes(GetUserContext());
            foreach (IPictureRelationType tempPictureRelationType in pictureRelationTypes)
            {
                pictureRelationType = GetPictureManager().GetPictureRelationType(GetUserContext(), tempPictureRelationType.Id);
                Assert.IsTrue(pictureRelationType.IsNotNull());
                Assert.AreEqual(tempPictureRelationType.Id, pictureRelationType.Id);
            }
        }

        [TestMethod]
        public void GetPictureRelationTypeByIdentifier()
        {
            IPictureRelationType pictureRelationType;
            PictureRelationTypeIdentifier identifier;

            foreach (IPictureRelationType tempPictureRelationType in GetPictureManager(true).GetPictureRelationTypes(GetUserContext()))
            {
                identifier = (PictureRelationTypeIdentifier)(Enum.Parse(typeof(PictureRelationTypeIdentifier),
                                                                               tempPictureRelationType.Identifier));
                pictureRelationType = GetPictureManager().GetPictureRelationType(GetUserContext(), identifier);
                Assert.IsNotNull(pictureRelationType);
                Assert.AreEqual(tempPictureRelationType.Identifier, pictureRelationType.Identifier);
            }

            foreach (PictureRelationTypeIdentifier pictureRelationTypeIdentifier in Enum.GetValues(typeof(PictureRelationTypeIdentifier)))
            {
                pictureRelationType = GetPictureManager().GetPictureRelationType(GetUserContext(), pictureRelationTypeIdentifier);
                Assert.IsNotNull(pictureRelationType);
                Assert.AreEqual(pictureRelationTypeIdentifier.ToString(), pictureRelationType.Identifier);
            }
        }

        [TestMethod]
        public void GetPictureRelationTypes()
        {
            PictureRelationTypeList pictureRelationTypes;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureRelationTypes = GetPictureManager(true).GetPictureRelationTypes(GetUserContext());
                Assert.IsTrue(pictureRelationTypes.IsNotEmpty());
            }

            pictureRelationTypes = GetPictureManager().GetPictureRelationTypes(GetUserContext());
            Assert.IsTrue(pictureRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureById()
        {
            Int64 pictureId = 2;
            IPicture picture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, null, null, 0, false, string.Empty);

            Assert.IsNotNull(picture);
            Assert.AreEqual(pictureId, picture.Id);
            Assert.IsNotNull(picture.Image);
            Assert.IsTrue(picture.Size == picture.OriginalSize);
        }

        [TestMethod]
        public void GetPictureById_Get100x100Thumbnail()
        {
            Int64 pictureId = 2;
            IPicture picture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, 100, 100, 0, true, string.Empty);

            Assert.IsNotNull(picture);
            Assert.AreEqual(pictureId, picture.Id);
            Assert.IsNotNull(picture.Image);
            Assert.IsTrue(picture.Size < picture.OriginalSize);
        }

        [TestMethod]
        public void GetPictureById_Resized640x480()
        {
            Int64 pictureId = 2;
            IPicture picture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, 480, 640, 0, true, string.Empty);

            Assert.IsNotNull(picture);
            Assert.AreEqual(pictureId, picture.Id);
            Assert.IsNotNull(picture.Image);
            Assert.IsTrue(picture.Size != picture.OriginalSize);
        }

        [TestMethod]
        public void GetPicturesByIds()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            PictureList pictures = GetPictureManager(true).GetPictures(GetUserContext(), pictureIds, null, null, 0, false, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (IPicture picture in pictures)
            {
                Assert.IsNotNull(picture.Image);
                Assert.IsTrue(picture.Size == picture.OriginalSize);
            }
        }

        [TestMethod]
        public void GetPicturesByIds_Get100x100Thumbnail()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            PictureList pictures = GetPictureManager(true).GetPictures(GetUserContext(), pictureIds, 100, 100, 0, true, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (IPicture picture in pictures)
            {
                Assert.IsNotNull(picture.Image);
                Assert.IsTrue(picture.Size < picture.OriginalSize);
            }
        }

        [TestMethod]
        public void GetPicturesByIds_Resized640x480()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            PictureList pictures = GetPictureManager(true).GetPictures(GetUserContext(), pictureIds, 480, 640, 0, true, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (IPicture picture in pictures)
            {
                Assert.IsNotNull(picture.Image);
                Assert.IsTrue(picture.Size != picture.OriginalSize);
            }
        }



        [TestMethod]
        public void GetAllRecommendedPictureIds()
        {

            IUserContext userContext = GetUserContext();
            PictureRelationTypeList relationTypes = GetPictureManager(true).GetPictureRelationTypes(userContext);
            IPictureRelationType pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.Factor);


            PictureGuidList guids = GetPictureManager(true).GetAllRecommendedPictureIds(userContext, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.SpeciesFact);
            guids = GetPictureManager(true).GetAllRecommendedPictureIds(userContext, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.TaxonRedList);
            guids = GetPictureManager(true).GetAllRecommendedPictureIds(userContext, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);
        }



        [TestMethod]
        public void GetPictureMetaDataById_With_Negative_PictureId_And_Null_MetaDataList()
        {
            long pictureId = -33;
            List<int> metaData = null;
            PictureMetaDataList pictureMetaDataAttributes = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, metaData);

            Assert.IsNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_Null_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = null;
            PictureMetaDataList pictureMetaDataAttributes = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_Empty_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int>();
            PictureMetaDataList pictureMetaDataAttributes = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int> { 2, 3 };
            PictureMetaDataList pictureMetaDataAttributes = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
            Assert.AreEqual(pictureMetaDataAttributes.Count, metaData.Count);
        }

        [TestMethod]
        public void GetPictureMetaDataById_MetaData_Not_Saved_on_db()
        {
            long pictureId = 33;
            PictureMetaDataList pictureMetaDataAttributes = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, null);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GetPictureInformationById_With_Null_MetaDataList()
        {
            List<int> metaData = null;
            IPictureInformation pictureInformation = GetPictureManager(true).GetPictureInformation(GetUserContext(), 0, null, null, metaData);
            PictureMetaDataDescriptionList pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext());

            Assert.IsNull(pictureInformation);
            Assert.IsNotNull(pictureMetaDataDescriptions);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }

        [TestMethod]
        public void GetPicturesInformationBySearchCriteria_With_Empty_MetaDataList()
        {
            List<IPictureInformation> pictureInformations;
            IPicturesSearchCriteria searchCriteria;
            List<int> metaData = new List<int>();

            // Test factors.
            searchCriteria = new PicturesSearchCriteria { Factors = new FactorList { CoreData.FactorManager.GetFactor(GetUserContext(), 2551) } };
            pictureInformations = GetPictureManager(true).GetPicturesInformation(GetUserContext(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());

            // Test taxa.
            searchCriteria = new PicturesSearchCriteria { Taxa = new TaxonList { CoreData.TaxonManager.GetTaxon(GetUserContext(), 233621) } };
            pictureInformations = GetPictureManager().GetPicturesInformation(GetUserContext(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());

            // Test factors and taxa.
            // Test species fact identifiers.
            searchCriteria = new PicturesSearchCriteria
            {
                Factors = new FactorList { CoreData.FactorManager.GetFactor(GetUserContext(), 2577) },
                Taxa = new TaxonList { CoreData.TaxonManager.GetTaxon(GetUserContext(), 101932) }
            };
            pictureInformations = GetPictureManager().GetPicturesInformation(GetUserContext(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());
        }


        [TestMethod]
        [Ignore]
        public void GetPictureInformationById_With_Empty_MetaDataList()
        {
            List<int> metaData = new List<int>();
            IPictureInformation pictureInformation = GetPictureManager(true).GetPictureInformation(GetUserContext(), 0, null, null, metaData);
            PictureMetaDataDescriptionList pictureMetaDataDescriptions = GetMetadataManager(true).GetPictureMetaDataDescriptions(GetUserContext());

            Assert.IsNull(pictureInformation);
            Assert.IsNotNull(pictureMetaDataDescriptions);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GetPictureInformationById_With_MetaDataList()
        {
            List<int> metaData = new List<int> { 2, 13, 18, 19 };
            IPictureInformation pictureInformation = GetPictureManager(true).GetPictureInformation(GetUserContext(), 0, null, null, metaData);

            Assert.IsNull(pictureInformation);
        }

        [TestMethod]
        public void GetAllRecommendedPicturesMetaData()
        {
            int copyright = 1;
            int pictureCreatedBy = 2;
            int pictureCreatedDate = 3;
            int description = 4;
            IUserContext userContext = GetUserContext();

            List<int> metaDataIds = new List<int> { copyright };
            PictureRelationTypeList relationTypes = GetPictureManager(true).GetPictureRelationTypes(userContext);
            IPictureRelationType pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.TaxonRedList);

            PictureMetaDataInformationList resultList = GetPictureManager(true).GetAllRecommendedPicturesMetaData(userContext, pictureRelationType, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);

            // Check that only copyright data is read
            foreach (IPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (IPictureMetaData pictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(pictureMetaData.Name.Equals("Copyright"));
                    Assert.IsTrue(pictureMetaData.Value.IsNotEmpty());
                }
            }

            metaDataIds = new List<int> { pictureCreatedBy, pictureCreatedDate };
            relationTypes.Get(PictureRelationTypeIdentifier.SpeciesFact);
            resultList = GetPictureManager(true).GetAllRecommendedPicturesMetaData(userContext, pictureRelationType, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data 
            foreach (IPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (IPictureMetaData pictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(pictureMetaData.Name.IsNotEmpty());
                    Assert.IsTrue(pictureMetaData.Value.IsNotEmpty());
                }
            }

            metaDataIds = new List<int> { description };
            resultList = GetPictureManager(true).GetAllRecommendedPicturesMetaData(userContext, pictureRelationType, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data
            foreach (IPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (IPictureMetaData pictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(pictureMetaData.Name.Equals("Description"));
                    Assert.IsTrue(pictureMetaData.Value.IsNotEmpty());
                }
            }

            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.Factor);
            metaDataIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            resultList = GetPictureManager(true).GetAllRecommendedPicturesMetaData(userContext, pictureRelationType, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data 
            foreach (IPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (IPictureMetaData pictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(pictureMetaData.Name.IsNotEmpty());
                    Assert.IsTrue(pictureMetaData.Value.IsNotEmpty());
                }
            }
        }

        [TestMethod]
        public void GetPicturesBySearchCriteria()
        {
            PictureList pictures;
            IPicturesSearchCriteria searchCriteria;

            // Test factors.
            searchCriteria = new PicturesSearchCriteria { Factors = new FactorList { CoreData.FactorManager.GetFactor(GetUserContext(), 2551) } };
            pictures = GetPictureManager(true).GetPictures(GetUserContext(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test taxa.
            searchCriteria = new PicturesSearchCriteria { Taxa = new TaxonList { CoreData.TaxonManager.GetTaxon(GetUserContext(), 233621) } };
            pictures = GetPictureManager().GetPictures(GetUserContext(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test factors and taxa.
            // Test species fact identifiers.
            searchCriteria = new PicturesSearchCriteria
                                {
                                    Factors = new FactorList { CoreData.FactorManager.GetFactor(GetUserContext(), 2577) },
                                    Taxa = new TaxonList { CoreData.TaxonManager.GetTaxon(GetUserContext(), 101932) }
                                };
            pictures = GetPictureManager().GetPictures(GetUserContext(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test metadata (on db).
            searchCriteria = new PicturesSearchCriteria
            {
                MetaData = new PictureMetaDataList
                                             {
                                                 new PictureMetaData
                                                     {
                                                         Id = 1,
                                                         Value = "2014"
                                                     },
                                                 new PictureMetaData
                                                     {
                                                         Id = 2
                                                     }
                                             }
            };
            pictures = GetPictureManager().GetPictures(GetUserContext(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());
        }

        [TestMethod]
        public void GetRecommendedPictureIdsByObjectGuid()
        {
            IPictureRelationType pictureRelationType;
            List<string> pictureGuids = new List<string> { "100024", "100025", "100026", "2582", "Taxon=226672,Factor=2540,IndividualCategory=0,Host=0,Period=0" };
            // "2582" , Type 1;
            // "Taxon = 42, Factor = 2540, IndividualCategory = 0, Host = 0, Period = 0" , Type 2;
            // "100024", "100025", "100026" , Type 4;
            PictureRelationTypeList relationTypes = GetPictureManager(true).GetPictureRelationTypes(GetUserContext());
            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.Factor);

            PictureGuidList guids = GetPictureManager(true).GetRecommendedPictureIdsByObjectGuid(GetUserContext(), pictureGuids, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.SpeciesFact);
            guids = GetPictureManager(true).GetRecommendedPictureIdsByObjectGuid(GetUserContext(), pictureGuids, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationType = relationTypes.Get(PictureRelationTypeIdentifier.TaxonRedList);
            guids = GetPictureManager(true).GetRecommendedPictureIdsByObjectGuid(GetUserContext(), pictureGuids, pictureRelationType);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);
           
        }

        [TestMethod]
        public void CreateDeletePictureFilename()
        {
            Int64 pictureId = 33;
            PictureMetaDataList pictureMetaData = new PictureMetaDataList
                                                      {
                                                          new PictureMetaData
                                                              {
                                                                  Id = 1,
                                                                  Value = "(C) Test 2014"
                                                              },
                                                          new PictureMetaData
                                                              {
                                                                  Id = 2,
                                                                  Value = "Test Author"
                                                              },
                                                          new PictureMetaData
                                                              {
                                                                  Id = 3,
                                                                  Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                              }
                                                      };
            IPicture picture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, null, null, 0, false, string.Empty);
            String filename = @"\Temp\JN_Leiobunum-blackwalli_226679_Hane_new.jpg";
            Int32 affectedRows;
            DateTime lastModified = DateTime.Now;
            String updatedBy = GetUserContext().User.GetPerson(GetUserContext()).FullName;
            List<Int32> pictureMetaDataIds = new List<int> { 1, 2, 3 };
            PictureMetaDataList newPictureMetaData;
            IPictureResponse pictureResponse;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                pictureResponse = GetPictureManager().CreatePictureFilename(GetUserContext(), picture.Image, filename, lastModified, picture.VersionId + 1, updatedBy, pictureMetaData);
                Assert.AreEqual(pictureResponse.AffectedRows, 1);
                Assert.IsTrue(pictureResponse.Id > 0);
                newPictureMetaData = GetPictureManager().GetPictureMetaData(GetUserContext(), pictureResponse.Id, pictureMetaDataIds);
                Assert.IsNotNull(newPictureMetaData);
                Assert.IsTrue(newPictureMetaData.IsNotEmpty());
                Assert.AreEqual(pictureMetaData.Count, newPictureMetaData.Count);
                for (int i = 0; i < newPictureMetaData.Count; i++)
                {
                    Assert.AreEqual(pictureMetaData[i].Id, newPictureMetaData[i].Id);
                    Assert.AreEqual(pictureMetaData[i].Value, newPictureMetaData[i].Value);
                }

                affectedRows = GetPictureManager().DeletePictureMetaData(GetUserContext(), pictureResponse.Id, pictureMetaData);
                Assert.AreEqual(affectedRows, pictureMetaData.Count);
                affectedRows = GetPictureManager().DeletePictureFilename(GetUserContext(), null, filename, picture.PictureStringId);
                Assert.IsTrue(0 < affectedRows);
            }
        }

        [TestMethod]
        public void CreateDeletePictureMetaData()
        {
            Int64 pictureId = 32;
            PictureMetaDataList pictureMetaData = new PictureMetaDataList
                                                      {
                                                          new PictureMetaData
                                                              {
                                                                  Id = 1,
                                                                  Value = "(C) Test 2014"
                                                              },
                                                          new PictureMetaData
                                                              {
                                                                  Id = 2,
                                                                  Value = "Test Author"
                                                              },
                                                          new PictureMetaData
                                                              {
                                                                  Id = 3,
                                                                  Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                              }
                                                      };
            PictureMetaDataList newPictureMetaData;
            Int32 affectedRows;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetPictureManager(true).CreatePictureMetaData(GetUserContext(), pictureId, pictureMetaData);
                newPictureMetaData = GetPictureManager().GetPictureMetaData(GetUserContext(), pictureId, null);
                Assert.AreEqual(newPictureMetaData.Count, pictureMetaData.Count);
                pictureMetaData[0].Value = string.Empty;
                pictureMetaData[1].Value = string.Empty;
                pictureMetaData[2].Value = string.Empty;
                affectedRows = GetPictureManager().DeletePictureMetaData(GetUserContext(), pictureId, pictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureMetaData.Count));
            }
        }

        [TestMethod]
        public void UpdatePictureMetaData()
        {
            Int64 pictureId = 33;
            PictureMetaDataList oldPictureMetaData = GetPictureManager(true).GetPictureMetaData(GetUserContext(), pictureId, null);
            PictureMetaDataList updatePictureMetaData = new PictureMetaDataList
                                                            {
                                                                new PictureMetaData
                                                                    {
                                                                        Id = 1,
                                                                        Value = "Test (C)"
                                                                    },
                                                                new PictureMetaData
                                                                    {
                                                                        Id = 2,
                                                                        Value = "Test Author"
                                                                    }
                                                            };
            String updatedBy = GetUserContext().User.GetPerson(GetUserContext()).FullName;
            Int32 affectedRows;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                affectedRows = GetPictureManager().UpdatePictureMetaData(GetUserContext(), pictureId, updatedBy, updatePictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == updatePictureMetaData.Count + 1)); // + 1: updatedBy.
                affectedRows = GetPictureManager().UpdatePictureMetaData(GetUserContext(), pictureId, updatedBy, oldPictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == oldPictureMetaData.Count + 1)); // + 1: updatedBy
            }
        }

        [TestMethod]
        public void CreatePictureRelations()
        {
            Boolean isRecommended = false;
            String objectGuid = "226679";
            Int64 pictureId = 33;
            Int32 typeId = 3;
            PictureRelationList pictureRelations = new PictureRelationList
                                                       {
                                                           new PictureRelation
                                                               {
                                                                   IsRecommended = isRecommended,
                                                                   ObjectGuid = objectGuid,
                                                                   PictureId = pictureId,
                                                                   SortOrder = 2,
                                                                   Type = CoreData.PictureManager.GetPictureRelationType(GetUserContext(), typeId)
                                                               },
                                                           new PictureRelation
                                                               {
                                                                   IsRecommended = isRecommended,
                                                                   ObjectGuid = objectGuid,
                                                                   PictureId = pictureId + 1,
                                                                   SortOrder = 3,
                                                                   Type = CoreData.PictureManager.GetPictureRelationType(GetUserContext(), typeId)
                                                               }
                                                       };
            PictureRelationList newPictureRelations;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetPictureManager(true).CreatePictureRelations(GetUserContext(),
                                                               pictureRelations);
                newPictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(),
                                                                              pictureId);
                Assert.IsTrue(0 < newPictureRelations.Count);
                newPictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(),
                                                                              pictureId + 1);
                Assert.IsTrue(0 < newPictureRelations.Count);
            }
        }

        [TestMethod]
        public void UpdatePictureRelations()
        {
            Boolean pictureRelationFound;
            IPictureRelation pictureRelation = new PictureRelation
                                                   {
                                                       Id = 1,
                                                       IsRecommended = false,
                                                       ObjectGuid = "1",
                                                       PictureId = 1,
                                                       SortOrder = 2,
                                                       Type = CoreData.PictureManager.GetPictureRelationType(GetUserContext(), 3)
                                                   };
            PictureRelationList pictureRelations = new PictureRelationList { pictureRelation };
            Int32 affectedRows;
            PictureRelationList updatedPictureRelations;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                affectedRows = GetPictureManager(true).UpdatePictureRelations(GetUserContext(),
                                                                                 pictureRelations);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureRelations.Count));
                updatedPictureRelations = this.GetPictureManager().GetPictureRelations(GetUserContext(),
                                                                                     pictureRelation.PictureId);
                Assert.IsNotNull(updatedPictureRelations);
                Assert.IsTrue(updatedPictureRelations.IsNotEmpty());

                pictureRelationFound = false;

                foreach (IPictureRelation updatedPictureRelation in updatedPictureRelations)
                {
                    if (pictureRelation.Id == updatedPictureRelation.Id)
                    {
                        Assert.AreEqual(pictureRelation.Id, updatedPictureRelation.Id);
                        Assert.AreEqual(pictureRelation.IsRecommended, updatedPictureRelation.IsRecommended);
                        Assert.AreEqual(pictureRelation.ObjectGuid, updatedPictureRelation.ObjectGuid);
                        Assert.AreEqual(pictureRelation.PictureId, updatedPictureRelation.PictureId);
                        Assert.AreEqual(pictureRelation.SortOrder, updatedPictureRelation.SortOrder);
                        Assert.AreEqual(pictureRelation.Type.Id, updatedPictureRelation.Type.Id);
                        pictureRelationFound = true;
                    }
                }

                Assert.IsTrue(pictureRelationFound);
            }
        }

        [TestMethod]
        public void DeletePictureRelations()
        {
            List<Int64> pictureRelationIds = new List<Int64> { 1, 16 };
            Int32 affectedRows;
            PictureRelationList deletedPictureRelations;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                affectedRows = GetPictureManager(true).DeletePictureRelations(GetUserContext(),
                                                                                 pictureRelationIds);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureRelationIds.Count));
                deletedPictureRelations = GetPictureManager().GetPictureRelations(GetUserContext(), 1);
                Assert.IsTrue(deletedPictureRelations.IsEmpty());
            }
        }

        [TestMethod]
        public void UpdatePictures()
        {
            Int64 pictureId = 33;
            IPicture oldPicture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, 50, 50, 0, false, string.Empty);
            PictureList updatePictures = new PictureList
                                             {
                                                 new Picture
                                                     {
                                                         Id = pictureId,
                                                         IsPublic = true,
                                                         IsArchived = true,
                                                         LastUpdated = DateTime.Now,
                                                         UpdatedBy = GetUserContext().User.GetPerson(GetUserContext()).FullName
                                                     }
                                             };
            String updatedBy = GetUserContext().User.GetPerson(GetUserContext()).FullName;
            Int32 affectedRows;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                affectedRows = GetPictureManager().UpdatePictures(GetUserContext(), updatePictures, updatedBy);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == updatePictures.Count));

                IPicture updatedPicture = GetPictureManager().GetPicture(GetUserContext(), pictureId, 50, 50, 0, false, string.Empty);

                Assert.AreEqual(pictureId, updatedPicture.Id);
                Assert.AreEqual(updatePictures[0].IsPublic, updatedPicture.IsPublic);
                Assert.AreEqual(updatePictures[0].IsArchived, updatedPicture.IsArchived);
                Assert.IsTrue(updatePictures[0].LastUpdated <= updatedPicture.LastUpdated);
                Assert.AreEqual(updatePictures[0].UpdatedBy, updatedPicture.UpdatedBy);
                affectedRows = GetPictureManager().UpdatePictures(GetUserContext(), new PictureList { oldPicture }, updatedBy);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == 1));
            }
        }

        [TestMethod]
        public void UpdatePicturesTaxon()
        {
            Int64 pictureId = 33;
            IPicture picture;
            String filename;
            Int32 affectedRows;
            DateTime lastModified;
            String updatedBy;
            PictureMetaDataList pictureMetaData;
            IPictureResponse pictureResponse;
            PictureList pictures;

            picture = GetPictureManager(true).GetPicture(GetUserContext(), pictureId, null, null, 0, false, string.Empty);
            filename = @"\Temp\JN_Leiobunum-blackwalli_226679_Hane_new.jpg";
            lastModified = DateTime.Now;
            updatedBy = GetUserContext().User.GetPerson(GetUserContext()).FullName;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                // Create picture.
                pictureResponse = GetPictureManager().CreatePictureFilename(GetUserContext(), picture.Image, filename, lastModified, picture.VersionId + 1, updatedBy, null);
                Assert.AreEqual(pictureResponse.AffectedRows, 1);
                Assert.IsTrue(pictureResponse.Id > 0);

                pictureMetaData = GetPictureManager().GetPictureMetaData(GetUserContext(), pictureResponse.Id, null);
                Assert.IsTrue(pictureMetaData.IsNotEmpty());
                picture = GetPictureManager().GetPicture(GetUserContext(),
                                                         pictureResponse.Id,
                                                         null,
                                                         null,
                                                         0,
                                                         false,
                                                         null);
                Assert.IsNotNull(picture);
                Assert.IsNull(picture.Taxon);

                // Set taxon.
                picture.Taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
                pictures = new PictureList();
                pictures.Add(picture);
                GetPictureManager().UpdatePictures(GetUserContext(), pictures, updatedBy);
                pictureMetaData = GetPictureManager().GetPictureMetaData(GetUserContext(), pictureResponse.Id, null);
                Assert.IsTrue(pictureMetaData.IsNotEmpty());
                picture = GetPictureManager().GetPicture(GetUserContext(),
                                                         pictureResponse.Id,
                                                         null,
                                                         null,
                                                         0,
                                                         false,
                                                         null);
                Assert.IsNotNull(picture);
                Assert.IsNotNull(picture.Taxon);
                Assert.AreEqual((Int32)(TaxonId.Bear), picture.Taxon.Id);

                // Remove taxon.
                picture.Taxon = null;
                pictures = new PictureList();
                pictures.Add(picture);
                GetPictureManager().UpdatePictures(GetUserContext(), pictures, updatedBy);
                pictureMetaData = GetPictureManager().GetPictureMetaData(GetUserContext(), pictureResponse.Id, null);
                Assert.IsTrue(pictureMetaData.IsNotEmpty());
                picture = GetPictureManager().GetPicture(GetUserContext(),
                                                         pictureResponse.Id,
                                                         null,
                                                         null,
                                                         0,
                                                         false,
                                                         null);
                Assert.IsNotNull(picture);
                Assert.IsNull(picture.Taxon);

                // Clean up.
                affectedRows = GetPictureManager().DeletePictureFilename(GetUserContext(), null, filename, picture.PictureStringId);
                Assert.IsTrue(0 < affectedRows);
            }
        }
    }
}