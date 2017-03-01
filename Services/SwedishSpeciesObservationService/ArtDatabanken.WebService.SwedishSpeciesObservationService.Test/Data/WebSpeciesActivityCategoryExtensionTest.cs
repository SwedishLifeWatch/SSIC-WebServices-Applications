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
    public class WebSpeciesActivityCategoryExtensionTest : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void LoadData()
        {
            WebSpeciesActivityCategory speciesActivityCategory;

            using (SpeciesObservationServer database = new SpeciesObservationServer())
            {
                using (DataReader dataReader = database.GetSpeciesActivityCategories((Int32)(LocaleId.sv_SE)))
                {
                    while (dataReader.Read())
                    {
                        speciesActivityCategory = new WebSpeciesActivityCategory();
                        speciesActivityCategory.LoadData(dataReader);
                        Assert.IsNotNull(speciesActivityCategory);
                        Assert.IsTrue(speciesActivityCategory.Guid.IsNotEmpty());
                        Assert.IsTrue(0 <= speciesActivityCategory.Id);
                        Assert.IsTrue(speciesActivityCategory.Identifier.IsNotEmpty());
                        Assert.IsTrue(speciesActivityCategory.Name.IsNotEmpty());
                    }
                }
            }
        }
    }
}
