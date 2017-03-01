using System;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    using ArtDatabanken.Data;

    [TestClass]
    public class WebClientTokenTest : TestBase
    {
        [TestMethod]
        public void Constructor()
        {
            WebClientToken clientToken;
            String token;

            Configuration.InstallationType = InstallationType.ServerTest;
            clientToken = new WebClientToken(WebServiceData.WebServiceManager.Name,
                                             ApplicationIdentifier.PrintObs.ToString(),
                                             WebServiceData.WebServiceManager.Key);
            token = clientToken.Token;
            Assert.IsTrue(token.IsNotEmpty());

            Configuration.InstallationType = InstallationType.Production;
            clientToken = new WebClientToken(token,
                                             WebServiceData.WebServiceManager.Key);
            token = clientToken.Token;
            Assert.IsTrue(token.IsNotEmpty());
            Configuration.InstallationType = InstallationType.ServerTest;
        }
    }
}
