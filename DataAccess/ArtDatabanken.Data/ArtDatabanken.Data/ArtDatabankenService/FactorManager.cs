using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;
using FactorUpdateMode = ArtDatabanken.Data.ArtDatabankenService.FactorUpdateMode;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of factor related objects.
    /// </summary>
    public class FactorManager : ManagerBase
    {
        private static FactorDataTypeList _factorDataTypes = null;
        private static FactorFieldEnumList _factorFieldEnums = null;
        private static FactorFieldTypeList _factorFieldTypes = null;
        private static FactorList _factors = null;
        private static FactorOriginList _factorOrigins = null;
        private static FactorTreeNodeList _factorTrees = null;
        private static FactorTreeNodeList _factorTreeNodes = null;
        private static FactorUpdateModeList _factorUpdateModes = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static FactorManager()
        {
            ManagerBase.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _factorDataTypes thread safe.
        /// </summary>
        private static FactorDataTypeList FactorDataTypes
        {
            get
            {
                FactorDataTypeList factorDataTypes;

                lock (_lockObject)
                {
                    factorDataTypes = _factorDataTypes;
                }
                return factorDataTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorDataTypes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorFieldEnums thread safe.
        /// </summary>
        private static FactorFieldEnumList FactorFieldEnums
        {
            get
            {
                FactorFieldEnumList factorFieldEnums;

                lock (_lockObject)
                {
                    factorFieldEnums = _factorFieldEnums;
                }
                return factorFieldEnums;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorFieldEnums = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorFieldTypes thread safe.
        /// </summary>
        private static FactorFieldTypeList FactorFieldTypes
        {
            get
            {
                FactorFieldTypeList factorFieldTypes;

                lock (_lockObject)
                {
                    factorFieldTypes = _factorFieldTypes;
                }
                return factorFieldTypes;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorFieldTypes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorOrigins thread safe.
        /// </summary>
        private static FactorOriginList FactorOrigins
        {
            get
            {
                FactorOriginList factorOrigins;

                lock (_lockObject)
                {
                    factorOrigins = _factorOrigins;
                }
                return factorOrigins;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorOrigins = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factors thread safe.
        /// </summary>
        private static FactorList Factors
        {
            get
            {
                FactorList factors;

                lock (_lockObject)
                {
                    factors = _factors;
                }
                return factors;
            }
            set
            {
                lock (_lockObject)
                {
                    _factors = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorTreeNodes thread safe.
        /// </summary>
        private static FactorTreeNodeList FactorTreeNodes
        {
            get
            {
                FactorTreeNodeList factorTreeNodes;

                lock (_lockObject)
                {
                    factorTreeNodes = _factorTreeNodes;
                }
                return factorTreeNodes;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorTreeNodes = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorTrees thread safe.
        /// </summary>
        private static FactorTreeNodeList FactorTrees
        {
            get
            {
                FactorTreeNodeList factorTrees;

                lock (_lockObject)
                {
                    factorTrees = _factorTrees;
                }
                return factorTrees;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorTrees = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _factorUpdateModes thread safe.
        /// </summary>
        private static FactorUpdateModeList FactorUpdateModes
        {
            get
            {
                FactorUpdateModeList factorUpdateModes;

                lock (_lockObject)
                {
                    factorUpdateModes = _factorUpdateModes;
                }
                return factorUpdateModes;
            }
            set
            {
                lock (_lockObject)
                {
                    _factorUpdateModes = value;
                }
            }
        }

        /// <summary>
        /// Get the requested factor.
        /// </summary>
        /// <param name='factorId'>Id of requested factor.</param>
        /// <returns>Requested factor.</returns>
        public static Factor GetFactor(FactorId factorId)
        {
            return GetFactor((Int32)factorId);
        }

        /// <summary>
        /// Get the requested factor.
        /// </summary>
        /// <param name='factorId'>Id of requested factor.</param>
        /// <returns>Requested factor.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor has the requested id.</exception>
        public static Factor GetFactor(Int32 factorId)
        {
            return GetFactors().Get(factorId);
        }

        /// <summary>
        /// Get Factor object with information
        /// from a WebFactor object.
        /// </summary>
        /// <param name="webFactor">A WebFactor object</param>
        /// <returns>A Factor object.</returns>
        public static Factor GetFactor(WebFactor webFactor)
        {
            return new Factor(webFactor.Id,
                              webFactor.SortOrder,
                              webFactor.Name,
                              webFactor.Label,
                              webFactor.Information,
                              webFactor.IsTaxonomic,
                              webFactor.HostLabel,
                              webFactor.DefaultHostParentTaxonId,
                              webFactor.FactorUpdateModeId,
                              webFactor.FactorDataTypeId,
                              webFactor.FactorOriginId,
                              webFactor.IsPublic,
                              webFactor.IsPeriodic,
                              webFactor.IsLeaf);
        }

        /// <summary>
        /// Get WebFactor object with information
        /// from a Factor object.
        /// </summary>
        /// <param name="factor">A Factor object</param>
        /// <returns>A WebFactor object.</returns>
        public static WebFactor GetFactor(Factor factor)
        {
            WebFactor webFactor;

            webFactor = new WebFactor();
            webFactor.Id = factor.Id;
#if DATA_SPECIFIED_EXISTS
            webFactor.IdSpecified = true;
#endif
            webFactor.SortOrder = factor.SortOrder;
#if DATA_SPECIFIED_EXISTS
            webFactor.SortOrderSpecified = true;
#endif
            webFactor.Name = factor.Name;
            webFactor.Label = factor.Label;
            webFactor.Information = factor.Information;
            webFactor.IsTaxonomic = factor.IsTaxonomic;
#if DATA_SPECIFIED_EXISTS
            webFactor.IsTaxonomicSpecified = true;
#endif
            webFactor.HostLabel = factor.HostLabel;
            webFactor.DefaultHostParentTaxonId = factor.DefaultHostParentTaxonId;
#if DATA_SPECIFIED_EXISTS
            webFactor.DefaultHostParentTaxonIdSpecified = true;
#endif
            webFactor.FactorUpdateModeId = factor.FactorUpdateMode.Id;
#if DATA_SPECIFIED_EXISTS
            webFactor.FactorUpdateModeIdSpecified = true;
#endif
            webFactor.FactorDataTypeId = factor.FactorDataType.Id;
#if DATA_SPECIFIED_EXISTS
            webFactor.FactorDataTypeIdSpecified = true;
#endif
            webFactor.FactorOriginId = factor.FactorOriginId;
#if DATA_SPECIFIED_EXISTS
            webFactor.FactorOriginIdSpecified = true;
#endif
            webFactor.IsPublic = factor.IsPublic;
#if DATA_SPECIFIED_EXISTS
            webFactor.IsPublicSpecified = true;
#endif
            webFactor.IsPeriodic = factor.IsPeriodic;
#if DATA_SPECIFIED_EXISTS
            webFactor.IsPeriodicSpecified = true;
#endif
            webFactor.IsLeaf = factor.IsLeaf;
#if DATA_SPECIFIED_EXISTS
            webFactor.IsLeafSpecified = true;
#endif
            return webFactor;
        }

        /// <summary>
        /// Get the requested factor data type.
        /// </summary>
        /// <param name='factorDataTypeId'>Id of requested factor data type.</param>
        /// <returns>Requested factor data type.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor data type has the requested id.</exception>
        public static FactorDataType GetFactorDataType(Int32 factorDataTypeId)
        {
            return GetFactorDataTypes().Get(factorDataTypeId);
        }

        /// <summary>
        /// Get all factor data type objects.
        /// </summary>
        /// <returns>All factor data types.</returns>
        public static FactorDataTypeList GetFactorDataTypes()
        {
            FactorDataTypeList factorDataTypes = null;

            for (Int32 getAttempts = 0; (factorDataTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorDataTypes();
                factorDataTypes = FactorDataTypes;
            }
            return factorDataTypes;
        }

        /// <summary>
        /// Convert an FactorField to a WebFactorField.
        /// </summary>
        /// <param name="factorField">A FactorField object</param>
        /// <returns>A WebFactorField object.</returns>
        public static WebFactorField GetFactorField(FactorField factorField)
        {
            WebFactorField webFactorField;

            webFactorField = new WebFactorField();
            webFactorField.FactorDataTypeId = factorField.FactorDataTypeId;
#if DATA_SPECIFIED_EXISTS
            webFactorField.FactorDataTypeIdSpecified = true;
#endif
            if (factorField.Type.DataType == FactorFieldDataTypeId.Enum)
            {
                webFactorField.FactorFieldEnumId = factorField.FactorFieldEnum.Id;
            }
            else
            {
                webFactorField.FactorFieldEnumId = -1;
            }
#if DATA_SPECIFIED_EXISTS
            webFactorField.FactorFieldEnumIdSpecified = true;
#endif
            webFactorField.Id = factorField.Id;
#if DATA_SPECIFIED_EXISTS
            webFactorField.IdSpecified = true;
#endif
            switch (factorField.Index)
            {
                case 0:
                    webFactorField.DatabaseFieldName = "tal1";
                    break;
                case 1:
                    webFactorField.DatabaseFieldName = "tal2";
                    break;
                case 2:
                    webFactorField.DatabaseFieldName = "tal3";
                    break;
                case 3:
                    webFactorField.DatabaseFieldName = "text1";
                    break;
                case 4:
                    webFactorField.DatabaseFieldName = "text2";
                    break;
                default:
                    throw new ApplicationException("Unknown field index = " + factorField.Index);
            }
            webFactorField.Information = factorField.Information;
            webFactorField.IsMain = factorField.IsMain;
#if DATA_SPECIFIED_EXISTS
            webFactorField.IsMainSpecified = true;
#endif
            webFactorField.IsSubstantial = factorField.IsSubstantial;
#if DATA_SPECIFIED_EXISTS
            webFactorField.IsSubstantialSpecified = true;
#endif
            webFactorField.Label = factorField.Label;
            webFactorField.Size = factorField.Size;
#if DATA_SPECIFIED_EXISTS
            webFactorField.SizeSpecified = true;
#endif
            webFactorField.TypeId = factorField.Type.Id;
#if DATA_SPECIFIED_EXISTS
            webFactorField.TypeIdSpecified = true;
#endif
            webFactorField.UnitLabel = factorField.UnitLabel;
            return webFactorField;
        }

        /// <summary>
        /// Get the requested factor field enum.
        /// </summary>
        /// <param name='factorFieldEnumId'>Id of requested factor field enum.</param>
        /// <returns>Requested factor field enum.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor field enum has the requested id.</exception>
        public static FactorFieldEnum GetFactorFieldEnum(FactorFieldEnumId factorFieldEnumId)
        {
            return GetFactorFieldEnum((Int32)factorFieldEnumId);
        }

        /// <summary>
        /// Get the requested factor field enum.
        /// </summary>
        /// <param name='factorFieldEnumId'>Id of requested factor field enum.</param>
        /// <returns>Requested factor field enum.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor field enum has the requested id.</exception>
        public static FactorFieldEnum GetFactorFieldEnum(Int32 factorFieldEnumId)
        {
            return GetFactorFieldEnums().Get(factorFieldEnumId);
        }

        /// <summary>
        /// Get all factor field enum objects.
        /// </summary>
        /// <returns>All factor field enums.</returns>
        public static FactorFieldEnumList GetFactorFieldEnums()
        {
            FactorFieldEnumList factorFieldEnums = null;

            for (Int32 getAttempts = 0; (factorFieldEnums.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorFieldEnums();
                factorFieldEnums = FactorFieldEnums;
            }
            return factorFieldEnums;
        }

        /// <summary>
        /// Get max number of factor fields that
        /// can be used for one factor.
        /// </summary>
        /// <returns>Max number of factor fields.</returns>
        public static Int32 GetFactorFieldMaxCount()
        {
            return 5;
        }

        /// <summary>
        /// Get the requested factor field type.
        /// </summary>
        /// <param name='factorFieldTypeId'>Id of requested factor field type.</param>
        /// <returns>Requested factor field type.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor field type has the requested id.</exception>
        public static FactorFieldType GetFactorFieldType(Int32 factorFieldTypeId)
        {
            return GetFactorFieldTypes().Get(factorFieldTypeId);
        }

        /// <summary>
        /// Get all factor field type objects.
        /// </summary>
        /// <returns>All factor field types.</returns>
        public static FactorFieldTypeList GetFactorFieldTypes()
        {
            FactorFieldTypeList factorFieldTypes = null;

            for (Int32 getAttempts = 0; (factorFieldTypes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorFieldTypes();
                factorFieldTypes = FactorFieldTypes;
            }
            return factorFieldTypes;
        }

        /// <summary>
        /// Get the requested factor origin object.
        /// </summary>
        /// <param name='factorOriginId'>Id of requested factor origin.</param>
        /// <returns>Requested factor origin.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor update mode has the requested id.</exception>
        public static FactorOrigin GetFactorOrigin(Int32 factorOriginId)
        {
            return GetFactorOrigins().Get(factorOriginId);
        }

        /// <summary>
        /// Get all factor origin objects.
        /// </summary>
        /// <returns>All factor origind.</returns>
        public static FactorOriginList GetFactorOrigins()
        {
            FactorOriginList factorOrigins = null;

            for (Int32 getAttempts = 0; (factorOrigins.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorOrigins();
                factorOrigins = FactorOrigins;
            }
            return factorOrigins;
        }

        /// <summary>
        /// Get all factor objects.
        /// </summary>
        /// <returns>All factors.</returns>
        public static FactorList GetFactors()
        {
            FactorList factors = null;

            for (Int32 getAttempts = 0; (factors.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorInformation();
                factors = Factors;
            }
            return factors;
        }

        /// <summary>
        /// Get information about factors.
        /// </summary>
        /// <param name="factorIds">Ids for factors to get information about.</param>
        /// <returns>Factors information.</returns>
        public static FactorList GetFactors(List<Int32> factorIds)
        {
            FactorList factors;

            // Check arguments.
            factorIds.CheckNotEmpty("factorIds");

            factors = new FactorList();
            foreach (Int32 factorId in factorIds)
            {
                factors.Add(GetFactor(factorId));
            }
            return factors;
        }

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Information about factors.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static FactorList GetFactorsBySearchCriteria(FactorSearchCriteria searchCriteria)
        {
            FactorList factors;
            List<WebFactor> webFactors;
            FactorTreeNode factorTreeNode;
            WebFactorSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            factors = new FactorList();
            if (searchCriteria.FactorNameSearchString.IsEmpty() &&
                (searchCriteria.RestrictSearchToScope == WebService.FactorSearchScope.NoScope) &&
                (searchCriteria.RestrictReturnToScope == WebService.FactorSearchScope.AllChildFactors) &&
                searchCriteria.RestrictSearchToFactorIds.IsNotEmpty())
            {
                // Get all child factors to specified factor trees.
                foreach (Int32 factorId in searchCriteria.RestrictSearchToFactorIds)
                {
                    factorTreeNode = GetFactorTreeNodes().Get(factorId);
                    factors.Merge(factorTreeNode.GetAllChildFactors());
                }
                return factors;
            }
            if (searchCriteria.FactorNameSearchString.IsEmpty() &&
                (searchCriteria.RestrictSearchToScope == WebService.FactorSearchScope.NoScope) &&
                (searchCriteria.RestrictReturnToScope == WebService.FactorSearchScope.LeafFactors) &&
                searchCriteria.RestrictSearchToFactorIds.IsNotEmpty())
            {
                // Get all leafs to specified factor trees.
                foreach (Int32 factorId in searchCriteria.RestrictSearchToFactorIds)
                {
                    factorTreeNode = GetFactorTreeNodes().Get(factorId);
                    factors.Merge(factorTreeNode.GetAllLeafFactors());
                }
                return factors;
            }

            // Get data from web service.
            webSearchCriteria = GetFactorSearchCriteria(searchCriteria);
            webFactors = WebServiceClient.GetFactorsBySearchCriteria(webSearchCriteria);
            foreach (WebFactor webFactor in webFactors)
            {
                factors.Add(GetFactor(webFactor));
            }
            return factors;
        }

        /// <summary>
        /// Convert FactorSearchCriteria to WebFactorSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">A FactorSearchCriteria instance.</param>
        /// <returns>A WebFactorSearchCriteria instance.</returns>
        private static WebFactorSearchCriteria GetFactorSearchCriteria(FactorSearchCriteria searchCriteria)
        {
            WebFactorSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebFactorSearchCriteria();

            webSearchCriteria.NameSearchString = searchCriteria.FactorNameSearchString;
            webSearchCriteria.NameSearchMethod = searchCriteria.NameSearchMethod;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.NameSearchMethodSpecified = true;
#endif
            webSearchCriteria.RestrictSearchToScope = searchCriteria.RestrictSearchToScope;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RestrictSearchToScopeSpecified = true;
#endif
            webSearchCriteria.IsIdInNameSearchString = searchCriteria.IdInNameSearchString;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IsIdInNameSearchStringSpecified = true;
#endif
            webSearchCriteria.RestrictReturnToScope = searchCriteria.RestrictReturnToScope;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RestrictReturnToScopeSpecified = true;
#endif
            if (searchCriteria.RestrictSearchToFactorIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToFactorIds = searchCriteria.RestrictSearchToFactorIds;
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="factorId">Factor id.</param>
        /// <returns>Specified factor tree node.</returns>
        public static FactorTreeNode GetFactorTree(FactorId factorId)
        {
            return GetFactorTreeNodes().Get((Int32)factorId);
        }

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="factorId">Factor id.</param>
        /// <returns>Specified factor tree node.</returns>
        public static FactorTreeNode GetFactorTree(Int32 factorId)
        {
            return GetFactorTreeNodes().Get(factorId);
        }

        /// <summary>
        /// Get FactorTreeNode instance with data
        /// from WebFactorTreeNode instance.
        /// </summary>
        /// <param name="webFactorTree">WebFactorTreeNode instance with data.</param>
        /// <returns>FactorTreeNode instance.</returns>
        private static FactorTreeNode GetFactorTree(WebFactorTreeNode webFactorTree)
        {
            Factor factor;
            FactorTreeNode factorTree;

            factor = GetFactor(webFactorTree.Factor);
            factorTree = new FactorTreeNode(factor);
            if (webFactorTree.Children.IsNotEmpty())
            {
                foreach (WebFactorTreeNode child in webFactorTree.Children)
                {
                    factorTree.AddChild(GetFactorTree(child));
                }
            }
            return factorTree;
        }

        /// <summary>
        /// Get all factor tree nodes.
        /// </summary>
        /// <returns>All factor tree nodes.</returns>
        private static FactorTreeNodeList GetFactorTreeNodes()
        {
            FactorTreeNodeList factorTreeNodes = null;

            for (Int32 getAttempts = 0; (factorTreeNodes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorInformation();
                factorTreeNodes = FactorTreeNodes;
            }
            return factorTreeNodes;
        }

        /// <summary>
        /// Get all top factor tree nodes.
        /// </summary>
        /// <returns>All top factor tree nodes.</returns>
        public static FactorTreeNodeList GetFactorTrees()
        {
            FactorTreeNodeList factorTrees = null;

            for (Int32 getAttempts = 0; (factorTrees.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorInformation();
                factorTrees = FactorTrees;
            }
            return factorTrees;
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The factor trees search criteria.</param>
        /// <returns>Factor trees</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static FactorTreeNodeList GetFactorTreesBySearchCriteria(FactorTreeSearchCriteria searchCriteria)
        {
            FactorTreeNodeList factorTrees;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            factorTrees = new FactorTreeNodeList();
            foreach (Int32 factorId in searchCriteria.RestrictSearchToFactorIds)
            {
                factorTrees.Add(GetFactorTreeNodes().Get(factorId));
            }

            return factorTrees;
        }

        /// <summary>
        /// Convert FactorTreeSearchCriteria to WebFactorTreeSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">A FactorTreeSearchCriteria instance.</param>
        /// <returns>A WebFactorTreeSearchCriteria instance.</returns>
        private static WebFactorTreeSearchCriteria GetFactorTreeSearchCriteria(FactorTreeSearchCriteria searchCriteria)
        {
            WebFactorTreeSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebFactorTreeSearchCriteria();
            if (searchCriteria.RestrictSearchToFactorIds.IsNotNull())
            {
                webSearchCriteria.RestrictSearchToFactorIds = searchCriteria.RestrictSearchToFactorIds;
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Get the requested factor update mode object.
        /// </summary>
        /// <param name='factorUpdateModeId'>Id of requested factor update mode.</param>
        /// <returns>Requested factor update mode.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor update mode has the requested id.</exception>
        public static FactorUpdateMode GetFactorUpdateMode(Int32 factorUpdateModeId)
        {
            return GetFactorUpdateModes().Get(factorUpdateModeId);
        }

        /// <summary>
        /// Get all factor update mode objects.
        /// </summary>
        /// <returns>All factor update modes.</returns>
        public static FactorUpdateModeList GetFactorUpdateModes()
        {
            FactorUpdateModeList factorUpdateModes = null;

            for (Int32 getAttempts = 0; (factorUpdateModes.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadFactorUpdateModes();
                factorUpdateModes = FactorUpdateModes;
            }
            return factorUpdateModes;
        }

        /// <summary>
        /// Initialises the internal cache of all factors. Primarily used when user is working offline.
        /// </summary>
        /// <param name="allFactors">List with all available factors</param>
        /// <param name="allTrees">All factor trees.</param>
        public static void InitialiseAllFactors(FactorList allFactors, FactorTreeNodeList allTrees)
        {
            Factors = allFactors;
            FactorTrees = allTrees;
        }

        /// <summary>
        /// Get factor data types from web service.
        /// </summary>
        private static void LoadFactorDataTypes()
        {
            Int32 factorFieldIndex, sortOrder = 0;
            FactorDataTypeList factorDataTypes;
            FactorFieldList factorFields;

            if (FactorDataTypes.IsNull())
            {
                // Get data from web service.
                factorDataTypes = new FactorDataTypeList(true);
                foreach (WebFactorDataType webFactorDataType in WebServiceClient.GetFactorDataTypes())
                {
                    factorFields = new FactorFieldList();
                    foreach (WebFactorField webFactorField in webFactorDataType.Fields)
                    {
                        switch (webFactorField.DatabaseFieldName)
                        {
                            case "tal1":
                                factorFieldIndex = 0;
                                break;
                            case "tal2":
                                factorFieldIndex = 1;
                                break;
                            case "tal3":
                                factorFieldIndex = 2;
                                break;
                            case "text1":
                                factorFieldIndex = 3;
                                break;
                            case "text2":
                                factorFieldIndex = 4;
                                break;
                            default:
                                throw new ApplicationException("Unknown database field name = " + webFactorField.DatabaseFieldName);
                        }
                        factorFields.Add(new FactorField(webFactorField.Id,
                                                         sortOrder++,
                                                         webFactorField.FactorDataTypeId,
                                                         factorFieldIndex,
                                                         webFactorField.Label,
                                                         webFactorField.Information,
                                                         webFactorField.IsMain,
                                                         webFactorField.IsSubstantial,
                                                         webFactorField.TypeId,
                                                         webFactorField.Size,
                                                         webFactorField.FactorFieldEnumId,
                                                         webFactorField.UnitLabel));
                    }

                    factorDataTypes.Add(new FactorDataType(webFactorDataType.Id,
                                                           webFactorDataType.Id,
                                                           webFactorDataType.Name,
                                                           webFactorDataType.Definition,
                                                           factorFields));
                }
                FactorDataTypes = factorDataTypes;
            }
        }

        /// <summary>
        /// Get factor field enums from web service.
        /// </summary>
        private static void LoadFactorFieldEnums()
        {
            FactorFieldEnumList factorFieldEnums;
            FactorFieldEnumValueList values;

            if (FactorFieldEnums.IsNull())
            {
                // Get data from web service.
                factorFieldEnums = new FactorFieldEnumList(true);
                foreach (WebFactorFieldEnum webFactorFieldEnum in WebServiceClient.GetFactorFieldEnums())
                {
                    values = new FactorFieldEnumValueList();
                    foreach (WebFactorFieldEnumValue webFactorFieldEnumValue in webFactorFieldEnum.Values)
                    {
                        values.Add(new FactorFieldEnumValue(webFactorFieldEnumValue.Id,
                                                            webFactorFieldEnumValue.FactorFieldEnumId,
                                                            webFactorFieldEnumValue.KeyText,
                                                            webFactorFieldEnumValue.KeyInteger,
                                                            webFactorFieldEnumValue.IsKeyIntegerSpecified,
                                                            webFactorFieldEnumValue.Label,
                                                            webFactorFieldEnumValue.Information,
                                                            webFactorFieldEnumValue.ShouldBeSaved,
                                                            webFactorFieldEnumValue.SortOrder));
                    }
                    factorFieldEnums.Add(new FactorFieldEnum(webFactorFieldEnum.Id, webFactorFieldEnum.Id, values));
                }
                FactorFieldEnums = factorFieldEnums;
            }
        }

        /// <summary>
        /// Get factor field types from web service.
        /// </summary>
        private static void LoadFactorFieldTypes()
        {
            FactorFieldTypeList factorFieldTypes;

            if (FactorFieldTypes.IsNull())
            {
                // Get data from web service.
                factorFieldTypes = new FactorFieldTypeList(true);
                foreach (WebFactorFieldType webFactorFieldType in WebServiceClient.GetFactorFieldTypes())
                {
                    factorFieldTypes.Add(new FactorFieldType(webFactorFieldType.Id,
                                                             webFactorFieldType.Id,
                                                             webFactorFieldType.Name,
                                                             webFactorFieldType.Definition));
                }
                FactorFieldTypes = factorFieldTypes;
            }
        }

        /// <summary>
        /// Get information about factors and 
        /// factor trees from web service.
        /// </summary>
        private static void LoadFactorInformation()
        {
            FactorList factors;
            FactorTreeNodeList factorTreeNodes, factorTrees;

            if (FactorTreeNodes.IsNull() &&
                UserManager.IsUserLoggedIn())
            {
                // Get data from web service.
                factors = new FactorList(true);
                factorTreeNodes = new FactorTreeNodeList();
                factorTrees = new FactorTreeNodeList();
                foreach (WebFactorTreeNode webFactorTree in WebServiceClient.GetFactorTrees())
                {
                    factorTrees.Add(GetFactorTree(webFactorTree));
                }

                // Extract factor tree nodes and factors information.
                foreach (FactorTreeNode factorTree in factorTrees)
                {
                    LoadFactorInformation(factorTree, factorTreeNodes, factors);
                }
                lock (_lockObject)
                {
                    Factors = factors;
                    FactorTreeNodes = factorTreeNodes;
                    FactorTrees = factorTrees;
                }
            }
        }

        /// <summary>
        /// Extract factor and factor tree nodes from factor tree
        /// </summary>
        /// <param name="factorTree">Factor tree to extract information from.</param>
        /// <param name="factorTreeNodes">Information about factor tree nodes in tree.</param>
        /// <param name="factors">Information about factors in tree.</param>
        private static void LoadFactorInformation(FactorTreeNode factorTree,
                                                  FactorTreeNodeList factorTreeNodes,
                                                  FactorList factors)
        {
            factorTreeNodes.Merge(factorTree);
            factors.Merge(factorTree.Factor);
            if (factorTree.Children.IsNotEmpty())
            {
                foreach (FactorTreeNode child in factorTree.Children)
                {
                    LoadFactorInformation(child, factorTreeNodes, factors);
                }
            }
        }

        /// <summary>
        /// Get factor origins from web service.
        /// </summary>
        private static void LoadFactorOrigins()
        {
            FactorOriginList factorOrigins;

            if (FactorOrigins.IsNull())
            {
                // Get data from web service.
                factorOrigins = new FactorOriginList(true);

                foreach (WebFactorOrigin webFactorOrigin in WebServiceClient.GetFactorOrigins())
                {
                    factorOrigins.Add(new FactorOrigin(webFactorOrigin.Id,
                                                       webFactorOrigin.Name,
                                                       webFactorOrigin.Definition,
                                                       webFactorOrigin.SortOrder));
                }
                FactorOrigins = factorOrigins;
            }
        }

        /// <summary>
        /// Get factor update modes from web service.
        /// </summary>
        private static void LoadFactorUpdateModes()
        {
            FactorUpdateModeList factorUpdateModes = null;

            if (FactorUpdateModes.IsNull())
            {
                // Get data from web service.
                factorUpdateModes = new FactorUpdateModeList(true);
                foreach (WebFactorUpdateMode webFactorUpdateMode in WebServiceClient.GetFactorUpdateModes())
                {
                    factorUpdateModes.Add(new FactorUpdateMode(webFactorUpdateMode.Id,
                                                               webFactorUpdateMode.Name,
                                                               webFactorUpdateMode.Type,
                                                               webFactorUpdateMode.Definition,
                                                               webFactorUpdateMode.IsUpdateAllowed));
                }
                FactorUpdateModes = factorUpdateModes;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            lock (_lockObject)
            {
                FactorDataTypes = null;
                FactorFieldEnums = null;
                FactorFieldTypes = null;
                FactorOrigins = null;
                Factors = null;
                FactorTreeNodes = null;
                FactorTrees = null;
                FactorUpdateModes = null;
            }
        }
    }
}
