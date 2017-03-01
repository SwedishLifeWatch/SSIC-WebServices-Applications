using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Database
{
    /// <summary>
    /// Class that simplifies the construction of an sql
    /// query that selects species facts.
    /// This version of the class assumes that taxa is
    /// the requested output and that these taxa are
    /// inserted into UserSelectedTaxa table.
    /// </summary>
    public class SqlSpeciesFactQueryBuilder
    {
        /// <summary>
        /// Id for operators and structurs that are used in data
        /// conditions. Data conditions are used in WebDataQuery.
        /// </summary>
        public enum ConditionType
        {
            /// <summary>
            /// And.
            /// </summary>
            And,
            /// <summary>
            /// LeftBracket.
            /// </summary>
            LeftBracket,
            /// <summary>
            /// Not.
            /// </summary>
            Not,
            /// <summary>
            /// Or.
            /// </summary>
            Or,
            /// <summary>
            /// RightBracket.
            /// </summary>
            RightBracket,
            /// <summary>
            /// SpeciesFactCondition.
            /// </summary>
            SpeciesFactCondition
        }

        /// <summary>
        /// Table af_data may be used several times in the
        /// query. Different usage of af_data is handled
        /// with the help of "AS" followed with value of the
        /// constant "TABLE_NAME" and indexed with a number.
        /// Starting with 1 and then upwards.
        /// </summary>
        private const String TABLE_NAME = "Data";
        private const String SPECIES_FACT_CONDITION_PLACE_HOLDER = "SPECIES_FACT_CONDITION";

        private Int32 _conditionIndex;
        private readonly List<ConditionType> _conditionTypes;
        private readonly List<List<Int32>> _factorIds;
        private readonly List<List<Int32>> _hostIds;
        private readonly List<List<Int32>> _individualCategoryIds;
        private readonly List<List<Int32>> _periodIds;
        private readonly List<List<WebSpeciesFactFieldCondition>> _speciesFactFieldConditions;
        private readonly StringBuilder _whereClause;

        /// <summary>
        /// Create a SqlSpeciesFactQueryBuilder instance.
        /// </summary>
        public SqlSpeciesFactQueryBuilder()
        {
            _conditionIndex = -1;
            _conditionTypes = new List<ConditionType>();
            _factorIds = new List<List<Int32>>();
            _hostIds = new List<List<Int32>>();
            _individualCategoryIds = new List<List<Int32>>();
            _periodIds = new List<List<Int32>>();
            _speciesFactFieldConditions = new List<List<WebSpeciesFactFieldCondition>>();
            _whereClause = new StringBuilder();
        }

        /// <summary>
        /// Add factor to current condition.
        /// </summary>
        /// <param name='factorId'>Factor id.</param>
        public void AddFactor(Int32 factorId)
        {
            _factorIds[_conditionIndex].Add(factorId);
        }

        /// <summary>
        /// Add host to current condition.
        /// </summary>
        /// <param name='hostId'>Host id.</param>
        public void AddHost(Int32 hostId)
        {
            _hostIds[_conditionIndex].Add(hostId);
        }

        /// <summary>
        /// Add individual category to current condition.
        /// </summary>
        /// <param name='individualCategoryId'>Individual category id.</param>
        public void AddIndividualCategory(Int32 individualCategoryId)
        {
            _individualCategoryIds[_conditionIndex].Add(individualCategoryId);
        }

        /// <summary>
        /// Add period to current condition.
        /// </summary>
        /// <param name='periodId'>Period id.</param>
        public void AddPeriod(Int32 periodId)
        {
            _periodIds[_conditionIndex].Add(periodId);
        }

        /// <summary>
        /// Add condition to the query.
        /// </summary>
        /// <param name='conditionType'>Type of condition.</param>
        public void AddQueryCondition(ConditionType conditionType)
        {
            switch (conditionType)
            {
                case ConditionType.And:
                    _whereClause.Append(" AND ");
                    break;
                case ConditionType.LeftBracket:
                    _whereClause.Append("(");
                    break;
                case ConditionType.Not:
                    _whereClause.Append(" NOT ");
                    break;
                case ConditionType.Or:
                    _whereClause.Append(" OR ");
                    break;
                case ConditionType.RightBracket:
                    _whereClause.Append(")");
                    break;
                case ConditionType.SpeciesFactCondition:
                    _conditionIndex++;
                    _conditionTypes.Add(conditionType);
                    _factorIds.Add(new List<Int32>());
                    _hostIds.Add(new List<Int32>());
                    _individualCategoryIds.Add(new List<Int32>());
                    _periodIds.Add(new List<Int32>());
                    _speciesFactFieldConditions.Add(new List<WebSpeciesFactFieldCondition>());
                    _whereClause.Append(SPECIES_FACT_CONDITION_PLACE_HOLDER + _conditionIndex);
                    break;
                default:
                    throw new NotImplementedException("Unhandled species fact condition type = " + conditionType);
            }
        }

        /// <summary>
        /// Add species fact field condition to current condition.
        /// </summary>
        /// <param name='speciesFactFieldCondition'>Species fact field condition.</param>
        public void AddSpeciesFactFieldCondition(WebSpeciesFactFieldCondition speciesFactFieldCondition)
        {
            _speciesFactFieldConditions[_conditionIndex].Add(speciesFactFieldCondition);
        }

        /// <summary>
        /// Add all species fact field conditions to the sql query.
        /// It is assumed that all conditions on the same field
        /// are of the same data type and applies to all factors.
        /// It is also assumed that data type for the condition
        /// matches the data type for the specified
        /// species fact field.
        /// These assumptions are not yet controlled.
        /// </summary>
        /// <param name="speciesFactFieldConditions">Species fact field conditions.</param>
        /// <param name="tableIndex">Index of table name in query.</param>
        /// <param name="query">Sql query.</param>
        private void GetQuery(List<WebSpeciesFactFieldCondition> speciesFactFieldConditions,
                              Int32 tableIndex,
                              StringBuilder query)
        {
            Hashtable fieldConditionTable;
            List<WebSpeciesFactFieldCondition> fieldConditions;

            // Sort species fact field conditions based on field name.
            fieldConditionTable = new Hashtable();
            if (speciesFactFieldConditions.IsNotEmpty())
            {
                foreach (WebSpeciesFactFieldCondition speciesFactFieldCondition in speciesFactFieldConditions)
                {
                    fieldConditions = (List<WebSpeciesFactFieldCondition>)(fieldConditionTable[speciesFactFieldCondition.DatabaseFieldName]);
                    if (fieldConditions.IsNull())
                    {
                        fieldConditions = new List<WebSpeciesFactFieldCondition>();
                        fieldConditionTable[speciesFactFieldCondition.DatabaseFieldName] = fieldConditions;
                    }
                    fieldConditions.Add(speciesFactFieldCondition);
                }
            }

            // Add species fact field conditions for each field.
            if (fieldConditionTable.Keys.IsNotEmpty())
            {
                foreach (String key in fieldConditionTable.Keys)
                {
                    fieldConditions = (List<WebSpeciesFactFieldCondition>)(fieldConditionTable[key]);

                    query.Append(" AND ");

                    if (fieldConditions.Count == 1)
                    {
                        query.Append(TABLE_NAME + tableIndex + "." + fieldConditions[0].DatabaseFieldName + " = ");
                        switch (fieldConditions[0].DataType)
                        {
                            case WebDataType.Boolean:
                                if (fieldConditions[0].GetBoolean())
                                {
                                    query.Append(1);
                                }
                                else
                                {
                                    query.Append(0);
                                }
                                break;
                            case WebDataType.Int32:
                                query.Append(fieldConditions[0].GetInt32());
                                break;
                            case WebDataType.String:
                                query.Append("''" + fieldConditions[0].GetString() + "''");
                                break;
                            default:
                                throw new ApplicationException("SpeciesFactCondition on data type " + fieldConditions[0].DataType + " is not implemented!");
                        }
                    }
                    else
                    {
                        query.Append(TABLE_NAME + tableIndex + "." + fieldConditions[0].DatabaseFieldName + " IN (");
                        for (Int32 valueIndex = 0; valueIndex < fieldConditions.Count; valueIndex++)
                        {
                            if (valueIndex > 0)
                            {
                                query.Append(", ");
                            }
                            switch (fieldConditions[valueIndex].DataType)
                            {
                                case WebDataType.Boolean:
                                    if (fieldConditions[valueIndex].GetBoolean())
                                    {
                                        query.Append(1);
                                    }
                                    else
                                    {
                                        query.Append(0);
                                    }
                                    break;
                                case WebDataType.Int32:
                                    query.Append(fieldConditions[valueIndex].GetInt32());
                                    break;
                                case WebDataType.String:
                                    query.Append("''" + fieldConditions[valueIndex].GetString() + "''");
                                    break;
                                default:
                                    throw new ApplicationException("SpeciesFactCondition on data type " + fieldConditions[valueIndex].DataType + " is not implemented!");
                            }
                        }
                        query.Append(")");
                    }
                }
            }
        }

        /// <summary>
        /// Generat sql query based on information set
        /// in this instance of SqlSpeciesFactQueryBuilder.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>The species fact sql query.</returns>
        public String GetQuery(WebServiceContext context)
        {
            Int32 tableIndex;
            StringBuilder query, speciesFactCondition;

            // Check conditions.
            _factorIds.CheckNotEmpty("_factorIds");
            foreach (List<Int32> tempFactorIds in _factorIds)
            {
                tempFactorIds.CheckNotEmpty("tempFactorIds");
            }

            query = new StringBuilder();

            // Add standard start of query.
            query.Append(" SELECT DISTINCT ");
            query.Append(context.RequestId + " AS RequestId, ");
            query.Append(TABLE_NAME + "1." + SpeciesFactData.TAXON_ID_COLUMN + " AS TaxonId, ");
            query.Append("''Output'' AS TaxonUsage ");
            query.Append("FROM " + SpeciesFactData.TABLE_NAME + " AS " + TABLE_NAME + "1 WITH (NOLOCK) ");

            // Add join of all conditions.
            for (Int32 conditionIndex = 1; conditionIndex < _factorIds.Count; conditionIndex++)
            {
                tableIndex = conditionIndex + 1;
                query.Append("INNER JOIN ");
                query.Append(SpeciesFactData.TABLE_NAME + " AS " + TABLE_NAME + tableIndex);
                query.Append(" WITH (NOLOCK) ON ");
                query.Append(TABLE_NAME + "1." + SpeciesFactData.TAXON_ID_COLUMN);
                query.Append(" = ");
                query.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.TAXON_ID_COLUMN + " ");
            }

            // Add where clause.
            query.Append("WHERE " + _whereClause);

            // Add each condition.
            for (Int32 conditionIndex = 0; conditionIndex < _factorIds.Count; conditionIndex++)
            {
                // Add start of condition.
                tableIndex = conditionIndex + 1;
                speciesFactCondition = new StringBuilder();
                speciesFactCondition.Append("(");

                // Add all factors.
                if (_factorIds[conditionIndex].Count == 1)
                {
                    speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.FACTOR_ID_COLUMN + " = " + _factorIds[conditionIndex][0]);
                }
                else
                {
                    speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.FACTOR_ID_COLUMN + " IN (");
                    for (Int32 factorIndex = 0; factorIndex < _factorIds[conditionIndex].Count; factorIndex++)
                    {
                        if (factorIndex > 0)
                        {
                            speciesFactCondition.Append(", ");
                        }
                        speciesFactCondition.Append(_factorIds[conditionIndex][factorIndex]);
                    }
                    speciesFactCondition.Append(")");
                }

                // Add all hosts.
                if (_hostIds[conditionIndex].IsNotEmpty())
                {
                    speciesFactCondition.Append(" AND ");
                    if (_hostIds[conditionIndex].Count == 1)
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.HOST_ID_COLUMN + " = " + _hostIds[conditionIndex][0]);
                    }
                    else
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.HOST_ID_COLUMN + " IN (");
                        for (Int32 hostIndex = 0; hostIndex < _hostIds[conditionIndex].Count; hostIndex++)
                        {
                            if (hostIndex > 0)
                            {
                                speciesFactCondition.Append(", ");
                            }
                            speciesFactCondition.Append(_hostIds[conditionIndex][hostIndex]);
                        }
                        speciesFactCondition.Append(")");
                    }
                }

                // Add individual categories.
                if (_individualCategoryIds[conditionIndex].IsNotEmpty())
                {
                    speciesFactCondition.Append(" AND ");
                    if (_individualCategoryIds[conditionIndex].Count == 1)
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.INDIVIDUAL_CATEGORY_ID_COLUMN + " = " + _individualCategoryIds[conditionIndex][0]);
                    }
                    else
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.INDIVIDUAL_CATEGORY_ID_COLUMN + " IN (");
                        for (Int32 individualCategoryIndex = 0; individualCategoryIndex < _individualCategoryIds[conditionIndex].Count; individualCategoryIndex++)
                        {
                            if (individualCategoryIndex > 0)
                            {
                                speciesFactCondition.Append(", ");
                            }
                            speciesFactCondition.Append(_individualCategoryIds[conditionIndex][individualCategoryIndex]);
                        }
                        speciesFactCondition.Append(")");
                    }
                }

                // Add all periods
                if (_periodIds[conditionIndex].IsNotEmpty())
                {
                    speciesFactCondition.Append(" AND ");

                    if (_periodIds[conditionIndex].Count == 1)
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.PERIOD_ID_COLUMN + " = " + _periodIds[conditionIndex][0]);
                    }
                    else
                    {
                        speciesFactCondition.Append(TABLE_NAME + tableIndex + "." + SpeciesFactData.PERIOD_ID_COLUMN + " IN (");
                        for (Int32 valueIndex = 0; valueIndex < _periodIds[conditionIndex].Count; valueIndex++)
                        {
                            if (valueIndex > 0)
                            {
                                speciesFactCondition.Append(", ");
                            }
                            speciesFactCondition.Append(_periodIds[conditionIndex][valueIndex]);
                        }
                        speciesFactCondition.Append(")");
                    }
                }

                // Add species fact field conditions.
                GetQuery(_speciesFactFieldConditions[conditionIndex],
                         tableIndex,
                         speciesFactCondition);

                speciesFactCondition.Append(")");
                query.Replace(SPECIES_FACT_CONDITION_PLACE_HOLDER + conditionIndex.ToString(),
                              speciesFactCondition.ToString());
            }

            return query.ToString();
        }
    }
}
