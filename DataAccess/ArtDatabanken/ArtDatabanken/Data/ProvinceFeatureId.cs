using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of Swedish provinces
    /// </summary>
    public class ProvinceFeatureId
    {
        /// <summary>
        /// Special province Lappland that have province parts.
        /// This province exists in the table Province in database SwedishSpeciesObservationResources
        /// </summary>
        public const string Lappland = "100";

        /// <summary>
        /// Lule lappmark, province part of Lappland
        /// </summary>
        public const string LuleLappmark = "25";

        /// <summary>
        /// Lycksele lappmark, province part of Lappland
        /// </summary>
        public const string LyckseleLappmark = "26";

        /// <summary>
        /// Pite lappmark, province part of Lappland
        /// </summary>
        public const string PiteLappmark = "27";

        /// <summary>
        /// Torne lappmark, province part of Lappland
        /// </summary>
        public const string TorneLappmark = "28";

        /// <summary>
        /// Åsele lappmark, province part of Lappland
        /// </summary>
        public const string AseleLappmark = "29";
    }
}
