using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.PictureService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PictureManagerSingleThreadCacheTest : TestBase
    {
        private PictureManagerSingleThreadCache _pictureManager;

        public PictureManagerSingleThreadCacheTest()
        {
            _pictureManager = null;
        }

        private PictureManager GetPictureManager(Boolean refresh = false)
        {
            if (_pictureManager.IsNull() || refresh)
            {
                _pictureManager = new PictureManagerSingleThreadCache();
                _pictureManager.DataSource = new PictureDataSource();
            }

            return _pictureManager;
        }

        [TestMethod]
        public void GetPictureRelationDataTypes()
        {
            Int64 durationFirst, durationSecond;
            PictureRelationDataTypeList pictureRelationDataTypes;

            Stopwatch.Start();
            pictureRelationDataTypes = GetPictureManager(true).GetPictureRelationDataTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());

            Stopwatch.Start();
            pictureRelationDataTypes = GetPictureManager().GetPictureRelationDataTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(pictureRelationDataTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetPictureRelationTypes()
        {
            Int64 durationFirst, durationSecond;
            PictureRelationTypeList pictureRelationTypes;

            Stopwatch.Start();
            pictureRelationTypes = GetPictureManager(true).GetPictureRelationTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(pictureRelationTypes.IsNotEmpty());

            Stopwatch.Start();
            pictureRelationTypes = GetPictureManager().GetPictureRelationTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(pictureRelationTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }
    }
}
