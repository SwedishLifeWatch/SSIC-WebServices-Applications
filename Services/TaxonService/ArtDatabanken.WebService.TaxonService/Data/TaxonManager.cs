using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;
using WebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using WebTaxonName = ArtDatabanken.WebService.Data.WebTaxonName;
using WebTaxonNameSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonNameSearchCriteria;
using WebTaxonSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonSearchCriteria;
using WebTaxonTreeNode = ArtDatabanken.WebService.Data.WebTaxonTreeNode;
using WebTaxonTreeSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonTreeSearchCriteria;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Manager of the taxon information.
    /// </summary>
    public class TaxonManager
    {
        /// <summary>
        /// Check for circular taxon trees.
        /// The the circular chain are broken up.
        /// </summary>
        /// <param name="taxonTrees">Taxon trees that should be checked.</param>
        private static void CheckCircularTree(List<TaxonTreeNode> taxonTrees)
        {
            DataIdInt32List taxonIds;

            if (taxonTrees.IsNotEmpty())
            {
                foreach (TaxonTreeNode taxonTree in taxonTrees)
                {
                    taxonIds = new DataIdInt32List(true);
                    taxonIds.Add(taxonTree.TaxonId);
                    CheckCircularTree(taxonTree, taxonIds);
                }
            }
        }

        /// <summary>
        /// Check for circular taxon trees.
        /// The the circular chain are broken up.
        /// </summary>
        /// <param name="taxonTree">Taxon tree that should be checked.</param>
        /// <param name="taxonIds">Child taxon ids found so far in the taxon tree.</param>
        private static void CheckCircularTree(TaxonTreeNode taxonTree,
                                              DataIdInt32List taxonIds)
        {
            Int32 childIndex;
            DataIdInt32List childTaxonIds;

            if (taxonTree.IsNotNull())
            {
                if (taxonTree.Children.IsNotEmpty())
                {
                    for (childIndex = taxonTree.Children.Count - 1; 0 <= childIndex; childIndex--)
                    {
                        if (taxonIds.Exists(taxonTree.Children[childIndex].TaxonId))
                        {
                            // Remove taxon tree node from children.
                            if (taxonTree.ChildrenCircular.IsNull())
                            {
                                taxonTree.ChildrenCircular = new List<TaxonTreeNode>();
                            }

                            taxonTree.ChildrenCircular.Add(taxonTree.Children[childIndex]);
                            taxonTree.Children.RemoveAt(childIndex);
                        }
                        else
                        {
                            childTaxonIds = new DataIdInt32List(true);
                            childTaxonIds.AddRange(taxonIds);
                            childTaxonIds.Add(taxonTree.Children[childIndex].TaxonId);
                            CheckCircularTree(taxonTree.Children[childIndex], childTaxonIds);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clear the cache with roles for user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void ClearCacheForUserRoles(WebServiceContext context)
        {
            String cacheKey = "RolesForUser:" +
                       context.ClientToken.UserName +
                       ":WhenUsingApplication:" +
                       context.ClientToken.ApplicationIdentifier +
                       ":WithLocale:" +
                       context.Locale.ISOCode;
            context.RemoveCachedObject(cacheKey);
        }

        /// <summary>
        /// Clear cached taxon information.
        /// This must be done since WebTaxon contains 
        /// property IsInRevision that indicates if the
        /// taxon is in an ongoing revision or not.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void ClearCachedTaxa(WebServiceContext context)
        {
            IDictionaryEnumerator cacheEnum;
            List<String> cacheKeys;
            String cacheKeyStart;

            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    WebServiceData.LogManager.Log(context, "Start of clearing cached taxa in CheckOutTaxonRevision.",
                                                  LogType.Information, null);
                    cacheKeyStart = Settings.Default.TaxaCacheKey + ":";
                    cacheEnum = HttpContext.Current.Cache.GetEnumerator();
                    cacheKeys = new List<String>();
                    while (cacheEnum.MoveNext())
                    {
                        if (((String) (cacheEnum.Key)).StartsWith(cacheKeyStart))
                        {
                            cacheKeys.Add((String) (cacheEnum.Key));
                        }
                    }
                    foreach (String cacheKey in cacheKeys)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                    WebServiceData.LogManager.Log(context, "End of clearing cached taxa in CheckOutTaxonRevision.",
                                                  LogType.Information, null);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="lumpSplitEvent">
        /// The lump split event.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebLumpSplitEvent CreateLumpSplitEvent(WebServiceContext context, WebLumpSplitEvent lumpSplitEvent)
        {
            Int32? changedInRevisionEventId, replacedInRevisionEventId;

            context.CheckTransaction();
            lumpSplitEvent.CheckNotNull("lumpSplitEvent");
            lumpSplitEvent.CheckData();
            changedInRevisionEventId = null;
            if (lumpSplitEvent.IsChangedInTaxonRevisionEventIdSpecified)
            {
                changedInRevisionEventId = lumpSplitEvent.ChangedInTaxonRevisionEventId;
            }
            replacedInRevisionEventId = null;
            if (lumpSplitEvent.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                replacedInRevisionEventId = lumpSplitEvent.ReplacedInTaxonRevisionEventId;
            }

            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);
            var id = context.GetTaxonDatabase().CreateLumpSplitEvent(replacedInRevisionEventId, context.GetUser(), DateTime.Now, lumpSplitEvent.Description, DateTime.Now, lumpSplitEvent.TypeId, GetPersonName(context), changedInRevisionEventId, lumpSplitEvent.TaxonIdAfter, lumpSplitEvent.TaxonIdBefore, context.Locale.Id);
            return GetLumpSplitEventById(context, id);
        }

        /// <summary>
        /// Creates the revision.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>WebRevision object with the created revision</returns>
        public static WebTaxonRevision CreateRevision(WebServiceContext context, WebTaxonRevision revision)
        {
            WebTaxonRevision updatedRevision;

            // Check authority - AuthorityIdentifier.TaxonRevisionAdministration
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.TaxonRevisionAdministration);

            // Check data.
            context.CheckTransaction();
            revision.CheckData();

            // Create revision.
            var revisionId = context.GetTaxonDatabase().CreateRevision(revision.RootTaxon.Id, revision.Description, revision.ExpectedStartDate, revision.ExpectedEndDate, revision.StateId, revision.CreatedBy, context.Locale.Id, null);
            // save all taxon ids into table TaxonInRevision
            context.GetTaxonDatabase().CreateTaxonInRevision(revisionId);
            updatedRevision = GetRevisionById(context, revisionId);
            ClearCacheForUserRoles(context);

            return updatedRevision;
        }

        /// <summary>
        /// Create a new taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxon">Object representing the taxon.</param>
        /// <param name="revisionEvent">Revision event.</param>
        /// <returns>WebTaxon object with the created taxon.</returns>
        public static WebTaxon CreateTaxon(WebServiceContext context,
                                           WebTaxon taxon,
                                           WebTaxonRevisionEvent revisionEvent)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            Int32 taxonId;
            String personName;

            // Check arguments.
            context.CheckTransaction();
            taxon.CheckNotNull("taxon");
            taxon.CheckData();
            personName = GetPersonName(context);
            personName = personName.CheckInjection();
            taxonId = context.GetTaxonDatabase().CreateTaxon(WebServiceData.UserManager.GetUser(context).Id, personName,
                taxon.ValidFromDate, taxon.ValidToDate, context.Locale.Id, revisionEvent != null ? revisionEvent.Id : 0,
                taxon.IsPublished, null);

            return GetTaxonByIdAfterCreate(context, taxonId);
        }

        /// <summary>
        /// Creates a new taxon name
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonName">Object representing the TaxonName.</param>
        /// <returns>WebTaxon object with the created Taxon.</returns>
        public static WebTaxonName CreateTaxonName(WebServiceContext context, WebTaxonName taxonName)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);
            if (!WebServiceData.AuthorizationManager.IsIdentiferInCurrentRole(context, Settings.Default.TaxonRevisionGuidPrefix))
            {
                throw new ApplicationException("User:" + context.GetUser().UserName + " is not editor of revision in current role.");
            }

            Int32 taxonNameVersion;
            Int32 nameCategoryId;
            Int32 nameUsageId;
            Int32 nameUsageNewId;
            Int32? revisionEventId;
            String personName;

            // Check arguments.
            context.CheckTransaction();
            taxonName.CheckNotNull("TaxonName");
            taxonName.CheckData();
            nameCategoryId = taxonName.CategoryId;
            nameUsageId = taxonName.StatusId;
            nameUsageNewId = taxonName.GetNameUsageId();
            revisionEventId = null;
            if (taxonName.IsChangedInTaxonRevisionEventIdSpecified)
            {
                revisionEventId = taxonName.ChangedInTaxonRevisionEventId;
            }

            personName = GetPersonName(context);
            personName = personName.CheckInjection();
            taxonNameVersion = context.GetTaxonDatabase().CreateTaxonName(taxonName.Taxon.Id, taxonName.Id, taxonName.Name, taxonName.Author, nameCategoryId, nameUsageId, nameUsageNewId,
                        personName, taxonName.IsRecommended, taxonName.CreatedDate, WebServiceData.UserManager.GetUser(context).Id, taxonName.ValidFromDate, taxonName.ValidToDate,
                        taxonName.Description, revisionEventId, taxonName.IsPublished, taxonName.IsOkForSpeciesObservation, taxonName.IsOriginalName, context.Locale.Id);

            WebTaxonName newTaxonName = GetTaxonNameByVersion(context, taxonNameVersion);
            return newTaxonName;
        }

        /// <summary>
        /// Get all taxa that has been published.
        /// This method should not be called from within a revision.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxa that has been published.</returns>
        private static Hashtable GetCachedTaxa(WebServiceContext context)
        {
            Hashtable cachedTaxa;
            String cacheKey;
            WebTaxon taxon;

            // Get cached information.
            cacheKey = Settings.Default.TaxaCacheKey + ":" + context.Locale.ISOCode;
            cachedTaxa = (Hashtable)(context.GetCachedObject(cacheKey));

            if (cachedTaxa.IsNull())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxa(context.Locale.Id))
                {
                    cachedTaxa = new Hashtable();
                    while (dataReader.Read())
                    {
                        taxon = new WebTaxon();
                        taxon.LoadData(dataReader);
                        SetTaxonChangeStatus(taxon);
                        cachedTaxa[GetTaxonCacheKey(context, taxon.Id)] = taxon;
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey,
                                            cachedTaxa,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.AboveNormal);
                }
            }

            return cachedTaxa;
        }

        /// <summary>
        /// Get taxon from cache.
        /// This method should not be called from within a revision.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Cached taxon or null if taxon was not found in cache.</returns>
        private static WebTaxon GetCachedTaxon(WebServiceContext context, Int32 taxonId)
        {
            return (WebTaxon)(GetCachedTaxa(context)[GetTaxonCacheKey(context, taxonId)]);
        }

        /// <summary>
        /// Get taxon relations from cache.
        /// Only information that is not in a revision is handled by this method.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Cached taxon relations.</returns>
        private static List<WebTaxonRelation> GetCachedTaxonRelations(WebServiceContext context)
        {
            List<WebTaxonRelation> cachedTaxonRelations;
            String cacheKey;
            WebTaxonRelation webTaxonRelation;

            // Get cached information.
            cacheKey = Settings.Default.TaxonRelationsCacheKey;
            cachedTaxonRelations = (List<WebTaxonRelation>)(context.GetCachedObject(cacheKey));

            if (cachedTaxonRelations.IsNull())
            {
                // Data not in cache. 
                // Create all taxon relations.
                cachedTaxonRelations = new List<WebTaxonRelation>();

                // Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRelations(null))
                {
                    while (dataReader.Read())
                    {
                        webTaxonRelation = new WebTaxonRelation();
                        webTaxonRelation.LoadData(dataReader);
                        cachedTaxonRelations.Add(webTaxonRelation);
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey,
                                            cachedTaxonRelations,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.AboveNormal);
                }
            }

            return cachedTaxonRelations;
        }

        /// <summary>
        /// Get taxon tree roots from cache.
        /// Only information that is not in a revision is handled by this method
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Cached taxon tree roots.</returns>
        private static List<TaxonTreeNode> GetCachedTaxonTreeRoots(WebServiceContext context,
                                                                   WebTaxonTreeSearchCriteria searchCriteria)
        {
            Hashtable taxonTreeNodes;
            List<TaxonTreeNode> taxonTrees;
            List<WebTaxonRelation> taxonRelations;
            String cacheKey;
            WebTaxonRelationSearchCriteria taxonRelationSearchCriteria;

            // Get cached information.
            cacheKey = Settings.Default.TaxonTreeRootsCacheKey;
            if (searchCriteria.IsMainRelationRequired)
            {
                cacheKey += ":isMainRelationRequired";
            }
            if (searchCriteria.IsValidRequired)
            {
                cacheKey += ":isValidRequired";
            }
            taxonTrees = (List<TaxonTreeNode>)(context.GetCachedObject(cacheKey));

            if (taxonTrees.IsNull())
            {
                // Data not in cache.
                taxonRelationSearchCriteria = GetTaxonRelationSearchCriteria(searchCriteria);
                taxonRelations = GetTaxonRelationsBySearchCriteria(context, taxonRelationSearchCriteria);
                taxonTreeNodes = GetTaxonTrees(context, taxonRelations);
                taxonTrees = GetTaxonTreeRoots(taxonTreeNodes,
                                               searchCriteria.IsMainRelationRequired,
                                               searchCriteria.IsValidRequired,
                                               searchCriteria.TaxonIds);

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonTrees,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }

            return taxonTrees;
        }

        /// <summary>
        /// Get taxon trees from cache.
        /// Only information that is not in a revision is handled by this method
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Cached taxon trees.</returns>
        private static Hashtable GetCachedTaxonTrees(WebServiceContext context)
        {
            Hashtable cachedTaxonTrees;
            List<WebTaxon> taxa;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.TaxonTreesCacheKey;
            cachedTaxonTrees = (Hashtable)(context.GetCachedObject(cacheKey));

            if (cachedTaxonTrees.IsNull())
            {
                // Data not in cache.
                // Get taxon trees.
                taxa = new List<WebTaxon>();
                foreach (WebTaxon taxon in GetCachedTaxa(context).Values)
                {
                    taxa.Add(taxon);
                }
                cachedTaxonTrees = GetTaxonTrees(taxa, GetCachedTaxonRelations(context));

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        cachedTaxonTrees,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }

            return cachedTaxonTrees;
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All lump split event types.</returns>
        public static List<WebLumpSplitEventType> GetLumpSplitEventTypes(WebServiceContext context)
        {
            List<WebLumpSplitEventType> lumpSplitEventTypes;
            String cacheKey;
            WebLumpSplitEventType lumpSplitEventType;

            // Get cached information.
            cacheKey = Settings.Default.LumpSplitEventTypeCacheKey + ":" + context.Locale.ISOCode;
            lumpSplitEventTypes = (List<WebLumpSplitEventType>)(context.GetCachedObject(cacheKey));

            if (lumpSplitEventTypes.IsEmpty())
            {
                // Data not in cache. Get information.
                lumpSplitEventTypes = new List<WebLumpSplitEventType>();
                foreach (LumpSplitEventTypeId lumpSplitEventTypeId in Enum.GetValues(typeof(LumpSplitEventTypeId)))
                {
                    lumpSplitEventType = new WebLumpSplitEventType();
                    lumpSplitEventType.Id = (Int32)lumpSplitEventTypeId;
                    lumpSplitEventType.Identifier = lumpSplitEventTypeId.ToString();
                    lumpSplitEventTypes.Add(lumpSplitEventType);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        lumpSplitEventTypes,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return lumpSplitEventTypes;
        }

        /// <summary>
        /// Get the Swedish concept defintion for a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxon">Object representing the Taxon.</param>
        /// <returns>A string represnting the definition of a taxon.</returns>
        private static String GetSwedishTaxonConceptDefinition(WebServiceContext context, WebTaxon taxon)
        {
            //Resources in swedish
            const String AND_DELIMITER_TEXT = "och";
            const String CHANGE_STATUS_GRADED_TEXT = "Taxonet har bytt kategori från [FormerTaxonCategory] till [CurrentTaxonCategory].";
            const String CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT = "Taxonet har slagits samman med ett annat taxon och har därmed ersatts av [TaxonNameAndIdOfReplacingTaxon].";
            const String CHANGE_STATUS_LUMPED_TEXT = "Detta taxon har slagits samman med [TaxonNamesAndIdsOfOtherLumpedTaxa] har därmed ersatts av ett nytt taxon, [TaxonNameAndIdOfReplacingTaxon] ([TaxonCategoryOfReplacingTaxon]). Taxonet är alltså inte längre accepterat.";
            const String CHANGE_STATUS_REPLACED_LUMPED_TAXA_TEXT = "Detta taxon har ersatt [TaxonNamesAndIdsOfLumpedTaxa]. De betraktas inte längre som separata taxa och har därför slagits ihop. ";
            const String CHANGE_STATUS_REMOVED_TEXT = "Detta taxon är inte längre accepterat och ska därför inte användas. Information som knutits till dess taxonid bör uppdateras. Taxonet ska endast förekomma i listor över taxa som underkänts.";
            //const String CHANGE_STATUS_SPLIT_TEXT = "Detta taxon har delats upp och ersatts av [TaxonNamesOfReplacingTaxa]. Detta innebär att taxonet inte längre är formellt accepterat som [FormerTaxonCategory] men kan fortsättningsvis användas som ett kollektivtaxon.";
            const String CHANGE_STATUS_SPLIT_TEXT = "Detta taxon har delats upp och utgör nu en grupp av helt separata taxa: [TaxonNamesOfReplacingTaxa]. Taxonet är inte längre accepterat som representant för sin ursprungliga kategori men kan med fördel användas som ett kollektivtaxon.";
            const String CHANGE_STATUS_REPLACED_SPLIT_TAXA_TEXT = "Detta taxon är ett resultat från uppdelningen av nuvarande [ReplacedTaxonNameAndId].";

            var revisionId = GetRevisionIdFromRole(context);
            String delimiter = " " + AND_DELIMITER_TEXT;
            String generatedText = string.Empty;
            List<WebTaxonProperties> taxonProperties = GetTaxonPropertiesByTaxonId(context, taxon.Id);
//            var parentTaxonRelations = GetParentTaxonRelationsByTaxon(context, taxon.Id);
            var replacedBy = GetLumpSplitEventsByReplacedTaxon(context, taxon.Id);
            var replaces = GetLumpSplitEventsByTaxon(context, taxon.Id);
            var replacedTaxonIds = new List<int>();
            var replacingTaxonIds = new List<int>();

//            parentTaxonRelations = parentTaxonRelations.Where(x => x.IsMainRelation).ToList();

            if (revisionId <= 0)
            {
                taxonProperties = taxonProperties.Where(x => x.IsPublished).ToList();
//                parentTaxonRelations = parentTaxonRelations.Where(x => x.IsPublished).ToList();
                replacedBy = replacedBy.Where(x => x.IsPublished).ToList();
                replaces = replaces.Where(x => x.IsPublished).ToList();
            }

            //Temporal soution for calculating whether or not taxon.IsValid
            bool isValid = (from t in taxonProperties
                            where t.IsPublished && t.ValidToDate > DateTime.Now
                            select t.IsValid).FirstOrDefault();


            var noOfVersionsWithDifferentCategory = (from t in taxonProperties
                                                     where t.TaxonCategory.Id != taxon.CategoryId
                                                     select t).Count();

            int currentTaxonCategoryId = (from t in taxonProperties
                                          where t.ValidToDate > DateTime.Now
                                          select t.TaxonCategory.Id).FirstOrDefault();

            int previousTaxonCategoryId = (from t in taxonProperties
                                           where t.TaxonCategory.Id != currentTaxonCategoryId
                                           select t.TaxonCategory.Id).FirstOrDefault();




            if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToLump))
            {
                replacingTaxonIds = replacedBy.Select(x => x.TaxonIdAfter).ToList();
                // Check that the result of a lump returns one single taxon.
                if (replacingTaxonIds.Count != 1)
                {
                    throw new ArgumentException("Data for lump process found " + replacingTaxonIds.Count + " taxa.");
                }

                // Find out how many taxa that are replaced by the taxon that is the result of the lump.
                replacedTaxonIds = GetTaxonIdsReplacedInLump(context, replacingTaxonIds[0]);
                if (replacedTaxonIds.Count == 1)
                {
                    // "Technical lump" - one taxon lumped into another one
                    generatedText = CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT;
                }
                else
                {
                    // Classic lump - two (or more) taxa lumped into another taxon
                    generatedText = CHANGE_STATUS_LUMPED_TEXT;
                }
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToSplit))
            {
                replacingTaxonIds = replacedBy.Select(x => x.TaxonIdAfter).ToList();
                replacedTaxonIds.Add(taxon.Id);
                generatedText = CHANGE_STATUS_SPLIT_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.ValidAfterLump))
            {
                replacingTaxonIds.Add(taxon.Id);
                replacedTaxonIds = replaces.Select(x => x.TaxonIdBefore).ToList();
                generatedText = CHANGE_STATUS_REPLACED_LUMPED_TAXA_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.ValidAfterSplit))
            {
                replacingTaxonIds = GetLumpSplitEventsByReplacedTaxon(context, replaces.First().TaxonIdBefore).Select(x => x.TaxonIdAfter).ToList();
                replacedTaxonIds = replaces.Select(x => x.TaxonIdBefore).ToList();
                generatedText = CHANGE_STATUS_REPLACED_SPLIT_TAXA_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToDelete))
            {
                generatedText = CHANGE_STATUS_REMOVED_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.Unchanged))
            {
                bool isGraded = (noOfVersionsWithDifferentCategory > 0 && previousTaxonCategoryId > 0);
                if (isGraded)
                {
                    // Graded
                    generatedText = generatedText + " " + CHANGE_STATUS_GRADED_TEXT;
                }
            }

            // Replace tags with values
            // [TaxonNamesAndIdsOfLumpedTaxa]
            var taxonNamesAndIdsOfReplacedTaxa = string.Empty;
            var firstIteration = true;
            foreach (var replacedTaxonId in replacedTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa + delimiter;
                }
                var replacedTaxon = GetTaxonById(context, replacedTaxonId);
                taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa + " " + replacedTaxon.ScientificName + " [" + replacedTaxonId + "]";
                firstIteration = false;
            }

            taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa.Trim();
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpingForLumpedTaxa, taxonNamesAndIdsOfReplacedTaxa);

            // [TaxonNamesAndIdsOfOtherLumpedTaxa]
            var taxonNamesAndIdsOfOtherReplacedTaxa = string.Empty;
            firstIteration = true;
            foreach (var replacedTaxonId in replacedTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa + delimiter;
                }

                if (replacedTaxonId != taxon.Id)
                {
                    var replacedTaxon = GetTaxonById(context, replacedTaxonId);
                    taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa + " " + replacedTaxon.ScientificName + " [" + replacedTaxonId + "]";
                    firstIteration = false;
                }
            }

            taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa.Trim();
            if (taxonNamesAndIdsOfOtherReplacedTaxa.IsEmpty() && taxonNamesAndIdsOfReplacedTaxa.IsNotEmpty())
            {
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForOtherLumpedTaxa, taxonNamesAndIdsOfReplacedTaxa);
            }
            else
            {
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForOtherLumpedTaxa, taxonNamesAndIdsOfOtherReplacedTaxa);
            }

            if (replacingTaxonIds.Count == 1)
            {
                var replacingTaxon = GetTaxonById(context, replacingTaxonIds[0]);
                //[TaxonCategoryOfReplacingTaxon]
                var taxonCategoryOfReplacingTaxon = GetTaxonCategoryById(context, replacingTaxon.CategoryId).Name.ToLower();
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForCategoryOfReplacingTaxon, taxonCategoryOfReplacingTaxon);
                //[TaxonNameAndIdOfReplacingTaxon]
                var taxonNameAndIdOfReplacingTaxon = replacingTaxon.ScientificName + " [" + replacingTaxon.Id.ToString() + "]"; ;
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForReplacingTaxon, taxonNameAndIdOfReplacingTaxon);

                if (taxonNamesAndIdsOfOtherReplacedTaxa == taxonNameAndIdOfReplacingTaxon || taxonNamesAndIdsOfReplacedTaxa == taxonNameAndIdOfReplacingTaxon)
                {
                    generatedText = CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT;
                    generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForReplacingTaxon, taxonNameAndIdOfReplacingTaxon);
                }
            }

            //[TaxonNamesOfReplacingTaxa]
            var taxonNamesOfReplacingTaxa = string.Empty;
            firstIteration = true;
            foreach (var replacingTaxon in replacingTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesOfReplacingTaxa = taxonNamesOfReplacingTaxa + delimiter;
                }

                if (replacingTaxon != taxon.Id)
                {
                    taxonNamesOfReplacingTaxa = taxonNamesOfReplacingTaxa + " " + GetTaxonById(context, replacingTaxon).ScientificName + " [" + replacingTaxon + "]";
                    firstIteration = false;
                }
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittedForReplacingTaxa, taxonNamesOfReplacingTaxa);

            if (replacedTaxonIds.Count == 1)
            {
                var replacedTaxon = GetTaxonById(context, replacedTaxonIds[0]);
                //[TaxonCategoryOfReplacedTaxon] 
                var taxonCategoryOfReplacedTaxon = GetTaxonCategoryById(context, replacedTaxon.CategoryId).Name.ToLower();
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittingForCategoryOfReplacedTaxon, taxonCategoryOfReplacedTaxon);
                //[TaxonNameAndIdOfReplacedTaxon]
                var taxonNameAndIdOfReplacedTaxon = replacedTaxon.ScientificName + " [" + replacedTaxon.Id.ToString() + "]"; ;
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittingForReplacedTaxon, taxonNameAndIdOfReplacedTaxon);
            }

            // [FormerTaxonCategory]
            var previousTaxonCategoryOfCurrentTaxon = string.Empty;
            if (previousTaxonCategoryId > 0)
            {
                previousTaxonCategoryOfCurrentTaxon = GetTaxonCategoryById(context, previousTaxonCategoryId).Name.ToLower();
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagForFormerCategory, previousTaxonCategoryOfCurrentTaxon);

            // [CurrentTaxonCategory]
            var currentTaxonCategoryOfCurrentTaxon = string.Empty;
            if (currentTaxonCategoryId > 0)
            {
                currentTaxonCategoryOfCurrentTaxon = GetTaxonCategoryById(context, currentTaxonCategoryId).Name.ToLower();
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagForCurrentCategory, currentTaxonCategoryOfCurrentTaxon);

            //Add custom text
            if (taxon.PartOfConceptDefinition.IsNotEmpty())
            {
                generatedText = generatedText + " " + taxon.PartOfConceptDefinition;
            }

            return generatedText.Trim();
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon alert statuses.</returns>
        public static List<WebTaxonAlertStatus> GetTaxonAlertStatuses(WebServiceContext context)
        {
            List<WebTaxonAlertStatus> taxonAlertStatuses;
            String cacheKey;
            WebTaxonAlertStatus taxonAlertStatus;

            // Get cached information.
            cacheKey = Settings.Default.TaxonAlertStatusCacheKey + ":" + context.Locale.ISOCode;
            taxonAlertStatuses = (List<WebTaxonAlertStatus>)(context.GetCachedObject(cacheKey));

            if (taxonAlertStatuses.IsEmpty())
            {
                // Data not in cache. Get information.
                taxonAlertStatuses = new List<WebTaxonAlertStatus>();
                foreach (TaxonAlertStatusId taxonAlertStatusId in Enum.GetValues(typeof(TaxonAlertStatusId)))
                {
                    taxonAlertStatus = new WebTaxonAlertStatus();
                    taxonAlertStatus.Id = (Int32)taxonAlertStatusId;
                    taxonAlertStatus.Identifier = taxonAlertStatusId.ToString();
                    taxonAlertStatuses.Add(taxonAlertStatus);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonAlertStatuses,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonAlertStatuses;
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon change statuses.</returns>
        public static List<WebTaxonChangeStatus> GetTaxonChangeStatuses(WebServiceContext context)
        {
            List<WebTaxonChangeStatus> taxonChangeStatuses;
            String cacheKey;
            WebTaxonChangeStatus taxonChangeStatus;

            // Get cached information.
            cacheKey = Settings.Default.TaxonChangeStatusCacheKey + ":" + context.Locale.ISOCode;
            taxonChangeStatuses = (List<WebTaxonChangeStatus>)(context.GetCachedObject(cacheKey));

            if (taxonChangeStatuses.IsEmpty())
            {
                // Data not in cache. Get information.
                taxonChangeStatuses = new List<WebTaxonChangeStatus>();
                foreach (TaxonChangeStatusId taxonChangeStatusId in Enum.GetValues(typeof(TaxonChangeStatusId)))
                {
                    taxonChangeStatus = new WebTaxonChangeStatus();
                    taxonChangeStatus.Id = (Int32)taxonChangeStatusId;
                    taxonChangeStatus.Identifier = taxonChangeStatusId.ToString();
                    taxonChangeStatuses.Add(taxonChangeStatus);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonChangeStatuses,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonChangeStatuses;
        }

        /// <summary>
        /// Get the english concept defintion for a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxon">Object representing the Taxon.</param>
        /// <returns>A string represnting the definition of a taxon.</returns>
        private static String GetEnglishTaxonConceptDefinition(WebServiceContext context, WebTaxon taxon)
        {
            //Resources in english
            const String AND_DELIMITER_TEXT = "and";
            const String CHANGE_STATUS_GRADED_TEXT = "This taxon has changed rank from [FormerTaxonCategory] to [CurrentTaxonCategory].";
            const String CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT = "This taxon has been lumped into, and replaced by, [TaxonNameAndIdOfReplacingTaxon]. The taxon is no longer accepted.";
            const String CHANGE_STATUS_LUMPED_TEXT = "This taxon has been lumped with [TaxonNamesAndIdsOfOtherLumpedTaxa] replaced by the [TaxonCategoryOfReplacingTaxon] [TaxonNameAndIdOfReplacingTaxon]. The taxon is no longer an accepted [CurrentTaxonCategory].";
            const String CHANGE_STATUS_REPLACED_LUMPED_TAXA_TEXT = "This taxon is a result of the lumping of [TaxonNamesAndIdsOfLumpedTaxa].";
            const String CHANGE_STATUS_REMOVED_TEXT = "This taxon has been removed and any information that has been related to its taxon id should be revised. The taxon should only be listed when reporting taxon concepts that are no longer accepted.";
            //const String CHANGE_STATUS_SPLIT_TEXT = "This taxon has been split and is now replaced by [TaxonNamesOfReplacingTaxa]. The taxon is no longer accepted as a [FormerTaxonCategory] but can be used as a collective taxon representing the group of replacing taxa.";
            const String CHANGE_STATUS_SPLIT_TEXT = "This taxon has been split and is now replaced by [TaxonNamesOfReplacingTaxa]. The taxon is no longer accepted at its former rank, but can be used as a collective taxon representing the group of replacing taxa.";
            const String CHANGE_STATUS_REPLACED_SPLIT_TAXA_TEXT = "This taxon is the result of a split of what is now considered the [TaxonCategoryOfReplacedTaxon]  [ReplacedTaxonNameAndId].";

            var revisionId = GetRevisionIdFromRole(context);
            String delimiter = " " + AND_DELIMITER_TEXT;
            String generatedText = string.Empty;
            List<WebTaxonProperties> taxonProperties = GetTaxonPropertiesByTaxonId(context, taxon.Id);
//            var parentTaxonRelations = GetParentTaxonRelationsByTaxon(context, taxon.Id);
            var replacedBy = GetLumpSplitEventsByReplacedTaxon(context, taxon.Id);
            var replaces = GetLumpSplitEventsByTaxon(context, taxon.Id);
            var replacedTaxonIds = new List<int>();
            var replacingTaxonIds = new List<int>();

//            parentTaxonRelations = parentTaxonRelations.Where(x => x.IsMainRelation).ToList();

            if (revisionId <= 0)
            {
                taxonProperties = taxonProperties.Where(x => x.IsPublished).ToList();
//                parentTaxonRelations = parentTaxonRelations.Where(x => x.IsPublished).ToList();
                replacedBy = replacedBy.Where(x => x.IsPublished).ToList();
                replaces = replaces.Where(x => x.IsPublished).ToList();
            }

            //Temporal soution for calculating whether or not taxon.IsValid
            bool isValid = (from t in taxonProperties
                            where t.IsPublished && t.ValidToDate > DateTime.Now
                            select t.IsValid).FirstOrDefault();


            var noOfVersionsWithDifferentCategory = (from t in taxonProperties
                                                     where t.TaxonCategory.Id != taxon.CategoryId
                                                     select t).Count();

            int currentTaxonCategoryId = (from t in taxonProperties
                                          where t.ValidToDate > DateTime.Now
                                          select t.TaxonCategory.Id).FirstOrDefault();

            int previousTaxonCategoryId = (from t in taxonProperties
                                           where t.TaxonCategory.Id != currentTaxonCategoryId
                                           select t.TaxonCategory.Id).FirstOrDefault();




            if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToLump))
            {
                replacingTaxonIds = replacedBy.Select(x => x.TaxonIdAfter).ToList();
                // Check that the result of a lump returns one single taxon.
                if (replacingTaxonIds.Count != 1)
                {
                    throw new ArgumentException("Data for lump process found " + replacingTaxonIds.Count + " taxa.");
                }
                // Find out how many taxa that are replaced by the taxon that is the result of the lump.
                replacedTaxonIds = GetTaxonIdsReplacedInLump(context, replacingTaxonIds[0]);
                if (replacedTaxonIds.Count == 1)
                {
                    // "Technical lump" - one taxon lumped into another one
                    generatedText = CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT;
                }
                else
                {
                    // Classic lump - two (or more) taxa lumped into another taxon
                    generatedText = CHANGE_STATUS_LUMPED_TEXT;
                }
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToSplit))
            {
                replacingTaxonIds = replacedBy.Select(x => x.TaxonIdAfter).ToList();
                replacedTaxonIds.Add(taxon.Id);
                generatedText = CHANGE_STATUS_SPLIT_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.ValidAfterLump))
            {
                replacingTaxonIds.Add(taxon.Id);
                replacedTaxonIds = replaces.Select(x => x.TaxonIdBefore).ToList();
                generatedText = CHANGE_STATUS_REPLACED_LUMPED_TAXA_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.ValidAfterSplit))
            {
                replacingTaxonIds = GetLumpSplitEventsByReplacedTaxon(context, replaces.First().TaxonIdBefore).Select(x => x.TaxonIdAfter).ToList();
                replacedTaxonIds = replaces.Select(x => x.TaxonIdBefore).ToList();
                generatedText = CHANGE_STATUS_REPLACED_SPLIT_TAXA_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.InvalidDueToDelete))
            {
                generatedText = CHANGE_STATUS_REMOVED_TEXT;
            }
            else if (taxon.ChangeStatusId == (Int32)(TaxonChangeStatusId.Unchanged))
            {
                bool isGraded = (noOfVersionsWithDifferentCategory > 0 && previousTaxonCategoryId > 0);
                if (isGraded)
                {
                    // Graded
                    generatedText = generatedText + " " + CHANGE_STATUS_GRADED_TEXT;
                }
            }

            // Replace tags with values
            // [TaxonNamesAndIdsOfLumpedTaxa]
            var taxonNamesAndIdsOfReplacedTaxa = string.Empty;
            var firstIteration = true;
            foreach (var replacedTaxonId in replacedTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa + delimiter;
                }
                var replacedTaxon = GetTaxonById(context, replacedTaxonId);
                taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa + " " + replacedTaxon.ScientificName + " [" + replacedTaxonId + "]";
                firstIteration = false;
            }
            taxonNamesAndIdsOfReplacedTaxa = taxonNamesAndIdsOfReplacedTaxa.Trim();
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpingForLumpedTaxa, taxonNamesAndIdsOfReplacedTaxa);

            // [TaxonNamesAndIdsOfOtherLumpedTaxa]
            var taxonNamesAndIdsOfOtherReplacedTaxa = string.Empty;
            firstIteration = true;
            foreach (var replacedTaxonId in replacedTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa + delimiter;
                }

                if (replacedTaxonId != taxon.Id)
                {
                    var replacedTaxon = GetTaxonById(context, replacedTaxonId);
                    taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa + " " + replacedTaxon.ScientificName + " [" + replacedTaxonId + "]";
                    firstIteration = false;
                }
            }
            taxonNamesAndIdsOfOtherReplacedTaxa = taxonNamesAndIdsOfOtherReplacedTaxa.Trim();
            if (taxonNamesAndIdsOfOtherReplacedTaxa.IsEmpty() && taxonNamesAndIdsOfReplacedTaxa.IsNotEmpty())
            {
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForOtherLumpedTaxa, taxonNamesAndIdsOfReplacedTaxa);
            }
            else
            {
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForOtherLumpedTaxa, taxonNamesAndIdsOfOtherReplacedTaxa);
            }

            if (replacingTaxonIds.Count == 1)
            {
                var replacingTaxon = GetTaxonById(context, replacingTaxonIds[0]);
                //[TaxonCategoryOfReplacingTaxon]
                var taxonCategoryOfReplacingTaxon = GetTaxonCategoryById(context, replacingTaxon.CategoryId).Name.ToLower();
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForCategoryOfReplacingTaxon, taxonCategoryOfReplacingTaxon);
                //[TaxonNameAndIdOfReplacingTaxon]
                var taxonNameAndIdOfReplacingTaxon = replacingTaxon.ScientificName + " [" + replacingTaxon.Id.ToString() + "]"; ;
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForReplacingTaxon, taxonNameAndIdOfReplacingTaxon);

                if (taxonNamesAndIdsOfOtherReplacedTaxa == taxonNameAndIdOfReplacingTaxon || taxonNamesAndIdsOfReplacedTaxa == taxonNameAndIdOfReplacingTaxon)
                {
                    generatedText = CHANGE_STATUS_LUMPED_IMTO_REPLACING_TAXON_TEXT;
                    generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagLumpedForReplacingTaxon, taxonNameAndIdOfReplacingTaxon);
                }
            }

            //[TaxonNamesOfReplacingTaxa]
            var taxonNamesOfReplacingTaxa = string.Empty;
            firstIteration = true;
            foreach (var replacingTaxon in replacingTaxonIds)
            {
                if (!firstIteration)
                {
                    taxonNamesOfReplacingTaxa = taxonNamesOfReplacingTaxa + delimiter;
                }

                if (replacingTaxon != taxon.Id)
                {
                    taxonNamesOfReplacingTaxa = taxonNamesOfReplacingTaxa + " " + GetTaxonById(context, replacingTaxon).ScientificName + " [" + replacingTaxon + "]";
                    firstIteration = false;
                }
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittedForReplacingTaxa, taxonNamesOfReplacingTaxa);

            if (replacedTaxonIds.Count == 1)
            {
                var replacedTaxon = GetTaxonById(context, replacedTaxonIds[0]);
                //[TaxonCategoryOfReplacedTaxon] 
                var taxonCategoryOfReplacedTaxon = GetTaxonCategoryById(context, replacedTaxon.CategoryId).Name.ToLower();
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittingForCategoryOfReplacedTaxon, taxonCategoryOfReplacedTaxon);
                //[TaxonNameAndIdOfReplacedTaxon]
                var taxonNameAndIdOfReplacedTaxon = replacedTaxon.ScientificName + " [" + replacedTaxon.Id.ToString() + "]"; ;
                generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagSplittingForReplacedTaxon, taxonNameAndIdOfReplacedTaxon);
            }

            // [FormerTaxonCategory]
            var previousTaxonCategoryOfCurrentTaxon = string.Empty;
            if (previousTaxonCategoryId > 0)
            {
                previousTaxonCategoryOfCurrentTaxon = GetTaxonCategoryById(context, previousTaxonCategoryId).Name.ToLower();
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagForFormerCategory, previousTaxonCategoryOfCurrentTaxon);

            // [CurrentTaxonCategory]
            var currentTaxonCategoryOfCurrentTaxon = string.Empty;
            if (currentTaxonCategoryId > 0)
            {
                currentTaxonCategoryOfCurrentTaxon = GetTaxonCategoryById(context, currentTaxonCategoryId).Name.ToLower();
            }
            generatedText = generatedText.Replace(Resources.TaxonResource.ConceptDefinitionTagForCurrentCategory, currentTaxonCategoryOfCurrentTaxon);

            //Add custom text
            if (taxon.PartOfConceptDefinition.IsNotEmpty())
            {
                generatedText = generatedText + " " + taxon.PartOfConceptDefinition;
            }

            return generatedText.Trim();
        }

        /// <summary>
        /// Get the Concept defintion for a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxon">Object representing the Taxon.</param>
        /// <returns>A string represnting the definition of a taxon.</returns>
        public static string GetTaxonConceptDefinition(WebServiceContext context, WebTaxon taxon)
        {
            if (context.Locale.ISOCode == "sv-SE")
            {
                return GetSwedishTaxonConceptDefinition(context, taxon);
            }

            return GetEnglishTaxonConceptDefinition(context, taxon);
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon name category types.</returns>
        public static List<WebTaxonNameCategoryType> GetTaxonNameCategoryTypes(WebServiceContext context)
        {
            List<WebTaxonNameCategoryType> taxonNameCategoryTypes;
            String cacheKey;
            WebTaxonNameCategoryType taxonNameCategoryType;

            // Get cached information.
            cacheKey = Settings.Default.TaxonNameCategoryTypeCacheKey + ":" + context.Locale.ISOCode;
            taxonNameCategoryTypes = (List<WebTaxonNameCategoryType>)(context.GetCachedObject(cacheKey));

            if (taxonNameCategoryTypes.IsEmpty())
            {
                // Data not in cache. Get information.
                taxonNameCategoryTypes = new List<WebTaxonNameCategoryType>();
                foreach (TaxonNameCategoryTypeId taxonNameCategoryTypeId in Enum.GetValues(typeof(TaxonNameCategoryTypeId)))
                {
                    taxonNameCategoryType = new WebTaxonNameCategoryType();
                    taxonNameCategoryType.Id = (Int32)taxonNameCategoryTypeId;
                    taxonNameCategoryType.Identifier = taxonNameCategoryTypeId.ToString();
                    taxonNameCategoryTypes.Add(taxonNameCategoryType);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameCategoryTypes,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonNameCategoryTypes;
        }

        /// <summary>
        /// Get translation for specified string.
        /// Language is specified by locale in web service context.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">String id.</param>
        /// <returns>Translation for specified string.</returns>
        public static String GetTranslation(WebServiceContext context,
                                            int id)
        {
            return context.GetTaxonDatabase().GetTranslation(id, context.Locale.Id);
        }

        /// <summary>
        /// Updates taxon name
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonName">Object representing the TaxonName.</param>
        /// <returns>WebTaxon object with the updated Taxon.</returns>
        public static WebTaxonName UpdateTaxonName(WebServiceContext context, WebTaxonName taxonName)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            int taxonNameVersion;
            Int32? changedInRevisionEventId;
            WebTaxonName newTaxonName;

            // Check arguments.
            context.CheckTransaction();
            taxonName.CheckNotNull("TaxonName");
            taxonName.CheckData();
            changedInRevisionEventId = null;
            if (taxonName.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                changedInRevisionEventId = taxonName.ReplacedInTaxonRevisionEventId;
            }

            taxonNameVersion = context.GetTaxonDatabase().UpdateTaxonName(taxonName.GetVersion(),
                                                                          WebServiceData.UserManager.GetUser(context).Id,
                                                                          changedInRevisionEventId);
            newTaxonName = GetTaxonNameByVersion(context, taxonNameVersion);
            return newTaxonName;
        }

        /// <summary>Creates new taxon properties.</summary>
        /// <param name="context">The context.</param>
        /// <param name="taxonProperties">The taxonproperties object</param>
        /// <returns>The WebTaxonProperties that has been created.</returns>
        public static WebTaxonProperties CreateTaxonProperties(WebServiceContext context, WebTaxonProperties taxonProperties)
        {
            String personName;

            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);
            
            context.CheckTransaction();
            taxonProperties.CheckData();
            personName = GetPersonName(context);
            personName = personName.CheckInjection();
            var taxonPropertiesId = context.GetTaxonDatabase().CreateTaxonProperties(
                taxonProperties.Taxon.Id,
                taxonProperties.TaxonCategory.Id,
                taxonProperties.PartOfConceptDefinition,
                taxonProperties.ConceptDefinition,
                taxonProperties.AlertStatusId,
                taxonProperties.ValidFromDate,
                taxonProperties.ValidToDate,
                taxonProperties.IsValid,
                personName,
                WebServiceData.UserManager.GetUser(context).Id,
                taxonProperties.ChangedInTaxonRevisionEvent.Id,
                taxonProperties.ReplacedInTaxonRevisionEvent == null ? 0 : taxonProperties.ReplacedInTaxonRevisionEvent.Id,
                taxonProperties.IsPublished,
                taxonProperties.DataFields.GetBoolean(TaxonPropertiesData.IS_MICROSPECIES),
                context.Locale.Id
            );

            return GetTaxonPropertiesById(context, taxonPropertiesId);
        }

        /// <summary>
        /// Internal sorting of all taxa w/ same parent taxon.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="taxonIdParent">Id of the parent taxon.</param>
        /// <param name="taxaIdChildList">Sorted list of taxa ids.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        public static void SetTaxonTreeSortOrder(WebServiceContext context, Int32 taxonIdParent, IList<Int32> taxaIdChildList, Int32 revisionEventId )
        {
            String personName;

            context.CheckTransaction();
            personName = GetPersonName(context);
            personName = personName.CheckInjection();
            context.GetTaxonDatabase().SetTaxonTreeSortOrder(taxonIdParent, taxaIdChildList.ToList(), personName,
                                                             WebServiceData.UserManager.GetUser(context).Id,
                                                             revisionEventId);
        }

        /// <summary>
        /// Fetches all direct parentrelations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<WebTaxonRelation> GetParentTaxonRelationsByTaxon(WebServiceContext context, int id)
        {
            List<WebTaxonRelation> webTaxonRelations;

            // Get data from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetParentTaxonRelationsByTaxon(id, GetRevisionIdFromRole(context)))
            {
                webTaxonRelations = new List<WebTaxonRelation>();
                while (dataReader.Read())
                {
                    var webTaxonRelation = new WebTaxonRelation();
                    webTaxonRelation.LoadData(dataReader);
                    webTaxonRelations.Add(webTaxonRelation);
                }
            }

            return webTaxonRelations;
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<WebTaxonRelation> GetAllParentTaxonRelationsByTaxon(WebServiceContext context, int id)
        {
            List<WebTaxonRelation> webTaxonRelations;

            // Get data from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetAllParentTaxonRelationsByTaxon(id, GetRevisionIdFromRole(context)))
            {
                webTaxonRelations = new List<WebTaxonRelation>();
                while (dataReader.Read())
                {
                    var webTaxonRelation = new WebTaxonRelation();
                    webTaxonRelation.LoadData(dataReader);
                    webTaxonRelations.Add(webTaxonRelation);
                }
            }

            return webTaxonRelations;
        }

        /// <summary>
        /// Remove taxon relations that does not match search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonRelations">Taxon relations.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        private static void FilterTaxonRelations(WebServiceContext context,
                                                 List<WebTaxonRelation> taxonRelations,
                                                 WebTaxonRelationSearchCriteria searchCriteria)
        {
            Boolean isTaxonTreeRemoved, isValid;
            DateTime now;
            Dictionary<Int32, WebTaxonRelation> childTaxonRelations, parentTaxonRelations;
            Hashtable taxonTreeNodes;
            Int32 taxonRelationIndex, taxonTreeIndex;
            Int32? revisionId;
            List<TaxonTreeNode> taxonTrees;
            TaxonTreeNode tempTaxonTreeNode;
            WebTaxonRelation taxonRelation;

            if (taxonRelations.IsNotEmpty() &&
                searchCriteria.IsIsValidSpecified)
            {
                now = DateTime.Now;
                revisionId = GetRevisionIdFromRole(context);
                for (taxonRelationIndex = taxonRelations.Count - 1; taxonRelationIndex >= 0; taxonRelationIndex--)
                {
                    taxonRelation = taxonRelations[taxonRelationIndex];
                    if (revisionId.HasValue)
                    {
                        isValid = (now <= taxonRelation.ValidToDate) &&
                                   !taxonRelation.IsReplacedInTaxonRevisionEventIdSpecified;
                    }
                    else
                    {
                        isValid = now <= taxonRelation.ValidToDate;
                    }
                    if (isValid != searchCriteria.IsValid)
                    {
                        taxonRelations.RemoveAt(taxonRelationIndex);
                    }
                }
            }

            if (taxonRelations.IsNotEmpty() &&
                searchCriteria.IsIsMainRelationSpecified)
            {
                for (taxonRelationIndex = taxonRelations.Count - 1; taxonRelationIndex >= 0; taxonRelationIndex--)
                {
                    if (taxonRelations[taxonRelationIndex].IsMainRelation != searchCriteria.IsMainRelation)
                    {
                        taxonRelations.RemoveAt(taxonRelationIndex);
                    }
                }
            }

            if (taxonRelations.IsNotEmpty() &&
                searchCriteria.TaxonIds.IsNotEmpty() &&
                searchCriteria.Scope == TaxonRelationSearchScope.AllChildRelations)
            {
                // Remove taxon trees that is not related to specified
                // taxon ids anymore. This problem can occur when larger
                // parts of the taxon tree moves in the tree.
                isTaxonTreeRemoved = false;
                taxonTreeNodes = GetTaxonTrees(context, taxonRelations);
                taxonTrees = GetTaxonTreeRoots(taxonTreeNodes,
                                               false,
                                               false,
                                               searchCriteria.TaxonIds);
                for (taxonTreeIndex = taxonTrees.Count - 1; taxonTreeIndex >= 0; taxonTreeIndex--)
                {
                    if (!searchCriteria.TaxonIds.Contains(taxonTrees[taxonTreeIndex].TaxonId))
                    {
                        isTaxonTreeRemoved = true;
                        taxonTrees.RemoveAt(taxonTreeIndex);
                    }
                }

                if (isTaxonTreeRemoved)
                {
                    // Get all taxon relations for the remaining taxon trees.
                    childTaxonRelations = new Dictionary<Int32, WebTaxonRelation>();
                    foreach (TaxonTreeNode taxonTreeNode in taxonTrees)
                    {
                        GetChildTaxonRelations(childTaxonRelations, taxonTreeNode);
                    }

                    taxonRelations.Clear();
                    taxonRelations.AddRange(childTaxonRelations.Values);
                }
            }

            if (taxonRelations.IsNotEmpty() &&
                searchCriteria.TaxonIds.IsNotEmpty() &&
                searchCriteria.Scope == TaxonRelationSearchScope.AllParentRelations)
            {
                // Remove taxon trees that is not related to specified
                // taxon ids anymore. This problem can occur when larger
                // parts of the taxon tree moves in the tree.
                taxonTreeNodes = GetTaxonTrees(context, taxonRelations);

                // Get all taxon relations for requested taxa.
                parentTaxonRelations = new Dictionary<Int32, WebTaxonRelation>();
                foreach (Int32 taxonId in searchCriteria.TaxonIds)
                {
                    tempTaxonTreeNode = (TaxonTreeNode) (taxonTreeNodes[taxonId]);
                    GetParentTaxonRelations(parentTaxonRelations, tempTaxonTreeNode);
                }

                taxonRelations.Clear();
                taxonRelations.AddRange(parentTaxonRelations.Values);
            }
        }

        /// <summary>
        /// Fetches all childrelations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<WebTaxonRelation> GetAllChildTaxonRelationsByTaxon(WebServiceContext context, int id)
        {
            List<WebTaxonRelation> webTaxonRelations;

            // Get data from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetAllChildTaxonRelationsByTaxon(id, GetRevisionIdFromRole(context)))
            {
                webTaxonRelations = new List<WebTaxonRelation>();
                while (dataReader.Read())
                {
                    var webTaxonRelation = new WebTaxonRelation();
                    webTaxonRelation.LoadData(dataReader);
                    webTaxonRelations.Add(webTaxonRelation);
                }
            }

            return webTaxonRelations;
        }

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>revisionId
        /// <param name="context"></param>
        /// <param name="revisionId"></param>
        public static bool SetRevisionSpeciesFactPublished(WebServiceContext context, int revisionId)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            context.CheckTransaction();
            return context.GetTaxonDatabase().SetRevisionSpeciesFactPublished(revisionId);
        }

        /// <summary>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="taxonProperties">The taxom property object </param>
        /// <returns>
        /// </returns>
        public static WebTaxonProperties UpdateTaxonProperties(WebServiceContext context, WebTaxonProperties taxonProperties)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            context.CheckTransaction();
            taxonProperties.CheckData();
            var taxonPropertiesId = context.GetTaxonDatabase().UpdateTaxonProperties(
                taxonProperties.Id, taxonProperties.Taxon.Id, taxonProperties.TaxonCategory.Id,
                taxonProperties.PartOfConceptDefinition, taxonProperties.ConceptDefinition,
                taxonProperties.AlertStatusId, taxonProperties.ValidFromDate, taxonProperties.ValidToDate, taxonProperties.IsValid, taxonProperties.ModifiedBy.Id,
                taxonProperties.ChangedInTaxonRevisionEvent == null ? 0 : taxonProperties.ChangedInTaxonRevisionEvent.Id,
                taxonProperties.ReplacedInTaxonRevisionEvent == null ? 0 : taxonProperties.ReplacedInTaxonRevisionEvent.Id,
                taxonProperties.IsPublished, taxonProperties.DataFields.GetBoolean(TaxonPropertiesData.IS_MICROSPECIES), context.Locale.Id);
            
            return GetTaxonPropertiesById(context, taxonPropertiesId);
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="revision">
        /// The revision.
        /// </param>
        public static WebTaxonRevision UpdateRevision(WebServiceContext context, WebTaxonRevision revision)
        {
            WebTaxonRevision updatedRevision;

            // Check authority - AuthorityIdentifier.TaxonRevisionAdministration
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.TaxonRevisionAdministration);

            // Check data.
            context.CheckTransaction();
            revision.CheckData();

            // Update data.
            var revisionId = context.GetTaxonDatabase().UpdateRevision(revision.Id, revision.Description, revision.ExpectedStartDate, revision.ExpectedEndDate, revision.StateId, revision.CreatedBy, context.Locale.Id);
            updatedRevision = GetRevisionById(context, revisionId);
            ClearCacheForUserRoles(context);

            return updatedRevision;
        }

        /// <summary>
        /// Update description for a revision event
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        /// <returns>void</returns>
        public static void UpdateRevisionEvent(WebServiceContext context, int revisionEventId)
        {
            context.CheckTransaction();
            context.GetTaxonDatabase().UpdateRevisionEvent(revisionEventId, context.Locale.Id);
        }

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">Id for a taxon revision.</param>
        public static void DeleteRevision(WebServiceContext context,
                                          Int32 id)
        {
            WebTaxonRevision revision;

            // Check authority - AuthorityIdentifier.TaxonRevisionAdministration
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.TaxonRevisionAdministration);

            context.CheckTransaction();
            revision = GetRevisionById(context, id);
            if (revision.IsNotNull() &&
                revision.StateId == (Int32)(TaxonRevisionStateId.Ongoing))
            {
                UndoRevision(context, revision);
            }

            context.GetTaxonDatabase().DeleteRevision(id);

            if (revision.IsNotNull() &&
                revision.StateId == (Int32)(TaxonRevisionStateId.Ongoing))
            {
                ClearCachedTaxa(context);
            }
        }

        /// <summary>
        /// Delete specified taxon revision event.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonRevisionEventId">Id for the taxon revision event to be deleted.</param>
        public static void DeleteTaxonRevisionEvent(WebServiceContext context,
                                                    Int32 taxonRevisionEventId)
        {
            context.CheckTransaction();
            context.GetTaxonDatabase().UndoRevisionEvent(taxonRevisionEventId);
        }

        /// <summary>
        /// Creates a revision event.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="revisionEvent">Revision event object.</param>
        /// <returns>The created revision event object.</returns>
        public static WebTaxonRevisionEvent CreateRevisionEvent(WebServiceContext context, WebTaxonRevisionEvent revisionEvent)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            context.CheckTransaction();
            revisionEvent.CheckData();
            var revisionId = context.GetTaxonDatabase().CreateRevisionEvent(revisionEvent.RevisionId, revisionEvent.TypeId, revisionEvent.CreatedBy);

            return GetRevisionEventById(context, revisionId);
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context</param>
        /// <param name="revisionEventId">
        /// The revision event id.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebTaxonRevisionEvent GetRevisionEventById(WebServiceContext context, int revisionEventId)
        {
            WebTaxonRevisionEvent revisionEvent;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionEventById(revisionEventId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    revisionEvent = new WebTaxonRevisionEvent();
                    revisionEvent.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("RevisionEvent not found. RevisionId = " + revisionEventId);
                }
            }

            return revisionEvent;
        }

        /// <summary>
        /// Gets a revision event by its revisionId
        /// </summary>
        /// <param name="context">User context</param>
        /// <param name="revisionId">Revision id getting events for</param>
        /// <returns></returns>
        public static List<WebTaxonRevisionEvent> GetRevisionEventsByRevisionId(WebServiceContext context, int revisionId)
        {
            List<WebTaxonRevisionEvent> revisionEvents;
            WebTaxonRevisionEvent revisionEvent;


            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionEventsByRevisionId(revisionId, context.Locale.Id))
            {
                revisionEvents = new List<WebTaxonRevisionEvent>();
                while (dataReader.Read())
                {
                    revisionEvent = new WebTaxonRevisionEvent();
                    revisionEvent.LoadData(dataReader);
                    revisionEvents.Add(revisionEvent);
                }

            }

            return revisionEvents;
        }

        /// <summary>
        /// Load a revision based on identifier.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebTaxonRevision GetRevisionById(WebServiceContext context, int revisionId)
        {
            WebTaxonRevision revision;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionById(revisionId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    revision = new WebTaxonRevision();
                    revision.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Revision not found. RevisionId = " + revisionId);
                }
            }

            revision.RootTaxon = GetPublishedTaxonById(context, revision.RootTaxon.Id);

            return revision;
        }

        /// <summary>
        /// Get revision that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Revision search criteria.</param>
        /// <returns>List of web revisions.</returns>
        public static List<WebTaxonRevision> GetRevisionBySearchCriteria(WebServiceContext context,
                                                                    WebTaxonRevisionSearchCriteria searchCriteria)
        {
            List<WebTaxonRevision> revisions;
            WebTaxonRevision revision;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            //Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionsBySearchCriteria(searchCriteria.TaxonIds, searchCriteria.StateIds,
                                                                                                   context.Locale.Id))
            {
                revisions = new List<WebTaxonRevision>();
                while (dataReader.Read())
                {
                    revision = new WebTaxonRevision();
                    revision.LoadData(dataReader);
                    revisions.Add(revision);
                }
            }

            foreach (var webRevision in revisions)
            {
                webRevision.RootTaxon = GetPublishedTaxonById(context, webRevision.RootTaxon.Id);
            }

            return revisions;
        }

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>List of web revisions.</returns>
        public static List<WebTaxonRevision> GetRevisionsByTaxon(WebServiceContext context, int taxonId)
        {
            List<WebTaxonRevision> revisions;
            WebTaxonRevision revision;

            //Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionsByTaxon(taxonId, context.Locale.Id))
            {
                revisions = new List<WebTaxonRevision>();
                while (dataReader.Read())
                {
                    revision = new WebTaxonRevision();
                    revision.LoadData(dataReader);
                    revisions.Add(revision);
                }
            }

            foreach (var webRevision in revisions)
            {
                webRevision.RootTaxon = GetTaxonById(context, webRevision.RootTaxon.Id);
            }

            return revisions;
        }

        /// <summary>
        /// creates a new TaxonCategory
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonCategory">Object representing the taxon category.</param>
        /// <returns>WebTaxonCategory object with the created taxon category.</returns>
        public static WebTaxonCategory CreateTaxonCategory(WebServiceContext context, WebTaxonCategory taxonCategory)
        {
            // TODO: Check access rights. Needed or not??
            //AuthorizationManager.CheckSuperAdministrator(context);

            String cacheKey;
            Int32 taxonCategoryId;

            // Check arguments.
            context.CheckTransaction();
            taxonCategory.CheckNotNull("taxonCategory");
            taxonCategory.CheckData();
            taxonCategoryId = context.GetTaxonDatabase().CreateTaxonCategory(taxonCategory.Id, taxonCategory.Name, taxonCategory.IsMainCategory, taxonCategory.ParentId,
                                                                             taxonCategory.SortOrder, taxonCategory.IsTaxonomic, context.Locale.Id);
            // Release cached taxon categories.
            cacheKey = Settings.Default.TaxonCategoryCacheKey + ":" + context.Locale.ISOCode;
            context.RemoveCachedObject(cacheKey);

            return GetTaxonCategoryById(context, taxonCategoryId);
        }

        /// <summary>
        /// Get WebTaxonTreeNode from TaxonTreeNode.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxa">Cached taxa.</param>
        /// <param name="taxonTreeNode">Taxon tree node.</param>
        /// <returns>Web taxon tree information.</returns>
        private static WebTaxonTreeNode GetTaxonTree(WebServiceContext context,
                                                     Hashtable taxa,
                                                     TaxonTreeNode taxonTreeNode)
        {
            Int32 index;
            StringBuilder childrenTaxonIds;
            WebDataField dataField;
            WebTaxonTreeNode webTaxonTreeNode;

            webTaxonTreeNode = new WebTaxonTreeNode();
            webTaxonTreeNode.Taxon = (WebTaxon)(taxa[GetTaxonCacheKey(context, taxonTreeNode.TaxonId)]);
            if (taxonTreeNode.Children.IsNotEmpty())
            {
                for (index = 0; index < taxonTreeNode.Children.Count; index++)
                {
                    if (webTaxonTreeNode.Children.IsNull())
                    {
                        webTaxonTreeNode.Children = new List<WebTaxonTreeNode>();
                    }

                    webTaxonTreeNode.Children.Add(GetTaxonTree(context,
                                                                taxa,
                                                                taxonTreeNode.Children[index]));
                }
            }

            if (taxonTreeNode.ChildrenCircular.IsNotEmpty())
            {
                // Add circular children as dynamic data.
                childrenTaxonIds = new StringBuilder();
                foreach (TaxonTreeNode child in taxonTreeNode.ChildrenCircular)
                {
                    if (childrenTaxonIds.ToString().IsNotEmpty())
                    {
                        childrenTaxonIds.Append(", ");
                    }

                    childrenTaxonIds.Append(child.TaxonId);
                }

                if (webTaxonTreeNode.DataFields.IsNull())
                {
                    webTaxonTreeNode.DataFields = new List<WebDataField>();
                }

                dataField = new WebDataField();
                dataField.Name = Settings.Default.WebDataChildrenCircularTaxonIds;
                dataField.Type = WebDataType.String;
                dataField.Value = childrenTaxonIds.ToString();
                webTaxonTreeNode.DataFields.Add(dataField);
            }

            return webTaxonTreeNode;
        }

        /// <summary>
        /// Transform taxon relations information into taxon trees.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonRelations">Taxon relations used in taxon trees.</param>
        /// <returns>
        /// Taxon trees created with data from parameter taxonRelations.
        /// Each child and parent taxon id in parameter taxonRelations
        /// is a key in returned Hashtable and the value related to this
        /// key is a TaxonTreeNode instance associated with the taxon.
        /// </returns>
        private static Hashtable GetTaxonTrees(WebServiceContext context, 
                                               List<WebTaxonRelation> taxonRelations)
        {
            Hashtable taxonIdCache;
            Hashtable taxonTrees;
            List<Int32> taxonIds;
            List<WebTaxon> taxa;

            taxonTrees = new Hashtable();
            if (taxonRelations.IsNotEmpty())
            {
                // Get all unique taxon ids.
                taxonIdCache = new Hashtable();
                foreach (WebTaxonRelation taxonRelation in taxonRelations)
                {
                    if (!taxonIdCache.ContainsKey(taxonRelation.ChildTaxonId))
                    {
                        taxonIdCache[taxonRelation.ChildTaxonId] = taxonRelation.ChildTaxonId;
                    }
                    if (!taxonIdCache.ContainsKey(taxonRelation.ParentTaxonId))
                    {
                        taxonIdCache[taxonRelation.ParentTaxonId] = taxonRelation.ParentTaxonId;
                    }
                }
                taxonIds = new List<Int32>();
                foreach (Int32 taxonId in taxonIdCache.Values)
                {
                    taxonIds.Add(taxonId);
                }

                // Get taxa.
                taxa = GetTaxaByIds(context, taxonIds);

                // Get taxon trees.
                taxonTrees = GetTaxonTrees(taxa, taxonRelations);
            }

            return taxonTrees;
        }

        /// <summary>
        /// Transform taxa and taxon relations information into taxon trees.
        /// </summary>
        /// <param name="taxa">Taxa in taxon trees.</param>
        /// <param name="taxonRelations">Taxon relations used in taxon trees.</param>
        /// <returns>
        /// Taxon trees created with data from parameter taxa and 
        /// taxonRelations. Each taxon id in parameter taxa is a key
        /// in returned Hashtable and the value related to this key
        /// is a TaxonTreeNode instance associated with the taxon.
        /// </returns>
        private static Hashtable GetTaxonTrees(IEnumerable<WebTaxon> taxa,
                                               IEnumerable<WebTaxonRelation> taxonRelations)
        {
            Hashtable taxonRelationCache, taxonTrees;
            Int32 childTaxonId, parentTaxonId;
            TaxonTreeNode childTaxonTreeNode, parentTaxonTreeNode, taxonTreeNode;

            taxonTrees = new Hashtable();
            foreach (WebTaxon taxon in taxa)
            {
                taxonTreeNode = new TaxonTreeNode(taxon);
                taxonTrees[taxonTreeNode.TaxonId] = taxonTreeNode;
            }

            // Add information about taxon relations.
            taxonRelationCache = new Hashtable();
            foreach (WebTaxonRelation taxonRelation in taxonRelations)
            {
                if (!taxonRelationCache.ContainsKey(taxonRelation.Id))
                {
                    taxonRelationCache[taxonRelation.Id] = taxonRelation.Id;
                    childTaxonId = taxonRelation.ChildTaxonId;
                    childTaxonTreeNode = (TaxonTreeNode)(taxonTrees[childTaxonId]);
                    parentTaxonId = taxonRelation.ParentTaxonId;
                    parentTaxonTreeNode = (TaxonTreeNode)(taxonTrees[parentTaxonId]);
                    parentTaxonTreeNode.AddChild(childTaxonTreeNode, taxonRelation);
                    childTaxonTreeNode.AddParent(parentTaxonTreeNode, taxonRelation);
                }
                // else: Don't add same taxon relation more than once.
            }

            return taxonTrees;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public static List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebServiceContext context,
                                                                           WebTaxonTreeSearchCriteria searchCriteria)
        {
            Hashtable taxa, taxonTreeNodes;
            List<TaxonTreeNode> taxonTrees;
            List<WebTaxonRelation> taxonRelations;
            List<WebTaxonTreeNode> webTaxonTrees;
            WebTaxonRelationSearchCriteria taxonRelationSearchCriteria;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get taxon trees.
            if (IsUserInRevision(context) ||
                searchCriteria.TaxonIds.IsNotEmpty())
            {
                taxonRelationSearchCriteria = GetTaxonRelationSearchCriteria(searchCriteria);
                taxonRelations = GetTaxonRelationsBySearchCriteria(context, taxonRelationSearchCriteria);
                taxonTreeNodes = GetTaxonTrees(context, taxonRelations);
                taxonTrees = GetTaxonTreeRoots(taxonTreeNodes,
                                               searchCriteria.IsMainRelationRequired,
                                               searchCriteria.IsValidRequired,
                                               searchCriteria.TaxonIds);
            }
            else
            {
                taxonTrees = GetCachedTaxonTreeRoots(context,
                                                     searchCriteria);
            }

            // Get taxon instances.
            taxa = GetTaxa(context, taxonTrees);

            lock (taxonTrees)
            {
                CheckCircularTree(taxonTrees);
            }

            // Convert to web taxon tree nodes.
            webTaxonTrees = new List<WebTaxonTreeNode>();
            if (taxonTrees.IsNotEmpty())
            {
                foreach (TaxonTreeNode tempTaxonTreeNode in taxonTrees)
                {
                    webTaxonTrees.Add(GetTaxonTree(context,
                                                   taxa,
                                                   tempTaxonTreeNode));
                }
            }

            return webTaxonTrees;
        }

        /// <summary>
        /// Get taxon tree roots.
        /// That is all taxon tree nodes that has no parents.
        /// </summary>
        /// <param name="taxonTreeNodes">Taxon tree nodes.</param>
        /// <param name="isMainRelationRequired">Indicates if only main relations are considered or not.</param>
        /// <param name="isValidRequired">Indicates if only valid parent tree nodes are considered or not.</param>
        /// <param name="taxonIds">These taxon ids should be included in the returned taxon trees.</param>
        /// <returns>Taxon tree roots.</returns>
        private static List<TaxonTreeNode> GetTaxonTreeRoots(Hashtable taxonTreeNodes,
                                                             Boolean isMainRelationRequired,
                                                             Boolean isValidRequired,
                                                             List<Int32> taxonIds)
        {
            List<TaxonTreeNode> taxonTrees;
            DataIdInt32List taxonIdsInTree; 
                
            taxonTrees = new List<TaxonTreeNode>();
            foreach (TaxonTreeNode tempTaxonTreeNode in taxonTreeNodes.Values)
            {
                if ((!isValidRequired || tempTaxonTreeNode.IsValid) &&
                    tempTaxonTreeNode.IsTreeRoot(isMainRelationRequired, isValidRequired))
                {
                    taxonTrees.Add(tempTaxonTreeNode);
                }
            }

            if (taxonIds.IsNotEmpty())
            {
                taxonIdsInTree = new DataIdInt32List(true);
                foreach (TaxonTreeNode taxonTreeNode in taxonTrees)
                {
                    GetTaxonIdsInTree(taxonTreeNode, taxonIdsInTree);
                }

                foreach (int taxonId in taxonIds)
                {
                    if (!(taxonIdsInTree.Contains(taxonId)) &&
                        taxonTreeNodes.ContainsKey(taxonId))
                    {
                        TaxonTreeNode taxonTreeNode = (TaxonTreeNode)(taxonTreeNodes[taxonId]);
                        taxonTrees.Add(taxonTreeNode);
                        GetTaxonIdsInTree(taxonTreeNode, taxonIdsInTree);
                    }
                }
            }

            return taxonTrees;
        }

        /// <summary>
        /// creates a new TaxonNameCategory
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameCategory">Object representing the taxon name category.</param>
        /// <returns>WebTaxonNameCategory object with the created taxon name category.</returns>
        public static WebTaxonNameCategory CreateTaxonNameCategory(WebServiceContext context, WebTaxonNameCategory taxonNameCategory)
        {
            // TODO: Check access rights. Needed or not??
            //AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments.
            context.CheckTransaction();
            taxonNameCategory.CheckNotNull("taxonNameCategory");
            taxonNameCategory.CheckData();
            Int32 taxonNameCategoryId = context.GetTaxonDatabase().CreateTaxonNameCategory(taxonNameCategory.Name, taxonNameCategory.ShortName,
                                                                                            taxonNameCategory.SortOrder, context.Locale.Id, (Int32)(taxonNameCategory.TypeId));
            return GetTaxonNameCategoryById(context, taxonNameCategoryId);
        }

        /// <summary>
        /// Get taxa for all taxon tree nodes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTrees">Taxon trees.</param>
        /// <returns>Taxa for all taxon tree nodes.</returns>
        private static Hashtable GetTaxa(WebServiceContext context,
                                         List<TaxonTreeNode> taxonTrees)
        {
            Hashtable taxa, tempTaxonIds;
            List<Int32> taxonIds;

            if (IsUserInRevision(context) && taxonTrees.IsNotEmpty())
            {
                // Get all taxon ids.
                taxonIds = new List<Int32>();
                tempTaxonIds = new Hashtable();
                foreach (TaxonTreeNode taxonTreeNode in taxonTrees)
                {
                    if (!tempTaxonIds.ContainsKey(taxonTreeNode.TaxonId))
                    {
                        taxonIds.Add(taxonTreeNode.TaxonId);
                        tempTaxonIds[taxonTreeNode.TaxonId] = taxonTreeNode.TaxonId;
                        GetTaxonIds(taxonTreeNode.Children,
                                    taxonIds,
                                    tempTaxonIds);
                    }
                }

                // Get all taxa.
                taxa = new Hashtable();
                foreach (WebTaxon taxon in GetTaxaByIds(context, taxonIds))
                {
                    taxa[GetTaxonCacheKey(context, taxon.Id)] = taxon;
                }
            }
            else
            {
                taxa = GetCachedTaxa(context);
            }
            return taxa;
        }

        /// <summary>
        /// Get taxon ids for all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTrees">Taxon trees.</param>
        /// <param name="taxonIds">Accumulated taxon ids.</param>
        /// <param name="tempTaxonIds">Temporary taxon id cache.</param>
        private static void GetTaxonIds(List<TaxonTreeNode> taxonTrees,
                                        List<Int32> taxonIds,
                                        Hashtable tempTaxonIds)
        {
            if (taxonTrees.IsNotEmpty())
            {
                foreach (TaxonTreeNode taxonTreeNode in taxonTrees)
                {
                    if (!tempTaxonIds.ContainsKey(taxonTreeNode.TaxonId))
                    {
                        taxonIds.Add(taxonTreeNode.TaxonId);
                        tempTaxonIds[taxonTreeNode.TaxonId] = taxonTreeNode.TaxonId;
                        GetTaxonIds(taxonTreeNode.Children,
                                    taxonIds,
                                    tempTaxonIds);
                    }
                }
            }
        }

        /// <summary>
        /// Get taxon ids for all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Taxon tree.</param>
        /// <param name="taxonIdsInTree">Accumulated taxon ids.</param>
        private static void GetTaxonIdsInTree(TaxonTreeNode taxonTreeNode,
                                              DataIdInt32List taxonIdsInTree)
        {
            if (taxonTreeNode.IsNotNull() &&
                !taxonIdsInTree.Contains(taxonTreeNode.TaxonId))
            {
                taxonIdsInTree.Add(taxonTreeNode.TaxonId);
                if (taxonTreeNode.Children.IsNotEmpty())
                {
                    foreach (TaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                    {
                        GetTaxonIdsInTree(childTaxonTreeNode, taxonIdsInTree);
                    }
                }
            }
        }
                    
        /// <summary>
        /// Add taxon information to taxon names.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNames">Taxon names.</param>
        private static void GetTaxa(WebServiceContext context,
                                    List<WebTaxonName> taxonNames)
        {
            Hashtable cachedTaxa;
            List<Int32> taxonIds;
            List<WebTaxon> taxa;

            if (taxonNames.IsNotEmpty())
            {
                if (IsUserInRevision(context))
                {
                    // Get taxon ids.
                    taxonIds = new List<Int32>();
                    foreach (WebTaxonName taxonName in taxonNames)
                    {
                        if (!taxonIds.Contains(taxonName.Taxon.Id))
                        {
                            taxonIds.Add(taxonName.Taxon.Id);
                        }
                    }

                    // Get taxa.
                    taxa = GetTaxaByIds(context, taxonIds);
                    cachedTaxa = new Hashtable();
                    foreach (WebTaxon taxon in taxa)
                    {
                        cachedTaxa[taxon.Id] = taxon;
                    }

                    // Add taxon information to taxon names.
                    foreach (WebTaxonName taxonName in taxonNames)
                    {
                        taxonName.Taxon = (WebTaxon)(cachedTaxa[taxonName.Taxon.Id]);
                    }
                }
                else
                {
                    foreach (WebTaxonName taxonName in taxonNames)
                    {
                        taxonName.Taxon = GetCachedTaxon(context, taxonName.Taxon.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Add taxon information to taxon name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonName">Taxon name.</param>
        private static void GetTaxon(WebServiceContext context,
                                     WebTaxonName taxonName)
        {
            List<WebTaxonName> taxonNames;

            taxonNames = new List<WebTaxonName>();
            taxonNames.Add(taxonName);
            GetTaxa(context, taxonNames);
        }

        /// <summary>
        /// Get taxon cache key.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Taxon cache key.</returns>
        private static String GetTaxonCacheKey(WebServiceContext context,
                                               Int32 taxonId)
        {
            return "Taxon=" + taxonId + "#" + "Locale=" + context.Locale.ISOCode;
        }

        /// <summary>
        /// Get list of taxa, were taxa is defined by its id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIdList">List of taxon id values.</param>
        /// <returns>Returns list of taxa or null if 
        ///          taxonid doesn't match.</returns>
        public static List<WebTaxon> GetTaxaByIds(WebServiceContext context,
                                                  List<Int32> taxonIdList)
        {
            List<WebTaxon> taxa;
            WebTaxon taxon;

            taxa = new List<WebTaxon>();
            if (taxonIdList.IsNotEmpty())
            {
                if (IsUserInRevision(context))
                {
                    // Get taxa from database.
                    using (DataReader dataReader = context.GetTaxonDatabase().GetTaxaByIds(taxonIdList, GetRevisionIdFromRole(context), context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            taxon = new WebTaxon();
                            taxon.LoadData(dataReader);
                            SetTaxonChangeStatus(taxon);
                            taxa.Add(taxon);
                        }
                    }
                }
                else
                {
                    foreach (Int32 taxonId in taxonIdList)
                    {
                        taxa.Add(GetCachedTaxon(context, taxonId));
                    }
                }
            }
            return taxa;
        }

        /// <summary>
        /// Get taxa that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Taxa search criteria.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context,
                                                             WebTaxonSearchCriteria searchCriteria)
        {
            Boolean? isValidTaxon;
            List<WebTaxon> taxa;
            List<Int32> taxonIdsForArtFaktaDataSearch = new List<Int32>();
            Boolean isArtFaktaDataSearch = false;
            String taxonNameSearchString = null;
            WebTaxon taxon;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();
            if (searchCriteria.TaxonNameSearchString.IsNotNull())
            {
                searchCriteria.TaxonNameSearchString.SearchString = searchCriteria.TaxonNameSearchString.SearchString.CheckInjection();
            }

            isValidTaxon = null;
            if (searchCriteria.IsIsValidTaxonSpecified)
            {
                isValidTaxon = searchCriteria.IsValidTaxon;
            }

            taxonNameSearchString = null;
            if (searchCriteria.TaxonNameSearchString.IsNotNull())
            {
                taxonNameSearchString = searchCriteria.TaxonNameSearchString.SearchString;
            }

            // Make special handling if criterias for SwedishImmigrationHistory or SwedishOccurrence exists in the searchCritera 
            if (searchCriteria.SwedishImmigrationHistory.IsNotEmpty() || searchCriteria.SwedishOccurrence.IsNotEmpty())
            {
                isArtFaktaDataSearch = true;
                // searchCriteria.SearchScope = TaxaSearchScope.NoScope;
                searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            }

            //Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxaBySearchCriteria(searchCriteria.TaxonIds,
                                                                                                searchCriteria.TaxonCategoryIds,
                                                                                                searchCriteria.SwedishImmigrationHistory,
                                                                                                searchCriteria.SwedishOccurrence,
                                                                                                taxonNameSearchString,
                                                                                                isValidTaxon,
                                                                                                searchCriteria.Scope.ToString(),
                                                                                                GetRevisionIdFromRole(context), context.Locale.Id))
            {
                taxa = new List<WebTaxon>();
                while (dataReader.Read())
                {
                    taxon = new WebTaxon();
                    taxon.LoadData(dataReader);
                    // make new list of taxon id:s for another call to GetTaxaBySearchCriteria
                    if (isArtFaktaDataSearch)
                    {
                        taxonIdsForArtFaktaDataSearch.Add(taxon.Id);
                    }
                    else
                    {
                        SetTaxonChangeStatus(taxon);
                        taxa.Add(taxon);
                    }
                }
            }

            // SwedishImmigrationHistory or SwedishOccurrence exists in the searchCritera - make another call to GetTaxaBySearchCriteria
            // with taxonid list received in the first call.
            if (isArtFaktaDataSearch && taxonIdsForArtFaktaDataSearch.IsNotEmpty())
            {
                searchCriteria.TaxonIds = taxonIdsForArtFaktaDataSearch;
                const string searchScopeForArtFaktaDataSearch = "AllParentTaxa";
                //Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxaBySearchCriteria(searchCriteria.TaxonIds, searchCriteria.TaxonCategoryIds,
                                                                                                 null, null, null, isValidTaxon,
                                                                                                 searchScopeForArtFaktaDataSearch, GetRevisionIdFromRole(context), context.Locale.Id))
                {
                    taxa = new List<WebTaxon>();
                    while (dataReader.Read())
                    {
                        taxon = new WebTaxon();
                        taxon.LoadData(dataReader);
                        SetTaxonChangeStatus(taxon);
                        taxa.Add(taxon);
                    }
                }
            }  // end if (isArtFaktaDataSearch)
            
            // GetTaxonNamesFromTaxa(context, taxa);

            //Remove possible duplicates because certain taxa generate duplicates when in a revision (probably a bug in stored proc).
            taxa = taxa.Distinct(new WebTaxonEqualityComparer()).ToList();

            return taxa;
        }

        /// <summary>
        /// Get id values for taxa that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Taxa search criteria.</param>
        /// <returns>Taxon names.</returns>
        public static List<Int32> GetTaxaIdsBySearchCriteria(WebServiceContext context,
                                                             WebTaxonSearchCriteria searchCriteria)
        {
            List<Int32> taxaIds = null;
            List<WebTaxon> taxa = GetTaxaBySearchCriteria(context, searchCriteria);

            if (taxa.IsNotEmpty())
            {
                taxaIds = new List<Int32>();
                foreach (WebTaxon taxon in taxa)
                {
                    taxaIds.Add(taxon.Id);
                }
            }
            return taxaIds;
        }

        /// <summary>
        /// Get taxa that are possible parents for input taxonId.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxon> GetTaxaPossibleParents(WebServiceContext context, Int32 taxonId)
        {
            List<WebTaxon> taxa;
            WebTaxon taxon;

            //Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxaPossibleParents(taxonId, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                taxa = new List<WebTaxon>();
                while (dataReader.Read())
                {
                    taxon = new WebTaxon();
                    taxon.LoadData(dataReader);
                    SetTaxonChangeStatus(taxon);
                    taxa.Add(taxon);
                }
            }

            // GetTaxonNamesFromTaxa(context, taxa);
            return taxa;
        }

        /// <summary>
        /// Get information about a taxon
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Returns taxon information or null if taxonid doesn't match.</returns>
        public static WebTaxon GetTaxonById(WebServiceContext context, Int32 taxonId)
        {
            WebTaxon taxon;

            if (IsUserInRevision(context))
            {
                // Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonById(taxonId,
                                                                                       GetRevisionIdFromRole(context),
                                                                                       context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        taxon = new WebTaxon();
                        taxon.LoadData(dataReader);
                        SetTaxonChangeStatus(taxon);
                    }
                    else
                    {
                        throw new ArgumentException("Taxon not found. TaxonId = " + taxonId);
                    }
                }
            }
            else
            {
                taxon = GetCachedTaxon(context, taxonId);
            }
            return taxon;
        }

        /// <summary>
        /// Get information about a taxon by GUID
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="GUID">Taxons GUID.</param>
        /// <returns>Returns taxon information or null if taxonid doesn't match.</returns>
        public static WebTaxon GetTaxonByGUID(WebServiceContext context, String GUID)
        {
            WebTaxon taxon;

            // Get information from database.
            GUID = GUID.CheckInjection();
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonByGUID(GUID, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxon = new WebTaxon();
                    taxon.LoadData(dataReader);
                    SetTaxonChangeStatus(taxon);
                }
                else
                {
                    throw new ArgumentException("Taxon not found. GUID = " + GUID);
                }
            }

            //SetTaxonNamesInTaxon(context, taxon);
            return taxon;
        }

        /// <summary>
        /// Get a taxonname by GUID
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="GUID">Taxons GUID.</param>
        /// <returns>
        /// </returns>    
        public static WebTaxonName GetTaxonNameByGUID(WebServiceContext context,
                                                      String GUID)
        {
            WebTaxonName taxonName;

            // Get information from database.
            GUID.CheckNotEmpty("GUID");
            GUID = GUID.CheckInjection();
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNameByGuid(GUID, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxonName = new WebTaxonName();
                    taxonName.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("TaxonName not found. GUID = " + GUID);
                }
            }

            GetTaxon(context, taxonName);
            return taxonName;
        }

        /// <summary>
        /// Get a lumpsplitevent by GUID
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="GUID">Taxons GUID.</param>
        /// <returns>
        /// </returns>    
        public static WebLumpSplitEvent GetLumpSplitEventByGUID(WebServiceContext context, String GUID)
        {
            WebLumpSplitEvent lumpSplitEvent;

            // Get information from database.
            GUID.CheckNotEmpty("GUID");
            GUID = GUID.CheckInjection();
            using (DataReader dataReader = context.GetTaxonDatabase().GetLumpSplitEventByGUID(GUID, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    lumpSplitEvent = new WebLumpSplitEvent();
                    lumpSplitEvent.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("LumpSplitEvent not found. GUID = " + GUID);
                }
            }

            return lumpSplitEvent;
        }

        /// <summary>
        /// Get a revision by GUID
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="GUID">Taxons GUID.</param>
        /// <returns>
        /// WebTaxon with information about a Taxon
        /// or NULL if the Taxon is not found.
        /// </returns>    
        public static WebTaxonRevision GetRevisionByGUID(WebServiceContext context, String GUID)
        {
            WebTaxonRevision revision;

            // Get information from database.
            GUID.CheckNotEmpty("GUID");
            GUID = GUID.CheckInjection();
            using (DataReader dataReader = context.GetTaxonDatabase().GetRevisionByGUID(GUID, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    revision = new WebTaxonRevision();
                    revision.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Revision not found. GUID = " + GUID);
                }
            }

            revision.RootTaxon = GetPublishedTaxonById(context, revision.RootTaxon.Id);

            return revision;
        }

        /// <summary>
        /// Get information about a taxon
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Returns taxon information or null if taxonid doesn't match.</returns>
        private static WebTaxon GetPublishedTaxonById(WebServiceContext context, Int32 taxonId)
        {
            WebTaxon taxon;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonById(taxonId, null, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxon = new WebTaxon();
                    taxon.LoadData(dataReader);
                    SetTaxonChangeStatus(taxon);
                }
                else
                {
                    throw new ArgumentException("Taxon not found. TaxonId = " + taxonId);
                }
            }

            // SetTaxonNamesInTaxon(context, taxon);
            return taxon;
        }

        /// <summary>
        /// Set property ChangeStatus in WebTaxon object.
        /// Set to -1 if taxon is not lumped or split and taxon is not valid.
        /// </summary>
        /// <param name="taxon">WebTaxon object.</param>
        /// <returns></returns>
        private static void SetTaxonChangeStatus (WebTaxon taxon)
        {
            if (taxon.ChangeStatusId == 0 && !taxon.IsValid)
            {
                taxon.ChangeStatusId = (Int32)(TaxonChangeStatusId.InvalidDueToDelete);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebLumpSplitEvent GetLumpSplitEventById(WebServiceContext context, int id)
        {
            WebLumpSplitEvent lumpSplitEvent;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetLumpSplitEventById(id, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    lumpSplitEvent = new WebLumpSplitEvent();
                    lumpSplitEvent.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("LumpSplitEvent not found. Id = " + id);
                }
            }

            return lumpSplitEvent;
        }

/*        /// <summary>
        /// Set taxon name information in the taxon object
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxon">The WebTaxon object in which to add names.</param>
        /// <returns></returns>
        
        private static void SetTaxonNamesInTaxon(WebServiceContext context, WebTaxon taxon)
        {
            taxon.TaxonNames = GetTaxonNamesByTaxonId(context, taxon.Id);
            // set to RecommendedGUID to null
            taxon.RecommendedGUID = null;
            foreach (var taxonName in taxon.TaxonNames)
            {
                if (taxonName.ValidToDate > DateTime.Now)
                {
                    // nameUsage = Godkänd namngivning
                    if (taxonName.StatusId == (Int32)(TaxonNameStatusId.ApprovedNaming))
                    {
                        // type = SCIENTIFIC
                        if (taxonName.NameCategory.Type.Equals(1))
                        {
                            if (taxonName.CategoryId.Equals(0) && taxonName.IsRecommended)
                            {
                                taxon.RecommendedScientificName = taxonName;
                            }
                            else
                            {
                                taxon.Synonyms.Add(taxonName);
                            }
                        }
                        // type = IDENTIFIER
                        else if (taxonName.NameCategory.Type.Equals(3))
                        {
                            if (taxonName.IsRecommended && taxonName.CategoryId.Equals(16))
                            {
                                taxon.RecommendedGUID = taxonName;
                            }
                            else
                            {
                                taxon.Identifiers.Add(taxonName);
                            }
                        }
                        // type = COMMON_NAMES                    
                        else if (taxonName.NameCategory.LocaleId.Equals(context.Locale.Id))
                        {
                            if (taxonName.IsRecommended)
                            {
                                taxon.RecommendedCommonName = taxonName;
                            }
                            else
                            {
                                taxon.OtherNames.Add(taxonName);
                            }
                        }
                        else
                        {
                            taxon.OtherNames.Add(taxonName);
                        }
                    }
                    // nameUsage = NOT godkänd namngivning
                    else
                    {
                        taxon.OtherNames.Add(taxonName);
                    }
                }
            }
            if (taxon.RecommendedGUID.IsNull())
            {
                // TODO Fyll på med ett komplett WebTaxonName-objekt
                taxon.RecommendedGUID = new WebTaxonName();
                taxon.RecommendedGUID.Name = taxon.GUID;
            }
        }
         */

        /// <summary>
        /// Get information about a TaxonName
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameId">TaxonName id.</param>
        /// <returns>Returns taxonname information or null if taxonNameId doesn't match.</returns>
        public static WebTaxonName GetTaxonNameById(WebServiceContext context, Int32 taxonNameId)
        {
            WebTaxonName taxonName;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNameById(taxonNameId,
                                                                                       GetRevisionIdFromRole(context),
                                                                                       context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxonName = new WebTaxonName();
                    taxonName.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("TaxonName not found. TaxonNameId = " + taxonNameId);
                }
            }

            GetTaxon(context, taxonName);
            return taxonName;
        }

        /// <summary>
        /// Get taxon name by version.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameVersion">Taxon name object.</param>
        /// <returns>Taxon name with specified version.</returns>
        private static WebTaxonName GetTaxonNameByVersion(WebServiceContext context,
                                                          Int32 taxonNameVersion)
        {
            String guid;

            guid = "urn:lsid:dyntaxa.se:TaxonName:-1:" + taxonNameVersion;
            return GetTaxonNameByGUID(context, guid);
        }

        /*
        /// <summary>
        /// Get taxon namelist from taxon id
        /// </summary>
        /// <param name="context">service context</param>
        /// <param name="taxa">a list of taxon.</param>
        private static void GetTaxonNamesFromTaxa(WebServiceContext context, List<WebTaxon> taxa)
        {
            foreach (WebTaxon webTaxon in taxa)
            {
                SetTaxonNamesInTaxon(context, webTaxon);
                //List<WebTaxonName> taxonNames = GetTaxonNamesByTaxonId(context, webTaxon.Id);
                //webTaxon.TaxonNames = taxonNames;
            }
        }
        */

        /// <summary>
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="taxonPropertiesId">
        /// The taxon properties id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private static WebTaxonProperties GetTaxonPropertiesById(WebServiceContext context, int taxonPropertiesId)
        {
            var taxonProperties = new WebTaxonProperties();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonPropertiesById(taxonPropertiesId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxonProperties = new WebTaxonProperties();
                    taxonProperties.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Taxonproperties not found. TaxonPropertiesId = " + taxonProperties);
                }

            }
            if (taxonProperties.IsNotNull())
            {
                // Get TaxonCategory for this TaxonProperty
                taxonProperties.TaxonCategory = GetTaxonCategoryById(context, taxonProperties.TaxonCategory.Id);
             
            }
            return taxonProperties; 
        }

        /// <summary>
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static List<WebTaxonProperties> GetTaxonPropertiesByTaxonId(WebServiceContext context, int taxonId)
        {
            var taxonPropertiesList = new List<WebTaxonProperties>();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonPropertiesByTaxonId(taxonId, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    var taxonProperties = new WebTaxonProperties();
                    taxonProperties.LoadData(dataReader);
                    taxonPropertiesList.Add(taxonProperties);
                }
            }
            if (taxonPropertiesList.IsNotEmpty())
            {
                foreach (WebTaxonProperties taxonProperty in taxonPropertiesList)
                {
                    // Get TaxonCategory for this TaxonProperty
                    taxonProperty.TaxonCategory = GetTaxonCategoryById(context, taxonProperty.TaxonCategory.Id);
                }
            }
            return taxonPropertiesList;
        }

        /// <summary>
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelationId">Id of reference relation record.</param>
        /// <returns>Returns reference relation or null if id doesn't match.</returns>
        public static WebReferenceRelation GetReferenceRelationById(WebServiceContext context,
                                                                    Int32 referenceRelationId)
        {
            WebServiceData.LogManager.Log(context, "Obsolete method GetReferenceRelationById() used.", LogType.Information, null);
            return WebServiceData.ReferenceManager.GetReferenceRelationById(context, referenceRelationId);
        }

        /// <summary>
        /// Get information about a taxon
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="relatedObjectGuid">The objects GUID.</param>
        /// <returns>Returns taxon information or null if taxonid doesn't match.</returns>
        public static List<WebReferenceRelation> GetReferenceRelationsByGuid(WebServiceContext context,
                                                                             String relatedObjectGuid)
        {
            WebServiceData.LogManager.Log(context, "Obsolete method GetReferenceRelationsByRelatedObjectGuid() used.", LogType.Information, null);
            return WebServiceData.ReferenceManager.GetReferenceRelationsByRelatedObjectGuid(context,
                                                                                            relatedObjectGuid);
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All reference relation types.</returns>
        public static List<WebReferenceRelationType> GetReferenceRelationTypes(WebServiceContext context)
        {
            WebServiceData.LogManager.Log(context, "Obsolete method GetReferenceRelationTypes() used.", LogType.Information, null);
            return WebServiceData.ReferenceManager.GetReferenceRelationTypes(context);
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<WebLumpSplitEvent> GetLumpSplitEventsByTaxon(WebServiceContext context, int id)
        {
            List<WebLumpSplitEvent> lumpSplitEvents = new List<WebLumpSplitEvent>();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetLumpSplitEventsByTaxon(id, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    var lumpSplitEvent = new WebLumpSplitEvent();
                    lumpSplitEvent.LoadData(dataReader);
                    lumpSplitEvents.Add(lumpSplitEvent);
                }
            }

            return lumpSplitEvents;
        }

        /// <summary>
        /// Get list of taxonIds that has been replaced in a lump.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">TaxonId of the taxon that has replaced other taxa one or several lump(s).</param>
        /// <returns>List of taxonIds that has been replaced.</returns>
        private static List<Int32> GetTaxonIdsReplacedInLump(WebServiceContext context, Int32 taxonId)
        {
            List<WebLumpSplitEvent> lumpSplitEvents;
            List<Int32> taxonIdsReplacedInLump = new List<Int32>();
            lumpSplitEvents = GetLumpSplitEventsByTaxon(context, taxonId);
            if (lumpSplitEvents.IsNotEmpty())
            {
                foreach (WebLumpSplitEvent lumpSplitEvent in lumpSplitEvents)
                {
                    taxonIdsReplacedInLump.Add(lumpSplitEvent.TaxonIdBefore);
                }
            }
            return taxonIdsReplacedInLump;
        }

        /// <summary>
        /// Get information about lumpsplit events for a specific taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">TaxonId.</param>
        /// <returns>List of WebLumpSplitEvent for selected taxon.</returns>
        public static List<WebLumpSplitEvent> GetLumpSplitEventsByReplacedTaxon(WebServiceContext context, int id)
        {
            List<WebLumpSplitEvent> lumpSplitEvents = new List<WebLumpSplitEvent>();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetLumpSplitEventsByReplacedTaxon(id, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    var lumpSplitEvent = new WebLumpSplitEvent();
                    lumpSplitEvent.LoadData(dataReader);
                    lumpSplitEvents.Add(lumpSplitEvent);
                }
            }

            return lumpSplitEvents;
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon categories.</returns>
        public static List<WebTaxonCategory> GetTaxonCategories(WebServiceContext context)
        {
            List<WebTaxonCategory> taxonCategories;
            String cacheKey;
            WebTaxonCategory taxonCategory;

            // Get cached information.
            cacheKey = Settings.Default.TaxonCategoryCacheKey + ":" + context.Locale.ISOCode;
            taxonCategories = (List<WebTaxonCategory>)(context.GetCachedObject(cacheKey));

            if (taxonCategories.IsEmpty())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonCategories(context.Locale.Id))
                {
                    taxonCategories = new List<WebTaxonCategory>();
                    while (dataReader.Read())
                    {
                        taxonCategory = new WebTaxonCategory();
                        taxonCategory.LoadData(dataReader);
                        taxonCategories.Add(taxonCategory);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonCategories,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonCategories;
        }

        /// <summary>
        /// Get taxon category with specific id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonCategoryId">Id of taxon category.</param>
        /// <returns>Taxon category with specific id.</returns>
        public static WebTaxonCategory GetTaxonCategoryById(WebServiceContext context,
                                                            Int32 taxonCategoryId)
        {
            foreach (WebTaxonCategory tempTaxonCategory in GetTaxonCategories(context))
            {
                if (tempTaxonCategory.Id.Equals(taxonCategoryId))
                {
                    return tempTaxonCategory;
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebTaxonCategory GetTaxonCategoryByTaxonPropertiesId(WebServiceContext context, int id)
        {
            // Get cached WebTaxonCategory object.
            WebTaxonCategory taxonCategory;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonCategoryByTaxonPropertiesId(id, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxonCategory = new WebTaxonCategory();
                    taxonCategory.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Taxon category not found. TaxonPropertiesId = " + id);
                }
            }

            return taxonCategory;
        }

        /// <summary>
        /// Get all taxon categories related to specified taxon.
        /// This includes: 
        /// Taxon categories for parent taxa to specified taxon.
        /// Taxon category for specified taxon.
        /// Taxon categories for child taxa to specified taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>All taxon categories related to specified taxon.</returns>
        public static List<WebTaxonCategory> GetTaxonCategoriesByTaxonId(WebServiceContext context,
                                                                         Int32 taxonId)
        {
            Boolean isUserInRevision;

            isUserInRevision = IsUserInRevision(context);
            return GetTaxonCategoriesForTaxonInTree(context, taxonId, taxonId, !isUserInRevision);
        }

        /// <summary>
        ///  Gets the Categories for a taxon in tree
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parentTaxonId">Id for parent taxon in tree for selected taxon.</param>
        /// <param name="taxonId">Id for the selected taxon.</param>
        /// <param name="isPublished">If published taxon categories are to be included or not.</param>
        /// <returns>A list of categories sorted in tree</returns>
        public static List<WebTaxonCategory> GetTaxonCategoriesForTaxonInTree(WebServiceContext context, int parentTaxonId, int taxonId,  bool isPublished)
        {
            List<WebTaxonCategory> taxonCategoryList;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonCategoriesForTaxonInTree(parentTaxonId, taxonId, context.Locale.Id, isPublished))
            {
                taxonCategoryList = new List<WebTaxonCategory>();
                while (dataReader.Read())
                {
                    var taxonCategory = new WebTaxonCategory();
                    taxonCategory.LoadData(dataReader);
                    // Since the readers data are sorted we will
                    // keep the sort order by using add on the list since
                    // add will perfome ie add last... 
                    taxonCategoryList.Add(taxonCategory);
                }
            }
            return taxonCategoryList;
        }

        /// <summary>
        /// Get list of changes made in taxon tree.
        /// Current version return changes regarding:
        /// - new taxon
        /// - new/edited taxon name (scientific + common)
        /// - lump/split events
        /// - taxon category changes
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="rootTaxonId">A root taxon id. Changes made for child taxa to this root taxon are returned.
        /// If parameter is NULL all changes are returned</param>
        /// <param name="isRootTaxonIdSpecified">Boolean true if rootTaxonId has a value, otherwise false.</param>
        /// <param name="dateFrom">Return changes from and including this date.</param>
        /// <param name="dateTo">Return changes to and including this date.</param>
        /// <returns>List of changes made</returns>
        public static List<WebTaxonChange> GetTaxonChange(WebServiceContext context, Int32 rootTaxonId, Boolean isRootTaxonIdSpecified, DateTime dateFrom, DateTime dateTo)
        {
            List<WebTaxonChange> taxonChangesList = new List<WebTaxonChange>();
            Int32? nullableRootTaxonId = null;

            if (isRootTaxonIdSpecified)
            {
                nullableRootTaxonId = rootTaxonId;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonChange(nullableRootTaxonId, dateFrom, dateTo))
            {
                while (dataReader.Read())
                {
                    WebTaxonChange taxonChange = new WebTaxonChange();
                    taxonChange.LoadData(dataReader);
                    taxonChangesList.Add(taxonChange);
                }
            }
            return taxonChangesList;
        }

        /// <summary>
        /// Get taxon name category for specific id
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameCategoryId">Id to get taxon name category for.</param>
        /// <returns></returns>
        private static WebTaxonNameCategory GetTaxonNameCategoryById(WebServiceContext context, int taxonNameCategoryId)
        {
            List<WebTaxonNameCategory> taxonNameCategories;
            WebTaxonNameCategory taxonNameCategory = null;

            taxonNameCategories = GetTaxonNameCategories(context);
            foreach (WebTaxonNameCategory tempTaxonNameCategory in taxonNameCategories)
            {
                if (tempTaxonNameCategory.Id == taxonNameCategoryId)
                {
                    taxonNameCategory = tempTaxonNameCategory;
                    break;
                }
            }
            
            return taxonNameCategory;
        }

        /// <summary>
        /// Get a TaxonNameCategory object from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameCategoryId">Id value of taxonNameCategory.</param>
        /// <returns>WebTaxonNameCategory object from cache.</returns>
        private static WebTaxonNameCategory GetTaxonNameCategory(WebServiceContext context, int taxonNameCategoryId)
        {
            WebTaxonNameCategory taxonNameCategory;
            // Get cached WebTaxonNameCategory object.
            taxonNameCategory = (WebTaxonNameCategory)context.GetCachedObject(GetIdCacheKey(Settings.Default.TaxonNameCategoryCacheKey, context.Locale.Id.ToString(), taxonNameCategoryId));

            // Data not in cache - store it in the cache.
            if (taxonNameCategory.IsNull())
            {
                List<WebTaxonNameCategory> taxonNameCategoryList = GetTaxonNameCategories(context);
                if (taxonNameCategoryList.IsNotEmpty())
                {
                    foreach (WebTaxonNameCategory category in taxonNameCategoryList)
                    {
                        // Add object to cache.
                        context.AddCachedObject(GetIdCacheKey(Settings.Default.TaxonNameCategoryCacheKey, context.Locale.Id.ToString(), category.Id),
                                                category,
                                                DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                                CacheItemPriority.High);
                        // set taxonNameCategory object
                        if (category.Id.Equals(taxonNameCategoryId))
                        {
                            taxonNameCategory = category;
                        }
                    }
                }
            }
            return taxonNameCategory;
        }

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public static List<WebTaxonNameStatus> GetTaxonNameStatus(WebServiceContext context)
        {
            List<WebTaxonNameStatus> taxonNameStatusList;
            String cacheKey;
            WebTaxonNameStatus taxonNameStatus;

            // Get cached information.
            cacheKey = Settings.Default.TaxonNameStatusCacheKey + context.Locale.ISOCode;
            taxonNameStatusList = (List<WebTaxonNameStatus>)(context.GetCachedObject(cacheKey));

            // Data not in cache - store it in the cache.
            if (taxonNameStatusList.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNameUsages(context.Locale.Id))
                {
                    taxonNameStatusList = new List<WebTaxonNameStatus>();
                    while (dataReader.Read())
                    {
                        taxonNameStatus = new WebTaxonNameStatus();
                        taxonNameStatus.LoadData(dataReader);
                        taxonNameStatusList.Add(taxonNameStatus);
                    }
                }

                // Add object to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameStatusList,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }
            return taxonNameStatusList;
        }

        /// <summary>
        /// Get information about possible usage for taxon names.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about possible usage for taxon names.</returns>
        public static List<WebTaxonNameUsage> GetTaxonNameUsage(WebServiceContext context)
        {
            List<WebTaxonNameUsage> taxonNameUsageList;
            String cacheKey;
            WebTaxonNameUsage taxonNameUsage;

            // Get cached information.
            cacheKey = Settings.Default.TaxonNameUsageCacheKey + context.Locale.ISOCode;
            taxonNameUsageList = (List<WebTaxonNameUsage>)(context.GetCachedObject(cacheKey));

            // Data not in cache - store it in the cache.
            if (taxonNameUsageList.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNameUsagesNew(context.Locale.Id))
                {
                    taxonNameUsageList = new List<WebTaxonNameUsage>();
                    while (dataReader.Read())
                    {
                        taxonNameUsage = new WebTaxonNameUsage();
                        taxonNameUsage.LoadData(dataReader);
                        taxonNameUsageList.Add(taxonNameUsage);
                    }
                }

                // Add object to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameUsageList,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }
            return taxonNameUsageList;
        }

        /// <summary>
        /// Get key used when handling a cached object.
        /// </summary>
        /// <param name="objectPrefix">String representing object type.</param>
        /// <param name="locale">String representing the language.</param>
        /// <param name="id">Id value.</param>
        /// <returns>The unique cache key.</returns>       
        private static String GetIdCacheKey(String objectPrefix, String locale, Int32 id)
        {
            return objectPrefix + ":" + locale + ":" +id;
        }

        /// <summary>
        /// Get information about all taxon name categories
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns list of taxon name categories objects or null if 
        /// no taxon name category exists.
        /// </returns>
        public static List<WebTaxonNameCategory> GetTaxonNameCategories(WebServiceContext context)
        {
            List<WebTaxonNameCategory> taxonNameCategoryList;
            WebTaxonNameCategory taxonNameCategory;
            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNameCategories(context.Locale.Id))
            {
                taxonNameCategoryList = new List<WebTaxonNameCategory>();
                while (dataReader.Read())
                {
                    taxonNameCategory = new WebTaxonNameCategory();
                    taxonNameCategory.LoadData(dataReader);
                    taxonNameCategoryList.Add(taxonNameCategory);
                }
            }
            return taxonNameCategoryList;
        }

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebServiceContext context,
                                                                       WebTaxonNameSearchCriteria searchCriteria)
        {
            Int32 movedTaxonNameCount, taxonNameIndex;
            List<WebTaxonName> taxonNames;
            Boolean? isRecommended, isOriginal, isOkForObsSystems, isUnique, isValidTaxon, isValidTaxonName;
            Boolean isAuthorIncludedInNameSearchString;
            Int32? nameUsageId, nameCategoryId;
            String nameSearchString = null;
            String authorSearchString = null;
            DateTime? modifiedDateStart, modifiedDateEnd;
            StringCompareOperator stringCompareOperator;
            List<Int32> taxonIdList = null;
            WebTaxonName webTaxonName;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get search criteria values.
            stringCompareOperator = StringCompareOperator.Contains;
            if (searchCriteria.NameSearchString.IsNotNull() &&
                searchCriteria.NameSearchString.SearchString.IsNotEmpty())
            {
                nameSearchString = searchCriteria.NameSearchString.SearchString.Trim();
                if (searchCriteria.NameSearchString.CompareOperators.IsNotEmpty())
                {
                    stringCompareOperator = searchCriteria.NameSearchString.CompareOperators[0];
                }
            }

            if (searchCriteria.AuthorSearchString.IsNotNull() &&
                searchCriteria.AuthorSearchString.SearchString.IsNotEmpty())
            {
                authorSearchString = searchCriteria.AuthorSearchString.SearchString.Trim();
            }

            if (searchCriteria.TaxonIds.IsNotEmpty())
            {
                taxonIdList = searchCriteria.TaxonIds;
            }

            isRecommended = null;
            if (searchCriteria.IsIsRecommendedSpecified)
            {
                isRecommended = searchCriteria.IsRecommended;
            }

            isOriginal = null;
            if (searchCriteria.IsIsOriginalNameSpecified)
            {
                isOriginal = searchCriteria.IsOriginalName;
            }

            isOkForObsSystems = null;
            if (searchCriteria.IsIsOkForSpeciesObservationSpecified)
            {
                isOkForObsSystems = searchCriteria.IsOkForSpeciesObservation;
            }

            isUnique = null;
            if (searchCriteria.IsIsUniqueSpecified)
            {
                isUnique = searchCriteria.IsUnique;
            }

            nameUsageId = null;
            if (searchCriteria.IsStatusIdSpecified)
            {
                nameUsageId = searchCriteria.StatusId;
            }

            nameCategoryId = null;
            if (searchCriteria.IsCategoryIdSpecified)
            {
                nameCategoryId = searchCriteria.CategoryId;
            }

            isValidTaxon = null;
            if (searchCriteria.IsIsValidTaxonSpecified)
            {
                isValidTaxon = searchCriteria.IsValidTaxon;
            }

            isValidTaxonName = null;
            if (searchCriteria.IsIsValidTaxonNameSpecified)
            {
                isValidTaxonName = searchCriteria.IsValidTaxonName;
            }

            isAuthorIncludedInNameSearchString = false;
            if (searchCriteria.IsAuthorIncludedInNameSearchString)
            {
                isAuthorIncludedInNameSearchString = true;
                // Remove multiple spaces
                nameSearchString = RemoveMultipleSpaces(nameSearchString);
            }

            modifiedDateStart = null;
            modifiedDateEnd = null;
            // modifiedDateStart and modifiedDateEnd are sent as DataFields
            if (searchCriteria.DataFields.IsNotNull())
            {
                foreach (WebDataField dataField in searchCriteria.DataFields)
                {
                    if (dataField.Name.Equals("ModifiedDateStart"))
                    {
                        modifiedDateStart = dataField.GetDateTime();
                    }
                    else if (dataField.Name.Equals("ModifiedDateEnd"))
                    {
                        modifiedDateEnd = dataField.GetDateTime();
                    }
                }
            }

            // Get information from database.
            taxonNames = new List<WebTaxonName>();
            if (nameSearchString.IsNotEmpty() &&
                searchCriteria.NameSearchString.CompareOperators.IsNotEmpty() &&
                (searchCriteria.NameSearchString.CompareOperators.Count > 1))
            {
                foreach (StringCompareOperator compareOperator in searchCriteria.NameSearchString.CompareOperators)
                {
                    using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNamesBySearchCriteria(nameSearchString,
                                                                                                            compareOperator.ToString(),
                                                                                                            authorSearchString,
                                                                                                            taxonIdList,
                                                                                                            nameUsageId,
                                                                                                            nameCategoryId,
                                                                                                            isRecommended,
                                                                                                            isOriginal,
                                                                                                            isOkForObsSystems,
                                                                                                            isUnique,
                                                                                                            isValidTaxon,
                                                                                                            isValidTaxonName,
                                                                                                            isAuthorIncludedInNameSearchString,
                                                                                                            modifiedDateStart,
                                                                                                            modifiedDateEnd,
                                                                                                            GetRevisionIdFromRole(context),
                                                                                                            context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            webTaxonName = new WebTaxonName();
                            webTaxonName.LoadData(dataReader);
                            taxonNames.Add(webTaxonName);
                        }
                    }
                    if (taxonNames.IsNotEmpty())
                    {
                        // At least one taxon name found.
                        // Stop taxon name search.
                        break;
                    }
                }
            }
            else
            {
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNamesBySearchCriteria(nameSearchString,
                                                                                                        stringCompareOperator.ToString(),
                                                                                                        authorSearchString,
                                                                                                        taxonIdList,
                                                                                                        nameUsageId,
                                                                                                        nameCategoryId,
                                                                                                        isRecommended,
                                                                                                        isOriginal,
                                                                                                        isOkForObsSystems,
                                                                                                        isUnique,
                                                                                                        isValidTaxon,
                                                                                                        isValidTaxonName,
                                                                                                        isAuthorIncludedInNameSearchString,
                                                                                                        modifiedDateStart,
                                                                                                        modifiedDateEnd,
                                                                                                        GetRevisionIdFromRole(context),
                                                                                                        context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        webTaxonName = new WebTaxonName();
                        webTaxonName.LoadData(dataReader);
                        taxonNames.Add(webTaxonName);
                    }
                }
            }

            // Move taxon names that begins with taxon name search string
            // to the top of returned taxon names.
            if (nameSearchString.IsNotEmpty() && taxonNames.IsNotEmpty())
            {
                nameSearchString = nameSearchString.ToLower();
                for (taxonNameIndex = taxonNames.Count - 1; taxonNameIndex >= 0; taxonNameIndex--)
                {
                    if (taxonNames[taxonNameIndex].Name.ToLower().StartsWith(nameSearchString))
                    {
                        movedTaxonNameCount = 0;
                        while (taxonNames[taxonNameIndex].Name.ToLower().StartsWith(nameSearchString))
                        {
                            webTaxonName = taxonNames[taxonNameIndex];
                            taxonNames.RemoveAt(taxonNameIndex);
                            taxonNames.Insert(0, webTaxonName);
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

            GetTaxa(context, taxonNames);
            return taxonNames;
        }

        /// <summary>
        ///  Finds any kind of whitespace (e.g. tabs, multiple spaces, etc.) and replace them with a single space.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>String with single spaces</returns>
        private static String RemoveMultipleSpaces (String source)
        {
            source = Regex.Replace(source, @"\s+", " ");
            return source;
        }

        /// <summary>
        /// Determines whether the taxon is in a checked out revision.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>
        ///   <c>true</c> if the taxon is in a checked out revision otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsTaxonInRevision(WebServiceContext context, Int32 taxonId)
        {
            // Get information from database.
            return context.GetTaxonDatabase().IsTaxonInRevision(taxonId);
        }

        /// <summary>
        /// Undo all events in a revision
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="revision">The revision.</param>
        public static void UndoRevision(WebServiceContext context, WebTaxonRevision revision)
        {
            context.CheckTransaction();
            context.GetTaxonDatabase().UndoRevision(revision.Id);
        }

        /// <summary>
        /// Undo a revision event
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="revisionEvent">
        /// The revision event.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebTaxonRevision UndoRevisionEvent(WebServiceContext context, WebTaxonRevisionEvent revisionEvent)
        {
            context.CheckTransaction();
            context.GetTaxonDatabase().UndoRevisionEvent(revisionEvent.Id);
            return GetRevisionById(context, revisionEvent.RevisionId);
        }

        /// <summary>
        /// Get all taxon names for a taxon id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNamesByTaxonId(WebServiceContext context, Int32 taxonId)
        {
            List<WebTaxonName> taxonNames;
            taxonNames = new List<WebTaxonName>();

            // Get information from database.
            using (var dataReader = context.GetTaxonDatabase().GetTaxonNamesByTaxonId(taxonId, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    var webTaxonName = new WebTaxonName();
                    webTaxonName.LoadData(dataReader);
                    taxonNames.Add(webTaxonName);
                }
            }

            GetTaxa(context, taxonNames);

            return taxonNames;
        }

        /// <summary>
        /// Get all taxon names for a taxon ids.
        /// The result is sorted in the same order as input taxon ids.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxon names.</returns>
        public static List<List<WebTaxonName>> GetTaxonNamesByTaxonIds(WebServiceContext context,
                                                                       List<Int32> taxonIds)
        {
            Int32 index;
            List<List<WebTaxonName>> allTaxonNames;
            List<WebTaxonName> taxonNames;
            WebTaxonName taxonName;

            allTaxonNames = new List<List<WebTaxonName>>();
            if (taxonIds.IsNotEmpty())
            {
                for (index = 0; index < taxonIds.Count; index++)
                {
                    allTaxonNames.Add(new List<WebTaxonName>());
                }

                // Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonNamesByTaxonIds(taxonIds,
                                                                                                  GetRevisionIdFromRole(context),
                                                                                                  context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        taxonName = new WebTaxonName();
                        taxonName.LoadData(dataReader);
                        taxonNames = allTaxonNames[taxonIds.IndexOf(taxonName.Taxon.Id)];
                        taxonNames.Add(taxonName);
                    }
                }

                foreach (List<WebTaxonName> tempTaxonNames in allTaxonNames)
                {
                    GetTaxa(context, tempTaxonNames);
                }
            }

            return allTaxonNames;
        }

        /// <summary>
        /// Check in a revision.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>WebRevision object with revision state = CLOSED</returns>
        public static WebTaxonRevision RevisionCheckIn(WebServiceContext context, WebTaxonRevision revision)
        {
            context.CheckTransaction();
            context.GetTaxonDatabase().RevisionCheckIn(revision.Id);
            revision.StateId = (int)TaxonRevisionStateId.Closed;
            revision = UpdateRevision(context, revision);

            context.ClearCache(false);
            return revision;
        }

        /// <summary>
        /// Check out a revision.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>WebRevision object with revision state = ONGOING</returns>
        public static WebTaxonRevision RevisionCheckOut(WebServiceContext context, WebTaxonRevision revision)
        {
            // check if root taxonId in revision already is in another revision in state ONGOING.
            context.CheckTransaction();
            if (IsTaxonInRevision(context, revision.RootTaxon.Id))
            {
                throw new ArgumentException("Taxon in revision in conflict with other revision already checked out.");                
            }
            revision.StateId = (int)TaxonRevisionStateId.Ongoing;
            // TODO What else needs to be done during RevisionCheckOut ?
            revision = UpdateRevision(context, revision);

            ClearCachedTaxa(context);

            return revision;
        }

        /// <summary>
        /// Creates a new taxonrelation.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="taxonRelation">The taxon relation.</param>
        /// <returns>
        /// </returns>
        public static WebTaxonRelation CreateTaxonRelation(WebServiceContext context, WebTaxonRelation taxonRelation)
        {
            String personName;

            context.CheckTransaction();
            personName = GetPersonName(context);
            personName = personName.CheckInjection();
            Int32 taxonNameCategoryId = context.GetTaxonDatabase().CreateTaxonRelation(taxonRelation.ChildTaxonId, 
                                                                                        taxonRelation.ParentTaxonId,
                                                                                        personName,
                                                                                        WebServiceData.UserManager.GetUser(context).Id, 
                                                                                        taxonRelation.SortOrder, 
                                                                                        taxonRelation.IsChangedInTaxonRevisionEventIdSpecified ? taxonRelation.ChangedInTaxonRevisionEventId : 0,
                                                                                        taxonRelation.IsMainRelation);
            return GetTaxonRelationById(context, taxonNameCategoryId);
        }

        /// <summary>
        /// Update TaxonRelation
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="taxonRelation">TaxonRelation object.</param>
        /// <returns>Updated WebTaxonRelation object.</returns>
        public static WebTaxonRelation UpdateTaxonRelation(WebServiceContext context, WebTaxonRelation taxonRelation)
        {
            context.CheckTransaction();
            Int32 id = context.GetTaxonDatabase().UpdateTaxonRelation(taxonRelation.Id, 
                                                                        taxonRelation.ChildTaxonId, 
                                                                        taxonRelation.ParentTaxonId, 
                                                                        taxonRelation.SortOrder, 
                                                                        taxonRelation.IsChangedInTaxonRevisionEventIdSpecified ? taxonRelation.ChangedInTaxonRevisionEventId : 0, 
                                                                        taxonRelation.IsReplacedInTaxonRevisionEventIdSpecified ? taxonRelation.ReplacedInTaxonRevisionEventId : 0, 
                                                                        taxonRelation.ValidFromDate, 
                                                                        taxonRelation.ValidToDate, 
                                                                        taxonRelation.IsPublished,
                                                                        WebServiceData.UserManager.GetUser(context).Id);
            return GetTaxonRelationById(context, id);
        }

        /// <summary>
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static WebTaxonRelation GetTaxonRelationById(WebServiceContext context, int id)
        {
            WebTaxonRelation taxonRelation = null;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRelationById(id))
            {
                if (dataReader.Read())
                {
                    taxonRelation = new WebTaxonRelation();
                    taxonRelation.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Taxon relation not found. TaxonRelationId = " + id);
                }
            }

            return taxonRelation;
        }

        /// <summary>
        /// Get child taxon relations from taxon tree.
        /// </summary>
        /// <param name="taxonRelations">Accumulated taxon relations.</param>
        /// <param name="taxonTreeNode">Taxon tree node.</param>
        private static void GetChildTaxonRelations(Dictionary<Int32, WebTaxonRelation> taxonRelations,
                                                   TaxonTreeNode taxonTreeNode)
        {
            Int32 childRelationCountBefore;

            if (taxonTreeNode.IsNotNull() &&
                taxonTreeNode.ChildRelations.IsNotEmpty())
            {
                childRelationCountBefore = taxonRelations.Count;
                foreach (WebTaxonRelation parentRelation in taxonTreeNode.ChildRelations)
                {
                    if (!(taxonRelations.ContainsKey(parentRelation.Id)))
                    {
                        taxonRelations[parentRelation.Id] = parentRelation;
                    }
                }

                if (childRelationCountBefore < taxonRelations.Count)
                {
                    foreach (TaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                    {
                        GetChildTaxonRelations(taxonRelations, childTaxonTreeNode);
                    }
                }
            }
        }

        /// <summary>
        /// Get child taxon relations from taxon tree.
        /// </summary>
        /// <param name="taxonRelations">Accumulated taxon relations.</param>
        /// <param name="taxonTreeNode">Taxon tree node.</param>
        private static void GetParentTaxonRelations(Dictionary<Int32, WebTaxonRelation> taxonRelations,
                                                    TaxonTreeNode taxonTreeNode)
        {
            Int32 parentRelationCountBefore;

            if (taxonTreeNode.IsNotNull() &&
                taxonTreeNode.ParentRelations.IsNotEmpty())
            {
                parentRelationCountBefore = taxonRelations.Count;
                foreach (WebTaxonRelation parentRelation in taxonTreeNode.ParentRelations)
                {
                    if (!(taxonRelations.ContainsKey(parentRelation.Id)))
                    {
                        taxonRelations[parentRelation.Id] = parentRelation;
                    }
                }

                if (parentRelationCountBefore < taxonRelations.Count)
                {
                    foreach (TaxonTreeNode parentTaxonTreeNode in taxonTreeNode.Parents)
                    {
                        GetParentTaxonRelations(taxonRelations, parentTaxonTreeNode);
                    }
                }
            }
        }

        /// <summary>
        /// Get taxon relations related to specified taxon.
        /// </summary>
        /// <param name="taxonRelations">Accumulated taxon relations.</param>
        /// <param name="taxonTreeNode">Taxon tree node.</param>
        /// <param name="taxonIds">
        /// Remember which taxon tree nodes that has been processed.
        /// This is performed to avoid infinite recursion.
        /// </param>
        /// <param name="scope">Taxon relation search scope.</param>
        /// <returns>Taxon relations related to specified taxon.</returns>
        private static void GetTaxonRelations(List<WebTaxonRelation> taxonRelations,
                                              TaxonTreeNode taxonTreeNode,
                                              DataIdInt32List taxonIds,
                                              TaxonRelationSearchScope scope)
        {
            if (taxonIds.Exists(taxonTreeNode.TaxonId))
            {
                // Avoid infinite recursion.
                return;
            }
            else
            {
                taxonIds.Add(taxonTreeNode.TaxonId);
            }

            switch (scope)
            {
                case TaxonRelationSearchScope.AllChildRelations:
                    if (taxonTreeNode.ChildRelations.IsNotEmpty())
                    {
                        taxonRelations.AddRange(taxonTreeNode.ChildRelations);
                        foreach (TaxonTreeNode childTaxonTree in taxonTreeNode.Children)
                        {
                            GetTaxonRelations(taxonRelations, childTaxonTree, taxonIds, scope);
                        }
                    }
                    break;
                case TaxonRelationSearchScope.AllParentRelations:
                    if (taxonTreeNode.ParentRelations.IsNotEmpty())
                    {
                        taxonRelations.AddRange(taxonTreeNode.ParentRelations);
                        foreach (TaxonTreeNode parentTaxonTree in taxonTreeNode.Parents)
                        {
                            GetTaxonRelations(taxonRelations, parentTaxonTree, taxonIds, scope);
                        }
                    }
                    break;
                case TaxonRelationSearchScope.NearestChildRelations:
                    if (taxonTreeNode.ChildRelations.IsNotEmpty())
                    {
                        taxonRelations.AddRange(taxonTreeNode.ChildRelations);
                    }
                    break;
                case TaxonRelationSearchScope.NearestParentRelations:
                    if (taxonTreeNode.ParentRelations.IsNotEmpty())
                    {
                        taxonRelations.AddRange(taxonTreeNode.ParentRelations);
                    }
                    break;
                default:
                    throw new ApplicationException("Not handled taxon relation search scope: " + scope.ToString());
            }
        }

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        public static List<WebTaxonRelation> GetTaxonRelationsBySearchCriteria(WebServiceContext context,
                                                                               WebTaxonRelationSearchCriteria searchCriteria)
        {
            DataIdInt32List taxonIds;
            Hashtable cachedTaxonTrees;
            Int32? revisionId;
            List<WebTaxonRelation> taxonRelations;
            TaxonTreeNode taxonTree;
            WebTaxonRelation webTaxonRelation;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get taxon relations.
            revisionId = GetRevisionIdFromRole(context);
            taxonRelations = new List<WebTaxonRelation>();
            if (revisionId.HasValue)
            {
                // In revision.
                if (searchCriteria.TaxonIds.IsEmpty())
                {
                    // Get all taxon relations.
                    using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRelations(revisionId))
                    {
                        while (dataReader.Read())
                        {
                            webTaxonRelation = new WebTaxonRelation();
                            webTaxonRelation.LoadData(dataReader);
                            taxonRelations.Add(webTaxonRelation);
                        }
                    }
                }
                else
                {
                    // Get taxon relations related to specified taxa.
                    using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRelationsByTaxa(searchCriteria.TaxonIds,
                                                                                                      searchCriteria.Scope))
                    {
                        while (dataReader.Read())
                        {
                            webTaxonRelation = new WebTaxonRelation();
                            webTaxonRelation.LoadData(dataReader);
                            taxonRelations.Add(webTaxonRelation);
                        }
                    }
                }
            }
            else
            {
                // Not in revision.
                if (searchCriteria.TaxonIds.IsEmpty())
                {
                    // Get all published taxon relations.
                    taxonRelations.AddRange(GetCachedTaxonRelations(context));
                }
                else
                {
                    // Get taxon relations related to specified taxa.
                    cachedTaxonTrees = GetCachedTaxonTrees(context);
                    foreach (Int32 taxonId in searchCriteria.TaxonIds)
                    {
                        taxonIds = new DataIdInt32List();
                        taxonTree = (TaxonTreeNode)(cachedTaxonTrees[taxonId]);
                        GetTaxonRelations(taxonRelations, taxonTree, taxonIds, searchCriteria.Scope);
                    }
                }
            }

            // Filter taxon relations.
            FilterTaxonRelations(context, taxonRelations, searchCriteria);
            RemoveTaxonRelationsDuplicates(taxonRelations);

            return taxonRelations;
        }

        /// <summary>
        /// Removes duplicate taxon relations from <paramref name="taxonRelations"/>.
        /// </summary>
        /// <param name="taxonRelations">Taxon relations list.</param>
        private static void RemoveTaxonRelationsDuplicates(List<WebTaxonRelation> taxonRelations)
        {
            HashSet<WebTaxonRelation> taxonRelationsHashSet = new HashSet<WebTaxonRelation>(taxonRelations, new WebTaxonRelationIdEqualityComparer());
            taxonRelations.Clear();
            taxonRelations.AddRange(taxonRelationsHashSet);            
        }

        /// <summary>
        /// Convert a WebTaxonTreeSearchCriteria into a
        /// WebTaxonRelationSearchCriteria instance.
        /// </summary>
        /// <param name="taxonTreeSearchCriteria">Taxon tree search criteria.</param>
        /// <returns>A WebTaxonRelationSearchCriteria instance.</returns>
        private static WebTaxonRelationSearchCriteria GetTaxonRelationSearchCriteria(WebTaxonTreeSearchCriteria taxonTreeSearchCriteria)
        {
            WebTaxonRelationSearchCriteria taxonRelationSearchCriteria;

            taxonRelationSearchCriteria = new WebTaxonRelationSearchCriteria();
            taxonRelationSearchCriteria.IsIsMainRelationSpecified = taxonTreeSearchCriteria.IsMainRelationRequired;
            if (taxonRelationSearchCriteria.IsIsMainRelationSpecified)
            {
                taxonRelationSearchCriteria.IsMainRelation = true;
            }
            taxonRelationSearchCriteria.IsIsValidSpecified = taxonTreeSearchCriteria.IsValidRequired;
            if (taxonRelationSearchCriteria.IsIsValidSpecified)
            {
                taxonRelationSearchCriteria.IsValid = true;
            }
            switch (taxonTreeSearchCriteria.Scope)
            {
                case TaxonTreeSearchScope.AllChildTaxa:
                    taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
                    break;
                case TaxonTreeSearchScope.AllParentTaxa:
                    taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
                    break;
                case TaxonTreeSearchScope.NearestChildTaxa:
                    taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
                    break;
                case TaxonTreeSearchScope.NearestParentTaxa:
                    taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
                    break;
                default:
                    throw new ApplicationException("Not handled taxon tree search scope " + taxonTreeSearchCriteria.Scope);
            }
            taxonRelationSearchCriteria.TaxonIds = taxonTreeSearchCriteria.TaxonIds;
            return taxonRelationSearchCriteria;
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon revision event types.</returns>
        public static List<WebTaxonRevisionEventType> GetTaxonRevisionEventTypes(WebServiceContext context)
        {
            List<WebTaxonRevisionEventType> taxonRevisionEventTypes;
            String cacheKey;
            WebTaxonRevisionEventType taxonRevisionEventType;

            // Get cached information.
            cacheKey = Settings.Default.TaxonRevisionEventTypeCacheKey + ":" + context.Locale.ISOCode;
            taxonRevisionEventTypes = (List<WebTaxonRevisionEventType>)(context.GetCachedObject(cacheKey));

            if (taxonRevisionEventTypes.IsEmpty())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRevisionEventTypes(context.Locale.Id))
                {
                    taxonRevisionEventTypes = new List<WebTaxonRevisionEventType>();
                    while (dataReader.Read())
                    {
                        taxonRevisionEventType = new WebTaxonRevisionEventType();
                        taxonRevisionEventType.LoadData(dataReader);
                        taxonRevisionEventTypes.Add(taxonRevisionEventType);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonRevisionEventTypes,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonRevisionEventTypes;
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxon revision states.</returns>
        public static List<WebTaxonRevisionState> GetTaxonRevisionStates(WebServiceContext context)
        {
            List<WebTaxonRevisionState> taxonRevisionStates;
            String cacheKey;
            WebTaxonRevisionState taxonRevisionState;

            // Get cached information.
            cacheKey = Settings.Default.TaxonRevisionStateCacheKey + ":" + context.Locale.ISOCode;
            taxonRevisionStates = (List<WebTaxonRevisionState>)(context.GetCachedObject(cacheKey));

            if (taxonRevisionStates.IsEmpty())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonRevisionStates(context.Locale.Id))
                {
                    taxonRevisionStates = new List<WebTaxonRevisionState>();
                    while (dataReader.Read())
                    {
                        taxonRevisionState = new WebTaxonRevisionState();
                        taxonRevisionState.LoadData(dataReader);
                        taxonRevisionStates.Add(taxonRevisionState);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonRevisionStates,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonRevisionStates;
        }

        /// <summary>
        /// Get taxon statistics from database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Returns list of taxon statistics objects or null if no taxon statistics exists.
        /// </returns>
        public static List<WebTaxonChildStatistics> GetTaxonStatistics(WebServiceContext context, Int32 taxonId)
        {
            List<WebTaxonChildStatistics> taxonStatisticsList;
            WebTaxonChildStatistics taxonStatistics;
            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonStatistics(taxonId, GetRevisionIdFromRole(context), context.Locale.Id))
            {
                taxonStatisticsList = new List<WebTaxonChildStatistics>();
                while (dataReader.Read())
                {
                    taxonStatistics = new WebTaxonChildStatistics();
                    taxonStatistics.LoadData(dataReader);
                    taxonStatisticsList.Add(taxonStatistics);
                }
            }
            return taxonStatisticsList;
        }

        /// <summary>
        /// Get taxon DynTaxa quality summary from database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Returns list of taxon qualitiy summary objects 
        /// </returns>
        public static List<WebTaxonChildQualityStatistics> GetTaxonQualitySummary(WebServiceContext context, Int32 taxonId)
        {
            List<WebTaxonChildQualityStatistics> taxonQualitySummaryList;
            WebTaxonChildQualityStatistics taxonQualitySummary;
            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonQualitySummary(taxonId, GetRevisionIdFromRole(context)))
            {
                taxonQualitySummaryList = new List<WebTaxonChildQualityStatistics>();
                while (dataReader.Read())
                {
                    taxonQualitySummary = new WebTaxonChildQualityStatistics();
                    taxonQualitySummary.LoadData(dataReader);
                    taxonQualitySummaryList.Add(taxonQualitySummary);
                }
            }
            return taxonQualitySummaryList;
        }

        /// <summary>
        /// Gets the revision id from role.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>RevisionId for current role or null if current role isn't a revision identifier</returns>
        private static int? GetRevisionIdFromRole (WebServiceContext context)
        {
            WebRole currentRole;
            int nIndex = -1;
            int? nRevisionId = null;
            if (context.CurrentRole.IsNotNull())
            {
                currentRole = context.CurrentRole;
                try
                {
                    // roleIdentifier = urn:lsid:artdata.slu.se:Revision:1
                    nIndex = currentRole.Identifier.IndexOf(Settings.Default.TaxonRevisionGuidPrefix);
                    if (nIndex > -1)
                    {
                        nIndex = currentRole.Identifier.LastIndexOf(":");
                        if (nIndex > -1)
                        {
                            nRevisionId = ParseNullableInt(currentRole.Identifier.Substring(nIndex + 1, currentRole.Identifier.Length - (nIndex + 1)));
                        }
                    }
                }
                catch (Exception)
                {
                    // roleIdentifier does not match a Revision GUID
                    // do nothing = null will be returned
                }
            }
            return nRevisionId;

        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="context">The context.</param>
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
        /// Token and user roles for the specified application
        /// or null if the login failed.
        /// </returns>       
        public static WebLoginResponse Login(WebServiceContext context,
                                             String userName,
                                             String password,
                                             String applicationIdentifier,
                                             Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceData.UserManager.Login(context,
                                                             userName,
                                                             password,
                                                             applicationIdentifier,
                                                             isActivationRequired);
            if (loginResponse.IsNotNull() &&
                loginResponse.User.IsPersonIdSpecified &&
                applicationIdentifier == ApplicationIdentifier.Dyntaxa.ToString())
            {
                ClearCacheForUserRoles(context);
            }
            return loginResponse;
        }

        private static int? ParseNullableInt(string value)
        {
            int intValue;     
            if (int.TryParse(value, out intValue))         
                return intValue;     
            return null;
        }

        /// <summary>
        /// Test if user is working in a revision.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>True if user is working in a revision.</returns>
        private static Boolean IsUserInRevision(WebServiceContext context)
        {
            return GetRevisionIdFromRole(context).HasValue;
        }

        /// <summary>
        /// Get person fullname from WebServiceContext.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns persons fullname.</returns>
        private static String GetPersonName(WebServiceContext context)
        {
            WebPerson person = WebServiceData.UserManager.GetPerson(context);
            String personName;
            personName = "";
            if (person.IsNotNull())
            {
                personName = (person.FirstName + " " + person.LastName).ToString().Trim();
            }
            return personName;

        }

        /// <summary>
        /// Get information about a taxon
        /// Called from CreateTaxon after a new Taxon has been created.
        /// The normal "GetTaxonById" handles revision.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Returns taxon information or null if taxonid doesn't match.</returns>
        private static WebTaxon GetTaxonByIdAfterCreate(WebServiceContext context, Int32 taxonId)
        {
            WebTaxon taxon;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetTaxonByIdAfterCreate(taxonId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    taxon = new WebTaxon();
                    taxon.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Taxon not found. TaxonId = " + taxonId);
                }
            }

            return taxon;
        }
    }
}
