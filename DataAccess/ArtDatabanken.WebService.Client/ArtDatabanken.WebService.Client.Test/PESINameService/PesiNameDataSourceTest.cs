using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Client.PESINameService;

namespace ArtDatabanken.WebService.Client.Test.PESINameService
{

    [TestClass]
    public class PesiNameDataSourceTest
    {
        private PesiNameDataSource _pesiNameDataSource;

        public PesiNameDataSourceTest()
        {
            _pesiNameDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PesiNameDataSource pesiNameDataSource;
            pesiNameDataSource = new PesiNameDataSource();

            Assert.IsNotNull(pesiNameDataSource);

        }

        [TestMethod]
        public void GetPesiGuidByScientificName()
        {
            String scientificName = "Geranium sylvaticum";
            String pesiGUID = null;
            pesiGUID = GetPesiNameDataSource(true).GetPesiGuidByScientificName(scientificName);
            Assert.AreEqual("4F551B65-C961-4A7F-B9AF-2212D800873E", pesiGUID);
        }

        private PesiNameDataSource GetPesiNameDataSource()
        {
            return GetPesiNameDataSource(false);
        }

        private PesiNameDataSource GetPesiNameDataSource(Boolean refresh)
        {
            if (_pesiNameDataSource.IsNull() || refresh)
            {
                _pesiNameDataSource = new PesiNameDataSource();
            }
            return _pesiNameDataSource;
        }
    }
}
