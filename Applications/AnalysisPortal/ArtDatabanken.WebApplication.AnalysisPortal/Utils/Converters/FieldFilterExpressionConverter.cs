using System;
using System.Collections.ObjectModel;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters
{
    /// <summary>
    /// Handle search criteria's field search criteria.
    /// </summary>
    public static class FieldFilterExpressionConverter
    {
        /// <summary>
        /// Get field filter expression by field search criteria.
        /// </summary>
        /// <returns></returns>
        public static String GetFieldFilterExpression(ObservableCollection<SpeciesObservationFieldSearchCriteria> searchCriterias, LogicalOperator logicalOperator)
        {
            StringBuilder fieldFilterExpression = new StringBuilder();
            String filterValue = String.Empty;
            String filterExpression = String.Empty;

            foreach (var searchCriteria in searchCriterias)
            {
                if (fieldFilterExpression.Length > 0)
                {
                    fieldFilterExpression.Append(" " + logicalOperator.ToString().ToUpper() + " ");
                }

                fieldFilterExpression.Append(searchCriteria.Property.GetName());
                filterValue = searchCriteria.Value;

                switch (searchCriteria.Operator)
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
                
                // Apply cituation signs
                if (searchCriteria.Type == DataType.String)
                {
                    filterExpression = String.Format(filterExpression, "'", filterValue, "'");
                }
                else
                {
                    filterExpression = String.Format(filterExpression, String.Empty, filterValue, String.Empty);
                }

                fieldFilterExpression.Append(filterExpression);
            }

           return fieldFilterExpression.ToString();
        }
    }
}
