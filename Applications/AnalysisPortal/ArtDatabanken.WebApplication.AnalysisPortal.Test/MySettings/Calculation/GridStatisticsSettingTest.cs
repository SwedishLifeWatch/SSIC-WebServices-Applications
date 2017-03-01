using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.MySettings.Calculation
{
  
    [TestClass]
    public class GridStatisticsSettingTest
    {
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GridStatisticsSettingPropertyTest()
        {
            GridStatisticsSetting setting = new GridStatisticsSetting();
            setting.ResultCacheNeedsRefresh = false;

            //************ Check all settings at startup as designed *************************
            //These setting is active at startup
            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservations);
            Assert.IsTrue(setting.CalculateNumberOfTaxa);
            CoordinateSystemId enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            string stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.SWEREF99.ToString()));
            Assert.IsTrue(setting.GridSize == 10000 );
            // This setting is default at start up, cash needs to be updated
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsFalse(setting.ResultCacheNeedsRefresh);

            //************ Update settings and test  *************************
            setting.CalculateNumberOfObservations = false;
            setting.CoordinateSystemId = 2;
            setting.GridSize = 20000;

            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            // These setting is active at startup
            Assert.IsTrue(setting.IsActive);
            Assert.IsFalse(setting.CalculateNumberOfObservations);
            Assert.IsTrue(setting.CalculateNumberOfTaxa);
            enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(setting.GridSize == 20000);
            // This setting is default at start up 
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values and check again 
            setting.ResetSettings();
            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservations);
            Assert.IsTrue(setting.CalculateNumberOfTaxa);
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);
            enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.SWEREF99.ToString()));
            Assert.IsTrue(setting.GridSize == 10000);
            // Now we change CalculateNumberOfSpeciesfromObsData setting
            setting.CalculateNumberOfTaxa = false;
            setting.GridSize = 50000;

            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            // These setting is active at startup
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservations);
            Assert.IsFalse(setting.CalculateNumberOfTaxa);
            enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.SWEREF99.ToString()));
            Assert.IsTrue(setting.GridSize == 50000);
            // This setting is not default and cash needs to be updated
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values
            setting.ResetSettings();
            // Now we change CalculateNumberOfSpeciesfromObsData and CalculateNumberOfObservationsfromObsData setting
            setting.CalculateNumberOfObservations = false;
            setting.CalculateNumberOfTaxa = false;
            setting.GridSize = 0;
            setting.CoordinateSystemId = 0;
            // TODO GridStatistics settings are always true
            //Assert.IsFalse(setting.HasActiveSettings);
            //Assert.IsFalse(setting.HasSettings);
            //// These setting is active at startup
            //Assert.IsTrue(setting.IsActive);
            Assert.IsFalse(setting.CalculateNumberOfObservations);
            Assert.IsFalse(setting.CalculateNumberOfTaxa);
            enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.None.ToString()));
            Assert.IsTrue(setting.GridSize == 0);
            // This setting is not default and cash needs to be updated
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values
            setting.ResetSettings();
            //Set this setting not to be active
            setting.IsActive = false;
            Assert.IsFalse(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            // These setting is active at startup
            Assert.IsFalse(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservations);
            Assert.IsTrue(setting.CalculateNumberOfTaxa);
            enumDisplayStatus = ((CoordinateSystemId)setting.CoordinateSystemId);
            stringValue = enumDisplayStatus.ToString();
            Assert.IsTrue(stringValue.Equals(CoordinateSystemId.SWEREF99.ToString()));
            Assert.IsTrue(setting.GridSize == 10000);
            // This setting is default at start up and no cash needs to be updated
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            setting = new GridStatisticsSetting();
            setting.ResultCacheNeedsRefresh = false;
            //Set alreday existing value and check that cash in not needed to be updated
            setting.CalculateNumberOfTaxa = true;
            Assert.IsTrue(setting.IsSettingsDefault());
            // TODO not required to always update cash ...Assert.IsFalse(setting.ResultCacheNeedsRefresh);

        }
    }
}
