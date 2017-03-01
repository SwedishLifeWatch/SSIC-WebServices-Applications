using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test
{
    public class TestBase
    {
        // These test assumes that the following information
        // has been stored in the tabel ObsUsers:ClientApplicationInformation:
        // WebServiceTest	1.2.3.4	1
        protected const Int32 AGRICULTURAL_LANDSCAPE_FACTOR_ID = 663;
        protected const Int32 BIUS_FOREST_FACTOR_ID = 2;
        protected const Int32 FOREST_LANSCAPE_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_FACTOR_ID = 750;
        protected const Int32 LANDSCAPES_FACTOR_ID = 661;
        protected const Int32 LANDSCAPE_FOREST_FACTOR_ID = 662;
        protected const Int32 LANDSCAPE_AGRICULTURE_FACTOR_ID = 663;
        protected const Int32 LANDSCAPE_MOUNTAIN_FACTOR_ID = 665;
        protected const Int32 LANDSCAPE_FRESH_WATER_FACTOR_ID = 667;
        protected const Int32 ORGANISM_GROUP_FACTOR_ID = 656;
        protected const Int32 REDLIST_FACTOR_ID = 542;
        protected const Int32 REDLIST_TAXON_TYPE_FACTOR_ID = 655;
        protected const Int32 SPECIES_FACT_DATABASE_FACTOR_ID = 985;

        protected const Int32 JAN_EDELSJO_REFERENCE_ID = 384;

        protected const Int32 BEAR_TAXON_ID = 100145;
        protected const Int32 BIRD_TAXON_ID = 4000104;
        protected const Int32 COMMON_BUZZARD_TAXON_ID = 102942; // Ormvråk.
        protected const Int32 DUMMY_TAXON_ID = 0;
        protected const Int32 GOLDEN_EAGLE_TAXON_ID = 100011;
        protected const Int32 MUTE_SWAN_TAXON_ID = 102927; // Knölsvan.
        protected const Int32 NORTHERN_HAWK_OWL_TAXON_ID = 102620;
        protected const Int32 RED_FOX_TAXON_ID = 206026;
        protected const Int32 HAWK_BIRDS_TAXON_ID = 2002066;
        protected const Int32 BADGER_TAXON_ID = 206036; // Grävling.
        protected const Int32 BEAVER_TAXON_ID = 102607; // Bäver.
        protected const Int32 HEDGEHOG_TAXON_ID = 100053; // Igelkott.
        protected const Int32 FALLOW_DEER_TAXON_ID = 206044; // Dovhjort.
        protected const Int32 MAMMAL_TAXON_ID = 4000107; // Däggdjur.

        protected const Int32 FAMILY_TAXON_TYPE_ID = 11;
        protected const Int32 GENUS_TAXON_TYPE_ID = 14;
        protected const Int32 SPECIES_TAXON_TYPE_ID = 17;

        protected String GetString(Int32 stringLength)
        {
            Int32 stringIndex;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder(stringLength);
            for (stringIndex = 0; stringIndex < stringLength; stringIndex++)
            {
                stringBuilder.Append("a");
            }
            return stringBuilder.ToString();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }
    }
}
