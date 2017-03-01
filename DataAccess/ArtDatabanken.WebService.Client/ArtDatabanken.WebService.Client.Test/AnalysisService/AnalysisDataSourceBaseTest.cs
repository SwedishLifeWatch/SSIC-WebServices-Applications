using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.AnalysisService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.AnalysisService
{
    
    [TestClass]
    public class AnalysisDataSourceBaseTest
    {
        private AnalysisDataSourceBase _analysisDataSourceBase;

        public AnalysisDataSourceBaseTest()
        {
            _analysisDataSourceBase = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AnalysisDataSourceBase analysisDataSourceBase;

            analysisDataSourceBase = new AnalysisDataSourceBase();
            Assert.IsNotNull(analysisDataSourceBase);
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            IDataSourceInformation dataSource;

            dataSource = GetAnalysisDataSourceBase(true).GetDataSourceInformation();
            Assert.IsNotNull(dataSource);
        }

        private AnalysisDataSourceBase GetAnalysisDataSourceBase()
        {
            return GetAnalysisDataSourceBase(false);
        }

        private AnalysisDataSourceBase GetAnalysisDataSourceBase(Boolean refresh)
        {
            if (_analysisDataSourceBase.IsNull() || refresh)
            {
                _analysisDataSourceBase = new AnalysisDataSourceBase();
            }
            return _analysisDataSourceBase;
        }
        
    }
}
