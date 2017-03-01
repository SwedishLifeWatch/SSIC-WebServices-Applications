using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Log manager for harvest service.
    /// </summary>
    public class LogManager : WebService.Data.LogManager
    {
        /// <summary>
        /// Get log specific for harvest service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="type">Get log rows of this type.</param>
        /// <param name="userName">Get log rows for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log rows to get.</param>
        /// <returns> Requested web log rows.</returns>
        public override List<WebLogRow> GetLog(WebServiceContext context, LogType type, string userName, int rowCount)
        {
            List<WebLogRow> logRows;
            String typeString;
            WebLogRow logRow;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);

            // Check arguments.
            if (type == LogType.SpeciesObservationStatistic)
            {
                typeString = type.ToString();
                userName = userName.CheckInjection();

                // Get information from database.
                logRows = new List<WebLogRow>();
                if (rowCount > 0)
                {
                    using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetLogStatistics(rowCount))
                    {
                        while (dataReader.Read())
                        {
                            logRow = new WebLogRow();
                            logRow.LoadData(dataReader);
                            logRows.Add(logRow);
                        }
                    }
                }

                return logRows;
            }

            if (type == LogType.SpeciesObservationUpdate)
            {
                typeString = type.ToString();
                userName = userName.CheckInjection();

                // Get information from database.
                logRows = new List<WebLogRow>();
                if (rowCount > 0)
                {
                    using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetLogUpdateError(rowCount))
                    {
                        while (dataReader.Read())
                        {
                            logRow = new WebLogRow();
                            logRow.LoadData(dataReader);
                            logRows.Add(logRow);
                        }
                    }
                }

                return logRows;
            }

            return base.GetLog(context, type, userName, rowCount);
        }
    }
}
