using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Cache for the taxonlistinformation
    /// </summary>
    [Serializable]
    public class TaxonInformationCache
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaxonInformationCache()
        {
            TaxonInformation = new Dictionary<int, TaxonListInformation>();
        }

        /// <summary>
        /// TaxonId/TaxonListInformation map
        /// </summary>
        public Dictionary<int, TaxonListInformation> TaxonInformation { get; set; }

        /// <summary>
        /// Version of the cache
        /// </summary>
        private const string CurrentVersion = "2015-05-28";

        /// <summary>
        /// Date and time when information was last read from data source.
        /// </summary>    
        public DateTime CachedDate { get; private set; }

        /// <summary>
        /// Cache handling version.
        /// </summary>    
        private string Version { get; set; }

        /// <summary>
        /// Test if cache has been initialized with correct data.
        /// </summary>
        /// <returns>True, if cache has been initialized with correct data.</returns>
        public bool IsOk()
        {
            return Version.IsNotEmpty() &&
                   (Version == CurrentVersion);
        }

        /// <summary>
        /// Init TaxonInformation cache.
        /// </summary>
        public void Init()
        {
            CachedDate = DateTime.Now;
            Version = CurrentVersion;
        }
    }
}
