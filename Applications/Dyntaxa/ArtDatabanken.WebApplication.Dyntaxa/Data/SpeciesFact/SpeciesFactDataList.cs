using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactDataList
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SpeciesFactDataList()
        {
        }

        /// <summary>
        /// Create a memory stream of a all factors for a taxon
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <param name="user"></param>
        /// <param name="dyntaxaFactors"></param>
        /// <param name="taxon"></param>
        /// <param name="showNonPublicData"></param>
        /// <returns></returns>
        public MemoryStream CreateSpeciesFactExcelFile(ExcelFileFormat fileFormat, IUserContext user, DyntaxaAllFactorData dyntaxaFactors, ITaxon taxon, bool showNonPublicData)
        {
            MemoryStream memoryStream;
            ExcelWorksheet workSheet = null;
            memoryStream = new MemoryStream();

            try
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    // Todo create a proper header
                    int rowId = 1;
                    rowId = CreateColumnHeaders(workSheet, rowId);
                    rowId = CreateTaxonInfoRows(user, workSheet, taxon, dyntaxaFactors.SwedishOccuranceInfo, dyntaxaFactors.SwedishHistoryInfo, rowId);
                    rowId = CreateSpeciesFactList(rowId, taxon, dyntaxaFactors.DyntaxaAllFactors, workSheet, dyntaxaFactors.HostList, dyntaxaFactors.PeriodList, dyntaxaFactors.IndividualCategoryList, dyntaxaFactors.DyntaxaAllSpeciesFactsForATaxon, showNonPublicData, null, true);
                    FormatColumns(workSheet, rowId);

                    package.Save();                    
                }

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
        /// Create a view model of a all factors for a taxon
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dyntaxaFactors"></param>
        /// <param name="taxon"></param>
        /// <param name="showNonPublicData"></param>
        /// <param name="model"></param>
        /// <param name="useAllFactors"></param>
        /// <returns></returns>
        public SpeciesFactViewModel CreateSpeciesFactViewData(IUserContext user, DyntaxaAllFactorData dyntaxaFactors, ITaxon taxon, bool showNonPublicData, SpeciesFactViewModel model, bool useAllFactors)
        {
             int rowId = 1;
             model.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
             CreateSpeciesFactList(rowId, taxon, dyntaxaFactors.DyntaxaAllFactors, null, dyntaxaFactors.HostList, dyntaxaFactors.PeriodList, dyntaxaFactors.IndividualCategoryList, dyntaxaFactors.DyntaxaAllSpeciesFactsForATaxon, showNonPublicData, model, useAllFactors);         
            
            return model;
        }

        /// <summary>
        /// Create filename.
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        private string GetFileName(ExcelFileFormat fileFormat)
        {
            string tempDirectory = Resources.DyntaxaSettings.Default.PathToTempDirectory;
            string fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, ExcelFileFormatHelper.GetExtension(fileFormat));
            string fullFileName = fileName;
            if (HttpContextSessionHelper.IsInTestMode)
            {
                fullFileName = Path.Combine(HttpContextSessionHelper.TestHelper.GetFromSession<string>("testFilePath"), fileName);
            }
            else
            {
               fullFileName = HttpContext.Current.Server.MapPath(Path.Combine(tempDirectory, fileName));
            }

            return fullFileName;
        }

        /// <summary>
        /// Reads created excel file into a memory stream 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Created memory stream</returns>
        private MemoryStream GetMemoryStream(string filename)
        {
            using (FileStream fileStream = File.OpenRead(filename))
            {
                var memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                return memStream;
            }
        }

        /// <summary>
        /// Build up the data to save in excel or to add to view data from factor list
        /// </summary>
        /// <param name="rowId"></param>
        /// <param name="taxon"></param>
        /// <param name="factors"></param>
        /// <param name="workSheet"></param>
        /// <param name="hosts"></param>
        /// <param name="periods"></param>
        /// <param name="individualCategories"></param>
        /// <param name="factorsForATaxon"></param>
        /// <param name="showNonPublicData"></param>
        /// <returns></returns>
        private int CreateSpeciesFactList(int rowId, ITaxon taxon, IList<DyntaxaFactor> factors, ExcelWorksheet workSheet, IList<DyntaxaHost> hosts, IList<DyntaxaPeriod> periods, IList<DyntaxaIndividualCategory> individualCategories, IList<DyntaxaSpeciesFact> factorsForATaxon, bool showNonPublicData, SpeciesFactViewModel viewModel, bool allFactors)
        {
            // _rowId = 1;
            Int32 factorOriginId = -1;
            string factorLabel = string.Empty;
            
            // Loop through all factors
            foreach (DyntaxaFactor factor in factors)
            {
                bool isSubHeaderSet = false;
                bool lastStatusStringIsSemiColon = false;                
                if (factor.FactorUpdateMode.IsHeader)
                {
                    bool isMainHeader = false;
                   
                    if (factor.FactorOrigin.OriginId != factorOriginId)
                    {
                        isMainHeader = true;
                        factorOriginId = factor.FactorOrigin.OriginId;
                    }
                    
                    if (viewModel.IsNotNull())
                    {
                        AddHeaderToViewData(viewModel, factor, isMainHeader);
                    }
                    else
                    {
                        AddHeaderToWorkSheetRow(workSheet, rowId, factor, isMainHeader);
                    }
                    
                    rowId++;
                }
                else
                {
                    // Check if factor is not a leaf, if not a leaf create a sub header
                    if (!factor.IsLeaf)
                    {
                        if (viewModel.IsNotNull())
                        {
                             AddHeaderTwoToViewData(viewModel, factor);
                        }
                        else
                        {
                            AddHeaderTwoToWorkSheetRow(workSheet, rowId, factor);
                        }
                       
                        isSubHeaderSet = true;
                        rowId++;
                    }

                    // Handle periodic factors
                    if (factor.IsPeriodic)
                    {
                         foreach (DyntaxaPeriod period in periods)
                        {
                            DyntaxaSpeciesFact dyntaxaSpeciesFact = null;
                            string identifier = SpeciesFactModelManager.GetSpeciesFactIdentifier(taxon, individualCategories.ElementAt(0), factor, null, period);
                            foreach (DyntaxaSpeciesFact speciesFact in factorsForATaxon)
                            {
                                if (speciesFact.Identifier == identifier)
                                {
                                    // Data found. Return it.
                                    dyntaxaSpeciesFact = speciesFact;
                                    break;
                                }
                            }

                            if (dyntaxaSpeciesFact != null)
                            {
                                 IList<DyntaxaFactorField> tempSpeciesFact = new List<DyntaxaFactorField>();
                                 tempSpeciesFact = dyntaxaSpeciesFact.Fields;

                                // Extract "Kommentar" information to be shown in the comment column, "sant" text and quality
                                string quality;
                                DyntaxaFactorField trueText;
                                IList<KeyValuePair<int, string>> qualities;
                                DyntaxaFactorField comments = GetCommentsTrueTextQuality(dyntaxaSpeciesFact, tempSpeciesFact, out quality, out trueText, out qualities);

                                // Create value information to be shown in the value column
                                string fieldValueString = string.Empty;
                                string fieldValueString2 = null;
                                fieldValueString = GetFactorFieldValueString(period, fieldValueString, tempSpeciesFact, ref lastStatusStringIsSemiColon);

                                // Here we create the factor and all it's data to be printed to a row
                                if (viewModel.IsNotNull() && allFactors)
                                {
                                    rowId = AddFactorToViewData(viewModel, rowId, null, factor, period, quality, dyntaxaSpeciesFact.Quality.QualityId, qualities, dyntaxaSpeciesFact.ReferenceId, comments, trueText, null, null, fieldValueString, null, null, null, allFactors, false, showNonPublicData, factorLabel);
                                }
                                else if (viewModel.IsNotNull() && !allFactors)
                                {
                                    // Get all avaliable valuse
                                    DyntaxaFactorFieldValues tempFieldValues = null;
                                    DyntaxaFactorFieldValues tempFieldValues2 = null;
                                    bool firstSet = false;
                                    foreach (DyntaxaFactorField factFields in tempSpeciesFact)
                                    {
                                        if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && factFields.IsMain)
                                        {
                                            if (!firstSet)
                                            {
                                                tempFieldValues = factFields.FactorFieldValues;
                                                firstSet = true;
                                            }
                                        }

                                        if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && !factFields.IsMain)
                                        {
                                            tempFieldValues2 = factFields.FactorFieldValues;
                                            fieldValueString2 = factFields.FactorFieldValues.FieldName;
                                            // use two enum valuse.. This should be nyttjande grad...
                                        }
                                    }

                                    rowId = AddFactorToViewData(viewModel, rowId, null, factor, period, quality, dyntaxaSpeciesFact.Quality.QualityId, qualities, dyntaxaSpeciesFact.ReferenceId, comments, trueText, null, null, fieldValueString, fieldValueString2, tempFieldValues, tempFieldValues2, allFactors, false, showNonPublicData, factorLabel);
                                }
                                else
                                {
                                    rowId = AddFactorToWorkSheetRow(workSheet, rowId, null, factor, period, quality, dyntaxaSpeciesFact.Quality.QualityId, comments, trueText, null, null, fieldValueString, false, showNonPublicData, factorLabel, false);
                                }
                            
                               factorLabel = factor.Label;
                            }
                        }
                    }
                    else
                    {
                        foreach (DyntaxaIndividualCategory individualCategory in individualCategories)
                        {
                            if (factor.IsTaxonomic)
                            {
                                IList<DyntaxaHost> hostList = new List<DyntaxaHost>();
                                IList<DyntaxaHost> tempHosts = new List<DyntaxaHost>();
                                foreach (DyntaxaHost host in hosts)
                                {
                                    DyntaxaSpeciesFact dyntaxaSpeciesFact = null;
                                    string identifier = SpeciesFactModelManager.GetSpeciesFactIdentifier(taxon, individualCategory, factor, host, null);

                                    foreach (DyntaxaSpeciesFact speciesFact in factorsForATaxon)
                                    {
                                        if (speciesFact.Identifier == identifier)
                                        {
                                            // Data found. Return it.
                                            dyntaxaSpeciesFact = speciesFact;
                                            
                                            break;
                                        }
                                    }

                                    if (dyntaxaSpeciesFact != null)
                                    {
                                        tempHosts.Add(host);
                                    }
                                }

                                foreach (DyntaxaHost tempHost in tempHosts)
                                {
                                    DyntaxaSpeciesFact dyntaxaSpeciesFactHost = null;
                                    string identifier = SpeciesFactModelManager.GetSpeciesFactIdentifier(taxon, individualCategory, factor, tempHost, null);
                                    string factorName = factor.Label;
                                    foreach (DyntaxaSpeciesFact speciesFact in factorsForATaxon)
                                    {
                                        if (speciesFact.Identifier == identifier)
                                        {
                                            // Data found. Return it.
                                            dyntaxaSpeciesFactHost = speciesFact;
                                            factor.HostId = Convert.ToInt32(tempHost.Id);
                                            factor.IsHost = true;
                                            
                                            break;
                                        }
                                    }

                                    if (dyntaxaSpeciesFactHost != null)
                                    {
                                        IList<DyntaxaFactorField> tempSpeciesFact = new List<DyntaxaFactorField>();

                                        tempSpeciesFact = dyntaxaSpeciesFactHost.Fields;

                                        //DyntaxaFactorField specification = tempSpeciesFact.FirstOrDefault(x => x.Label == "Specifikation");
                                        DyntaxaFactorField specification = tempSpeciesFact.FirstOrDefault(x => x.Label == "Kommentar");

                                        // Get host factor, information of the host factor is in stored in label "Specifikation"
                                        StringBuilder hostFactString = new StringBuilder();
                                        if (specification.IsNotNull())
                                        {
                                            hostFactString = GetHostStringValue(factorsForATaxon, dyntaxaSpeciesFactHost, hostFactString, tempHost);
                                            if (!(tempHost.Id.IsNull() || tempHost.Id.IsEmpty() || tempHost.Id.Equals("-1") || tempHost.Id.Equals("0")))
                                            {
                                                hostList.Add(new DyntaxaHost(tempHost.Id, tempHost.Label, tempHost.ScientificName, tempHost.CommonName));
                                            }

                                            tempSpeciesFact.Remove(specification);
                                        }

                                        // Extract "Kommentar" information to be shown in the comment column, "sant" text and quality
                                        string quality;
                                        DyntaxaFactorField trueText;
                                        IList<KeyValuePair<int, string>> qualities;
                                        DyntaxaFactorField comments = GetCommentsTrueTextQuality(dyntaxaSpeciesFactHost, tempSpeciesFact, out quality, out trueText, out qualities);

                                        // Create value information to be shown in the value column
                                        string fieldValueString = string.Empty;
                                        fieldValueString = GetFactorFieldValueString(null, fieldValueString, tempSpeciesFact, ref lastStatusStringIsSemiColon);

                                        bool isFirtsHost = false;
                                        if (tempHosts.First().Id == tempHost.Id && ((tempHosts.Count > 1 && allFactors) || (tempHosts.Count > 0 && !allFactors)))
                                        {
                                            isFirtsHost = true;
                                        }
                                        
                                        // Here we create the factor and all it's data to be printed to a row
                                        if (viewModel.IsNotNull() && allFactors)
                                        {
                                            rowId = AddFactorToViewData(viewModel, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFactHost.Quality.QualityId, qualities, dyntaxaSpeciesFactHost.ReferenceId, comments, trueText, hostFactString, tempHosts, fieldValueString, null, null, null, allFactors, isFirtsHost, showNonPublicData, string.Empty);
                                        }
                                        else if (viewModel.IsNotNull() && !allFactors)
                                        {
                                            // Get all avaliable values
                                            DyntaxaFactorFieldValues tempFieldValues = null;
                                            DyntaxaFactorFieldValues tempFieldValues2 = null;
                                            string fieldValueString2 = null;
                                            bool firstSet = false;
                                            foreach (DyntaxaFactorField factFields in tempSpeciesFact)
                                            {
                                                if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && factFields.IsMain)
                                                {
                                                    if (!firstSet)
                                                    {
                                                        tempFieldValues = factFields.FactorFieldValues;
                                                        firstSet = true;
                                                    }
                                                }

                                                if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && !factFields.IsMain)
                                                {
                                                    tempFieldValues2 = factFields.FactorFieldValues;
                                                    fieldValueString2 = factFields.FactorFieldValues.FieldName;
                                                    // use two enum valuse.. This should be nyttjande grad...
                                                }
                                            }

                                            rowId = AddFactorToViewData(viewModel, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFactHost.Quality.QualityId, qualities, dyntaxaSpeciesFactHost.ReferenceId, comments, trueText, hostFactString, tempHosts, fieldValueString, fieldValueString2, tempFieldValues, tempFieldValues2, allFactors, isFirtsHost, showNonPublicData, string.Empty);
                                        }
                                        else
                                        {
                                            rowId = AddFactorToWorkSheetRow(workSheet, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFactHost.Quality.QualityId, comments, trueText, hostFactString, tempHosts, fieldValueString, isFirtsHost, showNonPublicData);
                                         }
                                        
                                        // If this is the last host for this factor, create the host list.
                                        if (tempHosts.Last().Id == tempHost.Id && hostList.Count > 0)
                                        {
                                            if (viewModel.IsNotNull() && allFactors)
                                            {
                                                AddHostListToViewData(viewModel, hostList, dyntaxaSpeciesFactHost.HostLabel);
                                                rowId++;
                                            }
                                            else if (viewModel.IsNotNull())
                                            {
                                               // Don´'t create hostlist for selected factors
                                            }
                                            else 
                                            {
                                                AddHostListToWorkSheetRow(workSheet, rowId, hostList, dyntaxaSpeciesFactHost.HostLabel);
                                                rowId++;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DyntaxaSpeciesFact dyntaxaSpeciesFact = null;
                                string identifier = SpeciesFactModelManager.GetSpeciesFactIdentifier(taxon, individualCategory, factor, null, null);

                                foreach (DyntaxaSpeciesFact speciesFact in factorsForATaxon)
                                {
                                    if (speciesFact.Identifier == identifier)
                                    {
                                        // Data found. Return it.
                                        dyntaxaSpeciesFact = speciesFact;
                                        break;
                                    }
                                }

                                if (dyntaxaSpeciesFact != null)
                                {
                                     IList<DyntaxaFactorField> tempSpeciesFact = new List<DyntaxaFactorField>();
                                     tempSpeciesFact = dyntaxaSpeciesFact.Fields;

                                    // Extract "Kommentar" information to be shown in the comment column, "sant" text and quality
                                    string quality;
                                    DyntaxaFactorField trueText;
                                    IList<KeyValuePair<int, string>> qualities;
                                    DyntaxaFactorField comments = GetCommentsTrueTextQuality(dyntaxaSpeciesFact, tempSpeciesFact, out quality, out trueText, out qualities);

                                    // Create value information to be shown in the value column
                                    string fieldValueString = string.Empty;
                                    fieldValueString = GetFactorFieldValueString(null, fieldValueString, tempSpeciesFact, ref lastStatusStringIsSemiColon);
                                    if (viewModel.IsNotNull() && allFactors)
                                    {
                                        rowId = AddFactorToViewData(viewModel, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFact.Quality.QualityId, qualities, dyntaxaSpeciesFact.ReferenceId, comments, trueText, null, null, fieldValueString, null, null, null, allFactors, false, showNonPublicData, string.Empty);
                                    }
                                    else if (viewModel.IsNotNull() && !allFactors)
                                    {
                                        // Get all avaliable values
                                        DyntaxaFactorFieldValues tempFieldValues = null;
                                        DyntaxaFactorFieldValues tempFieldValues2 = null;
                                        string fieldValueString2 = null;
                                        bool firstSet = false;
                                        foreach (DyntaxaFactorField factFields in tempSpeciesFact)
                                        {
                                            if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && factFields.IsMain)
                                            {
                                                if (!firstSet)
                                                {
                                                    tempFieldValues = factFields.FactorFieldValues;
                                                    firstSet = true;
                                                }
                                            }

                                            if (factFields.DataType == DyntaxaFactorFieldDataTypeId.Enum && !factFields.IsMain)
                                            {
                                                tempFieldValues2 = factFields.FactorFieldValues;
                                                fieldValueString2 = factFields.FactorFieldValues.FieldName;
                                                // use two enum valuse.. This should be nyttjande grad...
                                            }
                                        }
                                        rowId = AddFactorToViewData(viewModel, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFact.Quality.QualityId, qualities, dyntaxaSpeciesFact.ReferenceId, comments, trueText, null, null, fieldValueString, fieldValueString2, tempFieldValues, tempFieldValues2, allFactors, false, showNonPublicData, string.Empty);                 
                                    }
                                    else
                                    {
                                        // Here we create the factor and all it's data to be printed to a row
                                        rowId = AddFactorToWorkSheetRow(workSheet, rowId, individualCategory, factor, null, quality, dyntaxaSpeciesFact.Quality.QualityId, comments, trueText, null, null, fieldValueString, false, showNonPublicData);
                                    }
                                }
                                else
                                {
                                    if (!factor.IsLeaf && !isSubHeaderSet)
                                    {
                                        if (viewModel.IsNotNull())
                                        {
                                            rowId = AddFactorToViewData(viewModel, rowId, individualCategory, factor, null, string.Empty, int.MaxValue, null, int.MaxValue, null, null, null, null, string.Empty, null, null, null, allFactors, false, showNonPublicData, string.Empty);
                                        }
                                        else
                                        {
                                            // Here we create the factor and all it's data to be printed to a row
                                            rowId = AddFactorToWorkSheetRow(workSheet, rowId, individualCategory, factor, null, string.Empty, int.MaxValue, null, null, null, null, string.Empty, false, showNonPublicData, string.Empty, true);
                                        }                                      
                                    }
                                }
                            }
                        } 
                    }
                }
            }
             
            return rowId;
        }

        /// <summary>
        /// Get comments truetest and quality
        /// </summary>
        /// <param name="fact"></param>
        /// <param name="tempSpeciesFact"></param>
        /// <param name="quality"></param>
        /// <param name="trueText"></param>
        /// <returns></returns>
        public static DyntaxaFactorField GetCommentsTrueTextQuality(
            DyntaxaSpeciesFact fact, 
            IList<DyntaxaFactorField> tempSpeciesFact,
            out string quality,
            out DyntaxaFactorField trueText,
            out IList<KeyValuePair<int, string>> qualities)
        {
            DyntaxaFactorField comments = tempSpeciesFact.FirstOrDefault(x => x.Label == "Kommentar");
            if (comments.IsNotNull())
            {
                tempSpeciesFact.Remove(comments);
            }

            // Extract sant information from the rest of the value information(not shown at the moment 2012-07-09)
            trueText = tempSpeciesFact.FirstOrDefault(x => x.Label == "Sant");
            if (trueText.IsNotNull())
            {
                tempSpeciesFact.Remove(trueText);
            }

            // Extract quality information
            quality = string.Empty;
            qualities = new List<KeyValuePair<int, string>>();
            if (fact.Quality.IsNotNull())
            {
                quality = fact.Quality.Quality;
                qualities = fact.Quality.Qualities;
            }
            
            return comments;
        }

        /// <summary>
        /// Gets the host information as string
        /// </summary>
        /// <param name="speciesFactFactors"></param>
        /// <param name="fact"></param>
        /// <param name="factString"></param>
        /// <param name="tempHost"></param>
        /// <returns></returns>
        private static StringBuilder GetHostStringValue(IList<DyntaxaSpeciesFact> speciesFactFactors, DyntaxaSpeciesFact fact, StringBuilder factString, DyntaxaHost tempHost)
        {
            if (tempHost.Id.Equals("0"))
            {
                factString.Append(fact.HostLabel.Substring(0, 1).ToUpper() + fact.HostLabel.Substring(1).ToLower() + ": ospecificerat");
            }
            else if (!(tempHost.Id.IsNull() || tempHost.Id.IsEmpty() || tempHost.Id.Equals("-1")))
            {
                if (speciesFactFactors.Count > 1)
                {
                    factString.Append(tempHost.Label);
                }
                else
                {
                    factString.Append(fact.HostLabel.Substring(0, 1).ToUpper() +
                                      fact.HostLabel.Substring(1).ToLower() + ": " + tempHost.Label);
                }
            }

            return factString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="fieldValueString"></param>
        /// <param name="tempSpeciesFact"></param>
        /// <param name="lastStatusStringIsSemiColon"></param>
        /// <returns></returns>
        public static string GetFactorFieldValueString(DyntaxaPeriod period, string fieldValueString, IList<DyntaxaFactorField> tempSpeciesFact, ref bool lastStatusStringIsSemiColon)
        {
            int index = tempSpeciesFact.Count(field => field.HasValue);
            int tempIndex = 0;
            foreach (DyntaxaFactorField field in tempSpeciesFact)
            {
                if (index == tempIndex)
                {
                    // Create the speciesFact fied string
                    fieldValueString = GetSpeciecFactFieldValue(true, field, fieldValueString, ref lastStatusStringIsSemiColon, period);
                }
                else
                {
                    fieldValueString = GetSpeciecFactFieldValue(false, field, fieldValueString, ref lastStatusStringIsSemiColon, period);
                }
            }

            return fieldValueString;
        }

        /// <summary>
        /// Get and adjust originName value from a factorfield
        /// </summary>
        /// <param name="isLastSpeciesFactField"></param>
        /// <param name="field"></param>
        /// <param name="statusString"></param>
        /// <param name="lastStatusStringIsSemiColon"></param>
        /// <param name="period"></param>
        /// <returns>Modified factor originName value</returns>
        private static string GetSpeciecFactFieldValue(bool isLastSpeciesFactField, DyntaxaFactorField field, string statusString, ref bool lastStatusStringIsSemiColon, DyntaxaPeriod period = null)
        {
            if (field.HasValue)
            {
                if (field.UnitLabel.IsNotEmpty())
                {
                    if (period.IsNotNull())
                    {
                        statusString = statusString + field.Label + " (" + period.Label + " " + field.UnitLabel + ")";
                    }
                    else
                    {
                        statusString = statusString + field.Label + " (" + field.UnitLabel + ")";
                    }
                }
                else
                {
                    if (period.IsNotNull())
                    {
                        statusString = statusString + field.Label + " (" + period.Label + ")";
                    }
                    else
                    {
                        statusString = statusString + field.Label;
                    }
                }

                if (field.DataType == DyntaxaFactorFieldDataTypeId.Boolean)
                {
                    String booleanValue = "Falskt";
                    if (Convert.ToBoolean(field.DataValue))
                    {
                        booleanValue = "Sant";
                    }

                    statusString = statusString + ": " + booleanValue;
                }
                else if (field.DataType == DyntaxaFactorFieldDataTypeId.Int32 ||
                         field.DataType == DyntaxaFactorFieldDataTypeId.Double)
                {
                    statusString = statusString + ": " + field.DataValue;
                }
                else
                {
                    statusString = statusString + ": " + field.DataValue.ToString();
                }

                lastStatusStringIsSemiColon = false;
            }

            if (!isLastSpeciesFactField && statusString.IsNotEmpty())
            {
                if (statusString.Length > 0 && !lastStatusStringIsSemiColon)
                {
                    statusString = statusString + "; ";
                    lastStatusStringIsSemiColon = true;
                }
            }

            return statusString;
        }

        /// <summary>
        /// Creates a host list for a specific factor
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="hostList"></param>
        /// <param name="hostLabel"></param>
        private void AddHostListToWorkSheetRow(ExcelWorksheet workSheet, int rowId, IList<DyntaxaHost> hostList, string hostLabel)
        {
            if (hostList.Count > 0)
            {
                StringBuilder hostTaxaText = new StringBuilder();                
                for (Int32 hostIndex = 0; hostIndex < hostList.Count; hostIndex++)
                {
                    if (hostTaxaText.ToString() != "")
                    {
                        if (hostIndex == hostList.Count - 1)
                        {
                            hostTaxaText.Append(" och ");
                        }
                        else
                        {
                            hostTaxaText.Append(", ");
                        }
                    }

                    hostTaxaText.Append(hostList[hostIndex].ScientificName);
                    if (hostList[hostIndex].CommonName.IsNotEmpty())
                    {
                        hostTaxaText.Append(" (" + hostList[hostIndex].CommonName + ")");
                    }
                }

                String hostString = hostLabel.Substring(0, 1).ToUpper() + hostLabel.Substring(1).ToLower() + " (lista): " + hostTaxaText.ToString();
                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = hostString;
                using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                {
                    range.Style.Font.Color.SetColor(ExcelHelper.ColorTable[int.Parse(DyntaxaSettings.Default.SpeciesFactExcelHostTextColor)]);                    
                }                
            }
        }

        /// <summary>
        /// Creates a host list for a specific factor to view data
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="hostList"></param>
        /// <param name="hostLabel"></param>
        private void AddHostListToViewData(SpeciesFactViewModel viewModel, IList<DyntaxaHost> hostList, string hostLabel)
        {
            if (hostList.Count > 0)
            {
                StringBuilder hostTaxaText = new StringBuilder();
                
                for (Int32 hostIndex = 0; hostIndex < hostList.Count; hostIndex++)
                {
                    if (hostTaxaText.ToString() != "")
                    {
                        if (hostIndex == hostList.Count - 1)
                        {
                            hostTaxaText.Append(" och ");
                        }
                        else
                        {
                            hostTaxaText.Append(", ");
                        }
                    }

                    hostTaxaText.Append(hostList[hostIndex].ScientificName);
                    if (hostList[hostIndex].CommonName.IsNotEmpty())
                    {
                        hostTaxaText.Append(" (" + hostList[hostIndex].CommonName + ")");
                    }
                }

                String hostString = hostLabel.Substring(0, 1).ToUpper() + hostLabel.Substring(1).ToLower() + " (lista): " + hostTaxaText.ToString();
                
                // get last item
                CreateSpeciesFactViewModelItem(viewModel);
                SpeciesFactViewModelItem lastItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();
                
                // get lastItem
                if (lastItem.IsMarked)
                {
                    SpeciesFactViewModelItem item = new SpeciesFactViewModelItem();
                    item.FactorName = hostString;
                    item.IsMarked = true;
                    item.UseDifferentColor = true;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(item);
                }
                else
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorName = hostString;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().UseDifferentColor = true;
                }
            }
        }

        /// <summary>
        /// Create view data classes if null
        /// </summary>
        /// <param name="viewModel"></param>
        private static void CreateSpeciesFactViewModelItem(SpeciesFactViewModel viewModel)
        {
            if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull() || viewModel.SpeciesFactViewModelHeaderItemList.Count == 0)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
                }

                SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);
            }

            if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull() ||
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Count == 0)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList =
                        new List<SpeciesFactViewModelSubHeaderItem>();
                }

                SpeciesFactViewModelSubHeaderItem subItem = new SpeciesFactViewModelSubHeaderItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Add(subItem);
            }

            if (
                viewModel.SpeciesFactViewModelHeaderItemList.Last()
                         .SpeciecFactViewModelSubHeaderItemList.Last()
                         .SpeciesFactViewModelItemList.IsNull() ||
                viewModel.SpeciesFactViewModelHeaderItemList.Last()
                         .SpeciecFactViewModelSubHeaderItemList.Last()
                         .SpeciesFactViewModelItemList.Count == 0)
            {
                if (
                    viewModel.SpeciesFactViewModelHeaderItemList.Last()
                             .SpeciecFactViewModelSubHeaderItemList.Last()
                             .SpeciesFactViewModelItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last()
                             .SpeciecFactViewModelSubHeaderItemList.Last()
                             .SpeciesFactViewModelItemList =
                        new List<SpeciesFactViewModelItem>();
                }

                SpeciesFactViewModelItem item = new SpeciesFactViewModelItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Last()
                         .SpeciecFactViewModelSubHeaderItemList.Last()
                         .SpeciesFactViewModelItemList.Add(item);
            }
        }

        /// <summary>
        /// Ad factor filed value to excel cell
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="statusString"></param>
        private void AddFactorFieldValueToWorkSheetRow(ExcelWorksheet workSheet, int rowId, string statusString)
        {
            if (statusString.IsNotNull() && statusString.IsNotEmpty())
            {
                int index = statusString.LastIndexOf(";");
                int length = statusString.Length;
                if (index >= 0)
                {
                    statusString = statusString.Remove(index);
                }
                    
                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = statusString;
            }
        }

        /// <summary>
        /// Ad factor fieled value to view data
        /// </summary>
        /// <param name="viewItem"></param>
        /// <param name="statusString"></param>
        private void AddFactorFieldValueToViewData(SpeciesFactViewModelItem viewItem, string statusString)
        {
            if (statusString.IsNotNull() && statusString.IsNotEmpty())
            {
                int index = statusString.LastIndexOf(";");
                int length = statusString.Length;
                if (index >= 0)
                {
                    statusString = statusString.Remove(index);
                }

                viewItem.FactorFieldValue = statusString;
            }
        }

        /// <summary>
        /// Ad factor fieled value to view data
        /// </summary>
        /// <param name="viewItem"></param>
        /// <param name="statusString"></param>
        private void AddFactorFieldValue2ToViewData(SpeciesFactViewModelItem viewItem, string statusString)
        {
            if (statusString.IsNotNull() && statusString.IsNotEmpty())
            {
                int index = statusString.LastIndexOf(";");
                int length = statusString.Length;
                if (index >= 0)
                {
                    statusString = statusString.Remove(index);
                }

                viewItem.FactorFieldValue2 = statusString;
            }
        }
       
        /// <summary>
        /// Add comment to excel cell
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="comments"></param>
        private void AddCommentToWorkSheetRow(ExcelWorksheet workSheet, int rowId, DyntaxaFactorField comments)
        {
            if (comments.IsNotNull() && comments.HasValue)
            {
                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_COMMENT].Value = comments.DataValue.ToString();
            }
        }

        /// <summary>
        /// Add comment to view data
        /// </summary>
        /// <param name="viewItem"></param>
        /// <param name="comments"></param>
        private void AddCommentToViewData(SpeciesFactViewModelItem viewItem,  DyntaxaFactorField comments)
        {
            if (comments.IsNotNull() && comments.HasValue)
            {
                viewItem.FactorFieldComment = comments.DataValue.ToString();
            }
        }
        
        /// <summary>
        /// Add true or false and peiod originName value to excel file or view data
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="trueText"></param>
        /// <param name="period"></param>
        private void AddTrueTextToWorkSheetRow(ExcelWorksheet workSheet, int rowId, DyntaxaFactorField trueText, DyntaxaPeriod period)
        {
            if (trueText.IsNotNull() && trueText.HasValue)
            {
                String periodValue = "Falskt";
                if (Convert.ToBoolean(trueText.DataValue))
                {
                    periodValue = "Sant";
                }

                if (period.IsNotNull())
                {
                    periodValue = "(" + period.Label + ") " + periodValue;
                    AddFactorFieldValueToWorkSheetRow(workSheet, rowId, periodValue);
                }         
            } 
        }

        /// <summary>
        /// Add true or false and peiod originName value to excel file or view data
        /// </summary>
        /// <param name="viewItem"></param>
        /// <param name="trueText"></param>
        /// <param name="period"></param>
        private void AddTrueTextToViewData(SpeciesFactViewModelItem viewItem, DyntaxaFactorField trueText, DyntaxaPeriod period)
        {
            if (trueText.IsNotNull() && trueText.HasValue)
            {
                String periodValue = "Falskt";
                if (Convert.ToBoolean(trueText.DataValue))
                {
                    periodValue = "Sant";
                }

                if (period.IsNotNull())
                {
                    periodValue = "(" + period.Label + ") " + periodValue;
                    AddFactorFieldValueToViewData(viewItem, periodValue);
                }
            }
        }
       
        /// <summary>
        /// Create the factor and all its valuse and set them to a  excel cells for a row.
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="individualCategory"></param>
        /// <param name="item"></param>
        /// <param name="period"></param>
        /// <param name="quality"></param>
        /// <param name="qualityId"></param>
        /// <param name="comments"></param>
        /// <param name="trueText"></param>
        /// <param name="factsString"></param>
        /// <param name="hosts"></param>
        /// <param name="fieldValueString"></param>
        /// <param name="firstHost"></param>
        /// <param name="showNoPublicData"></param>
        /// <param name="lastName"></param>
        /// <param name="updateFontSize"></param>
        /// <returns>Row index</returns>
        private int AddFactorToWorkSheetRow(ExcelWorksheet workSheet, int rowId, DyntaxaIndividualCategory individualCategory, DyntaxaFactor item, DyntaxaPeriod period, string quality, int qualityId, DyntaxaFactorField comments, DyntaxaFactorField trueText, StringBuilder factsString, IList<DyntaxaHost> hosts, string fieldValueString, bool firstHost = false, bool showNoPublicData = false, string lastName = "", bool updateFontSize = false)
        {
            bool isPublic = item.IsPublic;
            bool isQualityValid = !(qualityId > 5);
            bool createFactor = false;

            if (isPublic && isQualityValid)
            {
                createFactor = true;
            }
            else if (showNoPublicData)
            {
                createFactor = true;
            }

            if (createFactor)
            {
                if (!lastName.Equals(item.Label) && item.Label.IsNotNull())
                {
                    if (hosts.IsNotNull() && hosts.Count > 1)
                    {                   
                        // Check for first host id so add header else only add specification ie the host
                        if (firstHost)
                        {
                            workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = item.Label;
                            int factorId;
                            if (int.TryParse(item.Id, out factorId))
                            {
                                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = factorId;
                            }
                            else
                            {
                                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = item.Id;
                            }

                            workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER].Value = item.SortOrder;
                            using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                            {
                                range.Style.Font.Bold = true;
                                //range.Font.Bold = true;
                            }
                            
                            rowId++;
                        }

                        workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = factsString.ToString();
                        using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                        {
                            range.Style.Font.Color.SetColor(ExcelHelper.ColorTable[int.Parse(DyntaxaSettings.Default.SpeciesFactExcelHostTextColor)]);                            
                            //range.Font.ColorIndex = DyntaxaSettings.Default.SpeciesFactExcelHostTextColor;
                        }
                    }
                    else
                    {
                        string totFactorName = item.Label;                        
                        using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                        {
                            if (factsString.IsNull())
                            {
                                range.Value = totFactorName;

                                range.Style.Font.Bold = true;
                                if (updateFontSize)
                                {
                                    range.Style.Font.Size = 14;
                                }
                            }
                            else
                            {
                                range.IsRichText = true;
                                ExcelRichText ert = range.RichText.Add(totFactorName);
                                ert.Bold = true;
                                if (updateFontSize)
                                {
                                    ert.Size = 14;
                                }

                                ert = range.RichText.Add(string.Format(" ({0})", factsString));
                                ert.Bold = false;
                                if (updateFontSize)
                                {
                                    ert.Size = 14;
                                }                                

                                ert.Color = ExcelHelper.ColorTable[int.Parse(DyntaxaSettings.Default.SpeciesFactExcelHostTextColor)];
                            }
                        }

                        //string totFactorName = item.Label;
                        //int startIndex = totFactorName.Length;
                        //if (factsString.IsNotNull())
                        //{
                        //    totFactorName = totFactorName + " (" + factsString.ToString() + ")";
                        //}

                        //workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = totFactorName;
                        //using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                        //{
                        //    range.Style.Font.Bold = true;
                        //    if (updateFontSize)
                        //    {
                        //        range.Style.Font.Size = 14;
                        //    }

                        //    //range.Font.Bold = true;
                        //    //if (updateFontSize)
                        //    //    range.Font.Size = 14;

                        //    if (totFactorName.Length > startIndex) //todo
                        //    {
                        //        range.Characters[startIndex + 1, totFactorName.Length].Font.ColorIndex = DyntaxaSettings.Default.SpeciesFactExcelHostTextColor;
                        //        range.Characters[startIndex + 1, totFactorName.Length].Font.Bold = false;
                        //    }

                        //}
                    }
                }

                if (period.IsNotNull() && !lastName.Equals(item.Label))
                {
                }
                  
                AddCommentToWorkSheetRow(workSheet,  rowId, comments);
                AddTrueTextToWorkSheetRow(workSheet, rowId, trueText, period);
                if (quality.IsNotNull())
                {
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.QUALITY].Value = quality;
                }

                // Try to format as number if Id is a number. Probably Id is always a number.
                int id;
                if (int.TryParse(item.Id, out id))
                {
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = id;
                }
                else
                {
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = item.Id;    
                }

                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER].Value = item.SortOrder;
                if (individualCategory.IsNotNull() && !individualCategory.Label.Equals("Generellt"))
                {
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.INDIVIDUAL_CATEGORY].Value = individualCategory.Label;
                }

                AddFactorFieldValueToWorkSheetRow(workSheet, rowId, fieldValueString);
                rowId++;
            }

            return rowId;
        }

        /// <summary>
        /// Create the factor and all its vdta and add them to model.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="rowId"></param>
        /// <param name="individualCategory"></param>
        /// <param name="item"></param>
        /// <param name="period"></param>
        /// <param name="quality"></param>
        /// <param name="qualityId"></param>
        /// <param name="comments"></param>
        /// <param name="trueText"></param>
        /// <param name="factsString"></param>
        /// <param name="hosts"></param>
        /// <param name="fieldValueString"></param>
        /// <param name="firstHost"></param>
        /// <param name="showNoPublicData"></param>
        /// <param name="lastName"></param>
        /// <param name="updateFontSize"></param>
        /// <returns>Row index</returns>
        private int AddFactorToViewData(SpeciesFactViewModel viewModel, int rowId, DyntaxaIndividualCategory individualCategory, DyntaxaFactor item, DyntaxaPeriod period, string quality, int qualityId, IList<KeyValuePair<int, string>> qualities, int referenceId, DyntaxaFactorField comments, DyntaxaFactorField trueText, StringBuilder factsString, IList<DyntaxaHost> hosts, string fieldValueString, string fieldValueString2, DyntaxaFactorFieldValues fieldValues, DyntaxaFactorFieldValues fieldValues2, bool allFactors, bool firstHost, bool showNoPublicData, string lastName)
        {
            bool isPublic = item.IsPublic;
            bool isQualityValid = !(qualityId > 5);
            bool createFactor = false;
            bool viewDataCreated = false;
            
            if (isPublic && isQualityValid)
            {
                createFactor = true;
            }
            else if (showNoPublicData)
            {
                createFactor = true;
            }
            
            if (createFactor)
            {
                if (!lastName.Equals(item.Label) && item.Label.IsNotNull())
                {
                    viewDataCreated = true;
                    if (hosts.IsNotNull() && ((hosts.Count > 1 && allFactors) || (hosts.Count > 0 && !allFactors)))
                    {
                        // Check for first host id so add header else only add specification ie the host
                        if (firstHost)
                        {
                            CreateSpeciesFactViewModelItem(viewModel);
                            
                            // get last item
                            SpeciesFactViewModelItem lastItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();
                          
                            // get lastItem
                            if (lastItem.IsMarked)
                            {
                                SpeciesFactViewModelItem firstHostItem = new SpeciesFactViewModelItem();
                                firstHostItem.FactorName = item.Label;
                                firstHostItem.IsMarked = true;
                                firstHostItem.FactorId = Convert.ToString(item.FactorId);
                                firstHostItem.FactorSortOrder = Convert.ToString(item.SortOrder);
                                
                                if (item.IsHost)
                                {
                                     firstHostItem.IsHost = true;
                                     firstHostItem.HostId = item.HostId;
                                }
                               
                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(firstHostItem);
                                if (hosts.Count == 1 && !allFactors)
                                {
                                    //firstHostItem.HasHost = true;
                                    firstHostItem.FactorId = item.Id;
                                }
                                else if (hosts.Count > 1 && !allFactors)
                                {
                                    //lastItem.HasHost = true;
                                }
                            }
                            else
                            {
                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorName = item.Label;
                                if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorId.Equals(string.Empty))
                                {
                                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorId = Convert.ToString(item.FactorId);
                                }

                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
                                if (item.IsHost)
                                {
                                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsHost = true;
                                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().HostId = item.HostId;
                                }
                            }

                            rowId++;
                        }

                        CreateSpeciesFactViewModelItem(viewModel);
                        
                        // get last item
                        SpeciesFactViewModelItem hostItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();

                        if (hostItem.IsMarked)
                        {
                            SpeciesFactViewModelItem firstHostItem = new SpeciesFactViewModelItem();
                            firstHostItem.FactorName = factsString.ToString();
                            firstHostItem.IsMarked = true;
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(firstHostItem);
                            if (item.IsHost)
                            {
                                firstHostItem.IsHost = true;
                                firstHostItem.HostId = item.HostId;
                                firstHostItem.MainParentFactorId = (int)viewModel.MainParentFactorId;
                            }
                        }
                        else
                        {
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorName = factsString.ToString();
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
                            if (item.IsHost)
                            {
                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsHost = true;
                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().HostId = item.HostId;
                                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().MainParentFactorId = (int)viewModel.MainParentFactorId;
                            }
                        }

                        viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().UseDifferentColor = true;
                    }
                    else
                    {
                        string totFactorName = item.Label;
                        int startIndex = totFactorName.Length;
                        if (factsString.IsNotNull())
                        {
                            totFactorName = totFactorName + " (" + factsString.ToString() + ")";
                        }

                        CreateSpeciesFactViewModelItem(viewModel);

                        // get last item
                        SpeciesFactViewModelItem lastItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();
                   
                        if (lastItem.IsMarked)
                        {
                            SpeciesFactViewModelItem factorItem = new SpeciesFactViewModelItem();
                            factorItem.FactorName = totFactorName;
                            factorItem.IsMarked = true;
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(factorItem);
                        }
                        else
                        {
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorName = totFactorName;
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
                        }

                        if (totFactorName.Length > startIndex)
                        {
                           // Set different color in view from specific index
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().UseDifferentColorFromIndex = startIndex;
                            viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().UseDifferentColor = true;
                        }
                    }
                }

                if (period.IsNotNull() && !lastName.Equals(item.Label))
                {
                }

                if (!viewDataCreated)
                {
                    CreateSpeciesFactViewModelItem(viewModel);

                    // get last item
                    SpeciesFactViewModelItem lastItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();

                    if (lastItem.IsMarked)
                    {
                        SpeciesFactViewModelItem factorItem = new SpeciesFactViewModelItem();
                        factorItem.IsMarked = true;
                        viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(factorItem);
                    }
                    else
                    {
                        viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
                    }
                }

                AddCommentToViewData(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last(), comments);
                AddTrueTextToViewData(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last(), trueText, period);
               
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorId = item.Id;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorSortOrder = Convert.ToString(item.SortOrder);
                if (individualCategory.IsNotNull() && !individualCategory.Label.Equals("Generellt"))
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IndividualCategoryId = individualCategory.Id;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IndividualCategoryName = individualCategory.Label;
                }
                    
                AddFactorFieldValueToViewData(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last(), fieldValueString);         
                if (fieldValues.IsNotNull())
                {
                   SpeciesFactViewModelItemFieldValues modelItemFieldValues = new SpeciesFactViewModelItemFieldValues();
                   modelItemFieldValues.FieldName = fieldValues.FieldName;
                   modelItemFieldValues.FieldValue = fieldValues.FieldValue;
                   modelItemFieldValues.FactorFieldValues = fieldValues.FactorFields;
                   viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FieldValues = modelItemFieldValues;
                   viewModel.FactorFieldValueTableHeader = fieldValues.FieldName;
                   modelItemFieldValues.MainParentFactorId = viewModel.MainParentFactorId;
                   viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().MainParentFactorId = (int)viewModel.MainParentFactorId;
                   // TODO: Refactor to list
                   if (fieldValues2.IsNotNull())
                   {
                       SpeciesFactViewModelItemFieldValues modelItemFieldValues2 = new SpeciesFactViewModelItemFieldValues();
                       modelItemFieldValues2.FieldName = fieldValues2.FieldName;
                       if (fieldValues2.FieldValue.IsNotNull())
                        {
                            modelItemFieldValues2.FieldValue = fieldValues2.FieldValue;
                        }

                        modelItemFieldValues2.FactorFieldValues = fieldValues2.FactorFields;
                       modelItemFieldValues2.FactorFieldValues.Add(new KeyValuePair<string, int>(Resources.DyntaxaResource.SpeciesFactNoValueSet, SpeciesFactModelManager.SpeciesFactNoValueSet));
                       viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FieldValues2 = modelItemFieldValues2;
                       viewModel.FactorFieldValue2TableHeader = fieldValues2.FieldName;
                       modelItemFieldValues2.MainParentFactorId = viewModel.MainParentFactorId;
                    
                       if (fieldValueString2.IsNotNull())
                        {
                            AddFactorFieldValue2ToViewData(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last(), fieldValueString2);
                        }
                    }
               }  

                if (individualCategory.IsNotNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IndividualCategoryId = individualCategory.Id;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IndividualCategoryName = individualCategory.Label;
                }

                if (quality.IsNotNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().Quality = quality;
                }

                if (qualityId.IsNotNull())
                {
                    SpeciesFactViewModelItemFieldValues modelItemFieldValuesQuality = new SpeciesFactViewModelItemFieldValues();
                    modelItemFieldValuesQuality.FieldName = quality;
                    modelItemFieldValuesQuality.FieldValue = qualityId;
                    modelItemFieldValuesQuality.QualityValues = qualities;
                    modelItemFieldValuesQuality.MainParentFactorId = viewModel.MainParentFactorId;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().QualityId = qualityId;
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().QualityValues = modelItemFieldValuesQuality;
                }

                if (referenceId.IsNotNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().ReferenceId = referenceId;
                }

                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().TaxonId = Convert.ToString(viewModel.TaxonId);
                rowId++;
            }

            return rowId;
        }

        /// <summary>
        /// Add first header to excel row (factor header of type main or not)
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="item"></param>
        /// <param name="isMainHeader"></param>
        private void AddHeaderToWorkSheetRow(ExcelWorksheet workSheet, int rowId, DyntaxaFactor item, bool isMainHeader)
        {
            using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER, rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Font.Bold = true;                

                if (isMainHeader)
                {
                    range.Style.Font.Size = 16;
                    range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[DyntaxaSettings.Default.SpeciecFactExcelMainHeaderColor]);                    
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER].Value = item.Label;                    
                }
                else
                {
                    range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[DyntaxaSettings.Default.SpeciecFactExcelHeaderColor]);                    
                    workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SUB_HEADER].Value = item.Label;
                    range.Style.Font.Size = 14;
                }

                //range.Interior.Pattern = XlPattern.xlPatternSolid;
                //range.Font.Bold = true;

                //if (isMainHeader)
                //{
                //    range.Font.Size = 16;
                //    range.Interior.ColorIndex = DyntaxaSettings.Default.SpeciecFactExcelMainHeaderColor;
                //    workSheet.Rows[rowId].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER].Value = item.Label;
                //    // range.Font.ColorIndex = DyntaxaSettings.Default.SpeciecFactExcelMainHeaderTextColor;
                //}
                //else
                //{
                //    range.Interior.ColorIndex = DyntaxaSettings.Default.SpeciecFactExcelHeaderColor;
                //    workSheet.Rows[rowId].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_SUB_HEADER].Value = item.Label;
                //    range.Font.Size = 14;
                //}
            }
         }

        /// <summary>
        /// Add first header to view data
        /// </summary>
        /// <param name="viewItem"></param>
        /// <param name="factor"></param>
        /// <param name="isMainHeader"></param>
        private void AddHeaderToViewData(SpeciesFactViewModel viewModel, DyntaxaFactor factor, bool isMainHeader)
        {
            if (isMainHeader)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
                }

                SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
                headerItem.SpeciesFactViewModelItem = new SpeciesFactViewModelItem();
                headerItem.SpeciesFactViewModelItem.MainHeader = factor.Label;
                headerItem.SpeciesFactViewModelItem.IsMainHeader = true;
                headerItem.SpeciesFactViewModelItem.IsMarked = true;
                viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);
            }
            else
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull() || viewModel.SpeciesFactViewModelHeaderItemList.Count == 0)
                {
                    if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull())
                    {
                        viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
                    }

                    SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
                    viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);
                }

                if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull() ||
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Count == 0)
                {
                    if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull())
                    {
                        viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList =
                            new List<SpeciesFactViewModelSubHeaderItem>();
                    }
                }

                SpeciesFactViewModelSubHeaderItem subItem = new SpeciesFactViewModelSubHeaderItem();
                subItem.SpeciesFactViewModelItem = new SpeciesFactViewModelItem();
                subItem.SpeciesFactViewModelItem.SubHeader = factor.Label;
                subItem.SpeciesFactViewModelItem.IsSubHeader = true;
                subItem.SpeciesFactViewModelItem.IsMarked = true;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Add(subItem);
            }
        }

        /// <summary>
        /// Add second header to viewdata
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="factor"></param>
        private void AddHeaderTwoToViewData(SpeciesFactViewModel viewModel, DyntaxaFactor factor)
        {
            if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull() || viewModel.SpeciesFactViewModelHeaderItemList.Count == 0)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
                }

                SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);
            }

            if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull() ||
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Count == 0)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList =
                        new List<SpeciesFactViewModelSubHeaderItem>();
                }

                SpeciesFactViewModelSubHeaderItem subItem = new SpeciesFactViewModelSubHeaderItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Add(subItem);
            }

            if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.IsNull() ||
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Count == 0)
            {
                if (viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.IsNull())
                {
                    viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList =
                        new List<SpeciesFactViewModelItem>();
                }

                SpeciesFactViewModelItem superiorItem = new SpeciesFactViewModelItem();
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(superiorItem);
            }

            SpeciesFactViewModelItem lastItem = viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last();
            if (lastItem.IsMarked)
            {
                SpeciesFactViewModelItem superiorItem = new SpeciesFactViewModelItem();
                superiorItem.SuperiorHeader = factor.Label;
                superiorItem.FactorId = factor.Id;
                superiorItem.FactorSortOrder = Convert.ToString(factor.SortOrder);
                superiorItem.IsSuperiorHeader = true;
                superiorItem.IsMarked = true;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Add(superiorItem);
            }
            else
            {
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().SuperiorHeader = factor.Label;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorId = factor.Id;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().FactorSortOrder = Convert.ToString(factor.SortOrder);
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsSuperiorHeader = true;
                viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last().SpeciesFactViewModelItemList.Last().IsMarked = true;
            }
        }

        /// <summary>
        /// Add second header to worksheet row (factor sub header ie a factor that has child factors)
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <param name="item"></param>
        private void AddHeaderTwoToWorkSheetRow(ExcelWorksheet workSheet, int rowId, DyntaxaFactor item)
        {
            workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SUPERIOR_HEADER].Value = item.Label;
            int factorId;
            if (int.TryParse(item.Id, out factorId))
            {
                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = factorId;
            }
            else
            {
                workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = item.Id;
            }

            workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER].Value = item.SortOrder;
            // workSheet.Rows[_rowId + _existingColumnCount].Cells[DyntaxaFactorRowExcelColumnIds.INDIVIDUAL_CATEGORY].Value = item.Factor.individualCategory.Label;

            using (ExcelRange range = workSheet.Cells[rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SUPERIOR_HEADER])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 14;
            }            
        }

        /// <summary>
        /// Creates the columheader for excel worksheet
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        /// <returns>Row index</returns>
        private int CreateColumnHeaders(ExcelWorksheet workSheet, int rowId)
        {
            int row = rowId;

            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeader1;
            
            //workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_SUB_HEADER].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeader2;
            //workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_SUPERIOR_HEADER].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderSuperior;
            //workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderSupressed;
            //workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.HOST].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderHost;
            //workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.PERIOD].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderPeriod;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.INDIVIDUAL_CATEGORY].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderIndividualCategori;
           // workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_SPECIFICATION].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderSpecification;
           // workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderField;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_COMMENT].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderComment;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.QUALITY].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderQuality;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.FACTOR_ID].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderFactorID;
            workSheet.Cells[1, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER].Value = Resources.DyntaxaResource.SpeciesFactExcelListHeaderSortOrder;

            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER, row, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[DyntaxaSettings.Default.SpeciecFactExcelTableHeaderColor]);
                //range.Font.Bold = true;
                //range.Interior.ColorIndex = DyntaxaSettings.Default.SpeciecFactExcelTableHeaderColor;
                //range.Interior.Pattern = XlPattern.xlPatternSolid;
            }
            //string cellAddress1 = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER].Address(false, false);
            //string cellAddress2 = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER].Address(false, false);
            //Range range = workSheet.Range[cellAddress1, cellAddress2];

            row++;          
            return row;
        }

        /// <summary>
        /// Create the taxon information to be added ti the excel worksheet
        /// </summary>
        /// <param name="user"></param>
        /// <param name="workSheet"></param>
        /// <param name="taxon"></param>
        /// <param name="swedishOccuranceInfo"></param>
        /// <param name="swedishHistoryInfo"></param>
        /// <param name="rowId"></param>
        /// <returns>Row index</returns>
        private int CreateTaxonInfoRows(IUserContext user, ExcelWorksheet workSheet, ITaxon taxon, string swedishOccuranceInfo, string swedishHistoryInfo, int rowId)
        {
            int row = rowId;
          
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER].Value = Resources.DyntaxaResource.SpeciesFactExcelListInfoHeader;            
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER, row, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER])
            {                
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[int.Parse(DyntaxaSettings.Default.SpeciecFactExcelTableInfoHeaderColor)]);                
                range.Style.Font.Size = 14;
                //range.Font.Bold = true;
                //range.Interior.ColorIndex = DyntaxaSettings.Default.SpeciecFactExcelTableInfoHeaderColor;
                //range.Font.Size = 14;
                //range.Interior.Pattern = XlPattern.xlPatternSolid;
            }

            row++;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryIdLabel;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {                
                range.Style.Font.Bold = true;
            }
            
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = taxon.Id.ToString();
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            {                
                range.Style.Numberformat.Format = "@";
                //range.NumberFormat = "@";
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryGuidLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = taxon.Guid;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;    
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryValidityLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = GetValidityDescription(taxon);
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;    
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryConceptDefinitionLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = taxon.GetConceptDefinition(user);
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;    
            }
            
            row++;

            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Value = Resources.DyntaxaResource.TaxonSummaryCategoryLabel;
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;    
            }

            //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryCategoryLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = taxon.Category.Name;

            row++;

// START
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Value = Resources.DyntaxaResource.TaxonSummaryScientificNameLabel;
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;    
            }
            //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryScientificNameLabel;
            
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            {
                //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = result.ToString();
                range.IsRichText = true;
                ExcelRichText ert;

                ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(), TaxonCategoryId.Genus);
                bool isNameBold = false;
                if (taxon.SortOrder >= genusTaxonCategory.SortOrder)
                {
                    ert = range.RichText.Add(taxon.ScientificName);
                    ert.Bold = true;
                    ert.Italic = true;

                    // result.Append(taxon.ScientificName);
                    isNameBold = true;
                }
                else
                {
                    ert = range.RichText.Add(taxon.ScientificName);
                    //ert.Bold = false;

                    //result.Append(taxon.ScientificName);
                }

                if (!string.IsNullOrEmpty(taxon.Author))
                {
                    ert = range.RichText.Add(string.Format(" {0}", taxon.Author));                    
                    ert.Bold = isNameBold;
                    ert.Italic = false;

                    //result.Append(" ");
                    //result.Append(taxon.Author);
                }

                //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryScientificNameLabel;
                //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = result.ToString();
                //using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
                //{
                //    range.Style.Font.Bold = true;
                //    //range.Font.Bold = true;    
                //}

                //using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
                //{
                    //int scientificNameLength = taxon.ScientificName.Length;
                    //if (isNameBold)
                    //{
                    //    range.Style.Font.Bold = true;
                    //    range.Style.Font.Italic = true;
                    //    //range.Font.Bold = true;
                    //    //range.Characters[1, scientificNameLength].Font.Italic = true;
                    //}
                //}

                //ert = range.RichText.Add(string.Format("{0} ", relatedTaxon.Category.Name));
                //ert.Bold = false;
                //ert = range.RichText.Add(relatedTaxon.ScientificName);
                //ert.Bold = true;
            }

// GAMMAL KOD

            StringBuilder result = new StringBuilder();

            //ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(), TaxonCategoryId.Genus);
            //bool isNameBold = false;
            //if (taxon.SortOrder >= genusTaxonCategory.SortOrder)
            //{
            //    result.Append(taxon.ScientificName);
            //    isNameBold = true;
            //}
            //else
            //{
            //    result.Append(taxon.ScientificName);
            //}

            //if (!string.IsNullOrEmpty(taxon.Author))
            //{
            //    result.Append(" ");
            //    result.Append(taxon.Author);
            //}

            //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryScientificNameLabel;
            //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = result.ToString();
            //using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            //{
            //    range.Style.Font.Bold = true;
            //    //range.Font.Bold = true;    
            //}

            //using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            //{                
            //    int scientificNameLength = taxon.ScientificName.Length;
            //    if (isNameBold)
            //    {
            //        range.Style.Font.Bold = true;
            //        range.Style.Font.Italic = true;
            //        //range.Font.Bold = true;
            //        //range.Characters[1, scientificNameLength].Font.Italic = true;
            //    }
            //}

// SLUT

            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummarySynonymsLabel;
            IList<ITaxonName> names = taxon.GetSynonyms(user, true);
            StringBuilder builder = new StringBuilder();
            if (names.Count == 0)
            {
                builder.Append("-");
            }
            else
            {
                foreach (ITaxonName taxonName in names)
                {
                    if (names.Last().Id == taxonName.Id)
                    {
                        builder.Append(taxonName.Name);
                    }
                    else
                    {
                        builder.Append(taxonName.Name + ",");
                    }
                }
            }
           
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = builder.ToString();
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;     
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryCommonNamesLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = taxon.CommonName;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;     
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummaryClassificationLabel;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;     
            }
            
            // Classification
            var allParentTaxa = taxon.GetAllParentTaxonRelations(user, null, false, false, true);
            var distinctParentTaxa = allParentTaxa.GroupBy(x => x.ParentTaxon.Id).Select(x => x.First().ParentTaxon).ToList();
            StringBuilder classificationBuilder = new StringBuilder();
            IDictionary<int, int> boldString = new Dictionary<int, int>();
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            {               
                range.IsRichText = true;
                ExcelRichText ert;
                foreach (ITaxon relatedTaxon in distinctParentTaxa)
                {
                    if (relatedTaxon.Category.IsTaxonomic)
                    {
                        if (distinctParentTaxa.Last().Id == relatedTaxon.Id)
                        {
                            //classificationBuilder.Append(relatedTaxon.Category.Name + " ");
                            //int startIndex = classificationBuilder.Length + 1;
                            //classificationBuilder.Append(relatedTaxon.ScientificName);
                            //int stopIndex = classificationBuilder.Length;
                            //boldString.Add(startIndex, stopIndex);

                            ert = range.RichText.Add(string.Format("{0} ", relatedTaxon.Category.Name));
                            ert.Bold = false;
                            ert = range.RichText.Add(relatedTaxon.ScientificName);
                            ert.Bold = true;
                        }
                        else
                        {
                            //classificationBuilder.Append(relatedTaxon.Category.Name + " ");
                            //int startIndex = classificationBuilder.Length + 1;
                            //classificationBuilder.Append(relatedTaxon.ScientificName);
                            //int stopIndex = classificationBuilder.Length;
                            //boldString.Add(startIndex, stopIndex);
                            //classificationBuilder.Append(", ");

                            ert = range.RichText.Add(string.Format("{0} ", relatedTaxon.Category.Name));
                            ert.Bold = false;
                            ert = range.RichText.Add(relatedTaxon.ScientificName);
                            ert.Bold = true;
                            ert = range.RichText.Add(", ");
                            ert.Bold = false;
                        }
                    }
                }
            }

            //workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = classificationBuilder.ToString();
          
            //if (distinctParentTaxa.Count > 0)
            //{
            //    foreach (KeyValuePair<int, int> i in boldString)
            //    {
            //        using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            //        {
            //            //todo använd richtext                        

            //            int startValue = i.Key;
            //            int stopValue = i.Value;
            //            //range.Characters[startValue, stopValue - startValue + 1].Font.Bold = true; //todo
            //        }
            //    }
            //}
                           
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummarySwedishOccurrenceLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = swedishOccuranceInfo;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.TaxonSummarySwedishHistoryLabel;
            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = swedishHistoryInfo;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;
            }
            
            row++;

            workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME].Value = Resources.DyntaxaResource.SpeciesFactExcelListURLInfo;
            using (ExcelRange range = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_NAME])
            {
                range.Style.Font.Bold = true;
                //range.Font.Bold = true;
            }            

            // Write Hyperlink 
            string link = Resources.DyntaxaSettings.Default.UrlToDyntaxaInfo.Replace("[TaxonId]", taxon.Id.ToString());
            
            // workSheet.Rows[row].Cells[DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE].Value = link;
            
            using (ExcelRange cell = workSheet.Cells[row, DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE])
            {
                cell.Value = link;
                cell.Style.Font.UnderLine = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                cell.Hyperlink = new Uri(link, UriKind.Absolute);                        
            }
     
            row++;
            return row;
        }

        /// <summary>
        /// Checks if the taxon is valid ie gets information for taxon information to be shown in excelfile
        /// </summary>
        /// <param name="taxon"></param>
        /// <returns></returns>
        private static string GetValidityDescription(ITaxon taxon)
        {
            string text;
            if (taxon.IsValid)
            {
                if (taxon.Category.IsTaxonomic)
                {
                    text = Resources.DyntaxaResource.TaxonSummaryValitityValueAccepted;
                }
                else
                {
                    text = Resources.DyntaxaResource.TaxonSummaryValitityValuePragmatic;
                }
            }
            else
            {
                text = Resources.DyntaxaResource.TaxonSummaryValitityValueNotValid;
                var validToInformation = string.Format("{0} ({1})", taxon.ValidToDate.ToShortDateString(), taxon.ModifiedByPerson);
                text = text + ". " + Resources.DyntaxaResource.TaxonSummaryValidToLabel + " " + validToInformation;
            }

            return text;
        }

        /// <summary>
        /// Format all columns in correct way...
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="rowId"></param>
        private void FormatColumns(ExcelWorksheet workSheet, int rowId)
        {
            //string cellAddressStart = workSheet.Cells[1, 1].Address(false, false);
            //string cellAddressEnd = workSheet.Cells[1, _existingColumnCount].Address(false, false);
            //Range headerRange = workSheet.Range[cellAddressStart, cellAddressEnd];
            //headerRange.Font.Bold = true;

            using (ExcelRange formatRange = workSheet.Cells[1, 1, rowId, DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER])
            {
                //formatRange.AutoFitColumns();                
                // todo - EPPlus doesn't support row autofit.
                //formatRange.Rows.AutoFit();
                for (int i = 1; i <= DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER; i++)
                {
                    ExcelColumn column = workSheet.Column(i);
                    if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER || i == DyntaxaFactorRowExcelColumnIds.FACTOR_SUB_HEADER)
                    {
                        column.Width = 5.0; //7.14;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;                        
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_SUPERIOR_HEADER)
                    {
                        column.Width = 2.5; //17.71;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        //column.WrapText = true; 
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_NAME)
                    {
                        column.Width = 80;
                        column.Style.WrapText = true;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE)
                    {
                        column.Width = 80;
                        column.Style.WrapText = true;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    //else if (i == DyntaxaFactorRowExcelColumnIds.HOST)
                    //    column.AutoFit();
                    //else if (i == DyntaxaFactorRowExcelColumnIds.PERIOD)
                    //    column.AutoFit();
                    else if (i == DyntaxaFactorRowExcelColumnIds.INDIVIDUAL_CATEGORY)
                    {
                        column.AutoFit();
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    //else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_SPECIFICATION)
                    //{
                    //    column.ColumnWidth = 32.0;
                    //    column.WrapText = true;
                    //}
                    //else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD)
                    //{
                    //    column.ColumnWidth = 17.71;
                    //    column.WrapText = true;
                    //}
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_COMMENT)
                    {                        
                        column.Width = 32.0;
                        column.Style.WrapText = true;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.QUALITY)
                    {
                        column.AutoFit();
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_ID)
                    {
                        column.AutoFit();
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER)
                    {
                        column.Width = 16.0;
                        column.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                }

                //for (int i = 1; i <= formatRange.Columns.Count; i++)
                //{
                //    Range column = formatRange.Columns[i];
                //    if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_MAIN_HEADER || i == DyntaxaFactorRowExcelColumnIds.FACTOR_SUB_HEADER)
                //    {
                //        column.ColumnWidth = 5.0;//7.14;
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }
                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_SUPERIOR_HEADER)
                //    {
                //        column.ColumnWidth = 2.5;//17.71;
                //        column.VerticalAlignment = VerticalAlign.Top;
                //        //column.WrapText = true; 
                //    }
                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_NAME)
                //    {
                //        column.ColumnWidth = 80;
                //        column.WrapText = true;
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }
                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_VALUE)
                //    {
                //        column.ColumnWidth = 80;
                //        column.WrapText = true;
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }

                //    //else if (i == DyntaxaFactorRowExcelColumnIds.HOST)
                //    //    column.AutoFit();
                //    //else if (i == DyntaxaFactorRowExcelColumnIds.PERIOD)
                //    //    column.AutoFit();
                //    else if (i == DyntaxaFactorRowExcelColumnIds.INDIVIDUAL_CATEGORY)
                //    {
                //        column.AutoFit();
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }

                //    //else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_SPECIFICATION)
                //    //{
                //    //    column.ColumnWidth = 32.0;
                //    //    column.WrapText = true;
                //    //}
                //    //else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD)
                //    //{
                //    //    column.ColumnWidth = 17.71;
                //    //    column.WrapText = true;
                //    //}
                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_FIELD_COMMENT)
                //    {
                //        column.ColumnWidth = 32.0;
                //        column.WrapText = true;
                //        column.VerticalAlignment = VerticalAlign.Top;

                //    }
                //    else if (i == DyntaxaFactorRowExcelColumnIds.QUALITY)
                //    {

                //        column.AutoFit();
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }   // AUTO fit

                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_ID)
                //    {

                //        column.AutoFit();
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }    // AUTO fit

                //    else if (i == DyntaxaFactorRowExcelColumnIds.FACTOR_SORT_ORDER)
                //    {
                //        column.ColumnWidth = 16.0;
                //        column.VerticalAlignment = VerticalAlign.Top;
                //    }
                //}
            }
        }

        /// <summary>
        /// Definition of factor column ids.
        /// </summary>
        public struct DyntaxaFactorRowExcelColumnIds
        {
            /// <summary>Main header.</summary>
            public const Int32 FACTOR_MAIN_HEADER = 1;

            /// <summary>Sub header.</summary>
            public const Int32 FACTOR_SUB_HEADER = 2;

            /// <summary>Sub header.</summary>
            public const Int32 FACTOR_SUPERIOR_HEADER = 3;

            /// <summary>Factor name.</summary>
            public const Int32 FACTOR_NAME = 4;

            /// <summary>Factor originName value</summary>
            public const Int32 FACTOR_FIELD_VALUE = 5;
            // /// <summary>Period.</summary>
            //public const Int32 PERIOD = 6;

            /// <summary>Individual category.</summary>
            public const Int32 INDIVIDUAL_CATEGORY = 6;
            ///// <summary>Factor originName specification</summary>
            //public const Int32 FACTOR_FIELD_SPECIFICATION = 7;
            ///// <summary>Factor originName.</summary>
            //public const Int32 FACTOR_FIELD = 9;

             /// <summary>Factor originName comment</summary>
            public const Int32 FACTOR_FIELD_COMMENT = 7;

            /// <summary>QUALITY.</summary>
            public const Int32 QUALITY = 8;

            /// <summary>Factor id.</summary>
            public const Int32 FACTOR_ID = 9;

            /// <summary>Factor sort order.</summary>
            public const Int32 FACTOR_SORT_ORDER = 10;
            ///// <summary>Host.</summary>
          // public const Int32 HOST = 5;
        }
    }
}
