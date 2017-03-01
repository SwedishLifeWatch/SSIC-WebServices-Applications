using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations Excel file.
    /// </summary>
    public class SpeciesAOOEOOExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesAOOEOOExcelXml"/> class.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="data">The data.</param>
        /// <param name="addSettings">if set to <c>true</c> settings report should be added.</param>
        /// <param name="addProvenance">if set to <c>true</c> provenance report should be added.</param>
        /// <param name="aooEooExcelFormatter">The aoo eoo excel formatter.</param>
        public SpeciesAOOEOOExcelXml(
            IUserContext currentUser, 
            Tuple<int, string, string, string, string>[] data, 
            bool addSettings, 
            bool addProvenance,
            IAooEooExcelFormatter aooEooExcelFormatter)
            : base()
        {
            _xmlBuilder = new StringBuilder();

            // Add file definitions and basic format settings
            _xmlBuilder.AppendLine(GetInitialSection());

            // Specify column and row counts
            CoordinateSystemId coordinateSystemId = (CoordinateSystemId)SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.Value;
            List<string> columns = new List<string>();
            columns.Add(Resource.LabelTaxonId);
            columns.Add(Resource.TaxonSharedScientificName);
            columns.Add(Resource.TaxonSharedSwedishName);            
            columns.Add(aooEooExcelFormatter.GetAooHeader(coordinateSystemId));
            columns.Add(aooEooExcelFormatter.GetEooHeader(coordinateSystemId));

            _xmlBuilder.AppendLine(GetColumnInitialSection(columns.Count, data.Length));

            // Specify column widths
            foreach (var column in columns)
            {
                _xmlBuilder.AppendLine(GetColumnWidthLine(100));    
            }

            // Add row with column headers
            _xmlBuilder.AppendLine(GetRowStart());
            foreach (string column in columns)
            {
                _xmlBuilder.AppendLine(GetColumnNameRowLine(column));
            }            

            _xmlBuilder.AppendLine(GetRowEnd());

            // Data values
            foreach (var taxon in data)
            {
                _xmlBuilder.AppendLine(GetRowStart());

                _xmlBuilder.AppendLine(GetDataRowLine("Number", taxon.Item1.ToString(CultureInfo.InvariantCulture)));
                _xmlBuilder.AppendLine(GetDataRowLine("String", taxon.Item2));
                _xmlBuilder.AppendLine(GetDataRowLine("String", taxon.Item3));
                long result;
                if (aooEooExcelFormatter.TryConvertKm2StringToNumber(taxon.Item4, out result))
                {
                    _xmlBuilder.AppendLine(GetDataRowLine("Number", result.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    _xmlBuilder.AppendLine(GetDataRowLine("String", ""));                    
                }

                if (aooEooExcelFormatter.TryConvertKm2StringToNumber(taxon.Item5, out result))
                {
                    _xmlBuilder.AppendLine(GetDataRowLine("Number", result.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    _xmlBuilder.AppendLine(GetDataRowLine("String", ""));
                }                

                _xmlBuilder.AppendLine(GetRowEnd());                
            }

            _xmlBuilder.AppendLine(GetFinalSection(GetAditionalSheets(currentUser, addSettings, addProvenance))); 
        }
    }
}