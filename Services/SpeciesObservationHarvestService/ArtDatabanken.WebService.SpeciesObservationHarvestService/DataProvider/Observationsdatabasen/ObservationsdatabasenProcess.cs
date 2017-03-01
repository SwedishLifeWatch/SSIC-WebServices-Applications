using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen
{
    public class ObservationsdatabasenProcess : BaseProcess
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
                case "CreateOccurrenceID":
                    returnValue = CreateOccurrenceId(dictionaryWebData);
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
                case "GetCollectionCode":
                    returnValue = GetCollectionCode(dictionaryWebData, "collectioncode");
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
                case "GetLocality":
                    returnValue = GetLocality(dictionaryWebData);
                    break;
                case "GetModifiedDate":
                    returnValue = GetModifiedDate(dictionaryWebData, "editdate", "registerdate");
                    break;
                case "GetProtectionLevel":
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;
                case "GetRights":
                    returnValue = GetRights(dictionaryWebData);
                    break;

            }
            return returnValue;
        }

        /// <summary>
        /// Get the observation id that should be deleted from the database.
        /// </summary>
        /// <param name="idFromDataProvider">Observation id as returned from the data provider.</param>
        /// <returns>Observation id</returns>
        protected override string GetDeletedObservationId(string idFromDataProvider)
        {
            return CreateOccurrenceId(idFromDataProvider);
        }

        // -- Methods call to set calculated or fixed values


        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>OccurrenceId</returns>
        private String CreateOccurrenceId(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String id = dictionaryWebData["id"].Value;
            //todo: felhantering om id = null?
            return CreateOccurrenceId(id);
        }

        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="observationId">String with observationId.</param>
        /// <returns>OccurrenceId</returns>
        private String CreateOccurrenceId(String observationId)
        {
            return "urn:lsid:observationsdatabasen.se:Sighting:" + observationId;
        }

        /// <summary>
        /// Get locality value
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>Locality or parish</returns>
        private String GetLocality(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String locality = dictionaryWebData["locality"].Value;

            if (locality.IsEmpty())
                locality = dictionaryWebData["parish"].Value;

            return locality;
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
        /// OBSERVATIONSDATABASEN
        /// CoordinateSystemId.Rt90_25_gon_v
        /// coordinatex, coordinatey
        /// Converts the coordinates found in the dicitionaryWebData to all the coordinate systems listed in the <see cref="CoordinateSystemId"/> enumeration.
        /// </summary>
        /// <param name="dictionaryWebData"></param>
        protected override void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData, "coordinatex", "coordinatey", CoordinateSystemId.Rt90_25_gon_v);
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
            Dictionary<Int32, TaxonInformation> taxonInformation;
            Int32 taxonId;

            taxonId = dictionaryWebData["taxonid"].Value.WebParseInt32();
            taxonInformation = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);

            if (!taxonInformation.ContainsKey(taxonId) ||
                taxonInformation[taxonId].IsNull())
            {
                return null;
            }

            if (taxonInformation[taxonId].ProtectionLevel < 4)
            {
                return "3";
            }
            else
            {
                return taxonInformation[taxonId].ProtectionLevel.WebToString();
            }
        }
    }
}
