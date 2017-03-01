using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// This interface is used to handle species fact quality related information.
    /// </summary>
    public interface ISpeciesFactDataSource : IDataSource
    {
        /// <summary>
        /// Create species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        void CreateSpeciesFacts(IUserContext userContext,
                                SpeciesFactList createSpeciesFacts,
                                IReference defaultReference);

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        void DeleteSpeciesFacts(IUserContext userContext,
                                SpeciesFactList deleteSpeciesFacts);

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All species fact qualities.</returns>
        SpeciesFactQualityList GetSpeciesFactQualities(IUserContext userContext);

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species fact information.</returns>
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
        SpeciesFactList GetSpeciesFacts(IUserContext userContext, SpeciesFactList speciesFactIdentifiers);

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
        /// Get taxa count of taxa that matches fact search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches fact search criteria.</returns>
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
        /// Update species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        void UpdateSpeciesFacts(IUserContext userContext,
                                SpeciesFactList updateSpeciesFacts);
    }
}