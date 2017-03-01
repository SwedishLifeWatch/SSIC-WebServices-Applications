using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of taxon related objects.
    /// </summary>
    public class TaxonManager : DataQueryManager
    {
        private static readonly Hashtable _unknownTaxon = new Hashtable();
        private static TaxonNameTypeList _taxonNameTypes;
        private static TaxonNameUseTypeList _taxonNameUseTypes;
        private static TaxonTypeList _taxonTypes;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static TaxonManager()
        {
            RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _taxonNameTypes thread safe.
        /// </summary>
        private static TaxonNameTypeList TaxonNameTypes
        {
            get
            {
                TaxonNameTypeList taxonNameTypes;

                lock (_lockObject)
                {
                    taxonNameTypes = _taxonNameTypes;
                }
                return taxonNameTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _taxonNameTypes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _taxonNameUseTypes thread safe.
        /// </summary>
        private static TaxonNameUseTypeList TaxonNameUseTypes
        {
            get
            {
                TaxonNameUseTypeList taxonNameUseTypes;

                lock (_lockObject)
                {
                    taxonNameUseTypes = _taxonNameUseTypes;
                }
                return taxonNameUseTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _taxonNameUseTypes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _taxonTypes thread safe.
        /// </summary>
        private static TaxonTypeList TaxonTypes
        {
            get
            {
                TaxonTypeList taxonTypes;

                lock (_lockObject)
                {
                    taxonTypes = _taxonTypes;
                }
                return taxonTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _taxonTypes = value;
                }
            }
        }

        /// <summary>
        /// Get information about host taxa.
        /// </summary>
        /// <param name="factorId">Id for for factor to get host taxa information about.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        public static TaxonList GetHostTaxa(Int32 factorId,
                                            TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;

            // Get data from web service.
            webTaxa = WebServiceClient.GetHostTaxa(factorId, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get all host taxa associated with a certain taxon.
        /// The method is restricted to factors of type substrate.
        /// </summary>
        /// <param name="taxonId">Id of taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static TaxonList GetHostTaxaByTaxonId(Int32 taxonId,
                                                     TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;

            // Get data from web service.
            webTaxa = WebServiceClient.GetHostTaxaByTaxonId(taxonId, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get information about taxa with a protection level
        /// that is higher than public.
        /// </summary>
        /// <param name="includeSubTaxa">If true, all sub taxa are included in the result.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Information about protected taxa.</returns>
        public static TaxonList GetProtectedTaxa(Boolean includeSubTaxa,
                                                 TaxonInformationType taxonInformationType)
        {
            Factor factor;
            List<WebTaxon> webTaxa;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;
            SpeciesProtectionLevelEnum protectionLevel;
            WebDataQuery webDataQuery;

            // Create data query.
            speciesFactCondition = new SpeciesFactCondition();
            factor = FactorManager.GetFactor(FactorId.ProtectionLevel);
            speciesFactCondition.Factors.Add(factor);

            for (protectionLevel = SpeciesProtectionLevelEnum.Protected1;
                 protectionLevel <= SpeciesProtectionLevelEnum.MaxProtected;
                 protectionLevel++)
            {
                speciesFactFieldCondition = new SpeciesFactFieldCondition();
                speciesFactFieldCondition.FactorField = factor.FactorDataType.Field1;
                speciesFactFieldCondition.SetValue((Int32)protectionLevel);
                speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
            }

            // Get data from web service.
            webDataQuery = GetDataQuery(speciesFactCondition);
            webTaxa = WebServiceClient.GetTaxaByQuery(webDataQuery, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        public static TaxonList GetTaxa(List<Int32> taxonIds,
                                        TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;

            // Check arguments.
            taxonIds.CheckNotEmpty("taxonIds");

            // Get data from web service.
            webTaxa = WebServiceClient.GetTaxaById(taxonIds, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Convert a WebTaxon array to a TaxonList.
        /// </summary>
        /// <param name="webTaxa">The WebTaxon array.</param>
        /// <returns>A TaxonList.</returns>
        public static TaxonList GetTaxa(List<WebTaxon> webTaxa)
        {
            TaxonList taxa;

            // Convert taxon data.
            taxa = new TaxonList(true);
            if (webTaxa.IsNotEmpty())
            {
                foreach (WebTaxon webTaxon in webTaxa)
                {
                    taxa.Add(GetTaxon(webTaxon));
                }
            }
            return taxa;
        }

        /// <summary>
        /// Get taxa information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        public static TaxonList GetTaxaByOrganismOrRedlist(Int32? organismGroupId,
                                                           Int32? endangeredListId,
                                                           Int32? redlistCategoryId,
                                                           TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;

            // Get data from web service.
            webTaxa = WebServiceClient.GetTaxaByOrganismOrRedlist(organismGroupId.HasValue,
                                                                  organismGroupId.GetValueOrDefault(),
                                                                  endangeredListId.HasValue,
                                                                  endangeredListId.GetValueOrDefault(),
                                                                  redlistCategoryId.HasValue,
                                                                  redlistCategoryId.GetValueOrDefault(),
                                                                  taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get all taxa utelizing a certain host taxon and any of its child taxa.
        /// The method is restricted to factors of type substrate.
        /// </summary>
        /// <param name="hostTaxonId">Id of host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static TaxonList GetTaxaByHostTaxonId(Int32 hostTaxonId,
                                                     TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;

            // Get data from web service.
            webTaxa = WebServiceClient.GetTaxaByHostTaxonId(hostTaxonId, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get taxa information about taxa that matches the data query.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        public static TaxonList GetTaxaByQuery(IDataQuery dataQuery,
                                               TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> webTaxa;
            WebDataQuery webDataQuery;

            // Check arguments.
            dataQuery.CheckNotNull("dataQuery");

            // Get data from web service.
            webDataQuery = GetDataQuery(dataQuery);
            webTaxa = WebServiceClient.GetTaxaByQuery(webDataQuery, taxonInformationType);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static TaxonList GetTaxaBySearchCriteria(TaxonSearchCriteria searchCriteria)
        {
            List<WebTaxon> webTaxa;
            WebTaxonSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            webSearchCriteria = GetTaxonSearchCriteria(searchCriteria);
            webTaxa = WebServiceClient.GetTaxaBySearchCriteria(webSearchCriteria);
            return GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get Taxon instance with data from WebTaxon instance.
        /// </summary>
        /// <param name="webTaxon">WebTaxon instance with data.</param>
        /// <returns>Taxon instance.</returns>
        private static Taxon GetTaxon(WebTaxon webTaxon)
        {
            DataFieldList dataFields;
            Taxon taxon;

            dataFields = new DataFieldList(webTaxon.DataFields);
            switch (webTaxon.TaxonInformationType)
            {
                case TaxonInformationType.Basic:
                    taxon = new Taxon(webTaxon.Id,
                                      webTaxon.TaxonTypeId,
                                      webTaxon.SortOrder,
                                      webTaxon.TaxonInformationType,
                                      webTaxon.ScientificName,
                                      webTaxon.Author,
                                      webTaxon.CommonName);
                    break;

                case TaxonInformationType.PrintObs:
                    taxon = new TaxonPrintObs(webTaxon.Id,
                                              webTaxon.TaxonTypeId,
                                              webTaxon.SortOrder,
                                              webTaxon.TaxonInformationType,
                                              webTaxon.ScientificName,
                                              webTaxon.Author,
                                              webTaxon.CommonName,
                                              dataFields.GetString(TaxonPrintObs.PHYLUM_DATA_FIELD),
                                              dataFields.GetString(TaxonPrintObs.CLASS_DATA_FIELD),
                                              dataFields.GetString(TaxonPrintObs.ORDER_DATA_FIELD),
                                              dataFields.GetString(TaxonPrintObs.FAMILY_DATA_FIELD));
                    break;
                default:
                    throw new Exception("Unknown taxon information type " + webTaxon.TaxonInformationType.ToString() + "!");
            }
            return taxon;
        }

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxon information.</returns>
        public static Taxon GetTaxon(Int32 taxonId,
                                     TaxonInformationType taxonInformationType)
        {
            Taxon taxon;
            WebTaxon webTaxon;

            if (taxonId == (Int32)TaxonId.Dummy)
            {
                // Check if data is cached.
                lock (_unknownTaxon)
                {
                    taxon = (Taxon)(_unknownTaxon[taxonInformationType]);
                }

                if (taxon.IsNull())
                {
                    // Get data from web service.
                    webTaxon = WebServiceClient.GetTaxon(taxonId, taxonInformationType);
                    taxon = GetTaxon(webTaxon);

                    // Add data to cache.
                    lock (_unknownTaxon)
                    {
                        _unknownTaxon.Add(taxonInformationType, taxon);
                    }
                }
            }
            else
            {
                // Get data from web service.
                webTaxon = WebServiceClient.GetTaxon(taxonId, taxonInformationType);
                taxon = GetTaxon(webTaxon);
            }
            return taxon;
        }

        /// <summary>
        /// Get taxon names for specified taxon.
        /// </summary>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Taxon names.</returns>
        public static TaxonNameList GetTaxonNames(Int32 taxonId)
        {
            List<WebTaxonName> webTaxonNames;

            // Get data from web service.
            webTaxonNames = WebServiceClient.GetTaxonNames(taxonId);
            return GetTaxonNames(webTaxonNames);
        }

        /// <summary>
        /// Convert a WebTaxonName array to a TaxonNameList.
        /// </summary>
        /// <param name="webTaxonNames">Web taxon names.</param>
        /// <returns>Taxon names.</returns>
        private static TaxonNameList GetTaxonNames(List<WebTaxonName> webTaxonNames)
        {
            DataFieldList dataFields;
            TaxonName taxonName;
            TaxonNameList taxonNames = null;

            if (webTaxonNames.IsNotEmpty())
            {
                taxonNames = new TaxonNameList();
                foreach (WebTaxonName webTaxonName in webTaxonNames)
                {
                    dataFields = new DataFieldList(webTaxonName.Taxon.DataFields);
                    taxonName = new TaxonName(webTaxonName.Id,
                                              webTaxonName.Taxon.Id,
                                              webTaxonName.TaxonNameTypeId,
                                              webTaxonName.TaxonNameUseTypeId,
                                              webTaxonName.Name,
                                              webTaxonName.Author,
                                              webTaxonName.IsRecommended);
                    taxonName.Taxon = new TaxonPrintObs(webTaxonName.Taxon.Id,
                                                        webTaxonName.Taxon.TaxonTypeId,
                                                        webTaxonName.Taxon.SortOrder,
                                                        webTaxonName.Taxon.TaxonInformationType,
                                                        webTaxonName.Taxon.ScientificName,
                                                        webTaxonName.Taxon.Author,
                                                        webTaxonName.Taxon.CommonName,
                                                        dataFields.GetString(TaxonPrintObs.PHYLUM_DATA_FIELD),
                                                        dataFields.GetString(TaxonPrintObs.CLASS_DATA_FIELD),
                                                        dataFields.GetString(TaxonPrintObs.ORDER_DATA_FIELD),
                                                        dataFields.GetString(TaxonPrintObs.FAMILY_DATA_FIELD));
                    taxonNames.Add(taxonName);
                }
            }
            return taxonNames;
        }

        /// <summary>
        /// Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon name search criteria.</param>
        /// <returns>Taxon names.</returns>
        /// <exception cref="ArgumentException">Thrown if search criteria is not valid.</exception>
        public static TaxonNameList GetTaxonNamesBySearchCriteria(TaxonNameSearchCriteria searchCriteria)
        {
            List<WebTaxonName> webTaxonNames;
            WebTaxonNameSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Create WebSearchCriteria.
            webSearchCriteria = new WebTaxonNameSearchCriteria();
            webSearchCriteria.NameSearchMethod = searchCriteria.NameSearchMethod;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.NameSearchMethodSpecified = true;
#endif
            webSearchCriteria.NameSearchString = searchCriteria.NameSearchString;

            // Get data from web service.
            webTaxonNames = WebServiceClient.GetTaxonNamesBySearchCriteria(webSearchCriteria);
            return GetTaxonNames(webTaxonNames);
        }

        /// <summary>
        /// Get the requested taxon name type object.
        /// </summary>
        /// <param name='taxonNameTypeId'>Id of requested taxon name type.</param>
        /// <returns>Requested taxon name type.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon name type has the requested id.</exception>
        public static TaxonNameType GetTaxonNameType(Int32 taxonNameTypeId)
        {
            return GetTaxonNameTypes().Get(taxonNameTypeId);
        }

        /// <summary>
        /// Get all taxon name type objects.
        /// </summary>
        /// <returns>All taxon name types.</returns>
        public static TaxonNameTypeList GetTaxonNameTypes()
        {
            TaxonNameTypeList taxonNameTypes = null;

            for (Int32 getAttempts = 0; (taxonNameTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadTaxonNameTypes();
                taxonNameTypes = TaxonNameTypes;
            }
            return taxonNameTypes;
        }

        /// <summary>
        /// Get the requested taxon name use type object.
        /// </summary>
        /// <param name='taxonNameUseTypeId'>Id of requested taxon name use type.</param>
        /// <returns>Requested taxon name use type.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon name use type has the requested id.</exception>
        public static TaxonNameUseType GetTaxonNameUseType(Int32 taxonNameUseTypeId)
        {
            return GetTaxonNameUseTypes().Get(taxonNameUseTypeId);
        }

        /// <summary>
        /// Get all taxon name use type objects.
        /// </summary>
        /// <returns>All taxon name use types.</returns>
        public static TaxonNameUseTypeList GetTaxonNameUseTypes()
        {
            TaxonNameUseTypeList taxonNameUseTypes = null;

            for (Int32 getAttempts = 0; (taxonNameUseTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadTaxonNameUseTypes();
                taxonNameUseTypes = TaxonNameUseTypes;
            }
            return taxonNameUseTypes;
        }

        /// <summary>
        /// Convert TaxonSearchCriteria to WebTaxonSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">A TaxonSearchCriteria instance.</param>
        /// <returns>A WebTaxonSearchCriteria instance.</returns>
        private static WebTaxonSearchCriteria GetTaxonSearchCriteria(TaxonSearchCriteria searchCriteria)
        {
            WebTaxonSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebTaxonSearchCriteria();
            webSearchCriteria.RestrictReturnToScope = searchCriteria.RestrictReturnToScope;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RestrictReturnToScopeSpecified = true;
#endif
            webSearchCriteria.RestrictReturnToSwedishSpecies = searchCriteria.RestrictReturnToSwedishSpecies;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RestrictReturnToSwedishSpeciesSpecified = true;
#endif
            if (searchCriteria.RestrictReturnToTaxonTypeIds.IsNotNull())
            {
                webSearchCriteria.RestrictReturnToTaxonTypeIds = searchCriteria.RestrictReturnToTaxonTypeIds;
            }
            webSearchCriteria.RestrictSearchToSwedishSpecies = searchCriteria.RestrictSearchToSwedishSpecies;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RestrictSearchToSwedishSpeciesSpecified = true;
#endif
            if (searchCriteria.RestrictSearchToTaxonIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToTaxonIds = searchCriteria.RestrictSearchToTaxonIds;
            }
            if (searchCriteria.RestrictSearchToTaxonTypeIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToTaxonTypeIds = searchCriteria.RestrictSearchToTaxonTypeIds;
            }
            webSearchCriteria.TaxonInformationType = searchCriteria.TaxonInformationType;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.TaxonInformationTypeSpecified = true;
#endif
            webSearchCriteria.TaxonNameSearchString = searchCriteria.TaxonNameSearchString;
            return webSearchCriteria;
        }

        /// <summary>
        /// Get TaxonTreeNode instance with data
        /// from WebTaxonTreeNode instance.
        /// </summary>
        /// <param name="webTaxonTree">WebTaxonTreeNode instance with data.</param>
        /// <returns>TaxonTreeNode instance.</returns>
        private static TaxonTreeNode GetTaxonTree(WebTaxonTreeNode webTaxonTree)
        {
            TaxonTreeNode taxonTree;

            taxonTree = new TaxonTreeNode(GetTaxon(webTaxonTree.Taxon));
            if (webTaxonTree.Children.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode child in webTaxonTree.Children)
                {
                    taxonTree.AddChild(GetTaxonTree(child));
                }
            }
            return taxonTree;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon trees search criteria.</param>
        /// <returns>Taxon trees</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static TaxonTreeNodeList GetTaxonTreesBySearchCriteria(TaxonTreeSearchCriteria searchCriteria)
        {
            TaxonTreeNodeList taxonTrees;
            List<WebTaxonTreeNode> webTaxonTrees;
            WebTaxonTreeSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            webSearchCriteria = GetTaxonTreeSearchCriteria(searchCriteria);
            webTaxonTrees = WebServiceClient.GetTaxonTreesBySearchCriteria(webSearchCriteria);
            taxonTrees = new TaxonTreeNodeList();
            foreach (WebTaxonTreeNode webTaxonTree in webTaxonTrees)
            {
                taxonTrees.Add(GetTaxonTree(webTaxonTree));
            }
            return taxonTrees;
        }

        /// <summary>
        /// Convert TaxonTreeSearchCriteria to WebTaxonTreeSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">A TaxonTreeSearchCriteria instance.</param>
        /// <returns>A WebTaxonTreeSearchCriteria instance.</returns>
        private static WebTaxonTreeSearchCriteria GetTaxonTreeSearchCriteria(TaxonTreeSearchCriteria searchCriteria)
        {
            WebTaxonTreeSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebTaxonTreeSearchCriteria();
            if (searchCriteria.RestrictSearchToTaxonIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToTaxonIds = searchCriteria.RestrictSearchToTaxonIds;
            }
            if (searchCriteria.RestrictSearchToTaxonTypeIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToTaxonTypeIds = searchCriteria.RestrictSearchToTaxonTypeIds;
            }
            webSearchCriteria.TaxonInformationType = searchCriteria.TaxonInformationType;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.TaxonInformationTypeSpecified = true;
#endif
            return webSearchCriteria;
        }

        /// <summary>
        /// Get the requested taxon type object.
        /// </summary>
        /// <param name='taxonTypeId'>Id of requested taxon type.</param>
        /// <returns>Requested taxon type.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon type has the requested id.</exception>
        public static TaxonType GetTaxonType(Int32 taxonTypeId)
        {
            return GetTaxonTypes().Get(taxonTypeId);
        }

        /// <summary>
        /// Get the requested taxon type object.
        /// </summary>
        /// <param name='taxonTypeName'>Name of requested taxon type.</param>
        /// <returns>Requested taxon type.</returns>
        public static TaxonType GetTaxonType(String taxonTypeName)
        {
            foreach (TaxonType taxonType in GetTaxonTypes())
            {
                if (taxonType.Name == taxonTypeName)
                {
                    return taxonType;
                }
            }
            throw new ArgumentException("No taxon type with name " + taxonTypeName + "!");
        }

        /// <summary>
        /// Get the requested taxon type object.
        /// </summary>
        /// <param name='taxonTypeId'>Id of requested taxon type.</param>
        /// <returns>Requested taxon type.</returns>
        public static TaxonType GetTaxonType(TaxonTypeId taxonTypeId)
        {
            return GetTaxonType((Int32)(taxonTypeId));
        }

        /// <summary>
        /// Get all taxon type objects.
        /// </summary>
        /// <returns>All taxon types.</returns>
        public static TaxonTypeList GetTaxonTypes()
        {
            TaxonTypeList taxonTypes = null;

            for (Int32 getAttempts = 0; (taxonTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadTaxonTypes();
                taxonTypes = TaxonTypes;
            }
            return taxonTypes;
        }

        /// <summary>
        /// Get taxon name types from web service.
        /// </summary>
        private static void LoadTaxonNameTypes()
        {
            TaxonNameTypeList taxonNameTypes;

            if (TaxonNameTypes.IsNull())
            {
                // Get data from web service.
                taxonNameTypes = new TaxonNameTypeList();
                foreach (WebTaxonNameType webTaxonNameType in WebServiceClient.GetTaxonNameTypes())
                {
                    taxonNameTypes.Add(new TaxonNameType(webTaxonNameType.Id,
                                                         webTaxonNameType.Name,
                                                         webTaxonNameType.SortOrder));
                }
                TaxonNameTypes = taxonNameTypes;
            }
        }

        /// <summary>
        /// Get taxon name use types from web service.
        /// </summary>
        private static void LoadTaxonNameUseTypes()
        {
            TaxonNameUseTypeList taxonNameUseTypes;

            if (TaxonNameUseTypes.IsNull())
            {
                // Get data from web service.
                taxonNameUseTypes = new TaxonNameUseTypeList();
                foreach (WebTaxonNameUseType webTaxonNameUseType in WebServiceClient.GetTaxonNameUseTypes())
                {
                    taxonNameUseTypes.Add(new TaxonNameUseType(webTaxonNameUseType.Id,
                                                               webTaxonNameUseType.Name));
                }
                TaxonNameUseTypes = taxonNameUseTypes;
            }
        }

        /// <summary>
        /// Get taxon types from web service.
        /// </summary>
        private static void LoadTaxonTypes()
        {
            TaxonTypeList taxonTypes;

            if (TaxonTypes.IsNull())
            {
                // Get data from web service.
                taxonTypes = new TaxonTypeList();
                foreach (WebTaxonType webTaxonType in WebServiceClient.GetTaxonTypes())
                {
                    taxonTypes.Add(new TaxonType(webTaxonType.Id,
                                                 webTaxonType.Name,
                                                 webTaxonType.SortOrder));
                }
                TaxonTypes = taxonTypes;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            lock (_unknownTaxon)
            {
                _unknownTaxon.Clear();
            }
            TaxonNameTypes = null;
            TaxonNameUseTypes = null;
            TaxonTypes = null;
        }
    }
}
