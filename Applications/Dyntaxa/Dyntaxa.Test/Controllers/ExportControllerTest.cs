using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;
using ArtDatabanken.WebService.Proxy;
using Dyntaxa.Controllers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dyntaxa.Test.Controllers
{
    [TestClass]
    public class ExportControllerTest : ControllerNightlyTestBase
    {
        //[TestMethod]
        //public void GetTaxonTree()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        // Arrange
        //        LoginApplicationUserAndSetSessionVariables();
        //        SetSwedishLanguage();
        //        IUserContext userContext = ApplicationUserContextSV;
        //        TaxonRelationsCacheManager.InitTaxonRelationsCache(userContext);
        //        var tree = DyntaxaTreeManager.CreateDyntaxaTree(
        //            userContext,
        //            TaxonRelationsCacheManager.TaxonRelationList);

        //        int nrNodes = 0;
        //        foreach (DyntaxaTreeNode dyntaxaTreeNode in tree.AsDepthFirstIterator())
        //        {
        //            nrNodes++;
        //        }

        //        var mammaliaNode = tree.TreeNodeDictionary[4000107];
        //    }
        //}

        [TestMethod]
        public void CreateDatabaseExportForMammaliaUsingDyntaxaTree()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                List<ITaxon> taxa = new List<ITaxon>();
                taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, 4000107));
                
                // Act
                NewExportDataTableCollection model = ExportDatabaseViewModel.CreateDataTablesWithDyntaxaTree(
                    userContext, taxa);

                Assert.IsTrue(model.TaxonDataTable.Rows.Count > 200);
                Assert.IsTrue(model.ParentChildDataTable.Rows.Count > 200);
                Assert.IsTrue(model.TaxonReferencesDataTable.Rows.Count > 300);
                Assert.IsTrue(model.TaxonNameReferencesTable.Rows.Count > 900);
                Assert.IsTrue(model.LumpTaxonDataTable.Rows.Count >= 0);
                Assert.IsTrue(model.SplitTaxonDataTable.Rows.Count >= 0);
                Assert.IsTrue(model.TaxonNameDataTable.Rows.Count > 850);
            }
        }


        [TestMethod]
        public void CreateDatabaseExportUsingDyntaxaTree()
        {
            using (ShimsContext.Create())
            {
                WebServiceProxy.TaxonService.MaxBufferPoolSize = 0;

                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                List<ITaxon> taxa = new List<ITaxon>();
                taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, 4000107));
                //taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, 6000993));
                //taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, 102127));

                // Arrange old method             
                ExportDatabaseDataTableWriter exportDatabaseDataTableWriter = new ExportDatabaseDataTableWriter(userContext, taxa);

                var oldTaxonDataTable = exportDatabaseDataTableWriter.GetTaxonTable();
                var oldParentChildTaxaTable = exportDatabaseDataTableWriter.GetParentChildRelationsTable();
                var oldNameDataTable = exportDatabaseDataTableWriter.GetTaxonNameTable();
                var oldReferenceDataTable = exportDatabaseDataTableWriter.GetTaxonReferencesTable();
                var oldNameReferenceDataTable = exportDatabaseDataTableWriter.GetTaxonNameReferencesTable();
                var oldTaxonLumpTable = exportDatabaseDataTableWriter.GetTaxonLumpRelationTable();
                var oldTaxonSplitTable = exportDatabaseDataTableWriter.GetTaxonSplitRelationTable();
                var oldTaxonCategoriesTable = exportDatabaseDataTableWriter.GetTaxonCategoriesTable();
                var oldTaxonNameCategoriesTable = exportDatabaseDataTableWriter.GetTaxonNameCategoriesTable();
                var oldTaxonNameUsageTable = exportDatabaseDataTableWriter.GetTaxonNameUsageTable();
                var oldTaxonNameStatusTable = exportDatabaseDataTableWriter.GetTaxonNameStatusTable();
                var oldChangeStatusTable = exportDatabaseDataTableWriter.GetChangeStatusTable();
                var oldAlertStatusTable = exportDatabaseDataTableWriter.GetAlertStatusTable();
                var oldAllReferencesTable = exportDatabaseDataTableWriter.GetAllReferencesTable();    
                       
                // Act
                NewExportDataTableCollection model = ExportDatabaseViewModel.CreateDataTablesWithDyntaxaTree(userContext, taxa);

                

            //Dictionary<int, DataRow> dataRowDictionary = new Dictionary<int, DataRow>();
            //for (int i = 0; i < model.TaxonDataTable.Rows.Count; i++)
            //{
            //    int taxonId = (int)model.TaxonDataTable.Rows[i]["TaxonId"];                   
            //    if (!dataRowDictionary.ContainsKey(taxonId))
            //    {
            //        dataRowDictionary.Add(taxonId, model.TaxonDataTable.Rows[i]);
            //    }
            //}
            //Dictionary<int, DataRow> olddataRowDictionary = new Dictionary<int, DataRow>();

            //for (int i = 0; i < oldTaxonDataTable.Rows.Count; i++)
            //{
            //    int taxonId = (int)oldTaxonDataTable.Rows[i]["TaxonId"];
            //    if (!olddataRowDictionary.ContainsKey(taxonId))
            //    {
            //        olddataRowDictionary.Add(taxonId, oldTaxonDataTable.Rows[i]);
            //    }
            //}


            //HashSet<int> relationRowDictionary = new HashSet<>();
            //for (int i = 0; i < model.ParentChildDataTable.Rows.Count; i++)
            //{
            //    int taxonId = (int)model.ParentChildDataTable.Rows[i]["ParentTaxonId"];

            //    if (!relationRowDictionary.con(taxonId))
            //    {
            //        relationRowDictionary.Add(taxonId);
            //    }
            //}

            //HashSet<int> oldrelationRowDictionary = new HashSet<int>();
            //for (int i = 0; i < oldParentChildTaxaTable.Rows.Count; i++)
            //{
            //    int taxonId = (int)model.ParentChildDataTable.Rows[i]["ParentTaxonId"];
            //    int childId = (int)model.ParentChildDataTable.Rows[i]["ChildTaxonId"];
            //    if (!relationRowDictionary.Contains(taxonId) || !relationRowDictionary.Contains(childId))
            //    {
            //        oldrelationRowDictionary.Add(taxonId);
            //    }
            //}



            ////var newCompareTaxon = dataRowDictionary.Where(x => !olddataRowDictionary.ContainsKey(x.Key))
            ////        .ToDictionary(x => x.Key, x => x.Value);

            ////var oldCompareTaxon = olddataRowDictionary.Where(x => !dataRowDictionary.ContainsKey(x.Key))
            ////        .ToDictionary(x => x.Key, x => x.Value);


            //var newRelationTaxon = relationRowDictionary.Where(x => !oldrelationRowDictionary.ContainsKey(x.Key))
            //        .ToDictionary(x => x.Key, x => x.Value);

            //var oldRelationTaxon = oldrelationRowDictionary.Where(x => !relationRowDictionary.ContainsKey(x.Key))
            //        .ToDictionary(x => x.Key, x => x.Value);

                //Assert
                Assert.IsTrue(oldTaxonDataTable.Rows.Count == model.TaxonDataTable.Rows.Count);
                Assert.IsTrue(oldNameDataTable.Rows.Count == model.TaxonNameDataTable.Rows.Count);
                Assert.IsTrue(oldNameReferenceDataTable.Rows.Count == model.TaxonNameReferencesTable.Rows.Count);
                Assert.IsTrue(oldParentChildTaxaTable.Rows.Count == model.ParentChildDataTable.Rows.Count);
                Assert.IsTrue(oldReferenceDataTable.Rows.Count == model.TaxonReferencesDataTable.Rows.Count);
                Assert.IsTrue(oldTaxonLumpTable.Rows.Count == model.LumpTaxonDataTable.Rows.Count);
                Assert.IsTrue(oldTaxonSplitTable.Rows.Count == model.SplitTaxonDataTable.Rows.Count);
                Assert.IsTrue(oldTaxonCategoriesTable.Rows.Count == model.TaxonCategoriesTable.Rows.Count);
                Assert.IsTrue(oldTaxonNameCategoriesTable.Rows.Count == model.TaxonNameCategoriesTable.Rows.Count);
                Assert.IsTrue(oldTaxonNameUsageTable.Rows.Count == model.TaxonNameUsageTable.Rows.Count);
                Assert.IsTrue(oldTaxonNameStatusTable.Rows.Count == model.TaxonNameStatusTable.Rows.Count);
                Assert.IsTrue(oldChangeStatusTable.Rows.Count == model.ChangeStatusTable.Rows.Count);
                Assert.IsTrue(oldAlertStatusTable.Rows.Count == model.AlertStatusTable.Rows.Count);
                Assert.IsTrue(oldAllReferencesTable.Rows.Count == model.AllReferencesTable.Rows.Count);
            }
        }
        


        [TestMethod]
        public void CreateDatabaseExport()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;
                List<ITaxon> taxa = new List<ITaxon>();
                taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, 4000107));  
                              
                // Arrange old method
                ExportDatabaseDataTableWriter exportDatabaseDataTableWriter = new ExportDatabaseDataTableWriter(userContext, taxa);
                
                var oldTaxonDataTable = exportDatabaseDataTableWriter.GetTaxonTable();
                var oldParentChildTaxaTable = exportDatabaseDataTableWriter.GetParentChildRelationsTable();

               

                //Act                             
                NewExportDataTableCollection model = ExportDatabaseViewModel.CreateDataTablesWithTreesAlternative1(userContext, taxa);
                //var lumpTaxon = exportDatabaseDataTableWriter.GetTaxonLumpRelationTable();
                //var splitTaxon = exportDatabaseDataTableWriter.GetTaxonSplitRelationTable();


         




                //Assert Taxon Table
                for (int i = 0; i < oldTaxonDataTable.Rows.Count; i++)
                {
                    CollectionAssert.AreEqual(oldTaxonDataTable.Rows[i].ItemArray, model.TaxonDataTable.Rows[i].ItemArray);                    
                }

                //Assert Parent Child Table
                for (int i = 0; i < oldParentChildTaxaTable.Rows.Count; i++)
                {
                    CollectionAssert.AreEqual(oldParentChildTaxaTable.Rows[i].ItemArray, model.ParentChildDataTable.Rows[i].ItemArray);
                }

               

            }

          



            //var parentChildDictionary = new Dictionary<ITaxon, List<ITaxon>>()
        }

        /// <summary>
        ///A test for Http403
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http403Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http403() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("403"));
        }

        /// <summary>
        ///A test for Http404
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http404Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http404() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("404"));
        }

        /// <summary>
        ///A test for Http400
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http400Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http400() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("400"));
        }

        /// <summary>
        ///A test for Http500
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http500Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http500() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("500"));
        }
    }
}
