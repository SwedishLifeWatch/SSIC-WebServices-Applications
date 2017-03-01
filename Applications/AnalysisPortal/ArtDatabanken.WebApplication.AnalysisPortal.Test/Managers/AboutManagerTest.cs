using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Data
{
    [TestClass]
    public class AboutManagerTest
    {
        [Ignore]
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetAboutDataSourcesViewModel()
        {
            AboutViewModel model = AboutManager.GetAboutDataProvidersViewModel("");
            //Assert.AreEqual("Data sources", model.TitleLabel);
            //Assert.AreEqual("The portal is connected to several data sources by default. Environmental data of different types are available as separate map layers while species observations from several sources are provided in a unified format particularly suitable for biodiversity analyses.", model.Description);
            Assert.IsTrue(model.TitleLabel.IsNotEmpty());
            Assert.IsTrue(model.Description.Length > model.TitleLabel.Length);
            Assert.IsTrue(model.Items.Count == 4);
            foreach (AboutItem item in model.Items)
            {
                Assert.IsTrue(item.Header.IsNotEmpty());
                Assert.IsTrue(item.Description.Length > item.Header.Length);
            }
        }

        [Ignore]
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetSwedishAboutDataSourcesViewModel()
        {
            AboutViewModel model = AboutManager.GetAboutDataProvidersViewModel("sv-SE");
            Assert.IsTrue(model.TitleLabel.IsNotEmpty());
            Assert.IsTrue(model.Description.Length > model.TitleLabel.Length);
            Assert.IsTrue(model.Items.Count == 4);
            foreach (AboutItem item in model.Items)
            {
                Assert.IsTrue(item.Header.IsNotEmpty());
                Assert.IsTrue(item.Description.Length > item.Header.Length);
            }
        }

        [Ignore]
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetAboutFiltersViewModel()
        {
            AboutViewModel model = AboutManager.GetAboutFiltersViewModel("");
            
            Assert.IsTrue(model.TitleLabel.IsNotEmpty());
            Assert.IsTrue(model.Description.Length > model.TitleLabel.Length);
            Assert.IsTrue(model.Items.Count == 5);
            foreach (AboutItem item in model.Items)
            {
                Assert.IsTrue(item.Header.IsNotEmpty());
                Assert.IsTrue(item.Description.Length > item.Header.Length);
            }
        }

        [Ignore]
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetSwedishAboutFiltersViewModel()
        {
            AboutViewModel model = AboutManager.GetAboutFiltersViewModel("sv-SE");
            Assert.IsTrue(model.TitleLabel.IsNotEmpty());
            Assert.IsTrue(model.Description.Length > model.TitleLabel.Length);
            Assert.IsTrue(model.Items.Count == 4);
            foreach (AboutItem item in model.Items)
            {
                Assert.IsTrue(item.Header.IsNotEmpty());
                Assert.IsTrue(item.Description.Length > item.Header.Length);
            }
        }
    }
}
