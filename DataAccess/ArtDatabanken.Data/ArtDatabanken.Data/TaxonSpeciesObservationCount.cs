using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents the number of observed species related to a taxon.
    /// </summary>
    public class TaxonSpeciesObservationCount : ITaxonSpeciesObservationCount
    {

        /// <summary>
        /// Alert status taxon.
        /// A classification of the need for communication of
        /// problems related to the taxon status and recognition.
        /// Might be used to decide if description
        /// text is displayed as warning.
        /// </summary>
        public ITaxonAlertStatus AlertStatus { get; set; }

        /// <summary>
        /// Author of the recommended scientific name.
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        public ITaxonCategory Category { get; set; }

        /// <summary>
        /// ChangeStatus
        /// Indicates taxons lump-split status
        /// </summary>
        public ITaxonChangeStatus ChangeStatus { get; set; }

        /// <summary>
        /// Recommended common name.
        /// Not all taxa has a recommended common name.
        /// </summary>
        public String CommonName { get; set; }

        /// <summary>
        /// User that created the record.
        ///  Mandatory ie always required.
        /// </summary> 
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date record was created.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Unique identification of a taxon.
        /// Mandatory ie always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if this taxon is in a checked out revision
        /// and may be updated.
        /// </summary>
        public Boolean IsInRevision { get; set; }

        /// <summary>
        /// Gets or sets IsPublished
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// IsValid
        /// true - taxon is valid.
        /// false - taxon is NOT valid.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// Taxon was modified by the user with this id.
        /// Set by database.
        /// </summary>
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Person for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public String ModifiedByPerson { get; set; }

        /// <summary>
        /// Date taxon was modified.
        /// Set by database revision with taxon in is checked in
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        public String ScientificName { get; set; }

        /// <summary>
        /// SortOrder
        /// Sorting order for this taxon.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidToDate { get; set; }
        /// <summary>
        /// Number of observed species.
        /// </summary>

        public Int32 SpeciesObservationCount { get; set; }
    }
}