using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    [TestClass]
    public class WebSpeciesActivityExtensionTest : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void LoadData()
        {
            WebSpeciesActivity speciesActivity;

            using (SpeciesObservationServer database = new SpeciesObservationServer())
            {
                using (DataReader dataReader = database.GetSpeciesActivities((Int32)(LocaleId.sv_SE)))
                {
                    while (dataReader.Read())
                    {
                        speciesActivity = new WebSpeciesActivity();
                        speciesActivity.LoadData(dataReader);
                        Assert.IsNotNull(speciesActivity);
                        Assert.IsTrue(0 <= speciesActivity.CategoryId);
                        Assert.IsTrue(speciesActivity.Guid.IsNotEmpty());
                        Assert.IsTrue(0 <= speciesActivity.Id);
                        Assert.IsTrue(speciesActivity.Identifier.IsNotEmpty());
                        Assert.IsTrue(speciesActivity.Name.IsNotEmpty());
                        Assert.IsTrue(speciesActivity.TaxonIds.IsNotEmpty());
                    }
                }
            }
        }
    }
}
