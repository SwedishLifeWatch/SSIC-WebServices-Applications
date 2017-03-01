using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Number of observed species for a taxon.
    /// </summary>
    public interface ITaxonSpeciesObservationCount : IDataId32
    {
        /// <summary>
        /// Alert status for this taxon.
        /// A classification of the need for communication of
        /// problems related to the taxon status and recognition.
        /// Might be used to decide if description
        /// text is displayed as warning.
        /// </summary>
        ITaxonAlertStatus AlertStatus { get; set; }

        /// <summary>
        /// Author of the recommended scientific name.
        /// </summary>
        String Author { get; set; }

        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        ITaxonCategory Category { get; set; }

        /// <summary>
        /// Change status for this taxon.
        /// Indicates if this taxon has been lumped, splited or deleted.
        /// </summary>
        ITaxonChangeStatus ChangeStatus { get; set; }

        /// <summary>
        /// Recommended common name.
        /// Not all taxa has a recommended common name.
        /// </summary>
        String CommonName { get; set; }

        /// <summary>
        /// Id of user that created the taxon.
        /// Mandatory ie always required.
        /// </summary> 
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// The taxon was created at this date.
        /// Mandatory ie always required.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Indicates if this taxon is in a checked out revision
        /// and may be updated.
        /// </summary>
        Boolean IsInRevision { get; set; }

        /// <summary>
        /// Indicates if the information in this taxon instance
        /// has been published or not.
        /// </summary>
        Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates if this taxon is valid or not.
        /// </summary>
        Boolean IsValid { get; set; }

        /// <summary>
        /// Taxon was modified by the user with this id.
        /// </summary>
        Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Name of the person who last modified this taxon.
        /// Not required ie could be null.
        /// </summary>
        String ModifiedByPerson { get; set; }

        /// <summary>
        /// Date taxon was modified.
        /// Set by database revision with taxon in is checked in
        /// </summary>
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        String ScientificName { get; set; }

        /// <summary>
        /// Sorting order for this taxon.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidToDate { get; set; }

        /// <summary>
        /// Number of observed species
        /// </summary>
        Int32 SpeciesObservationCount { get; set; }
    }
}