using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.SpeciesObservationService
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationFieldMapping class.
    /// </summary>
    public static class WebSpeciesObservationFieldMappingExtension
    {
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
        /// Get the project id value, it is the id of the related project,
        /// used for columns of a ProjectParameter type.
        /// </summary>
        /// <param name="webSpeciesObservationFieldMapping">The object</param>
        /// <returns>The project id value, if none found null is returned</returns>
        public static Int32? GetProjectId(this WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            if (webSpeciesObservationFieldMapping.DataFields.IsEmpty())
            {
                return null;
            }

            WebDataField field = webSpeciesObservationFieldMapping.DataFields.FirstOrDefault(item => item.Name == "ProjectId");
            if (field.IsNull())
            {
                return null;
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                return field.Value.WebParseInt32();
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
