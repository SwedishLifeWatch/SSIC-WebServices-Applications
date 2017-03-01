using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a taxon name.
    /// </summary>
    [DataContract]
    public class WebTaxonName : WebData
    {
        /// <summary>
        /// Create a WebTaxonName instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebTaxonName(DataReader dataReader)
        {
            Id = dataReader.GetInt32(TaxonNameData.ID);
            IsRecommended = dataReader.GetBoolean(TaxonNameData.IS_RECOMMENDED);
            TaxonNameTypeId = dataReader.GetInt32(TaxonNameData.TAXON_NAME_TYPE_ID);
            TaxonNameUseTypeId = dataReader.GetInt32(TaxonNameData.TAXON_NAME_USE_TYPE_ID);
            Author = dataReader.GetString(TaxonNameData.AUTHOR);
            Name = dataReader.GetString(TaxonNameData.NAME);
            Taxon = new WebTaxon(dataReader);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Author of this taxon name.
        /// </summary>
        [DataMember]
        public String Author
        { get; set; }

        /// <summary>
        /// Id for this taxon name.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Indicates if this name is the recommended namn.
        /// </summary>
        [DataMember]
        public Boolean IsRecommended
        { get; set; }

        /// <summary>
        /// Name for this taxon name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Taxon that this name belongs to.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon
        { get; set; }

        /// <summary>
        /// Id for type of name.
        /// </summary>
        [DataMember]
        public Int32 TaxonNameTypeId
        { get; set; }

        /// <summary>
        /// Id for use of name.
        /// </summary>
        [DataMember]
        public Int32 TaxonNameUseTypeId
        { get; set; }
    }
}
