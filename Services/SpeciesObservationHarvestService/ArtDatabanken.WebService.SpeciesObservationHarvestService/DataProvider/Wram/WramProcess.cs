using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Wram
{
    /// <summary>
    /// The WRAM process.
    /// </summary>
    public class WramProcess : BaseProcess
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
                case "GetProtectionLevel":
                    //throw new NotImplementedException("Test this code before usage.");
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;
                case "CreateEventDate":
                    returnValue = CreateEventDate(dictionaryWebData, "start", "end", "start", "end");
                    break;
                case "CreateEventTime":
                    returnValue = CreateEventTime(dictionaryWebData);
                    break;
                case "CreateInstitutionId":
                    returnValue = "urn:lsid:artdata.slu.se:organization:" + dictionaryWebData["id"].Value.Trim();
                    break;
                case "GetCatalogNumber":
                    returnValue = GetCatalogNumber(dictionaryWebData);
                    break;
                case "FindTaxonID":
                    returnValue = FindTaxonId(dictionaryWebData, context);
                    break;
                case "GetBirdNestActivityId":
                    returnValue = GetBirdNestActivityId(dictionaryWebData, context);
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

                case "GetDecimalLatitude":
                    returnValue = GetDecimalLatitude(dictionaryWebData);
                    break;
                case "GetDecimalLongitude":
                    returnValue = GetDecimalLongitude(dictionaryWebData);
                    break;

                case "GetDyntaxaTaxonId":
                    returnValue = GetDyntaxaTaxonId(dictionaryWebData);
                    break;

                case "GetIsNeverFoundObservation":
                    returnValue = GetIsNeverFoundObservation(dictionaryWebData);
                    break;
                case "GetIsPositiveObservation":
                    returnValue = GetIsPositiveObservation(dictionaryWebData);
                    break;
                case "GetOccurrenceStatus":
                    returnValue = GetOccurrenceStatus(dictionaryWebData);
                    break;
                case "GetRecordedBy":
                    returnValue = GetRecordedBy(dictionaryWebData);
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
            String queryString = " and dyntaxaTaxonId=" + speciesObservationFields["dyntaxataxonid"].Value;

            return GetTaxonIdByQueryString(context, queryString);
        }

        /// <summary>
        /// The get recorded by.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetRecordedBy(Dictionary<string, WebDataField> dictionaryWebData)
        {
            if (dictionaryWebData["recordedby"].Value.IsNotNull())
            {
                return dictionaryWebData["RecordedBy"].Value;
            }

            return "WRAM, na";
        }

        /// <summary>
        /// The find taxon id.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
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

        ///// <summary>
        ///// MVM
        ///// CoordinateSystemId.WGS84.
        ///// </summary>
        ///// <param name="dictionaryWebData">
        ///// Dictionary of the current read record.
        ///// </param>
        //protected override void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData)
        //{
        //    if (CurrentRecord == dictionaryWebData["occurrenceid"].Value)
        //    {
        //        return;
        //    }

        //    CurrentRecord = dictionaryWebData["occurrenceid"].Value;
        //    WebPoints = null;
        //    WebPoints = new Dictionary<CoordinateSystemId, WebPoint>();
        //    Double coordinateX, coordinateY;

        //    if (String.IsNullOrEmpty(dictionaryWebData["decimallongitude"].Value))
        //    {
        //        throw new ArgumentException("longitude is not a valid WGS84 coordinate.");
        //    }

        //    coordinateX = dictionaryWebData["decimallongitude"].Value.Replace(",", ".").WebParseDouble();

        //    if (String.IsNullOrEmpty(dictionaryWebData["decimallatitude"].Value))
        //    {
        //        throw new ArgumentException("latitude is not a valid WGS84 coordinate.");
        //    }

        //    coordinateY = dictionaryWebData["decimallatitude"].Value.Replace(",", ".").WebParseDouble();

        //    WebCoordinateSystem fromWebCoordinateSystem = new WebCoordinateSystem();
        //    fromWebCoordinateSystem.Id = CoordinateSystemId.WGS84;

        //    WebCoordinateSystem toWebCoordinateSystem = new WebCoordinateSystem();

        //    foreach (CoordinateSystemId csid in Enum.GetValues(typeof(CoordinateSystemId)))
        //    {
        //        // following coordinate systems is not used and can be skipped
        //        if (csid == CoordinateSystemId.None)
        //        {
        //            continue;
        //        }

        //        ////if (csid == CoordinateSystemId.GoogleMercator) continue;
        //        // todo: remove this when there is support in ProjNet to convert to LAEA 
        //        //// http://projnet.codeplex.com/
        //        ////  if (csid == CoordinateSystemId.ETRS89_LAEA) continue;
        //        ////end

        //        toWebCoordinateSystem.Id = csid;
        //        WebPoint webPoint = new WebPoint(coordinateX, coordinateY);
        //        WebPoints.Add(csid, WebServiceData.CoordinateConversionManager.GetConvertedPoint(webPoint, fromWebCoordinateSystem, toWebCoordinateSystem));
        //    }
        //}

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
        /// Get the observation id that should be deleted from the database.
        /// </summary>
        /// <param name="speciesObservationFields">The dictionaryWebData.</param>
        /// <returns>Observation id.</returns>
        protected string GetDyntaxaTaxonId(Dictionary<string, WebDataField> speciesObservationFields)
        {
            String dyntaxaTaxonId = speciesObservationFields["dyntaxataxonid"].Value;

            if (dyntaxaTaxonId.IsNull() || dyntaxaTaxonId == "0")
            {
                throw new NotImplementedException("Det här kan vara fel??");
                dyntaxaTaxonId = "6000995";
            }

            return dyntaxaTaxonId;
        }

        /// <summary>
        /// Gets value for never found observation.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>String "1" if Dyntaxa taxon Id is 0.</returns>
        protected String GetIsNeverFoundObservation(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String dyntaxaTaxonId = dictionaryWebData["dyntaxataxonid"].Value;

            if (dyntaxaTaxonId.IsNull() || dyntaxaTaxonId == "0")
            {
                return "1";
            }

            return "0";
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
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The Catalog Number.</returns>
        private static String GetCatalogNumber(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String id = dictionaryWebData["occurrenceid"].Value;
            Int32 pos = id.LastIndexOf(":", StringComparison.Ordinal);
            return id.Substring(pos + 1);
        }

        /// <summary>
        /// Creates occurrence id.
        /// </summary>
        /// <param name="observationId">String with observationId.</param>
        /// <returns>The OccurrenceId.</returns>
        private static String CreateOccurrenceId(String observationId)
        {
            throw new NotImplementedException("Det här måste vara fel??");
            return "urn:lsid:mvm:Sighting:" + observationId;
        }
    }
}
