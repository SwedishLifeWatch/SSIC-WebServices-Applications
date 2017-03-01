using System;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.PESINameService
{
    /// <summary>
    /// This class is used to retrieve taxon
    /// information from PESI webservice.
    /// </summary>
    public class PesiNameDataSource : IPesiNameDataSource
    {
        /// <summary>
        /// Get PESI GUID by vernacular name.
        /// </summary>
        /// <param name="vernacularName">Vernacular name.</param>
        /// <returns>PESI GUID by vernacular name.</returns>       
        public String GetPesiGuidByVernacularName(String vernacularName)
        {
            return WebServiceProxy.PESINameService.GetPesiGuidByVernacularName(vernacularName);
        }

        /// <summary>
        /// Get PESI GUID by scientific name.
        /// </summary>
        /// <param name="scientificName">Information about the client that makes this web service call.</param>
        /// <returns>PESI GUID by scientific name.</returns>       
        public String GetPesiGuidByScientificName(String scientificName)
        {
            return WebServiceProxy.PESINameService.GetPesiGuidByScientificName(scientificName);
        }

        /// <summary>
        /// Set PESINameService as data source in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            var PESIDataSource = new PesiNameDataSource();
            CoreData.TaxonManager.PesiNameDataSource = PESIDataSource;
        }
    }
}
