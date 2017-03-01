using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Xml.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// The base process.
    /// </summary>
    public abstract class BaseProcess
    {
        /// <summary>
        /// The process observation.
        /// </summary>
        /// <param name="webData">
        /// The web data.
        /// </param>
        /// <param name="mappings">
        /// The mappings.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The harvestSpeciesObservation.<see cref="HarvestSpeciesObservation"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If some properties where not found.
        /// </exception>
        public virtual HarvestSpeciesObservation ProcessObservation(WebData webData, List<HarvestMapping> mappings, WebServiceContext context)
        {
            // Create a dictionary object from the WebData object (Not cachable, needs to be done for each row)
            // Can be a problem if same name exists in more than one class...
            Dictionary<string, WebDataField> dictionaryWebData = webData.DataFields.ToDictionary(row => row.Name.ToLower(), row => row);

            WebLocale webLocale = GetWebLocale();

            HarvestSpeciesObservation harvestSpeciesObservation = new HarvestSpeciesObservation();
            harvestSpeciesObservation.Fields = new List<HarvestSpeciesObservationField>();

            if (mappings.IsNotNull())
            {
                // Loop through the mappings + create objects for every field
                foreach (HarvestMapping mapping in mappings)
                {
                    String mapProperty;
                    if (!ReadElement(mapping.Property, out mapProperty))
                    {
                        throw new ArgumentNullException(String.Format("Error in mapping. Element <Property>, {0}, not found.", mapping.Property));
                    }

                    SpeciesObservationPropertyId speciesObservationPropertyId = SpeciesObservationPropertyId.None;

                    // This validation should not be performed on fields of ProjectParameter type
                    if (mapping.GUID == null || !mapping.GUID.Contains("projectparameter"))
                    {
                        if (!Enum.TryParse(mapProperty, true, out speciesObservationPropertyId))
                        {
                            throw new ArgumentNullException(String.Format("Property value: {0} not a valid property.", mapProperty));
                        }
                    }

                    Boolean isMandatory;
                    String isMandatoryString;
                    if (!ReadElement(mapping.Mandatory.ToString(), out isMandatoryString))
                    {
                        throw new ArgumentNullException(String.Format("Error in mapping. Element <IsMandatoryFromProvider>, {0}, not found.", mapping.IsMandatoryFromProvider));
                    }

                    isMandatory = mapping.Mandatory;

                    String mapClass;
                    if (!ReadElement(mapping.Class, out mapClass))
                    {
                        throw new ArgumentNullException(String.Format("Error in mapping. Element <Class>, {0}, not found.", mapping.Class));
                    }

                    SpeciesObservationClassId speciesObservationClassId;
                    if (!Enum.TryParse(mapClass, true, out speciesObservationClassId))
                    {
                        throw new ArgumentNullException(String.Format("Class value: {0} not a valid class.", mapClass));
                    }

                    String mapType;
                    if (!ReadElement(mapping.Type, out mapType))
                    {
                        throw new ArgumentNullException(String.Format("Error in mapping. Element <Type>, {0}, not found.", mapping.Type));
                    }

                    WebDataType webDataType;
                    if (!Enum.TryParse(mapType, true, out webDataType))
                    {
                        throw new ArgumentNullException(String.Format("Type value: {0} not a valid type.", mapType));
                    }

                    String mapName;
                    if (mapping.IsProjectParameter())
                    {
                        mapName = mapping.PropertyIdentifier;
                    }
                    else
                    {
                        ReadElement(mapping.Name, out mapName);
                    }

                    String mapMethod;
                    ReadElement(mapping.Method, out mapMethod);
                    String mapDefault;
                    ReadElement(mapping.Default, out mapDefault);

                    String fieldValue = String.Empty;
                    String fieldUnit = null;

                    if (mapName.IsNotEmpty())
                    {
                        try
                        {
                            if (dictionaryWebData.ContainsKey(mapName.ToLower()))
                            {
                                fieldValue = dictionaryWebData[mapName.ToLower()].Value;
                                fieldUnit = ReplaceInvalidCharacters(dictionaryWebData[mapName.ToLower()].Unit);
                                if (webDataType == WebDataType.String)
                                {
                                    fieldValue = ReplaceInvalidCharacters(fieldValue);
                                }
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ArgumentNullException(String.Format("Field:{0} not found.", mapName));
                        }
                    }

                    if (mapMethod.IsNotEmpty())
                    {
                        fieldValue = MethodWrapper(mapMethod, dictionaryWebData, context, mapping);
                    }

                    if (fieldValue.IsNull() || fieldValue.IsEmpty())
                    {
                        // field is empty - set to default value ?
                        if (mapDefault.IsNotEmpty())
                        {
                            fieldValue = mapDefault;
                        }
                    }

                    if (fieldValue.IsNotEmpty() || isMandatory)
                    {
                        HarvestSpeciesObservationField harvestSpeciesObservationField = new HarvestSpeciesObservationField();
                        harvestSpeciesObservationField.Class = new WebSpeciesObservationClass(speciesObservationClassId);

                        // The type SpeciesObservationPropertyId.None indicates the use of the Identifier instead
                        harvestSpeciesObservationField.Property = new WebSpeciesObservationProperty();
                        if (mapping.IsProjectParameter())
                        {
                            harvestSpeciesObservationField.Property.Id = SpeciesObservationPropertyId.None;
                            harvestSpeciesObservationField.Property.Identifier = mapping.PropertyIdentifier;
                        }
                        else
                        {
                            harvestSpeciesObservationField.Property.Id = speciesObservationPropertyId;
                            if (speciesObservationPropertyId == SpeciesObservationPropertyId.None)
                            {
                                harvestSpeciesObservationField.Property.Identifier = mapping.Name;
                            }
                        }

                        harvestSpeciesObservationField.Type = webDataType;
                        harvestSpeciesObservationField.Unit = fieldUnit;
                        harvestSpeciesObservationField.Value = fieldValue.CheckInjection();
                        harvestSpeciesObservationField.Locale = webLocale;
                        harvestSpeciesObservationField.IsDarwinCore = mapping.IsDarwinCore;
                        harvestSpeciesObservationField.IsSearchable = mapping.IsSearchable;
                        harvestSpeciesObservationField.IsMandatory = isMandatory;  // måste med till nästa steg
                        harvestSpeciesObservationField.IsMandatoryFromProvider = mapping.IsMandatoryFromProvider;
                        harvestSpeciesObservationField.IsObtainedFromProvider = mapping.IsObtainedFromProvider;
                        harvestSpeciesObservationField.PersistedInTable = mapping.PersistedInTable;

                        // add the field to the observation
                        harvestSpeciesObservation.Fields.Add(harvestSpeciesObservationField);
                    }
                }
            }

            // return one observation
            return harvestSpeciesObservation;
        }

        /// <summary>
        /// Processes deleted observation.
        /// </summary>
        /// <param name="webData">The web data.</param>
        /// <returns>String with catalog number for observation that has been deleted.</returns>
        public virtual String ProcessDeletedObservation(WebData webData)
        {
            String catalogNumber = String.Empty;
            if (webData.DataFields[0].IsNotNull())
            {
                catalogNumber = webData.DataFields[0].Value;
            }

            return catalogNumber;
        }

        /// <summary>
        /// Gets a WebLocale.
        /// Currently this method returns a swedish WebLocale.
        /// </summary>
        /// <returns>A swedish WebLocale object.</returns>
        protected virtual WebLocale GetWebLocale()
        {
            WebLocale webLocale = new WebLocale();
            webLocale.Id = (int)LocaleId.sv_SE;
            webLocale.ISOCode = "sv-SE";
            webLocale.Name = "Swedish (Sweden)";
            webLocale.NativeName = "svenska (Sverige)";
            return webLocale;
        }

        /// <summary>
        /// Reads value from an element object.
        /// </summary>
        /// <param name="element">The element as XElement.</param>
        /// <param name="value">Out parameter value.</param>
        /// <returns>Returns "true" if element could be read, otherwise "false".</returns>
        protected virtual bool ReadElement(XElement element, out String value)
        {
            if (element != null)
            {
                value = element.Value;
                return true;
            }

            value = String.Empty;
            return false;
        }

        /// <summary>
        /// The read element.
        /// </summary>
        /// <param name="element">
        /// The element as String.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// Returns "true" if element could be read, otherwise "false".<see cref="bool"/>.
        /// </returns>
        protected virtual bool ReadElement(String element, out String value)
        {
            if (element != null)
            {
                value = element;
                return true;
            }

            value = String.Empty;
            return false;
        }

        /// <summary>
        /// Reads attribute from an element object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute to read.</param>
        /// <returns>Attribute value.</returns>
        protected virtual String ReadAttribute(XElement element, String attributeName)
        {
            String attributeValue = String.Empty;
            if (element.HasAttributes)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute != null)
                {
                    attributeValue = attribute.Value;
                }
            }

            return attributeValue;
        }

        /// <summary>
        /// Intended to replace any misinterpreted culture specific characters in the string value, should be overridden in the inherited class.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>No replacement is performed, just returns the value</returns>
        protected virtual string ReplaceInvalidCharacters(string value)
        {
            return value;
        }

        /// <summary>
        /// The implementation is provided by an overriding method. 
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context">The WebServiceContext.</param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>The ReturnValue of the method.</returns>
        protected abstract String MethodWrapper(String methodName,
                                                Dictionary<string, WebDataField> dictionaryWebData,
                                                WebServiceContext context,
                                                HarvestMapping mapping);

        /// <summary>
        /// The implementation is provided by an overriding method. 
        /// </summary>
        /// <param name="idFromDataProvider">Observation id as returned from the data provider.</param>
        /// <returns>The deleted observation Id.</returns>
        protected abstract String GetDeletedObservationId(String idFromDataProvider);

        /// <summary>
        /// Used for ConvertCoordinates.
        /// </summary>
        protected string CurrentRecord = "x"; // holds the current record in the dictionary

        /// <summary>
        /// The web points.
        /// </summary>
        protected Dictionary<CoordinateSystemId, WebPoint> WebPoints { get; set; } // Dictionary to keep track of each coordinate system webpoint

        /// <summary>
        /// Creates the event date.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <param name="startDateId">
        /// The start Date Id.
        /// </param>
        /// <param name="endDateId">
        /// The end Date Id.
        /// </param>
        /// <param name="startTimeId">
        /// The start Time Id.
        /// </param>
        /// <param name="endTimeId">
        /// The end Time Id.
        /// </param>
        /// <returns>
        /// The event date in DarwinCore format.
        /// </returns>
        protected virtual String CreateEventDate(
            Dictionary<string, WebDataField> dictionaryWebData,
            String startDateId = "startdate",
            String endDateId = "enddate",
            String startTimeId = "starttime",
            String endTimeId = "endtime")
        {
            DateTime startDate = DateTime.MinValue;
            DateTime startTime = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;

            if (dictionaryWebData[startDateId].Value.IsNotNull())
            {
                startDate = dictionaryWebData[startDateId].GetDateTime();
            }

            if (dictionaryWebData.ContainsKey(startTimeId) && dictionaryWebData[startTimeId].Value.IsNotNull()
                && (dictionaryWebData[startTimeId].Value.IsNotEmpty()))
            {
                startTime = dictionaryWebData[startTimeId].GetDateTime();
            }

            DateTime startDateTime = startDate.Date + startTime.TimeOfDay;

            if (dictionaryWebData[endDateId].Value.IsNotNull())
            {
                endDate = dictionaryWebData[endDateId].GetDateTime();
            }

            if (dictionaryWebData.ContainsKey(endTimeId) && dictionaryWebData[endTimeId].Value.IsNotNull()
                && (dictionaryWebData[endTimeId].Value.IsNotEmpty()))
            {
                endTime = dictionaryWebData[endTimeId].GetDateTime();
            }

            DateTime endDateTime = endDate.Date + endTime.TimeOfDay;

            // todo: behöver justera ihopslagningen av eventdate då detta inte blir riktigt
            return startDateTime + "/" + endDateTime;
        }

        /// <summary>
        /// Creates the event time.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>The event time in DarwinCore format.</returns>
        protected virtual String CreateEventTime(Dictionary<string, WebDataField> dictionaryWebData)
        {
            DateTime startTime = DateTime.MinValue;

            DateTime endTime = DateTime.MinValue;
            string returnDateTime;

            try
            {
                if (dictionaryWebData.ContainsKey("starttime") && dictionaryWebData["starttime"].Value.IsNotNull()
                    && (dictionaryWebData["starttime"].Value.IsNotNull()))
                {
                    startTime = dictionaryWebData["starttime"].GetDateTime();
                }

                returnDateTime = startTime.ToString("HH:mm:ss");

                if (dictionaryWebData.ContainsKey("endtime") && dictionaryWebData["endtime"].Value.IsNotNull()
                    && (dictionaryWebData["endtime"].Value.IsNotNull()))
                {
                    endTime = dictionaryWebData["endtime"].GetDateTime();
                }

                returnDateTime = returnDateTime + " - " + endTime.ToString("HH:mm:ss");
            }
            catch
            {
                returnDateTime = "error";
            }

            return returnDateTime;
        }

        /// <summary>
        /// Returns true if the dicitionaryWebData instance is the same as last time this function was called (uses the property CurrentRecord to keep track).
        /// This instance uses values from "occurrenceid" to establish uniqueness.
        /// Can be overridden if some other field value should be used.
        /// </summary>
        /// <param name="dictionaryWebData">Dictionary of the current read record.</param>
        /// <returns></returns>
        protected virtual bool IsCurrentRecord(Dictionary<string, WebDataField> dictionaryWebData)
        {
            const string value = "occurrenceid";
            if (CurrentRecord == dictionaryWebData[value].Value)
            {
                return true;
            }

            CurrentRecord = dictionaryWebData[value].Value;
            return false;
        }

        /// <summary>
        /// Converts the coordinates found in the dicitionaryWebData to all the coordinate systems listed in the <see cref="CoordinateSystemId"/> enumeration.
        /// Can be overridden if the coordinate system is other than WGS84 or if the coordinate fields are not named "decimallongitude" and "decimallatitude".
        /// </summary>
        /// <param name="dictionaryWebData"></param>
        protected virtual void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData, "decimallongitude", "decimallatitude", CoordinateSystemId.WGS84);
        }

        /// <summary>
        /// Converts the coordinates found in the dicitionaryWebData to all the coordinate systems listed in the <see cref="CoordinateSystemId"/> enumeration.
        /// Supposed to be called from within any overridden ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData) method
        /// </summary>
        /// <param name="dictionaryWebData"></param>
        /// <param name="xCoordinatefieldName"></param>
        /// <param name="yCoordinateFieldname"></param>
        /// <param name="coordinateSystemId"></param>
        protected void ConvertCoordinates(Dictionary<string, WebDataField> dictionaryWebData, string xCoordinatefieldName, string yCoordinateFieldname, CoordinateSystemId coordinateSystemId)
        {
            if (IsCurrentRecord(dictionaryWebData) ||
                !dictionaryWebData.ContainsKey(xCoordinatefieldName) ||
                !dictionaryWebData[xCoordinatefieldName].Value.IsDouble() ||
                !dictionaryWebData.ContainsKey(yCoordinateFieldname) ||
                !dictionaryWebData[yCoordinateFieldname].Value.IsDouble())
            {
                return;
            }

            var webpoint = new WebPoint(dictionaryWebData[xCoordinatefieldName].Value.WebParseDouble(), dictionaryWebData[yCoordinateFieldname].Value.WebParseDouble());
            ConvertCoordinates(webpoint, coordinateSystemId);
        }

        /// <summary>
        /// Converts the webPoint from the coordinate system given by the coordinateSystemId to all the coordinate systems listed in the <see cref="CoordinateSystemId"/> enumeration.
        /// The results are stored in the property WebPoints' dictionary
        /// </summary>
        /// <param name="webPoint"></param>
        /// <param name="coordinateSystemId"></param>
        private void ConvertCoordinates(WebPoint webPoint, CoordinateSystemId coordinateSystemId)
        {
            WebPoints = null;
            WebPoints = new Dictionary<CoordinateSystemId, WebPoint>();
            var fromWebCoordinateSystem = new WebCoordinateSystem { Id = coordinateSystemId };

            foreach (CoordinateSystemId csid in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                // following coordinate systems is not used and can be skipped
                if (csid == CoordinateSystemId.None)
                {
                    continue;
                }

                //// if (csid == CoordinateSystemId.GoogleMercator) continue;
                //// todo: remove this when there is support in ProjNet to convert to LAEA 
                //// http://projnet.codeplex.com/
                ////  if (csid == CoordinateSystemId.ETRS89_LAEA) continue;
                //// end
                WebPoints.Add(csid, WebServiceData.CoordinateConversionManager.GetConvertedPoint(webPoint, fromWebCoordinateSystem, new WebCoordinateSystem { Id = csid }));
            }
        }

        /// <summary>
        /// Returns the X coordinate in GoogleMercator.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The X coordinate in GoogleMercator.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateX(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.GoogleMercator].X.WebToString();
        }

        /// <summary>
        /// Returns the Y coordinate in GoogleMercator.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Y coordinate in GoogleMercator.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateY(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.GoogleMercator].Y.WebToString();
        }

        /// <summary>
        /// Returns the decimalLongitude.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The decimalLongitude.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetDecimalLongitude(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.WGS84].X.WebToString();
        }

        /// <summary>
        /// Returns the DecimalLatitude.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The DecimalLatitude.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetDecimalLatitude(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.WGS84].Y.WebToString();
        }

        /// <summary>
        /// Returns the X coordinate in WGS84.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The X coordinate in WGS84.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateX_WGS84(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.WGS84].X.WebToString();
        }

        /// <summary>
        /// Returns the Y coordinate in WGS84.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Y coordinate in WGS84.<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateY_WGS84(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.WGS84].Y.WebToString();
        }

        /// <summary>
        /// Returns the X coordinate in RT90 25 gon väst.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The X coordinate in RT90_25_gon_v.<see cref="string"/>.
        /// </returns>
        protected virtual String GetCoordinateX_RT90_25_gon_v(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.Rt90_25_gon_v].X.WebToString();
        }

        /// <summary>
        /// Returns the Y coordinate in RT90 25 gon väst.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Y coordinate in RT90_25_gon_v.<see cref="string"/>.
        /// </returns>
        protected virtual String GetCoordinateY_RT90_25_gon_v(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.Rt90_25_gon_v].Y.WebToString();
        }

        /// <summary>
        /// Returns the X coordinate in SWEREF99 TM.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The X coordinate in SWEREF99_TM.<see cref="string"/>.
        /// </returns>
        protected virtual String GetCoordinateX_SWEREF99_TM(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.SWEREF99_TM].X.WebToString();
        }

        /// <summary>
        /// Returns the Y coordinate in SWEREF99 TM.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Y coordinate in SWEREF99_TM.<see cref="string"/>.
        /// </returns>
        protected virtual String GetCoordinateY_SWEREF99_TM(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return (WebPoints == null) || (WebPoints.Count == 0) ? string.Empty : WebPoints[CoordinateSystemId.SWEREF99_TM].Y.WebToString();
        }

        /// <summary>
        /// The get GetCoordinateX_ETRS89_LAEA.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The X coordinate in ETRS89_LAEA<see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateX_ETRS89_LAEA(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return String.Empty;

            // todo: fixa så dessa koordinater kommer med
            //// return (WebPoints == null) || (WebPoints.Count == 0) ? "" : WebPoints[CoordinateSystemId.ETRS89_LAEA].X.WebToString();
        }

        /// <summary>
        /// The get coordinateY_ETRS89_LAEA.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Y coordinate in ETRS89_LAEA <see cref="string"/>.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        protected virtual String GetCoordinateY_ETRS89_LAEA(Dictionary<string, WebDataField> dictionaryWebData)
        {
            ConvertCoordinates(dictionaryWebData);
            return String.Empty;

            // todo: fixa så dessa koordinater kommer med
            //// return (WebPoints == null) || (WebPoints.Count == 0) ? "" : WebPoints[CoordinateSystemId.ETRS89_LAEA].Y.WebToString();
        }

        /// <summary>
        /// Returns a string with the municipality (Kommun) name.
        /// If municipality is included in data from data provider this value is used.
        /// Oterwise municipality is matched from the coordinates.
        /// May return an empty string if observation is located outside Swedish territorial border.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context"> The context. </param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>Municipality (Kommun) name.</returns>
        protected virtual string GetMunicipality(Dictionary<string, WebDataField> dictionaryWebData,
                                                 WebServiceContext context,
                                                 HarvestMapping mapping)
        {
            String coordinateX, coordinateY, municipality;

            municipality = String.Empty;
            if (mapping.Name.IsNotEmpty() &&
                dictionaryWebData.ContainsKey(mapping.Name.ToLower()) &&
                dictionaryWebData[mapping.Name.ToLower()].Value.IsNotEmpty())
            {
                // Get municipality from data provider.
                municipality = ReplaceInvalidCharacters(dictionaryWebData[mapping.Name.ToLower()].Value);
            }
            else
            {
                // Get municipality from coordinates.
                coordinateX = GetCoordinateX(dictionaryWebData);
                coordinateY = GetCoordinateY(dictionaryWebData);
                if (coordinateX.IsDouble() && coordinateY.IsDouble())
                {
                    municipality = context.GetSpeciesObservationDatabase().GetMunicipalityFromCoordinates(coordinateX.WebParseDouble(), coordinateY.WebParseDouble());
                }
            }

            return municipality;
        }

        /// <summary>
        /// Returns a string with the parish (församling) name.
        /// If parish is included in data from data provider this value is used.
        /// Oterwise municipality is matched from the coordinates.
        /// May return an empty string if observation is located outside Swedish territorial border.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context"> The context. </param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>Parish (församling) name.</returns>
        protected virtual string GetParish(Dictionary<string, WebDataField> dictionaryWebData,
                                           WebServiceContext context,
                                           HarvestMapping mapping)
        {
            String coordinateX, coordinateY, parish;

            parish = String.Empty;
            if (mapping.Name.IsNotEmpty() &&
                dictionaryWebData.ContainsKey(mapping.Name.ToLower()) &&
                dictionaryWebData[mapping.Name.ToLower()].Value.IsNotEmpty())
            {
                // Get parish from data provider.
                parish = ReplaceInvalidCharacters(dictionaryWebData[mapping.Name.ToLower()].Value);
            }
            else
            {
                // Get parish from coordinates.
                coordinateX = GetCoordinateX(dictionaryWebData);
                coordinateY = GetCoordinateY(dictionaryWebData);
                if (coordinateX.IsDouble() && coordinateY.IsDouble())
                {
                    parish = context.GetSpeciesObservationDatabase().GetParishFromCoordinates(coordinateX.WebParseDouble(), coordinateY.WebParseDouble());
                }
            }

            return parish;
        }

        /// <summary>
        /// Returns a string with the StateProvince (landskap) name.
        /// If parish is included in data from data provider this value is used.
        /// Oterwise StateProvince is matched from the coordinates.
        /// May return an empty string if observation is located outside Swedish territorial border.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context"> The context. </param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>StateProvince (landskap) name.</returns>
        protected virtual string GetStateProvince(Dictionary<string, WebDataField> dictionaryWebData,
                                                  WebServiceContext context,
                                                  HarvestMapping mapping)
        {
            String coordinateX, coordinateY, stateProvince;

            stateProvince = String.Empty;
            if (mapping.Name.IsNotEmpty() &&
                dictionaryWebData.ContainsKey(mapping.Name.ToLower()) &&
                dictionaryWebData[mapping.Name.ToLower()].Value.IsNotEmpty())
            {
                // Get StateProvince from data provider.
                stateProvince = ReplaceInvalidCharacters(dictionaryWebData[mapping.Name.ToLower()].Value);
            }
            else
            {
                // Get StateProvince from coordinates.
                coordinateX = GetCoordinateX(dictionaryWebData);
                coordinateY = GetCoordinateY(dictionaryWebData);
                if (coordinateX.IsDouble() && coordinateY.IsDouble())
                {
                    stateProvince = context.GetSpeciesObservationDatabase().GetStateProvinceFromCoordinates(coordinateX.WebParseDouble(), coordinateY.WebParseDouble());
                }
            }

            return stateProvince;
        }

        /// <summary>
        /// Returns a string with the county (län) name.
        /// If parish is included in data from data provider this value is used.
        /// Oterwise county is matched from the coordinates.
        /// May return an empty string if observation is located outside Swedish territorial border.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="context"> The context. </param>
        /// <param name="mapping">Mapping where the method is used.</param>
        /// <returns>County (län) name.</returns>
        protected virtual string GetCounty(Dictionary<string, WebDataField> dictionaryWebData,
                                           WebServiceContext context,
                                           HarvestMapping mapping)
        {
            String coordinateX, coordinateY, county;

            county = String.Empty;
            if (mapping.Name.IsNotEmpty() &&
                dictionaryWebData.ContainsKey(mapping.Name.ToLower()) &&
                dictionaryWebData[mapping.Name.ToLower()].Value.IsNotEmpty())
            {
                // Get county from data provider.
                county = ReplaceInvalidCharacters(dictionaryWebData[mapping.Name.ToLower()].Value);
            }
            else
            {
                // Get county from coordinates.
                coordinateX = GetCoordinateX(dictionaryWebData);
                coordinateY = GetCoordinateY(dictionaryWebData);
                if (coordinateX.IsDouble() && coordinateY.IsDouble())
                {
                    county = context.GetSpeciesObservationDatabase().GetCountyFromCoordinates(coordinateX.WebParseDouble(), coordinateY.WebParseDouble());
                }
            }

            return county;
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
        protected virtual String GetDyntaxaTaxonId(Dictionary<string, WebDataField> speciesObservationFields, WebServiceContext context)
        {
            String queryString = " dyntaxaTaxonId=" + speciesObservationFields["dyntaxaTaxonId"].Value;

            return GetTaxonIdByQueryString(context, queryString);
        }

        /// <summary>
        /// The get taxon id by query string.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <returns>
        /// The Taxon Id.<see cref="string"/>.
        /// </returns>
        protected static string GetTaxonIdByQueryString(WebServiceContext context, String queryString)
        {
            // only valid and with queryString
            var dataRows = GetTaxonTable(context).Select(" isValid=1 " + queryString);

            if (dataRows.Length == 1)
            {
                //Return the first row's first column, the dyntaxaTaxonId
                return dataRows[0][0].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns all rows (not all columns' included) in the Taxon table,
        /// uses the context's cache to store the data table to reduce the
        /// number of calls to the database.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DataTable GetTaxonTable(WebServiceContext context)
        {
            //TODO: remove the isocode from the cachekey, unnecessary when no translated values are stored?
            var cacheKey = Settings.Default.TaxonTableCacheKey + ":" + context.Locale.ISOCode;

            var dataTable = (DataTable)(context.GetCachedObject(cacheKey));

            if (dataTable.IsNull())
            {
                // hela taxontabellen
                dataTable = context.GetSpeciesObservationDatabase().GetDyntaxaTaxonForTaxonIdQueryString();
                context.AddCachedObject(cacheKey, dataTable, DateTime.Now.AddHours(2), CacheItemPriority.High);
            }

            return dataTable;
        }

        /// <summary>
        /// Gets the occurrence status.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// Return "Present" or "Absent depending on if any of the properties 
        /// IsNeverFoundObservation or IsNotRediscoveredObservation indicating absence is true.
        /// </returns>
        protected virtual String GetOccurrenceStatus(Dictionary<string, WebDataField> dictionaryWebData)
        {
            if (dictionaryWebData["isneverfoundobservation"].Value.Equals("1")
                || dictionaryWebData["isneverfoundobservation"].Value.Equals("True")
                || dictionaryWebData["isnotrediscoveredobservation"].Value.Equals("1")
                || dictionaryWebData["isnotrediscoveredobservation"].Value.Equals("True"))
            {
                return "Absent";
            }

            return "Present";
        }

        /// <summary>
        /// The get bird nest activity id.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The bird nest activity id, 0 if not applicable.<see cref="string"/>.
        /// </returns>
        protected virtual String GetBirdNestActivityId(
            Dictionary<string, WebDataField> dictionaryWebData,
            WebServiceContext context)
        {
            Int32 taxonId = dictionaryWebData["taxonid"].Value.WebParseInt32();

            // ReadTaxonTable Find Fåglar i taxontabellen
            Dictionary<int, TaxonInformation> taxonDictionary = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);

            if (!taxonDictionary.ContainsKey(taxonId) ||
                taxonDictionary[taxonId].IsNull())
            {
                return null;
            }

            // If not bird observation return value 0.
            if (!(GetBirdTaxonIds(context).Contains(taxonId)))
            {
                return "0";
            }

            String activityId;
            if (dictionaryWebData.ContainsKey("activityid"))
            {
                // ArtPortalen
                activityId = dictionaryWebData["activityid"].Value;
            }
            else if (dictionaryWebData.ContainsKey("speciesactivityid"))
            {
                // ArtDatabankenService
                activityId = dictionaryWebData["speciesactivityid"].Value;
            }
            else
            {
                return "1000000";
            }

            if (activityId.IsNull())
            {
                return "1000000";
            }

            // om AP och Fåglar och 0, sätt till 1000000
            if (activityId.WebParseInt32() == 0)
            {
                return "1000000";
            }

            // om AP och Fåglar, sätt till värdet
            return activityId;
        }

        /// <summary>
        /// Get bird taxon ids.
        /// </summary>
        /// <param name="context">Web service request context</param>
        /// <returns>Bird taxon ids.</returns>
        private DataIdInt32List GetBirdTaxonIds(WebServiceContext context)
        {
            DataIdInt32List birdTaxonIds;
            List<Int32> childTaxonIds, parentTaxonIds;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.BirdTaxonIdsCacheKey;
            birdTaxonIds = (DataIdInt32List)(context.GetCachedObject(cacheKey));

            if (birdTaxonIds.IsNull())
            {
                // Get information from database.
                parentTaxonIds = new List<Int32>();
                parentTaxonIds.Add((Int32)(TaxonId.Birds));
                childTaxonIds = WebServiceData.TaxonManager.GetChildTaxonIds(context, parentTaxonIds);
                birdTaxonIds = new DataIdInt32List(true);
                birdTaxonIds.AddRange(childTaxonIds);

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        birdTaxonIds,
                                        DateTime.Now + new TimeSpan(0, 12, 0, 0),
                                        CacheItemPriority.High);
            }

            return birdTaxonIds;
        }

        /// <summary>
        /// Gets value for positive observation.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <returns>String "0" if observation is of type NeverFound or NotRediscovered, otherwise "1".</returns>
        protected virtual String GetIsPositiveObservation(Dictionary<string, WebDataField> dictionaryWebData)
        {
            if (GetOccurrenceStatus(dictionaryWebData).Equals("Absent"))
            {
                return "0";
            }

            return "1";
        }

        /// <summary>
        /// Gets value for positive observation.
        /// </summary>
        /// <param name="dictionaryWebData">The dictionary web data.</param>
        /// <param name="defaultColumn">The column where modified date should be.</param>
        /// <param name="fallbackColumn">If not in modified date, try the fallback.</param>
        /// <returns>String "0" if observation is of type NeverFound or NotRediscovered, otherwise "1".</returns>
        protected virtual String GetModifiedDate(Dictionary<string, WebDataField> dictionaryWebData, String defaultColumn, String fallbackColumn)
        {
            DateTime dateTime;
            try
            {
                dateTime = dictionaryWebData[defaultColumn].GetDateTime();
            }
            catch (Exception)
            {
                dateTime = dictionaryWebData[fallbackColumn].GetDateTime();
            }

            return dateTime.WebToString();
        }

        /// <summary>
        /// The protection level based on dyntaxa taxon id.
        /// </summary>
        /// <param name="speciesObservationData">Species observation data.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Protection level based on dyntaxa taxon id.</returns>
        protected virtual string GetProtectionLevel(Dictionary<string, WebDataField> speciesObservationData, WebServiceContext context)
        {
            string taxonIdString = GetDyntaxaTaxonId(speciesObservationData, context);
            if (taxonIdString.IsEmpty())
            {
                // TODO: Add error handling.
                return 1.ToString();
            }
            else
            {
                return GetProtectionLevel(taxonIdString.WebParseInt32(), context);
            }
        }

        /// <summary>
        /// Get protection level based on dyntaxa taxon id.
        /// </summary>
        /// <param name="taxonId">A dyntaxa taxon id.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Protection level based on dyntaxa taxon id.</returns>
        protected virtual string GetProtectionLevel(int taxonId, WebServiceContext context)
        {
            Dictionary<int, TaxonInformation> taxonInformation = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);

            if (!taxonInformation.ContainsKey(taxonId) ||
                taxonInformation[taxonId].IsNull())
            {
                return null;
            }

            return taxonInformation[taxonId].ProtectionLevel.WebToString();
        }

        /// <summary>
        /// The get collection code.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <param name="column">
        /// The column where to find the collection code.
        /// </param>
        /// <returns>
        /// The Collection code.<see cref="string"/>.
        /// </returns>
        protected virtual String GetCollectionCode(Dictionary<string, WebDataField> dictionaryWebData, String column)
        {
            if (!dictionaryWebData.ContainsKey(column))
            {
                return String.Empty; // will be set to default
            }

            if (dictionaryWebData[column].Value.IsNotNull() && dictionaryWebData[column].Value.Trim().Length > 0)
            {
                return dictionaryWebData[column].Value.Trim();
            }

            return String.Empty; // will be set to default
        }

        /// <summary>
        /// The get rights.
        /// </summary>
        /// <param name="dictionaryWebData">
        /// The dictionary web data.
        /// </param>
        /// <returns>
        /// The Rights for the observation.<see cref="string"/>.
        /// </returns>
        protected virtual String GetRights(Dictionary<string, WebDataField> dictionaryWebData)
        {
            String protectionlevel = dictionaryWebData["protectionlevel"].Value;
            const string Returnvalue = "Not for public usage";
            try
            {
                if (protectionlevel.IsNotNull())
                {
                    protectionlevel = protectionlevel.Trim();

                    if (protectionlevel.Length > 0)
                    {
                        if (protectionlevel.ToLower() == "1")
                        {
                            return "Free usage";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Returnvalue;
            }

            return Returnvalue;
        }
    }
}
