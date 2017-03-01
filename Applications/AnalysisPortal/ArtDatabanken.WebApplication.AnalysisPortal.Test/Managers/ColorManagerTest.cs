using System.Collections.Generic;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Data
{
    
        
    [TestClass()]
    public class ColorManagerTest
    {


        


        
        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void Test_colors()
        {
            Color color = ColorManager.GetRandomColor();
            
            color = ColorManager.GetColor(0);
            Assert.AreEqual(ColorManager.ColorFromHtml("#99CCFF"), color);

            color = ColorManager.GetColor(1);
            Assert.AreEqual(ColorManager.ColorFromHtml("#CCFFCC"), color);
            
            var colors = new List<Color>();
            colors.Add(ColorManager.GetColor(0));
            colors.Add(ColorManager.GetColor(1));
            colors.Add(ColorManager.GetColor(3));
            color = ColorManager.GetNextUnusedColor(colors);
            Assert.AreEqual(ColorManager.GetColor(2), color);

            var strColors = new List<string>();
            strColors.Add("#99CCFF");
            strColors.Add("#CCFFCC");
            strColors.Add("#FFFF99");
            strColors.Add("#CC99CC");
            strColors.Add("#FF9999");
            color = ColorManager.GetNextUnusedColor(strColors);
            Assert.AreEqual(ColorManager.GetColor(4), color);

            strColors.Clear();
            color = ColorManager.GetNextUnusedColor(strColors);
            Assert.AreEqual(ColorManager.GetColor(0), color);

            strColors = null;
            color = ColorManager.GetNextUnusedColor(strColors);
            Assert.AreEqual(ColorManager.GetColor(0), color);

            Assert.AreEqual("#99CCFF", ColorManager.GetColor(0).ToHexString());
        }


    }

}
