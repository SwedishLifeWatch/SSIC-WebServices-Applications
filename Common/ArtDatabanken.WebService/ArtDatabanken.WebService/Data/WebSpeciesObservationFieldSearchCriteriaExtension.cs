using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods for class WebSpeciesObservationFieldSearchCriteria.
    /// </summary>
    public static class WebSpeciesObservationFieldSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="mapping">Information about fields in Elasticsearch.</param>
        public static void CheckData(this WebSpeciesObservationFieldSearchCriteria searchCriteria,
                                     Dictionary<String, WebSpeciesObservationField> mapping)
        {
            String fieldName;
            WebSpeciesObservationField field;

            if (searchCriteria.IsNotNull())
            {
                searchCriteria.Class.CheckNotNull("Class");
                searchCriteria.Class.GetClass().CheckNotEmpty("Class string");
                searchCriteria.Property.CheckNotNull("Property");
                searchCriteria.Property.GetProperty().CheckNotEmpty("Property string");
                searchCriteria.Value.CheckNotEmpty("Value");
                searchCriteria.Value = searchCriteria.Value.CheckJsonInjection();

                fieldName = searchCriteria.GetFieldName();
                if (mapping.ContainsKey(fieldName))
                {
                    field = mapping[fieldName];
                }
                else
                {
                    throw new ArgumentException("No field in Elasticsearch with name = " + fieldName);
                }

                if ((fieldName == "Location_CoordinateX") ||
                    (fieldName == "Location_CoordinateY"))
                {
                    throw new ArgumentException("Can not make field search on field " + fieldName);
                }

                if ((field.Type != searchCriteria.Type) &&
                    !((field.Type == WebDataType.Int64) &&
                      (searchCriteria.Type == WebDataType.Int32)))
                {
                    throw new ArgumentException("Different data types. Field type = " + field.Type + ". Field search criteria type = " + searchCriteria.Type + ".");
                }

                switch (searchCriteria.Type)
                {
                    case WebDataType.Boolean:
                        if (!((searchCriteria.Operator == CompareOperator.Equal) ||
                              (searchCriteria.Operator == CompareOperator.NotEqual)))
                        {
                            throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type " + searchCriteria.Type + ".");
                        }
                        break;

                    case WebDataType.DateTime:
                    case WebDataType.Float64:
                    case WebDataType.Int32:
                    case WebDataType.Int64:
                        if (!((searchCriteria.Operator == CompareOperator.Equal) ||
                              (searchCriteria.Operator == CompareOperator.Greater) ||
                              (searchCriteria.Operator == CompareOperator.GreaterOrEqual) ||
                              (searchCriteria.Operator == CompareOperator.Less) ||
                              (searchCriteria.Operator == CompareOperator.LessOrEqual) ||
                              (searchCriteria.Operator == CompareOperator.NotEqual)))
                        {
                            throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type " + searchCriteria.Type + ".");
                        }
                        break;

                    case WebDataType.String:
                        if (!((searchCriteria.Operator == CompareOperator.BeginsWith) ||
                              (searchCriteria.Operator == CompareOperator.Contains) ||
                              (searchCriteria.Operator == CompareOperator.EndsWith) ||
                              (searchCriteria.Operator == CompareOperator.Equal) ||
                              (searchCriteria.Operator == CompareOperator.Like) ||
                              (searchCriteria.Operator == CompareOperator.NotEqual)))
                        {
                            throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type " + searchCriteria.Type + ".");
                        }
                        break;

                    default:
                        throw new ArgumentException("Not supported data type = " + searchCriteria.Type);
                }
            }
        }

        /// <summary>
        /// Get boolean species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        private static String GetBooleanFilter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            Boolean compareValue;

            compareValue = searchCriteria.Value.WebParseBoolean();
            if (searchCriteria.Operator == CompareOperator.NotEqual)
            {
                compareValue = !compareValue;
            }

            return "{\"term\": {\"" + GetFieldName(searchCriteria) +
                   "\" : " + compareValue.ToString().ToLower() + "}}";
        }

        /// <summary>
        /// Get date time species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        private static String GetDateTimeFilter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            DateTime compareValue;

            compareValue = searchCriteria.Value.WebParseDateTime();
            switch (searchCriteria.Operator)
            {
                case CompareOperator.Equal:
                    return "{\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : \"" + compareValue.WebToString() + "\"}}";
                case CompareOperator.Greater:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gt\": \"" + compareValue.WebToString() + "\"}}}";
                case CompareOperator.GreaterOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gte\": \"" + compareValue.WebToString() + "\"}}}";
                case CompareOperator.Less:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lt\": \"" + compareValue.WebToString() + "\"}}}";
                case CompareOperator.LessOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lte\": \"" + compareValue.WebToString() + "\"}}}";
                case CompareOperator.NotEqual:
                    return "{\"bool\":{ \"must_not\" : {\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : \"" + compareValue.WebToString() + "\"}}}}";
                default:
                    throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type DateTime.");
            }
        }

        /// <summary>
        /// Get field name in Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Field name in Elasticsearch.</returns>
        public static String GetFieldName(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            if ((searchCriteria.Class.Id == SpeciesObservationClassId.Project)
                && (searchCriteria.Property.Id == SpeciesObservationPropertyId.None))
            {
                return searchCriteria.Property.Identifier;
            }
            else
            {
                return searchCriteria.Class.GetClass() + "_" +
                       searchCriteria.Property.GetProperty();
            }
        }

        /// <summary>
        /// Get species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        public static String GetFilter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            switch (searchCriteria.Type)
            {
                case WebDataType.Boolean:
                    return GetBooleanFilter(searchCriteria);
                case WebDataType.DateTime:
                    return GetDateTimeFilter(searchCriteria);
                case WebDataType.Float64:
                    return GetFloat64Filter(searchCriteria);
                case WebDataType.Int32:
                case WebDataType.Int64:
                    return GetIntegerFilter(searchCriteria);
                case WebDataType.String:
                    return GetStringFilter(searchCriteria);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get float 64 species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        private static String GetFloat64Filter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            Double compareValue;

            compareValue = searchCriteria.Value.WebParseDouble();
            switch (searchCriteria.Operator)
            {
                case CompareOperator.Equal:
                    return "{\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : " + compareValue.WebToStringR() + "}}";
                case CompareOperator.Greater:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gt\": " + compareValue.WebToStringR() + "}}}";
                case CompareOperator.GreaterOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gte\": " + compareValue.WebToStringR() + "}}}";
                case CompareOperator.Less:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lt\": " + compareValue.WebToStringR() + "}}}";
                case CompareOperator.LessOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lte\": " + compareValue.WebToStringR() + "}}}";
                case CompareOperator.NotEqual:
                    return "{\"bool\":{ \"must_not\" : {\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : " + compareValue.WebToStringR() + "}}}}";
                default:
                    throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type Double.");
            }
        }

        /// <summary>
        /// Get integer species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        private static String GetIntegerFilter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            Int64 compareValue;

            compareValue = searchCriteria.Value.WebParseInt64();
            switch (searchCriteria.Operator)
            {
                case CompareOperator.Equal:
                    return "{\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : " + compareValue.WebToString() + "}}";
                case CompareOperator.Greater:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gt\": " + compareValue.WebToString() + "}}}";
                case CompareOperator.GreaterOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"gte\": " + compareValue.WebToString() + "}}}";
                case CompareOperator.Less:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lt\": " + compareValue.WebToString() + "}}}";
                case CompareOperator.LessOrEqual:
                    return "{\"range\": {\"" + GetFieldName(searchCriteria) +
                           "\" : { \"lte\": " + compareValue.WebToString() + "}}}";
                case CompareOperator.NotEqual:
                    return "{\"bool\":{ \"must_not\" : {\"term\": {\"" + GetFieldName(searchCriteria) +
                           "\" : " + compareValue.WebToString() + "}}}}";
                default:
                    throw new ArgumentException("Operator " + searchCriteria.Operator + " is not supported on data of type Integer.");
            }
        }

        /// <summary>
        /// Get string filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>String filter.</returns>
        private static String GetStringFilter(this WebSpeciesObservationFieldSearchCriteria searchCriteria)
        {
            if (searchCriteria.IsNotNull())
            {
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.BeginsWith:
                        return "{ \"prefix\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                               "\" : \"" + searchCriteria.Value.ToLower() + "\" }}";

                    case CompareOperator.Contains:
                        return "{\"query\" : { \"wildcard\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                               "\" : \"*" + searchCriteria.Value.ToLower() + "*\"}}}";

                    case CompareOperator.EndsWith:
                        return "{\"query\" : { \"wildcard\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                               "\" : \"*" + searchCriteria.Value.ToLower() + "\"}}}";

                    case CompareOperator.Equal:
                        return "{ \"term\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                               "\" : \"" + searchCriteria.Value.ToLower() + "\" }}";

                    case CompareOperator.Like:
                        return "{\"query\" : { \"wildcard\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                               "\" : \"" + searchCriteria.Value.ToLower() + "\"}}}";

                    case CompareOperator.NotEqual:
                        return "{not : { \"term\" : { \"" + GetFieldName(searchCriteria) + "_Lowercase" +
                                "\" : \"" + searchCriteria.Value.ToLower() + "\" }}}";

                    default:
                        throw new ArgumentException("Not handled string compare operator = " + searchCriteria.Operator);
                }
            }

            return null;
        }

        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified field search criteria.
        /// </summary>
        /// <param name="fieldSearchCriterias">Field search criteria.</param>
        /// <param name="fieldLogicalOperator">Defines how field search criterias are combined.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias,  LogicalOperator fieldLogicalOperator)
        {
            StringBuilder whereCondition;
            String logicalOperator;
            Int32 fieldSearchCriteriaCount = 0;
            String selectTableName, tableName;

            whereCondition = new StringBuilder();
            whereCondition.Append("(");
            selectTableName = String.Empty;
            switch (fieldLogicalOperator)
            {
                case LogicalOperator.And:
                    logicalOperator = "AND";
                    break;
                case LogicalOperator.Or:
                    logicalOperator = "OR";
                    break;
                default:
                    throw new ArgumentException("Not handled field logical operator: " + fieldLogicalOperator);
            }

            // Special case if there are a UncertainDetermination criteria in the list of criterias
            WebSpeciesObservationFieldSearchCriteria uncertainDeterminationSearchCriteria = 
                fieldSearchCriterias.FirstOrDefault(x => x.Property.Id == SpeciesObservationPropertyId.UncertainDetermination);

            if (uncertainDeterminationSearchCriteria != null)
            {
                fieldSearchCriterias.Remove(uncertainDeterminationSearchCriteria);

                if (uncertainDeterminationSearchCriteria.Value.WebParseBoolean())
                {
                    whereCondition.Append(" (O.uncertainDetermination = 1) ");
                }
                else
                {
                    whereCondition.Append(" (O.uncertainDetermination = 0) ");
                }

                if (fieldSearchCriterias.IsEmpty())
                {
                    whereCondition.Append(") ");
                    return whereCondition.ToString();
                }

                whereCondition.Append(logicalOperator);
            }

            whereCondition.Append(" O.[Id] IN (");
            switch (fieldLogicalOperator)
            {
                case LogicalOperator.And:
                    foreach (WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria in fieldSearchCriterias)
                    {
                        tableName = "SOF" + (fieldSearchCriteriaCount + 1);
                        if (fieldSearchCriteriaCount == 0)
                        {
                            selectTableName = tableName;
                            whereCondition.Append("SELECT DISTINCT " + tableName + ".[observationId] ");
                            whereCondition.Append("FROM [SpeciesObservationField] AS " + tableName + " ");
                        }
                        else
                        {
                            whereCondition.Append("INNER JOIN [SpeciesObservationField] AS " + tableName + " ");
                            whereCondition.Append("ON " + tableName + ".[observationId] = " + selectTableName + ".[observationId] ");
                        }

                        fieldSearchCriteriaCount++;
                    }

                    whereCondition.Append("WHERE ");
                    fieldSearchCriteriaCount = 0;
                    foreach (WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria in fieldSearchCriterias)
                    {
                        tableName = "SOF" + (fieldSearchCriteriaCount + 1);
                        if (fieldSearchCriteriaCount > 0)
                        {
                            whereCondition.Append(" AND ");
                        }

                        whereCondition.Append(GetWhereCondition(fieldSearchCriteria, tableName));
                        fieldSearchCriteriaCount++;
                    }
                    break;

                case LogicalOperator.Or:
                    foreach (WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria in fieldSearchCriterias)
                    {
                        if (fieldSearchCriteriaCount > 0)
                        {
                            whereCondition.Append(" UNION ");
                        }

                        fieldSearchCriteriaCount++;
                        tableName = "SOF" + fieldSearchCriteriaCount;

                        whereCondition.Append("SELECT " + tableName + ".[observationId] ");
                        whereCondition.Append("FROM [SpeciesObservationField] AS " + tableName + " ");
                        whereCondition.Append("WHERE ");
                        whereCondition.Append(GetWhereCondition(fieldSearchCriteria, tableName));
                    }
                    break;

                default:
                    throw new ArgumentException("Not handled field logical operator: " + fieldLogicalOperator);
            }

            whereCondition.Append(")) ");
            return whereCondition.ToString();
        }

        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified field search criteria.
        /// </summary>
        /// <param name="fieldSearchCriteria">Field search criteria.</param>
        /// <param name="tableName">Logical name of the species observation field table.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria, String tableName)
        {
            StringBuilder whereCondition;
            String fieldProperty, fieldValue, filterExpression;

            whereCondition = new StringBuilder();
            whereCondition.Append(" (");
            fieldValue = fieldSearchCriteria.Value.CheckInjection();
            if (fieldSearchCriteria.Property.Id == SpeciesObservationPropertyId.None)
            {
                fieldProperty = fieldSearchCriteria.Property.Identifier.CheckInjection();
            }
            else
            {
                fieldProperty = fieldSearchCriteria.Property.Id.ToString();
            }

            whereCondition.Append(tableName + ".[class] = '" + fieldSearchCriteria.Class.Id + "'");
            whereCondition.Append(" AND ");
            whereCondition.Append(tableName + ".[property] = '" + fieldProperty + "'");

            // Decide which data type column the value should be read from
            switch (fieldSearchCriteria.Type)
            {
                case WebDataType.Boolean:
                    whereCondition.Append(" AND " + tableName + ".[value_Boolean]");
                    break;
                case WebDataType.DateTime:
                    whereCondition.Append(" AND " + tableName + ".[value_DateTime]");
                    break;
                case WebDataType.Float64:
                    whereCondition.Append(" AND " + tableName + ".[value_Double]");
                    break;
                case WebDataType.Int32:
                case WebDataType.Int64:
                    whereCondition.Append(" AND " + tableName + ".[value_Int]");
                    break;
                case WebDataType.String:
                    whereCondition.Append(" AND " + tableName + ".[value_String]");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Create SQL search condition.
            switch (fieldSearchCriteria.Operator)
            {
                case CompareOperator.BeginsWith:
                    filterExpression = " LIKE '{1}%'";
                    break;
                case CompareOperator.Contains:
                    filterExpression = " LIKE '%{1}%'";
                    break;
                case CompareOperator.EndsWith:
                    filterExpression = " LIKE '%{1}'";
                    break;
                case CompareOperator.Equal:
                    filterExpression = " = {0}{1}{2}";
                    break;
                case CompareOperator.Greater:
                    filterExpression = " > {0}{1}{2}";
                    break;
                case CompareOperator.GreaterOrEqual:
                    filterExpression = " >= {0}{1}{2}";
                    break;
                case CompareOperator.Less:
                    filterExpression = " < {0}{1}{2}";
                    break;
                case CompareOperator.LessOrEqual:
                    filterExpression = " <= {0}{1}{2}";
                    break;
                case CompareOperator.Like:
                    filterExpression = " LIKE '{1}'";
                    break;
                case CompareOperator.NotEqual:
                    filterExpression = " <> {0}{1}{2}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Apply cituation signs.
            switch (fieldSearchCriteria.Type)
            {
                case WebDataType.Boolean:
                    if (fieldValue.WebParseBoolean())
                    {
                        filterExpression = String.Format(filterExpression, String.Empty, "1", String.Empty);
                    }
                    else
                    {
                        filterExpression = String.Format(filterExpression, String.Empty, "0", String.Empty);
                    }

                    break;
                case WebDataType.DateTime:
                case WebDataType.String:
                    filterExpression = String.Format(filterExpression, "'", fieldValue, "'");
                    break;
                default:
                    filterExpression = String.Format(filterExpression, String.Empty, fieldValue, String.Empty);
                    break;
            }

            whereCondition.Append(filterExpression);
            whereCondition.Append(")");
            return whereCondition.ToString();
        }
    }
}
