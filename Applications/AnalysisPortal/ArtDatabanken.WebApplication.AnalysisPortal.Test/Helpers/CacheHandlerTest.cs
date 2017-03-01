using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;


namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Helpers
{

    [TestClass]
    public class CacheHandlerTest
    {  
        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void AddApplicationUserContextToCacheTest()        
        {
           using (ShimsContext.Create())
           {
                IUserContext userContext = new UserContext();
                IUser user = new User(userContext);
                user.UserName = "testuser";
                userContext.User = user;                
                CacheHandler.SetApplicationUserContext("mykey", userContext);
                IUserContext cachedUserContext = CacheHandler.GetApplicationUserContext("mykey");
                Assert.AreEqual<string>("testuser", cachedUserContext.User.UserName);
            }
        }
    }
}
