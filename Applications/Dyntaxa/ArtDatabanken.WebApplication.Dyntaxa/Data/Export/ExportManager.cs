using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ArtDatabanken.Data;
using TaxonList = ArtDatabanken.Data.TaxonList;
using TaxonNameList = ArtDatabanken.Data.TaxonNameList;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    /// <summary>
    /// Export taxon related information to Excel file.
    /// </summary>
    public class ExportManager
    {
        private readonly ExportViewModel _options;
        private readonly IUserContext _userContext;
        private List<ExportTaxonItem> _exportTaxonItems;

        public List<ExportTaxonItem> ExportTaxonItems
        {
            get
            {
                if (_exportTaxonItems == null)
                {
                    _exportTaxonItems = new List<ExportTaxonItem>();
                }

                return _exportTaxonItems;
            }
        }

        /// <summary>
        /// Constructor of ExportManager.
        /// </summary>
        public ExportManager(ExportViewModel options, IUserContext userContext)
        {
            _options = options;            
            _userContext = userContext;
        }

        public void CreateExportItems()
        {
            _exportTaxonItems = GetExportTaxonItems();            
        }

        public MemoryStream CreateExcelFile(ExcelFileFormat fileFormat)
        {
            ExportTaxonListExcelFile excelFile;
                        
            excelFile = new ExportTaxonListExcelFile(_options, _exportTaxonItems, fileFormat);
            return excelFile.CreateExcelFile();
        }

        /// <summary>
        /// Method that generate a list of taxon items with the most basic set of parameters.
        /// </summary>        
        /// <returns>Export taxon items.</returns>
        private List<ExportTaxonItem> GetExportTaxonItems()
        {
            Boolean hasOutputTaxonNames;
            ExportTaxonItem exportTaxonItem;
            Int32 index;
            ITaxon taxon;
            List<ExportTaxonItem> exportTaxonList;
            List<TaxonNameList> allTaxonNames = null;
            ArtDatabanken.Data.SpeciesFact speciesFact;
            Stopwatch stopwatch;
            TaxonCategoryList outputTaxonCategories;
            TaxonList filteredTaxa;

            stopwatch = new Stopwatch();
            stopwatch.Start();

            exportTaxonList = new List<ExportTaxonItem>();
            hasOutputTaxonNames = HasOutputTaxonNames();
            filteredTaxa = _options.GetFilteredTaxa(this._userContext);
            outputTaxonCategories = _options.GetOutputTaxonCategories();
            if (filteredTaxa.IsNotEmpty())
            {
                if (hasOutputTaxonNames)
                {
                    allTaxonNames = CoreData.TaxonManager.GetTaxonNames(_userContext, filteredTaxa);
                }

                for (index = 0; index < filteredTaxa.Count; index++)
                {
                    taxon = filteredTaxa[index];
                    exportTaxonItem = new ExportTaxonItem(
                        taxon,
                        _options.GetTaxonTreeNode(taxon),
                        outputTaxonCategories);

                    // Add swedish history information.
                    if (_options.OutputSwedishHistory)
                    {
                        speciesFact = _options.GetSpeciesFact(this._userContext, ArtDatabanken.Data.FactorId.SwedishHistory, exportTaxonItem.Taxon);
                        if (speciesFact.IsNotNull() &&
                            speciesFact.MainField.IsNotNull() &&
                            speciesFact.MainField.EnumValue.IsNotNull())
                        {
                            exportTaxonItem.SwedishHistory = speciesFact.MainField.EnumValue.OriginalLabel;
                        }
                    }

                    // Add swedish occurrence information.
                    if (_options.OutputSwedishOccurrence)
                    {
                        speciesFact = _options.GetSpeciesFact(this._userContext, ArtDatabanken.Data.FactorId.SwedishOccurrence, exportTaxonItem.Taxon);
                        if (speciesFact.IsNotNull() &&
                            speciesFact.MainField.IsNotNull() &&
                            speciesFact.MainField.EnumValue.IsNotNull())
                        {
                            exportTaxonItem.SwedishOccurrence = speciesFact.MainField.EnumValue.OriginalLabel;
                        }
                    }

                    // Add taxon name information.
                    if (hasOutputTaxonNames)
                    {
                        exportTaxonItem.TaxonNames = allTaxonNames[index];
                    }
                                        
                    // Synonyms
                    if (_options.OutputSynonyms)
                    {
                        exportTaxonItem.Synonyms = GetSynonyms(taxon);
                    }

                    // Synonyms
                    if (_options.OutputProParteSynonyms)
                    {
                        exportTaxonItem.ProParteSynonyms = GetProParteSynonyms(taxon);
                    }

                    // Synonyms
                    if (_options.OutputMisappliedNames)
                    {
                        exportTaxonItem.MisappliedNames = GetMisappliedNames(taxon);
                    }

                    // Synonyms excl. auktor
                    //if (_options.OutputExcludeAuctor)
                    //{
                    //    //exportTaxonItem.MisappliedNames = GetMisappliedNames(taxon);
                    //}

                    exportTaxonList.Add(exportTaxonItem);
                }
            }

            stopwatch.Stop();
            DyntaxaLogger.WriteMessage("Export: Get all data to populate Excel file: {0:N0} milliseconds", stopwatch.ElapsedMilliseconds);

            return exportTaxonList;
        }

        private List<TaxonNameViewModel> GetMisappliedNames(ITaxon taxon)
        {
            List<TaxonNameViewModel> misappliedNamesList = new List<TaxonNameViewModel>();
            List<ITaxonName> misappliedNames = taxon.GetMisappliedNames(_userContext);
            foreach (ITaxonName taxonName in misappliedNames)
            {
                misappliedNamesList.Add(new TaxonNameViewModel(taxonName, taxon));
            }

            return misappliedNamesList;            
        }

        private List<TaxonNameViewModel> GetProParteSynonyms(ITaxon taxon)
        {
            List<TaxonNameViewModel> proParteSynonymsList = new List<TaxonNameViewModel>();
            List<ITaxonName> proParteSynonyms = taxon.GetProParteSynonyms(_userContext);
            foreach (ITaxonName taxonName in proParteSynonyms)
            {
                proParteSynonymsList.Add(new TaxonNameViewModel(taxonName, taxon));
            }

            return proParteSynonymsList;            
        }

        private List<TaxonNameViewModel> GetSynonyms(ITaxon taxon)
        {
            var synonymsList = new List<TaxonNameViewModel>();
            var synonyms = taxon.GetSynonyms(_userContext, false);
            foreach (ITaxonName taxonName in synonyms)
            {
                synonymsList.Add(new TaxonNameViewModel(taxonName, taxon));
            }

            return synonymsList;
        }

        private Boolean HasOutputTaxonCategories()
        {
            Boolean hasOutputTaxonCategories;

            if (_options.OutputAllTaxonCategories.IsEmpty())
            {
                return false;
            }

            hasOutputTaxonCategories = false;
            foreach (ExportTaxonCategory exportTaxonCategory in _options.OutputAllTaxonCategories)
            {
                if (exportTaxonCategory.IsChecked)
                {
                    hasOutputTaxonCategories = true;
                    break;
                }
            }

            return hasOutputTaxonCategories;
        }

        /// <summary>
        /// Test if taxon names are used during Excel export.
        /// </summary>
        /// <returns>True, if taxon names are used during excel export.</returns>
        private Boolean HasOutputTaxonNames()
        {
            Boolean isOutputTaxonNamesUsed;

            if (_options.OutputRecommendedGUID)
            {
                return true;
            }

            isOutputTaxonNamesUsed = false;
            if (_options.OutputTaxonNameCategories.IsNotEmpty())
            {
                foreach (ExportNameType exportTaxonNameCategory in _options.OutputTaxonNameCategories)
                {
                    if (exportTaxonNameCategory.IsChecked)
                    {
                        isOutputTaxonNamesUsed = true;
                        break;
                    }
                }
            }

            return isOutputTaxonNamesUsed;
        }
    }
}
