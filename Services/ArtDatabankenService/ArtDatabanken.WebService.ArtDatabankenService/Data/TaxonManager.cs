using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of taxa information.
    /// </summary>
    public class TaxonManager : DataQueryManager, ITaxonManager
    {
        /// <summary>
        /// Inserts a list of taxon ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds">Id for taxa to insert.</param>
        /// <param name="taxonUsage">How user selected taxa should be used.</param>
        public static void AddUserSelectedTaxa(WebServiceContext context,
                                               List<Int32> taxonIds,
                                               UserSelectedTaxonUsage taxonUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable taxaTable;

            if (taxonIds.IsNotEmpty())
            {
                // Delete all taxa that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedTaxa(context);

                // Insert the new list of taxa.
                taxaTable = new DataTable(UserSelectedTaxaData.TABLE_NAME);
                column = new DataColumn(UserSelectedTaxaData.REQUEST_ID, typeof(Int32));
                taxaTable.Columns.Add(column);
                column = new DataColumn(UserSelectedTaxaData.TAXON_ID, typeof(Int32));
                taxaTable.Columns.Add(column);
                column = new DataColumn(UserSelectedTaxaData.TAXON_USAGE, typeof(String));
                taxaTable.Columns.Add(column);
                foreach (Int32 taxonId in taxonIds)
                {
                    row = taxaTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = taxonId;
                    row[2] = taxonUsage.ToString();
                    taxaTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedTaxa(context, taxaTable);
            }
        }

        /// <summary>
        /// Inserts a list of taxon type ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTypeIds">Id for taxon types to insert.</param>
        /// <param name="taxonTypeUsage">How user selected taxon types should be used.</param>
        public static void AddUserSelectedTaxonTypes(WebServiceContext context,
                                                     List<Int32> taxonTypeIds,
                                                     UserSelectedTaxonTypeUsage taxonTypeUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable taxonTypesTable;

            if (taxonTypeIds.IsNotEmpty())
            {
                // Delete all taxon types that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedTaxonTypes(context);

                // Insert the new list of taxon types.
                taxonTypesTable = new DataTable(UserSelectedTaxonTypesData.TABLE_NAME);
                column = new DataColumn(UserSelectedTaxonTypesData.REQUEST_ID, typeof(Int32));
                taxonTypesTable.Columns.Add(column);
                column = new DataColumn(UserSelectedTaxonTypesData.TAXON_TYPE_ID, typeof(Int32));
                taxonTypesTable.Columns.Add(column);
                column = new DataColumn(UserSelectedTaxonTypesData.TAXON_TYPE_USAGE, typeof(String));
                taxonTypesTable.Columns.Add(column);
                foreach (Int32 taxonTypeId in taxonTypeIds)
                {
                    row = taxonTypesTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = taxonTypeId;
                    row[2] = taxonTypeUsage.ToString();
                    taxonTypesTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedTaxonTypes(context, taxonTypesTable);
            }
        }

        /// <summary>
        /// Delete all taxa that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedTaxa(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedTaxa(context);
        }

        /// <summary>
        /// Delete all taxon types that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedTaxonTypes(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedTaxonTypes(context);
        }

        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <returns>Taxa information.</returns>
        public virtual List<WebTaxon> GetTaxaByIds(WebServiceContext context,
                                                   List<Int32> taxonIds)
        {
            List<WebTaxon> taxa;
            List<WebService.Data.WebTaxon> webTaxa;
            WebService.Data.WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            webTaxa = WebServiceProxy.TaxonService.GetTaxaByIds(clientInformation,
                                                                taxonIds);
            taxa = new List<WebTaxon>();
            if (webTaxa.IsNotEmpty())
            {
                foreach (WebService.Data.WebTaxon webTaxon in webTaxa)
                {
                    taxa.Add(GetTaxon(webTaxon));
                }
            }
            return taxa;
        }

        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxaById(WebServiceContext context,
                                                 List<Int32> taxonIds,
                                                 TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa;

            // Check arguments.
            taxonIds.CheckNotEmpty("taxonIds");

            // Get data from database.
            try
            {
                AddUserSelectedTaxa(context, taxonIds, UserSelectedTaxonUsage.Output);
                taxa = new List<WebTaxon>();
                using (DataReader dataReader = DataServer.GetTaxa(context, taxonInformationType.ToString()))
                {
                    while (dataReader.Read())
                    {
                        taxa.Add(new WebTaxon(dataReader));
                    }
                }
            }
            finally
            {
                DeleteUserSelectedTaxa(context);
            }

            if (taxa.Count != taxonIds.Count)
            {
                // Probably invalid taxon ids.
                throw new ArgumentException("Invalid taxon ids!");
            }
            return taxa;
        }

        /// <summary>
        /// Get taxa information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hasOrganismGroupId">Indicates if organism group id is set.</param>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="hasEndangeredListId">Indicates if endangered list id is set.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="hasRedlistCategoryId">Indicates if redlist category id is set.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxaByOrganismOrRedlist(WebServiceContext context,
                                                                Boolean hasOrganismGroupId,
                                                                Int32 organismGroupId,
                                                                Boolean hasEndangeredListId,
                                                                Int32 endangeredListId,
                                                                Boolean hasRedlistCategoryId,
                                                                Int32 redlistCategoryId,
                                                                TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa;

            // Get data from database.
            taxa = new List<WebTaxon>();
            using (DataReader dataReader = DataServer.GetTaxaByOrganismOrRedlist(context,
                                                                                 hasOrganismGroupId,
                                                                                 organismGroupId,
                                                                                 hasEndangeredListId,
                                                                                 endangeredListId,
                                                                                 hasRedlistCategoryId,
                                                                                 redlistCategoryId,
                                                                                 taxonInformationType.ToString()))
            {
                while (dataReader.Read())
                {
                    taxa.Add(new WebTaxon(dataReader));
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get information about taxa that matches the query.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dataQuery">Data query.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if query is null.</exception>
        public static List<WebTaxon> GetTaxaByQuery(WebServiceContext context,
                                                    WebDataQuery dataQuery,
                                                    TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa;

            // Check data.
            dataQuery.CheckNotNull("dataQuery");
            dataQuery.CheckData();

            // Get data from database.
            taxa = new List<WebTaxon>();
            using (DataReader dataReader = DataServer.GetTaxaByQuery(context,
                                                                     GetDataQuery(context, dataQuery),                                                                                 
                                                                     taxonInformationType.ToString()))
            {
                while (dataReader.Read())
                {
                    taxa.Add(new WebTaxon(dataReader));
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if taxonSearchCriteria is null.</exception>
        public static List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context,
                                                             WebTaxonSearchCriteria searchCriteria)
        {
            List<WebTaxon> taxa;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            try
            {
                // Insert search information into database
                if (searchCriteria.RestrictReturnToTaxonTypeIds.IsNotEmpty())
                {
                    AddUserSelectedTaxonTypes(context,
                                              searchCriteria.RestrictReturnToTaxonTypeIds,
                                              UserSelectedTaxonTypeUsage.Output);
                }
                if (searchCriteria.RestrictSearchToTaxonIds.IsNotEmpty())
                {
                    AddUserSelectedTaxa(context, searchCriteria.RestrictSearchToTaxonIds, UserSelectedTaxonUsage.Input);
                }
                else
                {
                    // Delete all taxa that belong to this request from the "temporary" tables.
                    // This is done to avoid problem with restarts of the webservice.
                    DeleteUserSelectedTaxa(context);
                }
                if (searchCriteria.RestrictSearchToTaxonTypeIds.IsNotEmpty())
                {
                    AddUserSelectedTaxonTypes(context,
                                              searchCriteria.RestrictSearchToTaxonTypeIds,
                                              UserSelectedTaxonTypeUsage.Input);
                }

                // Get taxon information from database.
                taxa = new List<WebTaxon>();
                using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(context,
                                                                                  searchCriteria.TaxonNameSearchString,
                                                                                  searchCriteria.TaxonInformationType.ToString(),
                                                                                  searchCriteria.RestrictSearchToTaxonIds.IsNotEmpty(),
                                                                                  searchCriteria.RestrictSearchToTaxonTypeIds.IsNotEmpty(),
                                                                                  searchCriteria.RestrictSearchToSwedishSpecies,
                                                                                  searchCriteria.RestrictReturnToScope.ToString(),
                                                                                  searchCriteria.RestrictReturnToSwedishSpecies,
                                                                                  searchCriteria.RestrictReturnToTaxonTypeIds.IsNotEmpty()))
                {
                    while (dataReader.Read())
                    {
                        taxa.Add(new WebTaxon(dataReader));
                    }
                }
            }
            finally
            {
                // Clean up.
                DeleteUserSelectedTaxa(context);
                if (searchCriteria.RestrictReturnToTaxonTypeIds.IsNotEmpty() ||
                    searchCriteria.RestrictSearchToTaxonTypeIds.IsNotEmpty())
                {
                    DeleteUserSelectedTaxonTypes(context);
                }
            }

            return taxa;
        }

        /// <summary>
        /// Convert a ArtDatabanken.WebService.Data.WebTaxon instance into
        /// a ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxon instance.
        /// </summary>
        /// <param name="webTaxon">A ArtDatabanken.WebService.Data.WebTaxon instance.</param>
        /// <returns>A ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxon instance.</returns>
        private WebTaxon GetTaxon(WebService.Data.WebTaxon webTaxon)
        {
            WebTaxon taxon;

            taxon = new WebTaxon();
            taxon.Author = webTaxon.Author;
            taxon.CommonName = webTaxon.CommonName;
            taxon.Id = webTaxon.Id;
            taxon.ScientificName = webTaxon.ScientificName;
            taxon.SortOrder = webTaxon.SortOrder;
            taxon.TaxonInformationType = TaxonInformationType.Basic;
            taxon.TaxonTypeId = webTaxon.CategoryId;
            return taxon;
        }

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <returns>Taxon information.</returns>
        public virtual WebTaxon GetTaxon(WebServiceContext context,
                                         Int32 taxonId)
        {
            WebService.Data.WebTaxon webTaxon;
            WebService.Data.WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            webTaxon = WebServiceProxy.TaxonService.GetTaxonById(clientInformation,
                                                                 taxonId);
            return GetTaxon(webTaxon);
        }

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxon information.</returns>
        public static WebTaxon GetTaxon(WebServiceContext context,
                                        Int32 taxonId,
                                        TaxonInformationType taxonInformationType)
        {
            String cacheKey;
            WebTaxon taxon;

            if (taxonId == WebTaxon.UNKNOWN_TAXON_ID)
            {
                // Get cached information.
                cacheKey = "UnknownTaxon" + taxonInformationType;
                taxon = (WebTaxon)context.GetCachedObject(cacheKey);
                if (taxon.IsNotNull())
                {
                    return taxon;
                }
            }

            using (DataReader dataReader = DataServer.GetTaxon(context, taxonId, taxonInformationType.ToString()))
            {
                if (dataReader.Read())
                {
                    taxon = new WebTaxon(dataReader);
                }
                else
                {
                    // Probably invalid taxon id.
                    throw new ArgumentException("Invalid taxon id " + taxonId + "!");
                }
            }

            if (taxonId == WebTaxon.UNKNOWN_TAXON_ID)
            {
                // Add information to cache.
                cacheKey = "UnknownTaxon" + taxonInformationType;
                context.AddCachedObject(cacheKey, taxon, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.Normal);
            }

            return taxon;
        }

        /// <summary>
        /// Get taxa information from DataReader.
        /// This metod does not close the DataReader.
        /// That must be done in the calling method.
        /// </summary>
        /// <param name="dataReader">An open DataReader.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxa(DataReader dataReader)
        {
            List<WebTaxon> taxa;

            taxa = new List<WebTaxon>();
            while (dataReader.Read())
            {
                taxa.Add(new WebTaxon(dataReader));
            }
            return taxa;
        }

        /// <summary>
        /// Get all taxa utelizing a sertain host taxon and any of its child taxa.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hostTaxonId">Id of host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static List<WebTaxon> GetTaxaByHostTaxonId(WebServiceContext context,
                             Int32 hostTaxonId,
                             TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa = new List<WebTaxon>();

            DeleteUserSelectedTaxa(context);

            // Get data from database.
            using (DataReader dataReader = DataServer.GetTaxaByHostTaxonId(context, hostTaxonId, taxonInformationType.ToString()))
            {
                while (dataReader.Read())
                {
                    taxa.Add(new WebTaxon(dataReader));
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get taxon names for specified taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNames(WebServiceContext context,
                                                       Int32 taxonId)
        {
            List<WebTaxonName> taxonNames;

            // Get information from database.
            taxonNames = new List<WebTaxonName>();
            using (DataReader dataReader = DataServer.GetTaxonNames(context, taxonId))
            {
                while (dataReader.Read())
                {
                    taxonNames.Add(new WebTaxonName(dataReader));
                }
            }
            return taxonNames;
        }

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Taxon name search criteria.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebServiceContext context,
                                                                       WebTaxonNameSearchCriteria searchCriteria)
        {
            Int32 movedTaxonNameCount, taxonNameIndex;
            List<WebTaxonName> taxonNames;
            String nameSearchString;
            WebTaxonName taxonName;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get information from database.
            taxonNames = new List<WebTaxonName>();
            using (DataReader dataReader = DataServer.GetTaxonNamesBySearchCriteria(context, searchCriteria.NameSearchString, searchCriteria.NameSearchMethod.ToString()))
            {
                while (dataReader.Read())
                {
                    taxonNames.Add(new WebTaxonName(dataReader));
                }
            }

            // Move taxon names that begins with taxon name search string
            // to the top of returned taxon names.
            if ((searchCriteria.NameSearchMethod == SearchStringComparisonMethod.Contains) &&
                searchCriteria.NameSearchString.IsNotEmpty() &&
                taxonNames.IsNotEmpty())
            {
                nameSearchString = searchCriteria.NameSearchString.ToLower();
                for (taxonNameIndex = taxonNames.Count - 1; taxonNameIndex >= 0; taxonNameIndex--)
                {
                    if (taxonNames[taxonNameIndex].Name.ToLower().StartsWith(nameSearchString))
                    {
                        movedTaxonNameCount = 0;
                        while (taxonNames[taxonNameIndex].Name.ToLower().StartsWith(nameSearchString))
                        {
                            taxonName = taxonNames[taxonNameIndex];
                            taxonNames.RemoveAt(taxonNameIndex);
                            taxonNames.Insert(0, taxonName);
                            movedTaxonNameCount++;
                            if (movedTaxonNameCount > taxonNameIndex)
                            {
                                break;
                            }
                        }
                        taxonNameIndex = -1;
                    }
                }
            }

            return taxonNames;
        }

        /// <summary>
        /// Get all taxon name types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon name types.</returns>
        public static List<WebTaxonNameType> GetTaxonNameTypes(WebServiceContext context)
        {
            List<WebTaxonNameType> taxonNameTypes;
            String cacheKey;
            TaxonNameCategoryList taxonNameCategories;
            WebTaxonNameType taxonNameType;

            // Get cached information.
            cacheKey = "AllTaxonNameTypes";
            taxonNameTypes = (List<WebTaxonNameType>)context.GetCachedObject(cacheKey);

            if (taxonNameTypes.IsNull())
            {
                // Get information from TaxonService.
                taxonNameCategories = CoreData.TaxonManager.GetTaxonNameCategories(ManagerBase.GetUserContext(context));
                taxonNameTypes = new List<WebTaxonNameType>();
                foreach (ITaxonNameCategory taxonNameCategory in taxonNameCategories)
                {
                    taxonNameType = new WebTaxonNameType();
                    taxonNameType.Id = taxonNameCategory.Id;
                    taxonNameType.Name = taxonNameCategory.Name;
                    taxonNameType.SortOrder = taxonNameCategory.SortOrder;
                    taxonNameTypes.Add(taxonNameType);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameTypes,
                                        DateTime.Now + new TimeSpan(12, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }
            return taxonNameTypes;
        }

        /// <summary>
        /// Get all taxon name use types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon name use types.</returns>
        public static List<WebTaxonNameUseType> GetTaxonNameUseTypes(WebServiceContext context)
        {
            List<WebTaxonNameUseType> taxonNameUseTypes;
            String cacheKey;
            TaxonNameStatusList taxonNameStatusList;
            WebTaxonNameUseType taxonNameUseType;

            // Get cached information.
            cacheKey = "AllTaxonNameUseTypes";
            taxonNameUseTypes = (List<WebTaxonNameUseType>)context.GetCachedObject(cacheKey);

            if (taxonNameUseTypes.IsNull())
            {
                // Get information from TaxonService.
                taxonNameUseTypes = new List<WebTaxonNameUseType>();
                taxonNameStatusList = CoreData.TaxonManager.GetTaxonNameStatuses(ManagerBase.GetUserContext(context));
                foreach (ITaxonNameStatus taxonNameStatus in taxonNameStatusList)
                {
                    taxonNameUseType = new WebTaxonNameUseType();
                    taxonNameUseType.Id = taxonNameStatus.Id;
                    taxonNameUseType.Name = taxonNameStatus.Name;
                    taxonNameUseTypes.Add(taxonNameUseType);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameUseTypes,
                                        DateTime.Now + new TimeSpan(12, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }
            return taxonNameUseTypes;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        public static List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebServiceContext context,
                                                                           WebTaxonTreeSearchCriteria searchCriteria)
        {
            Int32 childTaxonTreeIndex, taxonTreeIndex;
            List<WebTaxonTreeNode> children, taxonTrees = null;
            WebTaxonTreeNode parentTreeNode, childTreeNode;
            WebTaxonTreeNodeList taxonTreeNodes;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            try
            {
                // Insert search information into database
                if (searchCriteria.RestrictSearchToTaxonIds.IsNotEmpty())
                {
                    AddUserSelectedTaxa(context, searchCriteria.RestrictSearchToTaxonIds, UserSelectedTaxonUsage.Input);
                }
                else
                {
                    // Delete all taxa that belong to this request from the "temporary" tables.
                    // This is done to avoid problem with restarts of the webservice.
                    DeleteUserSelectedTaxa(context);
                }

                // Get taxon tree information from database.
                taxonTreeNodes = new WebTaxonTreeNodeList();
                using (DataReader dataReader = DataServer.GetTaxonTreesBySearchCriteria(context,
                                                                                        searchCriteria.TaxonInformationType.ToString(),
                                                                                        searchCriteria.RestrictSearchToTaxonIds.IsNotEmpty()))
                {
                    // Get all taxon tree nodes.
                    while (dataReader.Read())
                    {
                        taxonTreeNodes.Add(new WebTaxonTreeNode(dataReader));
                    }

                    if (taxonTreeNodes.IsNotEmpty())
                    {
                        // Get next result set.
                        if (!dataReader.NextResultSet())
                        {
                            throw new ApplicationException("No information about taxa relations when getting taxon tree");
                        }

                        // Get taxa relations and build taxon tree. 
                        while (dataReader.Read())
                        {
                            parentTreeNode = taxonTreeNodes.Get(dataReader.GetInt32(TaxonTreeData.PARENT_TAXON_ID));
                            childTreeNode = taxonTreeNodes.Get(dataReader.GetInt32(TaxonTreeData.CHILD_TAXON_ID));
                            parentTreeNode.AddChild(childTreeNode);
                        }

                        // Extract all taxon tree nodes that
                        // are not child tree nodes.
                        taxonTrees = new List<WebTaxonTreeNode>();
                        foreach (WebTaxonTreeNode taxonTreeNode in taxonTreeNodes)
                        {
                            if (!taxonTreeNode.IsChild)
                            {
                                taxonTrees.Add(taxonTreeNode);
                            }
                        }

                        // Remove levels in taxon tree that should
                        // not be returned.
                        if (searchCriteria.RestrictSearchToTaxonTypeIds.IsNotEmpty())
                        {
                            // Remove not requested levels in the tree.
                            foreach (WebTaxonTreeNode taxonTreeNode in taxonTrees)
                            {
                                taxonTreeNode.RestrictTaxonTypes(searchCriteria.RestrictSearchToTaxonTypeIds);
                            }

                            // Remove not requested levels from top of tree.
                            for (taxonTreeIndex = 0; taxonTreeIndex < taxonTrees.Count; taxonTreeIndex++)
                            {
                                if (!searchCriteria.RestrictSearchToTaxonTypeIds.Contains(taxonTrees[taxonTreeIndex].Taxon.TaxonTypeId))
                                {
                                    // Compress one taxon type level in tree.
                                    // Get childes children.
                                    children = taxonTrees[taxonTreeIndex].Children;

                                    // Remove child.
                                    taxonTrees.RemoveAt(taxonTreeIndex);

                                    // Add childes children.
                                    if (children.IsNotEmpty())
                                    {
                                        childTaxonTreeIndex = taxonTreeIndex;
                                        foreach (WebTaxonTreeNode child in children)
                                        {
                                            child.IsChild = false;
                                            taxonTrees.Insert(childTaxonTreeIndex, child);
                                            childTaxonTreeIndex++;
                                        }
                                    }

                                    taxonTreeIndex--;
                                }
                            }
                        }
                    }
                    // else No trees to create.
                }
            }
            finally
            {
                // Clean up.
                DeleteUserSelectedTaxa(context);
            }

            return taxonTrees;
        }

        /// <summary>
        /// Get all taxon types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon types.</returns>
        public static List<WebTaxonType> GetTaxonTypes(WebServiceContext context)
        {
            List<WebTaxonType> taxonTypes;
            String cacheKey;
            TaxonCategoryList taxonCategories;
            WebTaxonType taxonType;

            // Get cached information.
            cacheKey = "AllTaxonTypes";
            taxonTypes = (List<WebTaxonType>)context.GetCachedObject(cacheKey);

            if (taxonTypes.IsNull())
            {
                // Get information from TaxonService.
                taxonCategories = CoreData.TaxonManager.GetTaxonCategories(ManagerBase.GetUserContext(context));
                taxonTypes = new List<WebTaxonType>();
                foreach (ITaxonCategory taxonCategory in taxonCategories)
                {
                    taxonType = new WebTaxonType();
                    taxonType.Id = taxonCategory.Id;
                    taxonType.Name = taxonCategory.Name;
                    taxonType.SortOrder = taxonCategory.SortOrder;
                    taxonTypes.Add(taxonType);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonTypes,
                                        DateTime.Now + new TimeSpan(12, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }

            return taxonTypes;
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="taxonTree">Taxon tree.</param>
        /// <param name="taxa">Aggregated taxa.</param>
        private static void GetChildTaxa(WebTaxonTreeNode taxonTree,
                                         List<WebTaxon> taxa)
        {
            taxa.Add(taxonTree.Taxon);
            if (taxonTree.Children.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode childTaxonTree in taxonTree.Children)
                {
                    GetChildTaxa(childTaxonTree, taxa);
                }
            }
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parentTaxonGuids">GUIDs for parent taxa.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>All child taxa.</returns>
        public static List<WebTaxon> GetChildTaxa(WebServiceContext context,
                                                  List<String> parentTaxonGuids,
                                                  TaxonInformationType taxonInformationType)
        {
            List<Int32> parentTaxonIds;

            parentTaxonIds = new List<Int32>();
            foreach (String parentTaxonGUID in parentTaxonGuids)
            {
                // TODO: This assumption about taxon GUIDs may
                // change in the future.
                parentTaxonIds.Add(Int32.Parse(parentTaxonGUID));
            }

            return GetChildTaxa(context, parentTaxonIds, taxonInformationType);
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parentTaxonIds">Ids for parent taxa.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>All child taxa.</returns>
        public static List<WebTaxon> GetChildTaxa(WebServiceContext context,
                                                  List<Int32> parentTaxonIds,
                                                  TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> childTaxa;
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria taxonTreeSearchCriteria;

            taxonTreeSearchCriteria = new WebTaxonTreeSearchCriteria();
            taxonTreeSearchCriteria.RestrictSearchToTaxonIds = parentTaxonIds;
            taxonTreeSearchCriteria.TaxonInformationType = taxonInformationType;
            taxonTrees = GetTaxonTreesBySearchCriteria(context, taxonTreeSearchCriteria);
            childTaxa = new List<WebTaxon>();
            if (taxonTrees.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode taxonTree in taxonTrees)
                {
                    GetChildTaxa(taxonTree, childTaxa);
                }
            }

            return childTaxa;
        }

        /// <summary>
        /// Get all host taxa associated with a factor
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorId">Id of factor which host taxa are related to.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static List<WebTaxon> GetHostTaxa(WebServiceContext context,
                                     Int32 factorId,
                                     TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa = new List<WebTaxon>();

            DeleteUserSelectedTaxa(context);

            // Get data from database.
            using (DataReader dataReader = DataServer.GetHostTaxa(context, factorId, taxonInformationType.ToString()))
            {
                while (dataReader.Read())
                {
                    taxa.Add(new WebTaxon(dataReader));
                }
            }
           
            return taxa;
        }

        /// <summary>
        /// Get all host taxa associated with a sertain taxon.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static List<WebTaxon> GetHostTaxaByTaxonId(WebServiceContext context,
                             Int32 taxonId,
                             TaxonInformationType taxonInformationType)
        {
            List<WebTaxon> taxa = new List<WebTaxon>();

            DeleteUserSelectedTaxa(context);

            // Get data from database.
            using (DataReader dataReader = DataServer.GetHostTaxaByTaxonId(context, taxonId, taxonInformationType.ToString()))
            {
                while (dataReader.Read())
                {
                    taxa.Add(new WebTaxon(dataReader));
                }
            }

            return taxa;
        }

        /// <summary>
        /// Handle automatic update of taxon information.
        /// </summary>
        public static void UpdateTaxonInformation()
        {
            ITaxonTreeSearchCriteria taxonTreeSearchCriteria;
            TaxonTreeNodeList taxonTrees;
            WebServiceContext context;

            // Create contexts.
            context = new WebServiceContext(new WebClientToken(WebServiceData.WebServiceManager.Name,
                                                               ApplicationIdentifier.ArtDatabankenSOA.ToString(), WebServiceData.WebServiceManager.Key).Token);

            try
            {
                LogManager.Log(context, "Begin taxon information update", LogType.Information, null);

                // Create taxon tables.
                DataServer.CreateTaxonTables(context);

                // Get taxon and taxon tree information.
                taxonTreeSearchCriteria = new TaxonTreeSearchCriteria();
                taxonTreeSearchCriteria.IsValidRequired = true;
                taxonTrees = CoreData.TaxonManager.GetTaxonTrees(ManagerBase.GetUserContext(context),
                                                                 taxonTreeSearchCriteria);

                // Update table Taxon.
                UpdateTaxonInformation(context, taxonTrees);

                // Update table TaxonName.
                UpdateTaxonNameInformation(context, taxonTrees);

                // Update table TaxonTree.
                UpdateTaxonTreeInformation(context, taxonTrees);

                // Update taxon tables.
                DataServer.UpdateTaxonTables(context);

                LogManager.Log(context, "End taxon information update", LogType.Information, null);
            }
            catch (Exception exception)
            {
                // All errors are catched.
                // Keep taxon information update thread alive
                // so that it will try again to update taxon information.
                try
                {
                    LogManager.LogError(context, exception);
                }
                catch
                {
                }
            }
            finally
            {
                // Release cached data.
                CacheManager.FireRefreshCache(ManagerBase.GetUserContext(context));
            }
        }

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTrees">Contains information about all valid taxon trees.</param>
        private static void UpdateTaxonInformation(WebServiceContext context,
                                                   TaxonTreeNodeList taxonTrees)
        {
            DataColumn column;
            DataRow row;
            DataTable taxaTable;
            Hashtable taxonTreeCache;
            TaxonList parentTaxa;

            taxonTreeCache = new Hashtable();
            foreach (ITaxonTreeNode taxonTree in taxonTrees)
            {
                foreach (ITaxonTreeNode taxonTreeNode in taxonTree.GetTaxonTreeNodes())
                {
                    if (!taxonTreeCache.ContainsKey(taxonTreeNode.Taxon.Id))
                    {
                        taxonTreeCache[taxonTreeNode.Taxon.Id] = taxonTreeNode;
                    }
                }
            }

            // Create table.
            taxaTable = new DataTable(TaxonData.TABLE_UPDATE_NAME);
            column = new DataColumn(TaxonData.TAXON_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SORT_ORDER, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_TYPE_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SCIENTIFIC_NAME, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.AUTHOR, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.COMMON_NAME, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.KINGDOM, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.PHYLUM, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.CLASS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORDER, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.FAMILY, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.GENUS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORGANISM_GROUP_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.IS_SWEDISH_TAXON, typeof(Boolean));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.IS_REDLISTED, typeof(Boolean));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.IS_REDLISTED_SPECIES, typeof(Boolean));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.IS_NATURA2000_LISTED, typeof(Boolean));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.REDLIST_CATEGORY_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORGANISM_GROUP, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORGANISM_SUB_GROUP_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORGANISM_SUB_GROUP, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.REDLIST_TAXON_CATEGORY_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.REDLIST_CATEGORY, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.REDLIST_CRITERIA, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.LANDSCAPE, typeof(String));
            taxaTable.Columns.Add(column);

            // Add information into table.
            foreach (ITaxonTreeNode taxonTreeNode in taxonTreeCache.Values)
            {
                row = taxaTable.NewRow();
                row[0] = taxonTreeNode.Taxon.Id;
                row[1] = taxonTreeNode.Taxon.SortOrder;
                row[2] = taxonTreeNode.Taxon.Category.Id;
                row[3] = taxonTreeNode.Taxon.ScientificName;
                row[4] = taxonTreeNode.Taxon.Author;
                row[5] = taxonTreeNode.Taxon.CommonName;
                parentTaxa = new TaxonList();
                parentTaxa.Add(taxonTreeNode.Taxon);
                parentTaxa.AddRange(taxonTreeNode.GetParentTaxa());
                row[6] = null; // Kingdom.
                row[7] = null; // Phylum.
                row[8] = null; // Class.
                row[9] = null; // Order.
                row[10] = null; // Family.
                row[11] = null; // Genus.
                foreach (ITaxon parentTaxon in parentTaxa)
                {
                    switch ((TaxonCategoryId)(parentTaxon.Category.Id))
                    {
                        case TaxonCategoryId.Kingdom:
                            row[6] = parentTaxon.ScientificName;
                            break;
                        case TaxonCategoryId.Phylum:
                            row[7] = parentTaxon.ScientificName;
                            break;
                        case TaxonCategoryId.Class:
                            row[8] = parentTaxon.ScientificName;
                            break;
                        case TaxonCategoryId.Order:
                            row[9] = parentTaxon.ScientificName;
                            break;
                        case TaxonCategoryId.Family:
                            row[10] = parentTaxon.ScientificName;
                            break;
                        case TaxonCategoryId.Genus:
                            row[11] = parentTaxon.ScientificName;
                            break;
                    }
                }
                row[12] = -333;  // Species fact added in stored procedure.
                row[13] = false; // Species fact added in stored procedure.
                row[14] = false; // Species fact added in stored procedure.
                row[15] = false; // Species fact added in stored procedure.
                row[16] = false; // Species fact added in stored procedure.
                row[17] = -333;  // Species fact added in stored procedure.
                row[18] = null;  // Species fact added in stored procedure.
                row[19] = -333;  // Species fact added in stored procedure.
                row[20] = null;  // Species fact added in stored procedure.
                row[21] = null;  // Species fact added in stored procedure.
                row[22] = null;  // Species fact added in stored procedure.
                row[23] = null;  // Species fact added in stored procedure.
                row[24] = null;  // Species fact added in stored procedure.
                taxaTable.Rows.Add(row);
            }

            // Copy data into database.
            DataServer.AddTaxa(context, taxaTable);
        }

        /// <summary>
        /// Update taxon name information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTrees">Contains information about all valid taxon trees.</param>
        private static void UpdateTaxonNameInformation(WebServiceContext context,
                                                       TaxonTreeNodeList taxonTrees)
        {
            DataColumn column;
            DataRow row;
            DataTable taxonNameTable;
            List<TaxonNameList> allTaxonNames;
            TaxonList taxa;

            // Get taxon names.
            taxa = new TaxonList(true);
            foreach (ITaxonTreeNode taxonTree in taxonTrees)
            {
                taxa.Merge(taxonTree.GetTaxa());
            }
            allTaxonNames = CoreData.TaxonManager.GetTaxonNames(ManagerBase.GetUserContext(context),
                                                                taxa);

            // Create table.
            taxonNameTable = new DataTable(TaxonNameData.TABLE_UPDATE_NAME);
            column = new DataColumn(TaxonNameData.TAXON_NAME_ID, typeof(Int32));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.TAXON_ID, typeof(Int32));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.TAXON_NAME_TYPE_ID, typeof(Int32));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.TAXON_NAME_USE_TYPE_ID, typeof(Int32));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.NAME, typeof(String));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.AUTHOR, typeof(String));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.IS_RECOMMENDED, typeof(Boolean));
            taxonNameTable.Columns.Add(column);
            column = new DataColumn(TaxonNameData.TAXON_TYPE_ID, typeof(Int32));
            taxonNameTable.Columns.Add(column);

            // Add information into table.
            foreach (TaxonNameList taxonNames in allTaxonNames)
            {
                if (taxonNames.IsNotEmpty())
                {
                    foreach (ITaxonName taxonName in taxonNames)
                    {
                        if ((taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming)) ||
                            (taxonName.Status.Id == (Int32)(TaxonNameStatusId.InvalidNaming)) ||
                            (taxonName.Status.Id == (Int32)(TaxonNameStatusId.Misspelled)))
                        {
                            row = taxonNameTable.NewRow();
                            row[0] = taxonName.Id;
                            row[1] = taxonName.Taxon.Id;
                            row[2] = taxonName.Category.Id;
                            row[3] = taxonName.Status.Id;
                            row[4] = taxonName.Name;
                            row[5] = taxonName.Author;
                            row[6] = taxonName.IsRecommended;
                            row[7] = taxonName.Taxon.Category.Id;
                            taxonNameTable.Rows.Add(row);
                        }
                    }
                }
            }

            // Copy data into database.
            DataServer.AddTaxonNames(context, taxonNameTable);
        }

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="taxonTree">Contains information about current taxon tree.</param>
        /// <param name="isRootTaxonTree">True if taxon tree is a root taxon tree node.</param>
        /// <param name="taxonTreeTable">Contains information about current taxon tree.</param>
        private static void UpdateTaxonTreeInformation(ITaxonTreeNode taxonTree,
                                                       Boolean isRootTaxonTree,
                                                       DataTable taxonTreeTable)
        {
            DataRow row;
            TaxonList childTaxa;

            // Add information about current taxon tree node.
            row = taxonTreeTable.NewRow();
            row[0] = taxonTree.Taxon.Id;
            row[1] = taxonTree.Taxon.Id;
            if (isRootTaxonTree)
            {
                row[2] = 0;
            }
            else
            {
                row[2] = 1;
            }
            taxonTreeTable.Rows.Add(row);

            if (taxonTree.Children.IsNotEmpty())
            {
                // Add information about nearest child taxa relations
                foreach (ITaxonTreeNode nearestChild in taxonTree.Children)
                {
                    row = taxonTreeTable.NewRow();
                    row[0] = taxonTree.Taxon.Id;
                    row[1] = nearestChild.Taxon.Id;
                    row[2] = 2;
                    taxonTreeTable.Rows.Add(row);
                }

                // Add information about child taxa relations.
                childTaxa = new TaxonList(true);
                foreach (ITaxonTreeNode nearestChild in taxonTree.Children)
                {
                    childTaxa.Merge(nearestChild.GetChildTaxa());
                }
                foreach (ITaxon childTaxon in childTaxa)
                {
                    row = taxonTreeTable.NewRow();
                    row[0] = taxonTree.Taxon.Id;
                    row[1] = childTaxon.Id;
                    row[2] = 3;
                    taxonTreeTable.Rows.Add(row);
                }

                // Add information about child taxon trees.
                foreach (ITaxonTreeNode nearestChild in taxonTree.Children)
                {
                    UpdateTaxonTreeInformation(nearestChild, false, taxonTreeTable);
                }
            }
        }

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTrees">Contains information about all valid taxon trees.</param>
        private static void UpdateTaxonTreeInformation(WebServiceContext context,
                                                       TaxonTreeNodeList taxonTrees)
        {
            DataColumn column;
            DataTable taxonTreeTable;

            // Create table.
            taxonTreeTable = new DataTable(TaxonTreeData.TABLE_UPDATE_NAME);
            column = new DataColumn(TaxonTreeData.PARENT_TAXON_ID, typeof(Int32));
            taxonTreeTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeData.CHILD_TAXON_ID, typeof(Int32));
            taxonTreeTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeData.PARENT_CHILD_RELATION_ID, typeof(Int32));
            taxonTreeTable.Columns.Add(column);

            // Add information into table.
            foreach (ITaxonTreeNode taxonTree in taxonTrees)
            {
                UpdateTaxonTreeInformation(taxonTree, true, taxonTreeTable);
            }

            // Copy data into database.
            DataServer.AddTaxonTrees(context, taxonTreeTable);
        }
    }
}
