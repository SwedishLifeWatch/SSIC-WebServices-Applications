using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservation class.
    /// </summary>
    public static class WebSpeciesObservationExtension
    {
        /// <summary>
        /// Get species observation field in Json representation.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="json">Species observation in Json format.</param>
        /// <param name="field">Species observation field.</param>
        private static void GetFieldJson(WebServiceContext context,
                                         StringBuilder json,
                                         WebSpeciesObservationField field)
        {
            DateTime dateTime;
            String fieldName;

            if (json.ToString().Length > 10)
            {
                // Not first column.
                json.Append(", ");
            }

            // Add field name.
            if ((field.ClassIdentifier == "Project") &&
                field.PropertyIdentifier.StartsWith("ProjectParameter"))
            {
                // Special handling of species observation project parameters.
                fieldName = field.PropertyIdentifier;
            }
            else
            {
                fieldName = field.ClassIdentifier + "_" + field.PropertyIdentifier;
            }

            json.Append("\"" + fieldName + "\": ");

            // Add value.
            switch (field.Type)
            {
                case WebDataType.Boolean:
                    json.Append(field.Value.ToLower());
                    break;

                case WebDataType.DateTime:
                    json.Append("\"");
                    json.Append(field.Value);
                    json.Append("\"");

                    // Add date time special fields.
                    dateTime = field.Value.WebParseDateTime();
                    json.Append(", \"" + fieldName + "_DatePartOnly" + "\": \"" + dateTime.Date.ToString("yyyy-MM-dd") + "\"");
                    json.Append(", \"" + fieldName + "_DayOfMonth" + "\": " + dateTime.Day);
                    json.Append(", \"" + fieldName + "_DayOfYear" + "\": " + dateTime.DayOfYear);
                    json.Append(", \"" + fieldName + "_MonthOfYear" + "\": " + dateTime.Month);
                    json.Append(", \"" + fieldName + "_WeekOfYear" + "\": " + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                    json.Append(", \"" + fieldName + "_Year" + "\": " + dateTime.Year);
                    json.Append(", \"" + fieldName + "_YearAndMonth" + "\": \"" + dateTime.Date.ToString("yyyy-MM") + "\"");
                    json.Append(", \"" + fieldName + "_YearAndWeek" + "\": \"" + string.Format("{0}-{1}", dateTime.Date.ToString("yyyy"), CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)) + "\"");
                    break;

                case WebDataType.Float64:
                case WebDataType.Int32:
                case WebDataType.Int64:
                    json.Append(field.Value);
                    break;

                case WebDataType.String:
                    json.Append("\"");
                    json.Append(GetStringValueJson(context, field));
                    json.Append("\"");

                    json.Append(", \"" + fieldName + "_Lowercase" + "\": ");
                    json.Append("\"");
                    json.Append(GetStringValueJson(context, field).ToLower());
                    json.Append("\"");
                    break;

                default:
                    throw new Exception("Not handled data type, type = " + field.Type);
            }
        }

        /// <summary>
        /// Get species observation in Json representation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="context">Web service context.</param>
        /// <returns>Specified species observation field.</returns>
        public static String GetJson(this WebSpeciesObservation speciesObservation,
                                     WebServiceContext context)
        {
            StringBuilder json;

            json = new StringBuilder();
            if (speciesObservation.IsNotNull() &&
                speciesObservation.Fields.IsNotEmpty())
            {
                json.Append(@"{ ");
                foreach (WebSpeciesObservationField field in speciesObservation.Fields)
                {
                    GetFieldJson(context, json, field);
                }

                GetSpecialFieldsJson(json, speciesObservation.Fields);
                json.Append(@" }");
            }

            return json.ToString();
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        
        /// <summary>
        /// Get special species observation field in Json representation.
        /// </summary>
        /// <param name="json">Species observation in Json format.</param>
        /// <param name="fields">Species observation fields.</param>
        private static void GetSpecialFieldsJson(StringBuilder json, List<WebSpeciesObservationField> fields)
        {
            Boolean observationDateTimeIsOneDay, observationDateTimeIsOneWeek,
                    observationDateTimeIsOneMonth, observationDateTimeIsOneYear;
            DateTime end, start;
            Int32 coordinateUncertaintyInMeters, disturbanceRadius;
            Int64 observationDateTimeAccuracy;
            WebSpeciesObservationField coordinateUncertaintyInMetersField,
                                       disturbanceRadiusField,
                                       latitudeField, longitudeField,
                                       observationStart, observationEnd;

            coordinateUncertaintyInMetersField = fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                                 SpeciesObservationPropertyId.CoordinateUncertaintyInMeters.ToString());
            coordinateUncertaintyInMeters = coordinateUncertaintyInMetersField.Value.WebParseInt32();
            if (coordinateUncertaintyInMeters < 1)
            {
                coordinateUncertaintyInMeters = 1;
            }

            disturbanceRadiusField = fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                     "DisturbanceRadius");
            disturbanceRadius = disturbanceRadiusField.Value.WebParseInt32();

            latitudeField = fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                            SpeciesObservationPropertyId.DecimalLatitude.ToString());
            longitudeField = fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                             SpeciesObservationPropertyId.DecimalLongitude.ToString());
            observationEnd = fields.GetField(SpeciesObservationClassId.Event.ToString(),
                                             SpeciesObservationPropertyId.End.ToString());
            end = observationEnd.Value.WebParseDateTime();
            observationStart = fields.GetField(SpeciesObservationClassId.Event.ToString(),
                                               SpeciesObservationPropertyId.Start.ToString());
            start = observationStart.Value.WebParseDateTime();
            observationDateTimeAccuracy = (end - start).GetTotalSeconds();
            observationDateTimeIsOneDay = (end.Year == start.Year) && (end.DayOfYear == start.DayOfYear);
            observationDateTimeIsOneWeek = (end.Year == start.Year) &&
                                           (GetIso8601WeekOfYear(end) == GetIso8601WeekOfYear(start));
            observationDateTimeIsOneMonth = (end.Year == start.Year) && (end.Month == start.Month);
            observationDateTimeIsOneYear = end.Year == start.Year;

            if (latitudeField.IsNotNull() &&
                longitudeField.IsNotNull())
            {
                // Add point for location.
                json.Append(", \"Location\": {");
                json.Append(" \"type\": \"point\",");
                json.Append(" \"coordinates\" : [");
                json.Append(longitudeField.Value + ", ");
                json.Append(latitudeField.Value + "]}");

                // Add coordinate uncertainty in meters as circle.
                json.Append(", \"CoordinateUncertaintyInMeters\": { ");
                json.Append("\"type\": \"circle\", ");
                json.Append("\"coordinates\": ");
                json.Append("[" + longitudeField.Value);
                json.Append(", " + latitudeField.Value + "], ");
                json.Append("\"radius\": \"" + coordinateUncertaintyInMeters + "m\" }");

                // Add disturbance radius as circle.
                if (1 <= disturbanceRadius)
                {
                    json.Append(", \"DisturbanceRadius\": { ");
                    json.Append("\"type\": \"circle\", ");
                    json.Append("\"coordinates\": ");
                    json.Append("[" + longitudeField.Value);
                    json.Append(", " + latitudeField.Value + "], ");
                    json.Append("\"radius\": \"" + disturbanceRadius + "m\" }");
                }
            }

            json.Append(", \"ObservationDateTimeAccuracy\": " + observationDateTimeAccuracy);
            json.Append(", \"ObservationDateTimeIsOneDay\": " + observationDateTimeIsOneDay.WebToString().ToLower());
            json.Append(", \"ObservationDateTimeIsOneWeek\": " + observationDateTimeIsOneWeek.WebToString().ToLower());
            json.Append(", \"ObservationDateTimeIsOneMonth\": " + observationDateTimeIsOneMonth.WebToString().ToLower());
            json.Append(", \"ObservationDateTimeIsOneYear\": " + observationDateTimeIsOneYear.WebToString().ToLower());
        }

        /// <summary>
        /// Get species observation field of data type
        /// String in Json representation.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="field">Species observation field.</param>
        private static String GetStringValueJson(WebServiceContext context,
                                                 WebSpeciesObservationField field)
        {
            String errorMessage, stringValue;

            // Handle accepted special characters.
            stringValue = field.Value.CheckJsonInjection();

            // Remove unwanted special characters.
            if (stringValue.Contains("\x6") || 
                stringValue.Contains("\x7") ||
                stringValue.Contains("\x8") ||
                stringValue.Contains("\x9") ||
                stringValue.Contains("\xA") ||
                stringValue.Contains("\xB") ||
                stringValue.Contains("\xC") ||
                stringValue.Contains("\xD"))
            {
                if (stringValue.Contains("\x6"))
                {
                    errorMessage = "Species observation field: " +
                                   field.ClassIdentifier + " " +
                                   field.PropertyIdentifier + " " +
                                   " contains character Hex:6.";
                    WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\x6", "");
                }

                if (stringValue.Contains("\x7"))
                {
                    errorMessage = "Species observation field: " +
                                   field.ClassIdentifier + " " +
                                   field.PropertyIdentifier + " " +
                                   " contains character Hex:7.";
                    WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\x7", "");
                }

                if (stringValue.Contains("\x8"))
                {
                    errorMessage = "Species observation field: " +
                                   field.ClassIdentifier + " " +
                                   field.PropertyIdentifier + " " +
                                   " contains character Hex:8.";
                    WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\x8", "");
                }

                if (stringValue.Contains("\x9"))
                {
                    //errorMessage = "Species observation field: " +
                    //               field.ClassIdentifier + " " +
                    //               field.PropertyIdentifier + " " +
                    //               " contains character Hex:9.";
                    //WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\x9", "");
                }

                if (stringValue.Contains("\xA"))
                {
                    //errorMessage = "Species observation field: " +
                    //               field.ClassIdentifier + " " +
                    //               field.PropertyIdentifier + " " +
                    //               " contains character Hex:A.";
                    //WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\xA", "");
                }

                if (stringValue.Contains("\xB"))
                {
                    errorMessage = "Species observation field: " +
                                   field.ClassIdentifier + " " +
                                   field.PropertyIdentifier + " " +
                                   " contains character Hex:B.";
                    WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\xB", "");
                }

                if (stringValue.Contains("\xC"))
                {
                    errorMessage = "Species observation field: " +
                                   field.ClassIdentifier + " " +
                                   field.PropertyIdentifier + " " +
                                   " contains character Hex:C.";
                    WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\xC", "");
                }

                if (stringValue.Contains("\xD"))
                {
                    //errorMessage = "Species observation field: " +
                    //               field.ClassIdentifier + " " +
                    //               field.PropertyIdentifier + " " +
                    //               " contains character Hex:D.";
                    //WebServiceData.LogManager.Log(context, errorMessage, LogType.Error, null);
                    stringValue = stringValue.Replace("\xD", "");
                }
            }

            return stringValue;
        }
    }
}
