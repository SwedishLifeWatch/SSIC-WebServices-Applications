using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains taxon information about a species
    /// observation in Darwin Core 1.5 compatible format.
    /// Further information about the properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public class DarwinCoreTaxon : IDarwinCoreTaxon
    {
        /// <summary>
        /// Darwin Core term name: acceptedNameUsage.
        /// The full name, with authorship and date information
        /// if known, of the currently valid (zoological) or
        /// accepted (botanical) taxon.
        /// This property is currently not used.
        /// </summary>
        public String AcceptedNameUsage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: acceptedNameUsageID.
        /// An identifier for the name usage (documented meaning of
        /// the name according to a source) of the currently valid
        /// (zoological) or accepted (botanical) taxon.
        /// This property is currently not used.
        /// </summary>
        public String AcceptedNameUsageID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: class.
        /// The full scientific name of the class in which
        /// the taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Class
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Taxon id (not GUID) value in Dyntaxa.
        /// </summary>
        public Int32 DyntaxaTaxonID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: family.
        /// The full scientific name of the family in which
        /// the taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Family
        { get; set; }

        /// <summary>
        /// Darwin Core term name: genus.
        /// The full scientific name of the genus in which
        /// the taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Genus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: higherClassification.
        /// A list (concatenated and separated) of taxa names
        /// terminating at the rank immediately superior to the
        /// taxon referenced in the taxon record.
        /// Recommended best practice is to order the list
        /// starting with the highest rank and separating the names
        /// for each rank with a semi-colon (";").
        /// This property is currently not used.
        /// </summary>
        public String HigherClassification
        { get; set; }

        /// <summary>
        /// Darwin Core term name: infraspecificEpithet.
        /// The name of the lowest or terminal infraspecific epithet
        /// of the scientificName, excluding any rank designation.
        /// This property is currently not used.
        /// </summary>
        public String InfraspecificEpithet
        { get; set; }

        /// <summary>
        /// Darwin Core term name: kingdom.
        /// The full scientific name of the kingdom in which the
        /// taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Kingdom
        { get; set; }

        /// <summary>
        /// Darwin Core term name: nameAccordingTo.
        /// The reference to the source in which the specific
        /// taxon concept circumscription is defined or implied -
        /// traditionally signified by the Latin "sensu" or "sec."
        /// (from secundum, meaning "according to").
        /// For taxa that result from identifications, a reference
        /// to the keys, monographs, experts and other sources should
        /// be given.
        /// This property is currently not used.
        /// </summary>
        public String NameAccordingTo
        { get; set; }

        /// <summary>
        /// Darwin Core term name: nameAccordingToID.
        /// An identifier for the source in which the specific
        /// taxon concept circumscription is defined or implied.
        /// See nameAccordingTo.
        /// This property is currently not used.
        /// </summary>
        public String NameAccordingToID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: namePublishedIn.
        /// A reference for the publication in which the
        /// scientificName was originally established under the rules
        /// of the associated nomenclaturalCode.
        /// This property is currently not used.
        /// </summary>
        public String NamePublishedIn
        { get; set; }

        /// <summary>
        /// Darwin Core term name: namePublishedInID.
        /// An identifier for the publication in which the
        /// scientificName was originally established under the
        /// rules of the associated nomenclaturalCode.
        /// This property is currently not used.
        /// </summary>
        public String NamePublishedInID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: namePublishedInYear.
        /// The four-digit year in which the scientificName
        /// was published.
        /// This property is currently not used.
        /// </summary>
        public String NamePublishedInYear
        { get; set; }

        /// <summary>
        /// Darwin Core term name: nomenclaturalCode.
        /// The nomenclatural code (or codes in the case of an
        /// ambiregnal name) under which the scientificName is
        /// constructed.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        public String NomenclaturalCode
        { get; set; }

        /// <summary>
        /// Darwin Core term name: nomenclaturalStatus.
        /// The status related to the original publication of the
        /// name and its conformance to the relevant rules of
        /// nomenclature. It is based essentially on an algorithm
        /// according to the business rules of the code.
        /// It requires no taxonomic opinion.
        /// This property is currently not used.
        /// </summary>
        public String NomenclaturalStatus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: order.
        /// The full scientific name of the order in which
        /// the taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Order
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Common name of the organism group that observed species
        /// belongs to. Classification of species groups is the same as
        /// used in latest 'Red List of Swedish Species'.
        /// </summary>
        public String OrganismGroup
        { get; set; }

        /// <summary>
        /// Darwin Core term name: originalNameUsage.
        /// The taxon name, with authorship and date information
        /// if known, as it originally appeared when first established
        /// under the rules of the associated nomenclaturalCode.
        /// The basionym (botany) or basonym (bacteriology) of the
        /// scientificName or the senior/earlier homonym for replaced
        /// names.
        /// This property is currently not used.
        /// </summary>
        public String OriginalNameUsage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: originalNameUsageID.
        /// An identifier for the name usage (documented meaning of
        /// the name according to a source) in which the terminal
        /// element of the scientificName was originally established
        /// under the rules of the associated nomenclaturalCode.
        /// This property is currently not used.
        /// </summary>
        public String OriginalNameUsageID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: parentNameUsage.
        /// The full name, with authorship and date information
        /// if known, of the direct, most proximate higher-rank
        /// parent taxon (in a classification) of the most specific
        /// element of the scientificName.
        /// This property is currently not used.
        /// </summary>
        public String ParentNameUsage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: parentNameUsageID.
        /// An identifier for the name usage (documented meaning
        /// of the name according to a source) of the direct,
        /// most proximate higher-rank parent taxon
        /// (in a classification) of the most specific
        /// element of the scientificName.
        /// This property is currently not used.
        /// </summary>
        public String ParentNameUsageID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: phylum.
        /// The full scientific name of the phylum or division
        /// in which the taxon is classified.
        /// This property is currently not used.
        /// </summary>
        public String Phylum
        { get; set; }

        /// <summary>
        /// Darwin Core term name: scientificName.
        /// The full scientific name, with authorship and date
        /// information if known. When forming part of an
        /// Identification, this should be the name in lowest level
        /// taxonomic rank that can be determined.
        /// This term should not contain identification qualifications,
        /// which should instead be supplied in the
        /// IdentificationQualifier term.
        /// Currently scientific name without author is provided.
        /// </summary>
        public String ScientificName
        { get; set; }

        /// <summary>
        /// Darwin Core term name: scientificNameAuthorship.
        /// The authorship information for the scientificName
        /// formatted according to the conventions of the applicable
        /// nomenclaturalCode.
        /// This property is currently not used.
        /// </summary>
        public String ScientificNameAuthorship
        { get; set; }

        /// <summary>
        /// Darwin Core term name: scientificNameID.
        /// An identifier for the nomenclatural (not taxonomic)
        /// details of a scientific name.
        /// This property is currently not used.
        /// </summary>
        public String ScientificNameID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: specificEpithet.
        /// The name of the first or species epithet of
        /// the scientificName.
        /// This property is currently not used.
        /// </summary>
        public String SpecificEpithet
        { get; set; }

        /// <summary>
        /// Darwin Core term name: subgenus.
        /// The full scientific name of the subgenus in which
        /// the taxon is classified. Values should include the
        /// genus to avoid homonym confusion.
        /// This property is currently not used.
        /// </summary>
        public String Subgenus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: taxonConceptID.
        /// An identifier for the taxonomic concept to which the record
        /// refers - not for the nomenclatural details of a taxon.
        /// In SwedishSpeciesObservationSOAPService this property
        /// has the same value as property TaxonID. 
        /// GUID in Dyntaxa is used as value for this property.
        /// This property is currently not used.
        /// </summary>
        public String TaxonConceptID
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Status of the taxon concept.
        /// Examples of possible values are Valid, Invalid, Lumped
        /// and Splited,
        /// </summary>
        public String TaxonConceptStatus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: taxonID.
        /// An identifier for the set of taxon information
        /// (data associated with the Taxon class). May be a global
        /// unique identifier or an identifier specific to the data set.
        /// In SwedishSpeciesObservationSOAPService this property
        /// has the same value as property TaxonConceptID. 
        /// GUID in Dyntaxa is used as value for this property.
        /// </summary>
        public String TaxonID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: taxonomicStatus.
        /// The status of the use of the scientificName as a label
        /// for a taxon. Requires taxonomic opinion to define the
        /// scope of a taxon. Rules of priority then are used to
        /// define the taxonomic status of the nomenclature contained
        /// in that scope, combined with the experts opinion.
        /// It must be linked to a specific taxonomic reference that
        /// defines the concept.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        public String TaxonomicStatus
        { get; set; }

        /// <summary>
        /// Darwin Core term name: taxonRank.
        /// The taxonomic rank of the most specific name in the
        /// scientificName. Recommended best practice is to use
        /// a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        public String TaxonRank
        { get; set; }

        /// <summary>
        /// Darwin Core term name: taxonRemarks.
        /// Comments or notes about the taxon or name.
        /// This property is currently not used.
        /// </summary>
        public String TaxonRemarks
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Sort order of taxon according to Dyntaxa.
        /// This property is currently not used.
        /// </summary>
        public Int32 TaxonSortOrder
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// taxon. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        public String TaxonURL
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// ScientificName as it appears in the original record.
        /// This property is currently not used.
        /// </summary>
        public String VerbatimScientificName
        { get; set; }

        /// <summary>
        /// Darwin Core term name: verbatimTaxonRank.
        /// The taxonomic rank of the most specific name in the
        /// scientificName as it appears in the original record.
        /// This property is currently not used.
        /// </summary>
        public String VerbatimTaxonRank
        { get; set; }

        /// <summary>
        /// Darwin Core term name: vernacularName.
        /// A common or vernacular name.
        /// </summary>
        public String VernacularName
        { get; set; }

    }
}
