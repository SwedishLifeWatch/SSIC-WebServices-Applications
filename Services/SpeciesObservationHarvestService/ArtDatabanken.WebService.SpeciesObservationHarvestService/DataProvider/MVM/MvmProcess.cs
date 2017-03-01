using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Mvm
{
    /// <summary>
    /// Handle Mvm process methods.
    /// </summary>
    public class MvmProcess : BaseProcess
    {
        /// <summary>
        /// The get dyntaxa taxon id.
        /// </summary>
        /// <param name="speciesObservationFields">Species observation fields.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Dyntaxa taxon id.</returns>
        protected override String GetDyntaxaTaxonId(Dictionary<string, WebDataField> speciesObservationFields,
                                                    WebServiceContext context)
        {
            String queryString = " AND dyntaxaTaxonId=" + speciesObservationFields["dyntaxataxonid"].Value;

            return GetTaxonIdByQueryString(context, queryString);
        }

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
                case "GetDyntaxaTaxonId":
                    returnValue = GetDyntaxaTaxonId(dictionaryWebData, context);
                    break;

                case "FindTaxonID":
                    returnValue = FindTaxonId(dictionaryWebData, context);
                    break;
                case "CreateOccurrenceID":
                    returnValue = CreateOccurrenceId(dictionaryWebData);
                    break;
                case "CreateEventDate":
                    returnValue = CreateEventDate(dictionaryWebData, "start", "end", "start", "end");
                    break;

                case "GetRecordedBy":
                    returnValue = GetRecordedBy(dictionaryWebData);
                    break;

                case "GetProtectionLevel":
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;

                case "GetCoordinateX":
                    returnValue = GetCoordinateX(dictionaryWebData);
                    break;
                case "GetCoordinateY":
                    returnValue = GetCoordinateY(dictionaryWebData);
                    break;
                case "CreateEventTime":
                    returnValue = CreateEventTime(dictionaryWebData);
                    break;

                case "CreateInstitutionId":
                    returnValue = "urn:lsid:artdata.slu.se:organization:" + dictionaryWebData["id"].Value.Trim();
                    break;
                case "GetBirdNestActivityId":
                    returnValue = GetBirdNestActivityId(dictionaryWebData, context);
                    break;
                case "GetCollectionCode":
                    returnValue = GetCollectionCode(dictionaryWebData, "collection");
                    break;

                case "GetCoordinateX_RT90":
                    returnValue = GetCoordinateX_RT90_25_gon_v(dictionaryWebData);
                    break;
                case "GetCoordinateX_SWEREF99":
                    returnValue = GetCoordinateX_SWEREF99_TM(dictionaryWebData);
                    break;
                case "GetCoordinateX_ETRS89_LAEA":
                    returnValue = GetCoordinateX_ETRS89_LAEA(dictionaryWebData);
                    break;

                case "GetCoordinateY_RT90":
                    returnValue = GetCoordinateY_RT90_25_gon_v(dictionaryWebData);
                    break;
                case "GetCoordinateY_SWEREF99":
                    returnValue = GetCoordinateY_SWEREF99_TM(dictionaryWebData);
                    break;
                case "GetCoordinateY_ETRS89_LAEA":
                    returnValue = GetCoordinateY_ETRS89_LAEA(dictionaryWebData);
                    break;

                case "GetDecimalLongitude":
                    returnValue = GetDecimalLongitude(dictionaryWebData);
                    break;
                case "GetDecimalLatitude":
                    returnValue = GetDecimalLatitude(dictionaryWebData);
                    break;

                case "GetIsPositiveObservation":
                    returnValue = GetIsPositiveObservation(dictionaryWebData);
                    break;
                case "GetOccurrenceStatus":
                    returnValue = GetOccurrenceStatus(dictionaryWebData);
                    break;
                case "GetRights":
                    returnValue = GetRights(dictionaryWebData);
                    break;

                case "GetYear":
                    returnValue = dictionaryWebData["end"].GetDateTime().Year.WebToString();
                    break;
                case "GetMonth":
                    returnValue = dictionaryWebData["end"].GetDateTime().Month.WebToString();
                    break;
                case "GetDay":
                    returnValue = dictionaryWebData["end"].GetDateTime().Day.WebToString();
                    break;
                case "GetEndDayOfYear":
                    returnValue = dictionaryWebData["end"].GetDateTime().DayOfYear.WebToString();
                    break;
                case "GetStartDayOfYear":
                    returnValue = dictionaryWebData["start"].GetDateTime().DayOfYear.WebToString();
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Get RecordedBy value from web data object. This is the name of the observer.
        /// </summary>
        /// <param name="dictionaryWebData">Contains arbitrary data.</param>
        /// <returns>Returns RecordedBy value or "MVM, na" if missing.</returns>
        private string GetRecordedBy(Dictionary<string, WebDataField> dictionaryWebData)
        {
            if (dictionaryWebData.ContainsKey("recordedby") &&
                dictionaryWebData["recordedby"].Value.IsNotEmpty())
            {
                return dictionaryWebData["recordedby"].Value;
            }

            return "MVM, na";
        }

        /// <summary>
        /// Get taxon id in species observation database by value in web data object.
        /// </summary>
        /// <param name="dictionaryWebData">Contains arbitrary data.</param>
        /// <param name="context">Web service context.</param>
        /// <returns>Returns taxon id.</returns>
        private string FindTaxonId(Dictionary<string, WebDataField> dictionaryWebData, WebServiceContext context)
        {
            String scientificName = String.Empty;
            if (dictionaryWebData.ContainsKey("scientificname"))
            {
                scientificName = dictionaryWebData["scientificname"].Value;
            }

            string calculatedTaxonId = context.GetDatabase().GetTaxonIdByScientificName(scientificName).WebToString();

            return calculatedTaxonId;
        }

        /// <summary>
        /// Returns true if the dicitionaryWebData instance is the same as last time this function was called (uses the property CurrentRecord to keep track)
        /// This instance uses "catalognumber" value to establish uniqueness
        /// </summary>
        /// <param name="dictionaryWebData">Dictionary of the current read record.</param>
        /// <returns></returns>
        protected override bool IsCurrentRecord(Dictionary<string, WebDataField> dictionaryWebData)
        {
            const string value = "catalognumber";
            if (CurrentRecord == dictionaryWebData[value].Value)
            {
                return true;
            }

            CurrentRecord = dictionaryWebData[value].Value;
            return false;
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

        // -- Methods called to set calculated values

        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>Occurrence id.</returns>
        private static String CreateOccurrenceId(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String id = dictionaryWebData["id"].Value;

            return CreateOccurrenceId(id);
        }

        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="observationId">String with observationId.</param>
        /// <returns>Occurrence id.</returns>
        private static String CreateOccurrenceId(String observationId)
        {
            return "urn:lsid:mvm:Sighting:" + observationId;
        }
    }
}
