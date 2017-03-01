using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Base class that contains generic data query handling.
    /// </summary>
    public class DataQueryManager : ManagerBase
    {
        /// <summary>
        /// Convert an DataAndCondition to a WebDataLogicCondition.
        /// </summary>
        /// <param name="dataAndCondition">The data and condition.</param>
        /// <returns>A WebDataLogicCondition.</returns>
        private static WebDataLogicCondition GetDataAndCondition(DataAndCondition dataAndCondition)
        {
            WebDataLogicCondition webDataLogicCondition;

            webDataLogicCondition = new WebDataLogicCondition();
            webDataLogicCondition.Operator = DataLogicConditionOperatorId.And;
#if DATA_SPECIFIED_EXISTS
            webDataLogicCondition.OperatorSpecified = true;
#endif
            webDataLogicCondition.DataQueries = new List<WebDataQuery>();
            foreach (IDataQuery dataQuery in dataAndCondition.DataQueries)
            {
                webDataLogicCondition.DataQueries.Add(GetDataQuery(dataQuery));
            }

            return webDataLogicCondition;
        }

        /// <summary>
        /// Convert an DataOrCondition to a WebDataLogicCondition.
        /// </summary>
        /// <param name="dataOrCondition">The data or condition.</param>
        /// <returns>A WebDataLogicCondition.</returns>
        private static WebDataLogicCondition GetDataOrCondition(DataOrCondition dataOrCondition)
        {
            WebDataLogicCondition webDataLogicCondition;

            webDataLogicCondition = new WebDataLogicCondition();
            webDataLogicCondition.Operator = DataLogicConditionOperatorId.Or;
#if DATA_SPECIFIED_EXISTS
            webDataLogicCondition.OperatorSpecified = true;
#endif
            webDataLogicCondition.DataQueries = new List<WebDataQuery>();
            foreach (IDataQuery dataQuery in dataOrCondition.DataQueries)
            {
                webDataLogicCondition.DataQueries.Add(GetDataQuery(dataQuery));
            }

            return webDataLogicCondition;
        }

        /// <summary>
        /// Convert an IDataQuery to a WebDataQuery.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <returns>A WebDataQuery.</returns>
        protected static WebDataQuery GetDataQuery(IDataQuery dataQuery)
        {
            WebDataLogicCondition webDataLogicCondition;
            WebDataCondition webDataCondition;
            WebDataQuery webDataQuery;
            WebSpeciesFactCondition webSpeciesFactCondition;

            switch (dataQuery.Type)
            {
                case DataQueryType.AndCondition:
                    webDataLogicCondition = GetDataAndCondition((DataAndCondition)dataQuery);
                    webDataCondition = new WebDataCondition();
                    webDataCondition.DataLogicCondition = webDataLogicCondition;
                    webDataQuery = new WebDataQuery();
                    webDataQuery.DataCondition = webDataCondition;
                    break;
                case DataQueryType.OrCondition:
                    webDataLogicCondition = GetDataOrCondition((DataOrCondition)dataQuery);
                    webDataCondition = new WebDataCondition();
                    webDataCondition.DataLogicCondition = webDataLogicCondition;
                    webDataQuery = new WebDataQuery();
                    webDataQuery.DataCondition = webDataCondition;
                    break;
                case DataQueryType.SpeciesFactCondition:
                    webSpeciesFactCondition = GetSpeciesFactCondition((SpeciesFactCondition)dataQuery);
                    webDataCondition = new WebDataCondition();
                    webDataCondition.SpeciesFactCondition = webSpeciesFactCondition;
                    webDataQuery = new WebDataQuery();
                    webDataQuery.DataCondition = webDataCondition;
                    break;
                default:
                    throw new ApplicationException("Handling of data query type '" + dataQuery.Type + "' is not implemeted!");
            }
            return webDataQuery;
        }

        /// <summary>
        /// Convert an SpeciesFactCondition to a WebSpeciesFactCondition.
        /// </summary>
        /// <param name="speciesFactCondition">The species fact condition.</param>
        /// <returns>A WebSpeciesFactCondition.</returns>
        private static WebSpeciesFactCondition GetSpeciesFactCondition(SpeciesFactCondition speciesFactCondition)
        {
            WebSpeciesFactCondition webSpeciesFactCondition;

            webSpeciesFactCondition = new WebSpeciesFactCondition();
            
            // Add all factors.
            if (speciesFactCondition.Factors.IsNotEmpty())
            {
                webSpeciesFactCondition.Factors = new List<WebFactor>();
                foreach (Factor factor in speciesFactCondition.Factors)
                {
                    webSpeciesFactCondition.Factors.Add(FactorManager.GetFactor(factor));
                }
            }

            // Add all periods.
            if (speciesFactCondition.Periods.IsNotEmpty())
            {
                webSpeciesFactCondition.Periods = new List<WebPeriod>();
                foreach (Period period in speciesFactCondition.Periods)
                {
                    webSpeciesFactCondition.Periods.Add(PeriodManager.GetPeriod(period));
                }
            }

            // Add all individual categories.
            if (speciesFactCondition.IndividualCategories.IsNotEmpty())
            {
                webSpeciesFactCondition.IndividualCategories = new List<WebIndividualCategory>();
                foreach (IndividualCategory individualCategory in speciesFactCondition.IndividualCategories)
                {
                    webSpeciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetIndividualCategory(individualCategory));
                }
            }

            // Add all species fact field conditions.
            if (speciesFactCondition.SpeciesFactFieldConditions.IsNotEmpty())
            {
                webSpeciesFactCondition.SpeciesFactFieldConditions = new List<WebSpeciesFactFieldCondition>();
                foreach (SpeciesFactFieldCondition speciesFactFieldCondition in speciesFactCondition.SpeciesFactFieldConditions)
                {
                    webSpeciesFactCondition.SpeciesFactFieldConditions.Add(GetSpeciesFactFieldCondition(speciesFactFieldCondition));
                }
            }

            return webSpeciesFactCondition;
        }

        /// <summary>
        /// Convert an SpeciesFactFieldCondition to a WebSpeciesFactFieldCondition.
        /// </summary>
        /// <param name="speciesFactFieldCondition">The species fact field condition.</param>
        /// <returns>A WebSpeciesFactFieldCondition.</returns>
        private static WebSpeciesFactFieldCondition GetSpeciesFactFieldCondition(SpeciesFactFieldCondition speciesFactFieldCondition)
        {
            WebSpeciesFactFieldCondition webSpeciesFactFieldCondition;

            webSpeciesFactFieldCondition = new WebSpeciesFactFieldCondition();
            webSpeciesFactFieldCondition.FactorField = FactorManager.GetFactorField(speciesFactFieldCondition.FactorField);
            webSpeciesFactFieldCondition.IsEnumAsString = speciesFactFieldCondition.IsEnumAsString;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFactFieldCondition.IsEnumAsStringSpecified = true;
#endif
            webSpeciesFactFieldCondition.Operator = speciesFactFieldCondition.Operator;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFactFieldCondition.OperatorSpecified = true;
#endif
            webSpeciesFactFieldCondition.Value = speciesFactFieldCondition.Value;

            return webSpeciesFactFieldCondition;
        }
    }
}
