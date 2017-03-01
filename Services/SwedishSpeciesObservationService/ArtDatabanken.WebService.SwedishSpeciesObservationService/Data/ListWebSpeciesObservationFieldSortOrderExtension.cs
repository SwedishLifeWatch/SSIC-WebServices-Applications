using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    using System.Text;

    /// <summary>
    /// Contains extension methods to a generic list of type
    /// WebSpeciesObservationFieldSortOrder.
    /// </summary>
    public static class ListWebSpeciesObservationFieldSortOrderExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="sortOrders">Sort orders.</param>
        public static void CheckData(this List<WebSpeciesObservationFieldSortOrder> sortOrders)
        {
            if (sortOrders.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldSortOrder sortOrder in sortOrders)
                {
                    sortOrder.CheckData();
                }
            }
        }

        /// <summary>
        /// Get sort order in json format.
        /// </summary>
        /// <param name="sortOrders">Species observation sort order specification.</param>
        /// <param name="mapping">Elasticsearch mapping.</param>
        /// <returns>Sort order in json format.</returns>
        public static String GetSortOrderJson(this List<WebSpeciesObservationFieldSortOrder> sortOrders,
                                              Dictionary<String, WebSpeciesObservationField> mapping)
        {
            Boolean isFirstSortOrder;
            String fieldName;
            StringBuilder sortOrderJson;

            sortOrderJson = new StringBuilder();
            sortOrderJson.Append("\"sort\": {");
            isFirstSortOrder = true;
            if (sortOrders.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldSortOrder sortOrder in sortOrders)
                {
                    fieldName = sortOrder.Class.GetClass() + "_" +
                                sortOrder.Property.GetProperty();
                    if (mapping.ContainsKey(fieldName))
                    {
                        if (isFirstSortOrder)
                        {
                            isFirstSortOrder = false;
                        }
                        else
                        {
                            sortOrderJson.Append(", ");
                        }

                        sortOrderJson.Append(sortOrder.GetSortOrderJson());
                    }
                    // else: Unknown field. Can not sort on this field.
                }
            }

            if (isFirstSortOrder)
            {
                // Use default sort order.
                sortOrderJson.Append("\"Event_Start\": {\"order\": \"desc\"}");
            }

            sortOrderJson.Append("}");
            return sortOrderJson.ToString();
        }

        /// <summary>
        /// Get sort order in SQL format.
        /// </summary>
        /// <param name="sortOrder">Species observation sort order specification.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="isDefaultSortOrderUsed">
        /// Specifies if default sort order should be used if
        /// no other sort order has been specified.
        /// </param>
        /// <returns>Sort order in SQL format.</returns>
        public static String GetSqlSortOrder(this List<WebSpeciesObservationFieldSortOrder> sortOrder,
                                             WebServiceContext context,
                                             WebSpeciesObservationSearchCriteria searchCriteria,
                                             Boolean isDefaultSortOrderUsed)
        {
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            String currentField, tableName;
            String sqlSortOrder = String.Empty, separator = String.Empty;

            if (sortOrder.IsNotEmpty())
            {
                if (searchCriteria.Polygons.IsNotEmpty() ||
                    searchCriteria.RegionGuids.IsNotEmpty())
                {
                    tableName = "[SpeciesObservation]";
                }
                else
                {
                    tableName = "[O]";
                }

                if (sortOrder.IsNotEmpty())
                {
                    // Get field descriptions.
                    fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context);

                    foreach (WebSpeciesObservationFieldSortOrder fieldSortOrder in sortOrder)
                    {
                        // Try to find if the requested search field
                        // is in field descriptions.
                        currentField = String.Empty;
                        if (fieldSortOrder.Property.Id.IsNotNull() && (fieldSortOrder.Property.Id.ToString() != "None"))
                        {
                            currentField = fieldSortOrder.Property.Id.ToString();
                        }
                        else if (fieldSortOrder.Property.Identifier.IsNotNull())
                        {
                            currentField = fieldSortOrder.Property.Identifier.CheckInjection();
                        }

                        if (currentField == String.Empty) continue;

                        // Create SQL sort order.
                        foreach (WebSpeciesObservationFieldDescriptionExtended webSpeciesObservationFieldDescriptionExtended in fieldDescriptions)
                        {
                            if (webSpeciesObservationFieldDescriptionExtended.Name.Equals(currentField))
                            {
                                if (!webSpeciesObservationFieldDescriptionExtended.IsPublic) break;
                                if (!webSpeciesObservationFieldDescriptionExtended.IsSortable) break;
                                sqlSortOrder = sqlSortOrder + separator + tableName + "." + fieldSortOrder.GetSortOrder();
                                separator = ", ";
                                break;
                            }
                        }
                    }
                }

                if (sqlSortOrder.IsEmpty() && isDefaultSortOrderUsed)
                {
                    // Use default sort order.
                    sqlSortOrder = tableName + ".[Id] ASC";
                }
            }

            return sqlSortOrder;
        }
    }
}
