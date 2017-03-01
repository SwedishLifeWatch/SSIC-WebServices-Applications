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
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using CsvHelper;
using CsvHelper.Configuration;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// This class is used for creating species observations comma separated file (CSV).
    /// </summary>
    public class SpeciesObservationsCsv
    {
        private List<Dictionary<ViewTableField, string>> _speciesObservations;        
        private bool useLabelAsColumnHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationsCsv"/> class.
        /// </summary>
        /// <param name="speciesObservations">The species observations.</param>        
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>
        public SpeciesObservationsCsv(
            List<Dictionary<ViewTableField, string>> speciesObservations,            
            bool useLabelAsColumnHeader)
        {
            _speciesObservations = speciesObservations;            
            this.useLabelAsColumnHeader = useLabelAsColumnHeader;
        }        

        /// <summary>
        /// Gets the file format setting that exists in MySettings.
        /// </summary>
        public PresentationFileFormatSetting FileFormatSetting
        {
            get { return SessionHandler.MySettings.Presentation.FileFormat; }
        }

        /// <summary>
        /// Writes species observations result as comma separated format (CSV) to a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="speciesObservations">The data to write.</param>        
        public void WriteDataToFile(string filePath, List<Dictionary<ViewTableField, string>> speciesObservations)
        {
            WriteDataToStream(new FileStream(filePath, FileMode.Create), speciesObservations);
        }

        /// <summary>
        /// Writes species observations result as comma separated format (CSV) to a stream.
        /// </summary>
        /// <param name="stream">The stream to be written to.</param>        
        /// <param name="speciesObservations">The data to write.</param>        
        public void WriteDataToStream(Stream stream, List<Dictionary<ViewTableField, string>> speciesObservations)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = FileFormatSetting.CsvFileSettings.QuoteAllColumns;
            csvConfiguration.Delimiter = FileFormatSetting.CsvFileSettings.GetSeparator();
            csvConfiguration.Encoding = Encoding.UTF8;
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
            {
                foreach (ViewTableField tableField in speciesObservations[0].Keys)
                {
                    if (useLabelAsColumnHeader)
                    {
                        writer.WriteField(tableField.Title);
                    }
                    else
                    {
                        writer.WriteField(tableField.DataField);
                    }                    
                }

                writer.NextRecord();                
                foreach (Dictionary<ViewTableField, string> speciesObservation in speciesObservations)
                {
                    foreach (string val in speciesObservation.Values)
                    {
                        writer.WriteField(val);
                    }

                    writer.NextRecord();                    
                }
            }
        }

        /// <summary>
        /// Writes species observations result as comma separated format (CSV) to a MemoryStream.
        /// </summary>
        /// <returns>CSV file MemoryStream.</returns>
        public MemoryStream ToStream()
        {
            MemoryStream memoryStream = new MemoryStream();
            WriteDataToStream(memoryStream, _speciesObservations);
            return memoryStream;
        }
    }
}
