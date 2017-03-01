using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisPortal;
using AnalysisPortal.Controllers;

namespace AnalysisPortal.Tests
{
    [TestClass]
    public class HomeControllerTest : DBTestControllerBaseTest
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [Ignore] // Test method AnalysisPortal.Tests.HomeControllerTest.Index threw exception: System.IO.DirectoryNotFoundException: Det gick inte att hitta en del av sökvägen C:\Dev\ArtDatabanken\Applications\AnalysisPortal\AnalysisPortal.Test\bin\Debugcontent\News\News.xml.
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            

            // Assert
            Assert.IsNotNull(result);
        }
     
    }
}
