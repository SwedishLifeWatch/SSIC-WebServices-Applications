using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Shark
{
    /// <summary>
    /// The Shark process.
    /// </summary>
    public class SharkProcess : BaseProcess
    {
        /// <summary>
        /// Wrapper to method call.
        /// </summary>
        /// <param name="methodName">Name of the method to execute.</param>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context">Web service context.</param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>String value as returned from the method that was being called.</returns>
        protected override String MethodWrapper(String methodName,
                                                Dictionary<string, WebDataField> dictionaryWebData,
                                                WebServiceContext context,
                                                HarvestMapping mapping)
        {
            String returnValue = String.Empty;

            switch (methodName)
            {
                case "GetCoordinateX":
                    returnValue = GetCoordinateX(dictionaryWebData);
                    break;
                case "GetCoordinateX_RT90":
                    returnValue = GetCoordinateX_RT90_25_gon_v(dictionaryWebData);
                    break;
                case "GetCoordinateX_SWEREF99":
                    returnValue = GetCoordinateX_SWEREF99_TM(dictionaryWebData);
                    break;
                case "GetCoordinateY":
                    returnValue = GetCoordinateY(dictionaryWebData);
                    break;
                case "GetCoordinateY_RT90":
                    returnValue = GetCoordinateY_RT90_25_gon_v(dictionaryWebData);
                    break;
                case "GetCoordinateY_SWEREF99":
                    returnValue = GetCoordinateY_SWEREF99_TM(dictionaryWebData);
                    break;
                case "CreateEventTime":
                    returnValue = CreateEventTime(dictionaryWebData);
                    break;
                case "GetCatalogNumber":
                    returnValue = GetCatalogNumber(dictionaryWebData);
                    break;
                case "GetYear":
                    if (dictionaryWebData["eventdate"].GetString().IsDateTime())
                    {
                        returnValue = dictionaryWebData["eventdate"].GetString().WebParseDateTime().Year.WebToString();
                    }

                    break;
                case "GetMonth":
                    if (dictionaryWebData["eventdate"].GetString().IsDateTime())
                    {
                        returnValue = dictionaryWebData["eventdate"].GetString().WebParseDateTime().Month.WebToString();
                    }

                    break;
                case "GetDay":
                    if (dictionaryWebData["eventdate"].GetString().IsDateTime())
                    {
                        returnValue = dictionaryWebData["eventdate"].GetString().WebParseDateTime().Day.WebToString();
                    }

                    break;
                case "GetEndDayOfYear":
                    if (dictionaryWebData["eventdate"].GetString().IsDateTime())
                    {
                        returnValue = dictionaryWebData["eventdate"].GetString().WebParseDateTime().DayOfYear.WebToString();
                    }

                    break;
                case "GetProtectionLevel":
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;
                case "GetStartDayOfYear":
                    if (dictionaryWebData["eventdate"].GetString().IsDateTime())
                    {
                        returnValue = dictionaryWebData["eventdate"].GetString().WebParseDateTime().DayOfYear.WebToString();
                    }

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// The get dyntaxa taxon id.
        /// </summary>
        /// <param name="speciesObservationFields">
        /// The dictionary web data.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The dyntaxa taxon id.<see cref="string"/>.
        /// </returns>
        protected override String GetDyntaxaTaxonId(Dictionary<string, WebDataField> speciesObservationFields, WebServiceContext context)
        {
            return speciesObservationFields.FirstOrDefault(item => item.Key.ToLower() == "dyntaxataxonid").Value.Value;
        }

        /// <summary>
        /// The protection level based on dyntaxa taxon id.
        /// </summary>
        /// <param name="dictionaryWebData">Species observation data.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Protection level based on dyntaxa taxon id.</returns>
        protected override string GetProtectionLevel(Dictionary<String, WebDataField> dictionaryWebData, WebServiceContext context)
        {
            var taxonId = dictionaryWebData["dyntaxataxonid"];
            if (taxonId == null || !taxonId.Value.IsInteger())
            {
                return null;
            }

            try
            {
                return base.GetProtectionLevel(dictionaryWebData, context);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the observation id that should be deleted from the database.
        /// </summary>
        /// <param name="idFromDataProvider">Observation id as returned from the data provider.</param>
        /// <returns>Observation id.</returns>
        protected override string GetDeletedObservationId(string idFromDataProvider)
        {
            return CreateOccurrenceId(idFromDataProvider);
        }

        /// <summary>
        /// Gets value for positive observation.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>String "0" if Dyntaxa Taxon Id is 0.</returns>
        protected override String GetIsPositiveObservation(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String dyntaxaTaxonId = dictionaryWebData["dyntaxataxonid"].Value;

            if (dyntaxaTaxonId.IsNull() || dyntaxaTaxonId == "0")
            {
                return "0";
            }

            return "1";
        }

        /// <summary>
        /// Gets the occurrence status.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>Returns "Present" or "Absent depending on if any of the properties 
        /// IsNeverFoundObservation or IsNotRediscoveredObservation indicating absence is true.</returns>
        protected override String GetOccurrenceStatus(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String dyntaxaTaxonId = dictionaryWebData["dyntaxataxonid"].Value;

            if (dyntaxaTaxonId.IsNull() || dyntaxaTaxonId == "0")
            {
                return "Absent";
            }

            return "Present";
        }


        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="observationId">String with observationId.</param>
        /// <returns>The OccurrenceId.</returns>
        private static String CreateOccurrenceId(String observationId)
        {
            return "urn:lsid:shark:Sighting:" + observationId;
        }

        /// <summary>
        /// Create catalog number by last part of the occurrence id.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The Catalog Number.</returns>
        private static String GetCatalogNumber(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String id = dictionaryWebData["occurrenceid"].Value;
            Int32 pos = id.LastIndexOf(":", StringComparison.Ordinal);
            return id.Substring(pos + 1);
        }

        /// <summary>
        /// Creates the event time.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event time in DarwinCore format.</returns>
        protected override String CreateEventTime(Dictionary<string, WebDataField> dictionaryWebData)
        {
            string returnDateTime = "error";

            try
            {
                if (dictionaryWebData.ContainsKey("eventdate") && dictionaryWebData["eventdate"].Value.IsNotNull())
                {
                    returnDateTime = dictionaryWebData["eventdate"].GetString().WebParseDateTime().TimeOfDay.ToString();
                }
            }
            catch
            {
                returnDateTime = "error";
            }

            return returnDateTime;
        }

        /// <summary>
        /// Checks if the DyntaxaTaxonId in the row exists in the Taxon table with isValid set to true
        /// </summary>
        /// <param name="row"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsDyntaxaTaxonValid(WebData row, WebServiceContext context)
        {
            return !string.IsNullOrEmpty(GetTaxonIdByQueryString(context, " AND currentDyntaxaTaxonId=" + row.DataFields.FirstOrDefault(item => item.Name.ToLower() == "dyntaxataxonid").Value));
        }
    }
}
