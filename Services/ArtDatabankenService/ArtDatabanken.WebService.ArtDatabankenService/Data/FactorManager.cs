using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of factor information.
    /// </summary>
    public class FactorManager
    {
        /// <summary>
        /// Inserts a list of factor ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorIds">Id for factors to insert.</param>
        /// <param name="factorUsage">How user selected factors should be used.</param>
        public static void AddUserSelectedFactors(WebServiceContext context,
                                                  List<Int32> factorIds,
                                                  UserSelectedFactorUsage factorUsage)
        {
            DataTable factorTable;

            if (factorIds.IsNotEmpty())
            {
                // Delete all factor ids that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedFactors(context);

                // Insert the new list of factors.
                factorTable = GetUserSelectedFactorsTable(context,
                                                          factorIds,
                                                          factorUsage);
                DataServer.AddUserSelectedFactors(context, factorTable);
            }
        }

        /// <summary>
        /// Delete all factors that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedFactors(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedFactors(context);
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

            // Get cached information.
            cacheKey = "AllFactorDataTypes";
            factorDataTypes = (List<WebFactorDataType>)context.GetCachedObject(cacheKey);

            if (factorDataTypes.IsNull())
            {
                // Get information from database.
                factorDataTypes = new List<WebFactorDataType>();
                using (DataReader dataReader = DataServer.GetFactorFields(context))
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
                            dataReader.ColumnNamePrefix = "FactorDataType";
                            factorDataType = new WebFactorDataType(dataReader);
                            factorDataTypes.Add(factorDataType);
                            dataReader.ColumnNamePrefix = null;
                        }

                        // Add field.
                        factorDataType.Fields.Add(new WebFactorField(dataReader));
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, factorDataTypes, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
            }
            return factorDataTypes;
        }

        /// <summary>
        /// Get all factor field enums.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor field enums.</returns>
        public static List<WebFactorFieldEnum> GetFactorFieldEnums(WebServiceContext context)
        {
            Int32 factorFieldEnumId;
            List<WebFactorFieldEnum> factorFieldEnums;
            String cacheKey;
            WebFactorFieldEnum factorFieldEnum;

            // Get cached information.
            cacheKey = "AllFactorFieldEnums";
            factorFieldEnums = (List<WebFactorFieldEnum>)context.GetCachedObject(cacheKey);

            if (factorFieldEnums.IsNull())
            {
                // Get information from database.
                factorFieldEnums = new List<WebFactorFieldEnum>();
                using (DataReader dataReader = DataServer.GetFactorFieldEnums(context))
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
                            dataReader.ColumnNamePrefix = "FactorFieldEnum";
                            factorFieldEnum = new WebFactorFieldEnum(dataReader);
                            factorFieldEnums.Add(factorFieldEnum);
                            dataReader.ColumnNamePrefix = null;
                        }

                        // Add enum value.
                        factorFieldEnum.Values.Add(new WebFactorFieldEnumValue(dataReader));
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, factorFieldEnums, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
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

            // Get cached information.
            cacheKey = "AllFactorFieldTypes";
            factorFieldTypes = (List<WebFactorFieldType>)context.GetCachedObject(cacheKey);

            if (factorFieldTypes.IsNull())
            {
                // Get information from database.
                factorFieldTypes = new List<WebFactorFieldType>();
                using (DataReader dataReader = DataServer.GetFactorFieldTypes(context))
                {
                    while (dataReader.Read())
                    {
                        factorFieldTypes.Add(new WebFactorFieldType(dataReader));
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, factorFieldTypes, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return factorFieldTypes;
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>FactorOrigins.</returns>
        public static List<WebFactorOrigin> GetFactorOrigins(WebServiceContext context)
        {
            List<WebFactorOrigin> factorOrigins;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllFactorOrigins";
            factorOrigins = (List<WebFactorOrigin>)context.GetCachedObject(cacheKey);

            if (factorOrigins.IsNull())
            {
                // Get information from database.
                factorOrigins = new List<WebFactorOrigin>();
                using (DataReader dataReader = DataServer.GetFactorOrigins(context))
                {
                    while (dataReader.Read())
                    {
                        factorOrigins.Add(new WebFactorOrigin(dataReader));
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, factorOrigins, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return factorOrigins;
        }

        /// <summary>
        /// Get information about factor trees and factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor information.</returns>
        private static FactorInformation GetFactorInformation(WebServiceContext context)
        {
            FactorInformation factorInformation;
            String cacheKey;

            // Get cached information.
            cacheKey = "FactorInformation";
            factorInformation = (FactorInformation)context.GetCachedObject(cacheKey);

            if (factorInformation.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = DataServer.GetFactorTrees(context))
                {
                    factorInformation = new FactorInformation(dataReader);

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, factorInformation, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return factorInformation;
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factors.</returns>
        public static List<WebFactor> GetFactors(WebServiceContext context)
        {
            return GetFactorInformation(context).Factors;
        }

        /// <summary>
        /// Get information about all factor trees.
        /// Only factors that are in the factor cache are returned.
        /// This is done in order to avoid problem where cached
        /// factor data differ from factor data in database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factors">Factor list where factors are added.</param>
        /// <param name="factorTreeNodes">All factor tree nodes.</param>
        /// <param name="dataReader">An open DataReader with information about factors.</param>
        private static void GetFactors(WebServiceContext context,
                                       List<WebFactor> factors,
                                       WebFactorTreeNodeList factorTreeNodes,
                                       DataReader dataReader)
        {
            WebFactor factor;

            while (dataReader.Read())
            {
                factor = new WebFactor(dataReader);
                if (factorTreeNodes.Contains(factor.Id))
                {
                    factors.Add(factorTreeNodes.Get(factor.Id).Factor);
                }
            }
        }

        /// <summary>
        /// Get information about factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorIds">Ids for factors to get information about.</param>
        /// <returns>Factors information.</returns>
        public static List<WebFactor> GetFactorsById(WebServiceContext context,
                                                     List<Int32> factorIds)
        {
            List<WebFactor> factors;
            WebFactorTreeNodeList factorTreeNodes;

            // Check arguments.
            factorIds.CheckNotEmpty("factorIds");

            // Get factors.
            factors = new List<WebFactor>();
            factorTreeNodes = GetFactorInformation(context).FactorTreeNodes;
            foreach (Int32 faktorId in factorIds)
            {
                factors.Add(factorTreeNodes.Get(faktorId).Factor);
            }
            return factors;
        }

        /// <summary>
        /// Get factors that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Factors.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        public static List<WebFactor> GetFactorsBySearchCriteria(WebServiceContext context,
                                                                 WebFactorSearchCriteria searchCriteria)
        {
            List<WebFactor> factors = null;
            Int32 factorId = -1;
            List<Int32> factorIds = null;
            WebFactorTreeNodeList factorTreeNodes;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            if (searchCriteria.NameSearchString.IsNotEmpty() &&
                searchCriteria.IsIdInNameSearchString)
            {
                try
                {
                    factorId = Convert.ToInt32(searchCriteria.NameSearchString);
                }
                catch
                {
                    //Nothing
                }

                if (factorId != -1)
                {
                    factorIds = new List<Int32>();
                    factorIds.Add(factorId);
                    searchCriteria.RestrictSearchToFactorIds = factorIds;
                    searchCriteria.NameSearchString = "";
                }
            }

            try
            {
                // Insert search information into database
                if (searchCriteria.RestrictSearchToFactorIds.IsNotEmpty())
                {
                    AddUserSelectedFactors(context, searchCriteria.RestrictSearchToFactorIds, UserSelectedFactorUsage.Input);
                }
                else
                {
                    // Delete all factors that belong to this request from the "temporary" tables.
                    // This is done to avoid problem with restarts of the webservice.
                    DeleteUserSelectedFactors(context);
                }

                // Get information about factors from database.
                factors = new List<WebFactor>();
                factorTreeNodes = GetFactorInformation(context).FactorTreeNodes;
                if (searchCriteria.NameSearchMethod == SearchStringComparisonMethod.Iterative)
                {
                    String nameSearchMethodString;

                    //Step 1
                    nameSearchMethodString = SearchStringComparisonMethod.Exact.ToString();

                    using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                        context,
                        searchCriteria.NameSearchString,
                        nameSearchMethodString,
                        searchCriteria.RestrictSearchToFactorIds.IsNotEmpty(),
                        searchCriteria.RestrictSearchToScope.ToString(),
                        searchCriteria.RestrictReturnToScope.ToString()))
                    {
                        GetFactors(context, factors, factorTreeNodes, dataReader);
                    }

                    if (factors.IsEmpty())
                    {
                        //Step 2
                        nameSearchMethodString = SearchStringComparisonMethod.Like.ToString();

                        using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                            context,
                            searchCriteria.NameSearchString + "%",
                            nameSearchMethodString,
                            searchCriteria.RestrictSearchToFactorIds.IsNotEmpty(),
                            searchCriteria.RestrictSearchToScope.ToString(),
                            searchCriteria.RestrictReturnToScope.ToString()))
                        {
                            GetFactors(context, factors, factorTreeNodes, dataReader);
                        }
                    }

                    if (factors.IsEmpty())
                    {
                        //Step 3
                        nameSearchMethodString = SearchStringComparisonMethod.Like.ToString();

                        using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                            context,
                            "%" + searchCriteria.NameSearchString + "%",
                            nameSearchMethodString,
                            searchCriteria.RestrictSearchToFactorIds.IsNotEmpty(),
                            searchCriteria.RestrictSearchToScope.ToString(),
                            searchCriteria.RestrictReturnToScope.ToString()))
                        {
                            GetFactors(context, factors, factorTreeNodes, dataReader);
                        }
                    }

                }
                else
                {
                    using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                        context,
                        searchCriteria.NameSearchString,
                        searchCriteria.NameSearchMethod.ToString(),
                        searchCriteria.RestrictSearchToFactorIds.IsNotEmpty(),
                        searchCriteria.RestrictSearchToScope.ToString(),
                        searchCriteria.RestrictReturnToScope.ToString()))
                    {
                        GetFactors(context, factors, factorTreeNodes, dataReader);
                    }
                }
            }
            finally
            {
                // Clean up.
                DeleteUserSelectedFactors(context);
            }

            return factors;
        }

        /// <summary>
        /// Get information about all factor trees.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Factor tree information.</returns>
        public static List<WebFactorTreeNode> GetFactorTrees(WebServiceContext context)
        {
            return GetFactorInformation(context).FactorTrees;
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The factor tree search criteria.</param>
        /// <returns>Factor tree information.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebServiceContext context,
                                                                             WebFactorTreeSearchCriteria searchCriteria)
        {
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeNodeList factorTreeNodes;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            if (searchCriteria.RestrictSearchToFactorIds.IsEmpty())
            {
                // Get all factor trees.
                factorTrees = GetFactorTrees(context);
            }
            else
            {
                // Get all factor trees that are requested.
                factorTrees = new List<WebFactorTreeNode>();
                factorTreeNodes = GetFactorInformation(context).FactorTreeNodes;
                foreach (Int32 factorId in searchCriteria.RestrictSearchToFactorIds)
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

            // Get cached information.
            cacheKey = "AllFactorUpdateModes";
            factorUpdateModes = (List<WebFactorUpdateMode>)context.GetCachedObject(cacheKey);

            if (factorUpdateModes.IsNull())
            {
                // Get information from database.
                factorUpdateModes = new List<WebFactorUpdateMode>();
                using (DataReader dataReader = DataServer.GetFactorUpdateModes(context))
                {
                    while (dataReader.Read())
                    {
                        factorUpdateModes.Add(new WebFactorUpdateMode(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, factorUpdateModes, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return factorUpdateModes;
        }

        /// <summary>
        /// Get a DataTable object with information about
        /// user selected factors.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorIds">Id for factors to insert.</param>
        /// <param name="factorUsage">How user selected factors should be used.</param>
        /// <returns>DataTable with information about user selected factors.</returns>
        public static DataTable GetUserSelectedFactorsTable(WebServiceContext context,
                                                            List<Int32> factorIds,
                                                            UserSelectedFactorUsage factorUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable factorTable;

            // Insert the new list of factors.
            factorTable = new DataTable(UserSelectedFactorData.TABLE_NAME);
            column = new DataColumn(UserSelectedFactorData.REQUEST_ID, typeof(Int32));
            factorTable.Columns.Add(column);
            column = new DataColumn(UserSelectedFactorData.FACTOR_ID, typeof(Int32));
            factorTable.Columns.Add(column);
            column = new DataColumn(UserSelectedFactorData.FACTOR_USAGE, typeof(String));
            factorTable.Columns.Add(column);
            foreach (Int32 factorId in factorIds)
            {
                row = factorTable.NewRow();
                row[0] = context.RequestId;
                row[1] = factorId;
                row[2] = factorUsage.ToString();
                factorTable.Rows.Add(row);
            }

            return factorTable;
        }

        /// <summary>
        /// Class that holds information about factors and factor trees.
        /// An instance of this class is used when caching factor
        /// and factor tree information.
        /// </summary>
        private class FactorInformation
        {
            private List<WebFactor> _factors;
            private List<WebFactorTreeNode> _factorTrees;
            private WebFactorTreeNodeList _factorTreeNodes;

            /// <summary>
            /// Create a FactorInformation instance.
            /// </summary>
            /// <param name='dataReader'>An open data reader.</param>
            public FactorInformation(DataReader dataReader)
            {
                WebFactorTreeNode childTreeNode, factorTreeNode, parentTreeNode;

                // Get all factors and factor tree nodes.
                _factorTreeNodes = new WebFactorTreeNodeList();
                _factors = new List<WebFactor>();
                while (dataReader.Read())
                {
                    factorTreeNode = new WebFactorTreeNode(dataReader);
                    _factorTreeNodes.Merge(factorTreeNode);
                    _factors.Add(factorTreeNode.Factor);
                }

                // Get next result set.
                if (!dataReader.NextResultSet())
                {
                    throw new ApplicationException("No information about factors relations when getting factor tree");
                }

                // Get factor relations and build factor trees. 
                while (dataReader.Read())
                {
                    parentTreeNode = _factorTreeNodes.Get(dataReader.GetInt32(FactorTreeData.PARENT_FACTOR_ID));
                    childTreeNode = _factorTreeNodes.Get(dataReader.GetInt32(FactorTreeData.CHILD_FACTOR_ID));
                    parentTreeNode.AddChild(childTreeNode);
                }

                // Extract all factor tree nodes that
                // are not child tree nodes.
                _factorTrees = new List<WebFactorTreeNode>();
                foreach (WebFactorTreeNode factorTree in _factorTreeNodes)
                {
                    if (!factorTree.IsChild)
                    {
                        _factorTrees.Add(factorTree);
                    }
                }
            }

            /// <summary>
            /// Get all factors.
            /// </summary>
            public List<WebFactor> Factors
            {
                get { return _factors; }
            }

            /// <summary>
            /// Get all factor tree nodes.
            /// </summary>
            public WebFactorTreeNodeList FactorTreeNodes
            {
                get { return _factorTreeNodes; }
            }

            /// <summary>
            /// Get all factor trees.
            /// </summary>
            public List<WebFactorTreeNode> FactorTrees
            {
                get { return _factorTrees; }
            }
        }
    }
}
