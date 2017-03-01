using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    /// <summary>
    /// Handles export of taxon lists to Excel file.
    /// </summary>
    public class ExportTaxonListExcelFile
    {
        private Dictionary<Int32, Int32> _hierarchicalParentTaxonColumnIds;
        private readonly ExcelFileFormat _fileFormat;
        private readonly ExportViewModel _options;
        private readonly List<ExportTaxonItem> _exportTaxonItems;
        private Int32 _authorColumnId;
        private Int32 _columnCount;
        private Int32 _commonNameColumnId;
        private Int32 _guidColumnId;
        private Int32 _hierarchicalParentTaxaColumnId;
        private Int32 _recommendedGuidColumnId;
        private readonly Int32 _rowCount;
        private Int32 _scientificNameColumnId;
        private Int32 _swedishHistoryColumnId;
        private Int32 _synonymsColumnId;
        private Int32 _proParteSynonymsColumnId;
        private Int32 _misappliedNamesColumnId;
        private Int32 _excludeAuctorsColumnId;
        private Int32 _swedishOccurrenceColumnId;
        private Int32 _taxonCategoryColumnId;
        private Int32 _taxonIdColumnId;
        private Int32 _taxonUrlColumnId;
        private readonly ITaxonCategory _genusTaxonCategory;
        private List<Int32> _parentTaxonColumnIds;
        private List<Int32> _taxonNameCategoryColumnIds;
        private readonly List<String> _parentTaxaCategoryNames;
        private readonly String _taxonBaseUrl;
        private readonly UTF7Encoding _utf7Encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportTaxonListExcelFile"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="exportTaxonItems">The export taxon items.</param>
        /// <param name="fileFormat">The file format.</param>
        public ExportTaxonListExcelFile(
            ExportViewModel options,
            List<ExportTaxonItem> exportTaxonItems,
            ExcelFileFormat fileFormat)
        {
            HttpRequest request;

            _options = options;
            _columnCount = 0;
            _exportTaxonItems = exportTaxonItems;
            _fileFormat = fileFormat;
            _genusTaxonCategory = options.TaxonCategories.Get((Int32)TaxonCategoryId.Genus);
            _parentTaxaCategoryNames = new List<String>();
            _rowCount = exportTaxonItems.Count + 1;
            _taxonBaseUrl = "";
            _utf7Encoding = new UTF7Encoding();
            if (options.OutputTaxonUrl && HttpContext.Current.IsNotNull())
            {
                request = HttpContext.Current.Request;
                _taxonBaseUrl = request.Url.GetLeftPart(UriPartial.Authority).ToLower() + request.ApplicationPath.ToLower();
                if (!_taxonBaseUrl.EndsWith("/"))
                {
                    _taxonBaseUrl += "/";
                }

                _taxonBaseUrl += @"taxon/info/";
            }
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddHeaders(ExcelWorksheet worksheet)
        {
            Int32 columnIndex;

            columnIndex = 1;
            if (_options.OutputTaxonId)
            {
                _taxonIdColumnId = columnIndex++;
                worksheet.Cells[1, _taxonIdColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnTaxonId;
            }

            if (_options.IsHierarchical)
            {
                _hierarchicalParentTaxaColumnId = columnIndex;
                _hierarchicalParentTaxonColumnIds = new Dictionary<Int32, Int32>();
                foreach (ITaxonCategory taxonCategory in _options.GetFilteredTaxonCategories())
                {
                    _hierarchicalParentTaxonColumnIds.Add(taxonCategory.Id, columnIndex);
                    using (ExcelRange range = worksheet.Cells[1, columnIndex])
                    {
                        range.Value = taxonCategory.Name;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(GetHierarchicalColumnColor(columnIndex++));
                    }
                }
            }
            else if (_options.OutputTaxonCategory)
            {
                _taxonCategoryColumnId = columnIndex++;
                worksheet.Cells[1, _taxonCategoryColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnTaxonCategory;
            }

            if (_options.OutputScientificName)
            {
                _scientificNameColumnId = columnIndex++;
                worksheet.Cells[1, _scientificNameColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnScientificName;
            }

            if (_options.OutputAuthor)
            {
                _authorColumnId = columnIndex++;
                worksheet.Cells[1, _authorColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnAuthor;
            }

            if (_options.OutputCommonName)
            {
                _commonNameColumnId = columnIndex++;
                worksheet.Cells[1, _commonNameColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnCommonName;
            }

            if (_options.OutputGUID)
            {
                _guidColumnId = columnIndex++;
                worksheet.Cells[1, _guidColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnGUID;
            }

            if (_options.OutputRecommendedGUID)
            {
                _recommendedGuidColumnId = columnIndex++;
                worksheet.Cells[1, _recommendedGuidColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnRecommendedGUID;
            }

            if (_options.OutputTaxonUrl)
            {
                _taxonUrlColumnId = columnIndex++;
                worksheet.Cells[1, _taxonUrlColumnId].Value = Resources.DyntaxaResource.ExportStraightColumnTaxonInfoUrl;
            }

            if (_options.OutputSwedishOccurrence)
            {
                _swedishOccurrenceColumnId = columnIndex++;
                worksheet.Cells[1, _swedishOccurrenceColumnId].Value = Resources.DyntaxaResource.ExportStraightSwedishOccurrence;
            }

            if (_options.OutputSwedishHistory)
            {
                _swedishHistoryColumnId = columnIndex++;
                worksheet.Cells[1, _swedishHistoryColumnId].Value = Resources.DyntaxaResource.ExportStraightSwedishHistory;
            }

            if (_options.OutputSynonyms)
            {
                _synonymsColumnId = columnIndex++;
                worksheet.Cells[1, _synonymsColumnId].Value = Resources.DyntaxaResource.ExportStraightSynonyms;
            }

            if (_options.OutputProParteSynonyms)
            {
                _proParteSynonymsColumnId = columnIndex++;
                worksheet.Cells[1, _proParteSynonymsColumnId].Value = Resources.DyntaxaResource.ExportStraightProParteSynonyms;
            }
            if (_options.OutputProParteSynonyms)
            {
                _proParteSynonymsColumnId = columnIndex++;
                worksheet.Cells[1, _proParteSynonymsColumnId].Value = Resources.DyntaxaResource.ExportStraightProParteSynonyms;
            } 
            if (_options.OutputMisappliedNames)
            {
                _misappliedNamesColumnId = columnIndex++;
                worksheet.Cells[1, _misappliedNamesColumnId].Value = Resources.DyntaxaResource.ExportStraightMisappliedNames;
            }
            if (_options.GetOutputTaxonCategories().IsNotEmpty())
            {
                _parentTaxonColumnIds = new List<Int32>();
                //_parentTaxaColumnId = columnIndex++;
                //worksheet.Cells[1, _parentTaxaColumnId].Value = Resources.DyntaxaResource.ExportStraightParentTaxa;

                foreach (ITaxonCategory taxonCategory in _options.GetOutputTaxonCategories())
                {
                    _parentTaxonColumnIds.Add(columnIndex);
                    worksheet.Cells[1, columnIndex++].Value = taxonCategory.Name;
                    _parentTaxaCategoryNames.Add(taxonCategory.Name);
                }
            }

            if (_options.GetOutputTaxonNameCategories().IsNotEmpty())
            {
                _taxonNameCategoryColumnIds = new List<Int32>();
                foreach (ITaxonNameCategory taxonNameCategory in _options.GetOutputTaxonNameCategories())
                {
                    _taxonNameCategoryColumnIds.Add(columnIndex);
                    worksheet.Cells[1, columnIndex++].Value = taxonNameCategory.Name;
                }
            }

            _columnCount = columnIndex - 1;

            if (_columnCount > 0)
            {
                using (ExcelRange range = worksheet.Cells[1, 1, 1, _columnCount])
                {
                    range.Style.Font.Bold = true;
                }
            }
        }

        /// <summary>
        /// Adds the scientific name of the taxon.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="taxonItem">The taxon item.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        private void AddScientificName(
            ExcelWorksheet worksheet,
            ExportTaxonItem taxonItem,
            Int32 rowIndex,
            Int32 columnIndex)
        {
            if ((_options.OutputAuthorInAllNameCells || _options.OutputCommonNameInAllNameCells) &&
                (taxonItem.Taxon.Category.SortOrder >= _genusTaxonCategory.SortOrder))
            {
                using (ExcelRange range = worksheet.Cells[rowIndex, columnIndex])
                {
                    if (!_options.OutputAuthorInAllNameCells && !_options.OutputCommonNameInAllNameCells)
                    {
                        //Måste ändå kolla på synonymer _options.OutputExcludeAuthorForSynonyms
                        range.Value = taxonItem.Taxon.ScientificName;
                    }
                    else
                    {
                        range.IsRichText = true;
                        ExcelRichText ert = range.RichText.Add(taxonItem.Taxon.ScientificName);
                        ert.Italic = true;

                        //Kolla också på synonymer _options.OutputExcludeAuthorForSynonyms
                        if (_options.OutputAuthorInAllNameCells && !string.IsNullOrEmpty(taxonItem.Taxon.Author))
                        {
                            ert = range.RichText.Add(string.Format(" {0}", taxonItem.Taxon.Author));
                            ert.Italic = false;
                        }

                        if (_options.OutputCommonNameInAllNameCells && !string.IsNullOrEmpty(taxonItem.Taxon.CommonName))
                        {
                            ert = range.RichText.Add(string.Format(" {0}", taxonItem.Taxon.CommonName));
                            ert.Italic = false;
                        }
                    }
                }
            }
            else
            {
                worksheet.Cells[rowIndex, columnIndex].Value = taxonItem.GetScientificName(
                    _options.OutputAuthorInAllNameCells,
                    _options.OutputCommonNameInAllNameCells);
            }
        }

        /// <summary>
        /// Adds the taxon information data.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddTaxonInformationData(ExcelWorksheet worksheet)
        {
            ExportTaxonItem taxonItem;
            Int32 hierarchicalColumnId,
               // parentTaxonColumnId,
                rowIndex,
              //  rowRange,
                taxonNameCategoryColumnId,
                taxonNameCategoryIndex,
                taxonItemIndex;
          //  ITaxon parentTaxon;
            ITaxonCategory taxonCategory;
            ITaxonName taxonName;
            ITaxonNameCategory taxonNameCategory;
            String taxonUrl;

            rowIndex = 2;
           // parentsScientificName = new Hashtable();

            for (taxonItemIndex = 0; taxonItemIndex < _exportTaxonItems.Count; taxonItemIndex++)
            {
                taxonItem = _exportTaxonItems[taxonItemIndex];

                if (_options.OutputTaxonId)
                {
                    worksheet.Cells[rowIndex, _taxonIdColumnId].Value = taxonItem.Taxon.Id;
                }

                if (_options.IsHierarchical)
                {
                    // Hierarchical representations of taxa.
                    if (_hierarchicalParentTaxonColumnIds.ContainsKey(taxonItem.Taxon.Category.Id))
                    {
                        hierarchicalColumnId = _hierarchicalParentTaxonColumnIds[taxonItem.Taxon.Category.Id];
                        AddScientificName(worksheet, taxonItem, rowIndex, hierarchicalColumnId);
                    }
                }

                if (_options.OutputScientificName)
                {
                    AddScientificName(worksheet, taxonItem, rowIndex, _scientificNameColumnId);
                }

                if (_options.OutputAuthor && taxonItem.Taxon.Author.IsNotEmpty())
                {
                    worksheet.Cells[rowIndex, _authorColumnId].Value = taxonItem.Taxon.Author;
                }

                if (_options.OutputCommonName && taxonItem.Taxon.CommonName.IsNotEmpty())
                {
                    worksheet.Cells[rowIndex, _commonNameColumnId].Value = taxonItem.Taxon.CommonName;
                }

                if (_options.OutputGUID && taxonItem.Taxon.Guid.IsNotEmpty())
                {
                    worksheet.Cells[rowIndex, _guidColumnId].Value = taxonItem.Taxon.Guid;
                }

                if (_options.OutputRecommendedGUID)
                {
                    worksheet.Cells[rowIndex, _recommendedGuidColumnId].Value = taxonItem.GetRecommendedGuid();
                }

                if (_options.OutputTaxonUrl)
                {
                    taxonUrl = _taxonBaseUrl + taxonItem.Taxon.Id;
                    using (ExcelRange cell = worksheet.Cells[rowIndex, _taxonUrlColumnId])
                    {
                        cell.Value = taxonUrl;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.Color.SetColor(Color.Blue);
                        //cell.Hyperlink = new Uri(taxonUrl, UriKind.Absolute);                        
                    }
                }

                if (_options.OutputSwedishOccurrence && taxonItem.SwedishOccurrence.IsNotEmpty())
                {
                    worksheet.Cells[rowIndex, _swedishOccurrenceColumnId].Value = taxonItem.SwedishOccurrence;
                }

                if (_options.OutputSwedishHistory && taxonItem.SwedishHistory.IsNotEmpty())
                {
                    worksheet.Cells[rowIndex, _swedishHistoryColumnId].Value = taxonItem.SwedishHistory;
                }

                if (_options.OutputSynonyms && taxonItem.Synonyms.IsNotEmpty())
                {
                    AddNames(taxonItem.Synonyms, worksheet, taxonItem, rowIndex, _synonymsColumnId);
                }

                if (_options.OutputProParteSynonyms && taxonItem.ProParteSynonyms.IsNotEmpty())
                {
                    AddNames(taxonItem.ProParteSynonyms, worksheet, taxonItem, rowIndex, _proParteSynonymsColumnId);
                }

                if (_options.OutputMisappliedNames && taxonItem.MisappliedNames.IsNotEmpty())
                {
                    AddNames(taxonItem.MisappliedNames, worksheet, taxonItem, rowIndex, _misappliedNamesColumnId);
                }                

                if (_options.GetOutputTaxonCategories().IsNotEmpty())
                {
                    // MK 2015-09-23 Överordnade taxa var en kolumn som skapades automatiskt om man
                    //               valt minst en kategori som utdata. Informationen i denna kolumn
                    //               var felaktig och det var konstigt att den skapades automatiskt.
                    //               Därför är den nu bortkommenterad.
                    //
                    //// Add accumulated parent taxa scientific names.
                    //if (taxonItem.ParentTaxon.IsNull())
                    //{
                    //    scientificNames = "";
                    //}
                    //else
                    //{
                    //    scientificNames = (String)(parentsScientificName[taxonItem.ParentTaxon.Id]);
                    //}

                    //scientificNames += taxonItem.Taxon.Category.Name + ":" + taxonItem.Taxon.ScientificName + ";";
                    //parentsScientificName[taxonItem.Taxon.Id] = scientificNames;
                    //worksheet.Cells[rowIndex, _parentTaxaColumnId].Value = scientificNames;

                    // Add parent taxa scientific names.
                    for (Int32 index = 0; index < _options.GetOutputTaxonCategories().Count; index++) // loop taxon category columns
                    {
                        taxonCategory = _options.GetOutputTaxonCategories()[index];                                                
                        ITaxon parentCategoryTaxon = taxonItem.GetParentTaxon(taxonCategory);

                        if (parentCategoryTaxon != null)
                        {
                            worksheet.Cells[rowIndex, _parentTaxonColumnIds[index]].Value = parentCategoryTaxon.ScientificName;                            
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, _parentTaxonColumnIds[index]].Value = "";
                        }
                    }
                }

                if (_options.GetOutputTaxonNameCategories().IsNotEmpty())
                {
                    for (taxonNameCategoryIndex = 0;
                        taxonNameCategoryIndex < _options.GetOutputTaxonNameCategories().Count;
                        taxonNameCategoryIndex++)
                    {
                        taxonNameCategory = _options.GetOutputTaxonNameCategories()[taxonNameCategoryIndex];
                        taxonName = taxonItem.GetTaxonName(taxonNameCategory);
                        if (taxonName.IsNotNull())
                        {
                            taxonNameCategoryColumnId = _taxonNameCategoryColumnIds[taxonNameCategoryIndex];
                            worksheet.Cells[rowIndex, taxonNameCategoryColumnId].Value = taxonName.Name;
                        }
                    }
                }

                // Add taxon categories.
                if (_options.OutputTaxonCategory && !_options.IsHierarchical)
                {
                    //_exportTaxonItems[taxonItemIndex]
                    worksheet.Cells[rowIndex, _taxonCategoryColumnId].Value = taxonItem.Taxon.Category.Name;
                }

                rowIndex++;
            }
        }

        //private void AddMisappliedNames(ExcelWorksheet worksheet, ExportTaxonItem taxonItem, int rowIndex, int columnIndex)
        //{
        //    if (taxonItem.MisappliedNames.IsEmpty())
        //    {
        //        return;
        //    }

        //    using (ExcelRange range = worksheet.Cells[rowIndex, columnIndex])
        //    {
        //        range.IsRichText = true;
        //        ExcelRichText ert;

        //        for (int i = 0; i < taxonItem.MisappliedNames.Count; i++)
        //        {
        //            TaxonNameViewModel taxonName = taxonItem.MisappliedNames[i];
        //            if (i > 0)
        //            {
        //                ert = range.RichText.Add("; ");
        //                ert.Italic = false;
        //            }

        //            ert = range.RichText.Add(taxonName.Name);
        //            ert.Italic = taxonName.IsScientificName;
        //            if (!string.IsNullOrEmpty(taxonName.Author))
        //            {
        //                ert = range.RichText.Add(string.Format(" {0}", taxonName.Author));
        //                ert.Italic = false;
        //            }
        //        }
        //    }            
        //}

        //private void AddProParteSynonyms(ExcelWorksheet worksheet, ExportTaxonItem taxonItem, int rowIndex, int columnIndex)
        //{
            
        //}

        private void AddNames(List<TaxonNameViewModel> names, ExcelWorksheet worksheet, ExportTaxonItem taxonItem, int rowIndex, int columnIndex)
        {
            if (names.IsEmpty())
            {
                return;
            }

            using (ExcelRange range = worksheet.Cells[rowIndex, columnIndex])
            {
                range.IsRichText = true;
                ExcelRichText ert;

                for (int i = 0; i < names.Count; i++)
                {
                    TaxonNameViewModel taxonName = names[i];
                    if (i > 0)
                    {
                        ert = range.RichText.Add("; ");
                        ert.Italic = false;
                    }

                    ert = range.RichText.Add(taxonName.Name);
                    ert.Italic = taxonName.IsScientificName;
                    if (!string.IsNullOrEmpty(taxonName.Author) && (!_options.OutputAuthorForSynonyms))
                    {
                        ert = range.RichText.Add(string.Format(" {0}", taxonName.Author));
                        ert.Italic = false;
                    }
                }
            }                
        }

        /// <summary>
        /// Adds the taxon information format.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddTaxonInformationFormat(ExcelWorksheet worksheet)
        {
            ExportTaxonItem taxonItem;
            Int32 hierarchicalColumnId, rowIndex, rowRange, taxonItemIndex;

            if (_options.IsHierarchical)
            {
                // Formating back ground colour of hierarchical cells.
                rowIndex = 2;
                for (taxonItemIndex = 0; taxonItemIndex < _exportTaxonItems.Count; taxonItemIndex++)
                {
                    taxonItem = _exportTaxonItems[taxonItemIndex];
                    if (_hierarchicalParentTaxonColumnIds.ContainsKey(taxonItem.Taxon.Category.Id))
                    {
                        hierarchicalColumnId = _hierarchicalParentTaxonColumnIds[taxonItem.Taxon.Category.Id];

                        rowRange = 0;
                        while (((taxonItemIndex + 1) < _exportTaxonItems.Count)
                               && (taxonItem.Taxon.Category.Id
                                   == _exportTaxonItems[taxonItemIndex + 1].Taxon.Category.Id))
                        {
                            rowRange++;
                            taxonItemIndex++;
                        }

                        using (
                            ExcelRange range =
                                worksheet.Cells[
                                    rowIndex,
                                    hierarchicalColumnId,
                                    rowIndex + rowRange,
                                    _hierarchicalParentTaxaColumnId + _hierarchicalParentTaxonColumnIds.Count - 1])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(GetHierarchicalColumnColor(hierarchicalColumnId));
                        }
                        rowIndex += rowRange + 1;
                    }
                }

                // Format scientific name to italic in hierarchical cells.
                if (!(_options.OutputAuthorInAllNameCells ||
                      _options.OutputCommonNameInAllNameCells))
                {
                    foreach (ITaxonCategory taxonCategory in _options.GetFilteredTaxonCategories())
                    {
                        if (taxonCategory.SortOrder >= _genusTaxonCategory.SortOrder)
                        {
                            hierarchicalColumnId = _hierarchicalParentTaxonColumnIds[taxonCategory.Id];                            
                            using (ExcelRange range = worksheet.Cells[2, hierarchicalColumnId, _rowCount, _hierarchicalParentTaxaColumnId + _hierarchicalParentTaxonColumnIds.Count - 1])
                            {
                                range.Style.Font.Italic = true;
                            }

                            break;
                        }
                    }
                }
            }
          
            // Adjust all cells to data size.
            if (_columnCount > 0)
            {                
                using (ExcelRange range = worksheet.Cells[1, 1, _rowCount, _columnCount])
                {
                    range.AutoFitColumns(0);
                }
            }
        }

        /// <summary>
        /// Creates the excel file.
        /// </summary>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public MemoryStream CreateExcelFile()
        {
            MemoryStream memoryStream;
            Stopwatch stopwatch;
            ExcelWorksheet worksheet;
            memoryStream = new MemoryStream();

            try
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();

                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    // add a new worksheet to the empty workbook
                    worksheet = package.Workbook.Worksheets.Add("Taxa");
                    AddHeaders(worksheet);
                    AddTaxonInformationData(worksheet);
                    AddTaxonInformationFormat(worksheet);
                    package.Save();
                }

                stopwatch.Stop();
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception)
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                throw;
            }
        }

        /// <summary>
        /// Gets the cell address.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>Cell address string.</returns>
        private String GetCellAddress(Int32 rowIndex, Int32 columnIndex)
        {
            Byte[] bytes;
            Int32 columnDiv, columnReminder;
            String cellAdress;

            columnDiv = (columnIndex - 1) / 26;
            columnReminder = ((columnIndex - 1) % 26) + 1;
            if (columnDiv > 0)
            {
                bytes = new Byte[2];
                bytes[0] = (Byte)(64 + columnDiv);
                bytes[1] = (Byte)(64 + columnReminder);
            }
            else
            {
                bytes = new Byte[1];
                bytes[0] = (Byte)(64 + columnReminder);
            }

            cellAdress = new String(_utf7Encoding.GetChars(bytes)) + rowIndex;
            return cellAdress;
        }

        /// <summary>
        /// Gets the color of the hierarchical column.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Indexed color.</returns>
        private Color GetHierarchicalColumnColor(Int32 id)
        {
            int colorIndex = GetHierarchicalColumnColorIndex(id);
            return ExcelHelper.ColorTable[colorIndex];
        }

        /// <summary>
        /// Gets the index of the hierarchical column color.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Color index.</returns>
        private Int32 GetHierarchicalColumnColorIndex(Int32 id)
        {
            Int32 index;

            switch (id)
            {
                case 1:
                    index = 24;
                    break;
                case 2:
                    index = 33;
                    break;
                case 3:
                    index = 19;
                    break;
                case 4:
                    index = 34;
                    break;
                case 5:
                    index = 35;
                    break;
                case 6:
                    index = 36;
                    break;
                case 7:
                    index = 37;
                    break;
                case 8:
                    index = 39;
                    break;
                case 9:
                    index = 42;
                    break;
                case 10:
                    index = 43;
                    break;
                case 11:
                    index = 44;
                    break;
                case 12:
                    index = 18;
                    break;
                case 13:
                    index = 19;
                    break;
                case 14:
                    index = 34;
                    break;
                case 15:
                    index = 35;
                    break;
                case 16:
                    index = 36;
                    break;
                case 17:
                    index = 37;
                    break;
                case 18:
                    index = 39;
                    break;
                case 19:
                    index = 42;
                    break;
                case 20:
                    index = 43;
                    break;
                case 21:
                    index = 44;
                    break;
                case 22:
                    index = 18;
                    break;
                case 23:
                    index = 19;
                    break;
                case 24:
                    index = 34;
                    break;
                case 25:
                    index = 35;
                    break;
                case 26:
                    index = 36;
                    break;
                case 27:
                    index = 37;
                    break;
                case 28:
                    index = 39;
                    break;
                case 29:
                    index = 42;
                    break;
                case 30:
                    index = 43;
                    break;
                case 31:
                    index = 44;
                    break;
                default:
                    index = 1;
                    break;
            }

            return index;
        }
    }
}
