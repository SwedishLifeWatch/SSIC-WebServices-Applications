//#define EXAMINE_PROBLEM
using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.TaxonAttributeService
{
    /// <summary>
    /// This class is used to handle species fact related information.
    /// </summary>
    public class SpeciesFactDataSource : TaxonAttributeDataSource, ISpeciesFactDataSource
    {
        /// <summary>
        /// Create species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        public virtual void CreateSpeciesFacts(IUserContext userContext,
                                               SpeciesFactList createSpeciesFacts,
                                               IReference defaultReference)
        {
            List<WebSpeciesFact> webCreateSpeciesFacts;

            CheckTransaction(userContext);
            webCreateSpeciesFacts = GetWebSpeciesFacts(createSpeciesFacts, defaultReference);

            // Create species facts.
            WebServiceProxy.TaxonAttributeService.CreateSpeciesFacts(GetClientInformation(userContext),
                                                                     webCreateSpeciesFacts);
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        public void DeleteSpeciesFacts(IUserContext userContext,
                                       SpeciesFactList deleteSpeciesFacts)
        {
            List<WebSpeciesFact> webDeleteSpeciesFacts;

            CheckTransaction(userContext);
            webDeleteSpeciesFacts = GetWebSpeciesFacts(deleteSpeciesFacts);

            // Update species facts.
            WebServiceProxy.TaxonAttributeService.DeleteSpeciesFacts(GetClientInformation(userContext),
                                                                     webDeleteSpeciesFacts);
        }

        /// <summary>
        /// Get ids for all references that are used in these species facts.
        /// </summary>
        /// <param name="speciesFacts">Species facts.</param>
        /// <returns>Ids for all references that are used in these species facts.</returns>
        private List<Int32> GetReferenceIds(List<WebSpeciesFact> speciesFacts)
        {
            List<Int32> referenceIds;

            referenceIds = new List<Int32>();
            if (speciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    if ((speciesFact.ReferenceId > 0) &&
                        !referenceIds.Contains(speciesFact.ReferenceId))
                    {
                        referenceIds.Add(speciesFact.ReferenceId);
                    }
                }
            }

            return referenceIds;
        }

        /// <summary>
        /// Convert a WebSpeciesFactQuality instance into
        /// an ISpeciesFactQuality instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webSpeciesFactQuality">A WebSpeciesFactQuality instance.</param>
        /// <returns>An ISpeciesFactQuality instance.</returns>
        private ISpeciesFactQuality GetSpeciesFactQuality(IUserContext userContext, WebSpeciesFactQuality webSpeciesFactQuality)
        {
            ISpeciesFactQuality speciesFactQuality;

            speciesFactQuality = new SpeciesFactQuality();
            speciesFactQuality.DataContext = GetDataContext(userContext);
            speciesFactQuality.Definition = webSpeciesFactQuality.Definition;
            speciesFactQuality.Id = webSpeciesFactQuality.Id;
            speciesFactQuality.Name = webSpeciesFactQuality.Name;
            return speciesFactQuality;
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
            List<WebSpeciesFactQuality> webSpeciesFactQualities;

            CheckTransaction(userContext);
            webSpeciesFactQualities = WebServiceProxy.TaxonAttributeService.GetSpeciesFactQualities(GetClientInformation(userContext));
            return GetSpeciesFactQualities(userContext, webSpeciesFactQualities);
        }

        /// <summary>
        /// Convert a list of WebSpeciesFactQuality instances
        /// to a SpeciesFactQualityList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webSpeciesFactQualities">List of WebSpeciesFactQuality instances.</param>
        /// <returns>Species fact qualities.</returns>
        private SpeciesFactQualityList GetSpeciesFactQualities(IUserContext userContext,
                                                               List<WebSpeciesFactQuality> webSpeciesFactQualities)
        {
            SpeciesFactQualityList speciesFactQualities;

            speciesFactQualities = null;
            if (webSpeciesFactQualities.IsNotEmpty())
            {
                speciesFactQualities = new SpeciesFactQualityList();
                foreach (WebSpeciesFactQuality webSpeciesFactQuality in webSpeciesFactQualities)
                {
                    speciesFactQualities.Add(GetSpeciesFactQuality(userContext, webSpeciesFactQuality));
                }

                speciesFactQualities.Sort();
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Convert a WebSpeciesFact instance into
        /// an ISpeciesFact instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webSpeciesFact">A WebSpeciesFact instance.</param>
        /// <param name="factors">List of factors.</param>
        /// <param name="individualCategories">List of individual categories.</param>
        /// <param name="periods">List of periods.</param>
        /// <param name="references">List of references.</param>
        /// <param name="speciesFactQualities">List of species fact qualities.</param>
        /// <param name="taxa">Taxa that are used in the species facts.</param>
        /// <returns>An ISpeciesFact instance.</returns>
        private ISpeciesFact GetSpeciesFact(IUserContext userContext,
                                            WebSpeciesFact webSpeciesFact,
                                            FactorList factors,
                                            IndividualCategoryList individualCategories,
                                            PeriodList periods,
                                            ReferenceList references,
                                            SpeciesFactQualityList speciesFactQualities,
                                            TaxonList taxa)
        {
            IFactor factor;
            IPeriod period;
            ISpeciesFact speciesFact;
            ITaxon host, taxon;

            factor = factors.Get(webSpeciesFact.FactorId);
            if (webSpeciesFact.IsHostSpecified)
            {
#if EXAMINE_PROBLEM
                if (!(taxa.Contains(webSpeciesFact.HostId)))
                {
                    throw new Exception("Host with id = " + webSpeciesFact.HostId + " is not in taxon list." + webSpeciesFact.GetString());
                }
#endif

                host = taxa.Get(webSpeciesFact.HostId);
            }
            else
            {
                if (factor.IsTaxonomic)
                {
                    host = CoreData.TaxonManager.GetTaxon(userContext, 0);
                }
                else
                {
                    host = null;
                }
            }

            if (webSpeciesFact.IsPeriodSpecified)
            {
#if EXAMINE_PROBLEM
                if (!(periods.Contains(webSpeciesFact.PeriodId)))
                {
                    throw new Exception("Period with id = " + webSpeciesFact.PeriodId + " is not in period list." + webSpeciesFact.GetString());
                }
#endif

                period = periods.Get(webSpeciesFact.PeriodId);
            }
            else
            {
                period = null;
            }

#if EXAMINE_PROBLEM
            if (!(taxa.Contains(webSpeciesFact.TaxonId)))
            {
                throw new Exception("Taxon with id = " + webSpeciesFact.TaxonId + " is not in taxon list." + webSpeciesFact.GetString());
            }
#endif

            taxon = taxa.Get(webSpeciesFact.TaxonId);
#if EXAMINE_PROBLEM
            if (!(individualCategories.Contains(webSpeciesFact.IndividualCategoryId)))
            {
                throw new Exception("Individual category with id = " + webSpeciesFact.IndividualCategoryId + " is not in individual category list." + webSpeciesFact.GetString());
            }

            if (!(speciesFactQualities.Contains(webSpeciesFact.QualityId)))
            {
                throw new Exception("Quality with id = " + webSpeciesFact.QualityId + " is not in quality list." + webSpeciesFact.GetString());
            }

            if (!(references.Contains(webSpeciesFact.ReferenceId)))
            {
                throw new Exception("Reference with id = " + webSpeciesFact.ReferenceId + " is not in reference list." + webSpeciesFact.GetString());
            }
#endif

            speciesFact = CoreData.SpeciesFactManager.GetSpeciesFact(userContext,
                                                                     webSpeciesFact.Id,
                                                                     taxon,
                                                                     individualCategories.Get(webSpeciesFact.IndividualCategoryId),
                                                                     factor,
                                                                     host,
                                                                     period,
                                                                     webSpeciesFact.FieldValue1,
                                                                     webSpeciesFact.IsFieldValue1Specified,
                                                                     webSpeciesFact.FieldValue2,
                                                                     webSpeciesFact.IsFieldValue2Specified,
                                                                     webSpeciesFact.FieldValue3,
                                                                     webSpeciesFact.IsFieldValue3Specified,
                                                                     webSpeciesFact.FieldValue4,
                                                                     webSpeciesFact.IsFieldValue4Specified,
                                                                     webSpeciesFact.FieldValue5,
                                                                     webSpeciesFact.IsFieldValue5Specified,
                                                                     speciesFactQualities.Get(webSpeciesFact.QualityId),
                                                                     references.Get(webSpeciesFact.ReferenceId),
                                                                     webSpeciesFact.ModifiedBy,
                                                                     webSpeciesFact.ModifiedDate);

            return speciesFact;
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
            List<WebSpeciesFact> webSpeciesFacts;

            CheckTransaction(userContext);
            webSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(userContext), speciesFactIds);
            return GetSpeciesFacts(userContext, webSpeciesFacts);
        }

        /// <summary>
        /// Convert a list of WebSpeciesFact instances
        /// to a SpeciesFactList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webSpeciesFacts">List of WebSpeciesFact instances.</param>
        /// <returns>List of SpeciesFact instances.</returns>
        protected virtual SpeciesFactList GetSpeciesFacts(IUserContext userContext, List<WebSpeciesFact> webSpeciesFacts)
        {
            SpeciesFactList speciesFacts;
            FactorList factors = CoreData.FactorManager.GetFactors(userContext);
            IndividualCategoryList individualCategories = CoreData.FactorManager.GetIndividualCategories(userContext);
            PeriodList periods = CoreData.FactorManager.GetPeriods(userContext);
            ReferenceList references;
            SpeciesFactQualityList speciesFactQualities = CoreData.SpeciesFactManager.GetSpeciesFactQualities(userContext);
            List<Int32> referenceIds, taxonIds;
            TaxonList taxa;

            speciesFacts = null;
            if (webSpeciesFacts.IsNotEmpty())
            {
                referenceIds = GetReferenceIds(webSpeciesFacts);
                references = CoreData.ReferenceManager.GetReferences(userContext, referenceIds);
                taxonIds = GetTaxonIds(webSpeciesFacts);
                taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);
                speciesFacts = new SpeciesFactList();
                foreach (WebSpeciesFact webSpeciesFact in webSpeciesFacts)
                {
                    speciesFacts.Add(GetSpeciesFact(userContext, webSpeciesFact, factors, individualCategories, periods, references, speciesFactQualities, taxa));
                }
            }

            return speciesFacts;
        }

        /// <summary>
        /// Converts an ISpeciesFact instance to a WebSpeciesFact instance.
        /// </summary>
        /// <param name="speciesFact">An ISpeciesFact instance.</param>
        /// <returns>A WebSpeciesFact instance.</returns>
        private WebSpeciesFact GetWebSpeciesFact(ISpeciesFact speciesFact)
        {
            WebSpeciesFact webSpeciesFact = new WebSpeciesFact
                                                {
                                                    FactorId = speciesFact.Factor.Id,
                                                    HostId = speciesFact.HasHost ? speciesFact.Host.Id : -1,
                                                    Id = speciesFact.Id,
                                                    IndividualCategoryId = speciesFact.IndividualCategory.Id,
                                                    IsHostSpecified = speciesFact.HasHost,
                                                    IsPeriodSpecified = speciesFact.HasPeriod,
                                                    ModifiedBy = speciesFact.ModifiedBy,
                                                    ModifiedDate = speciesFact.ModifiedDate,
                                                    PeriodId = speciesFact.HasPeriod ? speciesFact.Period.Id : -1,
                                                    QualityId = speciesFact.Quality.Id,
                                                    ReferenceId = speciesFact.HasReference ? speciesFact.Reference.Id : -1,
                                                    TaxonId = speciesFact.Taxon.Id
                                                };

            foreach (ISpeciesFactField field in speciesFact.Fields)
            {
                switch (field.Index)
                {
                    case 0:
                        webSpeciesFact.IsFieldValue1Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue1 = field.GetDoubleValue();
                        }

                        break;
                    case 1:
                        webSpeciesFact.IsFieldValue2Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue2 = field.GetDoubleValue();
                        }

                        break;
                    case 2:
                        webSpeciesFact.IsFieldValue3Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue3 = field.GetDoubleValue();
                        }

                        break;
                    case 3:
                        webSpeciesFact.IsFieldValue4Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue4 = field.GetStringValue().Trim();
                        }

                        break;
                    case 4:
                        webSpeciesFact.IsFieldValue5Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue5 = field.GetStringValue().Trim();
                        }

                        break;
                    default:
                        throw new Exception("Unknown data field!");
                }
            }

            return webSpeciesFact;
        }

        /// <summary>
        /// Converts an ISpeciesFact instance to a WebSpeciesFact instance.
        /// </summary>
        /// <param name="speciesFact">An ISpeciesFact instance.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        /// <returns>A WebSpeciesFact instance.</returns>
        private WebSpeciesFact GetWebSpeciesFact(ISpeciesFact speciesFact,
                                                 IReference defaultReference)
        {
            WebSpeciesFact webSpeciesFact = new WebSpeciesFact
            {
                FactorId = speciesFact.Factor.Id,
                HostId = speciesFact.HasHost ? speciesFact.Host.Id : -1,
                Id = speciesFact.Id,
                IndividualCategoryId = speciesFact.IndividualCategory.Id,
                IsHostSpecified = speciesFact.HasHost,
                IsPeriodSpecified = speciesFact.HasPeriod,
                ModifiedBy = speciesFact.ModifiedBy,
                ModifiedDate = speciesFact.ModifiedDate,
                PeriodId = speciesFact.HasPeriod ? speciesFact.Period.Id : -1,
                QualityId = speciesFact.Quality.Id,
                ReferenceId = speciesFact.HasReference ? speciesFact.Reference.Id : defaultReference.Id,
                TaxonId = speciesFact.Taxon.Id
            };

            foreach (ISpeciesFactField field in speciesFact.Fields)
            {
                switch (field.Index)
                {
                    case 0:
                        webSpeciesFact.IsFieldValue1Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue1 = field.GetDoubleValue();
                        }

                        break;
                    case 1:
                        webSpeciesFact.IsFieldValue2Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue2 = field.GetDoubleValue();
                        }

                        break;
                    case 2:
                        webSpeciesFact.IsFieldValue3Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue3 = field.GetDoubleValue();
                        }

                        break;
                    case 3:
                        webSpeciesFact.IsFieldValue4Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue4 = field.GetStringValue().Trim();
                        }

                        break;
                    case 4:
                        webSpeciesFact.IsFieldValue5Specified = field.HasValue;
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue5 = field.GetStringValue().Trim();
                        }

                        break;
                    default:
                        throw new Exception("Unknown data field!");
                }
            }

            return webSpeciesFact;
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
            List<WebSpeciesFact> webSpeciesFactIdentifiers, webSpeciesFacts;

            CheckTransaction(userContext);
            webSpeciesFactIdentifiers = GetWebSpeciesFacts(speciesFactIdentifiers);
            webSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIdentifiers(GetClientInformation(userContext), webSpeciesFactIdentifiers);
            return GetSpeciesFacts(userContext, webSpeciesFacts);
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
            List<WebSpeciesFact> webSpeciesFacts;
            WebSpeciesFactSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetSpeciesFactSearchCriteria(searchCriteria);
            webSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(userContext), webSearchCriteria);
            return GetSpeciesFacts(userContext, webSpeciesFacts);
        }

        /// <summary>
        /// Get name of person that made the last
        /// modification this piece of data.
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <returns>Name of person that made the last modification this piece of data.</returns>
        private String GetModifiedByPerson(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified(Settings.Default.WebDataModifiedByPerson))
            {
                return webData.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
            }
            else
            {
                // This will happen when ModifiedByPerson has been
                // replaced by ModifiedBy in TaxonService.
                return null;
            }
        }

        /// <summary>
        /// Update taxon object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="webTaxon">Web taxon.</param>
        private void UpdateTaxon(IUserContext userContext,
                                 ITaxon taxon,
                                 WebTaxon webTaxon)
        {
            taxon.AlertStatus = CoreData.TaxonManager.GetTaxonAlertStatus(userContext, webTaxon.AlertStatusId);
            taxon.Author = webTaxon.Author;
            taxon.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxon.CategoryId);
            taxon.ChangeStatus = CoreData.TaxonManager.GetTaxonChangeStatus(userContext, webTaxon.ChangeStatusId);
            taxon.CommonName = webTaxon.CommonName;
            taxon.CreatedBy = webTaxon.CreatedBy;
            taxon.CreatedDate = webTaxon.CreatedDate;
            taxon.DataContext = GetDataContext(userContext);
            taxon.Guid = webTaxon.Guid;
            taxon.Id = webTaxon.Id;
            taxon.IsInRevision = webTaxon.IsInRevision;
            taxon.IsPublished = webTaxon.IsPublished;
            taxon.IsValid = webTaxon.IsValid;
            taxon.ModifiedBy = webTaxon.ModifiedBy;
            taxon.ModifiedByPerson = GetModifiedByPerson(webTaxon); //taxon.GetModifiedByPersonFullname(userContext);
            taxon.ModifiedDate = webTaxon.ModifiedDate;
            taxon.PartOfConceptDefinition = webTaxon.PartOfConceptDefinition;
            taxon.ScientificName = webTaxon.ScientificName;
            taxon.SortOrder = webTaxon.SortOrder;
            taxon.ValidFromDate = webTaxon.ValidFromDate;
            taxon.ValidToDate = webTaxon.ValidToDate;
        }

        /// <summary>
        /// Convert a WebTaxon instance to an ITaxon instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxon">An WebTaxon object.</param>
        /// <returns>An ITaxon object.</returns>
        private ITaxon GetTaxon(IUserContext userContext,
                                WebTaxon webTaxon)
        {
            ITaxon taxon;

            taxon = new Taxon();
            UpdateTaxon(userContext, taxon, webTaxon);
            return taxon;
        }

        /// <summary>
        /// Convert a list of WebTaxon instances to a TaxonList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxa">List of WebTaxon instances.</param>
        /// <returns>Taxa.</returns>
        private TaxonList GetTaxa(IUserContext userContext, List<WebTaxon> webTaxa)
        {
            TaxonList taxa;

            taxa = new TaxonList();
            if (webTaxa.IsNotEmpty())
            {
                for (Int32 index = 0; index < webTaxa.Count; index++)
                {
                    if (Configuration.Debug && webTaxa[index].IsNull())
                    {
                        // This may happend in test since species fact
                        // data base has been updated with latest from
                        // production but taxon data base has
                        // not been updated.
                        continue;
                    }

                    taxa.Add(GetTaxon(userContext, webTaxa[index]));
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get taxon ids of taxa that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>List of taxon ids of taxa that matches search criteria.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext, ISpeciesFactSearchCriteria searchCriteria)
        {
            List<WebTaxon> taxa;
            WebSpeciesFactSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetSpeciesFactSearchCriteria(searchCriteria);
            taxa = WebServiceProxy.TaxonAttributeService.GetTaxaBySearchCriteria(GetClientInformation(userContext), webSearchCriteria);

            return GetTaxa(userContext, taxa);
        }

        /// <summary>
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        public virtual Int32 GetTaxaCount(IUserContext userContext,
                                                       ISpeciesFactSearchCriteria searchCriteria)
        {
            Int32 taxonCount;
            WebSpeciesFactSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetSpeciesFactSearchCriteria(searchCriteria);
            taxonCount = WebServiceProxy.TaxonAttributeService.GetTaxaCountBySearchCriteria(GetClientInformation(userContext), webSearchCriteria);
            return taxonCount;
        }

        /// <summary>
        /// Get ids for all taxa that are used in these species facts.
        /// </summary>
        /// <param name="speciesFacts">Species facts.</param>
        /// <returns>Ids for all taxa that are used in these species facts.</returns>
        private List<Int32> GetTaxonIds(List<WebSpeciesFact> speciesFacts)
        {
            DataIdInt32List taxonIds;

            taxonIds = new DataIdInt32List(true);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.HostId > 0)
                    {
                        taxonIds.Merge(new DataId32(speciesFact.HostId));
                    }

                    taxonIds.Merge(new DataId32(speciesFact.TaxonId));
                }
            }

            return taxonIds.GetInt32List();
        }

        /// <summary>
        /// Converts a SpeciesFactList instance to a list of WebSpeciesFact instances.
        /// </summary>
        /// <param name="speciesFacts">List of SpeciesFact instances.</param>
        /// <returns>List of WebSpeciesFact instances.</returns>
        private List<WebSpeciesFact> GetWebSpeciesFacts(SpeciesFactList speciesFacts)
        {
            List<WebSpeciesFact> webSpeciesFacts = null;

            if (speciesFacts.IsNotEmpty())
            {
                webSpeciesFacts = new List<WebSpeciesFact>();
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    webSpeciesFacts.Add(GetWebSpeciesFact(speciesFact));
                }
            }

            return webSpeciesFacts;
        }

        /// <summary>
        /// Converts a SpeciesFactList instance to a list of WebSpeciesFact instances.
        /// </summary>
        /// <param name="speciesFacts">List of SpeciesFact instances.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        /// <returns>List of WebSpeciesFact instances.</returns>
        private List<WebSpeciesFact> GetWebSpeciesFacts(SpeciesFactList speciesFacts,
                                                        IReference defaultReference)
        {
            List<WebSpeciesFact> webSpeciesFacts = null;

            if (speciesFacts.IsNotEmpty())
            {
                webSpeciesFacts = new List<WebSpeciesFact>();
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    webSpeciesFacts.Add(GetWebSpeciesFact(speciesFact, defaultReference));
                }
            }

            return webSpeciesFacts;
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public virtual void UpdateSpeciesFacts(IUserContext userContext,
                                               SpeciesFactList updateSpeciesFacts)
        {
            List<WebSpeciesFact> webUpdateSpeciesFacts;

            CheckTransaction(userContext);
            webUpdateSpeciesFacts = GetWebSpeciesFacts(updateSpeciesFacts);

            // Update species facts.
            WebServiceProxy.TaxonAttributeService.UpdateSpeciesFacts(GetClientInformation(userContext),
                                                                     webUpdateSpeciesFacts);
        }
    }
}