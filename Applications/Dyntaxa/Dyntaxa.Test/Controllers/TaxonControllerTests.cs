using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Data;
using Dyntaxa.Controllers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dyntaxa.Test.Controllers
{
    using Dyntaxa.Test;

    /// <summary>
    /// The taxon controller tests.
    /// </summary>
    [TestClass]
    public class TaxonControllerTests : ControllerNightlyTestBase
    {

        [TestMethod]
        public void GetTaxonParents()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, 6008300);
                //Act                
                var parents = taxon.GetAllParentTaxonRelations(userContext, null, false, false);

                //Assert
                Assert.IsNotNull(parents);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxonTrees_BreadthFirstAndDepthFirstIterationTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                const int CanidaeTaxonId = 2002154;
                /* Hierarchical structure for Canidae. 2016-05-12.
                  Canidae (2002154)
                   Canis
                    Canis Lupus
                     Canis lupus lupus
                     Canis lupus familiaris
                   Vulpes
                    Vulpes vulpes
                    Vulpes lagopus
                   Nycterutes
                    Nyctereutes procyonides
               */
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                TaxonTreeSearchCriteria taxonTreeSearchCriteria = new TaxonTreeSearchCriteria();
                taxonTreeSearchCriteria.TaxonIds = new List<int>();
                taxonTreeSearchCriteria.TaxonIds.Add(CanidaeTaxonId);
                taxonTreeSearchCriteria.IsValidRequired = true;
                taxonTreeSearchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
                
                //Act
                var taxonTreeNodeList = CoreData.TaxonManager.GetTaxonTrees(userContext, taxonTreeSearchCriteria);
                List<ITaxonTreeNode> breadthFirstList = new List<ITaxonTreeNode>();
                Debug.WriteLine("Breadth first");
                Debug.WriteLine("==============");
                foreach (ITaxonTreeNode taxonTreeNode in taxonTreeNodeList.AsBreadthFirstIterator())
                {
                    breadthFirstList.Add(taxonTreeNode);
                    Debug.WriteLine(taxonTreeNode.Taxon.ScientificName);                    
                }
                Debug.WriteLine("");
                Debug.WriteLine("Depth first");
                Debug.WriteLine("==============");
                List<ITaxonTreeNode> depthFirstList = new List<ITaxonTreeNode>();
                foreach (ITaxonTreeNode taxonTreeNode in taxonTreeNodeList.AsDepthFirstIterator())
                {
                    depthFirstList.Add(taxonTreeNode);
                    Debug.WriteLine(taxonTreeNode.Taxon.ScientificName);                    
                }

                //Assert
                // Check contains same elements.
                CollectionAssert.AreEquivalent(breadthFirstList, depthFirstList);

                // Check the order of the elements are not equal.
                CollectionAssert.AreNotEqual(breadthFirstList, depthFirstList);

            }
        }
        


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void CreateBiotaTaxonRelationsParentsTreeTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                const int BiotaTaxonId = 0;
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, BiotaTaxonId);                
                TaxonRelationsTree tree = TaxonRelationsTreeManager.CreateTaxonRelationsParentsTree(userContext, taxon);
                ITaxonRelationsTreeNode node = tree.GetTreeNode(BiotaTaxonId);

                // Act
                List<ITaxonRelationsTreeEdge> allValidParentEdgesTopToBottom = node.GetAllValidParentEdgesTopToBottom(true);

                // Assert
                Assert.IsNotNull(allValidParentEdgesTopToBottom);
                Assert.AreEqual(0, allValidParentEdgesTopToBottom.Count);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetAllBiotaParentsHierarchicalTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                const int BiotaTaxonId = 0;
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, BiotaTaxonId);

                // Act
                TaxonInfoViewModel taxonInfoViewModel = new TaxonInfoViewModel(taxon, userContext, null, null);

                // Assert
                Assert.IsTrue(taxonInfoViewModel.ParentTaxa != null);
                Assert.IsTrue(taxonInfoViewModel.ParentTaxa.Count == 0);
                Assert.IsTrue(taxonInfoViewModel.OtherParentTaxa != null);
                Assert.IsTrue(taxonInfoViewModel.OtherParentTaxa.Count == 0);
            }
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetAllGuldmosslavParentsHierarchicalTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                const int GuldmosslavTaxonId = 228321;
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
                searchCriteria.Taxa = new TaxonList {CoreData.TaxonManager.GetTaxon(userContext, GuldmosslavTaxonId)};
                TaxonRelationList guldmosslavAllParentRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
                TaxonRelationsTree tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, guldmosslavAllParentRelations, searchCriteria.Taxa);
                ITaxonRelationsTreeNode node = tree.GetTreeNode(GuldmosslavTaxonId);

                //Act
                List<ITaxonRelationsTreeEdge> parentEdges = node.GetAllValidParentEdgesTopToBottom(true);

                //Assert
                Assert.AreEqual(0, parentEdges.First().Parent.Taxon.Category.Id); // Assert first item is Biota
                Assert.AreEqual(GuldmosslavTaxonId, parentEdges.Last().Child.Taxon.Id); // Assert last item is guldmosslav
                // When getting guldmosslav taxonrelations a non valid parent relation is included.
                // which should be removed when getting all parents hierarchical using the tree.
                Assert.AreNotEqual(guldmosslavAllParentRelations.Count, parentEdges.Count);                
            }
        }

        private LumpSplitEventList GetLumpEvents(IUserContext userContext, ITaxon taxon)
        {
            LumpSplitEventList lumpEvents = null;
            if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.InvalidDueToLump)
            {
                lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);
            }
            else if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.ValidAfterLump)
            {
                lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);
            }

            return lumpEvents;            
        }

        private LumpSplitEventList GetSplitEvents(IUserContext userContext, ITaxon taxon)
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

            return lumpEvents;
        }

        

        [TestMethod]
        public void SplitGraphTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                const int lumpSplitTaxonId = 2755;
                //TaxonIdBefore: 233285
                //TaxonIdAfter: 2755
                
                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                TaxonList allTaxa = CoreData.TaxonManager.GetTaxa(userContext, taxonSearchCriteria);

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                TaxonRelationList allRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
                TaxonRelationsTree tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, allRelations,
                    allTaxa, false);

                var edges = tree.GetAllChildAndParentEdges(lumpSplitTaxonId);
                var edges2 = tree.GetAllValidChildAndParentEdges(lumpSplitTaxonId);
                List<ITaxonRelationsTreeNode> sourceNodes = new List<ITaxonRelationsTreeNode> { tree.GetTreeNode(lumpSplitTaxonId) };
                GraphVizFormat graphVizFormat = new GraphVizFormat()
                {
                    ShowLumpsAndSplits = true,
                    ShowRelationId = false
                };

                string graphRepresentation2 = GraphvizManager.CreateGraphvizFormatRepresentation(
                    userContext, 
                    tree,
                    edges2, 
                    sourceNodes, 
                    graphVizFormat);
                int x = 8;
            }
        }


        [
            TestMethod]
        public void LumpSplitGraphTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                const int lumpSplitTaxonId = 1013561;

                const int splitTaxonId = 233285; // Before: 233285, After: 2755

                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                TaxonList allTaxa = CoreData.TaxonManager.GetTaxa(userContext, taxonSearchCriteria);

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                TaxonRelationList allRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
                TaxonRelationsTree tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, allRelations, allTaxa, false);

                var edges = tree.GetAllChildAndParentEdges(lumpSplitTaxonId);
                var edges2 = tree.GetAllValidChildAndParentEdges(lumpSplitTaxonId);
                GraphVizFormat graphVizFormat = new GraphVizFormat()
                {
                    ShowLumpsAndSplits = true,
                    ShowRelationId = false
                };

                string graphRepresentation2 =
                    GraphvizManager.CreateGraphvizFormatRepresentation(
                        userContext, 
                        tree,
                        edges2, 
                        null, 
                        graphVizFormat);                
            }
        }

        [TestMethod]
        public void OtherParentTaxaTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;                
                const int hybridTaxonId = 266838;
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, hybridTaxonId);

                //Act
                TaxonInfoViewModel model = new TaxonInfoViewModel(taxon, userContext, null, null);

                //Assert
                Assert.AreEqual(3, model.OtherParentTaxa.Count);
                Assert.AreEqual("Artkomplex", model.OtherParentTaxa[0].Category);
                Assert.AreEqual("Art", model.OtherParentTaxa[1].Category);
                Assert.AreEqual("Art", model.OtherParentTaxa[2].Category);
            }
        }

        [TestMethod]
        public void ValidateTaxonRelationsTree()
        {
            using (ShimsContext.Create())
            {
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;

                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                var allTaxa = CoreData.TaxonManager.GetTaxa(userContext, taxonSearchCriteria);

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                var allRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
                var tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, allRelations, allTaxa, false);

                List<ITaxonRelationsTreeNode> collectiveTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> hybridTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> artkomplexTaxa = new List<ITaxonRelationsTreeNode>();

                List<ITaxonRelationsTreeNode> validCollectiveTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> validHybridTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> validArtkomplexTaxa = new List<ITaxonRelationsTreeNode>();

                List<ITaxonRelationsTreeNode> possiblyInvalidCollectiveTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> possiblyInvalidHybridTaxa = new List<ITaxonRelationsTreeNode>();
                List<ITaxonRelationsTreeNode> possiblyInvalidArtkomplexTaxa = new List<ITaxonRelationsTreeNode>();

                List<ITaxonRelationsTreeNode> notYetHandledHybridTaxa = new List<ITaxonRelationsTreeNode>();


                foreach (var node in tree.AsDepthFirstNodeIterator())
                {
                    if (node.Taxon.Category.Id == 27)
                    {
                        collectiveTaxa.Add(node);

                        // Kollektivtaxon ska ha 0 eller flera barn?
                        // MainChildren = 0 && SecondaryChildren >= 0
                        if (node.ValidMainChildren == null)
                        {
                            validCollectiveTaxa.Add(node);
                        }
                        else
                        {
                            possiblyInvalidCollectiveTaxa.Add(node);
                        }
                    }
                    if (node.Taxon.Category.Id == 21)
                    {
                        hybridTaxa.Add(node);

                        // Hybrider ska ha exakt 1 primär förälder precis som alla andra taxon.
                        // När alla Hybrider har hanterats i revisioner (Mora är på gång att ta tag i det så ska den ha
                        // 1 primär relation oftast under ett släkte (minsta gemensamma nämnaren för föräldrarna) 
                        // men ibland även högre upp. Tyvärr verkar det ligga någon regel i Dyntaxa som 
                        // förhindrar vissa av dessa saker. För släktet Anodonta kan jag t ex inte lägga till 
                        // ett artkomplex (möjligen samma för hybrider) som sekundär förälder.
                        // 2 eller flera sekundära föräldrar. Oftast till arter.

                        // Hybrider ska ha exakt 2 sekundära föräldrar.
                        // och exakt 1 primär förälder.
                        if (node.ValidMainParents != null && node.ValidMainParents.Count == 1 &&
                            node.ValidSecondaryParents != null && node.ValidSecondaryParents.Count >= 2)
                        {
                            validHybridTaxa.Add(node);
                        }
                        else
                        {
                            notYetHandledHybridTaxa.Add(node);
                            //possiblyInvalidHybridTaxa.Add(node);
                        }
                    }
                    if (node.Taxon.Category.Id == 28)
                    {
                        artkomplexTaxa.Add(node);

                        // Artkomplex ska ha 0 eller fler barn?
                        // MainChildren = 0 && SecondaryChildren >= 0
                        if (node.ValidMainChildren == null)
                        {
                            validArtkomplexTaxa.Add(node);
                        }
                        else
                        {
                            possiblyInvalidArtkomplexTaxa.Add(node);
                        }

                        if (node.ValidSecondaryChildren != null && node.ValidSecondaryChildren.Count == 1)
                        {
                            int z = 8;
                        }
                    }
                }
                int t = 8;
            }
        }

        [TestMethod]
        public void CreateCompleteTaxonRelationsTree()
        {
            using (ShimsContext.Create())
            {
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;

                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                var allTaxa = CoreData.TaxonManager.GetTaxa(userContext, taxonSearchCriteria);

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                var allRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);

                var tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, allRelations, allTaxa, false);

                //1012265
                //1011617
                //6001191 ensam                               
                var edges = tree.GetAllChildAndParentEdges(266838);
                string graphviz = GraphvizManager.CreateGraphvizFormatRepresentation(edges);

                // find suspicious hybrids
                List<ITaxonRelationsTreeNode> invalidHybrids;

                
                // find root nodes                
                tree.RootNodes = new HashSet<ITaxonRelationsTreeNode>();
                tree.ValidRootNodes = new HashSet<ITaxonRelationsTreeNode>();
                foreach (var node in tree.AllTreeNodes)
                {
                    tree.RootNodes.Add(node.RootNode);

                    if (node.RootNode.Taxon.IsValid)
                    {
                        tree.ValidRootNodes.Add(node.RootNode);
                    }
                }
            }
        }       

        //[TestMethod]
        //public void LoadWithLumpSplits()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        LoginApplicationUserAndSetSessionVariables();
        //        SetSwedishLanguage();
        //        IUserContext userContext = ApplicationUserContextSV;
        //        TaxonRelationsCacheManager.InitTaxonRelationsCache(userContext);

        //        var tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(
        //            userContext,
        //            TaxonRelationsCacheManager.TaxonRelationList);


        //        //var taxon = tree.TreeNodeDictionary[2002159];
        //        //HashSet<ITaxon> taxa = new HashSet<ITaxon>();

        //        List<int> taxonIds = new List<int>();
        //        taxonIds.Add(2002159);
        //        var edges = GetTreeEdges(tree, taxonIds);
        //        HashSet<ITaxon> taxa = new HashSet<ITaxon>();
        //        foreach (var edge in edges)
        //        {
        //            taxa.Add(edge.Child.Taxon);
        //            taxa.Add(edge.Parent.Taxon);
        //        }

        //        List<int> lumpSplitTaxonIds = new List<int>();
        //        lumpSplitTaxonIds.Add(2002157);
        //        var lumpSplitTreeNodes = GetTreeNodes(tree, lumpSplitTaxonIds);
        //        foreach (var node in lumpSplitTreeNodes)
        //        {
        //            taxa.Add(node.Taxon);
        //        }

        //        var lumpSplitEdges = GetTreeEdges(tree, lumpSplitTaxonIds);
        //        edges.UnionWith(lumpSplitEdges);
                
        //        foreach (var edge in edges)
        //        {                    
        //            Debug.WriteLine(edge);
        //        }



        //        //List<int> taxonIds = new List<int>();
        //        //taxonIds.Add(1012705);
        //        //taxonIds.Add(1016571);


        //        //var edges = GetTreeEdges(userContext, tree, taxonIds);
        //        //foreach (var edge in edges)
        //        //{
        //        //    Debug.WriteLine(edge);
        //        //}                
        //    }
        //}


        //[TestMethod]
        //public void LoadParents()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        LoginApplicationUserAndSetSessionVariables();
        //        SetSwedishLanguage();
        //        IUserContext userContext = ApplicationUserContextSV;
        //        TaxonRelationsCacheManager.InitTaxonRelationsCache(userContext);

        //        var tree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(
        //            userContext,
        //            TaxonRelationsCacheManager.TaxonRelationList);


        //        var mammaliaTree = GetTreeEdges(tree, new List<int> {4000107});
        //        var str = TaxonRelationsTreeManager.ToGraphvizFormat(mammaliaTree);

        //        var lampetraFluviatilis = tree.TreeNodeDictionary[102127];
        //        var petromyzontida = tree.TreeNodeDictionary[4000101];
        //        var vertebrata = tree.TreeNodeDictionary[6000993];
        //        var oldHashEdges = new HashSet<TaxonRelationsTreeEdge>();
        //        List<TaxonRelationsTreeEdge> edges = new List<TaxonRelationsTreeEdge>();
        //        // Get vertebrata tree
        //        foreach (var edge in petromyzontida.AsReversedBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //        {
        //            oldHashEdges.Add(edge);
        //            edges.Add(edge);                    
        //        }
        //        foreach (var edge in petromyzontida.AsDepthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.BothValidMainAndSecondaryChildren))
        //        {
        //            edges.Add(edge);
        //            oldHashEdges.Add(edge);                    
        //        }
        //        bool success = false;
        //        foreach (var edge in edges)
        //        {
        //            if (edge.TaxonRelation.Id == 98660)
        //            {
        //                success = true;
        //            }
        //        }
                

        //        HashSet<TaxonRelationsTreeEdge> hashEdges = new HashSet<TaxonRelationsTreeEdge>();
        //        // Get vertebrata tree
        //        foreach (var edge in petromyzontida.AsReversedBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //        {
        //            hashEdges.Add(edge);
        //        }
        //        foreach (var edge in petromyzontida.AsDepthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.BothValidMainAndSecondaryChildren))
        //        {
        //            hashEdges.Add(edge);

        //            if (edge.Child.ValidSecondaryParents != null)
        //            {
        //                foreach (var parentEdge in edge.Child.ValidSecondaryParents.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //                {
        //                    if (!hashEdges.Contains(parentEdge))
        //                    {
        //                        hashEdges.Add(parentEdge);
        //                    }                            
        //                }
        //            }
        //        }
        //        success = false;
        //        foreach (var edge in hashEdges)
        //        {
        //            if (edge.TaxonRelation.Id == 98660)
        //            {
        //                success = true;
        //            }
        //            if (edge.TaxonRelation.Id == 143327)
        //            {
        //                success = true;
        //            }
        //        }

        //        hashEdges.ExceptWith(oldHashEdges);

        //        // Get petromyzontida tree
        //        foreach (var edge in petromyzontida.AsReversedBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //        {
        //            edges.Add(edge);                    
        //        }
        //        foreach (var edge in petromyzontida.AsDepthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.BothValidMainAndSecondaryChildren))
        //        {
        //            edges.Add(edge);                    
        //        }
        //        foreach (var edge in edges)
        //        {
        //            Debug.WriteLine(edge);
        //        }
        //        int y = 8;



        //        Debug.WriteLine("AsBreadthFirstParentEdgeIterator - BothValidMainAndSecondaryParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsBreadthFirstParentEdgeIterator - OnlyValidMainParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.OnlyValidMainParents))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsReversedBreadthFirstParentEdgeIterator - BothValidMainAndSecondaryParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsReversedBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.BothValidMainAndSecondaryParents))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsReversedBreadthFirstParentEdgeIterator - OnlyValidMainParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsReversedBreadthFirstParentEdgeIterator(TaxonRelationsTreeIterationUpwardMode.OnlyValidMainParents))
        //        {
        //            Debug.WriteLine(edge);
        //        }


        //        Debug.WriteLine("AsDepthFirstChildEdgeIterator - BothValidMainAndSecondaryParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsDepthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.BothValidMainAndSecondaryChildren))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsDepthFirstChildEdgeIterator - OnlyValidMainChildren");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsDepthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.OnlyValidMainChildren))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsBreadthFirstChildEdgeIterator - BothValidMainAndSecondaryParents");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsBreadthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.BothValidMainAndSecondaryChildren))
        //        {
        //            Debug.WriteLine(edge);
        //        }

        //        Debug.WriteLine("");
        //        Debug.WriteLine("AsBreadthFirstChildEdgeIterator - OnlyValidMainChildren");
        //        Debug.WriteLine("=======================================");
        //        foreach (var edge in petromyzontida.AsBreadthFirstChildEdgeIterator(TaxonRelationsTreeIterationDownwardMode.OnlyValidMainChildren))
        //        {
        //            Debug.WriteLine(edge);
        //        }


        //        int nrNodes = 0;
        //        int nrNodesWithMultipleValidParents = 0;
        //        List<TaxonRelationsTreeNode> nodesWithMultipleValidParents = new List<TaxonRelationsTreeNode>();                
        //        foreach (TaxonRelationsTreeNode node in tree.AsDepthFirstIterator())
        //        {
        //            if (node.ValidMainParents != null && node.ValidMainParents.Count != 1)
        //            {
        //                nrNodesWithMultipleValidParents++;
        //                nodesWithMultipleValidParents.Add(node);
        //            }
        //            nrNodes++;
        //        }

        //        foreach (TaxonRelationsTreeNode node in nodesWithMultipleValidParents)
        //        {
        //            Debug.WriteLine("{0}, Föräldrar: {1}", node, string.Join(", ", node.ValidMainParents));
        //        }
        //    }
        //}
    }    
}
