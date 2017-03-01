using System;

namespace ArtDatabanken.Data.DataSource
{

    /// <summary>
    /// Definition of the PESINameServiceDataSource interface.
    /// This interface is used to retrieve names from PESI Webservice
    /// 
    /// PESI = Pan-European Species directories Infrastructure 
    /// PESI provides standardised and authoritative taxonomic information
    /// </summary>
    public interface IPesiNameDataSource
    {
        /// <summary>
        /// Get PESI GUID by vernacular name.
        /// </summary>
        /// <param name="vernacularName">Vernacular name.</param>
        /// <returns>PESI GUID by vernacular name.</returns>       
        String GetPesiGuidByVernacularName(String vernacularName);

        /// <summary>
        /// Get PESI GUID by scientific name.
        /// </summary>
        /// <param name="scientificName">Information about the client that makes this web service call.</param>
        /// <returns>PESI GUID by scientific name.</returns>       
        String GetPesiGuidByScientificName(String scientificName);
    }
}
