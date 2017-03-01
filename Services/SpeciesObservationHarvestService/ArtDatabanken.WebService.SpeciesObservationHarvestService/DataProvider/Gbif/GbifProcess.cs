using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using TaxonManager = ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.TaxonManager;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif
{
    public class GbifProcess : BaseProcess
    {
        /// <summary>
        /// The GBIF web site observation detail address
        /// </summary>
        private const string WebSiteObservationAddress = "http://www.gbif.org/occurrence/{0}";

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
                case "GetEventDate":
                    returnValue = GetEventDate(dictionaryWebData, context);
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
                case "GetCoordinateY":
                    returnValue = GetCoordinateY(dictionaryWebData);
                    break;
                case "GetCoordinateY_RT90":
                    returnValue = GetCoordinateY_RT90_25_gon_v(dictionaryWebData);
                    break;
                case "GetCoordinateY_SWEREF99":
                    returnValue = GetCoordinateY_SWEREF99_TM(dictionaryWebData);
                    break;

                //case "GetCountryCode":
                //    returnValue = GetCountryCode(dictionaryWebData);
                //    break;

                //case "CreateOccurrenceID":
                //    returnValue = CreateOccurrenceId(dictionaryWebData);
                //    break;
                case "CalculateFromEventDate":
                    returnValue = GetEventDateString(dictionaryWebData);
                    break;
                case "GetProtectionLevel":
                    returnValue = GetProtectionLevel(dictionaryWebData, context);
                    break;
                case "GetEndDayOfYear":
                    returnValue = GetEndDayOfYear(dictionaryWebData);
                    break;
                case "GetStartDayOfYear":
                    returnValue = GetStartDayOfYear(dictionaryWebData);
                    break;

                case "GetOccurrenceUrl":
                    returnValue = GetOccurrenceUrl(dictionaryWebData);
                    break;

                case "GetMunicipality":
                    returnValue = GetMunicipality(dictionaryWebData, context, mapping);
                    break;

                case "GetParish":
                    returnValue = GetParish(dictionaryWebData, context, mapping);
                    break;

                case "GetStateProvince":
                    returnValue = GetStateProvince(dictionaryWebData, context, mapping);
                    break;

                case "GetCounty":
                    returnValue = GetCounty(dictionaryWebData, context, mapping);
                    break;
            }

            return returnValue;
        }

        protected override string GetDeletedObservationId(string idFromDataProvider)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// DINA
        ///// Coordinate System Id.WGS84
        ///// decimal longitude, decimal latitude.
        ///// </summary>
        ///// <param name="dictionaryWebData">Dictionary of the current read record.</param>
        //protected override void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData)
        //{
        //    if (IsCurrentRecord(dictionaryWebData) ||
        //        !dictionaryWebData.ContainsKey("decimallongitude") ||
        //        !dictionaryWebData["decimallongitude"].Value.IsDouble() ||
        //        !dictionaryWebData.ContainsKey("decimallatitude") ||
        //        !dictionaryWebData["decimallatitude"].Value.IsDouble())
        //    {
        //        return;
        //    }

        //    var webpoint = new WebPoint(dictionaryWebData["decimallongitude"].Value.WebParseDouble(), dictionaryWebData["decimallatitude"].Value.WebParseDouble());
        //    ConvertCoordinates(webpoint, CoordinateSystemId.WGS84);
        //}

        /// <summary>
        /// Returns true if the dicitionaryWebData instance is the same as last time this function was called (uses the property CurrentRecord to keep track)
        /// This instance uses values from "occurrenceid" or "gbifid" to establish uniqueness
        /// </summary>
        /// <param name="dictionaryWebData">Dictionary of the current read record.</param>
        /// <returns></returns>
        protected override bool IsCurrentRecord(Dictionary<string, WebDataField> dictionaryWebData)
        {
            const string value = "occurrenceid";
            if (CurrentRecord == (dictionaryWebData.ContainsKey(value) ? dictionaryWebData[value].Value : dictionaryWebData["gbifid"].Value))
            {
                return true;
            }

            CurrentRecord = dictionaryWebData.ContainsKey(value) ? dictionaryWebData[value].Value : dictionaryWebData["gbifid"].Value;
            return false;
        }


        /// <summary>
        /// Creates the event date.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event date in DarwinCore format.</returns>
        private static String GetEventDateString(Dictionary<string, WebDataField> dictionaryWebData)
        {
            return GetEventDate(dictionaryWebData).WebToString();
        }

        /// <summary>
        /// Creates the event date.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event date in DarwinCore format.</returns>
        private static String GetStartDayOfYear(Dictionary<string, WebDataField> dictionaryWebData)
        {
            return GetEventDate(dictionaryWebData).DayOfYear.WebToString();
        }

        /// <summary>
        /// Creates the event date.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event date in DarwinCore format.</returns>
        private static String GetEndDayOfYear(Dictionary<string, WebDataField> dictionaryWebData)
        {
            return GetEventDate(dictionaryWebData).DayOfYear.WebToString();
        }


        /// <summary>
        /// Uses the columns 'year', 'month' and 'day' to create a value intended for the 'eventDate' column, returns a formatted date string (uses WebToString)
        /// </summary>
        /// <param name="dictionaryWebData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetEventDate(Dictionary<string, WebDataField> dictionaryWebData, WebServiceContext context)
        {
            if (dictionaryWebData.ContainsKey("year") && dictionaryWebData.ContainsKey("month") && dictionaryWebData.ContainsKey("day"))
            {
                int year, month, day;
                if (int.TryParse(dictionaryWebData["year"].Value, out year) & int.TryParse(dictionaryWebData["month"].Value, out month) & int.TryParse(dictionaryWebData["day"].Value, out day))
                {
                    try
                    {
                        return new DateTime(year, month, day).WebToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Creates the event date.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event date in DarwinCore format.</returns>
        private static DateTime GetEventDate(Dictionary<string, WebDataField> dictionaryWebData)
        {
            try
            {
                if (dictionaryWebData.ContainsKey("eventdate"))
                {
                    if (dictionaryWebData["eventdate"].Value.IsNotNull())
                    {
                        return dictionaryWebData["eventdate"].Value.WebParseDateTime();
                    }
                }
                else
                {
                    var date = GetEventDate(dictionaryWebData, null);
                    if (date.IsDateTime())
                    {
                        return DateTime.Parse(date);
                    }
                }

                return DateTime.MinValue;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        private static string GetOccurrenceUrl(Dictionary<string, WebDataField> dictionaryWebData)
        {
            if (dictionaryWebData.ContainsKey("gbifid"))
            {
                return string.Format(WebSiteObservationAddress, dictionaryWebData["gbifid"].Value.WebParseInt64());
            }
            return string.Empty;
        }

        /// <summary>
        /// Get dyntaxa taxon id as a string.
        /// This method is only used as an entrance to the protected method
        /// GetDyntaxaTaxonId for testing purpose.
        /// </summary>
        /// <param name="dictionaryWebData">Dictionary of web data fields.</param>
        /// <param name="context">Web service context.</param>
        /// <returns>Returns dyntaxa taxon id as a string.</returns>
        public string GetTaxonId(Dictionary<string, WebDataField> dictionaryWebData,
                                 WebServiceContext context)
        {
            return GetDyntaxaTaxonId(dictionaryWebData, context);
        }

        /// <summary>
        /// Get dyntaxa taxon id as a string.
        /// </summary>
        /// <param name="speciesObservationFields">Dictionary of web data fields.</param>
        /// <param name="context">Web service context.</param>
        /// <returns>Returns dyntaxa taxon id as a string.</returns>
        protected override string GetDyntaxaTaxonId(Dictionary<string, WebDataField> speciesObservationFields,
                                                    WebServiceContext context)
        {
            Dictionary<Int32, TaxonInformation> taxonInformationDictionary;
            TaxonInformation taxonInformation = null;
            TaxonNameDictionaries taxonNameDictionaries;

            taxonNameDictionaries = TaxonManager.GetTaxonNameDictionaries(context);

            // Try to get dyntaxaTaxonId through scientific name.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("scientificname"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.Get(speciesObservationFields["scientificname"].Value.ToLower());
            }

            // Try to get dyntaxaTaxonId through scientific name and author.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("scientificname"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNameAndAuthor.Get(speciesObservationFields["scientificname"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientific name of species.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("species"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.Get(speciesObservationFields["species"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientific name of genus.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genus"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.Get(speciesObservationFields["genus"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientificName of generic name.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genericname"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.Get(speciesObservationFields["genericname"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientific name as synonyms.
            // No synonyms in the species observation database.

            // Try to get the dyntaxaTaxonId through genus.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genus"))
            {
                taxonInformation = taxonNameDictionaries.Genus.Get(speciesObservationFields["genus"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientific name of genus and taxon rank.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genus") && speciesObservationFields.ContainsKey("taxonrank"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.GetByTaxonRank(speciesObservationFields["genus"].Value.ToLower(),
                                                                                        speciesObservationFields["taxonrank"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through genus and kingdom.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genus") && speciesObservationFields.ContainsKey("kingdom"))
            {
                taxonInformation = taxonNameDictionaries.Genus.GetByKingdom(speciesObservationFields["genus"].Value.ToLower(),
                                                                            speciesObservationFields["kingdom"].Value.ToLower());
            }

            // Try to get the dyntaxaTaxonId through scientificname by combining genericname and specificepithet.
            if (taxonInformation.IsNull() && speciesObservationFields.ContainsKey("genericname") && speciesObservationFields.ContainsKey("specificepithet"))
            {
                taxonInformation = taxonNameDictionaries.ScientificNames.Get(speciesObservationFields["genericname"].Value.ToLower() + " " + speciesObservationFields["specificepithet"].Value.ToLower());
            }

            // Loop through all invalid taxons until a valid taxon is found.
            if (taxonInformation.IsNotNull())
            {
                taxonInformationDictionary = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
                int previousTaxonId = -1;
                while (!(taxonInformation.IsValid) &&
                       (taxonInformation.DyntaxaTaxonId != previousTaxonId) &&
                       (0 <= taxonInformation.CurrentDyntaxaTaxonId))
                {
                    previousTaxonId = taxonInformation.DyntaxaTaxonId;
                    taxonInformation = taxonInformationDictionary[taxonInformation.CurrentDyntaxaTaxonId];
                }

                if (taxonInformation.IsValid)
                {
                    return taxonInformation.DyntaxaTaxonId.ToString();
                }
            }

            return string.Empty;
        }

        protected override string GetProtectionLevel(Dictionary<string, WebDataField> dictionaryWebData, WebServiceContext context)
        {
            var taxonId = GetDyntaxaTaxonId(dictionaryWebData, context);
            if (taxonId.IsInteger())
            {
                return base.GetProtectionLevel(int.Parse(taxonId), context);
            }
            return string.Empty;
        }

        /// <summary>
        /// Replaces any misinterpreted culture specific characters in the string value, like 'Ã¶' to 'ö'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string ReplaceInvalidCharacters(string value)
        {
            return value.ReplaceInvalidCharacters();
        }
    }
}
