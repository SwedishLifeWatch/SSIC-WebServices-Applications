using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.Test
{
    [TestClass]
    public class DataSourceBaseTest : TestBase
    {
        public DataSourceBaseTest()
        {
        }

        [TestMethod]
        public new void GetClientInformation()
        {
            WebClientInformation clientInformation;

            clientInformation = ((UserDataSource)(CoreData.UserManager.DataSource)).GetClientInformation(GetUserContext());
            Assert.IsNotNull(clientInformation);
        }
    }
}
