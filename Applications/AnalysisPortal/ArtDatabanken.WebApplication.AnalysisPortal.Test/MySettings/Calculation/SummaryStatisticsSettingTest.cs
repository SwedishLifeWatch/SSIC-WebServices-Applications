using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.MySettings.Calculation
{
    [TestClass]
    public class SummaryStatisticsSettingTest
    {
        [TestMethod]
        [TestCategory("Unit test")]
        public void SummaryStatisticsSettingPropertyTest()
        {
            SummaryStatisticsSetting setting = new SummaryStatisticsSetting();

            setting.ResultCacheNeedsRefresh = false;

            // ************ Check all settings at startup as designed *************************
            // These setting is active at startup
            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsTrue(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is default at start up, cash needs to be updated
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsFalse(setting.ResultCacheNeedsRefresh);

            // ************ Update settings and test  *************************
            // Now we change CalculateNumberOfObservationsfromObsData setting
            setting.CalculateNumberOfObservationsfromObsData = false;

            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);

            // These setting is active at startup
            Assert.IsTrue(setting.IsActive);
            Assert.IsFalse(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsTrue(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is default at start up
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values and check again
            setting.ResetSettings();
            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsTrue(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Now we change CalculateNumberOfSpeciesfromObsData setting
            setting.CalculateNumberOfSpeciesfromObsData = false;

            Assert.IsTrue(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);

            // These setting is active at startup
            Assert.IsTrue(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsFalse(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is not default and cash needs to be updated
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values
            setting.ResetSettings();

            // Now we change CalculateNumberOfSpeciesfromObsData and CalculateNumberOfObservationsfromObsData setting
            setting.CalculateNumberOfSpeciesfromObsData = false;
            setting.CalculateNumberOfObservationsfromObsData = false;

            Assert.IsFalse(setting.HasActiveSettings);
            Assert.IsFalse(setting.HasSettings);

            // These setting is active at startup
            Assert.IsTrue(setting.IsActive);
            Assert.IsFalse(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsFalse(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is not default and cash needs to be updated
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values
            setting.ResetSettings();

            // Set this setting not to be active
            setting.IsActive = false;
            Assert.IsFalse(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);

            // These setting is active at startup
            Assert.IsFalse(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsTrue(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is default at start up and no cash needs to be updated
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            // Set default values
            setting.ResetSettings();

            // Now we change WfsSummaryStatisticsLayerId setting
            setting.WfsSummaryStatisticsLayerId = 0;
            Assert.IsFalse(setting.HasActiveSettings);
            Assert.IsTrue(setting.HasSettings);
            Assert.IsFalse(setting.IsActive);
            Assert.IsTrue(setting.CalculateNumberOfObservationsfromObsData);
            Assert.IsTrue(setting.CalculateNumberOfSpeciesfromObsData);
            Assert.IsNotNull(setting.WfsSummaryStatisticsLayerId);

            // This setting is not default and cash needs to be updated
            Assert.IsFalse(setting.IsSettingsDefault());
            Assert.IsTrue(setting.ResultCacheNeedsRefresh);

            setting = new SummaryStatisticsSetting();
            setting.ResultCacheNeedsRefresh = false;

            // Set alreday existing value and check that cash in not needed to be updated
            setting.CalculateNumberOfSpeciesfromObsData = true;
            Assert.IsTrue(setting.IsSettingsDefault());
            Assert.IsFalse(setting.ResultCacheNeedsRefresh);
        }
    }
}