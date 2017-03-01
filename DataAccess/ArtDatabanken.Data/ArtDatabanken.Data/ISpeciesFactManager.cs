using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles species fact related information.
    /// </summary>
    public interface ISpeciesFactManager
    {
        /// <summary>
        /// This property is used to retrieve or update species fact information.
        /// </summary>
        ISpeciesFactDataSource DataSource { get; set; }

        /// <summary>
        /// Get default species fact quality.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Default species fact quality.</returns>
        ISpeciesFactQuality GetDefaultSpeciesFactQuality(IUserContext userContext);

        /// <summary>
        /// Creates a species fact instance with data from data source.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="taxon">Taxon of the species fact.</param>
        /// <param name="individualCategory">Individual Category of the species fact.</param>
        /// <param name="factor">Factor of the species fact.</param>
        /// <param name="host">Host taxon associated with the species fact.</param>
        /// <param name="period">Period of the species fact.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="quality">Quality of the species fact.</param>
        /// <param name="reference">Reference of the species fact.</param>
        /// <param name="modifiedBy">Full name of the update user of the species fact.</param>
        /// <param name="modifiedDate">Update date of the species fact.</param>
        ISpeciesFact GetSpeciesFact(IUserContext userContext,
                                    Int32 id,
                                    ITaxon taxon,
                                    IIndividualCategory individualCategory,
                                    IFactor factor,
                                    ITaxon host,
                                    IPeriod period,
                                    Double fieldValue1,
                                    Boolean hasFieldValue1,
                                    Double fieldValue2,
                                    Boolean hasFieldValue2,
                                    Double fieldValue3,
                                    Boolean hasFieldValue3,
                                    String fieldValue4,
                                    Boolean hasFieldValue4,
                                    String fieldValue5,
                                    Boolean hasFieldValue5,
                                    ISpeciesFactQuality quality,
                                    IReference reference,
                                    String modifiedBy,
                                    DateTime modifiedDate);

        /// <summary>
        /// Get an empty (only default data) SpeciesFact instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        /// <returns>A SpeciesFact instance.</returns>
        ISpeciesFact GetSpeciesFact(IUserContext userContext,
                                    ITaxon taxon,
                                    IIndividualCategory individualCategory,
                                    IFactor factor,
                                    ITaxon host,
                                    IPeriod period);

        /// <summary>
        /// Method that generates an unique identifier for a species fact.
        /// </summary>
        /// <param name="taxonId">Id of the taxon that is related to the species fact.</param>
        /// <param name="individualCategoryId">Id of the individual category that is related to the species fact.</param>
        /// <param name="factorId">Id of the factor that is related to the species fact.</param>
        /// <param name="hasHostId">Indicates if hostId has a value.</param>
        /// <param name="hostId">Id of the host taxon that is related to the species fact.</param>
        /// <param name="hasPeriodId">Indicates if periodId has a value.</param>
        /// <param name="periodId">Id of the period that is related to the species fact.</param>
        /// <returns>Identifier for a species fact.</returns>
        String GetSpeciesFactIdentifier(Int32 taxonId,
                                        Int32 individualCategoryId,
                                        Int32 factorId,
                                        Boolean hasHostId,
                                        Int32 hostId,
                                        Boolean hasPeriodId,
                                        Int32 periodId);

        /// <summary>
        /// Method that generates an unique identifier for a species fact.
        /// </summary>
        /// <param name="taxon">Taxon that is related to the species fact.</param>
        /// <param name="individualCategory">Individual category that is related to the species fact.</param>
        /// <param name="factor">Factor that is related to the species fact.</param>
        /// <param name="host">Host taxon that is related to the species fact.</param>
        /// <param name="period">Period that is related to the species fact.</param>
        /// <returns>Identifier for a species fact.</returns>
        String GetSpeciesFactIdentifier(ITaxon taxon,
                                        IIndividualCategory individualCategory,
                                        IFactor factor,
                                        ITaxon host,
                                        IPeriod period);

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All species fact qualities.</returns>
        SpeciesFactQualityList GetSpeciesFactQualities(IUserContext userContext);

        /// <summary>
        /// Get species fact quality with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactQualityId">Species fact quality id.</param>
        /// <returns>Species fact quality with specified id.</returns>
        ISpeciesFactQuality GetSpeciesFactQuality(IUserContext userContext,
                                                  Int32 speciesFactQualityId);

        /// <summary>
        /// Get species fact quality with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactQualityId">Species fact quality id.</param>
        /// <returns>Species fact quality with specified id.</returns>
        ISpeciesFactQuality GetSpeciesFactQuality(IUserContext userContext,
                                                  SpeciesFactQualityId speciesFactQualityId);

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species facts.</returns>
        SpeciesFactList GetSpeciesFacts(IUserContext userContext, List<Int32> speciesFactIds);

        /// <summary>
        /// Get species facts with specified identifiers.
        /// Only existing species facts are returned,
        /// e.g. species fact identifiers that does not
        /// match existing species fact does not affect
        /// the returned species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactIdentifiers">
        /// Species facts identifiers. E.g. ISpeciesFact
        /// instances where id for requested combination of
        /// factor, host, individual category, period and taxon
        /// has been set.
        /// Host id is only used together with taxonomic factors.
        /// Period id is only used together with periodic factors.
        /// </param>
        /// <returns>
        /// Existing species facts among the
        /// requested species facts.
        /// </returns>
        SpeciesFactList GetSpeciesFacts(IUserContext userContext,
                                        SpeciesFactList speciesFactIdentifiers);

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        SpeciesFactList GetSpeciesFacts(IUserContext userContext,
                                        ISpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        Int32 GetTaxaCount(IUserContext userContext, ISpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        TaxonList GetTaxa(IUserContext userContext, ISpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Update species facts in RAM memory
        /// with latest information from data source.
        /// This method works on species facts both with and without id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFacts">Species facts to update.</param>
        void UpdateSpeciesFacts(IUserContext userContext,
                                SpeciesFactList speciesFacts);

        /// <summary>
        /// Update values for species facts.
        /// This method updates information in data source but it
        /// does not update the species fact objects in the client. 
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFacts">Species facts to set.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        void UpdateSpeciesFacts(IUserContext userContext,
                                SpeciesFactList speciesFacts,
                                IReference defaultReference);
    }
}