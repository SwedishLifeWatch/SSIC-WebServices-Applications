using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AnalysisPortal.Helpers.ActionFilters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalysisPortal.Tests.Helpers.ActionFilters
{
    [TestClass]

    public class IndexedBySearchRobotsAttributeTests
    {

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetAllRobotIndexedPages_Default_ReturnAllRobotIndexedPages()
        {
            IEnumerable<MethodInfo> allRobotIndexedPages = IndexedBySearchRobotsAttribute.GetAllRobotIndexedPages();
            Assert.IsTrue(allRobotIndexedPages.Count() > 0);
        }
    }
}
