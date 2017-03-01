using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesFactFieldSearchCriteria class.
    /// </summary>
    public static class WebSpeciesFactFieldSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Species fact field search criteria.</param>
        public static void CheckData(this WebSpeciesFactFieldSearchCriteria searchCriteria)
        {
            Boolean booleanValue;
            Double doubleValue;
            Int32 index, int32Value;

            // Check that factor field has been specified.
            if (searchCriteria.FactorField.IsNull())
            {
                throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: No factor field specified!");
            }

            // Check that at least one compare value has been specified.
            if (searchCriteria.Values.IsEmpty())
            {
                throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: At least one compare value must be specified!");
            }

            // Check injection.
            for (index = 0; index < searchCriteria.Values.Count; index++)
            {
                searchCriteria.Values[index] = searchCriteria.Values[index].CheckInjection();
            }

            // Check that values is of expected type.
            foreach (String value in searchCriteria.Values)
            {
                switch (searchCriteria.GetDataType())
                {
                    case WebDataType.Boolean:
                        if (!Boolean.TryParse(value, out booleanValue))
                        {
                            throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Value is not of type Boolean! Value = " + value);
                        }

                        break;

                    case WebDataType.Float64:
                        if (!Double.TryParse(value, out doubleValue))
                        {
                            throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Value is not of type Float64! Value = " + value);
                        }

                        break;

                    case WebDataType.Int32:
                        if (!Int32.TryParse(value, out int32Value))
                        {
                            throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Value is not of type Int32! Value = " + value);
                        }

                        break;
                }
            }

            // Check that it is ok to use the operator with current data type.
            switch (searchCriteria.Operator)
            {
                case CompareOperator.Equal:
                case CompareOperator.Greater:
                case CompareOperator.GreaterOrEqual:
                case CompareOperator.Less:
                case CompareOperator.LessOrEqual:
                case CompareOperator.NotEqual:
                    // Ok to use these operators.
                    break;

                case CompareOperator.Like:
                case CompareOperator.BeginsWith:
                case CompareOperator.Contains:
                case CompareOperator.EndsWith:
                    if (searchCriteria.GetDataType() != WebDataType.String)
                    {
                        throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Operator " + searchCriteria.Operator.ToString().ToUpper() + " is only used with data type String! Data type = " + searchCriteria.GetDataType());
                    }

                    break;

                default:
                    throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Not supported operator! Operator = " + searchCriteria.Operator);
            }

            // Check that it is ok to use the operator with current number of compare values.
            if (searchCriteria.Values.Count > 1)
            {
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.Equal:
                    case CompareOperator.NotEqual:
                        // Ok to use these operators.
                        break;

                    default:
                        throw new ArgumentException("WebSpeciesFactFieldSearchCriteria: Not supported operator with several compare values! Operator = " + searchCriteria.Operator);
                }
            }
        }

        /// <summary>
        /// Get data type for this species factor field condition.
        /// </summary>
        /// <param name="searchCriteria">Species fact field search criteria.</param>
        /// <returns>Data type for this species factor field condition.</returns>
        private static WebDataType GetDataType(this WebSpeciesFactFieldSearchCriteria searchCriteria)
        {
            WebFactorFieldType type;

            if (searchCriteria.FactorField.IsEnumField && searchCriteria.IsEnumAsString)
            {
                return WebDataType.String;
            }
            else
            {
                type = new WebFactorFieldType();
                type.Id = searchCriteria.FactorField.TypeId;
                return type.GetDataType();
            }
        }

        /// <summary>
        /// Get compare operator as string.
        /// </summary>
        /// <param name="searchCriteria">Species fact field search criteria.</param>
        /// <returns>Compare operator as string.</returns>
        private static String GetOperator(this WebSpeciesFactFieldSearchCriteria searchCriteria)
        {
            switch (searchCriteria.Operator)
            {
                case CompareOperator.Equal:
                    if (searchCriteria.Values.Count == 1)
                    {
                        return "=";
                    }
                    else
                    {
                        // More than one value has been specified.
                        return "IN";
                    }

                case CompareOperator.Greater:
                    return ">";

                case CompareOperator.GreaterOrEqual:
                    return ">=";

                case CompareOperator.Less:
                    return "<";

                case CompareOperator.LessOrEqual:
                    return "<=";

                case CompareOperator.Like:
                case CompareOperator.BeginsWith:
                case CompareOperator.Contains:
                case CompareOperator.EndsWith:
                    return "LIKE";

                case CompareOperator.NotEqual:
                    if (searchCriteria.Values.Count == 1)
                    {
                        return "<>";
                    }
                    else
                    {
                        // More than one value has been specified.
                        return "NOT IN";
                    }

                default:
                    throw new ApplicationException("WebSpeciesFactFieldCondition, operator " + searchCriteria.Operator + " is not implemented!");
            }
        }

        /// <summary>
        /// Get SQL query where condition that matches search criteria.
        /// </summary>
        /// <param name="searchCriteria">Species fact field search criteria.</param>
        /// <returns>SQL query where condition that matches search criteria.</returns>
        public static String GetQuery(this WebSpeciesFactFieldSearchCriteria searchCriteria)
        {
            StringBuilder where;

            where = new StringBuilder();
            where.Append("(");
            where.Append("af_data.");
            where.Append(searchCriteria.FactorField.DatabaseFieldName);
            where.Append(" " + searchCriteria.GetOperator() + " ");
            where.Append(searchCriteria.GetValues());
            where.Append(")");
            return where.ToString();
        }

        /// <summary>
        /// Get values as string.
        /// </summary>
        /// <param name="searchCriteria">Species fact field search criteria.</param>
        /// <returns>Values as string.</returns>
        private static String GetValues(this WebSpeciesFactFieldSearchCriteria searchCriteria)
        {
            Boolean isFirstValue;
            StringBuilder values;

            isFirstValue = true;
            values = new StringBuilder();
            if (searchCriteria.Values.Count > 1)
            {
                values.Append("(");
            }

            foreach (String value in searchCriteria.Values)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                }
                else
                {
                    values.Append(", ");
                }

                switch (searchCriteria.GetDataType())
                {
                    case WebDataType.Boolean:
                        if (value.WebParseBoolean())
                        {
                            values.Append(1);
                        }
                        else
                        {
                            values.Append(0);
                        }

                        break;

                    case WebDataType.Float64:
                        values.Append(value.WebParseDouble());
                        break;

                    case WebDataType.Int32:
                        values.Append(value.WebParseInt32());
                        break;

                    case WebDataType.String:
                        switch (searchCriteria.Operator)
                        {
                            case CompareOperator.BeginsWith:
                                values.Append("'" + value + "%'");
                                break;
                            case CompareOperator.Contains:
                                values.Append("'%" + value + "%'");
                                break;
                            case CompareOperator.EndsWith:
                                values.Append("'%" + value + "'");
                                break;
                            default:
                                values.Append("'" + value + "'");
                                break;
                        }

                        break;

                    default:
                        throw new ApplicationException("WebSpeciesFactFieldCondition, data type " + searchCriteria.GetDataType() + " is not implemented!");
                }
            }

            if (searchCriteria.Values.Count > 1)
            {
                values.Append(")");
            }

            return values.ToString();
        }
    }
}
