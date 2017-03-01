using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using ApplicationManager = ArtDatabanken.WebService.ArtDatabankenService.Data.ApplicationManager;
using FactorManager = ArtDatabanken.WebService.ArtDatabankenService.Data.FactorManager;
using ReferenceManager = ArtDatabanken.WebService.ArtDatabankenService.Data.ReferenceManager;
using RegionManager = ArtDatabanken.WebService.ArtDatabankenService.Data.RegionManager;
using SpeciesObservationManager = ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesObservationManager;
using TaxonManager = ArtDatabanken.WebService.ArtDatabankenService.Data.TaxonManager;
using UserManager = ArtDatabanken.WebService.ArtDatabankenService.Data.UserManager;
using WebFactorFieldType = ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorFieldType;
using WebFactorOrigin = ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorOrigin;
using WebFactorUpdateMode = ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorUpdateMode;
using WebReference = ArtDatabanken.WebService.ArtDatabankenService.Data.WebReference;
using WebSpeciesFact = ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact;
using WebSpeciesObservationChange = ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationChange;
using WebTaxon = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxon;
using WebTaxonName = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonName;
using WebTaxonNameSearchCriteria = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonNameSearchCriteria;
using WebTaxonSearchCriteria = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonSearchCriteria;
using WebTaxonTreeNode = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonTreeNode;
using WebTaxonTreeSearchCriteria = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonTreeSearchCriteria;
using WebUser = ArtDatabanken.Data.WebService.WebUser;

namespace ArtDatabankenService
{
    /// <summary>
    /// Implementation of the ArtDatabanken web service.
    /// </summary>
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ArtDatabankenService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ArtDatabankenService : IArtDatabankenService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static ArtDatabankenService()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.TaxonManager = new TaxonManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
        }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void ClearCache(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "ClearCache"))
            {
                try
                {
                    context.ClearCache();
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void CommitTransaction(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "CommitTransaction"))
            {
                try
                {
                    context.CommitTransaction();
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="reference">New reference.</param>
        public void CreateReference(String clientToken,
                                    WebReference reference)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    ReferenceManager.CreateReference(context, reference);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void DeleteTrace(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "DeleteTrace"))
            {
                try
                {
                    LogManager.DeleteTrace(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about bird nest activities.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebBirdNestActivity> GetBirdNestActivities(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetBirdNestActivities(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>Information about cities.</returns>
        public List<WebCity> GetCitiesBySearchString(String clientToken,
                                                     String searchString)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return GeographicManager.GetCitiesBySearchString(context, searchString);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about swedish counties.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about swedish counties.</returns>
        public List<WebCounty> GetCounties(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return GeographicManager.GetCounties(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about databases.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Table with information about databases.</returns>
        public List<WebDatabase> GetDatabases(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return DatabaseManager.GetDatabases(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about database update.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about database update.</returns>
        public WebDatabaseUpdate GetDatabaseUpdate(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return DatabaseManager.GetDatabaseUpdate(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about different endangered lists.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about endangered lists.</returns>
        public List<WebEndangeredList> GetEndangeredLists(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetEndangeredLists(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor data types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor data types.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorDataType> GetFactorDataTypes(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorDataType> factorDataTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorDataTypes = FactorManager.GetFactorDataTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorDataTypes;
        }

        /// <summary>
        /// Get information about all factor field enums.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor field enums.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorFieldEnum> GetFactorFieldEnums(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorFieldEnum> factorFieldEnums;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorFieldEnums = FactorManager.GetFactorFieldEnums(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorFieldEnums;
        }

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor field types.</returns>
        public List<WebFactorFieldType> GetFactorFieldTypes(String clientToken)
        {
            List<WebFactorFieldType> factorFieldTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorFieldTypes = FactorManager.GetFactorFieldTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorFieldTypes;
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor origins.</returns>
        public List<WebFactorOrigin> GetFactorOrigins(String clientToken)
        {
            List<WebFactorOrigin> factorOrigins;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorOrigins = FactorManager.GetFactorOrigins(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorOrigins;
        }

        /// <summary>
        /// Get information about all factors.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factors.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactor> GetFactors(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactor> factors;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factors = FactorManager.GetFactors(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factors;
        }

        /// <summary>
        /// Get factor information about factors that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="factorSearchCriteria">The factor search criteria.</param>
        /// <returns>Information about factors.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactor> GetFactorsBySearchCriteria(String clientToken,
                                                          ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorSearchCriteria factorSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return FactorManager.GetFactorsBySearchCriteria(context, factorSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor trees.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor tree information.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorTreeNode> GetFactorTrees(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorTreeNode> factorTrees;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorTrees = FactorManager.GetFactorTrees(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorTrees;
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Factor tree information.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorTreeNode> GetFactorTreesBySearchCriteria(String clientToken,
                                                                      ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorTreeSearchCriteria searchCriteria)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebFactorTreeNode> factorTrees;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorTrees = FactorManager.GetFactorTreesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorTrees;
        }

        /// <summary>
        /// Get information about all factor update modes.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor update modes.</returns>
        public List<WebFactorUpdateMode> GetFactorUpdateModes(String clientToken)
        {
            List<WebFactorUpdateMode> factorUpdateModes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    factorUpdateModes = FactorManager.GetFactorUpdateModes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return factorUpdateModes;
        }

        /// <summary>
        /// Get information about host taxa.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="factorId">Id for for factor to get host taxa information about.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        public List<WebTaxon> GetHostTaxa(String clientToken,
                                          Int32 factorId,
                                          TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetHostTaxa(context, factorId, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all host taxa associated with a sertain taxon.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        public List<WebTaxon> GetHostTaxaByTaxonId(String clientToken,
                                  Int32 taxonId,
                                  TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetHostTaxaByTaxonId(context, taxonId, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all individual periods.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Individual Periods.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebIndividualCategory> GetIndividualCategories(String clientToken)
        {

            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebIndividualCategory> individualCategories;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    individualCategories = IndividualCategoryManager.GetIndividualCategories(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return individualCategories;
        }

        /// <summary>
        /// Get entries from the web service log
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebLogRow> GetLog(String clientToken, ArtDatabanken.WebService.ArtDatabankenService.Data.LogType type, String userName, Int32 rowCount)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebLogRow> logEntries;

            using (WebServiceContext context = new WebServiceContext(clientToken, "GetLog", type, userName, rowCount))
            {
                try
                {
                    logEntries = LogManager.GetLog(context, type, userName, rowCount);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return logEntries;
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Periods.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebPeriod> GetPeriods(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebPeriod> periods;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    periods = PeriodManager.GetPeriods(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return periods;
        }

        /// <summary>
        /// Get all periods types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Period types.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebPeriodType> GetPeriodTypes(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebPeriodType> periodTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    periodTypes = PeriodManager.GetPeriodTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return periodTypes;
        }

        /// <summary>
        /// Get information about swedish provinces.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about swedish provinces.</returns>
        public List<WebProvince> GetProvinces(String clientToken)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return GeographicManager.GetProvinces(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>References.</returns>
        public List<WebReference> GetReferences(String clientToken)
        {
            List<WebReference> references;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    references = ReferenceManager.GetReferences(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return references;
        }

        /// <summary>
        /// Get references by search string.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchString">Search string.</param>
        /// <returns></returns>
        public List<WebReference> GetReferencesBySearchString(String clientToken, String searchString)
        {
            List<WebReference> references;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    references = ReferenceManager.GetReferencesBySearchString(context, searchString);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return references;
        }

        /// <summary>
        /// Get information about all categories of species fact qualty.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Species fact qualities.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFactQuality> GetSpeciesFactQualities(String clientToken)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFactQuality> speciesFactQualities;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    speciesFactQualities = ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetSpeciesFactQualities(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species fact information.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> GetSpeciesFactsById(String clientToken, List<Int32> speciesFactIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetSpeciesFactsById(context, speciesFactIds);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesFacts">Species facts to get information about.</param>
        /// <returns>Species facts information.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> GetSpeciesFactsByIdentifier(String clientToken,
                                                                List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> speciesFacts)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetSpeciesFactsByIdentifier(context, speciesFacts);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about species facts that correspond to the combination of user parameters.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="userParameterSelection">The user parameter selection.</param>
        /// <returns>Species facts.</returns>
        public List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> GetSpeciesFactsByUserParameterSelection(String clientToken,
                                                                            WebUserParameterSelection userParameterSelection)
        {
            List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> speciesFacts;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    speciesFacts = ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(context, userParameterSelection);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return speciesFacts;
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 100000 observations of each change type (new or updated)
        /// with information can be retrieved in one call.
        /// Max 1000000 observations of each change type (deleted, new
        /// or updated), with GUIDs or ids, can be retrieved in one call.
        /// Parameters changedFrom and changedTo may be the same date.
        /// Parameter changedTo must not be today or in the future.
        /// If parameter changedTo is yesterday the method call
        /// must be made after the nightly update of the 
        /// species observations have been performed. 
        /// Currently it is ok to call this method after 05:00
        /// if yesterdays species observations should be retrieved.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <returns>Information about changed species observations.</returns>
        public WebSpeciesObservationChange GetSpeciesObservationChange(String clientToken,
                                                                       DateTime changedFrom,
                                                                       DateTime changedTo)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetSpeciesObservationChange(context,
                                                                                 changedFrom,
                                                                                 changedTo);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public Int32 GetSpeciesObservationCountBySearchCriteria(String clientToken,
                                                                ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(context,
                                                                                                speciesObservationSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 10000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="userRoleId">In which role is the user retrieving species observations.</param>
        /// <returns>Species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if to many species observation ids has been given.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationInformation GetSpeciesObservationsById(String clientToken,
                                                                           List<Int64> speciesObservationIds,
                                                                           Int32 userRoleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetSpeciesObservationsById(context,
                                                                                speciesObservationIds,
                                                                                userRoleId);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Information about requested species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(String clientToken,
                                                                                       ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetSpeciesObservations(context,
                                                                            speciesObservationSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Status for this web service.</returns>       
        public virtual List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebResourceStatus> GetStatus(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "GetStatus"))
            {
                try
                {
                    return WebServiceData.WebServiceManager.GetStatus(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxa utelizing a sertain host taxon and any of its child taxa.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="hostTaxonId">Id for host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        public List<WebTaxon> GetTaxaByHostTaxonId(String clientToken,
                                  Int32 hostTaxonId,
                                  TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetTaxaByHostTaxonId(context, hostTaxonId, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        public List<WebTaxon> GetTaxaById(String clientToken,
                                          List<Int32> taxonIds,
                                          TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetTaxaById(context, taxonIds, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="hasOrganismGroupId">Indicates if organism group id is set.</param>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="hasEndangeredListId">Indicates if endangered list id is set.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="hasRedlistCategoryId">Indicates if redlist category id is set.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        public List<WebTaxon> GetTaxaByOrganismOrRedlist(String clientToken,
                                                         Boolean hasOrganismGroupId,
                                                         Int32 organismGroupId,
                                                         Boolean hasEndangeredListId,
                                                         Int32 endangeredListId,
                                                         Boolean hasRedlistCategoryId,
                                                         Int32 redlistCategoryId,
                                                         TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetTaxaByOrganismOrRedlist(context,
                                                                   hasOrganismGroupId,
                                                                   organismGroupId,
                                                                   hasEndangeredListId,
                                                                   endangeredListId,
                                                                   hasRedlistCategoryId,
                                                                   redlistCategoryId,
                                                                   taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about taxa that matches the query.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="dataQuery">Data query.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if query is null.</exception>
        public List<WebTaxon> GetTaxaByQuery(String clientToken,
                                             WebDataQuery dataQuery,
                                             TaxonInformationType taxonInformationType)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetTaxaByQuery(context, dataQuery, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxa information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonSearchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if taxonSearchCriteria is null.</exception>
        public List<WebTaxon> GetTaxaBySearchCriteria(String clientToken,
                                                      WebTaxonSearchCriteria taxonSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return TaxonManager.GetTaxaBySearchCriteria(context, taxonSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxa for the species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public List<WebTaxon> GetTaxaBySpeciesObservations(String clientToken,
                                                           ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetTaxaBySpeciesObservations(context, speciesObservationSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public Int32 GetTaxaCountBySpeciesObservations(String clientToken,
                                                       ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    return SpeciesObservationManager.GetTaxaCountBySpeciesObservations(context, speciesObservationSearchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxon information.</returns>
        public WebTaxon GetTaxonById(String clientToken,
                                     Int32 taxonId,
                                     TaxonInformationType taxonInformationType)
        {
            WebTaxon taxon;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxon = TaxonManager.GetTaxon(context, taxonId, taxonInformationType);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxon;
        }

        /// <summary>
        /// Get information about occurence in swedish
        /// counties for specified taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Information about occurence in swedish counties for specified taxon.</returns>
        public List<WebTaxonCountyOccurrence> GetTaxonCountyOccurence(String clientToken,
                                                                      Int32 taxonId)
        {
            List<WebTaxonCountyOccurrence> countyOccurrencies;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    countyOccurrencies = ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.GetTaxonCountyOccurence(context, taxonId);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return countyOccurrencies;
        }

        /// <summary>
        /// Get taxon names for specified taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Taxon names.</returns>
        public List<WebTaxonName> GetTaxonNames(String clientToken,
                                                Int32 taxonId)
        {
            List<WebTaxonName> taxonNames;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonNames = TaxonManager.GetTaxonNames(context, taxonId);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonNames;
        }

        /// <summary>
        /// Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The taxon name search criteria.</param>
        /// <returns>Taxon names.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public List<WebTaxonName> GetTaxonNamesBySearchCriteria(String clientToken,
                                                                WebTaxonNameSearchCriteria searchCriteria)
        {
            List<WebTaxonName> taxonNames;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonNames;
        }

        /// <summary>
        /// Get information about all taxon name types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon name types.</returns>
        public List<WebTaxonNameType> GetTaxonNameTypes(String clientToken)
        {
            List<WebTaxonNameType> taxonNameTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonNameTypes = TaxonManager.GetTaxonNameTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonNameTypes;
        }

        /// <summary>
        /// Get information about all taxon name use types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon name use types.</returns>
        public List<WebTaxonNameUseType> GetTaxonNameUseTypes(String clientToken)
        {
            List<WebTaxonNameUseType> taxonNameUseTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonNameUseTypes = TaxonManager.GetTaxonNameUseTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonNameUseTypes;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(String clientToken,
                                                                    WebTaxonTreeSearchCriteria searchCriteria)
        {
            List<WebTaxonTreeNode> taxonTrees;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonTrees;
        }

        /// <summary>
        /// Get information about all taxon types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon types.</returns>
        public List<WebTaxonType> GetTaxonTypes(String clientToken)
        {
            List<WebTaxonType> taxonTypes;

            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    taxonTypes = TaxonManager.GetTaxonTypes(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return taxonTypes;
        }

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Returns user information.</returns>
        public ArtDatabanken.WebService.ArtDatabankenService.Data.WebUser GetUser(String clientToken)
        {
            WebPerson webPerson = null;
            List<WebRole> webRoles;
            ArtDatabanken.WebService.ArtDatabankenService.Data.WebUser user;
            ArtDatabanken.WebService.Data.WebUser webUser;
            WebUserRole role;

            using (WebServiceContext context = new WebServiceContext(clientToken, "GetUser"))
            {
                try
                {
                    // Get user related data.
                    webUser = context.GetUser();
                    if (webUser.Type == UserType.Person)
                    {
                        webPerson = WebServiceData.UserManager.GetPerson(context);
                    }
                    webRoles = WebServiceData.UserManager.GetRoles(context);

                    // Create user information.
                    user = new ArtDatabanken.WebService.ArtDatabankenService.Data.WebUser();
                    if (webUser.Type == UserType.Person)
                    {
                        user.FirstName = webPerson.FirstName;
                        user.LastName = webPerson.LastName;
                    }
                    else
                    {
                        user.FirstName = webUser.UserName;
                        user.LastName = webUser.UserName;
                    }
                    user.Id = webUser.Id;
                    user.UserName = webUser.UserName;
                    user.Roles = new List<WebUserRole>();
                    if (webRoles.IsNotEmpty())
                    {
                        foreach (WebRole webRole in webRoles)
                        {
                            role = new WebUserRole();
                            role.Description = webRole.Description;
                            role.Id = webRole.Id;
                            role.Name = webRole.ShortName;
                            user.Roles.Add(role);
                        }
                    }
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }

            return user;
        }

        /// <summary>
        /// Get web service context.
        /// This method is used to add Application Insights telemetry data from the request.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Web service context.</returns>
        private WebServiceContext GetWebServiceContext(String clientToken)
        {
            RequestTelemetry telemetry;
            WebServiceContext context;
            ArtDatabanken.WebService.Data.WebUser user;

            context = new WebServiceContext(clientToken);
            try
            {
                if (context.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                {
                    telemetry = OperationContext.Current.GetRequestTelemetry();
                    if (telemetry.IsNotNull())
                    {
                        if (context.ClientToken.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = context.ClientToken.ApplicationIdentifier;
                            telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = context.ClientToken.ClientIPAddress;
                        }

                        user = context.GetUser();
                        if (user.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.UserId.ToString()] = context.GetUser().Id.WebToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }

            return context;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Client token to use in furter requestes or
        /// null if user was not logged in.
        /// </returns>       
        public String Login(String userName,
                            String password,
                            String applicationIdentifier,
                            Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            using (WebServiceContext context = new WebServiceContext(new WebClientToken(userName, applicationIdentifier, WebServiceData.WebServiceManager.Key), false))
            {
                try
                {
                    loginResponse = WebServiceData.UserManager.Login(context, userName, password, applicationIdentifier, isActivationRequired);
                    if (loginResponse.IsNotNull())
                    {
                        return loginResponse.Token;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void Logout(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "Logout"))
            {
                try
                {
                    WebServiceData.UserManager.Logout(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public virtual Boolean Ping()
        {
            using (WebServiceContext context = new WebServiceContext(new WebClientToken(WebServiceData.WebServiceManager.Name,
                                                                                        ArtDatabanken.Data.ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                                        WebServiceData.WebServiceManager.Key), false))
            {
                try
                {
                    return WebServiceData.WebServiceManager.Ping(context);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void RollbackTransaction(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "RollbackTransaction"))
            {
                try
                {
                    context.RollbackTransaction();
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="userName">User name.</param>
        public void StartTrace(String clientToken, String userName)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "StartTrace", userName))
            {
                try
                {
                    context.StartTrace(userName);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(String clientToken,
                                     Int32 timeout)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "StartTransaction", timeout))
            {
                try
                {
                    context.StartTransaction(timeout);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        public void StopTrace(String clientToken)
        {
            using (WebServiceContext context = new WebServiceContext(clientToken, "StopTrace"))
            {
                try
                {
                    context.StopTrace();
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update species facts. This method should only be used by Dyntaxa web application.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        /// <param name="fullName">Full Name of editor.</param>
        public void UpdateDyntaxaSpeciesFacts(String clientToken,
                                              List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> createSpeciesFacts,
                                              List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> deleteSpeciesFacts,
                                              List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> updateSpeciesFacts,
                                              String fullName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.UpdateDyntaxaSpeciesFacts(context,
                                                                 createSpeciesFacts,
                                                                 deleteSpeciesFacts,
                                                                 updateSpeciesFacts,
                                                                 fullName);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update reference with specific Id.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="reference">Existing reference to update.</param>
        public void UpdateReference(String clientToken, WebReference reference)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    ReferenceManager.UpdateReference(context, reference);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public void UpdateSpeciesFacts(String clientToken,
                                       List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> createSpeciesFacts,
                                       List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> deleteSpeciesFacts,
                                       List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesFact> updateSpeciesFacts)
        {
            using (WebServiceContext context = GetWebServiceContext(clientToken))
            {
                try
                {
                    ArtDatabanken.WebService.ArtDatabankenService.Data.SpeciesFactManager.UpdateSpeciesFacts(context,
                                                          createSpeciesFacts,
                                                          deleteSpeciesFacts,
                                                          updateSpeciesFacts);
                }
                catch (Exception exception)
                {
                    LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
    }
}
