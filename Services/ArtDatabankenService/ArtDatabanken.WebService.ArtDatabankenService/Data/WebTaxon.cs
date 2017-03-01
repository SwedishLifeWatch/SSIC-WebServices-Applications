using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Is used to decide which information that should be
    /// returned together with the taxon information.
    /// </summary>
    [DataContract]
    public enum TaxonInformationType
    {
        /// <summary>
        /// Basic information about taxon.
        /// </summary>
        [EnumMember]
        Basic,
        /// <summary>
        /// Information about taxon that is shown in
        /// the application PrintObs.
        /// </summary>
        [EnumMember]
        PrintObs
    }

    /// <summary>
    /// Holds information about how user selected taxa should be used.
    /// </summary>
    public enum UserSelectedTaxonUsage
    {
        /// <summary>
        /// Taxa are used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// Taxa are used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    /// Contains information about a taxon.
    /// </summary>
    [DataContract]
    public class WebTaxon : WebData
    {
        /// <summary>
        /// Taxon that is used in situations where a 
        /// taxon is needed but it is unknown which taxon
        /// data belongs to.
        /// </summary>
        public const Int32 UNKNOWN_TAXON_ID = 0;

        /// <summary>
        /// Create a WebTaxon instance.
        /// </summary>
        public WebTaxon()
        {
        }

        /// <summary>
        /// Create a WebTaxon instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebTaxon(DataReader dataReader)
        {
            String authorDataString, idDataString;

            if (dataReader.HasColumn(TaxonData.TABLE_NAME + TaxonData.ID))
            {
                idDataString = TaxonData.TABLE_NAME + TaxonData.ID;
            }
            else
            {
                idDataString = TaxonData.ID;
            }
            Id = dataReader.GetInt32(idDataString);
            SortOrder = dataReader.GetInt32(TaxonData.SORT_ORDER);
            TaxonTypeId = dataReader.GetInt32(TaxonData.TAXON_TYPE_ID);
            if (dataReader.HasColumn(TaxonData.SCIENTIFIC_NAME + TaxonData.AUTHOR))
            {
                authorDataString = TaxonData.SCIENTIFIC_NAME + TaxonData.AUTHOR;
            }
            else
            {
                authorDataString = TaxonData.AUTHOR;
            }
            Author = dataReader.GetString(authorDataString);
            CommonName = dataReader.GetString(TaxonData.COMMON_NAME);
            ScientificName = dataReader.GetString(TaxonData.SCIENTIFIC_NAME);
            if (dataReader.HasColumn(TaxonData.TAXON_INFORMATION_TYPE))
            {
                TaxonInformationType = (TaxonInformationType)(Enum.Parse(typeof(TaxonInformationType), dataReader.GetString(TaxonData.TAXON_INFORMATION_TYPE)));
            }
            else
            {
                TaxonInformationType = TaxonInformationType.PrintObs;
            }
            LoadData(dataReader);
        }

        /// <summary>
        /// Author of the scientific name for this taxon.
        /// </summary>
        [DataMember]
        public String Author
        { get; set; }

        /// <summary>
        /// Common name for this taxon.
        /// </summary>
        [DataMember]
        public String CommonName
        { get; set; }

        /// <summary>
        /// Id for this taxon.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Scientific name for this taxon.
        /// </summary>
        [DataMember]
        public String ScientificName
        { get; set; }

        /// <summary>
        /// Sort order for this taxon.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Type of taxon information that this object contains.
        /// </summary>
        [DataMember]
        public TaxonInformationType TaxonInformationType
        { get; set; }

        /// <summary>
        /// Id for type of taxon.
        /// </summary>
        [DataMember]
        public Int32 TaxonTypeId
        { get; set; }
    }
}
