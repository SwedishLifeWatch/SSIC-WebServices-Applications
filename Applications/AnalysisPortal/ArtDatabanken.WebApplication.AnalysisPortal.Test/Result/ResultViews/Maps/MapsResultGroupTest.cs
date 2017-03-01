using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
     [TestClass]
    public class MapsResultGroupTest
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GridStMapsResultGroupTest()
        {
            MapsResultGroup resultGroup = new MapsResultGroup();
            Assert.IsNotNull(resultGroup);
            Assert.IsTrue(resultGroup.Items.Count >= 3);
        }
     }
}
