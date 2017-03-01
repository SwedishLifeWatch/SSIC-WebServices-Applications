using System;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorFieldType class.
    /// </summary>
    public static class WebFactorFieldTypeExtension
    {
        /// <summary>
        /// Get data type given a factor field type.
        /// </summary>
        /// <param name='factorFieldType'>Factor field type.</param>
        /// <returns>Data type.</returns>
        public static WebDataType GetDataType(this WebFactorFieldType factorFieldType)
        {
            switch (factorFieldType.Id)
            {
                case 0:
                    return WebDataType.Boolean;
                case 1:
                    return WebDataType.Int32;
                case 2:
                    return WebDataType.String;
                case 3:
                    return WebDataType.Int32;
                case 4:
                    return WebDataType.Float64;
                default:
                    throw new ApplicationException("Unknown factor field type, ID = " + factorFieldType.Id);
            }
        }
    }
}