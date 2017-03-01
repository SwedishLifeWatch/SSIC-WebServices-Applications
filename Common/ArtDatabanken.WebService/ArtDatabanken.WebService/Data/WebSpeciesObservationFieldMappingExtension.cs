using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationFieldMapping class.
    /// </summary>
    public static class WebSpeciesObservationFieldMappingExtension
    {
        /// <summary>
        /// Load data into a Web Species Observation Field Mapping object from database.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">Species Observation Field Mapping object.</param>
        /// <param name="dataReader">An open data reader.</param>
        public static void Load(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping, DataReader dataReader)
        {
            webSpeciesObservationFieldMapping.Id = dataReader.GetInt32(SpeciesObservationFieldMappingData.ID);
            webSpeciesObservationFieldMapping.FieldId = dataReader.GetInt32(SpeciesObservationFieldMappingData.FIELD_ID);
            webSpeciesObservationFieldMapping.DataProviderId = dataReader.GetInt32(SpeciesObservationFieldMappingData.DATA_PROVIDER_ID);
            webSpeciesObservationFieldMapping.SetGUID(dataReader.GetString(SpeciesObservationFieldMappingData.GUID));
            webSpeciesObservationFieldMapping.IsImplemented = dataReader.GetBoolean(SpeciesObservationFieldMappingData.IS_IMPLEMENTED);
            webSpeciesObservationFieldMapping.IsPlanned = dataReader.GetBoolean(SpeciesObservationFieldMappingData.IS_PLANNED);
            webSpeciesObservationFieldMapping.DefaultValue = dataReader.GetString(SpeciesObservationFieldMappingData.DEFAULT_VALUE);
            webSpeciesObservationFieldMapping.Documentation = dataReader.GetString(SpeciesObservationFieldMappingData.DOCUMENTATION);
            webSpeciesObservationFieldMapping.Method = dataReader.GetString(SpeciesObservationFieldMappingData.METHOD);
            if (dataReader.IsNotDbNull(SpeciesObservationFieldMappingData.PROJECT_ID))
            {
                webSpeciesObservationFieldMapping.SetProjectId(dataReader.GetInt32(SpeciesObservationFieldMappingData.PROJECT_ID));
            }

            webSpeciesObservationFieldMapping.SetProjectName(dataReader.GetString(SpeciesObservationFieldMappingData.PROJECT_NAME));
            webSpeciesObservationFieldMapping.SetPropertyIdentifier(dataReader.GetString(SpeciesObservationFieldMappingData.PROPERTY_IDENTIFIER));
            webSpeciesObservationFieldMapping.ProviderFieldName = dataReader.GetString(SpeciesObservationFieldMappingData.PROVIDER_FIELD_NAME);
            //    webSpeciesObservationFieldMapping.Type = dataReader.GetString(SpeciesObservationFieldDescriptionData.TYPE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='webSpeciesObservationFieldMapping'>The Species Observation Field Description.</param>
        public static void CheckData(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            if (!webSpeciesObservationFieldMapping.IsDataChecked)
            {
                webSpeciesObservationFieldMapping.CheckStrings();
                webSpeciesObservationFieldMapping.IsDataChecked = true;
            }
        }

        /// <summary>
        /// Get the GUID value, a string that can be used to map fields by their lsid for this Data provider
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <returns>The GUID value, if none found an empty string is returned</returns>
        public static string GetGUID(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                return string.Empty;
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "GUID");
            return field == null ? string.Empty : field.Value;
        }

        /// <summary>
        /// Set the GUID value, a string that can be used to map fields by their lsid for this Data provider
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <param name="value">The value</param>
        public static void SetGUID(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping, string value)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                webSpeciesObservationFieldMapping.DataFields = new List<WebDataField>();
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "GUID");
            if (field == null)
            {
                webSpeciesObservationFieldMapping.DataFields.Add(new WebDataField
                {
                    Type = WebDataType.String,
                    Name = "GUID",
                    Value = value
                });
            }
            else
            {
                field.Value = value;
            }
        }

        /// <summary>
        /// Get the ProjectName value, it is the name of the related project, used for columns of a ProjectParameter type.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <returns>The ProjectName value, if none found an empty string is returned</returns>
        public static string GetProjectName(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                return string.Empty;
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "ProjectName");
            return field == null ? string.Empty : field.Value;
        }

        /// <summary>
        /// Set the ProjectName value, it is the name of the related project, used for columns of a ProjectParameter type.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <param name="value">The value</param>
        public static void SetProjectId(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping, Int32 value)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                webSpeciesObservationFieldMapping.DataFields = new List<WebDataField>();
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "ProjectId");
            if (field == null)
            {
                webSpeciesObservationFieldMapping.DataFields.Add(new WebDataField
                {
                    Type = WebDataType.Int32,
                    Name = "ProjectId",
                    Value = value.WebToString()
                });
            }
            else
            {
                field.Value = value.WebToString();
            }
        }

        /// <summary>
        /// Set the ProjectName value, it is the name of the related project, used for columns of a ProjectParameter type.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <param name="value">The value</param>
        public static void SetProjectName(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping, string value)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                webSpeciesObservationFieldMapping.DataFields = new List<WebDataField>();
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "ProjectName");
            if (field == null)
            {
                webSpeciesObservationFieldMapping.DataFields.Add(new WebDataField
                {
                    Type = WebDataType.String,
                    Name = "ProjectName",
                    Value = value
                });
            }
            else
            {
                field.Value = value;
            }
        }

        /// <summary>
        /// Get the PropertyIdentifier value, it is a unique identifier that can be used as 'column  name' in Elastic Search.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <returns>The PropertyIdentifier value, if none found an empty string is returned</returns>
        public static string GetPropertyIdentifier(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                return string.Empty;
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "PropertyIdentifier");
            return field == null ? string.Empty : field.Value;
        }

        /// <summary>
        /// Set the PropertyIdentifier value, it is a unique identifier that can be used as 'column  name' in Elastic Search.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <param name="value">The value</param>
        public static void SetPropertyIdentifier(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping, string value)
        {
            if (webSpeciesObservationFieldMapping.DataFields == null)
            {
                webSpeciesObservationFieldMapping.DataFields = new List<WebDataField>();
            }

            var field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "PropertyIdentifier");
            if (field == null)
            {
                webSpeciesObservationFieldMapping.DataFields.Add(new WebDataField
                {
                    Type = WebDataType.String,
                    Name = "PropertyIdentifier",
                    Value = value
                });
            }
            else
            {
                field.Value = value;
            }
        }
    }
}
