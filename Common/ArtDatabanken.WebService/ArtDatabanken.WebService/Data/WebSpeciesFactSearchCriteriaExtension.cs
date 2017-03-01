using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesFactSearchCriteria class.
    /// </summary>
    public static class WebSpeciesFactSearchCriteriaExtension
    {
        /// <summary>
        /// Choose which select part in SQL query should be used.
        /// </summary>
        public enum QuerySelectPart
        {
            QueryDefault,
            QueryTaxonCount,
            QueryTaxonIds
        }

        private static String QueryDefault
        {
            get
            {
                return @"SELECT af_data.idnr AS [Id]," +
                               " af_data.faktor AS FactorId," +
                               " af_data.taxon AS TaxonId," +
                               " af_data.IndividualCategoryId AS IndividualCategoryId," +
                               " af_data.host AS HostId," +
                               " af_data.period AS PeriodId," +
                               " af_data.tal1 AS FieldValue1," +
                               " af_data.tal2 AS FieldValue2," +
                               " af_data.tal3 AS FieldValue3," +
                               " af_data.text1 AS FieldValue4," +
                               " af_data.text2 AS FieldValue5," +
                               " af_data.quality AS QualityId," +
                               " af_data.referens AS ReferenceId," +
                               " af_data.person AS UpdateUserFullName," +
                               " af_data.datum AS UpdateDate" +
                          " FROM af_data WITH (NOLOCK)";
            }
        }

        private static String QueryTaxonCount
        {
            get
            {
                return @"SELECT COUNT(DISTINCT af_data.taxon) AS TaxonCount" +
                    " FROM af_data WITH (NOLOCK)";
            }
        }

        private static String QueryTaxonIds
        {
            get
            {
                return @"SELECT DISTINCT af_data.taxon AS TaxonId" +
                    " FROM af_data WITH (NOLOCK)";
            }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        public static void CheckData(this WebSpeciesFactSearchCriteria searchCriteria)
        {
            Int32 index;

            if (searchCriteria.FieldSearchCriteria.IsNotEmpty())
            {
                if ((searchCriteria.FieldSearchCriteria.Count > 1) &&
                    (searchCriteria.FieldLogicalOperator != LogicalOperator.And) && 
                    (searchCriteria.FieldLogicalOperator != LogicalOperator.Or))
                {
                    // Not valid operator applied to field search criteria.
                    throw new ArgumentException("WebSpeciesFactSearchCriteria: FieldLogicalOperator must be And or Or. Current value = " + searchCriteria.FieldLogicalOperator);
                }

                foreach (WebSpeciesFactFieldSearchCriteria fieldSearchCriteria in searchCriteria.FieldSearchCriteria)
                {
                    fieldSearchCriteria.CheckData();
                }
            }

            if (searchCriteria.HostIds.IsNotEmpty())
            {
                for (index = searchCriteria.HostIds.Count - 1; index >= 0; index--)
                {
                    if (searchCriteria.HostIds[index] == 0)
                    {
                        // Host id equal to zero has a special meaning in the
                        // database and can not be used in species fact search.
                        // Host id equal to zero means that no value is specified.
                        searchCriteria.HostIds.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Get SQL query that matches search criteria.
        /// </summary>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <param name="selectPart">Select part of the query.</param>
        /// <returns>SQL query that matches search criteria.</returns>
        public static String GetQuery(this WebSpeciesFactSearchCriteria searchCriteria, QuerySelectPart selectPart)
        {
            Boolean isFirstItem;
            StringBuilder query, where;

            query = new StringBuilder();
            where = new StringBuilder();

            switch (selectPart)
            {
                case QuerySelectPart.QueryDefault:
                    query.Append(QueryDefault);
                    break;
                case QuerySelectPart.QueryTaxonCount:
                    query.Append(QueryTaxonCount);
                    break;
                case QuerySelectPart.QueryTaxonIds:
                    query.Append(QueryTaxonIds);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("selectPart");
            }

            if (!searchCriteria.IncludeNotValidHosts)
            {
                query.Append(@" INNER JOIN Taxon AS ValidHosts WITH (NOLOCK) ON ValidHosts.TaxonId = af_data.host");
            }

            if (!searchCriteria.IncludeNotValidTaxa)
            {
                query.Append(@" INNER JOIN Taxon AS ValidTaxa WITH (NOLOCK) ON ValidTaxa.TaxonId = af_data.taxon");
            }

            if (searchCriteria.FactorDataTypeIds.IsNotEmpty() ||
                searchCriteria.FactorIds.IsNotEmpty())
            {
                query.Append(@" INNER JOIN #FactorIds AS InputFactors ON InputFactors.FactorId = af_data.faktor");
            }

            if (searchCriteria.FieldSearchCriteria.IsNotEmpty())
            {
                if (where.ToString().IsEmpty())
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                if (searchCriteria.FieldSearchCriteria.Count == 1)
                {
                    where.Append(searchCriteria.FieldSearchCriteria[0].GetQuery());
                }
                else
                {
                    where.Append("(");
                    isFirstItem = true;
                    foreach (WebSpeciesFactFieldSearchCriteria fieldSearchCriteria in searchCriteria.FieldSearchCriteria)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            where.Append(" ");
                            where.Append(searchCriteria.FieldLogicalOperator);
                            where.Append(" ");
                        }

                        where.Append(fieldSearchCriteria.GetQuery());
                    }

                    where.Append(")");
                }
            }

            if (searchCriteria.HostIds.IsNotEmpty())
            {
                query.Append(@" INNER JOIN #HostIds AS InputHosts ON InputHosts.HostId = af_data.host");
            }

            if (searchCriteria.IndividualCategoryIds.IsNotEmpty())
            {
                if (where.ToString().IsEmpty())
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                if (searchCriteria.IndividualCategoryIds.Count == 1)
                {
                    where.Append("(af_data.IndividualCategoryId = ");
                    where.Append(searchCriteria.IndividualCategoryIds[0]);
                    where.Append(")");
                }
                else
                {
                    isFirstItem = true;
                    where.Append("(af_data.IndividualCategoryId IN (");
                    foreach (Int32 individualCategoryId in searchCriteria.IndividualCategoryIds)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            where.Append(", ");
                        }

                        where.Append(individualCategoryId);
                    }

                    where.Append("))");
                }
            }

            if (searchCriteria.PeriodIds.IsNotEmpty())
            {
                if (where.ToString().IsEmpty())
                {
                    where.Append(" WHERE ");
                }
                else
                {
                    where.Append(" AND ");
                }

                where.Append("((af_data.period IS NULL) OR ");

                if (searchCriteria.PeriodIds.Count == 1)
                {
                    where.Append("(af_data.period = ");
                    where.Append(searchCriteria.PeriodIds[0]);
                    where.Append(")");
                }
                else
                {
                    isFirstItem = true;
                    where.Append("(af_data.period IN (");
                    foreach (Int32 periodId in searchCriteria.PeriodIds)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            where.Append(", ");
                        }

                        where.Append(periodId);
                    }

                    where.Append("))");
                }
                where.Append(")");
            }

            if (searchCriteria.TaxonIds.IsNotEmpty())
            {
                query.Append(@" INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = af_data.taxon");
            }

            return query.ToString() + where;
        }
    }
}
