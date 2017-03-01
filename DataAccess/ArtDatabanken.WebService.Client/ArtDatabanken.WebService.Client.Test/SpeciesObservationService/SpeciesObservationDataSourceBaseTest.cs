using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.SpeciesObservationService
{
    
    [TestClass]
    public class SpeciesObservationDataSourceBaseTest
    {
        private SpeciesObservationDataSourceBase _speciesObservationDataSourceBase;

        public SpeciesObservationDataSourceBaseTest()
        {
            _speciesObservationDataSourceBase = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesObservationDataSourceBase speciesObservationDataSourceBase;

            speciesObservationDataSourceBase = new SpeciesObservationDataSourceBase();
            Assert.IsNotNull(speciesObservationDataSourceBase);
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            IDataSourceInformation dataSource;

            dataSource = GetSpeciesObservationDataSourceBase(true).GetDataSourceInformation();
            Assert.IsNotNull(dataSource);
        }

        private SpeciesObservationDataSourceBase GetSpeciesObservationDataSourceBase()
        {
            return GetSpeciesObservationDataSourceBase(false);
        }

        private SpeciesObservationDataSourceBase GetSpeciesObservationDataSourceBase(Boolean refresh)
        {
            if (_speciesObservationDataSourceBase.IsNull() || refresh)
            {
                _speciesObservationDataSourceBase = new SpeciesObservationDataSourceBase();
            }
            return _speciesObservationDataSourceBase;
        }
        
    }
}
