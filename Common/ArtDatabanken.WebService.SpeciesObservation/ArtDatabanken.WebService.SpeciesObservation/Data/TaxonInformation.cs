using System;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Class that contains information related to taxon.
    /// This information is added to species observations
    /// that are retrieved from web service.
    /// </summary>
    public class TaxonInformation
    {
        /// <summary>
        /// Dyntaxa taxon id.
        /// </summary>
        public Int32 DyntaxaTaxonId { get; set; }

        /// <summary>
        /// May point to taxon that has replaced this taxon.
        /// </summary>
        public Int32 CurrentDyntaxaTaxonId { get; set; }

        /// <summary>
        /// The third word in the scientific name of an infra-specific taxon, following the name of the species.
        /// </summary>
        public String InfraspecificEpithet { get; set; }

        /// <summary>
        /// Indicates if the taxon is valid.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// The reference to the source in which the specific
        /// taxon concept circumscription is defined or implied -
        /// traditionally signified by the Latin "sensu" or "sec.".
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String NameAccordingTo { get; set; }

        /// <summary>
        /// An identifier for the source in which the specific
        /// taxon concept circumscription is defined or implied.
        /// See nameAccordingTo.
        /// </summary>
        public String NameAccordingToId { get; set; }

        /// <summary>
        /// A reference for the publication in which the
        /// scientificName was originally established under the rules
        /// of the associated nomenclaturalCode.
        /// </summary>
        public String NamePublishedIn { get; set; }

        /// <summary>
        /// An identifier for the publication in which the
        /// scientificName was originally established under the
        /// rules of the associated nomenclaturalCode.
        /// </summary>
        public String NamePublishedInId { get; set; }

        /// <summary>
        /// The four-digit year in which the scientificName
        /// was published.
        /// </summary>
        public String NamePublishedInYear { get; set; }

        /// <summary>
        /// The nomenclatural code (or codes in the case of an
        /// ambiregnal name) under which the scientificName is
        /// constructed.
        /// Recommended best practice is to use a controlled vocabulary.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String NomenclaturalCode { get; set; }

        /// <summary>
        /// The status related to the original publication of the
        /// name and its conformance to the relevant rules of
        /// nomenclature. It is based essentially on an algorithm
        /// according to the business rules of the code.
        /// It requires no taxonomic opinion.
        /// </summary>
        public String NomenclaturalStatus { get; set; }

        /// <summary>
        /// The taxon name, with authorship and date information
        /// if known, as it originally appeared when first established
        /// under the rules of the associated nomenclaturalCode.
        /// The basionym (botany) or basonym (bacteriology) of the
        /// scientificName or the senior/earlier homonym for replaced
        /// names.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String OriginalNameUsage { get; set; }

        /// <summary>
        /// An identifier for the name usage (documented meaning of
        /// the name according to a source) in which the terminal
        /// element of the scientificName was originally established
        /// under the rules of the associated nomenclaturalCode.
        /// </summary>
        public String OriginalNameUsageId { get; set; }

        /// <summary>
        /// Protection level that indicates how important it is
        /// to protected species observation of this taxon.
        /// </summary>
        public Int32 ProtectionLevel { get; set; }

        /// <summary>
        /// The full scientific name, with authorship and date
        /// information if known. When forming part of an
        /// Identification, this should be the name in lowest level
        /// taxonomic rank that can be determined.
        /// This term should not contain identification qualifications,
        /// which should instead be supplied in the
        /// IdentificationQualifier term.
        /// Currently scientific name without author is provided.
        /// </summary>
        public String ScientificName { get; set; }

        /// <summary>
        /// The authorship information for the scientificName
        /// formatted according to the conventions of the applicable
        /// nomenclaturalCode.
        /// </summary>
        public String ScientificNameAuthorship { get; set; }

        /// <summary>
        /// An identifier for the nomenclatural (not taxonomic)
        /// details of a scientific name.
        /// </summary>
        public String ScientificNameId { get; set; }

        /// <summary>
        /// The dyntaxaTaxonId a specie or of the parent specie in the case that 'this' is a subspecie.
        /// </summary>
        public Int32 SpeciesTaxonId { get; set; }

        /// <summary>
        /// The name of the first or species epithet of
        /// the scientificName.
        /// </summary>
        public String SpecificEpithet { get; set; }

        /// <summary>
        /// An identifier for the taxonomic concept to which the record
        /// refers - not for the nomenclatural details of a taxon.
        /// In SwedishSpeciesObservationSOAPService this property
        /// has the same value as property TaxonID. 
        /// GUID in Dyntaxa is used as value for this property.
        /// </summary>
        public String TaxonConceptId { get; set; }

        /// <summary>
        /// Status of the taxon concept.
        /// Examples of possible values are InvalidDueToSplit,
        /// InvalidDueToLump, InvalidDueToDelete, Unchanged,
        /// ValidAfterLump or ValidAfterSplit.
        /// </summary>
        public String TaxonConceptStatus { get; set; }

        /// <summary>
        /// The status of the use of the scientificName as a label
        /// for a taxon. Requires taxonomic opinion to define the
        /// scope of a taxon. Rules of priority then are used to
        /// define the taxonomic status of the nomenclature contained
        /// in that scope, combined with the experts opinion.
        /// It must be linked to a specific taxonomic reference that
        /// defines the concept.
        /// Recommended best practice is to use a controlled vocabulary.
        /// </summary>
        public String TaxonomicStatus { get; set; }

        /// <summary>
        /// The taxonomic rank of the most specific name in the
        /// scientificName. Recommended best practice is to use
        /// a controlled vocabulary.
        /// </summary>
        public String TaxonRank { get; set; }

        /// <summary>
        /// Comments or notes about the taxon or name.
        /// This property is currently not used.
        /// </summary>
        public String TaxonRemarks { get; set; }

        /// <summary>
        /// Sort order of taxon according to Dyntaxa.
        /// </summary>
        public Int32 TaxonSortOrder { get; set; }

        /// <summary>
        /// A common or vernacular name.
        /// </summary>
        public String VernacularName { get; set; }

        //// Species fact information.

        /// <summary>
        /// Action plan.
        /// </summary>
        public Boolean ActionPlan { get; set; }

        /// <summary>
        /// Action plan id.
        /// </summary>
        public Int32 ActionPlanId { get; set; }

        /// <summary>
        /// Disturbance radius.
        /// </summary>
        public Int32 DisturbanceRadius { get; set; }

        /// <summary>
        /// This property indicates whether a species has been
        /// classified as nature conservation relevant
        /// ('naturvårdsintressant' in swedish).
        /// The concept 'nature conservation relevant' must be defined
        /// before this property can be used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public Boolean ConservationRelevant { get; set; }

        /// <summary>
        /// This property indicates whether
        /// the species is included in Natura 2000.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public Boolean Natura2000 { get; set; }

        /// <summary>
        /// This property indicates whether the species 
        /// is protected by the law in Sweden.
        /// </summary>
        public Boolean ProtectedByLaw { get; set; }

        /// <summary>
        /// Redlist category for redlisted species. The property also
        /// contains information about which redlist that is referenced.
        /// Example value: CR (Sweden, 2010). Possible redlist values
        /// are DD (Data Deficient), EX (Extinct),
        /// RE (Regionally Extinct), CR (Critically Endangered),
        /// EN (Endangered), VU (Vulnerable), NT (Near Threatened).
        /// Not redlisted species has no value in this property.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String RedlistCategory { get; set; }

        /// <summary>
        /// This property contains information about the species
        /// immigration history.
        /// </summary>
        public String SwedishImmigrationHistory { get; set; }

        /// <summary>
        /// Information about the species occurrence in Sweden.
        /// For example information about if the species reproduce
        /// in sweden.
        /// </summary>
        public String SwedishOccurrence { get; set; }

        /// <summary>
        /// Common name of the organism group that observed species
        /// belongs to. Classification of species groups is the same as
        /// used in latest 'Red List of Swedish Species'.
        /// </summary>
        public String OrganismGroup { get; set; }

        //// Taxon tree related information.

        /// <summary>
        /// A list (concatenated and separated) of taxa names
        /// terminating at the rank immediately superior to the
        /// taxon referenced in the taxon record.
        /// Recommended best practice is to order the list
        /// starting with the highest rank and separating the names
        /// for each rank with a semi-colon (";").
        /// </summary>
        public String HigherClassification { get; set; }

        /// <summary>
        /// The full scientific name of the class in which
        /// the taxon is classified.
        /// </summary>
        public String Class { get; set; }

        /// <summary>
        /// The full scientific name of the family in which
        /// the taxon is classified.
        /// </summary>
        public String Family { get; set; }

        /// <summary>
        /// The full scientific name of the genus in which
        /// the taxon is classified.
        /// </summary>
        public String Genus { get; set; }

        /// <summary>
        /// The full scientific name of the kingdom in which the
        /// taxon is classified.
        /// </summary>
        public String Kingdom { get; set; }

        /// <summary>
        /// The full scientific name of the order in which
        /// the taxon is classified.
        /// </summary>
        public String Order { get; set; }

        /// <summary>
        /// The full scientific name of the phylum or division
        /// in which the taxon is classified.
        /// </summary>
        public String Phylum { get; set; }

        /// <summary>
        /// The full scientific name of the subgenus in which
        /// the taxon is classified. Values should include the
        /// genus to avoid homonym confusion.
        /// </summary>
        public String Subgenus { get; set; }

        /// <summary>
        /// The full scientific name of the subgenus in which
        /// the taxon is classified. Values should include the
        /// genus to avoid homonym confusion.
        /// </summary>
        public Int32 TaxonCategoryId { get; set; }

        /// <summary>
        /// Populate taxon information object with content from data reader.
        /// </summary>
        /// <param name="dataReader">Data source that has content.</param>
        public void LoadData(DataReader dataReader)
        {
            // Taxon information.
            CurrentDyntaxaTaxonId = dataReader.GetInt32(TaxonInformationData.CURRENT_DYNTAXA_TAXON_ID);
            DyntaxaTaxonId = dataReader.GetInt32(TaxonInformationData.DYNTAXA_TAXON_ID);
            IsValid = dataReader.GetByte(TaxonInformationData.IS_VALID) == 1;
            InfraspecificEpithet = dataReader.GetString(TaxonInformationData.INFRASPECIFIC_EPITHET);
            NameAccordingTo = dataReader.GetString(TaxonInformationData.NAME_ACCORDING_TO);
            NameAccordingToId = dataReader.GetString(TaxonInformationData.NAME_ACCORDING_TO_ID);
            NamePublishedIn = dataReader.GetString(TaxonInformationData.NAME_PUBLISHED_IN);
            NamePublishedInId = dataReader.GetString(TaxonInformationData.NAME_PUBLISHED_IN_ID);
            NamePublishedInYear = dataReader.GetString(TaxonInformationData.NAME_PUBLISHED_IN_YEAR);
            NomenclaturalCode = dataReader.GetString(TaxonInformationData.NOMENCLATURAL_CODE);
            NomenclaturalStatus = dataReader.GetString(TaxonInformationData.NOMENCLATURAL_STATUS);
            OriginalNameUsage = dataReader.GetString(TaxonInformationData.ORIGINAL_NAME_USAGE);
            OriginalNameUsageId = dataReader.GetString(TaxonInformationData.ORIGINAL_NAME_USAGE_ID);
            ScientificName = dataReader.GetString(TaxonInformationData.SCIENTIFIC_NAME);
            ScientificNameAuthorship = dataReader.GetString(TaxonInformationData.SCIENTIFIC_NAME_AUTHORSHIP);
            ScientificNameId = dataReader.GetString(TaxonInformationData.SCIENTIFIC_NAME_ID);
            SpeciesTaxonId = dataReader.IsDbNull(TaxonInformationData.SPECIES_TAXON_ID) ? -1 : dataReader.GetInt32(TaxonInformationData.SPECIES_TAXON_ID);
            SpecificEpithet = dataReader.GetString(TaxonInformationData.SPECIFIC_EPITHET);
            TaxonCategoryId = dataReader.GetInt32(TaxonInformationData.TAXON_CATEGORY_ID);
            TaxonConceptId = dataReader.GetString(TaxonInformationData.TAXON_CONCEPT_ID);
            TaxonConceptStatus = dataReader.GetString(TaxonInformationData.TAXON_CONCEPT_STATUS);
            TaxonomicStatus = dataReader.GetString(TaxonInformationData.TAXONOMIC_STATUS);
            TaxonRank = dataReader.GetString(TaxonInformationData.TAXON_RANK);
            TaxonRemarks = dataReader.GetString(TaxonInformationData.TAXON_REMARK);
            TaxonSortOrder = dataReader.GetInt32(TaxonInformationData.TAXON_SORT_ORDER);
            VernacularName = dataReader.GetString(TaxonInformationData.VERNACULAR_NAME);

            // Species fact information.
            ActionPlanId = dataReader.GetByte(TaxonInformationData.ACTION_PLAN, 0);
            ActionPlan = (Settings.Default.MinActiveActionPlanId <= ActionPlanId) &&
                         (ActionPlanId <= Settings.Default.MaxActiveActionPlanId);
            ConservationRelevant = dataReader.GetByte(TaxonInformationData.CONSERVATION_RELEVANT, 0) == 1;
            Natura2000 = dataReader.GetByte(TaxonInformationData.NATURA_2000, 0) == 1;
            OrganismGroup = dataReader.GetString(TaxonInformationData.ORGANISM_GROUP);
            ProtectedByLaw = dataReader.GetByte(TaxonInformationData.PROTECTED_BY_LAW, 0) == 1;
            ProtectionLevel = dataReader.GetInt32(TaxonInformationData.PROTECTION_LEVEL);
            RedlistCategory = dataReader.GetString(TaxonInformationData.REDLIST_CATEGORY);
            SwedishImmigrationHistory = dataReader.GetString(TaxonInformationData.SWEDISH_IMMIGRATION_HISTORY);
            SwedishOccurrence = dataReader.GetString(TaxonInformationData.SWEDISH_OCCURRENCE);

            // Taxon tree related information.
            HigherClassification = dataReader.GetString(TaxonInformationData.HIGHER_CLASSIFICATION);
            Class = dataReader.GetString(TaxonInformationData.CLASS);
            Family = dataReader.GetString(TaxonInformationData.FAMILY);
            Genus = dataReader.GetString(TaxonInformationData.GENUS);
            Kingdom = dataReader.GetString(TaxonInformationData.KINGDOM);
            Order = dataReader.GetString(TaxonInformationData.ORDER);
            Phylum = dataReader.GetString(TaxonInformationData.PHYLUM);
            Subgenus = dataReader.GetString(TaxonInformationData.SUBGENUS);
        }
    }
}
