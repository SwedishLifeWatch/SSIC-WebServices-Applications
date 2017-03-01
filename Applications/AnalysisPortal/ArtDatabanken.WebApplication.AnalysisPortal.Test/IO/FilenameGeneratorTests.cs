using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.IO
{
    [TestClass]
    public class FilenameGeneratorTests
    {        
        [TestMethod]
        public void TestGenerateValidFilename()
        {
            //Arrange            
            DateTime date = new DateTime(2017, 1, 23, 13, 47, 52);

            //Act
            var filename = FilenameGenerator.CreateFilename("My test filename", FileType.ExcelXlsx, date);

            //Assert
            Assert.AreEqual("My test filename_20170123_13h47m52s.xlsx", filename);
        }
    }
}
