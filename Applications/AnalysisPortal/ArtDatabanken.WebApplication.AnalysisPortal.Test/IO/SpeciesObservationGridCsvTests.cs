using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.IO
{
    [TestClass]
    public class SpeciesObservationGridCsvTests
    {

        /// <summary>
        /// Removes the Byte Order Mark from UTF8 string if it exists.
        /// </summary>        
        /// <param name="str">The string.</param>
        /// <returns>A string without Byte Order Mark in the beginning.</returns>
        private static string RemoveBOM(string str)
        {
            string BOMMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (str.StartsWith(BOMMarkUtf8))
            {
                str = str.Remove(0, BOMMarkUtf8.Length);
            }

            return str.Replace("\0", "");
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteData_DataWithTwoCellResults_DataIsWrittenCsvFormatted()
        {
            SpeciesObservationGridCsv speciesObservationGridCsv = new SpeciesObservationGridCsv();
            SpeciesObservationGridResult data = CreateSpeciesObservationGridResultSampleData();
            CoordinateSystem toCoordinateSystem = new CoordinateSystem(CoordinateSystemId.WGS84);
            const string taxaName = "All taxa";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                speciesObservationGridCsv.WriteDataToStream(memoryStream, data, toCoordinateSystem, taxaName);
                
                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

                string expectedResult = 
@"""occurrenceID"",""nameComplete"",""decimalLatitude"",""decimalLongitude""
""Grid Cell 1"",""All taxa"",""67.4331616673203"",""21.665551487154""
""Grid Cell 2"",""All taxa"",""63.6149937978252"",""16.5126895276083""
";
                Assert.AreEqual(expectedResult, RemoveBOM(actual));
            }
        }





        private SpeciesObservationGridResult CreateSpeciesObservationGridResultSampleData()
        {
            SpeciesObservationGridResult speciesObservationGridResult = new SpeciesObservationGridResult();
            speciesObservationGridResult.GridCellCoordinateSystem = "SWEREF99_TM";
            speciesObservationGridResult.GridCellCoordinateSystemId = (int)CoordinateSystemId.SWEREF99_TM; // 3
            speciesObservationGridResult.GridCellSize = 10000;
            speciesObservationGridResult.Cells = new List<SpeciesObservationGridCellResult>
            {
                CreateSpeciesObservationGridCellResultSampleData1(),
                CreateSpeciesObservationGridCellResultSampleData2()
            };
            return speciesObservationGridResult;
        }

        private SpeciesObservationGridCellResult CreateSpeciesObservationGridCellResultSampleData1()
        {
            SpeciesObservationGridCellResult gridCell = new SpeciesObservationGridCellResult();
            gridCell.CentreCoordinateX = 2411798.159305429; // Google Mercator.
            gridCell.CentreCoordinateY = 10280580.099874405;
            gridCell.OriginalCentreCoordinateX = 785000; // SWEREF 99.
            gridCell.OriginalCentreCoordinateY = 7495000;
            gridCell.CentreCoordinate = null;
            gridCell.ObservationCount = 97;
            gridCell.BoundingBox = new double[4][];
            gridCell.BoundingBox[0] = new[] { 2397519.7798124184, 10269047.080624536 };
            gridCell.BoundingBox[1] = new[] { 2423290.380509268, 10266269.42946804 };
            gridCell.BoundingBox[2] = new[] { 2426124.137029141, 10292101.754520087 };
            gridCell.BoundingBox[3] = new[] { 2400258.3455469096, 10294902.037247345 };
            return gridCell;
        }

        private SpeciesObservationGridCellResult CreateSpeciesObservationGridCellResultSampleData2()
        {
            SpeciesObservationGridCellResult gridCell = new SpeciesObservationGridCellResult();
            gridCell.CentreCoordinateX = 1838184.1898407727; // Google Mercator.
            gridCell.CentreCoordinateY = 9252662.761889834;
            gridCell.OriginalCentreCoordinateX = 575000; // SWEREF 99.
            gridCell.OriginalCentreCoordinateY = 7055000;
            gridCell.CentreCoordinate = null;
            gridCell.ObservationCount = 19;
            gridCell.BoundingBox = new double[4][];
            gridCell.BoundingBox[0] = new[] { 1826714.9631208784, 9241691.279246228 };
            gridCell.BoundingBox[1] = new[] { 1849122.4978048601, 9241161.527512556 };
            gridCell.BoundingBox[2] = new[] { 1849688.763634222, 9263632.317236027 };
            gridCell.BoundingBox[3] = new[] { 1827210.534783919, 9264165.829016024 };
            return gridCell;
        }

    }
}
