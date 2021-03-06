﻿using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains identification information about a 
    /// species observation when a flexible species observation format is required. 
    /// This interface also includes all properties available in Darwin Core 1.5 
    /// se interface IDarwinCoreIdentification.
    /// Further information about the Darwin Core 1.5 properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public interface ISpeciesObservationIdentification
    {
        /// <summary>
        /// Darwin Core term name: dateIdentified.
        /// The date on which the subject was identified as
        /// representing the Taxon. Recommended best practice is
        /// to use an encoding scheme, such as ISO 8601:2004(E).
        /// This property is currently not used.
        /// </summary>
        String DateIdentified
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identificationID.
        /// An identifier for the Identification (the body of
        /// information associated with the assignment of a scientific
        /// name). May be a global unique identifier or an identifier
        /// specific to the data set.
        /// This property is currently not used.
        /// </summary>
        String IdentificationID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identificationQualifier.
        /// A brief phrase or a standard term ("cf.", "aff.") to
        /// express the determiner's doubts about the Identification.
        /// </summary>
        String IdentificationQualifier
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identificationReferences.
        /// A list (concatenated and separated) of references
        /// (publication, global unique identifier, URI) used in
        /// the Identification.
        /// This property is currently not used.
        /// </summary>
        String IdentificationReferences
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identificationRemarks.
        /// Comments or notes about the Identification.
        /// Contains for example information about that
        /// the observer is uncertain about which species
        /// that has been observed.
        /// </summary>
        String IdentificationRemarks
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identificationVerificationStatus.
        /// A categorical indicator of the extent to which the taxonomic
        /// identification has been verified to be correct.
        /// Recommended best practice is to use a controlled vocabulary
        /// such as that used in HISPID/ABCD.
        /// This property is currently not used.
        /// </summary>
        String IdentificationVerificationStatus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: identifiedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations who assigned the Taxon to the
        /// subject.
        /// </summary>
        String IdentifiedBy
        { get; set; }

        /// <summary>
        /// Darwin Core term name: typeStatus.
        /// A list (concatenated and separated) of nomenclatural
        /// types (type status, typified scientific name, publication)
        /// applied to the subject.
        /// This property is currently not used.
        /// </summary>
        String TypeStatus
        { get; set; }

        /// <summary>
        /// Indicates if the species observer himself is
        /// uncertain about the taxon determination.
        /// </summary>
        Boolean? UncertainDetermination
        { get; set; }
    }
}
