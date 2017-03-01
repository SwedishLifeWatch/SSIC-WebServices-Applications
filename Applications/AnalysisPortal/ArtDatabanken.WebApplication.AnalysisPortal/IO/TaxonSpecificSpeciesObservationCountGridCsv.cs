using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{       
    /// <summary>
    /// This class is used for creating comma separated file (CSV) with grid 
    /// based counts of number of species observations for each selected taxon.
    /// </summary>
    public class TaxonSpecificSpeciesObservationCountGridCsv
    {
        private TaxonSpecificGridSpeciesObservationCountResult _taxonSpecificSpeciesObservationCountGrid;
        private bool _formatCountAsOccurrence;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSpecificSpeciesObservationCountGridCsv"/> class.
        /// </summary>
        /// <param name="taxonSpecificSpeciesObservationCountGrid">The multiple species observation count grid data.</param>
        public TaxonSpecificSpeciesObservationCountGridCsv(TaxonSpecificGridSpeciesObservationCountResult taxonSpecificSpeciesObservationCountGrid, bool formatCountAsOccurrence)
        {
            _taxonSpecificSpeciesObservationCountGrid = taxonSpecificSpeciesObservationCountGrid;
            _formatCountAsOccurrence = formatCountAsOccurrence;
        }

        /// <summary>
        /// Gets the file format setting that exists in MySettings.
        /// </summary>
        public PresentationFileFormatSetting FileFormatSetting
        {
            get { return SessionHandler.MySettings.Presentation.FileFormat; }
        }

        /// <summary>
        /// Writes a comma separated file (CSV) with grid 
        /// based counts of number of species observations for each selected taxon.        
        /// </summary>
        /// <param name="filePath">The file path.</param>        
        /// <param name="taxonSpecificSpeciesObservationCountGrid">The data to write.</param>        
        public void WriteDataToFile(string filePath, TaxonSpecificGridSpeciesObservationCountResult taxonSpecificSpeciesObservationCountGrid, bool formatCountAsOccurrence)
        {
            WriteDataToStream(new FileStream(filePath, FileMode.Create), taxonSpecificSpeciesObservationCountGrid, formatCountAsOccurrence);
        }

        /// <summary>
        /// Writes a comma separated file (CSV) with grid 
        /// based counts of number of species observations for each selected taxon.        
        /// </summary>
        /// <param name="stream">
        /// The stream to be written to.
        /// </param>
        /// <param name="multipleSpeciesObservationCountGrid">
        /// The data to write.
        /// </param>
        public void WriteDataToStream(Stream stream, TaxonSpecificGridSpeciesObservationCountResult data, bool formatCountAsOccurrence)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = FileFormatSetting.CsvFileSettings.QuoteAllColumns;
            csvConfiguration.Delimiter = FileFormatSetting.CsvFileSettings.GetSeparator();
            csvConfiguration.Encoding = Encoding.UTF8;
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
            {                
                // Write header row
                string gridCoordinateSystemDescription = "";
                string displayCoordinateSystemDescription = "";
                if (data.GridCells.Count > 0)
                {
                    gridCoordinateSystemDescription = data.GridCells.First().Key.GridCoordinateSystem.ToString();
                    displayCoordinateSystemDescription = data.GridCells.First().Key.CoordinateSystem.Id.ToString();    
                }

                writer.WriteField("Id");
                writer.WriteField(String.Format("Centre coordinate X ({0})", gridCoordinateSystemDescription));
                writer.WriteField(String.Format("Centre coordinate Y ({0})", gridCoordinateSystemDescription));
                writer.WriteField(String.Format("Centre coordinate X ({0})", displayCoordinateSystemDescription));
                writer.WriteField(String.Format("Centre coordinate Y ({0})", displayCoordinateSystemDescription));
                writer.WriteField("Grid cell width (metres)");

                foreach (TaxonViewModel taxon in data.Taxa)
                {
                    writer.WriteField(string.Format("{0} (TaxonId {1})", taxon.ScientificName, taxon.TaxonId));
                }                

                writer.NextRecord();                

                // Write data
                // Data values.
                List<IGridCellBase> orderedGridCells = data.GridCells.Keys.OrderBy(x => x.Identifier).ToList();
                foreach (IGridCellBase gridCell in orderedGridCells)
                {                    
                    writer.WriteField(gridCell.Identifier);
                    writer.WriteField(gridCell.OrginalGridCellCentreCoordinate.X.ToString(CultureInfo.InvariantCulture));
                    writer.WriteField(gridCell.OrginalGridCellCentreCoordinate.Y.ToString(CultureInfo.InvariantCulture));
                    writer.WriteField(gridCell.GridCellCentreCoordinate.X.ToString(CultureInfo.InvariantCulture));
                    writer.WriteField(gridCell.GridCellCentreCoordinate.Y.ToString(CultureInfo.InvariantCulture));
                    writer.WriteField(gridCell.GridCellSize.ToString(CultureInfo.InvariantCulture));

                    foreach (TaxonViewModel taxon in data.Taxa)
                    {
                        long nrObservations = 0;
                        if (data.GridCells[gridCell].ContainsKey(taxon.TaxonId))
                        {
                            nrObservations = data.GridCells[gridCell][taxon.TaxonId].ObservationCount;
                        }

                        if (formatCountAsOccurrence)
                        {
                            int binaryVal = nrObservations > 0 ? 1 : 0;
                            writer.WriteField(binaryVal.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            writer.WriteField(nrObservations.ToString(CultureInfo.InvariantCulture));
                        }
                    }

                    writer.NextRecord();
                }
            }
        }

        /// <summary>
        /// Writes a comma separated file (CSV) with grid based counts 
        /// of number of species observations for each selected taxon to a MemoryStream.
        /// </summary>
        /// <returns>CSV file MemoryStream.</returns>
        public MemoryStream ToStream()
        {
            MemoryStream memoryStream = new MemoryStream();
            WriteDataToStream(memoryStream, _taxonSpecificSpeciesObservationCountGrid, _formatCountAsOccurrence);
            return memoryStream;
        }
    }
}
