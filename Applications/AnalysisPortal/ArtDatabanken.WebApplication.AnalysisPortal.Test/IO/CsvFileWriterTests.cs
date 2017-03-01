using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.IO
{
    [TestClass]
    public class CsvFileWriterTests
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
        public void WriteFile_WriteOneRowWithTwoColumns_ColumnsAreCommaSeparatedAndRowEndsWithCarriageReturnLineFeed()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = false;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Col1");
                    writer.WriteField("Col2");
                    writer.NextRecord();
                }
                
                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("Col1,Col2\r\n", RemoveBOM(actual));                
            }
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteFile_WriteTwoRowsWithThreeColumns_ColumnsAreCommaSeparatedAndRowsEndsWithCarriageReturnLineFeed()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = false;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Row1Col1");
                    writer.WriteField("Row1Col2");
                    writer.WriteField("Row1Col3");
                    writer.NextRecord();
                    writer.WriteField("Row2Col1");
                    writer.WriteField("Row2Col2");
                    writer.WriteField("Row2Col3");
                    writer.NextRecord();
                }

                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("Row1Col1,Row1Col2,Row1Col3\r\nRow2Col1,Row2Col2,Row2Col3\r\n", RemoveBOM(actual));
            }
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteFile_ColumnHasQuotationMark_ColumnIsWrappedInQuotationAndQuotationIsAlsoWrappedInQuotation()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = false;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Col1 \"cool\"");
                    writer.WriteField("Col2");
                    
                    writer.NextRecord();
                }

                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("\"Col1 \"\"cool\"\"\",Col2\r\n", RemoveBOM(actual));
            }
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteFile_ColumnHasComma_ColumnIsWrappedInQuotation()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = false;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Col1, has a comma");
                    writer.WriteField("Col2");
                    writer.NextRecord();
                }

                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("\"Col1, has a comma\",Col2\r\n", RemoveBOM(actual));
            }
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteFile_WriteOneRowWithTwoColumnsWithSettingWrapColumnsInQuotationMarks_ColumnsAreCommaSeparatedAndRowEndsWithCarriageReturnLineFeed()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = true;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Col1");
                    writer.WriteField("Col2");
                    writer.NextRecord();
                }

                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("\"Col1\",\"Col2\"\r\n", RemoveBOM(actual));
            }
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void WriteFile_ColumnHasQuotationMarkWithSettingWrapColumnsInQuotationMarks_ColumnIsWrappedInQuotationAndQuotationIsAlsoWrappedInQuotation()
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration();
            csvConfiguration.QuoteAllFields = true;
            csvConfiguration.Delimiter = ",";
            csvConfiguration.Encoding = Encoding.UTF8;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter writer = new CsvWriter(streamWriter, csvConfiguration))
                {
                    writer.WriteField("Col1 \"cool\"");
                    writer.WriteField("Col2");
                    writer.NextRecord();
                }

                string actual = Encoding.UTF8.GetString(memoryStream.ToArray());
                Assert.AreEqual("\"Col1 \"\"cool\"\"\",\"Col2\"\r\n", RemoveBOM(actual));
            }
        }
    }
}
