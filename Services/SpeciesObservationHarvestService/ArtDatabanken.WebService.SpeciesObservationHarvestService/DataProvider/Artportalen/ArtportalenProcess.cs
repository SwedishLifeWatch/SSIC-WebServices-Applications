using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen
{
    /// <summary>
    /// Process of Artportalen.
    /// </summary>
    public class ArtportalenProcess : BaseProcess
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
                case "CreateEventDate":
                    returnValue = CreateEventDate(dictionaryWebData);
                    break;
                case "CreateEventTime":
                    returnValue = CreateEventTime(dictionaryWebData);
                    break;
                case "CreateOccurrenceID":
                    returnValue = CreateOccurrenceId(dictionaryWebData);
                    break;
                case "GetIsPositiveObservation":
                    returnValue = GetIsPositiveObservation(dictionaryWebData);
                    break;
                case "GetOccurrenceStatus":
                    returnValue = GetOccurrenceStatus(dictionaryWebData);
                    break;
                case "GetCollectionCode":
                    returnValue = GetCollectionCode(dictionaryWebData, "collection");
                    break;
                case "GetCoordinateX":
                    returnValue = GetCoordinateX(dictionaryWebData);
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
                case "GetCoordinateY":
                    returnValue = GetCoordinateY(dictionaryWebData);
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
                case "GetYear":
                    returnValue = dictionaryWebData["enddate"].GetDateTime().Year.WebToString();
                    break;
                case "GetMonth":
                    returnValue = dictionaryWebData["enddate"].GetDateTime().Month.WebToString();
                    break;
                case "GetDay":
                    returnValue = dictionaryWebData["enddate"].GetDateTime().Day.WebToString();
                    break;
                case "GetEndDayOfYear":
                    returnValue = dictionaryWebData["enddate"].GetDateTime().DayOfYear.WebToString();
                    break;
                case "GetStartDayOfYear":
                    returnValue = dictionaryWebData["startdate"].GetDateTime().DayOfYear.WebToString();
                    break;
                case "GetModifiedDate":
                    returnValue = GetModifiedDate(dictionaryWebData, "editdate", "registerdate");
                    break;
                case "GetBirdNestActivityId":
                    returnValue = GetBirdNestActivityId(dictionaryWebData, context);
                    break;
                case "GetProtectionLevel":
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;
            }

            return returnValue;
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
            return "urn:lsid:artportalen.se:Sighting:" + observationId;
        }

        /// <summary>
        /// Returns true if the dicitionaryWebData instance is the same as last time this function was called (uses the property CurrentRecord to keep track)
        /// This instance uses "id" value to establish uniqueness
        /// </summary>
        /// <param name="dictionaryWebData">Dictionary of the current read record.</param>
        /// <returns></returns>
        protected override bool IsCurrentRecord(Dictionary<string, WebDataField> dictionaryWebData)
        {
            const string value = "id";
            if (CurrentRecord == dictionaryWebData[value].Value)
            {
                return true;
            }

            CurrentRecord = dictionaryWebData[value].Value;
            return false;
        }

        /// <summary>
        /// ARTPORTALEN
        /// CoordinateSystemId.WGS84
        /// longitude, latitude.
        /// Converts the coordinates found in the dicitionaryWebData to all the coordinate systems listed in the <see cref="CoordinateSystemId"/> enumeration.
        /// </summary>
        /// <param name="dictionaryWebData"></param>
        protected override void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData, "longitude", "latitude", CoordinateSystemId.WGS84);
        }

        /// <summary>
        /// Get the observation id that should be deleted from the database.
        /// </summary>
        /// <param name="speciesObservationFields">The dictionaryWebData.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Observation id.</returns>
        protected override string GetDyntaxaTaxonId(Dictionary<String, WebDataField> speciesObservationFields,
                                                    WebServiceContext context)
        {
            String dyntaxaTaxonId;

            dyntaxaTaxonId = speciesObservationFields["taxonid"].Value;

            return dyntaxaTaxonId;
        }

        /// <summary>
        /// The protection level based on dyntaxa taxon id.
        /// </summary>
        /// <param name="dictionaryWebData">Species observation data.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Protection level based on dyntaxa taxon id.</returns>
        protected override String GetProtectionLevel(Dictionary<String, WebDataField> dictionaryWebData,
                                                     WebServiceContext context)
        {
            String artportalenProtectionLevel, taxonProtectionLevel;

            artportalenProtectionLevel = dictionaryWebData["protectionlevel"].Value;
            taxonProtectionLevel = base.GetProtectionLevel(dictionaryWebData, context);
            if (taxonProtectionLevel.IsEmpty())
            {
                taxonProtectionLevel = "1";
            }

            if ((artportalenProtectionLevel == "3") &&
                (taxonProtectionLevel.WebParseInt32() < 3))
            {
                // May be a species observation that has been
                // hidden by provider.
                return artportalenProtectionLevel;
            }
            else
            {
                return taxonProtectionLevel;
            }
        }
    }
}
