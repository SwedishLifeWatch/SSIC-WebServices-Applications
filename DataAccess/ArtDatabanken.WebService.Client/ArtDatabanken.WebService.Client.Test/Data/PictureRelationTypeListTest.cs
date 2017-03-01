using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PictureRelationTypeListTest : TestBase
    {
        private PictureRelationTypeList _pictureRelationTypes;

        public PictureRelationTypeListTest()
        {
            _pictureRelationTypes = null;
        }

        protected override string GetTestApplicationName()
        {
            return ApplicationIdentifier.PictureAdmin.ToString();
        }

        [TestMethod]
        public void GetByIdentifier()
        {
            IPictureRelationType pictureRelationType;
            PictureRelationTypeIdentifier identifier;

            foreach (IPictureRelationType tempPictureRelationType in GetPictureRelationTypes(true))
            {
                identifier = (PictureRelationTypeIdentifier)(Enum.Parse(typeof(PictureRelationTypeIdentifier),
                                                                               tempPictureRelationType.Identifier));
                pictureRelationType = GetPictureRelationTypes().Get(identifier);
                Assert.IsNotNull(pictureRelationType);
                Assert.AreEqual(tempPictureRelationType.Identifier, pictureRelationType.Identifier);
            }

            foreach (PictureRelationTypeIdentifier pictureRelationTypeIdentifier in Enum.GetValues(typeof(PictureRelationTypeIdentifier)))
            {
                pictureRelationType = GetPictureRelationTypes().Get(pictureRelationTypeIdentifier);
                Assert.IsNotNull(pictureRelationType);
                Assert.AreEqual(pictureRelationTypeIdentifier.ToString(), pictureRelationType.Identifier);
            }
        }

        private PictureRelationTypeList GetPictureRelationTypes(Boolean refresh = false)
        {
            if (_pictureRelationTypes.IsNull() || refresh)
            {
                _pictureRelationTypes = CoreData.PictureManager.GetPictureRelationTypes(GetUserContext());
            }

            return _pictureRelationTypes;
        }
    }
}
