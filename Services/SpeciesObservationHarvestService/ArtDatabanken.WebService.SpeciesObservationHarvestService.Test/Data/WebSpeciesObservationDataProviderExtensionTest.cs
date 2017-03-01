using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationDataProviderExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            using (DataReader dataReader = GetContext().GetSpeciesObservationDatabase().GetSpeciesObservationDataProviders((Int32)(LocaleId.sv_SE)))
            {
                while (dataReader.Read())
                {
                    WebSpeciesObservationDataProvider dataProvider = new WebSpeciesObservationDataProvider();
                    dataProvider.LoadData(dataReader);
                    Assert.IsTrue(dataProvider.ContactEmail.IsNotEmpty());
                    Assert.IsTrue(dataProvider.ContactPerson.IsNotEmpty());
                    Assert.IsTrue(dataProvider.Guid.IsNotEmpty());
                    Assert.IsTrue(0 <= dataProvider.Id);
                    Assert.IsTrue(dataProvider.Name.IsNotEmpty());
                    Assert.IsTrue(0 <= dataProvider.NonPublicSpeciesObservationCount);
                    Assert.IsTrue(dataProvider.Organization.IsNotEmpty());
                    Assert.IsTrue(0 <= dataProvider.PublicSpeciesObservationCount);
                    Assert.IsTrue(0 <= dataProvider.SpeciesObservationCount);
                    Assert.IsTrue(dataProvider.Url.IsNotEmpty());
                }
            }
        }
    }
}
