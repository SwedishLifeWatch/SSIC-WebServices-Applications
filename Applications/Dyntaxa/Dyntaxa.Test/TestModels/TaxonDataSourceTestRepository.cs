using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using Dyntaxa.Helpers;

namespace Dyntaxa.Test.TestModels
{
    using Dyntaxa.Test;

    public class TaxonDataSourceTestRepository:ITaxonDataSource

    {

        public static int testTaxonId = DyntaxaTestSettings.Default.TestTaxonId;
        public static int testRevisonId = DyntaxaTestSettings.Default.TestRevisionId;
        public ITaxon TestTaxon { get; set; }
        public ITaxonRevision TestTaxonRevision { get; set; }
        private IDataSourceInformation _dataSourceInformation = null;
        private const string serviceName = "TaxonServiceTestName";
        private const string serviceEndPoint = "TaxonServiceTestNameAdress";
        TaxonNameCategoryList taxonNameCategoryList = new TaxonNameCategoryList();

        
        
        
        public IDataSourceInformation GetDataSourceInformation()
        {
            _dataSourceInformation = new DataSourceInformation(serviceName,
                                                                serviceEndPoint,
                                                                DataSourceType.WebService);
            return _dataSourceInformation;
        }

        public void CreateLumpSplitEvent(IUserContext userContext, ILumpSplitEvent lumpSplitEvent)
        {
            throw new NotImplementedException();
        }

        public void CreateTaxon(IUserContext userContext, ITaxon taxon, ITaxonRevisionEvent taxonRevisionEvent)
        {
            throw new NotImplementedException();
        }

        public void CreateTaxonProperties(IUserContext userContext, ITaxonProperties taxonProperties)
        {
            throw new NotImplementedException();
        }

        public void CreateTaxonRelation(IUserContext userContext, ITaxonRelation taxonRelation)
        {
            throw new NotImplementedException();
        }

        public void CreateTaxonRevisionEvent(IUserContext userContext, ITaxonRevisionEvent taxonRevisionEvent)
        {
            throw new NotImplementedException();
        }

        public LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public void SaveTaxonNames(IUserContext userContext, TaxonNameList taxonNames, ITaxonRevisionEvent taxonRevisionEvent)
        {
            return;
        }

        public void UpdateTaxonProperties(IUserContext userContext, ITaxonProperties taxonProperties)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaxonRelation(IUserContext userContext, ITaxonRelation taxonRelation)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaxonRevisionEvent(IUserContext userContext, ITaxonRevisionEvent taxonRevisionEvent)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaxonTreeSortOrder(IUserContext userContext, int taxonIdParent, List<int> taxonIdChildren, ITaxonRevisionEvent taxonRevisionEvent)
        {
            throw new NotImplementedException();
        }

        public void CreateTaxonName(IUserContext userContext, ITaxonName taxonName)
        {
            taxonName.Id = DyntaxaTestSettings.Default.TestTaxonNameId;
            return;
        }

        public void SaveRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            throw new NotImplementedException();
        }

        public void SaveTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            if (taxonRevision.State.Id == (int)TaxonRevisionStateId.Created || taxonRevision.State.Id == 0)
            {
                taxonRevision.Id = DyntaxaTestSettings.Default.TestRevisionId;
                
            }
            else if (taxonRevision.State.Id == (int)TaxonRevisionStateId.Ongoing)
            {
                taxonRevision.Id = DyntaxaTestSettings.Default.TestRevisionOngoingId;
            }
            else
            {
                taxonRevision.Id = DyntaxaTestSettings.Default.TestRevisionPublishedId;
                
            }
            return;
        }

        public TaxonRevisionList GetTaxonRevisions(IUserContext userContext, ITaxonRevisionSearchCriteria searchCriteria)
        {
            TaxonRevisionList revList = new TaxonRevisionList();
            ITaxonRevision taxonRevision;
            if (searchCriteria.StateIds != null && searchCriteria.StateIds.Count == 3 && searchCriteria.TaxonIds == null)
            {
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Created.ToString()); 
                revList.Add(taxonRevision);
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                revList.Add(taxonRevision);
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Closed.ToString());
                revList.Add(taxonRevision);
            }
            else if (searchCriteria.StateIds != null && searchCriteria.StateIds.ElementAtOrDefault(0) == 1 )
            {
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Created.ToString());
                revList.Add(taxonRevision);
            }
            else if (searchCriteria.StateIds != null && searchCriteria.StateIds.ElementAtOrDefault(0) == 2 )
            {
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                revList.Add(taxonRevision);
            }
            else if (searchCriteria.StateIds != null && searchCriteria.StateIds.ElementAtOrDefault(0) == 3 )
            {
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Closed.ToString());
                revList.Add(taxonRevision);
            }
            
            else 
            {
                taxonRevision = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Created.ToString());
                revList.Add(taxonRevision);
            }
            
           
            return revList;
        }

        public TaxonRevisionList GetTaxonRevisions(IUserContext userContext, ITaxon taxon)
        {
            TaxonRevisionList list = new TaxonRevisionList();
            ITaxonRevision rev;
            if (taxon.Id == DyntaxaTestSettings.Default.TestTaxonId)
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Created.ToString());
                list.Add(rev);
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                list.Add(rev);
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Closed.ToString());
                list.Add(rev);
            }
            else if (taxon.Id.ToString() == "0")
            {
                rev = null;
            }
            else
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Created.ToString());
                list.Add(rev);
            }
            return list;
        }

        public TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext, int taxonRevisionId)
        {
            TaxonRevisionEventList list = new TaxonRevisionEventList();
            list.Add(new TaxonRevisionEvent() { Id = 22, CreatedBy = userContext.User.Id,
                                           RevisionId = GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString()).Id, 
                                           CreatedDate =  new DateTime(2011), AffectedTaxa = "Delfiner", NewValue = "nya delfiner", OldValue = "gamla",
                                           Type = new TaxonRevisionEventType(){Description = "event", Id = 1,Identifier = "Identifier"}}
                                           );
            return list;
        }

        public TaxonList GetTaxa(IUserContext userContext, List<int> taxonIds)
        {
            throw new NotImplementedException();
        }

        public TaxonList GetTaxa(IUserContext userContext, ITaxonSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public ITaxon GetTaxon(IUserContext userContext, int taxonId)
        {
            if (taxonId == DyntaxaTestSettings.Default.TestTaxonId )
            {
                return GetReferenceTaxon(userContext, taxonId);
            } 
            if (taxonId == DyntaxaTestSettings.Default.TestParentTaxonId)
            {
                ITaxon parentTaxon = GetReferenceParentTaxon(userContext, taxonId);
                foreach (ITaxonRelation parent in parentTaxon.GetNearestParentTaxonRelations(userContext))
                {
                    TaxonRelationList relations = new TaxonRelationList();
                    ITaxon grandParentTaxon = GetReferenceGrandParentTaxon(userContext, DyntaxaTestSettings.Default.TestParentTaxonId + 10);
                    ITaxonRelation rel = new TaxonRelation() { ParentTaxon = grandParentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30), IsMainRelation = true, ReplacedInTaxonRevisionEventId = null};
                    relations.Add(rel);
                    parent.ChildTaxon = grandParentTaxon;
                    parent.ChildTaxon.SetParentTaxa(relations);
                }
            
                return parentTaxon;
            }
            if (taxonId == DyntaxaTestSettings.Default.PsophusStridulusTaxonId)
            {
                return GetReferenceTaxon(userContext, taxonId);
            }
            if (taxonId == DyntaxaTestSettings.Default.ParnassiusApolloId)
            {
                return GetReferenceTaxon(userContext, taxonId);
            }
            
            return null;
        }

        public ITaxon GetTaxon(IUserContext userContext, string taxonGuid)
        {
            throw new NotImplementedException();
        }

        public TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public ITaxonName GetTaxonName(IUserContext userContext, string GUID)
        {
            throw new NotImplementedException();
        }

        public TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public TaxonTreeNodeList GetTaxonTrees(IUserContext userContext, ITaxonTreeSearchCriteria searchCriteria)
        {
            TaxonTreeNodeList list = new TaxonTreeNodeList();
            TaxonTreeNode  treeNode = new TaxonTreeNode();
            treeNode.Id = 22;
            treeNode.Taxon = this.GetReferenceTaxon(userContext, testTaxonId);
            TaxonTreeNodeList parentList = new TaxonTreeNodeList();

            TaxonTreeNode parentTreeNode = new TaxonTreeNode();
            parentTreeNode.Id = 21;
            parentTreeNode.Taxon = GetReferenceTaxon(userContext, 100067);
            parentList.Add(parentTreeNode);
            treeNode.Parents = parentList;

            TaxonTreeNodeList childList = new TaxonTreeNodeList();

            TaxonTreeNode childTreeNode = new TaxonTreeNode();
            childTreeNode.Id = 21;
            ITaxon taxon = GetReferenceTaxon(userContext, 100068);
            taxon.Category.IsTaxonomic = false;
            childTreeNode.Taxon = taxon;
            childList.Add(childTreeNode);
            treeNode.Children = childList;

            list.Add(treeNode);
            return list;
        }

        public TaxonTreeNodeList GetTaxonTreesBySearchCriteria(IUserContext userContext, ITaxonTreeSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public ILumpSplitEvent GetLumpSplitEvent(IUserContext userContext, string GUID)
        {
            throw new NotImplementedException();
        }

        public ITaxonRevision GetTaxonRevision(IUserContext userContext, string taxonRevisionGuid)
        {
            throw new NotImplementedException();
        }

        public ITaxonRevision GetTaxonRevision(IUserContext userContext, int taxonRevisionId)
        {
            ITaxonRevision rev;
            if(taxonRevisionId == DyntaxaTestSettings.Default.TestRevisionId)
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Created.ToString());
            }
            else if (taxonRevisionId == DyntaxaTestSettings.Default.TestRevisionOngoingId || taxonRevisionId == DevelopmentHelper.DefaultRevisionId)
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Ongoing.ToString());
            }
            else if (taxonRevisionId == DyntaxaTestSettings.Default.TestRevisionPublishedId)
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Closed.ToString());
            }
            else if(taxonRevisionId.ToString() == "0")
            {
                rev = null;
            }
            else
            {
                rev = GetReferenceRevision(userContext, testTaxonId, TaxonRevisionStateId.Created.ToString());
            }
            return rev;
        }

        public TaxonCategoryList GetTaxonCategories(IUserContext userContext)
        {
            TaxonCategoryList taxonCategories = new TaxonCategoryList();
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = -2, Name = "Grupp", IsMainCategory = false, ParentId = -2, SortOrder = -2, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = -1, Name = "Lavgrupp", IsMainCategory = false, ParentId = -2, SortOrder = -1, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 1, Name = "Rike", IsMainCategory = true, ParentId = 0, SortOrder = 0, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 2, Name = "Stam", IsMainCategory = true, ParentId = 1, SortOrder = 1, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 3, Name = "Understam", IsMainCategory = false, ParentId = 2, SortOrder = 2, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 4, Name = "Överklass", IsMainCategory = false, ParentId = 2, SortOrder = 3, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 5, Name = "Klass", IsMainCategory = true, ParentId = 2, SortOrder = 4, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 6, Name = "Underklass", IsMainCategory = false, ParentId = 5, SortOrder = 5, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 7, Name = "Överordning", IsMainCategory = false, ParentId = 5, SortOrder = 10, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 8, Name = "Ordning", IsMainCategory = true, ParentId = 5, SortOrder = 11, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 9, Name = "Underordning", IsMainCategory = false, ParentId = 8, SortOrder = 12, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 10, Name = "Överfamilj", IsMainCategory = false, ParentId = 8, SortOrder = 15, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 11, Name = "Familj", IsMainCategory = true, ParentId = 8, SortOrder = 16, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 12, Name = "Underfamilj", IsMainCategory = false, ParentId = 11, SortOrder = 17, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 13, Name = "Tribus", IsMainCategory = false, ParentId = 11, SortOrder = 18, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 14, Name = "Släkte", IsMainCategory = true, ParentId = 11, SortOrder = 19, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 15, Name = "Undersläkte", IsMainCategory = false, ParentId = 14, SortOrder = 20, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 16, Name = "Sektion", IsMainCategory = false, ParentId = 14, SortOrder = 23, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 17, Name = "Art", IsMainCategory = true, ParentId = 14, SortOrder = 24, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 18, Name = "Underart", IsMainCategory = false, ParentId = 17, SortOrder = 25, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 19, Name = "Varietet", IsMainCategory = false, ParentId = 17, SortOrder = 26, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 20, Name = "Form", IsMainCategory = false, ParentId = 17, SortOrder = 27, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 21, Name = "Hybrid", IsMainCategory = false, ParentId = 17, SortOrder = 28, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 22, Name = "Kulturvarietet", IsMainCategory = false, ParentId = 17, SortOrder = 29, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 23, Name = "Population", IsMainCategory = false, ParentId = 17, SortOrder = 30, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 24, Name = "Grupp av familjer", IsMainCategory = false, ParentId = 8, SortOrder = 14, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 25, Name = "Infraklass", IsMainCategory = false, ParentId = 5, SortOrder = 6, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 26, Name = "Parvklass", IsMainCategory = false, ParentId = 5, SortOrder = 7, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 27, Name = "Sensu lato", IsMainCategory = false, ParentId = 14, SortOrder = 22, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 28, Name = "Svårbestämt artpar", IsMainCategory = false, ParentId = 14, SortOrder = 21, IsTaxonomic = false });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 29, Name = "Infraordning", IsMainCategory = false, ParentId = 8, SortOrder = 13, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 30, Name = "Avdelning", IsMainCategory = false, ParentId = 5, SortOrder = 8, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 31, Name = "Underavdelning", IsMainCategory = false, ParentId = 5, SortOrder = 9, IsTaxonomic = true });
            taxonCategories.Add(new TaxonCategory() { DataContext = new DataContext(userContext), Id = 32, Name = "Morfotyp", IsMainCategory = false, ParentId = 17, SortOrder = 31, IsTaxonomic = false });            

            return taxonCategories;

            //var s = CoreData.TaxonManager.GetTaxonCategories(CoreData.UserManager.GetCurrentUser());
            //foreach (ITaxonCategory taxonCategory in s)
            //{
            //    Debug.WriteLine(
            //        "taxonCategories.Add(new TaxonCategory(userContext) {{ Id = {0}, Name = \"{1}\", MainCategory = {2}, ParentCategory = {3}, SortOrder = {4}, Taxonomic = {5}}});",
            //        taxonCategory.Id, taxonCategory.Name, taxonCategory.MainCategory, taxonCategory.ParentCategory, taxonCategory.SortOrder,
            //        taxonCategory.Taxonomic);
            //    ;
            //}

        }

        public TaxonCategoryList GetTaxonCategories(IUserContext userContext, ITaxon taxon)
        {
            throw new NotImplementedException();
        }

        public TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext)
        {
            TaxonNameStatusList list = new TaxonNameStatusList();
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "Borttagen", Id = -1 });
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "Godkänd namngivining", Id = 0 });
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "Preliminärt namnförslag", Id = 1 });
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "Ogiltig namngivning", Id = 2 });
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "felstavat", Id = 3 });
            list.Add(new TaxonNameStatus() { DataContext = new DataContext(userContext), Name = "Obsrek", Id = 4 });
            return list;
        }

        public TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext)
        {
            TaxonNameUsageList list = new TaxonNameUsageList();
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "Accepterad", Id = 0 });
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "Synonym", Id = 1 });
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "Homotypisk", Id = 2 });
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "Heterotypisk", Id = 3 });
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "proParte-synonym", Id = 4 });
            list.Add(new TaxonNameUsage() { DataContext = new DataContext(userContext), Name = "Felanvänd (auct.-namn)", Id = 5 });
            return list;
        }

        public TaxonPropertiesList GetTaxonProperties(IUserContext userContext, ITaxon taxon)
        {
            TaxonPropertiesList properties = new TaxonPropertiesList();
            ITaxonCategory taxonCategory = GetReferenceTaxonCategory(userContext, 1);
            ITaxonProperties taxonProperties = new TaxonProperties() { DataContext = new DataContext(userContext), IsValid = true, TaxonCategory = taxonCategory, ValidToDate = new DateTime(2111, 12, 31) };
            taxon.SetTaxonProperties(new List<ITaxonProperties>() { taxonProperties });
            properties.Add(taxonProperties);
            return properties;
          
        }

        public TaxonRelationList GetTaxonRelations(IUserContext userContext, ITaxonRelationSearchCriteria searchCriteria)
        {
            ITaxon parentTaxon = GetReferenceParentTaxon(userContext, 5398);
            
            ITaxon taxon = new Taxon();
            taxon.Id = 3897845;
            taxon.SortOrder = 4;
            taxon.Category = new TaxonCategory(){Id = 2};
          
            ITaxonRelation taxonRel = new TaxonRelation() { ParentTaxon = parentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30), IsMainRelation = true, ReplacedInTaxonRevisionEventId = null, ChildTaxon = taxon, };
               
            TaxonRelationList list = new TaxonRelationList();
            list.Add(taxonRel);
            return list;
        }

        public TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext, ITaxon taxon)
        {
            throw new NotImplementedException();
        }

        public TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext, ITaxon taxon)
        {
            throw new NotImplementedException();
        }

        public ITaxonCategory GetTaxonCategoryById(IUserContext userContext, int taxonCategoryId)
        {
            return GetReferenceTaxonCategory(userContext, taxonCategoryId);
        }

        public TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext)
        {
            this.taxonNameCategoryList = new TaxonNameCategoryList();
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "Test", Id = 0 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "Scientific", Id = 1 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "Svenska", Id = 2 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "Engelska", Id = 3 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "Orginal", Id = 4 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "AnamorensName", Id = 5 });
            taxonNameCategoryList.Add(new TaxonNameCategory() { Name = "PESI", Id = 16 });
            return taxonNameCategoryList;
        }

        public void DeleteTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            throw new NotImplementedException();
        }

        public ITaxonName GetTaxonName(IUserContext userContext, int taxonNameId)
        {
            return GetReferenceTaxonName(userContext, DyntaxaTestSettings.Default.TestTaxonId, taxonNameId);
        }

        public TaxonNameList GetTaxonNames(IUserContext userContext, ITaxonNameSearchCriteria searchCriteria)
        {
            TaxonNameList nameList = new TaxonNameList();
            nameList.Add(GetReferenceTaxonName(userContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId));
            return  nameList;
           
        }

        public TaxonNameList GetTaxonNames(IUserContext userContext, ITaxon taxon)
        {
            TaxonNameList nameList = new TaxonNameList();
            nameList.Add(GetReferenceTaxonName(userContext, taxon.Id, DyntaxaTestSettings.Default.TestTaxonNameId));
            return nameList;
        }

        public List<TaxonNameList> GetTaxonNames(IUserContext userContext, TaxonList taxaIds)
        {
            throw new NotImplementedException();
        }

        public void CheckOutTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            return;
        }

        public void DeleteTaxonRevisionEvent(IUserContext userContext, ITaxonRevisionEvent taxonRevisionEvent, ITaxonRevision taxonRevision)
        {
            throw new NotImplementedException();
        }

        public void CheckInTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            return;
        }

        public ITaxonRevisionEvent GetTaxonRevisionEvent(IUserContext userContext, int taxonRevisionEventId)
        {
            throw new NotImplementedException();
        }

        public TaxonChangeList GetTaxonChange(IUserContext userContext, ITaxon rootTaxon, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }

        public LumpSplitEventList GetLumpSplitEventsByNewReplacingTaxon(IUserContext userContext, ITaxon taxon)
        {
            throw new NotImplementedException();
        }

        public LumpSplitEventList GetLumpSplitEventsByOldReplacedTaxon(IUserContext userContext, ITaxon oldReplacedTaxon)
        {
            throw new NotImplementedException();
        }

        public string GetTaxonConceptDefinition(IUserContext userContext, ITaxon taxon)
        {
            return "This is my concept definition string.";
        }


        /// <summary>
        /// Gets a Revision for test 
        /// </summary>
        /// <returns></returns>
        public ITaxonRevision GetReferenceRevision(IUserContext userContext, int taxonId, string state )
        {
            ITaxonRevisionEvent revEvent = new TaxonRevisionEvent();
            IUser user = new User(userContext);
            ITaxonRevision rev = new TaxonRevision();
            user.Id = userContext.User.Id;
            revEvent.CreatedBy = user.Id;
            revEvent.CreatedDate = DateTime.Now;
            if (state.Equals("Created"))
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 1, Identifier = "" };
                

            }
            else if (state.Equals("Ongoing"))
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 2, Identifier = "" };

            }
            else
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 3, Identifier = "" };

            }
            revEvent.RevisionId = rev.Id;
            List<ITaxonRevisionEvent> revisionEventList = new List<ITaxonRevisionEvent>();
            revisionEventList.Add((TaxonRevisionEvent)revEvent);

            rev.CreatedBy = user.Id;
            rev.CreatedDate = DateTime.Now;
             rev.ExpectedEndDate = new DateTime(2447, 08, 01);
            rev.ExpectedStartDate = DateTime.Now;
            if (state.Equals("Created"))
            {
                rev.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
                rev.Guid = DyntaxaTestSettings.Default.TestRevisionGUID;
                rev.Id = DyntaxaTestSettings.Default.TestRevisionId;
                rev.Description = "My revision no " + DyntaxaTestSettings.Default.TestRevisionId;
           
            

            }
            else if (state.Equals("Ongoing"))
            {
                rev.State = new TaxonRevisionState() { Id = 2, Identifier = TaxonRevisionStateId.Ongoing.ToString() };
                rev.Guid = DyntaxaTestSettings.Default.TestRevisionOngoingGUID;
                rev.Id = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                rev.Description = "My revision no " + DyntaxaTestSettings.Default.TestRevisionOngoingId;
           
            

            }
            else
            {
                rev.State = new TaxonRevisionState() { Id = 3, Identifier = TaxonRevisionStateId.Closed.ToString() };
                rev.Guid = DyntaxaTestSettings.Default.TestRevisionPublishedGUID;
                rev.Id = DyntaxaTestSettings.Default.TestRevisionPublishedId;
                rev.Description = "My revision no " + DyntaxaTestSettings.Default.TestRevisionPublishedId;
            

            }

            rev.SetRevisionEvents(revisionEventList);
            rev.RootTaxon = GetReferenceTaxon(userContext, taxonId); 

            return rev;
        }

        /// <summary>
        /// Gets a RevisionEvent for test TODO: for now this event exist in DB
        /// </summary>
        /// <returns></returns>
        public ITaxonRevisionEvent GetReferenceRevisionEvent(IUserContext userContext, int i = 0)
        {
            ITaxonRevisionEvent revEvent = new TaxonRevisionEvent();
            IUser user = new User(userContext);
            ITaxonRevision rev = new TaxonRevision();
            user.Id = userContext.User.Id;
            revEvent.CreatedBy = user.Id;
            revEvent.CreatedDate = DateTime.Now;
            revEvent.RevisionId = rev.Id;
            return revEvent;
        }

        /// <summary>
        /// Creates a taxon for test
        /// </summary>
        /// <returns></returns>
        public ITaxon GetReferenceTaxon(IUserContext userContext, int taxonId)
        {

            ITaxon refTaxon = new Taxon();

            string conceptDefinitionPartString = "ConceptDefinitionPartString Text";
            DateTime createdDate = new DateTime(2004, 01, 20);
            Int32 createdBy = userContext.User.Id;
            string personName = @"Hölje Soderås";
            DateTime validFromDate = new DateTime(1763, 02, 08);
            DateTime validToDate = new DateTime(2447, 08, 01);

           // refTaxon.ConceptDefinitionFullGeneratedString = conceptDefinitionFullGeneratedString;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPartString;
            refTaxon.CreatedBy = createdBy;
            refTaxon.CreatedDate = createdDate;
            refTaxon.DataContext = new DataContext(userContext);
            refTaxon.ModifiedByPerson = personName;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;            
            refTaxon.Id = taxonId;
            int taxonNameId = DyntaxaTestSettings.Default.TestTaxonNameId;
            //ITaxonName refTaxonName = GetReferenceTaxonName(userContext, taxonId, taxonNameId);
            //ITaxonName refTaxonName2 = GetReferenceTaxonName(userContext, taxonId, taxonNameId +1);
            //refTaxonName2.IsRecommended = false;
            ITaxonCategory taxonCategory = GetReferenceTaxonCategory(userContext, 0);
            ITaxonProperties taxonProperties = new TaxonProperties() { DataContext = new DataContext(userContext), IsValid = true, TaxonCategory = taxonCategory, ValidToDate = new DateTime(2111, 12, 31) };
            refTaxon.SetTaxonProperties(new List<ITaxonProperties>() { taxonProperties });
            refTaxon.Category = taxonCategory;
            //ITaxonName recName = new TaxonName(userContext);
            refTaxon.Author = "ReferenceAuthor";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Author;
            refTaxon.ScientificName = "ReferenceScentificName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Name;
            refTaxon.CommonName = "ReferenceCommonName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId + 1).Name;
            
            ITaxon parentTaxon = GetReferenceParentTaxon(userContext, DyntaxaTestSettings.Default.TestParentTaxonId);
            //TaxonRelationList relationList = refTaxon.GetNearestParentTaxonRelations(userContext);
            //ITaxonRelation taxonRel = new TaxonRelation() { ParentTaxon = parentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30), IsMainRelation = true, ReplacedInTaxonRevisionEventId = null };
            //relationList.Add(taxonRel);
            refTaxon.GetNearestParentTaxonRelations(userContext).Add(new TaxonRelation() { ParentTaxon = parentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30), IsMainRelation = true, ReplacedInTaxonRevisionEventId = null });
            ITaxon taxon = new Taxon();
            taxon.Id = 3897845;
            taxon.SortOrder = 4;
            taxon.Category = new TaxonCategory() { Id = 2 };
            TaxonRelationList parentsList = new TaxonRelationList();
            parentsList.Add(new TaxonRelation() { ParentTaxon = parentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30), IsMainRelation = true, ReplacedInTaxonRevisionEventId = null, ChildTaxon = taxon});
            refTaxon.IsInRevision = true;
            refTaxon.SetParentTaxa(parentsList);
            
            return refTaxon;
        }

        /// <summary>
        /// Creates a taxon for test
        /// </summary>
        /// <returns></returns>
        public ITaxon GetReferenceParentTaxon(IUserContext userContext, int taxonId)
        {
            ITaxon refTaxon = new Taxon();

            string conceptDefinitionPartString = "";
            DateTime createdDate = new DateTime(2004, 01, 20);
            Int32 createdBy = userContext.User.Id;
            string personName = @"Hölje Soderås";
            DateTime validFromDate = new DateTime(1763, 02, 08);
            DateTime validToDate = new DateTime(2447, 08, 01);

           // refTaxon.ConceptDefinitionFullGeneratedString = conceptDefinitionFullGeneratedString;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPartString;
            refTaxon.CreatedBy = createdBy;
            refTaxon.CreatedDate = createdDate;
            refTaxon.DataContext = new DataContext(userContext);
            refTaxon.ModifiedByPerson = personName;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;            
            refTaxon.Id = taxonId;
            int taxonNameId = DyntaxaTestSettings.Default.TestParentTaxonNameId;
           // ITaxonName refTaxonName = GetReferenceTaxonName(userContext, taxonId, taxonNameId);
            ITaxonCategory taxonCategory = GetReferenceTaxonCategory(userContext, 1);
            ITaxonProperties taxonProperties = new TaxonProperties() { IsValid = true, DataContext = new DataContext(userContext), TaxonCategory = taxonCategory, ValidToDate = new DateTime(2111, 12, 31) };
            refTaxon.SetTaxonProperties(new List<ITaxonProperties>() { taxonProperties });
            refTaxon.Category = taxonCategory;
           // ITaxonName recName = new TaxonName(userContext);
            refTaxon.Author = "ReferenceParentAuthor";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Author;
            refTaxon.ScientificName = "ReferenceParentScentificName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Name;
            refTaxon.CommonName = "ReferenceParentCommonName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId + 1).Name;
            //ITaxon grandParentTaxon = GetReferenceGrandParentTaxon(userContext, DyntaxaTestSettings.Default.TestParentTaxonId +10);
            //refTaxon.GetParentTaxa(userContext).Add(new TaxonRelation() { RelatedTaxon = grandParentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30) });
            //List<ITaxonRelation> grandParentsList = new List<ITaxonRelation>();
            //grandParentsList.Add(new TaxonRelation() { RelatedTaxon = grandParentTaxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30) });

            //refTaxon.ParentTaxa = grandParentsList;
            return refTaxon;
        }

        /// <summary>
        /// Creates a taxon for test
        /// </summary>
        /// <returns></returns>
        public ITaxon GetReferenceGrandParentTaxon(IUserContext userContext, int taxonId)
        {
            ITaxon refTaxon = new Taxon();

            string conceptDefinitionPartString = "";
            DateTime createdDate = new DateTime(2004, 01, 20);
            Int32 createdBy = userContext.User.Id;
            string personName = @"Hölje KAos";
            DateTime validFromDate = new DateTime(1763, 02, 08);
            DateTime validToDate = new DateTime(2447, 08, 01);

            // refTaxon.ConceptDefinitionFullGeneratedString = conceptDefinitionFullGeneratedString;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPartString;
            refTaxon.CreatedBy = createdBy;
            refTaxon.CreatedDate = createdDate;
            refTaxon.DataContext = new DataContext(userContext);
            refTaxon.ModifiedByPerson = personName;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;
            refTaxon.Id = taxonId;
             // ITaxonName refTaxonName = GetReferenceTaxonName(userContext, taxonId, taxonNameId);
            ITaxonCategory taxonCategory = GetReferenceTaxonCategory(userContext, 1);
            ITaxonProperties taxonProperties = new TaxonProperties() { IsValid = true, TaxonCategory = taxonCategory, ValidToDate = new DateTime(2111, 12, 31) };
            refTaxon.SetTaxonProperties(new List<ITaxonProperties>() { taxonProperties });
            refTaxon.Category = taxonCategory;
            // ITaxonName recName = new TaxonName(userContext);
            refTaxon.Author = "ReferenceGrandParentAuthor";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Author;
            refTaxon.ScientificName = "ReferenceGrandParentScentificName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId).Name;
            refTaxon.CommonName = "ReferenceGrandParentCommonName";//GetReferenceTaxonName(userContext, taxonId, taxonNameId + 1).Name;

            return refTaxon;
        }


        /// <summary>
        /// Create a taxon category for test.
        /// </summary>
        /// <returns></returns>
        public ITaxonCategory GetReferenceTaxonCategory(IUserContext userContext, int i = 0)
        {
            ITaxonCategory refTaxonCategory = new TaxonCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + i;
            Int32 parentCategory = 2 + i;
            Int32 sortOrder = 20 + i;
            bool mainCategory = true;
            bool taxonomic = true;
            Int32 categoryId = 17 + i;

            refTaxonCategory.Name = categoryName;
            refTaxonCategory.DataContext = new DataContext(userContext);
            refTaxonCategory.Id = categoryId;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic = taxonomic;
            

            return refTaxonCategory;
        }

        /// <summary>
        /// Creates a taxon name
        /// </summary>
        /// <returns></returns>
        public ITaxonName GetReferenceTaxonName(IUserContext userContext, int taxonId, int taxonNameId)
        {
            ITaxonName refTaxonName = new TaxonName();

            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);

            refTaxonName.DataContext = new DataContext(userContext);
            refTaxonName.Taxon = GetReferenceTaxon(userContext, taxonId);
            refTaxonName.Description = "test description";
            refTaxonName.Name = "TestTaxonName" + taxonNameId;
            refTaxonName.Category = new TaxonNameCategory();
            refTaxonName.Category.Id = 1;
            refTaxonName.Category.Type = new TaxonNameCategoryType() { Id = (Int32)TaxonNameCategoryTypeId.ScientificName };
            refTaxonName.Status = new TaxonNameStatus();
            refTaxonName.Status.DataContext = new DataContext(userContext);
            refTaxonName.Status.Id = 1;
            refTaxonName.IsOkForSpeciesObservation = true;
            refTaxonName.IsPublished = false;
            refTaxonName.IsRecommended = true;
            refTaxonName.IsUnique = false;
            refTaxonName.CreatedBy = userContext.User.Id;
            refTaxonName.ModifiedByPerson = "Test PersonName";
            refTaxonName.ValidFromDate = validFromDate;
            refTaxonName.ValidToDate = validToDate;
            refTaxonName.SetChangedInRevisionEvent(new TaxonRevisionEvent());
            refTaxonName.GetChangedInRevisionEvent(userContext).Id = 1;
            refTaxonName.SetReferences(new List<IReferenceRelation>());
            refTaxonName.Id = taxonNameId;
            refTaxonName.Author = @"Jag är författaren";

   

            return refTaxonName;
        }

        /// <summary>
        /// Create a taxon name category for test.
        /// </summary>
        /// <returns></returns>
        public ITaxonNameCategory GetReferenceTaxonNameCategory(IUserContext userContext)
        {
            ITaxonNameCategory refTaxonNameCategory = new TaxonNameCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt";
            string shortName = "Kort kort namn";
            Int32 sortOrder = 20;

            refTaxonNameCategory.Name = categoryName;
            refTaxonNameCategory.ShortName = shortName;
            refTaxonNameCategory.SortOrder = sortOrder;

            return refTaxonNameCategory;
        }

        /// <summary>
        /// Creates a new Revision and make CheckOut
        /// </summary>
        /// <param name="taxon">The taxonId</param>
        /// <returns></returns>
        public TaxonRevision GetRevisionInOngoingState(IUserContext userContext, ITaxon taxon)
        {
            var revision = new TaxonRevision();
            revision.RootTaxon = taxon;
            revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
            revision.ExpectedEndDate = DateTime.Now;
            revision.ExpectedStartDate = DateTime.Now;
            revision.CreatedBy = userContext.User.Id;
            revision.CreatedDate = DateTime.Now;
            revision.SetReferences(new List<IReferenceRelation>());
            revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());
            SaveTaxonRevision(userContext, revision);
            var revisionId = revision.Id;
            CheckOutTaxonRevision(userContext, revision);
            return revision;
        }
    }
}
