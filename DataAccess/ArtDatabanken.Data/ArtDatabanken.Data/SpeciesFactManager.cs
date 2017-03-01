using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles species fact related information.
    /// </summary>
    public class SpeciesFactManager : ISpeciesFactManager
    {
        /// <summary>
        /// This property is used to retrieve or update species fact information.
        /// </summary>
        public ISpeciesFactDataSource DataSource { get; set; }

        /// <summary>
        /// Get default species fact quality.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Default species fact quality.</returns>
        public virtual ISpeciesFactQuality GetDefaultSpeciesFactQuality(IUserContext userContext)
        {
            return GetSpeciesFactQuality(userContext,
                                         SpeciesFactQualityId.Acceptable);
        }

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
        public virtual ISpeciesFact GetSpeciesFact(IUserContext userContext,
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
                                                   DateTime modifiedDate)
        {
            ISpeciesFact speciesFact;

            switch (factor.Id)
            {
                case (Int32)(FactorId.RedListCategoryAutomatic):
                    speciesFact = new SpeciesFactRedListCategory(userContext,
                                                                 id,
                                                                 taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period,
                                                                 fieldValue1,
                                                                 hasFieldValue1,
                                                                 fieldValue2,
                                                                 hasFieldValue2,
                                                                 fieldValue3,
                                                                 hasFieldValue3,
                                                                 fieldValue4,
                                                                 hasFieldValue4,
                                                                 fieldValue5,
                                                                 hasFieldValue5,
                                                                 quality,
                                                                 reference,
                                                                 modifiedBy,
                                                                 modifiedDate);
                    break;

                case (Int32)(FactorId.RedListCriteriaAutomatic):
                    speciesFact = new SpeciesFactRedListCriteria(userContext,
                                                                 id,
                                                                 taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period,
                                                                 fieldValue1,
                                                                 hasFieldValue1,
                                                                 fieldValue2,
                                                                 hasFieldValue2,
                                                                 fieldValue3,
                                                                 hasFieldValue3,
                                                                 fieldValue4,
                                                                 hasFieldValue4,
                                                                 fieldValue5,
                                                                 hasFieldValue5,
                                                                 quality,
                                                                 reference,
                                                                 modifiedBy,
                                                                 modifiedDate);
                    break;

                case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                    speciesFact = new SpeciesFactRedListCriteriaDocumentation(userContext,
                                                                              id,
                                                                              taxon,
                                                                              individualCategory,
                                                                              factor,
                                                                              host,
                                                                              period,
                                                                              fieldValue1,
                                                                              hasFieldValue1,
                                                                              fieldValue2,
                                                                              hasFieldValue2,
                                                                              fieldValue3,
                                                                              hasFieldValue3,
                                                                              fieldValue4,
                                                                              hasFieldValue4,
                                                                              fieldValue5,
                                                                              hasFieldValue5,
                                                                              quality,
                                                                              reference,
                                                                              modifiedBy,
                                                                              modifiedDate);
                    break;

                default:
                    speciesFact = new SpeciesFact(id,
                                                  taxon,
                                                  individualCategory,
                                                  factor,
                                                  host,
                                                  period,
                                                  fieldValue1,
                                                  hasFieldValue1,
                                                  fieldValue2,
                                                  hasFieldValue2,
                                                  fieldValue3,
                                                  hasFieldValue3,
                                                  fieldValue4,
                                                  hasFieldValue4,
                                                  fieldValue5,
                                                  hasFieldValue5,
                                                  quality,
                                                  reference,
                                                  modifiedBy,
                                                  modifiedDate);
                    break;
            }

            return speciesFact;
        }

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
        public virtual ISpeciesFact GetSpeciesFact(IUserContext userContext,
                                                   ITaxon taxon,
                                                   IIndividualCategory individualCategory,
                                                   IFactor factor,
                                                   ITaxon host,
                                                   IPeriod period)
        {
            ISpeciesFact speciesFact;

            switch (factor.Id)
            {
                case (Int32)(FactorId.RedListCategoryAutomatic):
                    speciesFact = new SpeciesFactRedListCategory(userContext,
                                                                 taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                case (Int32)(FactorId.RedListCriteriaAutomatic):
                    speciesFact = new SpeciesFactRedListCriteria(userContext,
                                                                 taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                    speciesFact = new SpeciesFactRedListCriteriaDocumentation(userContext,
                                                                              taxon,
                                                                              individualCategory,
                                                                              factor,
                                                                              host,
                                                                              period);
                    break;
                default:
                    speciesFact = new SpeciesFact(userContext,
                                                  taxon,
                                                  individualCategory,
                                                  factor,
                                                  host,
                                                  period);
                    break;
            }

            return speciesFact;
        }

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
        public virtual String GetSpeciesFactIdentifier(Int32 taxonId,
                                                       Int32 individualCategoryId,
                                                       Int32 factorId,
                                                       Boolean hasHostId,
                                                       Int32 hostId,
                                                       Boolean hasPeriodId,
                                                       Int32 periodId)
        {
            if (!hasHostId)
            {
                hostId = 0;
            }

            if (!hasPeriodId)
            {
                periodId = 0;
            }

            return GetSpeciesFactIdentifier(taxonId,
                                            individualCategoryId,
                                            factorId,
                                            hostId,
                                            periodId);
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact.
        /// </summary>
        /// <param name="taxon">Taxon that is related to the species fact.</param>
        /// <param name="individualCategory">Individual category that is related to the species fact.</param>
        /// <param name="factor">Factor that is related to the species fact.</param>
        /// <param name="host">Host taxon that is related to the species fact.</param>
        /// <param name="period">Period that is related to the species fact.</param>
        /// <returns>Identifier for a species fact.</returns>
        public virtual String GetSpeciesFactIdentifier(ITaxon taxon,
                                                       IIndividualCategory individualCategory,
                                                       IFactor factor,
                                                       ITaxon host,
                                                       IPeriod period)
        {
            Int32 hostId = 0, periodId = 0;

            if ((host.IsNotNull()) && (factor.IsTaxonomic))
            {
                hostId = host.Id;
            }

            if ((period.IsNotNull()) && (factor.IsPeriodic))
            {
                periodId = period.Id;
            }

            return GetSpeciesFactIdentifier(taxon.Id,
                                            individualCategory.Id,
                                            factor.Id,
                                            hostId,
                                            periodId);
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact.
        /// </summary>
        /// <param name="taxonId">Id of the taxon that is related to the species fact.</param>
        /// <param name="individualCategoryId">Id of the individual category that is related to the species fact.</param>
        /// <param name="factorId">Id of the factor that is related to the species fact.</param>
        /// <param name="hostId">Id of the host taxon that is related to the species fact.</param>
        /// <param name="periodId">Id of the period that is related to the species fact.</param>
        /// <returns>Identifier for a species fact.</returns>
        private String GetSpeciesFactIdentifier(Int32 taxonId,
                                                Int32 individualCategoryId,
                                                Int32 factorId,
                                                Int32 hostId,
                                                Int32 periodId)
        {
            return "Taxon=" + taxonId + "," +
                   "Factor=" + factorId + "," +
                   "IndividualCategory=" + individualCategoryId + "," +
                   "Host=" + hostId + "," +
                   "Period=" + periodId;
        }

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All species fact qualities.</returns>
        public virtual SpeciesFactQualityList GetSpeciesFactQualities(IUserContext userContext)
        {
            return DataSource.GetSpeciesFactQualities(userContext);
        }

        /// <summary>
        /// Get species fact quality with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactQualityId">Species fact quality id.</param>
        /// <returns>Species fact quality with specified id.</returns>
        public virtual ISpeciesFactQuality GetSpeciesFactQuality(IUserContext userContext,
                                                                 Int32 speciesFactQualityId)
        {
            return GetSpeciesFactQualities(userContext).Get(speciesFactQualityId);
        }

        /// <summary>
        /// Get species fact quality with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactQualityId">Species fact quality id.</param>
        /// <returns>Species fact quality with specified id.</returns>
        public virtual ISpeciesFactQuality GetSpeciesFactQuality(IUserContext userContext,
                                                                 SpeciesFactQualityId speciesFactQualityId)
        {
            return GetSpeciesFactQuality(userContext, (Int32)speciesFactQualityId);
        }

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species facts.</returns>
        public virtual SpeciesFactList GetSpeciesFacts(IUserContext userContext, List<int> speciesFactIds)
        {
            return DataSource.GetSpeciesFacts(userContext, speciesFactIds);
        }

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
        public virtual SpeciesFactList GetSpeciesFacts(IUserContext userContext,
                                                       SpeciesFactList speciesFactIdentifiers)
        {
            return DataSource.GetSpeciesFacts(userContext, speciesFactIdentifiers);
        }

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public virtual SpeciesFactList GetSpeciesFacts(IUserContext userContext,
                                                       ISpeciesFactSearchCriteria searchCriteria)
        {
            return DataSource.GetSpeciesFacts(userContext, searchCriteria);
        }

        /// <summary>
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        public virtual Int32 GetTaxaCount(IUserContext userContext, ISpeciesFactSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxaCount(userContext, searchCriteria);
        }

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext, ISpeciesFactSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxa(userContext, searchCriteria);
        }

        /// <summary>
        /// Update species facts in RAM memory
        /// with latest information from data source.
        /// This method works on species facts both with and without id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFacts">Species facts to update.</param>
        public virtual void UpdateSpeciesFacts(IUserContext userContext,
                                               SpeciesFactList speciesFacts)
        {
            SpeciesFactList speciesFactsFromDataSource,
                            updatedSpeciesFacts;

            // Get updated data from web service.
            speciesFactsFromDataSource = GetSpeciesFacts(userContext, speciesFacts);

            // Update species facts.
            updatedSpeciesFacts = new SpeciesFactList();
            if (speciesFactsFromDataSource.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFactFromDataSource in speciesFactsFromDataSource)
                {
                    // Get species fact.
                    ISpeciesFact speciesFact = speciesFacts.Get(speciesFactFromDataSource.Identifier);

                    // Update species fact.
                    speciesFact.Update(speciesFactFromDataSource.Id,
                                       speciesFactFromDataSource.Reference,
                                       speciesFactFromDataSource.ModifiedDate,
                                       speciesFactFromDataSource.ModifiedBy,
                                       speciesFactFromDataSource.HasField1,
                                       speciesFactFromDataSource.HasField1 ? speciesFactFromDataSource.Field1.GetDoubleValue() : 0,
                                       speciesFactFromDataSource.HasField2,
                                       speciesFactFromDataSource.HasField2 ? speciesFactFromDataSource.Field2.GetDoubleValue() : 0,
                                       speciesFactFromDataSource.HasField3,
                                       speciesFactFromDataSource.HasField3 ? speciesFactFromDataSource.Field3.GetDoubleValue() : 0,
                                       speciesFactFromDataSource.HasField4,
                                       speciesFactFromDataSource.HasField4 ? speciesFactFromDataSource.Field4.GetStringValue() : null,
                                       speciesFactFromDataSource.HasField5,
                                       speciesFactFromDataSource.HasField5 ? speciesFactFromDataSource.Field5.GetStringValue() : null,
                                       speciesFactFromDataSource.Quality);
                    updatedSpeciesFacts.Add(speciesFact);
                }
            }

            // Update species facts that has been deleted.
            foreach (ISpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.HasId &&
                    !updatedSpeciesFacts.Exists(speciesFact.Identifier))
                {
                    speciesFact.Reset(userContext);
                }
            }
        }

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
        public virtual void UpdateSpeciesFacts(IUserContext userContext,
                                               SpeciesFactList speciesFacts,
                                               IReference defaultReference)
        {
            SpeciesFactList changedSpeciesFacts, createSpeciesFacts, deleteSpeciesFacts, updateSpeciesFacts;

            // Check arguments.
            speciesFacts.CheckNotNull("speciesFact");

            // Get all species facts that should be updated.
            changedSpeciesFacts = new SpeciesFactList();
            foreach (ISpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.AllowUpdate && speciesFact.HasChanged)
                {
                    changedSpeciesFacts.Add(speciesFact);
                }
            }

            if (changedSpeciesFacts.IsEmpty())
            {
                // Nothing to update.
                return;
            }

            // Split species facts that should be created, updated
            // or deleted into three different lists.
            createSpeciesFacts = new SpeciesFactList();
            deleteSpeciesFacts = new SpeciesFactList();
            updateSpeciesFacts = new SpeciesFactList();
            foreach (ISpeciesFact speciesFact in changedSpeciesFacts)
            {
                if (!speciesFact.HasId && speciesFact.ShouldBeSaved && !speciesFact.ShouldBeDeleted)
                {
                    // Create new species fact.
                    createSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId && speciesFact.ShouldBeDeleted)
                {
                    // Delete species fact.
                    deleteSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId && speciesFact.ShouldBeSaved && !speciesFact.ShouldBeDeleted)
                {
                    // Update species fact.
                    updateSpeciesFacts.Add(speciesFact);
                }
            }

            if (createSpeciesFacts.IsNotEmpty())
            {
                DataSource.CreateSpeciesFacts(userContext, createSpeciesFacts, defaultReference);
            }

            if (deleteSpeciesFacts.IsNotEmpty())
            {
                DataSource.DeleteSpeciesFacts(userContext, deleteSpeciesFacts);
            }

            if (updateSpeciesFacts.IsNotEmpty())
            {
                DataSource.UpdateSpeciesFacts(userContext, updateSpeciesFacts);
            }
        }
    }
}