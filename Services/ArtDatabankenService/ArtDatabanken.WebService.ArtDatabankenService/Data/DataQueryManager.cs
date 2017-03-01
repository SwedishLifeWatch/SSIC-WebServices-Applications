using System;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Base class that contains generic data query handling.
    /// </summary>
    public class DataQueryManager : ManagerBase
    {
        /// <summary>
        /// Move WebDataLogicCondition information into the
        /// SqlSpeciesFactQueryBuilder instance.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="logicCondition">The data logic condition.</param>
        /// <param name="queryBuilder">The query builder.</param>
        private static void GetDataLogicCondition(WebServiceContext context,
                                                  WebDataLogicCondition logicCondition,
                                                  SqlSpeciesFactQueryBuilder queryBuilder)
        {
            Int32 queryIndex;

            switch (logicCondition.Operator)
            {
                case DataLogicConditionOperatorId.And:
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
                    for (queryIndex = 0; queryIndex < logicCondition.DataQueries.Count; queryIndex++)
                    {
                        if (queryIndex > 0)
                        {
                            queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
                        }
                        GetDataQuery(context, logicCondition.DataQueries[queryIndex], queryBuilder);
                    }
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
                    break;

                case DataLogicConditionOperatorId.Not:
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Not);
                    GetDataQuery(context, logicCondition.DataQueries[0], queryBuilder);
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
                    break;

                case DataLogicConditionOperatorId.Or:
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
                    for (queryIndex = 0; queryIndex < logicCondition.DataQueries.Count; queryIndex++)
                    {
                        if (queryIndex > 0)
                        {
                            queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
                        }
                        GetDataQuery(context, logicCondition.DataQueries[queryIndex], queryBuilder);
                    }
                    queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
                    break;

                default:
                    throw new ApplicationException("Unknown logic condition operator " + logicCondition.Operator);
            }
        }

        /// <summary>
        /// Convert a WebDataQuery instance to a string that
        /// represents the query as a SQL condition.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dataQuery">The data query.</param>
        /// <returns>A SQL condition string.</returns>
        protected static String GetDataQuery(WebServiceContext context,
                                             WebDataQuery dataQuery)
        {
            SqlSpeciesFactQueryBuilder queryBuilder;

            queryBuilder = new SqlSpeciesFactQueryBuilder();
            GetDataQuery(context, dataQuery, queryBuilder);
            return queryBuilder.GetQuery(context);
        }

        /// <summary>
        /// Move WebDataQuery information into the
        /// SqlSpeciesFactQueryBuilder instance.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="queryBuilder">The query builder.</param>
        private static void GetDataQuery(WebServiceContext context,
                                         WebDataQuery dataQuery,
                                         SqlSpeciesFactQueryBuilder queryBuilder)
        {
            if (dataQuery.DataCondition.IsNotNull())
            {
                if (dataQuery.DataCondition.DataLogicCondition.IsNotNull())
                {
                    GetDataLogicCondition(context, dataQuery.DataCondition.DataLogicCondition, queryBuilder);
                }
                if (dataQuery.DataCondition.SpeciesFactCondition.IsNotNull())
                {
                    GetSpeciesFactCondition(context, dataQuery.DataCondition.SpeciesFactCondition, queryBuilder);
                }
            }
            if (dataQuery.DataConversion.IsNotNull())
            {
                throw new NotImplementedException("Conversion of data type in data query is not implemented!");
            }
            if (dataQuery.DataLimitation.IsNotNull())
            {
                throw new NotImplementedException("Limitation of data scope in data query is not implemented!");
            }
        }

        /// <summary>
        /// Move WebSpeciesFactCondition information into the
        /// SqlSpeciesFactQueryBuilder instance.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactCondition">The species fact condition.</param>
        /// <param name="queryBuilder">The query builder.</param>
        private static void GetSpeciesFactCondition(WebServiceContext context,
                                                    WebSpeciesFactCondition speciesFactCondition,
                                                    SqlSpeciesFactQueryBuilder queryBuilder)
        {
            // Start new species fact condition.
            queryBuilder.AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);

            // Add all factors.
            if (speciesFactCondition.Factors.IsNotEmpty())
            {
                foreach (WebFactor factor in speciesFactCondition.Factors)
                {
                    queryBuilder.AddFactor(factor.Id);
                }
            }

            // Add all hosts.
            if (speciesFactCondition.HostIds.IsNotEmpty())
            {
                foreach (Int32 hostId in speciesFactCondition.HostIds)
                {
                    queryBuilder.AddHost(hostId);
                }
            }

            // Add all individual categories.
            if (speciesFactCondition.IndividualCategories.IsNotEmpty())
            {
                foreach (WebIndividualCategory individualCategory in speciesFactCondition.IndividualCategories)
                {
                    queryBuilder.AddIndividualCategory(individualCategory.Id);
                }
            }

            // Add all periods.
            if (speciesFactCondition.Periods.IsNotEmpty() &&
                speciesFactCondition.Factors.IsNotEmpty() &&
                speciesFactCondition.Factors[0].IsPeriodic)
            {
                foreach (WebPeriod period in speciesFactCondition.Periods)
                {
                    queryBuilder.AddPeriod(period.Id);
                }
            }

            // Add all species fact field conditions.
            if (speciesFactCondition.SpeciesFactFieldConditions.IsNotEmpty())
            {
                foreach (WebSpeciesFactFieldCondition speciesFactFieldCondition in speciesFactCondition.SpeciesFactFieldConditions)
                {
                    queryBuilder.AddSpeciesFactFieldCondition(speciesFactFieldCondition);
                }
            }

            // Add all taxons.
            if (speciesFactCondition.TaxonIds.IsNotEmpty())
            {
                throw new NotImplementedException("Limitation on taxa in species fact condition is not implemented!");
            }
        }
    }
}
