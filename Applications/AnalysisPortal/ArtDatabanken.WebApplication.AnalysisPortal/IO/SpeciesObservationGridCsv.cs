using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// This class is used for creating species observation grid comma separated file (CSV).
    /// </summary>
    public class SpeciesObservationGridCsv
    {
        /// <summary>
        /// Writes species observation grid result as comma separated format (CSV) to a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="data">The data to write.</param>
        /// <param name="toCoordinateSystem">The coordinate system the coordinates should be converted to.</param>
        /// <param name="taxaName">Summary of species names.</param>
        public void WriteDataToFile(string filePath, SpeciesObservationGridResult data, CoordinateSystem toCoordinateSystem, string taxaName)
        {
            WriteDataToStream(new FileStream(filePath, FileMode.Create), data, toCoordinateSystem, taxaName);
        }

        /// <summary>
        /// Writes species observation grid result as comma separated format (CSV) to a stream.
        /// </summary>
        /// <param name="stream">The stream to be written to.</param>
        /// <param name="data">The data to write.</param>
        /// <param name="toCoordinateSystem">The coordinate system the coordinates should be converted to.</param>
        /// <param name="taxaName">Summary of species names.</param>
        public void WriteDataToStream(Stream stream, SpeciesObservationGridResult data, CoordinateSystem toCoordinateSystem, string taxaName)
        {
            CoordinateSystem fromCoordinateSystem = new CoordinateSystem((CoordinateSystemId)data.GridCellCoordinateSystemId);
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = true;
            csvConfiguration.Delimiter = ",";            
            csvConfiguration.Encoding = Encoding.UTF8;
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
            {
                writer.WriteField("occurrenceID");
                writer.WriteField("nameComplete");
                writer.WriteField("decimalLatitude");
                writer.WriteField("decimalLongitude");
                writer.NextRecord();
                                
                for (int i = 0; i < data.Cells.Count; i++)
                {
                    SpeciesObservationGridCellResult gridCell = data.Cells[i];
                    writer.WriteField(string.Format("Grid Cell {0}", i + 1));
                    Point convertedPoint =
                        GisTools.CoordinateConversionManager.GetConvertedPoint(
                            new Point(gridCell.OriginalCentreCoordinateX, gridCell.OriginalCentreCoordinateY),
                            fromCoordinateSystem, 
                            toCoordinateSystem);
                    writer.WriteField(taxaName);
                    writer.WriteField(convertedPoint.Y.ToString(CultureInfo.InvariantCulture));
                    writer.WriteField(convertedPoint.X.ToString(CultureInfo.InvariantCulture));                    
                    writer.NextRecord();
                }
            }
        }
    }
}
