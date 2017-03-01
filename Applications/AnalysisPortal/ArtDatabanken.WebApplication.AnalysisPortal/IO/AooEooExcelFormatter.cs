using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    public interface IAooEooExcelFormatter
    {
        /// <summary>
        /// Gets the AOO header.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <returns>AOO header.</returns>
        string GetAooHeader(CoordinateSystemId coordinateSystemId);

        /// <summary>
        /// Gets the EOO header.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <returns>EOO header.</returns>
        string GetEooHeader(CoordinateSystemId coordinateSystemId);

        /// <summary>
        /// Tries to convert a number string containing white space and km2 unit to a number.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if the conversion was successful; otherwise false. The result is in the <paramref name="result"/> parameter.</returns>
        bool TryConvertKm2StringToNumber(string str, out long result);
    }

    /// <summary>
    /// AOO & EOO excel formatter.
    /// </summary>
    public class AooEooExcelFormatter : IAooEooExcelFormatter
    {
        /// <summary>
        /// Gets the AOO header.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <returns>AOO header.</returns>
        public string GetAooHeader(CoordinateSystemId coordinateSystemId)
        {
            const string aooHeaderTemplate = "AOO (km2, {0})";
            var coordinateSystemName = coordinateSystemId.GetCoordinateSystemName();
            return string.Format(aooHeaderTemplate, coordinateSystemName);
        }

        /// <summary>
        /// Gets the EOO header.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <returns>EOO header.</returns>
        public string GetEooHeader(CoordinateSystemId coordinateSystemId)
        {
            const string eooHeaderTemplate = "EOO (km2, {0})";
            var coordinateSystemName = coordinateSystemId.GetCoordinateSystemName();
            return string.Format(eooHeaderTemplate, coordinateSystemName);
        }

        /// <summary>
        /// Tries to convert a number string containing white space and km2 unit to a number.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if the conversion was successful; otherwise false. The result is in the <paramref name="result"/> parameter.</returns>
        public bool TryConvertKm2StringToNumber(string str, out long result)
        {
            string cleanedString = RemoveKm2FromString(str);
            return long.TryParse(cleanedString, out result);
        }

        /// <summary>
        /// Removes the km2 from string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>A string where km2 and white space is removed.</returns>
        private string RemoveKm2FromString(string str)
        {
            if (str == null)
            {
                return str;
            }

            return str.Replace(" km2", "").Replace(" ", "");
        }
    }
}
