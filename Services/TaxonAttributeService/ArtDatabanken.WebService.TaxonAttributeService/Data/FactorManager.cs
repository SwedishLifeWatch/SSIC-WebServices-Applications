using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Handles factor information.
    /// </summary>
    public static class FactorManager
    {
        /// <summary>
        /// Indicates if factor related information has been changed.
        /// </summary>
        private static Boolean _isFactorInformationUpdated;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static FactorManager()
        {
            _isFactorInformationUpdated = false;
            WebServiceContext.CommitTransactionEvent += RemoveCachedObjects;
        }

        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor data types.</returns>
        public static List<WebFactorDataType> GetFactorDataTypes(WebServiceContext context)
        {
            Int32 factorDataTypeId;
            List<WebFactorDataType> factorDataTypes;
            String cacheKey;
            WebFactorDataType factorDataType;
            WebFactorField factorField;

            // Get cached information.
            factorDataTypes = null;
            cacheKey = Settings.Default.FactorDataTypeCacheKey;
            if (!context.IsInTransaction())
            {
                factorDataTypes = (List<WebFactorDataType>)context.GetCachedObject(cacheKey);
            }

            if (factorDataTypes.IsNull())
            {
                // Get information from database.
                factorDataTypes = new List<WebFactorDataType>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorFields())
                {
                    while (dataReader.Read())
                    {
                        // Get factor data type.
                        factorDataType = null;
                        factorDataTypeId = dataReader.GetInt32(FactorFieldData.FACTOR_DATA_TYPE_ID);
                        foreach (WebFactorDataType tempFactorDataType in factorDataTypes)
                        {
                            if (factorDataTypeId == tempFactorDataType.Id)
                            {
                                factorDataType = tempFactorDataType;
                            }
                        }

                        if (factorDataType.IsNull())
                        {
                            dataReader.ColumnNamePrefix = FactorDataTypeData.COLUMN_NAME_PREFIX;
                            factorDataType = new WebFactorDataType();
                            factorDataType.LoadData(dataReader);
                            factorDataTypes.Add(factorDataType);
                            dataReader.ColumnNamePrefix = null;
                        }

                        // Add field.
                        factorField = new WebFactorField();
                        factorField.LoadData(dataReader);

                        // ReSharper disable once PossibleNullReferenceException
                        factorDataType.Fields.Add(factorField);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey, factorDataTypes, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                    }
                }
            }

            return factorDataTypes;
        }

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor field enumerations.</returns>
        public static List<WebFactorFieldEnum> GetFactorFieldEnums(WebServiceContext context)
        {
            Int32 factorFieldEnumId;
            List<WebFactorFieldEnum> factorFieldEnums;
            String cacheKey;
            WebFactorFieldEnum factorFieldEnum;
            WebFactorFieldEnumValue factorFieldEnumValue;

            // Get cached information.
            factorFieldEnums = null;
            cacheKey = Settings.Default.FactorFieldEnumCacheKey;
            if (!context.IsInTransaction())
            {
                factorFieldEnums = (List<WebFactorFieldEnum>)context.GetCachedObject(cacheKey);
            }

            if (factorFieldEnums.IsNull())
            {
                // Get information from database.
                factorFieldEnums = new List<WebFactorFieldEnum>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorFieldEnums())
                {
                    while (dataReader.Read())
                    {
                        // Get factor field enum.
                        factorFieldEnum = null;
                        factorFieldEnumId = dataReader.GetInt32(FactorFieldEnumValueData.FACTOR_FIELD_ENUM_ID);
                        foreach (WebFactorFieldEnum tempFactorFieldEnum in factorFieldEnums)
                        {
                            if (factorFieldEnumId == tempFactorFieldEnum.Id)
                            {
                                factorFieldEnum = tempFactorFieldEnum;
                            }
                        }

                        if (factorFieldEnum.IsNull())
                        {
                            dataReader.ColumnNamePrefix = FactorFieldEnumData.COLUMN_NAME_PREFIX;
                            factorFieldEnum = new WebFactorFieldEnum();
                            factorFieldEnum.LoadData(dataReader);
                            factorFieldEnums.Add(factorFieldEnum);
                            dataReader.ColumnNamePrefix = null;
                        }

                        // Add enum value.
                        factorFieldEnumValue = new WebFactorFieldEnumValue();
                        factorFieldEnumValue.LoadData(dataReader);
                        // ReSharper disable once PossibleNullReferenceException
                        factorFieldEnum.Values.Add(factorFieldEnumValue);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey, factorFieldEnums, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                    }
                }
            }

            return factorFieldEnums;
        }

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor field types.</returns>
        public static List<WebFactorFieldType> GetFactorFieldTypes(WebServiceContext context)
        {
            List<WebFactorFieldType> factorFieldTypes;
            String cacheKey;
            WebFactorFieldType factorFieldType;

            // Get cached information.
            factorFieldTypes = null;
            cacheKey = Settings.Default.FactorFieldTypeCacheKey;
            if (!context.IsInTransaction())
            {
                factorFieldTypes = (List<WebFactorFieldType>)context.GetCachedObject(cacheKey);
            }

            if (factorFieldTypes.IsNull())
            {
                // Get information from database.
                factorFieldTypes = new List<WebFactorFieldType>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorFieldTypes())
                {
                    while (dataReader.Read())
                    {
                        factorFieldType = new WebFactorFieldType();
                        factorFieldType.LoadData(dataReader);
                        factorFieldTypes.Add(factorFieldType);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey,
                                                factorFieldTypes,
                                                DateTime.Now + new TimeSpan(12, 0, 0),
                                                CacheItemPriority.AboveNormal);
                    }
                }
            }

            return factorFieldTypes;
        }

        /// <summary>
        /// Check each role of current user if any authority has identifier SpeciesFact.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>True means user have right to read non public factors and public factors.</returns>
        private static Boolean IsUserAuthorizedToReadNonPublicFactors(WebServiceContext context)
        {
            Boolean addNonPublicFactor = false;

            // Check if non-public factors should be returned
            foreach (WebRole role in context.CurrentRoles)
            {
                if (role.Authorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if ((authority.Identifier == AuthorityIdentifier.EditSpeciesFacts.ToString()) ||
                            (authority.Identifier == AuthorityIdentifier.SpeciesFact.ToString()))
                        {
                            if (authority.ShowNonPublicData)
                            {
                                addNonPublicFactor = true;
                                break;
                            }
                        }
                    }
                }

                if (addNonPublicFactor)
                {
                    break;
                }
            }

            return addNonPublicFactor;
        }

        /// <summary>
        /// Collect factor information in two cache objects: one with all factors and one with only public factors.
        /// </summary>
        /// <param name="cacheAllFactors">True will cache all factors and False will cache only public factors.</param>
        /// <returns>Return Cache key.</returns>
        private static String GetFactorInformationCacheKeyByUserRole(Boolean cacheAllFactors)
        {
            String cacheKey;

            cacheKey = Settings.Default.FactorInformationCacheKey + "#" + Settings.Default.FactorInformationFullAccessRight + ":" + cacheAllFactors;

            return cacheKey;
        }

        /// <summary>
        /// Get information about factor trees and factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="getOnlyPublicFactors">If true: Get only public factor trees. If false: get all factor trees. Default is true.</param>
        /// <returns>Factor information.</returns>
        private static FactorInformation GetFactorInformation(WebServiceContext context, Boolean getOnlyPublicFactors = true)
        {
            FactorInformation factorInformation;
            String cacheKey;
            Boolean cacheAllFactors = !getOnlyPublicFactors;

            // Get cached information.
            cacheKey = GetFactorInformationCacheKeyByUserRole(cacheAllFactors);

            factorInformation = (FactorInformation)context.GetCachedObject(cacheKey);
            if (factorInformation.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorTrees(getOnlyPublicFactors))
                {
                    factorInformation = new FactorInformation(context, dataReader);

                    // Add information to the cache.
                    context.AddCachedObject(cacheKey, factorInformation, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            
            return factorInformation;
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All factor origins.</returns>
        public static List<WebFactorOrigin> GetFactorOrigins(WebServiceContext context)
        {
            List<WebFactorOrigin> factorOrigins;
            String cacheKey;
            WebFactorOrigin factorOrigin;

            // Get cached information.
            factorOrigins = null;
            cacheKey = Settings.Default.FactorOriginCacheKey;
            if (!context.IsInTransaction())
            {
                factorOrigins = (List<WebFactorOrigin>)context.GetCachedObject(cacheKey);
            }

            if (factorOrigins.IsNull())
            {
                // Get information from database.
                factorOrigins = new List<WebFactorOrigin>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorOrigins())
                {
                    while (dataReader.Read())
                    {
                        factorOrigin = new WebFactorOrigin();
                        factorOrigin.LoadData(dataReader);
                        factorOrigins.Add(factorOrigin);
                    }
                }

                if (!context.IsInTransaction())
                {
                    // Add information to cache.
                    context.AddCachedObject(cacheKey,
                                            factorOrigins,
                                            DateTime.Now + new TimeSpan(12, 0, 0),
                                            CacheItemPriority.AboveNormal);
                }
            }

            return factorOrigins;
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All factors.</returns>
        public static List<WebFactor> GetFactors(WebServiceContext context)
        {
            List<WebFactor> factors;
            WebFactor factor;
            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(context);

            // Get cached information.
            factors = null;
            if (!context.IsInTransaction())
            {
                factors = GetFactorInformation(context, getOnlyPublicFactors).Factors;
            }

            if (factors.IsNull())
            {
                // Get information from database.
                factors = new List<WebFactor>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactors(getOnlyPublicFactors))
                {
                    while (dataReader.Read())
                    {
                        factor = new WebFactor();
                        factor.LoadData(dataReader);
                        factors.Add(factor);
                    }
                }
            }

            return factors;
        }

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Factors that matches the search criteria.</returns>
        public static List<WebFactor> GetFactorsBySearchCriteria(WebServiceContext context,
                                                                 WebFactorSearchCriteria searchCriteria)
        {
            List<WebFactor> factors;
            Int32 factorId;
            String nameSearchMethodString, nameSearchString;
            WebFactor factor;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Test if name search string is a factor id.
            if (searchCriteria.NameSearchString.IsNotNull() &&
                searchCriteria.NameSearchString.SearchString.IsNotEmpty() &&
                searchCriteria.IsIdInNameSearchString &&
                Int32.TryParse(searchCriteria.NameSearchString.SearchString, out factorId))
            {
                if (searchCriteria.RestrictSearchToFactorIds.IsNull())
                {
                    searchCriteria.RestrictSearchToFactorIds = new List<Int32>();
                }

                searchCriteria.RestrictSearchToFactorIds.Add(factorId);
                searchCriteria.NameSearchString = null;
            }

            // Get information about factors from database.
            factors = new List<WebFactor>();
            if (searchCriteria.NameSearchString.IsNotNull())
            {
                // ReSharper disable once PossibleNullReferenceException
                foreach (StringCompareOperator compareOperator in searchCriteria.NameSearchString.CompareOperators)
                {
                    switch (compareOperator)
                    {
                        case StringCompareOperator.BeginsWith:
                            nameSearchMethodString = "Like";
                            nameSearchString = searchCriteria.NameSearchString.SearchString + "%";
                            break;
                        case StringCompareOperator.Contains:
                            nameSearchMethodString = "Like";
                            nameSearchString = "%" + searchCriteria.NameSearchString.SearchString + "%";
                            break;
                        case StringCompareOperator.EndsWith:
                            nameSearchMethodString = "Like";
                            nameSearchString = "%" + searchCriteria.NameSearchString.SearchString;
                            break;
                        case StringCompareOperator.Equal:
                            nameSearchMethodString = "Exact";
                            nameSearchString = searchCriteria.NameSearchString.SearchString;
                            break;
                        case StringCompareOperator.Like:
                            nameSearchMethodString = "Like";
                            nameSearchString = searchCriteria.NameSearchString.SearchString;
                            break;
                        default:
                            throw new ArgumentException("GetFactorsBySearchCriteria Not supported string compare operator = " + compareOperator);
                    }

                    using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorsBySearchCriteria(searchCriteria.RestrictSearchToFactorIds,
                                                                                                                  nameSearchString,
                                                                                                                  nameSearchMethodString,
                                                                                                                  searchCriteria.RestrictSearchToScope.ToString(),
                                                                                                                  searchCriteria.RestrictReturnToScope.ToString()))
                    {
                        while (dataReader.Read())
                        {
                            factor = new WebFactor();
                            factor.LoadData(dataReader);
                            factors.Add(factor);
                        }
                    }

                    if (factors.IsNotEmpty())
                    {
                        break;
                    }
                }
            }
            else
            {
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorsBySearchCriteria(searchCriteria.RestrictSearchToFactorIds,
                                                                                                              null,
                                                                                                              "Exact",
                                                                                                              searchCriteria.RestrictSearchToScope.ToString(),
                                                                                                              searchCriteria.RestrictReturnToScope.ToString()))
                {
                    while (dataReader.Read())
                    {
                        factor = new WebFactor();
                        factor.LoadData(dataReader);
                        factors.Add(factor);
                    }
                }
            }

            return factors;
        }

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All factor trees.</returns>
        public static List<WebFactorTreeNode> GetFactorTrees(WebServiceContext context)
        {
            FactorInformation factorInformation;
            List<WebFactorTreeNode> factorTrees;
            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(context);

            // Get cached information.
            factorTrees = null;
            if (!context.IsInTransaction())
            {
                factorTrees = GetFactorInformation(context, getOnlyPublicFactors).FactorTrees;
            }

            if (factorTrees.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorTrees(getOnlyPublicFactors))
                {
                    factorInformation = new FactorInformation(context, dataReader);
                }

                factorTrees = factorInformation.FactorTrees;
            }

            return factorTrees;
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Factor tree information.</param>
        /// <returns>Filtered factor trees.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        public static List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebServiceContext context,
                                                                             WebFactorTreeSearchCriteria searchCriteria)
        {
            FactorInformation factorInformation;
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeNodeList factorTreeNodes;
            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(context);

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();
            
            if (searchCriteria.FactorIds.IsEmpty())
            {
                // Get all factor trees.
                factorTrees = GetFactorTrees(context);
            }
            else
            {
                // Get all factor tree nodes.
                if (context.IsInTransaction())
                {
                    // Get information from database.
                    using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorTrees(getOnlyPublicFactors))
                    {
                        factorInformation = new FactorInformation(context, dataReader);
                    }

                    factorTreeNodes = factorInformation.FactorTreeNodes;
                }
                else
                {
                    factorTreeNodes = GetFactorInformation(context, getOnlyPublicFactors).FactorTreeNodes;
                }

                // Get all factor trees that are requested.
                factorTrees = new List<WebFactorTreeNode>();
                foreach (Int32 factorId in searchCriteria.FactorIds)
                {
                    if (factorTreeNodes.Contains(factorId))
                    {
                        factorTrees.Add(factorTreeNodes.Get(factorId));
                    }
                }
            }

            return factorTrees;
        }

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor update modes.</returns>
        public static List<WebFactorUpdateMode> GetFactorUpdateModes(WebServiceContext context)
        {
            List<WebFactorUpdateMode> factorUpdateModes;
            String cacheKey;
            WebFactorUpdateMode factorUpdateMode;

            // Get cached information.
            factorUpdateModes = null;
            cacheKey = Settings.Default.FactorUpdateModeCacheKey;
            if (!context.IsInTransaction())
            {
                factorUpdateModes = (List<WebFactorUpdateMode>)context.GetCachedObject(cacheKey);
            }

            if (factorUpdateModes.IsNull())
            {
                // Get information from database.
                factorUpdateModes = new List<WebFactorUpdateMode>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetFactorUpdateModes())
                {
                    while (dataReader.Read())
                    {
                        factorUpdateMode = new WebFactorUpdateMode();
                        factorUpdateMode.LoadData(dataReader);
                        factorUpdateModes.Add(factorUpdateMode);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey,
                                                factorUpdateModes,
                                                DateTime.Now + new TimeSpan(12, 0, 0),
                                                CacheItemPriority.AboveNormal);
                    }
                }
            }

            return factorUpdateModes;
        }

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All individual categories.</returns>
        public static List<WebIndividualCategory> GetIndividualCategories(WebServiceContext context)
        {
            List<WebIndividualCategory> individualCategories;
            String cacheKey;
            WebIndividualCategory individualCategory;

            // Get cached information.
            individualCategories = null;
            cacheKey = Settings.Default.IndividualCategoryCacheKey;
            if (!context.IsInTransaction())
            {
                individualCategories = (List<WebIndividualCategory>)context.GetCachedObject(cacheKey);
            }

            if (individualCategories.IsNull())
            {
                // Get information from database.
                individualCategories = new List<WebIndividualCategory>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetIndividualCategories())
                {
                    while (dataReader.Read())
                    {
                        individualCategory = new WebIndividualCategory();
                        individualCategory.LoadData(dataReader);
                        individualCategories.Add(individualCategory);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey,
                                                individualCategories,
                                                DateTime.Now + new TimeSpan(12, 0, 0),
                                                CacheItemPriority.AboveNormal);
                    }
                }
            }

            return individualCategories;
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All periods.</returns>
        public static List<WebPeriod> GetPeriods(WebServiceContext context)
        {
            List<WebPeriod> periods;
            String cacheKey;
            WebPeriod period;

            // Get cached information.
            periods = null;
            cacheKey = Settings.Default.PeriodCacheKey;
            if (!context.IsInTransaction())
            {
                periods = (List<WebPeriod>)context.GetCachedObject(cacheKey);
            }

            if (periods.IsNull())
            {
                // Get information from database.
                periods = new List<WebPeriod>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetPeriods())
                {
                    while (dataReader.Read())
                    {
                        period = new WebPeriod();
                        period.LoadData(dataReader);
                        periods.Add(period);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey,
                                                periods,
                                                DateTime.Now + new TimeSpan(12, 0, 0),
                                                CacheItemPriority.AboveNormal);
                    }
                }
            }

            return periods;
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Period types.</returns>
        public static List<WebPeriodType> GetPeriodTypes(WebServiceContext context)
        {
            List<WebPeriodType> periodTypes;
            String cacheKey;
            WebPeriodType periodType;

            // Get cached information.
            periodTypes = null;
            cacheKey = Settings.Default.PeriodTypeCacheKey;
            if (!context.IsInTransaction())
            {
                periodTypes = (List<WebPeriodType>)context.GetCachedObject(cacheKey);
            }

            if (periodTypes.IsNull())
            {
                // Get information from database.
                periodTypes = new List<WebPeriodType>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetPeriodTypes())
                {
                    while (dataReader.Read())
                    {
                        periodType = new WebPeriodType();
                        periodType.LoadData(dataReader);
                        periodTypes.Add(periodType);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey,
                                                periodTypes,
                                                DateTime.Now + new TimeSpan(12, 0, 0),
                                                CacheItemPriority.AboveNormal);
                    }
                }
            }

            return periodTypes;
        }

        /// <summary>
        /// Remove information objects from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void RemoveCachedObjects(WebServiceContext context)
        {
            string cacheKey;

            if (_isFactorInformationUpdated)
            {
                _isFactorInformationUpdated = false;
                cacheKey = Settings.Default.FactorDataTypeCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorFieldEnumCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorFieldTypeCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorInformationCacheKey + "#" + Settings.Default.FactorInformationFullAccessRight + ":" + true;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorInformationCacheKey + "#" + Settings.Default.FactorInformationFullAccessRight + ":" + false;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorOriginCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.FactorUpdateModeCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.IndividualCategoryCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.PeriodCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.PeriodTypeCacheKey;
                context.RemoveCachedObject(cacheKey);
            }
        }

        /// <summary>
        /// Class that holds information about factors and factor trees.
        /// An instance of this class is used when caching factor
        /// and factor tree information.
        /// </summary>
        private class FactorInformation
        {
            /// <summary>
            /// Create a FactorInformation instance.
            /// </summary>
            /// <param name="context">The web service context.</param>
            /// <param name='dataReader'>An open data reader.</param>
            public FactorInformation(WebServiceContext context, DataReader dataReader)
            {
                WebFactorTreeNode childTreeNode, factorTreeNode, parentTreeNode;

                // Get all factors and factor tree nodes.
                FactorTreeNodes = new WebFactorTreeNodeList();
                Factors = new List<WebFactor>();
                while (dataReader.Read())
                {
                    factorTreeNode = new WebFactorTreeNode();
                    factorTreeNode.LoadData(dataReader);

                    FactorTreeNodes.Merge(factorTreeNode);
                    Factors.Add(factorTreeNode.Factor);
                }

                // Get next result set.
                if (!dataReader.NextResultSet())
                {
                    throw new ApplicationException("No information about factors relations when getting factor tree");
                }

                // Get factor relations and build factor trees.
                while (dataReader.Read())
                {
                    try
                    {
                        parentTreeNode = FactorTreeNodes.Get(dataReader.GetInt32(FactorTreeData.PARENT_FACTOR_ID));
                        if (parentTreeNode != null)
                        {
                            childTreeNode = FactorTreeNodes.Get(dataReader.GetInt32(FactorTreeData.CHILD_FACTOR_ID));
                            if (childTreeNode != null)
                            {
                                parentTreeNode.AddChild(childTreeNode);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        // Suppress any errors of type ArgumentException
                    }
                }

                // Extract all factor tree nodes that
                // are not child tree nodes.
                FactorTrees = new List<WebFactorTreeNode>();
                foreach (WebFactorTreeNode factorTree in FactorTreeNodes)
                {
                    if (!factorTree.IsChild())
                    {
                        FactorTrees.Add(factorTree);
                    }
                }
            }

            /// <summary>
            /// All factors.
            /// </summary>
            public List<WebFactor> Factors { get; private set; }

            /// <summary>
            /// All factor tree nodes.
            /// </summary>
            public WebFactorTreeNodeList FactorTreeNodes { get; private set; }

            /// <summary>
            /// All factor trees.
            /// </summary>
            public List<WebFactorTreeNode> FactorTrees { get; private set; }
        }
    }
}
