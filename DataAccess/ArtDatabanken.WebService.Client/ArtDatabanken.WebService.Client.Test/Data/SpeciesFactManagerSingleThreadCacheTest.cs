namespace ArtDatabanken.WebService.Client.Test.Data
{
    using ArtDatabanken.Data;
    using ArtDatabanken.WebService.Client.TaxonAttributeService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SpeciesFactManagerSingleThreadCacheTest : TestBase
    {
        private SpeciesFactManagerSingleThreadCache _speciesFactManager;

        public SpeciesFactManagerSingleThreadCacheTest()
        {
            _speciesFactManager = null;
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            long durationFirst, durationSecond;
            SpeciesFactQualityList speciesFactQualities;

            Stopwatch.Start();
            speciesFactQualities = GetSpeciesFactManager(true).GetSpeciesFactQualities(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());

            Stopwatch.Start();
            speciesFactQualities = GetSpeciesFactManager().GetSpeciesFactQualities(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        private SpeciesFactManagerSingleThreadCache GetSpeciesFactManager(bool refresh = false)
        {
            if (_speciesFactManager.IsNull() || refresh)
            {
                _speciesFactManager = new SpeciesFactManagerSingleThreadCache { DataSource = new SpeciesFactDataSource() };
            }
            return _speciesFactManager;
        }
    }
}