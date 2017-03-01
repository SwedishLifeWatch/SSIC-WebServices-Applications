using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Helpers
{
    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass()]
    public class SessionHandlerTest
    {
    
        
        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void AddLanguageToSessionTest()
        {
            using (ShimsContext.Create())
            {
                //Set session helper for handling HttpContext data.
                DictionarySessionHelper sessionHelper = new DictionarySessionHelper();
                SessionHandler.SetSessionHelper(sessionHelper);
                SessionHandler.Language = "se";
                string lang = SessionHandler.Language;
                Assert.AreEqual<string>("se", lang);
            }
        }
    }
}
