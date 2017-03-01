using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class PictureServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        private WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.PictureService.Logout(_clientInformation);
                _clientInformation = null;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                // Test is done.
                // We are not interested in problems that
                // occur due to test of error handling.
            }
        }

        [TestMethod]
        public void GetPictureRelationDataTypes()
        {
            List<WebPictureRelationDataType> pictureRelationDataTypes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                pictureRelationDataTypes = WebServiceProxy.PictureService.GetPictureRelationDataTypes(GetClientInformation());
                Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());
            }

            pictureRelationDataTypes = WebServiceProxy.PictureService.GetPictureRelationDataTypes(GetClientInformation());
            Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelationsByObjectGuid()
        {
            Int32 pictureRelationTypeId;
            List<WebPictureRelation> pictureRelations;
            String objectGuid;

            objectGuid = "2581";
            pictureRelationTypeId = 1;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                pictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByObjectGuid(GetClientInformation(), objectGuid, pictureRelationTypeId);
                Assert.IsTrue(pictureRelations.IsNotEmpty());
            }

            pictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByObjectGuid(GetClientInformation(), objectGuid, pictureRelationTypeId);
            Assert.IsTrue(pictureRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelationsByPictureId()
        {
            Int64 pictureId;
            List<WebPictureRelation> pictureRelations;

            pictureId = 2;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                pictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(), pictureId);
                Assert.IsTrue(pictureRelations.IsNotEmpty());
            }

            pictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(), pictureId);
            Assert.IsTrue(pictureRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureRelationTypes()
        {
            List<WebPictureRelationType> pictureRelationTypes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                pictureRelationTypes = WebServiceProxy.PictureService.GetPictureRelationTypes(GetClientInformation());
                Assert.IsTrue(pictureRelationTypes.IsNotEmpty());
            }

            pictureRelationTypes = WebServiceProxy.PictureService.GetPictureRelationTypes(GetClientInformation());
            Assert.IsTrue(pictureRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureById()
        {
            Int64 pictureId = 2;
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, null, null, 0, false, string.Empty);

            Assert.IsNotNull(webPicture);
            Assert.AreEqual(pictureId, webPicture.Id);
            Assert.IsNotNull(webPicture.Image);
            Assert.IsTrue(webPicture.Size == webPicture.OriginalSize);
        }

        [TestMethod]
        public void GetPictureById_Get100x100Thumbnail()
        {
            Int64 pictureId = 2;
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, 100, 100, 0, true, string.Empty);

            Assert.IsNotNull(webPicture);
            Assert.AreEqual(pictureId, webPicture.Id);
            Assert.IsNotNull(webPicture.Image);
            Assert.IsTrue(webPicture.Size < webPicture.OriginalSize);
        }

        [TestMethod]
        public void GetPictureById_Resized640x480()
        {
            Int64 pictureId = 2;
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, 480, 640, 0, true, string.Empty);

            Assert.IsNotNull(webPicture);
            Assert.AreEqual(pictureId, webPicture.Id);
            Assert.IsNotNull(webPicture.Image);
            Assert.IsTrue(webPicture.Size != webPicture.OriginalSize);
        }

        [TestMethod]
        public void GetPicturesByIds()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            List<WebPicture> pictures = WebServiceProxy.PictureService.GetPicturesByIds(GetClientInformation(), pictureIds, null, null, 0, false, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (WebPicture webPicture in pictures)
            {
                Assert.IsNotNull(webPicture.Image);
                Assert.IsTrue(webPicture.Size == webPicture.OriginalSize);
            }
        }

        [TestMethod]
        public void GetPicturesByIds_Get100x100Thumbnail()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            List<WebPicture> pictures = WebServiceProxy.PictureService.GetPicturesByIds(GetClientInformation(), pictureIds, 100, 100, 0, true, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (WebPicture webPicture in pictures)
            {
                Assert.IsNotNull(webPicture.Image);
                Assert.IsTrue(webPicture.Size < webPicture.OriginalSize);
            }
        }

        [TestMethod]
        public void GetPicturesByIds_Resized640x480()
        {
            List<Int64> pictureIds = new List<Int64> { 2, 5, 6, 7, 8, 9, 10, 11 };
            List<WebPicture> pictures = WebServiceProxy.PictureService.GetPicturesByIds(GetClientInformation(), pictureIds, 480, 640, 0, true, string.Empty);

            Assert.IsNotNull(pictures);
            Assert.AreEqual(pictureIds.Count, pictures.Count);
            foreach (WebPicture webPicture in pictures)
            {
                Assert.IsNotNull(webPicture.Image);
                Assert.IsTrue(webPicture.Size != webPicture.OriginalSize);
            }
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_Null_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = null;
            List<WebPictureMetaData> pictureMetaDataAttributes = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_Empty_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int>();
            List<WebPictureMetaData> pictureMetaDataAttributes = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataById_With_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int> { 2, 3 };
            List<WebPictureMetaData> pictureMetaDataAttributes = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, metaData);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
            Assert.AreEqual(pictureMetaDataAttributes.Count, metaData.Count);
        }

        [TestMethod]
        public void GetPictureMetaDataById_MetaData_Not_Saved_on_db()
        {
            long pictureId = 33;
            List<WebPictureMetaData> pictureMetaDataAttributes = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, null);

            Assert.IsNotNull(pictureMetaDataAttributes);
            Assert.IsTrue(pictureMetaDataAttributes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureInformationById_With_Null_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = null;
            WebPictureInformation pictureInformation = WebServiceProxy.PictureService.GetPictureInformationById(GetClientInformation(), pictureId, null, null, metaData);

            Assert.IsNotNull(pictureInformation);
            Assert.AreEqual(pictureId, pictureInformation.Id);
            Assert.IsNotNull(pictureInformation.Picture.Image);
            Assert.IsTrue(pictureInformation.Picture.Image.Length > 0);
            Assert.IsNotNull(pictureInformation.MetaData);
            Assert.IsTrue(pictureInformation.MetaData.IsNotEmpty());
            Assert.IsNotNull(pictureInformation.Relations);
        }

        [TestMethod]
        public void GetPicturesInformationBySearchCriteria_With_Empty_MetaDataList()
        {
            List<WebPictureInformation> pictureInformations;
            WebPicturesSearchCriteria searchCriteria;
            List<int> metaData = new List<int>();

            // Test factors.
            searchCriteria = new WebPicturesSearchCriteria { FactorIds = new List<int> { 2551 } };
            pictureInformations = WebServiceProxy.PictureService.GetPicturesInformationBySearchCriteria(GetClientInformation(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());

            // Test taxa.
            searchCriteria = new WebPicturesSearchCriteria { TaxonIds = new List<int> { 233621 } };
            pictureInformations = WebServiceProxy.PictureService.GetPicturesInformationBySearchCriteria(GetClientInformation(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());

            // Test species fact identifiers.
            searchCriteria = new WebPicturesSearchCriteria { SpeciesFactIdentifiers = new List<string> { "Taxon=226672,Factor=2540,IndividualCategory=0,Host=0,Period=0" } };
            pictureInformations = WebServiceProxy.PictureService.GetPicturesInformationBySearchCriteria(GetClientInformation(), searchCriteria, null, null, null, string.Empty, metaData);
            Assert.IsTrue(pictureInformations.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureInformationById_With_Empty_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int>();
            WebPictureInformation pictureInformation = WebServiceProxy.PictureService.GetPictureInformationById(GetClientInformation(), pictureId, null, null, metaData);

            Assert.IsNotNull(pictureInformation);
            Assert.AreEqual(pictureId, pictureInformation.Id);
            Assert.IsNotNull(pictureInformation.Picture.Image);
            Assert.IsTrue(pictureInformation.Picture.Image.Length > 0);
            Assert.IsNotNull(pictureInformation.MetaData);
            Assert.IsTrue(pictureInformation.MetaData.IsNotEmpty());
            Assert.IsNotNull(pictureInformation.Relations);
        }

        [TestMethod]
        public void GetPictureInformationById_With_MetaDataList()
        {
            long pictureId = 33;
            List<int> metaData = new List<int> { 2, 3 };
            WebPictureInformation pictureInformation = WebServiceProxy.PictureService.GetPictureInformationById(GetClientInformation(), pictureId, null, null, metaData);

            Assert.IsNotNull(pictureInformation);
            Assert.AreEqual(pictureId, pictureInformation.Id);
            Assert.IsNotNull(pictureInformation.Picture.Image);
            Assert.IsTrue(pictureInformation.Picture.Image.Length > 0);
            Assert.IsNotNull(pictureInformation.MetaData);
            Assert.IsTrue(pictureInformation.MetaData.IsNotEmpty());
            Assert.AreEqual(pictureInformation.MetaData.Count, metaData.Count);
            Assert.IsNotNull(pictureInformation.Relations);
        }

        [TestMethod]
        public void GetPictureMetaDataDescriptions()
        {
            List<WebPictureMetaDataDescription> pictureMetaDataDescriptions;
            WebLocale locale = new WebLocale();
            WebClientInformation clientInformation = GetClientInformation();

            locale.Id = 49; // English
            clientInformation.Locale = locale;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(clientInformation, WebServiceProxy.PictureService))
            {
                pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptions(clientInformation);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptions(clientInformation);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());

            locale.Id = 175; // Swedish
            clientInformation.Locale = locale;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(clientInformation, WebServiceProxy.PictureService))
            {
                pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptions(clientInformation);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptions(clientInformation);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }

        [TestMethod]
        public void GetPictureMetaDataDescriptionsByIds()
        {
            List<Int32> metaDataIds = new List<int> { 2, 4 };
            List<WebPictureMetaDataDescription> pictureMetaDataDescriptions;
            WebLocale locale = new WebLocale();
            WebClientInformation clientInformation = GetClientInformation();

            locale.Id = 49; // English
            clientInformation.Locale = locale;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(clientInformation, WebServiceProxy.PictureService))
            {
                pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptionsByIds(clientInformation, metaDataIds);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptionsByIds(clientInformation, metaDataIds);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());

            locale.Id = 175; // Swedish
            clientInformation.Locale = locale;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(clientInformation, WebServiceProxy.PictureService))
            {
                pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptionsByIds(clientInformation, metaDataIds);
                Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
            }

            pictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptionsByIds(clientInformation, metaDataIds);
            Assert.IsTrue(pictureMetaDataDescriptions.IsNotEmpty());
        }

        [TestMethod]
        public void GetAllRecommendedPicturesMetaData()
        {
            int copyright = 1;
            int pictureCreatedBy = 2;
            int pictureCreatedDate = 3;
            int description = 4;
            WebClientInformation clientInformation = GetClientInformation();

            List<int> metaDataIds = new List<int> { copyright };
            Int32 pictureRelationTypeId = 4;

            List<WebPictureMetaDataInformation> resultList = WebServiceProxy.PictureService.GetAllRecommendedPicturesMetaData(clientInformation, pictureRelationTypeId, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);

            // Check that only copyright data is read
            foreach (WebPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (WebPictureMetaData webPictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(webPictureMetaData.Name.Equals("Copyright"));
                    Assert.IsTrue(webPictureMetaData.Value.IsNotEmpty());
                }
            }

            metaDataIds = new List<int> { pictureCreatedBy, pictureCreatedDate };
            resultList = WebServiceProxy.PictureService.GetAllRecommendedPicturesMetaData(clientInformation, pictureRelationTypeId, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data 
            foreach (WebPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (WebPictureMetaData webPictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(webPictureMetaData.Name.IsNotEmpty());
                    Assert.IsTrue(webPictureMetaData.Value.IsNotEmpty());
                }
            }

            metaDataIds = new List<int> { description };
            resultList = WebServiceProxy.PictureService.GetAllRecommendedPicturesMetaData(clientInformation, pictureRelationTypeId, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data
            foreach (WebPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (WebPictureMetaData webPictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(webPictureMetaData.Name.Equals("Description"));
                    Assert.IsTrue(webPictureMetaData.Value.IsNotEmpty());
                }
            }

            pictureRelationTypeId = 1;
            metaDataIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            resultList = WebServiceProxy.PictureService.GetAllRecommendedPicturesMetaData(clientInformation, pictureRelationTypeId, metaDataIds);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 0);
            // Check data 
            foreach (WebPictureMetaDataInformation dataInformations in resultList)
            {
                foreach (WebPictureMetaData webPictureMetaData in dataInformations.PictureMetaDataList)
                {
                    Assert.IsTrue(webPictureMetaData.Name.IsNotEmpty());
                    Assert.IsTrue(webPictureMetaData.Value.IsNotEmpty());
                }
            }
        }

        [TestMethod]
        public void GetPictureBySearchCriteria()
        {
            List<WebPicture> pictures;
            WebPicturesSearchCriteria searchCriteria;

            // Test factors.
            searchCriteria = new WebPicturesSearchCriteria { FactorIds = new List<int> { 2551 } };
            pictures = WebServiceProxy.PictureService.GetPictureBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test taxa.
            searchCriteria = new WebPicturesSearchCriteria { TaxonIds = new List<int> { 233621 } };
            pictures = WebServiceProxy.PictureService.GetPictureBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test species facts. 

            searchCriteria = new WebPicturesSearchCriteria { SpeciesFactIdentifiers = new List<string> { "Taxon=226672,Factor=2540,IndividualCategory=0,Host=0,Period=0" } };
            pictures = WebServiceProxy.PictureService.GetPictureBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());

            // Test metadata (on db).
            searchCriteria = new WebPicturesSearchCriteria
                                 {
                                     MetaData = new List<WebPictureMetaData>
                                             {
                                                 new WebPictureMetaData
                                                     {
                                                         PictureMetaDataId = 1,
                                                         Value = ""
                                                     }
                                             }
                                 };
            pictures = WebServiceProxy.PictureService.GetPictureBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(pictures.IsNotEmpty());
        }

        [TestMethod]
        public void GetRecommendedPictureIdsByObjectGuid()
        {

            Int32 pictureRelationTypeId;

            List<string> pictureGuids = new List<string> { "100024", "100025", "100026", "2582", "Taxon=226672,Factor=2540,IndividualCategory=0,Host=0,Period=0" };
            // "2582" , Type 1;
            // "Taxon = 42, Factor = 2540, IndividualCategory = 0, Host = 0, Period = 0" , Type 2;
            // "100024", "100025", "100026" , Type 4;
            pictureRelationTypeId = 1;

            List<WebPictureGuid> guids = WebServiceProxy.PictureService.GetRecommendedPictureIdsByObjectGuid(GetClientInformation(), pictureGuids, pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationTypeId = 2;
            guids = WebServiceProxy.PictureService.GetRecommendedPictureIdsByObjectGuid(GetClientInformation(), pictureGuids, pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationTypeId = 4;
            guids = WebServiceProxy.PictureService.GetRecommendedPictureIdsByObjectGuid(GetClientInformation(), pictureGuids, pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);
        }

        [TestMethod]
        public void GetAllRecommendedPictureIds()
        {

            Int32 pictureRelationTypeId;
            pictureRelationTypeId = 1;

            List<WebPictureGuid> guids = WebServiceProxy.PictureService.GetAllRecommendedPictureIds(GetClientInformation(), pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationTypeId = 2;
            guids = WebServiceProxy.PictureService.GetAllRecommendedPictureIds(GetClientInformation(), pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);

            pictureRelationTypeId = 4;
            guids = WebServiceProxy.PictureService.GetAllRecommendedPictureIds(GetClientInformation(), pictureRelationTypeId);
            Assert.IsNotNull(guids);
            Assert.IsTrue(guids.Count > 0);
        }

        [TestMethod]
        public void CreateDeletePictureFilename()
        {
            Int64 pictureId = 33;
            List<WebPictureMetaData> pictureMetaData = new List<WebPictureMetaData>
                                                           {
                                                               new WebPictureMetaData
                                                                   {
                                                                       PictureMetaDataId = 1,
                                                                       HasPictureMetaDataId = true,
                                                                       Value = "(C) Test 2014"
                                                                   },
                                                               new WebPictureMetaData
                                                                   {
                                                                       PictureMetaDataId = 2,
                                                                       HasPictureMetaDataId = true,
                                                                       Value = "Test Author"
                                                                   },
                                                               new WebPictureMetaData
                                                                   {
                                                                       PictureMetaDataId = 3,
                                                                       HasPictureMetaDataId = true,
                                                                       Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                                                                   }
                                                           };
            WebPicture picture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, null, null, 0, false, string.Empty);
            String filename = @"\Temp\JN_Leiobunum-blackwalli_226679_Hane_new.jpg";
            Int32 affectedRows;
            DateTime lastModified = DateTime.Now;
            List<Int32> pictureMetaDataIds = new List<int> { 1, 2, 3 };
            String updatedBy = "test test";
            List<WebPictureMetaData> newPictureMetaData;
            WebPictureResponse webPictureResponse;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                webPictureResponse = WebServiceProxy.PictureService.CreatePictureFilename(GetClientInformation(), picture.Image, filename, lastModified, true, picture.VersionId + 1, updatedBy, pictureMetaData);
                Assert.AreEqual(webPictureResponse.AffectedRows, 1);
                Assert.IsTrue(webPictureResponse.Id > 0);
                newPictureMetaData = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), webPictureResponse.Id, pictureMetaDataIds);
                Assert.IsNotNull(newPictureMetaData);
                Assert.IsTrue(newPictureMetaData.IsNotEmpty());
                Assert.AreEqual(pictureMetaData.Count, newPictureMetaData.Count);
                for (int i = 0; i < newPictureMetaData.Count; i++)
                {
                    Assert.AreEqual(pictureMetaData[i].PictureMetaDataId, newPictureMetaData[i].PictureMetaDataId);
                    Assert.AreEqual(pictureMetaData[i].Value, newPictureMetaData[i].Value);
                }

                affectedRows = WebServiceProxy.PictureService.DeletePictureMetaData(GetClientInformation(), webPictureResponse.Id, pictureMetaData);
                Assert.AreEqual(affectedRows, pictureMetaData.Count);
                affectedRows = WebServiceProxy.PictureService.DeletePictureFilename(GetClientInformation(), null, filename, picture.PictureStringId);
                Assert.AreEqual(affectedRows, 4);
            }
        }

        [TestMethod]
        public void CreateDeletePictureMetaData()
        {
            Int64 pictureId = 32;
            List<WebPictureMetaData> pictureMetaData = new List<WebPictureMetaData>
                                                           {
                                                               new WebPictureMetaData
                                                                   {
                                                                       HasPictureMetaDataId = true,
                                                                       PictureMetaDataId = 1,
                                                                       Value = "(C) Test 2014"
                                                                   },
                                                               new WebPictureMetaData
                                                                   {
                                                                       HasPictureMetaDataId = true,
                                                                       PictureMetaDataId = 2,
                                                                       Value = "Test Author"
                                                                   },
                                                               new WebPictureMetaData
                                                                   {
                                                                       HasPictureMetaDataId = true,
                                                                       PictureMetaDataId = 3,
                                                                       Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                                   }
                                                           };
            List<WebPictureMetaData> newPictureMetaData;
            Int32 affectedRows;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                WebServiceProxy.PictureService.CreatePictureMetaData(GetClientInformation(), pictureId, pictureMetaData);
                newPictureMetaData = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, null);
                Assert.AreEqual(newPictureMetaData.Count, pictureMetaData.Count);
                pictureMetaData[0].Value = string.Empty;
                pictureMetaData[1].Value = string.Empty;
                pictureMetaData[2].Value = string.Empty;
                affectedRows = WebServiceProxy.PictureService.DeletePictureMetaData(GetClientInformation(), pictureId, pictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureMetaData.Count));
            }
        }

        [TestMethod]
        public void UpdatePictureMetaData()
        {
            Int64 pictureId = 33;
            List<WebPictureMetaData> oldPictureMetaData = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(), pictureId, null);
            List<WebPictureMetaData> updatePictureMetaData = new List<WebPictureMetaData>
                                                                 {
                                                                     new WebPictureMetaData
                                                                         {
                                                                             HasPictureMetaDataId = true,
                                                                             PictureMetaDataId = 1,
                                                                             Value = "Test (C)"
                                                                         },
                                                                     new WebPictureMetaData
                                                                         {
                                                                             HasPictureMetaDataId = true,
                                                                             PictureMetaDataId = 2,
                                                                             Value = "Test Author"
                                                                         }
                                                                 };
            String updatedBy = "test test";
            Int32 affectedRows;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                affectedRows = WebServiceProxy.PictureService.UpdatePictureMetaData(GetClientInformation(), pictureId, updatedBy, updatePictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == updatePictureMetaData.Count + 1)); // + 1: updatedBy.
                affectedRows = WebServiceProxy.PictureService.UpdatePictureMetaData(GetClientInformation(), pictureId, updatedBy, oldPictureMetaData);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == oldPictureMetaData.Count + 1)); // + 1: updatedBy.
            }
        }

        [TestMethod]
        public void CreatePictureRelations()
        {
            Boolean isRecommended = false;
            String objectGuid = "226679";
            Int64 pictureId = 33;
            Int32 typeId = 3;
            List<WebPictureRelation> pictureRelations = new List<WebPictureRelation>
                                                            {
                                                                new WebPictureRelation
                                                                    {
                                                                        IsRecommended = isRecommended,
                                                                        ObjectGuid = objectGuid,
                                                                        PictureId = pictureId,
                                                                        SortOrder = 2,
                                                                        TypeId = typeId
                                                                    },
                                                            };
            List<WebPictureRelation> newPictureRelations;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                WebServiceProxy.PictureService.CreatePictureRelations(GetClientInformation(),
                                                                      pictureRelations);
                newPictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(),
                                                                                                    pictureId);
                Assert.IsNotNull(newPictureRelations);
                Assert.IsTrue(newPictureRelations.IsNotEmpty());
            }
        }

        [TestMethod]
        public void UpdatePictureRelations()
        {
            Boolean pictureRelationFound;
            WebPictureRelation pictureRelation = new WebPictureRelation
                                                     {
                                                         Id = 1,
                                                         IsRecommended = false,
                                                         ObjectGuid = "1",
                                                         PictureId = 1,
                                                         SortOrder = 2,
                                                         TypeId = 3
                                                     };
            List<WebPictureRelation> pictureRelations = new List<WebPictureRelation> { pictureRelation };
            Int32 affectedRows;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                affectedRows = WebServiceProxy.PictureService.UpdatePictureRelations(GetClientInformation(),
                                                                                        pictureRelations);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureRelations.Count));

                List<WebPictureRelation> updatedPictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(),
                                                                                                                                 pictureRelation.PictureId);

                Assert.IsNotNull(updatedPictureRelations);
                Assert.IsTrue(updatedPictureRelations.IsNotEmpty());

                pictureRelationFound = false;

                foreach (WebPictureRelation updatedPictureRelation in updatedPictureRelations)
                {
                    if (pictureRelation.Id == updatedPictureRelation.Id)
                    {
                        Assert.AreEqual(pictureRelation.Id, updatedPictureRelation.Id);
                        Assert.AreEqual(pictureRelation.IsRecommended, updatedPictureRelation.IsRecommended);
                        Assert.AreEqual(pictureRelation.ObjectGuid, updatedPictureRelation.ObjectGuid);
                        Assert.AreEqual(pictureRelation.PictureId, updatedPictureRelation.PictureId);
                        Assert.AreEqual(pictureRelation.SortOrder, updatedPictureRelation.SortOrder);
                        Assert.AreEqual(pictureRelation.TypeId, updatedPictureRelation.TypeId);
                        pictureRelationFound = true;
                    }
                }

                Assert.IsTrue(pictureRelationFound);
            }
        }

        [TestMethod]
        public void DeletePictureRelations()
        {
            List<Int64> pictureRelationIds = new List<Int64> { 1 , 16 };
            Int32 affectedRows;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                affectedRows = WebServiceProxy.PictureService.DeletePictureRelations(GetClientInformation(),
                                                                                     pictureRelationIds);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == pictureRelationIds.Count));

                List<WebPictureRelation> deletedPictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(), 1);

                Assert.IsTrue(deletedPictureRelations.IsEmpty());
            }
        }

        [TestMethod]
        public void UpdatePictures()
        {
            Int64 pictureId = 33;
            WebPicture oldPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, 50, 50, 0, false, string.Empty);
            List<WebPicture> updatePictures = new List<WebPicture>
                                                  {
                                                      new WebPicture
                                                          {
                                                              Id = pictureId,
                                                              IsPublic = true,
                                                              IsArchived = true,
                                                              LastUpdated = DateTime.Now,
                                                              UpdatedBy = "test test"
                                                          }
                                                  };
            String updatedBy = "test test";
            Int32 affectedRows;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.PictureService))
            {
                affectedRows = WebServiceProxy.PictureService.UpdatePictures(GetClientInformation(), updatePictures, updatedBy);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == updatePictures.Count));

                WebPicture updatedPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(), pictureId, 50, 50, 0, false, string.Empty);

                Assert.AreEqual(pictureId, updatedPicture.Id);
                Assert.AreEqual(updatePictures[0].IsPublic, updatedPicture.IsPublic);
                Assert.AreEqual(updatePictures[0].IsArchived, updatedPicture.IsArchived);
                Assert.IsTrue(updatePictures[0].LastUpdated <= updatedPicture.LastUpdated);
                Assert.AreEqual(updatePictures[0].UpdatedBy, updatedPicture.UpdatedBy);
                affectedRows = WebServiceProxy.PictureService.UpdatePictures(GetClientInformation(), new List<WebPicture> { oldPicture }, updatedBy);
                Assert.IsTrue((affectedRows > 0) && (affectedRows == 1));
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebLoginResponse loginResponse;

            Configuration.InstallationType = InstallationType.ServerTest;
            loginResponse = WebServiceProxy.PictureService.Login(Settings.Default.TestUserName,
                                                                 Settings.Default.TestPassword,
                                                                 ApplicationIdentifier.PictureAdmin.ToString(),
                                                                 false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Token = loginResponse.Token;
        }
    }
}
