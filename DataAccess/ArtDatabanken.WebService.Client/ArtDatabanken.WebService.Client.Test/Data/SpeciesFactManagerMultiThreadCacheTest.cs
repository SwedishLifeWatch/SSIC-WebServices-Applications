using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesFactManagerMultiThreadCacheTest : TestBase
    {
        private SpeciesFactManagerMultiThreadCache _speciesFactManager;

        public SpeciesFactManagerMultiThreadCacheTest()
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

        private SpeciesFactManagerMultiThreadCache GetSpeciesFactManager(bool refresh = false)
        {
            if (_speciesFactManager.IsNull() || refresh)
            {
                _speciesFactManager = new SpeciesFactManagerMultiThreadCache { DataSource = new SpeciesFactDataSource() };
            }
            return _speciesFactManager;
        }
    }
}