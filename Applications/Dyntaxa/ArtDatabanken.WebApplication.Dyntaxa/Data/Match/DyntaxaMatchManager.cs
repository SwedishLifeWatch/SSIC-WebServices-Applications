using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.IO;
using System.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using System.IO;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    /// <summary>
    /// A class that handles matching of taxa by names.
    /// </summary>
    public class DyntaxaMatchManager
    {
        private readonly IUserContext _user;

        public DyntaxaMatchManager(IUserContext user)
        {
            _user = user;
        }

        public MemoryStream CreateExcelFile(MatchSettingsViewModel options, List<DyntaxaMatchItem> items, ExcelFileFormat fileFormat)
        {
            var excelFile = new MatchExcelFile();
            var dataTable = GetResultTable(items, options);
            return excelFile.CreateExcelFile(dataTable, fileFormat, true);
        }

        /// <summary>
        /// The general matching algorithm.
        /// This method is called by all the other methods in order to get the match result.
        /// </summary>
        /// <param name="item">The match item.</param>
        /// <param name="options">Match options.</param>
        /// <returns></returns>
        private DyntaxaMatchItem GetMatch(DyntaxaMatchItem item, MatchSettingsViewModel options)
        {            
            item.Status = MatchStatus.NoMatch;
            if (options.ColumnContentAlternative == MatchColumnContentAlternative.NameAndAuthorCombined)
            {
                //TODO: split name and author
                /*
                StringBuilder name = new StringBuilder();
                StringBuilder author = new StringBuilder();
                bool nameIsCompleted = false;
                string[] stringArray = item.NameString.Split(' ');
                for (int i = 0; i < stringArray.Length; i++)
                {
                    if (!nameIsCompleted)
                    {
                        if (name.Length > 0)
                        {
                            name.Append(" ");
                            
                        }
                        name.Append(stringArray[i]);
                    }
                }
                 */
            }

            //ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
            //taxonSearchCriteria.TaxonName = item.NameString;
            ////taxonSearchCriteria.TaxonIds = 
            ////taxonSearchCriteria.TaxonCategoryIds =
            //CoreData.TaxonManager.GetTaxaBySearchCriteria(_user, taxonSearchCriteria);

            //var taxonNameSearchCriteria = new TaxonNameSearchCriteria();
            //taxonNameSearchCriteria.NameSearchString = new StringSearchCriteria();
            //taxonNameSearchCriteria.NameSearchString.SearchString = item.NameString;
            //taxonNameSearchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            //taxonNameSearchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Iterative);
            
            //if (options.LimitToTaxon && options.LimitToParentTaxonId.HasValue)
            //{                
            //    taxonNameSearchCriteria.TaxonIdList = new List<int> { options.LimitToParentTaxonId.Value };
            //}
            options.SearchOptions.NameSearchString = item.NameString;
            TaxonNameSearchCriteria taxonNameSearchCriteria = options.SearchOptions.CreateTaxonNameSearchCriteriaObject();
            if (options.MatchToType == MatchTaxonToType.TaxonNameAndAuthor)
            {
                taxonNameSearchCriteria.IsAuthorIncludedInNameSearchString = true;
            }            
            TaxonNameList taxonNames = CoreData.TaxonManager.GetTaxonNames(_user, taxonNameSearchCriteria);

            if (taxonNames.IsNotEmpty())
            {
                var dicTaxonNames = new Dictionary<int, ITaxonName>();
                foreach (ITaxonName taxonName in taxonNames)
                {
                    if (taxonName.Status.Id == (int)TaxonNameStatusId.Removed)
                    {
                        continue;
                    }

                    if (!dicTaxonNames.ContainsKey(taxonName.Taxon.Id))
                    {
                        dicTaxonNames.Add(taxonName.Taxon.Id, taxonName);
                    }
                }

                if (dicTaxonNames.Count == 1)
                {
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, dicTaxonNames.Keys.First());                                        
                    item.SetTaxon(taxon, options);
                    item.Status = MatchStatus.Exact;
                }
                else if (dicTaxonNames.Count > 1)
                {
                    item.Status = MatchStatus.NeedsManualSelection;
                    item.DropDownListIdentifier = "AlternativeTaxa" + item.RowNumber.ToString();
                    item.AlternativeTaxa = new TaxonSelectList(taxonNames).GetList(); // todo - den här ska vara med
                }
            }
            else
            {
                //TODO: Using som fuzzy algorithms to obtain taxon suggestions.
            }
            return item;
        }

        /// <summary>
        /// A method that perform matches of taxon identifiers provided in a list.
        /// </summary>
        /// <param name="items">List of match items or identifiers that should be matched with taxon concepts in Dyntaxa.</param>
        /// <param name="options">Options determining the matching process.</param>
        /// <returns>A list of matches</returns>
        public List<DyntaxaMatchItem> GetMatches(List<DyntaxaMatchItem> items, MatchSettingsViewModel options)
        {
            var matches = new List<DyntaxaMatchItem>();            
            int row = 1;
            foreach (var item in items)
            {
                item.RowNumber = row++;
                matches.Add(item.Status.Equals(MatchStatus.Undone) ? GetMatch(item, options) : item);
            }            
            return matches;
        }

        /// <summary>
        /// Method that generate a list och match items based on a single string.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="options">Options determining the match proces.</param>
        /// <returns></returns>
        public List<DyntaxaMatchItem> GetDyntaxaMatchItemsFromText(string text, MatchSettingsViewModel options)
        {
            string[] stringArray = null;
            switch (options.RowDelimiter)
            {
                case MatchTaxonRowDelimiter.ReturnLinefeed:
                    stringArray = options.ClipBoard.Split('\n');
                    break;

                case MatchTaxonRowDelimiter.Semicolon:
                    stringArray = options.ClipBoard.Split(';');
                    break;

                case MatchTaxonRowDelimiter.Tab:
                    stringArray = options.ClipBoard.Split('\t');
                    break;

                case MatchTaxonRowDelimiter.VerticalBar:
                    stringArray = options.ClipBoard.Split('|');
                    break;

                default:
                    stringArray = options.ClipBoard.Split('\n');
                    break;
            }
            return GetMatches(stringArray, options);
        }

        /// <summary>
        /// A method that perform matches of taxon identifiers provided in a list.
        /// </summary>
        /// <param name="filePath">Full file name including its path of an excel file including the provided list of taxon identifiers.</param>
        /// <param name="options">Options determining the matching process.</param>
        /// <returns>A list of matches.</returns>
        public List<DyntaxaMatchItem> GetMatches(string filePath, MatchSettingsViewModel options)
        {
            string strName;
            var list = new List<DyntaxaMatchItem>();
            //var file = new ExcelFile(filePath, options.IsFirstRowColumnName);
            var file = new XlsxExcelFile(filePath, options.IsFirstRowColumnName);
            DataTable table = file.DataTable;
            if (table.Rows.Count > 0 && table.Columns.Count > 0)
            {
                if (options.MatchToType == MatchTaxonToType.TaxonId)
                {
                    var ids = new List<Int32>();
                    foreach (DataRow row in table.Rows)
                    {
                        int id;                        
                        if (Int32.TryParse(row[0].ToString(), out id))
                        {
                            ids.Add(id);
                        }                        
                    }
                    list = this.GetMatches(ids, options);
                }
                else if (options.MatchToType == MatchTaxonToType.TaxonNameAndAuthor)
                {
                    if (table.Columns.Count >= 2)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            string strColumn1 = row[0].ToString();
                            string strColumn2 = row[1].ToString();
                            if (string.IsNullOrEmpty(strColumn1))
                            {
                                strName = strColumn2;
                            }
                            else if (string.IsNullOrEmpty(strColumn2))
                            {
                                strName = strColumn1;
                            }
                            else
                            {
                                strName = string.Format("{0} {1}", row[0], row[1]).Trim();
                            }

                            if (!string.IsNullOrEmpty(strName))
                            {
                                var matchItem = new DyntaxaMatchItem(strName);
                                list.Add(matchItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            strName = row[0].ToString();
                            if (!string.IsNullOrEmpty(strName))
                            {
                                var matchItem = new DyntaxaMatchItem(strName);
                                list.Add(matchItem);    
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in table.Rows)
                    {
                        strName = row[0].ToString();
                        if (!string.IsNullOrEmpty(strName))
                        {
                            var matchItem = new DyntaxaMatchItem(strName);
                            list.Add(matchItem);
                        }
                    }
                }
            }
            return GetMatches(list, options);
        }

        /// <summary>
        /// A method that perform matches of taxon identifiers provided in a list.
        /// </summary>
        /// <param name="items">Array of strings representing names or identifiers that should be matched with taxon concepts in Dyntaxa.</param>
        /// <param name="options">Options determining the matching process.</param>
        /// <returns>A list of matches</returns>
        public List<DyntaxaMatchItem> GetMatches(string[] items, MatchSettingsViewModel options)
        {
            var list = new List<DyntaxaMatchItem>();
            if (options.MatchToType == MatchTaxonToType.TaxonId)
            {
                var ids = new List<Int32>();
                foreach (var item in items)
                {
                    Int32 id;
                    if (Int32.TryParse(item, out id))
                    {
                        ids.Add(id);
                    }
                }
                return this.GetMatches(ids, options);
            }
            else
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    string item = items[i];
                    if (!((i == items.Count() - 1) && item == ""))
                    {
                        DyntaxaMatchItem matchItem;
                        if (options.ColumnContentAlternative == MatchColumnContentAlternative.NameAndAuthorInSeparateColumns)
                        {
                            string[] stringArray = null;
                            switch (options.ColumnDelimiter)
                            {
                                case MatchTaxonColumnDelimiter.Semicolon:
                                    stringArray = item.Split(';');
                                    break;

                                case MatchTaxonColumnDelimiter.Tab:
                                    stringArray = item.Split('\t');
                                    break;

                                case MatchTaxonColumnDelimiter.VerticalBar:
                                    stringArray = item.Split('|');
                                    break;

                                default:
                                    stringArray = item.Split('\t');
                                    break;
                            }

                            if (stringArray.Length > 1)
                            {
                                matchItem = new DyntaxaMatchItem(stringArray[0], stringArray[1]);
                            }
                            else
                            {
                                matchItem = new DyntaxaMatchItem(item);
                            }
                        }
                        else
                        {
                            matchItem = new DyntaxaMatchItem(item);
                        }
                        list.Add(matchItem);
                    }
                }
            }
            return GetMatches(list, options);
        }

        public List<DyntaxaMatchItem> GetMatches(List<Int32> taxonIds, MatchSettingsViewModel options)
        {
            var list = new List<DyntaxaMatchItem>();

            // todo - it would be nice if GetTaxaByIds don't throw Exception on non existing taxon ids.
            //var taxa = CoreData.TaxonManager.GetTaxaByIds(_user, taxonIds);

            int row = 1;
            foreach (Int32 id in taxonIds)
            {
                var matchItem = new DyntaxaMatchItem(id);
                matchItem.RowNumber = row++;
                ITaxon taxon = null;

                try
                {
                    taxon = CoreData.TaxonManager.GetTaxon(_user, id);
                }
                catch { }

                if (taxon != null)
                {
                    matchItem.SetTaxon(taxon, options);
                    matchItem.Status = MatchStatus.Exact;
                }
                else
                {
                    matchItem.Status = MatchStatus.NoMatch;
                }

                list.Add(matchItem);
            }

            return list;
        }

        public List<DyntaxaMatchItem> GetMatchProblems(List<DyntaxaMatchItem> items)
        {
            var matches = new List<DyntaxaMatchItem>();

            foreach (var item in items)
            {
                if (item.Status == MatchStatus.NeedsManualSelection || item.Status == MatchStatus.NoMatch)
                {
                    matches.Add(item);
                }
            }
            return matches;
        }

        public List<DyntaxaMatchItem> GetMatchResults(List<DyntaxaMatchItem> items, MatchSettingsViewModel options)
        {
            foreach (DyntaxaMatchItem item in items)
            {
                if (item.Status == MatchStatus.NeedsManualSelection)
                {
                    if (item.TaxonId != 0)
                    {
                        try
                        {
                            ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, item.TaxonId);
                            item.SetTaxon(taxon, options);
                            item.Status = MatchStatus.ManualSelection;

                            if (options.OutputParentTaxa)
                            {
                                item.ParentTaxa = GetParenTaxa(taxon);
                            }

                            if (options.OutputScientificSynonyms)
                            {
                                item.ScientificSynonyms = GetScientificSynonyms(taxon);
                            }
                        }
                        catch (Exception)
                        {
                            // int x = 8;
                        }
                    }
                }
                else if (item.Status == MatchStatus.Exact || item.Status == MatchStatus.ManualSelection)
                {
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, item.TaxonId);
                    //item.SetTaxon(taxon);                    

                    if (options.OutputParentTaxa)
                    {
                        item.ParentTaxa = GetParenTaxa(taxon);
                    }

                    if (options.OutputScientificSynonyms)
                    {
                        item.ScientificSynonyms = GetScientificSynonyms(taxon);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Procedure that writes match results into a DataTable.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataTable GetResultTable(List<DyntaxaMatchItem> items, MatchSettingsViewModel options)
        {
            var table = new DataTable();
            //Provided
            var column = new DataColumn();
            column.Caption = options.LabelForProvidedText;
            column.ColumnName = "Provided";
            if (options.MatchToType == MatchTaxonToType.TaxonId)
            {
                column.DataType = typeof(Int32);
            }
            table.Columns.Add(column);

            //Match status
            column = new DataColumn();
            column.Caption = Resources.DyntaxaResource.MatchOptionsOutputMatchStatusLabel;
            column.ColumnName = "MatchStatus";
            table.Columns.Add(column);

            //Taxon id
            if (options.OutputTaxonId)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputTaxonIdLabel;
                column.ColumnName = "TaxonId";
                column.DataType = typeof(Int32);
                table.Columns.Add(column);
            }

            //Recommended scientific name
            if (options.OutputScientificName)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputScientificNameLabel;
                column.ColumnName = "ScientificName";
                table.Columns.Add(column);
            }

            //Author of recommended scientific name
            if (options.OutputAuthor)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputAuthorLabel;
                column.ColumnName = "Author";
                table.Columns.Add(column);
            }

            //Recommended common name
            if (options.OutputCommonName)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputCommonNameLabel;
                column.ColumnName = "CommonName";
                table.Columns.Add(column);
            }

            //Taxon category
            if (options.OutputTaxonCategory)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputTaxonCetegoryLabel;
                column.ColumnName = "TaxonCategory";
                table.Columns.Add(column);
            }

            //Scientific Synonyms
            if (options.OutputScientificSynonyms)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputScientificSynonymsLabel;
                column.ColumnName = "ScientificSynonyms";
                table.Columns.Add(column);
            }

            //Parent taxa
            if (options.OutputParentTaxa)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputParentTaxaLabel;
                column.ColumnName = "ParentTaxa";
                table.Columns.Add(column);
            }

            //GUID
            if (options.OutputGUID)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.MatchOptionsOutputGUIDLabel;
                column.ColumnName = "GUID";
                table.Columns.Add(column);
            }

            // Recommended GUID
            if (options.OutputRecommendedGUID)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.ExportStraightColumnRecommendedGUID;
                column.ColumnName = "RecommendedGUID";
                table.Columns.Add(column);
            }

            // Swedish occurrence
            if (options.OutputSwedishOccurrence)
            {
                column = new DataColumn();
                column.Caption = Resources.DyntaxaResource.ExportStraightSwedishOccurrence;
                column.ColumnName = "SwedishOccurrence";
                table.Columns.Add(column);
            }
            foreach (DyntaxaMatchItem item in items)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Provided"] = item.ProvidedText;
                table.Rows[table.Rows.Count - 1]["MatchStatus"] = item.Status;
                if (item.Status == MatchStatus.Exact || item.Status == MatchStatus.Fuzzy || item.Status == MatchStatus.ManualSelection)
                {
                    if (options.OutputTaxonId)
                    {
                        table.Rows[table.Rows.Count - 1]["TaxonId"] = item.TaxonId;
                    }

                    if (options.OutputScientificName)
                    {
                        table.Rows[table.Rows.Count - 1]["ScientificName"] = item.ScientificName;
                    }

                    if (options.OutputAuthor)
                    {
                        table.Rows[table.Rows.Count - 1]["Author"] = item.Author;
                    }

                    if (options.OutputCommonName)
                    {
                        table.Rows[table.Rows.Count - 1]["CommonName"] = item.CommonName;
                    }

                    if (options.OutputTaxonCategory)
                    {
                        table.Rows[table.Rows.Count - 1]["TaxonCategory"] = item.TaxonCategory;
                    }

                    if (options.OutputScientificSynonyms)
                    {
                        table.Rows[table.Rows.Count - 1]["ScientificSynonyms"] = item.ScientificSynonyms;
                    }

                    if (options.OutputParentTaxa)
                    {
                        table.Rows[table.Rows.Count - 1]["ParentTaxa"] = item.ParentTaxa;
                    }

                    if (options.OutputGUID)
                    {
                        table.Rows[table.Rows.Count - 1]["GUID"] = item.GUID;
                    }

                    if (options.OutputRecommendedGUID)
                    {
                        table.Rows[table.Rows.Count - 1]["RecommendedGUID"] = item.RecommendedGUID;
                    }

                    if (options.OutputSwedishOccurrence)
                    {
                        table.Rows[table.Rows.Count - 1]["SwedishOccurrence"] = item.SwedishOccurrence;
                    }
                }

                if (item.Status == MatchStatus.NeedsManualSelection)
                {
                    if (options.OutputScientificName)
                    {
                        var alternativeTaxa = new StringBuilder();

                        foreach (var taxon in item.AlternativeTaxa)
                        {
                            if (taxon.Value != "0")
                            {
                                alternativeTaxa.Append("; ");
                            }

                            alternativeTaxa.Append(taxon.Text);
                        }

                        table.Rows[table.Rows.Count - 1]["ScientificName"] = alternativeTaxa.ToString();
                    }
                }
            }

            return table;
        }

        public string GetScientificSynonyms(ITaxon taxon)
        {
            if (taxon == null || taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), true) == null)
            {
                return "";
            }

            var names = new StringBuilder();

            foreach (var taxonName in taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), true))
            {
                if (names.Length > 0)
                {
                    names.Append("; ");
                }

                names.Append(taxonName.Name);

                if (!string.IsNullOrEmpty(taxonName.Author))
                {
                    names.Append(" ");
                    names.Append(taxonName.Author);
                }                
            }
            //IList<ITaxonName> list = taxon.GetTaxonNamesBySearchCriteria(null, null, null, false, true, false);      
            return names.ToString();
        }
        
        public string GetParenTaxa(ITaxon taxon)
        {
            if (taxon == null)
            {
                return "";
            }

            var parents = new StringBuilder();            
            var parentRelations = taxon.GetAllParentTaxonRelations(_user, null, false, false);
            foreach (var taxonRelation in parentRelations)
            {
                var relatedTaxon = taxonRelation.ParentTaxon;
                if (parents.Length > 0)
                {
                    parents.Append("; ");
                }

                parents.Append(relatedTaxon.Category.Name);
                parents.Append(": ");
                parents.Append(relatedTaxon.GetLabel());
            }      

            return parents.ToString();
        }
    }
}
