using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Manager class for Metadata handling.
    /// </summary>
    public class MetadataManager : ManagerBase, IMetadataManager
    {
        /// <summary>
        /// Clears the cache, so that the next call that is made reads the information from the database
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private void ClearCache(string key, WebServiceContext context)
        {
            context.RemoveCachedObject(this.GetCacheKey(key, context));
            context.RemoveCachedObject(Settings.Default.DarwinCoreFieldMappingCacheKey);
        }

        /// <summary>
        /// (cached) Return a list of fieldDescription and mapping
        /// Get all Species observation field descriptions from the DB.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reloadCache">Optional, if set to true the cache will be reset.</param>
        /// <returns>A List with all Species observation field Descriptions Extended.</returns>
        public List<WebSpeciesObservationFieldDescriptionExtended> GetSpeciesObservationFieldDescriptionsExtended(WebServiceContext context, Boolean reloadCache = false)
        {
            var cacheKey = this.GetCacheKey(Settings.Default.DarwinCoreFieldDescriptionExtendedCacheKey, context);

            if (reloadCache)
            {
                this.ClearCache(cacheKey, context);
            }

            var darwinCoreFieldDescriptionsExtended = (List<WebSpeciesObservationFieldDescriptionExtended>)(context.GetCachedObject(cacheKey));

            if (darwinCoreFieldDescriptionsExtended.IsEmpty() || reloadCache)
            {
                var darwinCoreFieldMappings = GetSpeciesObservationFieldMappings(context);
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationFieldDescriptions(context.Locale.Id))
                {
                    darwinCoreFieldDescriptionsExtended = new List<WebSpeciesObservationFieldDescriptionExtended>();
                    while (dataReader.Read())
                    {
                        var speciesObservationFieldDescriptionExtended = new WebSpeciesObservationFieldDescriptionExtended();
                        speciesObservationFieldDescriptionExtended.Load(dataReader);
                        if (!speciesObservationFieldDescriptionExtended.IsImplemented) continue;

                        speciesObservationFieldDescriptionExtended.Mappings = GetSpeciesObservationFieldMappings(darwinCoreFieldMappings, speciesObservationFieldDescriptionExtended.Id);

                        darwinCoreFieldDescriptionsExtended.Add(speciesObservationFieldDescriptionExtended);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        darwinCoreFieldDescriptionsExtended,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }
            return darwinCoreFieldDescriptionsExtended;
        }

        /// <summary>
        /// (cached) Return a list of fieldDescription and mapping
        /// Get all Species observation field descriptions from the DB.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reloadCache">Optional, if set to true the cache will be reset.</param>
        /// <returns>A List with all public Species observation field Descriptions.</returns>
        public List<WebSpeciesObservationFieldDescription> GetSpeciesObservationFieldDescriptions(WebServiceContext context, Boolean reloadCache = false)
        {
            var cacheKey = this.GetCacheKey(Settings.Default.DarwinCoreFieldDescriptionCacheKey, context);

            if (reloadCache)
            {
                this.ClearCache(cacheKey, context);
            }

            var darwinCoreFieldDescriptions = (List<WebSpeciesObservationFieldDescription>)(context.GetCachedObject(cacheKey));

            if (darwinCoreFieldDescriptions.IsEmpty() || reloadCache)
            {
                List<WebSpeciesObservationFieldDescriptionExtended> darwinCoreFieldDescriptionsExtended = GetSpeciesObservationFieldDescriptionsExtended(context, false);
                darwinCoreFieldDescriptions = new List<WebSpeciesObservationFieldDescription>();

                foreach (WebSpeciesObservationFieldDescriptionExtended webSpeciesObservationFieldDescriptionExtended in darwinCoreFieldDescriptionsExtended)
                {
                    if (webSpeciesObservationFieldDescriptionExtended.IsPublic)
                    {
                        var speciesObservationFieldDescription = new WebSpeciesObservationFieldDescription();

                        speciesObservationFieldDescription.Class = webSpeciesObservationFieldDescriptionExtended.Class;
                        speciesObservationFieldDescription.Definition = webSpeciesObservationFieldDescriptionExtended.Definition;
                        speciesObservationFieldDescription.DefinitionUrl = webSpeciesObservationFieldDescriptionExtended.DefinitionUrl;
                        speciesObservationFieldDescription.Documentation = webSpeciesObservationFieldDescriptionExtended.Documentation;
                        speciesObservationFieldDescription.DocumentationUrl = webSpeciesObservationFieldDescriptionExtended.DocumentationUrl;
                        speciesObservationFieldDescription.Guid = webSpeciesObservationFieldDescriptionExtended.Guid;
                        speciesObservationFieldDescription.Id = webSpeciesObservationFieldDescriptionExtended.Id;
                        speciesObservationFieldDescription.Importance = webSpeciesObservationFieldDescriptionExtended.Importance;
                        speciesObservationFieldDescription.IsAcceptedByTdwg = webSpeciesObservationFieldDescriptionExtended.IsAcceptedByTdwg; //added
                        speciesObservationFieldDescription.IsClass = webSpeciesObservationFieldDescriptionExtended.IsClass;
                        //speciesObservationFieldDescription.IsDarwinCoreProperty = webSpeciesObservationFieldDescriptionExtended.IsDarwinCoreProperty
                        speciesObservationFieldDescription.IsImplemented = webSpeciesObservationFieldDescriptionExtended.IsImplemented; //added
                        speciesObservationFieldDescription.IsMandatory = webSpeciesObservationFieldDescriptionExtended.IsMandatory;
                        speciesObservationFieldDescription.IsMandatoryFromProvider = webSpeciesObservationFieldDescriptionExtended.IsMandatoryFromProvider;
                        speciesObservationFieldDescription.IsObtainedFromProvider = webSpeciesObservationFieldDescriptionExtended.IsObtainedFromProvider;
                        speciesObservationFieldDescription.IsPlanned = webSpeciesObservationFieldDescriptionExtended.IsPlanned;
                        //speciesObservationFieldDescription.IsPublic = webSpeciesObservationFieldDescriptionExtended.IsPublic; //added
                        speciesObservationFieldDescription.IsSearchable = webSpeciesObservationFieldDescriptionExtended.IsSearchable;
                        speciesObservationFieldDescription.IsSortable = webSpeciesObservationFieldDescriptionExtended.IsSortable;
                        speciesObservationFieldDescription.Label = webSpeciesObservationFieldDescriptionExtended.Label;
                        speciesObservationFieldDescription.Mappings = webSpeciesObservationFieldDescriptionExtended.Mappings;
                        speciesObservationFieldDescription.Name = webSpeciesObservationFieldDescriptionExtended.Name;
                        speciesObservationFieldDescription.Property = webSpeciesObservationFieldDescriptionExtended.Property;
                        speciesObservationFieldDescription.Remarks = webSpeciesObservationFieldDescriptionExtended.Remarks;
                        speciesObservationFieldDescription.SortOrder = webSpeciesObservationFieldDescriptionExtended.SortOrder;
                        speciesObservationFieldDescription.Type = webSpeciesObservationFieldDescriptionExtended.Type;
                        speciesObservationFieldDescription.Uuid = webSpeciesObservationFieldDescriptionExtended.Uuid;

                        darwinCoreFieldDescriptions.Add(speciesObservationFieldDescription);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        darwinCoreFieldDescriptions,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }
            return darwinCoreFieldDescriptions;
        }

        private string GetCacheKey(string key, WebServiceContext context)
        {
            return key + ":" + context.Locale.ISOCode;
        }

        /// <summary>
        /// Get all species observation field mappings for a single field.
        /// </summary>
        /// <param name="allMappings">All field mappings.</param>
        /// <param name="fieldId">Id of requsted field.</param>
        /// <returns>List of field mapping objects.</returns>
        private List<WebSpeciesObservationFieldMapping> GetSpeciesObservationFieldMappings(List<WebSpeciesObservationFieldMapping> allMappings, Int32 fieldId)
        {
            List<WebSpeciesObservationFieldMapping> fieldMappings = new List<WebSpeciesObservationFieldMapping>();

            foreach (WebSpeciesObservationFieldMapping fieldMapping in allMappings)
            {
                if (fieldMapping.FieldId == fieldId)
                {
                    fieldMappings.Add(fieldMapping);
                }
            }
            return fieldMappings;
        }

        /// <summary>
        /// (cached) Get all species observation field mappings.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A List with all species observation field mappings.</returns>
        private List<WebSpeciesObservationFieldMapping> GetSpeciesObservationFieldMappings(WebServiceContext context)
        {
            List<WebSpeciesObservationFieldMapping> fieldMappings;
            String cacheKey;
            WebSpeciesObservationFieldMapping speciesObservationFieldMapping;
            // Get cached information.
            cacheKey = Settings.Default.DarwinCoreFieldMappingCacheKey;
            fieldMappings = (List<WebSpeciesObservationFieldMapping>)(context.GetCachedObject(cacheKey));

            if (fieldMappings.IsEmpty())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationFieldMappings())
                {
                    fieldMappings = new List<WebSpeciesObservationFieldMapping>();
                    while (dataReader.Read())
                    {
                        speciesObservationFieldMapping = new WebSpeciesObservationFieldMapping();
                        speciesObservationFieldMapping.Load(dataReader);
                        fieldMappings.Add(speciesObservationFieldMapping);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        fieldMappings,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }
            return fieldMappings;
        }

        /// <summary>
        /// Adds or updates SpeciesObservationFieldDescriptions to the database, the cache is cleared afterwards
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="table">The SpeciesObservationFieldDescription table that's supposed to be added</param>
        public void UpdateSpeciesObservationFieldDescription(WebServiceContext context, DataTable table)
        {
            context.GetDatabase().UpdateSpeciesObservationFieldDescription(table);
            this.ClearCache(this.GetCacheKey(Settings.Default.DarwinCoreFieldDescriptionCacheKey, context), context);
            this.ClearCache(this.GetCacheKey(Settings.Default.DarwinCoreFieldDescriptionExtendedCacheKey, context), context);
        }

        /// <summary>
        /// Adds or updates SpeciesObservationFieldMappings to the database, the cache is cleared afterwards
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="table">The SpeciesObservationFieldMapping table that's supposed to be added</param>
        public void UpdateSpeciesObservationFieldMapping(WebServiceContext context, DataTable table)
        {
            context.GetDatabase().UpdateSpeciesObservationFieldMapping(table);
            this.ClearCache(Settings.Default.DarwinCoreFieldMappingCacheKey, context);
        }
    }
}
