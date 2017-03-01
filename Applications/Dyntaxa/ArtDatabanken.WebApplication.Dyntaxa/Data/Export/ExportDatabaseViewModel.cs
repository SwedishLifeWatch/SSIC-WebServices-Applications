using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.IO;
using System.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using System.IO;
using System.ServiceModel.Channels;
using System.Web.UI.WebControls;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{

    public struct NewExportDataTableCollection
    {
        public DataTable TaxonDataTable { get; set; }
        public DataTable ParentChildDataTable { get; set; }
        public DataTable TaxonNameDataTable { get; set; }
        public DataTable SplitTaxonDataTable { get; set; }
        public DataTable LumpTaxonDataTable { get; set; }
        public DataTable TaxonReferencesDataTable { get; set; }
        public DataTable TaxonNameReferencesTable { get; set; }
        public DataTable TaxonCategoriesTable { get; set; }
        public DataTable TaxonNameCategoriesTable { get; set; }
        public DataTable TaxonNameStatusTable { get; set; }
        public DataTable TaxonNameUsageTable { get; set; }
        public DataTable ChangeStatusTable { get; set; }
        public DataTable AlertStatusTable { get; set; }
        public DataTable AllReferencesTable { get; set; }
    }

    public class ExportDatabaseViewModel
    {
        private ExportDatabaseDataTableWriter tableWriter;
        public IUserContext UserContext { get; set; }

        /// <summary>
        /// Type of the Column delimiter
        /// </summary>
        [LocalizedDisplayName("MatchOptionsInputRowDelimeterLabel",
            NameResourceType = typeof (Resources.DyntaxaResource))]
        public MatchTaxonRowDelimiter RowDelimiter { get; set; }

        /// <summary>
        /// String pasted into textarea
        /// </summary>
        [LocalizedDisplayName("ExportDatabaseClipboard", NameResourceType = typeof (Resources.DyntaxaResource))]
        public string ClipBoard { get; set; }

        public int TaxonId { get; set; }

        public List<int> InputTaxonIds { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public List<DataTable> DataTables
        {
            get { return _dataTables; }
        }

        private List<DataTable> _dataTables;

        public static List<DataTable> NewDataTables
        {
            get { return newDataTables; }
        }

        private static List<DataTable> newDataTables;

        public List<DataTable> DataTablesWithTrees
        {
            get { return _dataTablesWithTrees; }
        }

        private List<DataTable> _dataTablesWithTrees;

        public static ExportDatabaseViewModel Create(IUserContext user, ITaxon taxon)
        {
            var model = new ExportDatabaseViewModel();
            model.UserContext = user;
            if (taxon != null)
            {
                model.TaxonId = taxon.Id;
            }
            return model;
        }

        public List<int> GetTaxonIdsFromString()
        {
            string[] stringArray = null;
            switch (this.RowDelimiter)
            {
                case MatchTaxonRowDelimiter.ReturnLinefeed:
                    stringArray = this.ClipBoard.Split('\n');
                    break;

                case MatchTaxonRowDelimiter.Semicolon:
                    stringArray = this.ClipBoard.Split(';');
                    break;

                case MatchTaxonRowDelimiter.Tab:
                    stringArray = this.ClipBoard.Split('\t');
                    break;

                case MatchTaxonRowDelimiter.VerticalBar:
                    stringArray = this.ClipBoard.Split('|');
                    break;

                default:
                    stringArray = this.ClipBoard.Split('\n');
                    break;
            }
            var taxonIds = new List<int>();
            foreach (string str in stringArray)
            {
                int number;
                if (int.TryParse(str, out number))
                {
                    taxonIds.Add(number);
                }
            }
            return taxonIds;
        }

        public List<ITaxon> GetTaxa()
        {
            List<int> taxonIds = GetTaxonIdsFromString();
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, taxonIds);
            List<ITaxon> taxa = taxonList.Cast<ITaxon>().ToList();
            return taxa;
        }

        private static List<ITaxonRelationsTreeNode> GetTreeNodes(TaxonRelationsTree tree, List<int> taxonIds)
        {
            List<ITaxonRelationsTreeNode> treeNodes = new List<ITaxonRelationsTreeNode>();
            foreach (int taxonId in taxonIds)
            {
                ITaxonRelationsTreeNode node = tree.GetTreeNode(taxonId);
                if (node != null)
                {
                    treeNodes.Add(node);
                } 
            }

            return treeNodes;
        }

        private static HashSet<ITaxonRelationsTreeEdge> GetTreeEdges(TaxonRelationsTree tree, List<int> taxonIds)
        {
            List<ITaxonRelationsTreeNode> treeNodes = GetTreeNodes(tree, taxonIds);
            return GetTreeEdges(tree, treeNodes);
        }

        private static HashSet<ITaxonRelationsTreeEdge> GetTreeEdges(TaxonRelationsTree tree, List<ITaxonRelationsTreeNode> treeNodes)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();            
            foreach (var edge in treeNodes.AsTopDownBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
            {
                hashEdges.Add(edge);
            }
            foreach (var edge in treeNodes.AsBreadthFirstChildEdgeIterator(TaxonRelationsTreeChildrenIterationMode.BothValidMainAndSecondaryChildren))            
            {
                hashEdges.Add(edge);

                if (edge.Child.ValidSecondaryParents != null)
                {
                    foreach (var parentEdge in edge.Child.ValidSecondaryParents.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
                    {
                        if (!hashEdges.Contains(parentEdge))
                        {
                            hashEdges.Add(parentEdge);
                        }
                    }
                }
            }

            return hashEdges;
        }

        private static HashSet<LumpSplitExportModel> CreateLumpExportList(IUserContext userContext, List<ITaxon> taxa)
        {
            var lumpExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in taxa)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(userContext, lumpSplitEvent);
                        if (!lumpExportList.Contains(lumpExportModel))
                        {
                            lumpExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return lumpExportList;
        }

        private static HashSet<LumpSplitExportModel> CreateSplitExportList(IUserContext userContext, List<ITaxon> taxa)
        {
            var splitExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in taxa)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(userContext, lumpSplitEvent);
                        if (!splitExportList.Contains(lumpExportModel))
                        {
                            splitExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return splitExportList;
        }

        private static List<ITaxon> GetNotLoadedTaxoFromLumpSplitEvent(
            HashSet<LumpSplitExportModel> lumps,
            HashSet<LumpSplitExportModel> splits,
            HashSet<ITaxon> allTaxon)
        {
            var taxa = new List<ITaxon>();
            foreach (LumpSplitExportModel splitExportModel in splits)
            {
                bool found = allTaxon.Any(taxon => taxon.Id == splitExportModel.TaxonAfterId);                
                if (found == false)
                {
                    taxa.Add(splitExportModel.TaxonAfter);
                }
            }
            foreach (LumpSplitExportModel splitExportModel in splits)
            {
                bool found = allTaxon.Any(taxon => splitExportModel.TaxonBeforeId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(splitExportModel.TaxonBefore);
                }
            }

            foreach (LumpSplitExportModel lumpExportModel in lumps)
            {
                bool found = allTaxon.Any(taxon => lumpExportModel.TaxonAfterId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(lumpExportModel.TaxonAfter);
                }
            }
            foreach (LumpSplitExportModel lumpExportModel in lumps)
            {
                bool found = allTaxon.Any(taxon => lumpExportModel.TaxonBeforeId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(lumpExportModel.TaxonBefore);
                }
            }

            return taxa;
        }

        public static NewExportDataTableCollection CreateDataTablesWithDyntaxaTree(
            IUserContext userContext,
            List<ITaxon> taxa)
        {
            TaxonRelationsTree tree = TaxonRelationsTreeCacheManager.CachedTaxonRelationTree;
            
            if (TaxonRelationsTreeCacheManager.CachedTaxonRelationTree == null)
            {
                ScheduledTasksManager.ExecuteTaskNow(ScheduledTaskType.RefreshDyntaxaTaxonTree);
                tree = TaxonRelationsTreeCacheManager.CachedTaxonRelationTree;
            }

            List<int> taxonIds = new List<int>();
            foreach (var taxon in taxa)
            {
                taxonIds.Add(taxon.Id);
            }
            
            var edges = GetTreeEdges(tree, taxonIds);
            HashSet<ITaxon> allTaxon = new HashSet<ITaxon>();
            foreach (var edge in edges)
            {
                allTaxon.Add(edge.Parent.Taxon);
                allTaxon.Add(edge.Child.Taxon);                
            }

            // Lump & Splits
            HashSet<LumpSplitExportModel> lumps = CreateLumpExportList(userContext, allTaxon.ToList());
            HashSet<LumpSplitExportModel> splits = CreateSplitExportList(userContext, allTaxon.ToList());            
            // add taxa that is referenced in lump split events and not yet exists in allTaxon
            List<ITaxon> lumpSplitExtraTaxa = GetNotLoadedTaxoFromLumpSplitEvent(lumps, splits, allTaxon);            
            List<int> lumpSplitTaxonIds = lumpSplitExtraTaxa.Select(taxon => taxon.Id).ToList();            
            List<ITaxonRelationsTreeNode> lumpSplitTreeNodes = GetTreeNodes(tree, lumpSplitTaxonIds);
            foreach (ITaxonRelationsTreeNode node in lumpSplitTreeNodes)
            {
                allTaxon.Add(node.Taxon);
            }            
            var lumpSplitEdges = GetTreeEdges(tree, lumpSplitTaxonIds);
            edges.UnionWith(lumpSplitEdges);

            foreach (var edge in edges)
            {
                Debug.WriteLine(edge);
            }

            var taxaList = allTaxon.ToList();
            NewExportDataTableCollection model = new NewExportDataTableCollection();

            model.TaxonDataTable = ExportDatabaseDataTableWriter.GetTaxonTable(userContext, taxaList);
            model.ParentChildDataTable = ExportDatabaseDataTableWriter.GetParentChildRelationsTable(edges.ToList());
                       
            //var searchCriteria = new TaxonNameSearchCriteria();
            //searchCriteria.TaxonIds = taxaList.Select(taxon => taxon.Id).ToList();
            //TaxonNameList taxonNameListrelation = CoreData.TaxonManager.GetTaxonNames(userContext, searchCriteria);
            //HashSet<ITaxonName> taxonNamesHash = new HashSet<ITaxonName>();
            //Dictionary<int, List<ITaxonName>> taxonNamesDictionary = new Dictionary<int, List<ITaxonName>>();
            //foreach (ITaxonName taxonName in taxonNameListrelation)
            //{
                
            //    if (!taxonNamesDictionary.ContainsKey(taxonName.Taxon.Id))
            //    {
            //        taxonNamesDictionary.Add(taxonName.Taxon.Id, new List<ITaxonName>());
            //    }
            //    taxonNamesDictionary[taxonName.Taxon.Id].Add(taxonName);
            //}

            model.TaxonNameDataTable = ExportDatabaseDataTableWriter.GetTaxonNameTable(userContext, taxaList);
            model.SplitTaxonDataTable = ExportDatabaseDataTableWriter.GetTaxonSplitRelationTable(userContext, splits);
            model.LumpTaxonDataTable = ExportDatabaseDataTableWriter.GetTaxonLumpRelationTable(userContext, lumps);
            model.TaxonReferencesDataTable = ExportDatabaseDataTableWriter.GetTaxonReferencesTable(userContext, taxaList);
            model.TaxonNameReferencesTable = ExportDatabaseDataTableWriter.GetTaxonNameReferencesTable(userContext, taxaList);
            model.TaxonCategoriesTable = ExportDatabaseDataTableWriter.GetTaxonCategoriesTable(userContext, taxaList);
            model.TaxonNameCategoriesTable = ExportDatabaseDataTableWriter.GetTaxonNameCategoriesTable(userContext, taxaList);
            model.TaxonNameUsageTable = ExportDatabaseDataTableWriter.GetTaxonNameUsageTable(userContext, taxaList);
            model.TaxonNameStatusTable = ExportDatabaseDataTableWriter.GetTaxonNameStatusTable(userContext, taxaList);
            model.ChangeStatusTable = ExportDatabaseDataTableWriter.GetChangeStatusTable(userContext, taxaList);
            model.AlertStatusTable = ExportDatabaseDataTableWriter.GetAlertStatusTable(userContext, taxaList);
            model.AllReferencesTable = ExportDatabaseDataTableWriter.GetAllReferencesTable(userContext, taxaList);

            newDataTables = new List<DataTable>();
             
            newDataTables.Add(model.TaxonDataTable);
            newDataTables.Add(model.ParentChildDataTable);
            newDataTables.Add(model.TaxonNameDataTable);
            newDataTables.Add(model.SplitTaxonDataTable);
            newDataTables.Add(model.LumpTaxonDataTable);
            newDataTables.Add(model.TaxonReferencesDataTable);
            newDataTables.Add(model.TaxonNameReferencesTable);
            newDataTables.Add(model.TaxonCategoriesTable);
            newDataTables.Add(model.TaxonNameCategoriesTable);
            newDataTables.Add(model.TaxonNameUsageTable);
            newDataTables.Add(model.TaxonNameStatusTable);
            newDataTables.Add(model.ChangeStatusTable);
            newDataTables.Add(model.AlertStatusTable);
            newDataTables.Add(model.AllReferencesTable);

            return model;

        }
        public MemoryStream CreateNewExcelFile(ExcelFileFormat fileFormat)
        {           
            var excelFile = new ExportDatabaseExcelFile();
            return excelFile.CreateExcelFile(newDataTables, fileFormat, true);
        }

        //  public static NewExportDataTableCollection CreateDataTablesWithDyntaxaTree(
        //      IUserContext userContext,
        //      List<ITaxon> taxa)
        //  {
        //      TaxonRelationsCacheManager.InitTaxonRelationsCache(userContext);
        //      var tree = DyntaxaTreeManager.CreateDyntaxaTree(
        //          userContext,
        //          TaxonRelationsCacheManager.TaxonRelationList);

        //      List<ITaxon> taxaList = new List<ITaxon>();
        //      Dictionary<ITaxon, List<ITaxon>> parentChildDictionary = new Dictionary<ITaxon, List<ITaxon>>();

        //      // Start parents
        //      // Get parent relations
        //      ITaxonRelationSearchCriteria taxonRelationSearchCriteria = new TaxonRelationSearchCriteria();
        //      taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
        //      taxonRelationSearchCriteria.IsValid = true;
        //      taxonRelationSearchCriteria.IsMainRelation = null;
        //      taxonRelationSearchCriteria.Taxa = new TaxonList();
        //      foreach (var taxon in taxa)
        //      {
        //          taxonRelationSearchCriteria.Taxa.Add(taxon);
        //      }
        //      var taxonRelationList = CoreData.TaxonManager.GetTaxonRelations(userContext, taxonRelationSearchCriteria);

        //      foreach (ITaxonRelation taxonRelation in taxonRelationList)
        //      {
        //          if (!taxaList.Contains(taxonRelation.ParentTaxon))
        //          {
        //              taxaList.Add(taxonRelation.ParentTaxon);
        //          }

        //          if (!parentChildDictionary.ContainsKey(taxonRelation.ParentTaxon))
        //          {
        //              parentChildDictionary.Add(taxonRelation.ParentTaxon, new List<ITaxon>());
        //          }

        //              parentChildDictionary[taxonRelation.ParentTaxon].Add(taxonRelation.ChildTaxon);                

        //      }
        //      // End parents

        //      List<TaxonRelationsTreeNode> baseTreeNodes = new List<TaxonRelationsTreeNode>();
        //      foreach (ITaxon taxon in taxa)
        //      {
        //          TaxonRelationsTreeNode treeNode = tree.TreeNodeDictionary[taxon.Id];
        //          baseTreeNodes.Add(treeNode);
        //      }

        //      foreach (TaxonRelationsTreeNode treeNode in baseTreeNodes.AsDepthFirstIterator())
        //      {
        //          if (treeNode.Taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.InvalidDueToDelete)
        //          {
        //              continue;
        //          }
        //          if (!taxaList.Contains(treeNode.Taxon))
        //          {
        //              taxaList.Add(treeNode.Taxon);

        //              if (treeNode.ValidMainChildren != null)
        //              {

        //                if (!parentChildDictionary.ContainsKey(treeNode.Taxon))
        //                  {
        //                      parentChildDictionary.Add(treeNode.Taxon, new List<ITaxon>());
        //                  }

        //                  foreach (var child in treeNode.ValidMainChildren)
        //                  {
        //                      parentChildDictionary[treeNode.Taxon].Add(child.Taxon);
        //                  }
        //              }

        //              if (treeNode.ValidSecondaryChildren != null)
        //              {
        //                  if (!parentChildDictionary.ContainsKey(treeNode.Taxon))
        //                  {
        //                      parentChildDictionary.Add(treeNode.Taxon, new List<ITaxon>());
        //                  }

        //                  foreach (var child in treeNode.ValidSecondaryChildren)
        //                  {
        //                      parentChildDictionary[treeNode.Taxon].Add(child.Taxon);
        //                  }
        //              }

        //              if (treeNode.ValidSecondaryParents != null)
        //              {
        //                  foreach (var node in treeNode.AllValidSecondaryParentsUpward)
        //                  {
        //                      if (!taxaList.Contains(node.Taxon))
        //                      {
        //                          taxaList.Add(node.Taxon);
        //                      }
        //                              if (!parentChildDictionary.ContainsKey(node.Taxon))
        //                             {
        //                                  parentChildDictionary.Add(node.Taxon, new List<ITaxon>());
        //                             }
        //                      //        //        //foreach (var child in node.ValidSecondaryChildren)
        //                      //        //        //{
        //                      //        //        //    parentChildDictionary[node.Taxon].Add(child.Taxon);
        //                      //        //        //}
        //                              }
        //                        }
        //                  }
        //}


        //    NewExportDataTableCollection model = new NewExportDataTableCollection();

        //    model.TaxonDataTable = ExportDatabaseDataTableWriter.GetTaxonTable(userContext, taxaList);
        //    model.ParentChildDataTable = ExportDatabaseDataTableWriter.GetParentChildRelationsTable(parentChildDictionary);
        //    return model;
        //}

        public static NewExportDataTableCollection CreateDataTablesWithTreesAlternative1(IUserContext userContext, List<ITaxon> taxa)
        {
            // Hämta enbart det vi behöver just nu. 
            // Går snabbare än alternativ 2 om trädet inte redan är cachat.

            // Get children tree

            var searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.TaxonIds = taxa.Select(taxon => taxon.Id).ToList();
            searchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            //searchCriteria.IsValidRequired = true; 
            TaxonTreeNodeList taxonTreeNodeList = CoreData.TaxonManager.GetTaxonTrees(userContext, searchCriteria);
            foreach (var taxonTreeNode in taxonTreeNodeList.AsDepthFirstIterator())
            {

            }

            // Get parent relations
            ITaxonRelationSearchCriteria taxonRelationSearchCriteria = new TaxonRelationSearchCriteria();
            taxonRelationSearchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelationSearchCriteria.IsValid = true;
            taxonRelationSearchCriteria.IsMainRelation = null;
            taxonRelationSearchCriteria.Taxa = new TaxonList();
            foreach (var taxon in taxa)
            {
                taxonRelationSearchCriteria.Taxa.Add(taxon);

            }

            var taxonRelationList = CoreData.TaxonManager.GetTaxonRelations(userContext, taxonRelationSearchCriteria);

            //get parent child relation

            List<ITaxon> taxaList = new List<ITaxon>();
            Dictionary<ITaxon, List<ITaxon>> parentChildDictionary = new Dictionary<ITaxon, List<ITaxon>>();
            foreach (ITaxonRelation taxonRelation in taxonRelationList)
            {
                taxaList.Add(taxonRelation.ParentTaxon);
                parentChildDictionary.Add(taxonRelation.ParentTaxon, new List<ITaxon>());
                parentChildDictionary[taxonRelation.ParentTaxon].Add(taxonRelation.ChildTaxon);
            }
            foreach (var taxonTreeNode in taxonTreeNodeList.AsDepthFirstIterator())
            {
                if (taxonTreeNode.Taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.InvalidDueToDelete)
                {
                    continue;
                }
                if (!taxaList.Contains(taxonTreeNode.Taxon))
                {
                    taxaList.Add(taxonTreeNode.Taxon);

                    if (taxonTreeNode.Taxon.IsValid)
                    {
                        if (taxonTreeNode.Children != null)
                        {
                            if (!parentChildDictionary.ContainsKey(taxonTreeNode.Taxon))
                            {
                                parentChildDictionary.Add(taxonTreeNode.Taxon, new List<ITaxon>());
                            }

                            foreach (var child in taxonTreeNode.Children)
                            {
                                parentChildDictionary[taxonTreeNode.Taxon].Add(child.Taxon);
                            }

                        }
                    }
                }


            }

            NewExportDataTableCollection model = new NewExportDataTableCollection();
            
            model.TaxonDataTable = ExportDatabaseDataTableWriter.GetTaxonTable(userContext, taxaList);
            model.ParentChildDataTable = ExportDatabaseDataTableWriter.GetParentChildRelationsTable(parentChildDictionary);            

            return model;
        }

        public void CreateDataTablesWithTreesAlternative2()
        {
            // Hämta hela trädet. Borde cachas någonstans senare. 
            // Går snabbare än alternativ 1 om trädet är cachat, annars långsammare.
            // How to cache it? That's the question.
            // Hämta hela trädet.
            var searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(0);
            searchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            searchCriteria.IsValidRequired = true;
            TaxonTreeNodeList taxonTreeNodeList = CoreData.TaxonManager.GetTaxonTrees(UserContext, searchCriteria);

            // Create lookup table for fast access to tree nodes.
            Dictionary<int, ITaxonTreeNode> treeNodeLookupByTaxonId = new Dictionary<int, ITaxonTreeNode>();                        
            foreach (ITaxonTreeNode taxonTreeNode in taxonTreeNodeList.AsDepthFirstIterator())
            {               
                if (!treeNodeLookupByTaxonId.ContainsKey(taxonTreeNode.Taxon.Id))
                {
                    treeNodeLookupByTaxonId.Add(taxonTreeNode.Taxon.Id, taxonTreeNode);
                }                
            }

            List<ITaxon> taxa = GetTaxa();
            foreach (var taxon in taxa)
            {
                var treeNode = treeNodeLookupByTaxonId[taxon.Id];
            }

        }

        public void CreateDataTables()
        {

            //CreateDataTablesWithTreesAlternative1();
            //CreateDataTablesWithTreesAlternative2();            
            List<ITaxon> taxa = GetTaxa();
            tableWriter = new ExportDatabaseDataTableWriter(UserContext, taxa);                      
            _dataTables = new List<DataTable>();

            _dataTables.Add(tableWriter.GetTaxonTable());
            _dataTables.Add(tableWriter.GetTaxonNameTable());
            _dataTables.Add(tableWriter.GetParentChildRelationsTable());
            _dataTables.Add(tableWriter.GetTaxonSplitRelationTable());
            _dataTables.Add(tableWriter.GetTaxonLumpRelationTable());
            _dataTables.Add(tableWriter.GetTaxonReferencesTable());
            _dataTables.Add(tableWriter.GetTaxonNameReferencesTable());
            _dataTables.Add(tableWriter.GetTaxonCategoriesTable());
            _dataTables.Add(tableWriter.GetTaxonNameCategoriesTable());
            _dataTables.Add(tableWriter.GetTaxonNameStatusTable());
            _dataTables.Add(tableWriter.GetTaxonNameUsageTable());
            _dataTables.Add(tableWriter.GetChangeStatusTable());
            _dataTables.Add(tableWriter.GetAlertStatusTable());
            _dataTables.Add(tableWriter.GetAllReferencesTable());            
        }

        public MemoryStream CreateExcelFile(ExcelFileFormat fileFormat)
        {            
            var excelFile = new ExportDatabaseExcelFile();            
            return excelFile.CreateExcelFile(_dataTables, fileFormat, true);
        }

        private readonly ModelLabels _labels = new ModelLabels();        

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.ExportDatabaseTitle; } }
            public string Input { get { return Resources.DyntaxaResource.MatchOptionsInput; } }
            public string GetExcelFile { get { return Resources.DyntaxaResource.ExportStraightGetExcelFile; } }
        }
    }

    public class ExportDatabaseDataTableWriter
    { 
        private readonly IUserContext _user;        
        private readonly Dictionary<ITaxon, List<ITaxon>> _parentChildDictionary;
        private readonly HashSet<LumpSplitExportModel> _lumpExportList;
        private readonly HashSet<LumpSplitExportModel> _splitExportList;

        public ExportDatabaseDataTableWriter(IUserContext user, IEnumerable<ITaxon> taxa)
        {
            _user = user;            
            _parentChildDictionary = CreateParentChildDictionary(taxa);
            _lumpExportList = CreateLumpExportList(_parentChildDictionary);
            _splitExportList = CreateSplitExportList(_parentChildDictionary);


            // add taxa that is referenced in lump split events and not yet exists in _parentChildDictionary
            List<ITaxon> lumpSplitExtraTaxa = GetNotLoadedTaxoFromLumpSplitEvent();
            var dicLumpSplitExtraTaxa = CreateParentChildDictionary(lumpSplitExtraTaxa);
            foreach (KeyValuePair<ITaxon, List<ITaxon>> pair in dicLumpSplitExtraTaxa)
            {
                if (!_parentChildDictionary.ContainsKey(pair.Key))
                {
                    _parentChildDictionary.Add(pair.Key, pair.Value);
                }
            }
        }

        private List<ITaxon> GetNotLoadedTaxoFromLumpSplitEvent()
        {
            var taxa = new List<ITaxon>();
            foreach (LumpSplitExportModel splitExportModel in _splitExportList)
            {
                bool found = _parentChildDictionary.Keys.Any(taxon => splitExportModel.TaxonAfterId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(splitExportModel.TaxonAfter);
                }
            }
            foreach (LumpSplitExportModel splitExportModel in _splitExportList)
            {
                bool found = _parentChildDictionary.Keys.Any(taxon => splitExportModel.TaxonBeforeId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(splitExportModel.TaxonBefore);
                }
            }

            foreach (LumpSplitExportModel lumpExportModel in _lumpExportList)
            {
                bool found = _parentChildDictionary.Keys.Any(taxon => lumpExportModel.TaxonAfterId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(lumpExportModel.TaxonAfter);
                }
            }
            foreach (LumpSplitExportModel lumpExportModel in _lumpExportList)
            {
                bool found = _parentChildDictionary.Keys.Any(taxon => lumpExportModel.TaxonBeforeId == taxon.Id);
                if (found == false)
                {
                    taxa.Add(lumpExportModel.TaxonBefore);
                }
            }

            return taxa;
        }

       

        public static DataTable GetTaxonTable(IUserContext userContext, List<ITaxon> taxa)
        {
            var table = new DataTable();
            table.TableName = "Taxon";

            AddColumnStatic(table, "TaxonId", typeof(Int32));
            AddColumnStatic(table, "ScientificName");
            AddColumnStatic(table, "Author");
            AddColumnStatic(table, "CommonName");
            AddColumnStatic(table, "TaxonCategory");
            AddColumnStatic(table, "TaxonCategoryId", typeof(Int32));
            AddColumnStatic(table, "GUID");
            AddColumnStatic(table, "RecommendedGUID");
            AddColumnStatic(table, "ConceptDefinitionPartString");
            AddColumnStatic(table, "ConceptDefinition");
            AddColumnStatic(table, "CreatedBy");
            AddColumnStatic(table, "CreatedDate");
            AddColumnStatic(table, "ModifiedDate");
            AddColumnStatic(table, "ValidFromDate");
            AddColumnStatic(table, "ValidToDate");
            AddColumnStatic(table, "ChangeStatus");
            AddColumnStatic(table, "PersonName");
            AddColumnStatic(table, "AlertStatus");
            AddColumnStatic(table, "IsPublished");
            AddColumnStatic(table, "IsValid");

            foreach (ITaxon taxon in taxa)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                table.Rows[table.Rows.Count - 1]["ScientificName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
                table.Rows[table.Rows.Count - 1]["Author"] = taxon.Author.IsNotEmpty() ? taxon.Author : "";
                table.Rows[table.Rows.Count - 1]["CommonName"] = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
                table.Rows[table.Rows.Count - 1]["TaxonCategory"] = taxon.Category != null ? taxon.Category.Name : "";
                table.Rows[table.Rows.Count - 1]["TaxonCategoryId"] = taxon.Category != null ? taxon.Category.Id.ToString() : "";
                table.Rows[table.Rows.Count - 1]["GUID"] = taxon.Guid ?? "";
                table.Rows[table.Rows.Count - 1]["RecommendedGUID"] = taxon.GetRecommendedGuid(userContext) ?? "";
                table.Rows[table.Rows.Count - 1]["ConceptDefinitionPartString"] = taxon.PartOfConceptDefinition;
                if (taxon.AlertStatus.Id != (Int32)TaxonAlertStatusId.Green)
                {
                    table.Rows[table.Rows.Count - 1]["ConceptDefinition"] = CoreData.TaxonManager.GetTaxonConceptDefinition(userContext, taxon);
                }

                table.Rows[table.Rows.Count - 1]["CreatedBy"] = taxon.CreatedBy;
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = taxon.CreatedDate;
                table.Rows[table.Rows.Count - 1]["ModifiedDate"] = taxon.ModifiedDate;
                table.Rows[table.Rows.Count - 1]["ValidFromDate"] = taxon.ValidFromDate;

                table.Rows[table.Rows.Count - 1]["ValidToDate"] = taxon.ValidToDate;
                table.Rows[table.Rows.Count - 1]["ChangeStatus"] = taxon.ChangeStatus.Identifier;
                table.Rows[table.Rows.Count - 1]["PersonName"] = taxon.ModifiedByPerson;
                table.Rows[table.Rows.Count - 1]["AlertStatus"] = taxon.AlertStatus.Identifier;

                // table.Rows[table.Rows.Count - 1]["NumberOfSpeciesInSweden"] = taxon.NumberOfSpeciesInSweden;
                table.Rows[table.Rows.Count - 1]["IsPublished"] = taxon.IsPublished;
                table.Rows[table.Rows.Count - 1]["IsValid"] = taxon.IsValid;
            }

            return table;
        }

        public DataTable GetTaxonTable()
        {
            var table = new DataTable();                        
            table.TableName = "Taxon";

            AddColumn(table, "TaxonId", typeof(Int32));
            AddColumn(table, "ScientificName");
            AddColumn(table, "Author");
            AddColumn(table, "CommonName");
            AddColumn(table, "TaxonCategory");
            AddColumn(table, "TaxonCategoryId", typeof(Int32));
            AddColumn(table, "GUID");
            AddColumn(table, "RecommendedGUID");
            AddColumn(table, "ConceptDefinitionPartString");
            AddColumn(table, "ConceptDefinition");
            AddColumn(table, "CreatedBy");
            AddColumn(table, "CreatedDate");
            AddColumn(table, "ModifiedDate");
            AddColumn(table, "ValidFromDate");
            AddColumn(table, "ValidToDate");
            AddColumn(table, "ChangeStatus");
            AddColumn(table, "PersonName");
            AddColumn(table, "AlertStatus");
            // AddColumn(table, "NumberOfSpeciesInSweden", typeof(Int32));
            AddColumn(table, "IsPublished");
            AddColumn(table, "IsValid");
            
            foreach (ITaxon taxon in _parentChildDictionary.Keys)
            {
                table.Rows.Add();                
                table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;                    
                table.Rows[table.Rows.Count - 1]["ScientificName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
                table.Rows[table.Rows.Count - 1]["Author"] = taxon.Author.IsNotEmpty() ? taxon.Author : "";
                table.Rows[table.Rows.Count - 1]["CommonName"] = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
                table.Rows[table.Rows.Count - 1]["TaxonCategory"] = taxon.Category != null ? taxon.Category.Name : "";
                table.Rows[table.Rows.Count - 1]["TaxonCategoryId"] = taxon.Category != null ? taxon.Category.Id.ToString() : "";
                table.Rows[table.Rows.Count - 1]["GUID"] = taxon.Guid ?? "";
                table.Rows[table.Rows.Count - 1]["RecommendedGUID"] = taxon.GetRecommendedGuid(_user) ?? "";
                table.Rows[table.Rows.Count - 1]["ConceptDefinitionPartString"] = taxon.PartOfConceptDefinition;
                if (taxon.AlertStatus.Id != (Int32)TaxonAlertStatusId.Green)
                {
                    table.Rows[table.Rows.Count - 1]["ConceptDefinition"] = CoreData.TaxonManager.GetTaxonConceptDefinition(_user, taxon);
                }

                table.Rows[table.Rows.Count - 1]["CreatedBy"] = taxon.CreatedBy;
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = taxon.CreatedDate;
                table.Rows[table.Rows.Count - 1]["ModifiedDate"] = taxon.ModifiedDate;
                table.Rows[table.Rows.Count - 1]["ValidFromDate"] = taxon.ValidFromDate;

                table.Rows[table.Rows.Count - 1]["ValidToDate"] = taxon.ValidToDate;
                table.Rows[table.Rows.Count - 1]["ChangeStatus"] = taxon.ChangeStatus.Identifier;
                table.Rows[table.Rows.Count - 1]["PersonName"] = taxon.ModifiedByPerson;
                table.Rows[table.Rows.Count - 1]["AlertStatus"] = taxon.AlertStatus.Identifier;

                // table.Rows[table.Rows.Count - 1]["NumberOfSpeciesInSweden"] = taxon.NumberOfSpeciesInSweden;
                table.Rows[table.Rows.Count - 1]["IsPublished"] = taxon.IsPublished;
                table.Rows[table.Rows.Count - 1]["IsValid"] = taxon.IsValid;
            }

            return table;
        }

        public static DataTable GetTaxonNameTable(IUserContext userContext, List<ITaxon> taxa)
        {
            var table = new DataTable();
            table.TableName = "TaxonName";

            AddColumnStatic(table, "TaxonId", typeof(Int32));
            AddColumnStatic(table, "TaxonName");
            AddColumnStatic(table, "TaxonNameId");
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "Author");
            AddColumnStatic(table, "NameCategory");
            AddColumnStatic(table, "NameCategoryId");
            AddColumnStatic(table, "IsRecommended");
            AddColumnStatic(table, "GUID");
            AddColumnStatic(table, "Description");
            AddColumnStatic(table, "NameUsageId");
            AddColumnStatic(table, "NameUsage");
            AddColumnStatic(table, "NameStatusId");
            AddColumnStatic(table, "NameStatus");
            AddColumnStatic(table, "IsOkForObsSystems");
            AddColumnStatic(table, "IsOriginalName");
            AddColumnStatic(table, "CreatedBy");
            AddColumnStatic(table, "CreatedDate");
            AddColumnStatic(table, "ModifiedBy");
            AddColumnStatic(table, "ModifiedDate");
            AddColumnStatic(table, "PersonName");
            AddColumnStatic(table, "ValidFromDate");
            AddColumnStatic(table, "ValidToDate");

            foreach (ITaxon taxon in taxa)
            {
                //List<ITaxonName> taxonNames;
                //if (taxonNamesDictionary.TryGetValue(taxon.Id, out taxonNames))
                //{
                    foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
                    {

                        table.Rows.Add();
                        table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                        table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty()
                            ? taxon.ScientificName
                            : "";
                        table.Rows[table.Rows.Count - 1]["TaxonNameId"] = taxonName.Id;
                        table.Rows[table.Rows.Count - 1]["Name"] = taxonName.Name;
                        table.Rows[table.Rows.Count - 1]["Author"] = taxonName.Author;
                        table.Rows[table.Rows.Count - 1]["NameCategory"] = taxonName.Category.Name;
                        table.Rows[table.Rows.Count - 1]["NameCategoryId"] = taxonName.Category.Id;
                        table.Rows[table.Rows.Count - 1]["IsRecommended"] = taxonName.IsRecommended;
                        table.Rows[table.Rows.Count - 1]["GUID"] = taxonName.Guid;
                        table.Rows[table.Rows.Count - 1]["Description"] = taxonName.Description;
                        table.Rows[table.Rows.Count - 1]["NameUsageId"] = taxonName.NameUsage.Id;
                        table.Rows[table.Rows.Count - 1]["NameUsage"] = taxonName.NameUsage.Name;
                        table.Rows[table.Rows.Count - 1]["NameStatusId"] = taxonName.Status.Id;
                        table.Rows[table.Rows.Count - 1]["NameStatus"] = taxonName.Status.Name;
                        table.Rows[table.Rows.Count - 1]["IsOkForObsSystems"] = taxonName.IsOkForSpeciesObservation;
                        table.Rows[table.Rows.Count - 1]["IsOriginalName"] = taxonName.IsOriginalName;
                        table.Rows[table.Rows.Count - 1]["CreatedBy"] = taxonName.CreatedBy;
                        table.Rows[table.Rows.Count - 1]["CreatedDate"] = taxonName.CreatedDate;
                        table.Rows[table.Rows.Count - 1]["ModifiedBy"] = taxonName.ModifiedBy;
                        table.Rows[table.Rows.Count - 1]["ModifiedDate"] = taxonName.ModifiedDate;
                        table.Rows[table.Rows.Count - 1]["PersonName"] = taxonName.ModifiedByPerson;
                        table.Rows[table.Rows.Count - 1]["ValidFromDate"] = taxonName.ValidFromDate;
                        table.Rows[table.Rows.Count - 1]["ValidToDate"] = taxonName.ValidToDate;
                    }
                }
            


            return table;
        }
        public DataTable GetTaxonNameTable()
        {
            var table = new DataTable();            
            table.TableName = "TaxonName";

            AddColumn(table, "TaxonId", typeof(Int32));
            AddColumn(table, "TaxonName");
            AddColumn(table, "TaxonNameId");
            AddColumn(table, "Name");
            AddColumn(table, "Author");
            AddColumn(table, "NameCategory");
            AddColumn(table, "NameCategoryId");
            AddColumn(table, "IsRecommended");
            AddColumn(table, "GUID");
            AddColumn(table, "Description");
            AddColumn(table, "NameUsageId");
            AddColumn(table, "NameUsage");
            AddColumn(table, "NameStatusId");
            AddColumn(table, "NameStatus");
            AddColumn(table, "IsOkForObsSystems");
            AddColumn(table, "IsOriginalName");
            AddColumn(table, "CreatedBy");
            AddColumn(table, "CreatedDate");
            AddColumn(table, "ModifiedBy");
            AddColumn(table, "ModifiedDate");
            AddColumn(table, "PersonName");
            AddColumn(table, "ValidFromDate");
            AddColumn(table, "ValidToDate");

            foreach (ITaxon taxon in _parentChildDictionary.Keys)
            {
                foreach (ITaxonName taxonName in taxon.GetTaxonNames(_user))
                {
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                    table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["TaxonNameId"] = taxonName.Id;
                    table.Rows[table.Rows.Count - 1]["Name"] = taxonName.Name;
                    table.Rows[table.Rows.Count - 1]["Author"] = taxonName.Author;
                    table.Rows[table.Rows.Count - 1]["NameCategory"] = taxonName.Category.Name;
                    table.Rows[table.Rows.Count - 1]["NameCategoryId"] = taxonName.Category.Id;                    
                    table.Rows[table.Rows.Count - 1]["IsRecommended"] = taxonName.IsRecommended;
                    table.Rows[table.Rows.Count - 1]["GUID"] = taxonName.Guid;
                    table.Rows[table.Rows.Count - 1]["Description"] = taxonName.Description;
                    table.Rows[table.Rows.Count - 1]["NameUsageId"] = taxonName.NameUsage.Id;
                    table.Rows[table.Rows.Count - 1]["NameUsage"] = taxonName.NameUsage.Name;
                    table.Rows[table.Rows.Count - 1]["NameStatusId"] = taxonName.Status.Id;
                    table.Rows[table.Rows.Count - 1]["NameStatus"] = taxonName.Status.Name;
                    table.Rows[table.Rows.Count - 1]["IsOkForObsSystems"] = taxonName.IsOkForSpeciesObservation;
                    table.Rows[table.Rows.Count - 1]["IsOriginalName"] = taxonName.IsOriginalName;                    
                    table.Rows[table.Rows.Count - 1]["CreatedBy"] = taxonName.CreatedBy;
                    table.Rows[table.Rows.Count - 1]["CreatedDate"] = taxonName.CreatedDate;
                    table.Rows[table.Rows.Count - 1]["ModifiedBy"] = taxonName.ModifiedBy;
                    table.Rows[table.Rows.Count - 1]["ModifiedDate"] = taxonName.ModifiedDate;
                    table.Rows[table.Rows.Count - 1]["PersonName"] = taxonName.ModifiedByPerson;
                    table.Rows[table.Rows.Count - 1]["ValidFromDate"] = taxonName.ValidFromDate;
                    table.Rows[table.Rows.Count - 1]["ValidToDate"] = taxonName.ValidToDate;                    
                }                
            }

            return table;
        }

        public DataTable GetAllReferencesTable()
        {
            var table = new DataTable();
            table.TableName = "ReferenceList";

            AddColumn(table, "ReferenceId", typeof(Int32));
            AddColumn(table, "ReferenceName");
            AddColumn(table, "ReferenceText");
            AddColumn(table, "ReferenceYear", typeof(Int32));

            var references = ReferenceHelper.GetReferenceList(this._user);            
            foreach (var reference in references)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Title;
                table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year.GetValueOrDefault(-1);
            }

            return table;
        }

        public static DataTable GetAllReferencesTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "ReferenceList";

            AddColumnStatic(table, "ReferenceId", typeof(Int32));
            AddColumnStatic(table, "ReferenceName");
            AddColumnStatic(table, "ReferenceText");
            AddColumnStatic(table, "ReferenceYear", typeof(Int32));

            var references = ReferenceHelper.GetReferenceList(userContext);
            foreach (var reference in references)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Title;
                table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year.GetValueOrDefault(-1);
            }

            return table;
        }

        public DataTable GetTaxonReferencesTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonReferences";

            AddColumn(table, "TaxonId", typeof(Int32));
            AddColumn(table, "TaxonName");
            AddColumn(table, "ReferenceId", typeof(Int32));
            AddColumn(table, "ReferenceName");
            AddColumn(table, "ReferenceText");
            AddColumn(table, "ReferenceYear", typeof(Int32));
            AddColumn(table, "ReferenceUsageTypeId", typeof(Int32));
            AddColumn(table, "ReferenceUsage");

            foreach (ITaxon taxon in _parentChildDictionary.Keys)
            {
                var references = ReferenceHelper.GetReferences(taxon.Guid);
                foreach (var reference in references)
                {
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                    table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                    table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                    table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Text;
                    table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year;
                    table.Rows[table.Rows.Count - 1]["ReferenceUsageTypeId"] = reference.UsageTypeId;
                    table.Rows[table.Rows.Count - 1]["ReferenceUsage"] = reference.Usage;
                }
            }

            return table;    
        }

        public static DataTable GetTaxonReferencesTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonReferences";

            AddColumnStatic(table, "TaxonId", typeof(Int32));
            AddColumnStatic(table, "TaxonName");
            AddColumnStatic(table, "ReferenceId", typeof(Int32));
            AddColumnStatic(table, "ReferenceName");
            AddColumnStatic(table, "ReferenceText");
            AddColumnStatic(table, "ReferenceYear", typeof(Int32));
            AddColumnStatic(table, "ReferenceUsageTypeId", typeof(Int32));
            AddColumnStatic(table, "ReferenceUsage");

            foreach (ITaxon taxon in taxaList)
            {
                var references = ReferenceHelper.GetReferences(taxon.Guid);
                foreach (var reference in references)
                {
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                    table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                    table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                    table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Text;
                    table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year;
                    table.Rows[table.Rows.Count - 1]["ReferenceUsageTypeId"] = reference.UsageTypeId;
                    table.Rows[table.Rows.Count - 1]["ReferenceUsage"] = reference.Usage;
                }
            }

            return table;
        }

        public DataTable GetTaxonNameReferencesTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonNameReferences";

            AddColumn(table, "TaxonId", typeof(Int32));
            AddColumn(table, "TaxonName");
            AddColumn(table, "Name");
            AddColumn(table, "NameId");
            AddColumn(table, "ReferenceId", typeof(Int32));
            AddColumn(table, "ReferenceName");
            AddColumn(table, "ReferenceText");
            AddColumn(table, "ReferenceYear", typeof(Int32));
            AddColumn(table, "ReferenceUsageTypeId", typeof(Int32));
            AddColumn(table, "ReferenceUsage");

            foreach (ITaxon taxon in _parentChildDictionary.Keys)
            {
                foreach (ITaxonName taxonName in taxon.GetTaxonNames(_user))
                {
                    var references = ReferenceHelper.GetReferences(taxonName.Guid);

                    foreach (var reference in references)
                    {
                        table.Rows.Add();
                        table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                        table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";                        
                        table.Rows[table.Rows.Count - 1]["TaxonName"] = taxonName.Name;
                        table.Rows[table.Rows.Count - 1]["NameId"] = taxonName.Id;
                        table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                        table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                        table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Text;
                        table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year;
                        table.Rows[table.Rows.Count - 1]["ReferenceUsageTypeId"] = reference.UsageTypeId;
                        table.Rows[table.Rows.Count - 1]["ReferenceUsage"] = reference.Usage;
                    }
                }
            }
            return table;
        }

        public static DataTable GetTaxonNameReferencesTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonNameReferences";

            AddColumnStatic(table, "TaxonId", typeof (Int32));
            AddColumnStatic(table, "TaxonName");
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "NameId");
            AddColumnStatic(table, "ReferenceId", typeof (Int32));
            AddColumnStatic(table, "ReferenceName");
            AddColumnStatic(table, "ReferenceText");
            AddColumnStatic(table, "ReferenceYear", typeof (Int32));
            AddColumnStatic(table, "ReferenceUsageTypeId", typeof (Int32));
            AddColumnStatic(table, "ReferenceUsage");

            foreach (ITaxon taxon in taxaList)
            {
                //List<ITaxonName> taxonNames;
                //if (taxonNameDictionary.TryGetValue(taxon.Id, out taxonNames))
                //{
                    foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
                    {
                        var references = ReferenceHelper.GetReferences(taxonName.Guid);

                        foreach (var reference in references)
                        {
                            table.Rows.Add();
                            table.Rows[table.Rows.Count - 1]["TaxonId"] = taxon.Id;
                            table.Rows[table.Rows.Count - 1]["TaxonName"] = taxon.ScientificName.IsNotEmpty()
                                ? taxon.ScientificName
                                : "";
                            table.Rows[table.Rows.Count - 1]["TaxonName"] = taxonName.Name;
                            table.Rows[table.Rows.Count - 1]["NameId"] = taxonName.Id;
                            table.Rows[table.Rows.Count - 1]["ReferenceId"] = reference.Id;
                            table.Rows[table.Rows.Count - 1]["ReferenceName"] = reference.Name;
                            table.Rows[table.Rows.Count - 1]["ReferenceText"] = reference.Text;
                            table.Rows[table.Rows.Count - 1]["ReferenceYear"] = reference.Year;
                            table.Rows[table.Rows.Count - 1]["ReferenceUsageTypeId"] = reference.UsageTypeId;
                            table.Rows[table.Rows.Count - 1]["ReferenceUsage"] = reference.Usage;
                        }
                    }
                }
            
            return table;
            }
        

        //protected Dictionary<int, List<ITaxonName>> CreateTaxonNameDictionary(IList<ITaxonName> taxonNames)
        //{
        //    var dic = new Dictionary<int, List<ITaxonName>>();
        //    foreach (ITaxonName name in taxonNames)
        //    {
        //        if (!dic.ContainsKey(name.NameCategory.Id))
        //            dic.Add(name.NameCategory.Id, new List<ITaxonName>());
        //        dic[name.NameCategory.Id].Add(name);
        //    }
        //    return dic;
        //}

        public static DataTable GetParentChildRelationsTable(List<ITaxonRelationsTreeEdge> taxonRelations)
        {
            var table = new DataTable();
            table.TableName = "TaxonParentRelation";
            AddColumnStatic(table, "ParentTaxonId", typeof(Int32));
            AddColumnStatic(table, "ParentTaxonName");
            AddColumnStatic(table, "ParentTaxonCategoryId", typeof(Int32));
            AddColumnStatic(table, "ParentTaxonCategoryName");
            AddColumnStatic(table, "ChildTaxonId", typeof(Int32));
            AddColumnStatic(table, "ChildTaxonName");
            AddColumnStatic(table, "ChildTaxonCategoryId", typeof(Int32));
            AddColumnStatic(table, "ChildTaxonCategoryName");

            foreach (var relation in taxonRelations)
            {
                //if (pair.Value == null)
                //{
                //    continue; // leaf
                //}

                //var parentTaxon = pair.Key;
                //foreach (var childTaxon in pair.Value)
                //{
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["ChildTaxonId"] = relation.Child.Taxon.Id;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonName"] = relation.Child.Taxon.ScientificName.IsNotEmpty() ? relation.Child.Taxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryId"] = relation.Child.Taxon.Category != null ? relation.Child.Taxon.Category.Id : -1;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryName"] = relation.Child.Taxon.Category != null ? relation.Child.Taxon.Category.Name : "";

                    table.Rows[table.Rows.Count - 1]["ParentTaxonId"] = relation.Parent.Taxon.Id;
                    table.Rows[table.Rows.Count - 1]["ParentTaxonName"] = relation.Parent.Taxon.ScientificName.IsNotEmpty() ? relation.Parent.Taxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryId"] = relation.Parent.Taxon.Category != null ? relation.Parent.Taxon.Category.Id.ToString() : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryName"] = relation.Parent.Taxon.Category != null ? relation.Parent.Taxon.Category.Name : "";
                //}
            }
            return table;
        }

        public static DataTable GetParentChildRelationsTable(Dictionary<ITaxon, List<ITaxon>> _parentChildDictionary)
        {
            var table = new DataTable();
            table.TableName = "TaxonParentRelation";
            AddColumnStatic(table, "ParentTaxonId", typeof(Int32));
            AddColumnStatic(table, "ParentTaxonName");
            AddColumnStatic(table, "ParentTaxonCategoryId", typeof(Int32));
            AddColumnStatic(table, "ParentTaxonCategoryName");
            AddColumnStatic(table, "ChildTaxonId", typeof(Int32));
            AddColumnStatic(table, "ChildTaxonName");
            AddColumnStatic(table, "ChildTaxonCategoryId", typeof(Int32));
            AddColumnStatic(table, "ChildTaxonCategoryName");

            foreach (KeyValuePair<ITaxon, List<ITaxon>> pair in _parentChildDictionary)
            {
                if (pair.Value == null)
                {
                    continue; // leaf
                }

                var parentTaxon = pair.Key;
                foreach (var childTaxon in pair.Value)
                {
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["ChildTaxonId"] = childTaxon.Id;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonName"] = childTaxon.ScientificName.IsNotEmpty() ? childTaxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryId"] = childTaxon.Category != null ? childTaxon.Category.Id : -1;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryName"] = childTaxon.Category != null ? childTaxon.Category.Name : "";

                    table.Rows[table.Rows.Count - 1]["ParentTaxonId"] = parentTaxon.Id;
                    table.Rows[table.Rows.Count - 1]["ParentTaxonName"] = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryId"] = parentTaxon.Category != null ? parentTaxon.Category.Id.ToString() : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryName"] = parentTaxon.Category != null ? parentTaxon.Category.Name : "";
                }
            }
            return table;          
        }


        public DataTable GetParentChildRelationsTable()
        {
            var table = new DataTable();            
            table.TableName = "TaxonParentRelation";           
            AddColumn(table, "ParentTaxonId", typeof(Int32));
            AddColumn(table, "ParentTaxonName");
            AddColumn(table, "ParentTaxonCategoryId", typeof(Int32));
            AddColumn(table, "ParentTaxonCategoryName");            
            AddColumn(table, "ChildTaxonId", typeof(Int32));
            AddColumn(table, "ChildTaxonName");
            AddColumn(table, "ChildTaxonCategoryId", typeof(Int32));
            AddColumn(table, "ChildTaxonCategoryName");

            foreach (KeyValuePair<ITaxon, List<ITaxon>> pair in _parentChildDictionary)
            {
                if (pair.Value == null)
                {
                    continue; // leaf
                }

                var parentTaxon = pair.Key;
                foreach (var childTaxon in pair.Value)
                {
                    table.Rows.Add();
                    table.Rows[table.Rows.Count - 1]["ChildTaxonId"] = childTaxon.Id;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonName"] = childTaxon.ScientificName.IsNotEmpty() ? childTaxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryId"] = childTaxon.Category != null ? childTaxon.Category.Id : -1;
                    table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryName"] = childTaxon.Category != null ? childTaxon.Category.Name : "";

                    table.Rows[table.Rows.Count - 1]["ParentTaxonId"] = parentTaxon.Id;
                    table.Rows[table.Rows.Count - 1]["ParentTaxonName"] = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryId"] = parentTaxon.Category != null ? parentTaxon.Category.Id.ToString() : "";
                    table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryName"] = parentTaxon.Category != null ? parentTaxon.Category.Name : "";                                                
                }
            }
            return table;

            //var taxonRelations = new HashSet<ITaxonRelation>();
                        
            //foreach (ITaxon taxon in taxa)
            //{
            //    List<ITaxon> leafs = GetLeafs(taxon);
            //    foreach (ITaxon leaf in leafs)
            //    {
            //        Debug.WriteLine("Leaf Id: {0}, Name: {1}", leaf.Id, leaf.GetScientificAndCommonName());
            //        IList<ITaxonRelation> parents = leaf.GetAllParentTaxonRelations(null, false, false);
            //        foreach (ITaxonRelation taxonRelation in parents)
            //        {
            //            ITaxon c = taxonRelation.Taxon;
            //            ITaxon p = taxonRelation.RelatedTaxon;
            //            Debug.WriteLine("Relation ChildId: {0}, ChildName: {1}, ParentId: {2}, ParentName: {3}, ChildValid: {4}, ParentValid: {5}", c.Id, c.GetScientificAndCommonName(), p.Id, p.GetScientificAndCommonName(), c.IsValid, p.IsValid);
            //        }

            //        foreach (ITaxonRelation taxonRelation in parents)
            //        {
            //            ITaxon childTaxon = CoreData.TaxonManager.GetTaxonById(_user, taxonRelation.Taxon.Id);
            //            ITaxon parentTaxon = taxonRelation.RelatedTaxon;

            //            if (childTaxon.IsValid == false || parentTaxon.IsValid == false)
            //                continue;

            //            if (!taxonRelations.Contains(taxonRelation))
            //            {
            //                taxonRelations.Add(taxonRelation);                            
            //                table.Rows.Add();                            

            //                table.Rows[table.Rows.Count - 1]["ChildTaxonId"] = childTaxon.Id;
            //                table.Rows[table.Rows.Count - 1]["ChildTaxonName"] = childTaxon.ScientificName.IsNotEmpty() ? childTaxon.ScientificName : "";
            //                table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryId"] = childTaxon.Category != null ? childTaxon.Category.Id : -1;
            //                table.Rows[table.Rows.Count - 1]["ChildTaxonCategoryName"] = childTaxon.Category != null ? childTaxon.Category.Name : "";

            //                table.Rows[table.Rows.Count - 1]["ParentTaxonId"] = parentTaxon.Id;
            //                table.Rows[table.Rows.Count - 1]["ParentTaxonName"] = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : "";
            //                table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryId"] = parentTaxon.Category != null ? parentTaxon.Category.Id.ToString() : "";
            //                table.Rows[table.Rows.Count - 1]["ParentTaxonCategoryName"] = parentTaxon.Category != null ? parentTaxon.Category.Name : "";                            
            //            }
            //        }
            //    }                

            //}

            //return table;
        }

        protected List<ITaxon> GetLeafs(ITaxon taxon)
        {
            var leafs = new List<ITaxon>();
            // todo check if current taxon is leaf
            if (taxon.GetChildTaxonRelations(_user, false, false).Count == 0)
            {
                leafs.Add(taxon);
            }

            TaxonRelationList childTaxonRelations = taxon.GetAllChildTaxonRelations(_user);
            if (childTaxonRelations != null)
            {
                foreach (ITaxonRelation childTaxonRelation in childTaxonRelations)
                {
                    // todo check if related taxon is leaf                       
                    var childTaxon = CoreData.TaxonManager.GetTaxon(_user, childTaxonRelation.ChildTaxon.Id);
                    var children = childTaxon.GetChildTaxonRelations(_user, false, false);

                    if (children.Count == 0)
                    {
                        leafs.Add(childTaxon);
                    }                    
                }
            }
            return leafs;
        }

        private Dictionary<ITaxon, List<ITaxon>> CreateParentChildDictionary(IEnumerable<ITaxon> taxa)
        {            
            var taxonRelations = new HashSet<ITaxonRelation>();
            var parentChildDictionary = new Dictionary<ITaxon, List<ITaxon>>();
            bool getAllChildren = true;

            if (getAllChildren)
            {
                foreach (ITaxon taxon in taxa)
                {
                    List<ITaxon> leafs = GetLeafs(taxon);
                    foreach (ITaxon leaf in leafs)
                    {
                        IList<ITaxonRelation> parents = leaf.GetAllParentTaxonRelations(_user, null, false, false);

                        foreach (ITaxonRelation taxonRelation in parents)
                        {         
                            ITaxon childTaxon = CoreData.TaxonManager.GetTaxon(_user, taxonRelation.ChildTaxon.Id);
                            ITaxon parentTaxon = taxonRelation.ParentTaxon;

                            if (childTaxon.IsValid == false || parentTaxon.IsValid == false)
                            {
                                continue;
                            }

                            if (!taxonRelations.Contains(taxonRelation))
                            {
                                taxonRelations.Add(taxonRelation);

                                if (!parentChildDictionary.ContainsKey(parentTaxon))
                                {
                                    parentChildDictionary.Add(parentTaxon, new List<ITaxon>());
                                }

                                parentChildDictionary[parentTaxon].Add(childTaxon);
                            }
                        }

                        // add leaf
                        if (!parentChildDictionary.ContainsKey(leaf))
                        {
                            parentChildDictionary.Add(leaf, null);
                        }
                    }
                }
            }
            else
            {
                // Kod för att bara gå uppåt i trädet.
                foreach (ITaxon taxon in taxa)
                {
                    IList<ITaxonRelation> parents = taxon.GetAllParentTaxonRelations(_user, null, false, false);

                    foreach (ITaxonRelation taxonRelation in parents)
                    {
                        ITaxon childTaxon = CoreData.TaxonManager.GetTaxon(_user, taxonRelation.ChildTaxon.Id);
                        ITaxon parentTaxon = taxonRelation.ParentTaxon;

                        if (childTaxon.IsValid == false || parentTaxon.IsValid == false)
                        {
                            continue;
                        }

                        if (!taxonRelations.Contains(taxonRelation))
                        {
                            taxonRelations.Add(taxonRelation);

                            if (!parentChildDictionary.ContainsKey(parentTaxon))
                            {
                                parentChildDictionary.Add(parentTaxon, new List<ITaxon>());
                            }
                            if (parentChildDictionary[parentTaxon] == null)
                            {
                                parentChildDictionary[parentTaxon] = new List<ITaxon>();
                            }
                            parentChildDictionary[parentTaxon].Add(childTaxon);
                        }
                    }

                    // add leaf
                    if (!parentChildDictionary.ContainsKey(taxon))
                    {
                        parentChildDictionary.Add(taxon, null);
                    }
                }
            }

            return parentChildDictionary;
        }

        private HashSet<LumpSplitExportModel> CreateLumpExportList(Dictionary<ITaxon, List<ITaxon>> parentChildDictionary)
        {
            var lumpExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in parentChildDictionary.Keys)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(_user, taxon);                    
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(_user, taxon);                    
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(_user, lumpSplitEvent);
                        if (!lumpExportList.Contains(lumpExportModel))
                        {
                            lumpExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return lumpExportList;
        }

        private HashSet<LumpSplitExportModel> CreateSplitExportList(Dictionary<ITaxon, List<ITaxon>> parentChildDictionary)
        {
            var splitExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in parentChildDictionary.Keys)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(_user, taxon);
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(_user, taxon);
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(_user, lumpSplitEvent);
                        if (!splitExportList.Contains(lumpExportModel))
                        {
                            splitExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return splitExportList;
        }

        public DataTable GetTaxonLumpRelationTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonLumpRelation";

            AddColumn(table, "CreatedDate");
            AddColumn(table, "TaxonBeforeId", typeof(Int32));
            AddColumn(table, "TaxonBeforeName");
            AddColumn(table, "TaxonAfterId", typeof(Int32));
            AddColumn(table, "TaxonAfterName");

            foreach (var lumpModel in _lumpExportList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = lumpModel.CreatedDate;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeId"] = lumpModel.TaxonBeforeId;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeName"] = lumpModel.TaxonBeforeName;
                table.Rows[table.Rows.Count - 1]["TaxonAfterId"] = lumpModel.TaxonAfterId;
                table.Rows[table.Rows.Count - 1]["TaxonAfterName"] = lumpModel.TaxonAfterName;
            }

            return table;
        }

        public static DataTable GetTaxonLumpRelationTable(IUserContext userContext, HashSet<LumpSplitExportModel> lumps)
        {
            var table = new DataTable();
            table.TableName = "TaxonLumpRelation";

            AddColumnStatic(table, "CreatedDate");
            AddColumnStatic(table, "TaxonBeforeId", typeof(Int32));
            AddColumnStatic(table, "TaxonBeforeName");
            AddColumnStatic(table, "TaxonAfterId", typeof(Int32));
            AddColumnStatic(table, "TaxonAfterName");

            foreach (var lumpModel in lumps)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = lumpModel.CreatedDate;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeId"] = lumpModel.TaxonBeforeId;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeName"] = lumpModel.TaxonBeforeName;
                table.Rows[table.Rows.Count - 1]["TaxonAfterId"] = lumpModel.TaxonAfterId;
                table.Rows[table.Rows.Count - 1]["TaxonAfterName"] = lumpModel.TaxonAfterName;
            }

            return table;
        }




        public
            DataTable GetTaxonSplitRelationTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonSplitRelation";

            AddColumn(table, "CreatedDate");
            AddColumn(table, "TaxonBeforeId", typeof(Int32));
            AddColumn(table, "TaxonBeforeName");
            AddColumn(table, "TaxonAfterId", typeof(Int32));
            AddColumn(table, "TaxonAfterName");
            
            foreach (var splitModel in _splitExportList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = splitModel.CreatedDate;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeId"] = splitModel.TaxonBeforeId;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeName"] = splitModel.TaxonBeforeName;
                table.Rows[table.Rows.Count - 1]["TaxonAfterId"] = splitModel.TaxonAfterId;
                table.Rows[table.Rows.Count - 1]["TaxonAfterName"] = splitModel.TaxonAfterName;
            }

            return table;
        }

        public static DataTable GetTaxonSplitRelationTable(IUserContext userContext, HashSet<LumpSplitExportModel> splits)
        {
            var table = new DataTable();
            table.TableName = "TaxonSplitRelation";

            AddColumnStatic(table, "CreatedDate");
            AddColumnStatic(table, "TaxonBeforeId", typeof(Int32));
            AddColumnStatic(table, "TaxonBeforeName");
            AddColumnStatic(table, "TaxonAfterId", typeof(Int32));
            AddColumnStatic(table, "TaxonAfterName");

            foreach (var splitModel in splits)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["CreatedDate"] = splitModel.CreatedDate;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeId"] = splitModel.TaxonBeforeId;
                table.Rows[table.Rows.Count - 1]["TaxonBeforeName"] = splitModel.TaxonBeforeName;
                table.Rows[table.Rows.Count - 1]["TaxonAfterId"] = splitModel.TaxonAfterId;
                table.Rows[table.Rows.Count - 1]["TaxonAfterName"] = splitModel.TaxonAfterName;
            }

            return table;
        }

        public DataTable GetRemovedTaxonsTable(List<ITaxon> taxa)
        {
            var table = new DataTable();
            table.TableName = "Borttagna taxon";

            return table;
        }
        public DataTable GetTaxonNameCategoriesTable()
        {
            var table = new DataTable();            
            table.TableName = "TaxonNameCategories";
            TaxonNameCategoryList nameCategories = CoreData.TaxonManager.GetTaxonNameCategories(_user);

            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");
            AddColumn(table, "ShortName");
            AddColumn(table, "SortOrder", typeof(Int32));

            foreach (TaxonNameCategory taxonNameCategory in nameCategories)
            {                
                table.Rows.Add();

                table.Rows[table.Rows.Count - 1]["Id"] = taxonNameCategory.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = taxonNameCategory.Name;
                table.Rows[table.Rows.Count - 1]["ShortName"] = taxonNameCategory.ShortName;
                table.Rows[table.Rows.Count - 1]["SortOrder"] = taxonNameCategory.SortOrder;
            }

            return table;
        }

        public static DataTable GetTaxonNameCategoriesTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonNameCategories";
            TaxonNameCategoryList nameCategories = CoreData.TaxonManager.GetTaxonNameCategories(userContext);

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "ShortName");
            AddColumnStatic(table, "SortOrder", typeof(Int32));

            foreach (TaxonNameCategory taxonNameCategory in nameCategories)
            {
                table.Rows.Add();

                table.Rows[table.Rows.Count - 1]["Id"] = taxonNameCategory.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = taxonNameCategory.Name;
                table.Rows[table.Rows.Count - 1]["ShortName"] = taxonNameCategory.ShortName;
                table.Rows[table.Rows.Count - 1]["SortOrder"] = taxonNameCategory.SortOrder;
            }

            return table;
        }

        public DataTable GetChangeStatusTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonChangeStatus";
            
            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");

            Array enumValues = Enum.GetValues(typeof(TaxonChangeStatusId));
            string[] enumNames = Enum.GetNames(typeof(TaxonChangeStatusId));

            for (int i = 0; i < enumValues.Length && i < enumNames.Length; i++)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = enumValues.GetValue(i);
                table.Rows[table.Rows.Count - 1]["Name"] = enumNames[i];                
            }
            return table;            
        }

        public static DataTable GetChangeStatusTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonChangeStatus";

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");

            Array enumValues = Enum.GetValues(typeof(TaxonChangeStatusId));
            string[] enumNames = Enum.GetNames(typeof(TaxonChangeStatusId));

            for (int i = 0; i < enumValues.Length && i < enumNames.Length; i++)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = enumValues.GetValue(i);
                table.Rows[table.Rows.Count - 1]["Name"] = enumNames[i];
            }
            return table;
        }

        public DataTable GetAlertStatusTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonAlertStatus";

            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");

            Array enumValues = Enum.GetValues(typeof(TaxonAlertStatusId));
            string[] enumNames = Enum.GetNames(typeof(TaxonAlertStatusId));

            for (int i = 0; i < enumValues.Length && i < enumNames.Length; i++)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = enumValues.GetValue(i);
                table.Rows[table.Rows.Count - 1]["Name"] = enumNames[i];
            }
            return table;
        }

        public static DataTable GetAlertStatusTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonAlertStatus";

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");

            Array enumValues = Enum.GetValues(typeof(TaxonAlertStatusId));
            string[] enumNames = Enum.GetNames(typeof(TaxonAlertStatusId));

            for (int i = 0; i < enumValues.Length && i < enumNames.Length; i++)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = enumValues.GetValue(i);
                table.Rows[table.Rows.Count - 1]["Name"] = enumNames[i];
            }
            return table;
        }

        public DataTable GetTaxonNameStatusTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonNameStatus";
            TaxonNameStatusList nameStatusList = CoreData.TaxonManager.GetTaxonNameStatuses(_user);

            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");
            AddColumn(table, "Description");

            foreach (ITaxonNameStatus nameStatus in nameStatusList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = nameStatus.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = nameStatus.Name;
                table.Rows[table.Rows.Count - 1]["Description"] = nameStatus.Description;                
            }

            return table;
        }

        public static DataTable GetTaxonNameStatusTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonNameStatus";
            TaxonNameStatusList nameStatusList = CoreData.TaxonManager.GetTaxonNameStatuses(userContext);

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "Description");

            foreach (ITaxonNameStatus nameStatus in nameStatusList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = nameStatus.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = nameStatus.Name;
                table.Rows[table.Rows.Count - 1]["Description"] = nameStatus.Description;
            }

            return table;
        }

        public DataTable GetTaxonNameUsageTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonNameUsage";
            TaxonNameUsageList nameUsageList = CoreData.TaxonManager.GetTaxonNameUsages(_user);

            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");
            AddColumn(table, "Description");

            foreach (ITaxonNameUsage nameUsage in nameUsageList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = nameUsage.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = nameUsage.Name;
                table.Rows[table.Rows.Count - 1]["Description"] = nameUsage.Description;
            }

            return table;
        }

        public static DataTable GetTaxonNameUsageTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonNameUsage";
            TaxonNameUsageList nameUsageList = CoreData.TaxonManager.GetTaxonNameUsages(userContext);

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "Description");

            foreach (ITaxonNameUsage nameUsage in nameUsageList)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = nameUsage.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = nameUsage.Name;
                table.Rows[table.Rows.Count - 1]["Description"] = nameUsage.Description;
            }

            return table;
        }

        public DataTable GetTaxonCategoriesTable()
        {
            var table = new DataTable();
            table.TableName = "TaxonCategories";            
            TaxonCategoryList categories = CoreData.TaxonManager.GetTaxonCategories(_user);
            
            AddColumn(table, "Id", typeof(Int32));
            AddColumn(table, "Name");
            AddColumn(table, "IsMainCategory");
            AddColumn(table, "IsTaxonomic");
            AddColumn(table, "ParentId");
            AddColumn(table, "SortOrder");

            foreach (ITaxonCategory category in categories)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = category.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = category.Name;
                table.Rows[table.Rows.Count - 1]["IsMainCategory"] = category.IsMainCategory;
                table.Rows[table.Rows.Count - 1]["IsTaxonomic"] = category.IsTaxonomic;
                table.Rows[table.Rows.Count - 1]["ParentId"] = category.ParentId;
                table.Rows[table.Rows.Count - 1]["SortOrder"] = category.SortOrder;                
            }

            return table;
        }

        public static DataTable GetTaxonCategoriesTable(IUserContext userContext, List<ITaxon> taxaList)
        {
            var table = new DataTable();
            table.TableName = "TaxonCategories";
            TaxonCategoryList categories = CoreData.TaxonManager.GetTaxonCategories(userContext);

            AddColumnStatic(table, "Id", typeof(Int32));
            AddColumnStatic(table, "Name");
            AddColumnStatic(table, "IsMainCategory");
            AddColumnStatic(table, "IsTaxonomic");
            AddColumnStatic(table, "ParentId");
            AddColumnStatic(table, "SortOrder");

            foreach (ITaxonCategory category in categories)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1]["Id"] = category.Id;
                table.Rows[table.Rows.Count - 1]["Name"] = category.Name;
                table.Rows[table.Rows.Count - 1]["IsMainCategory"] = category.IsMainCategory;
                table.Rows[table.Rows.Count - 1]["IsTaxonomic"] = category.IsTaxonomic;
                table.Rows[table.Rows.Count - 1]["ParentId"] = category.ParentId;
                table.Rows[table.Rows.Count - 1]["SortOrder"] = category.SortOrder;
            }

            return table;
        }

        protected void AddColumn(DataTable table, string caption, string columnName)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = caption,
                ColumnName = columnName,
            });
        }

        protected void AddColumn(DataTable table, string caption, string columnName, Type type)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = caption,
                ColumnName = columnName,
                DataType = type
            });
        }

        protected void AddColumn(DataTable table, string columnName)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = columnName,
                ColumnName = columnName
            });
        }

        protected void AddColumn(DataTable table, string columnName, Type type)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = columnName,
                ColumnName = columnName,
                DataType = type
            });
        }

        public static void AddColumnStatic(DataTable table, string columnName)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = columnName,
                ColumnName = columnName
            });
        }

        public static void AddColumnStatic(DataTable table, string columnName, Type type)
        {
            table.Columns.Add(new DataColumn
            {
                Caption = columnName,
                ColumnName = columnName,
                DataType = type
            });
        }
    }

    public class LumpSplitExportModel
    {
        public DateTime CreatedDate { get; set; }
        public int TaxonBeforeId { get; set; }
        public int TaxonAfterId { get; set; }
        public string TaxonBeforeName { get; set; }
        public string TaxonAfterName { get; set; }
        public ITaxon TaxonBefore { get; set; }
        public ITaxon TaxonAfter { get; set; }

        public static LumpSplitExportModel Create(IUserContext userContext, ILumpSplitEvent lumpSplitEvent)
        {
            var model = new LumpSplitExportModel();            
            model.CreatedDate = lumpSplitEvent.CreatedDate;
            model.TaxonBeforeId = lumpSplitEvent.TaxonBefore.Id;
            model.TaxonAfterId = lumpSplitEvent.TaxonAfter.Id;
            model.TaxonBefore = CoreData.TaxonManager.GetTaxon(userContext, model.TaxonBeforeId);
            model.TaxonAfter = CoreData.TaxonManager.GetTaxon(userContext, model.TaxonAfterId);
            model.TaxonBeforeName = model.TaxonBefore.ScientificName;
            model.TaxonAfterName = model.TaxonAfter.ScientificName;

            //ITaxonName beforeName = lumpSplitEvent.TaxonBefore.GetScientificName(CoreData.UserManager.GetCurrentUser());
            //model.TaxonBeforeName = beforeName != null ? beforeName.Name : "";
            //ITaxonName afterName = lumpSplitEvent.TaxonAfter.GetScientificName(CoreData.UserManager.GetCurrentUser());
            //model.TaxonAfterName = afterName != null ? afterName.Name : "";

            return model;
        }

        public bool Equals(LumpSplitExportModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.TaxonBeforeId == TaxonBeforeId && other.TaxonAfterId == TaxonAfterId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(LumpSplitExportModel))
            {
                return false;
            }

            return Equals((LumpSplitExportModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TaxonBeforeId * 397) ^ TaxonAfterId;
            }
        }

        public static bool operator ==(LumpSplitExportModel left, LumpSplitExportModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LumpSplitExportModel left, LumpSplitExportModel right)
        {
            return !Equals(left, right);
        }
    }
}
