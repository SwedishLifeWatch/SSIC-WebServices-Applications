using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon
{
    /// <summary>
    /// View Manager for Taxon/Move
    /// </summary>
    public class TaxonMoveViewManager
    {
        private readonly IUserContext _user;

        public TaxonMoveViewManager(IUserContext user)
        {
            _user = user;
        }

        public void ReInitializeTaxonMoveViewModel(TaxonMoveViewModel model, ITaxonRevision taxonRevision, bool isOkToMove = true) //, ITaxon taxon, bool isOkToMove = true)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, model.OldParentTaxonId);
            model.HasChildren = true;
            model.IsOkToMove = isOkToMove;

            // Child taxa            
            model.ChildTaxa = new List<RelatedTaxonViewModel>();
            if (taxon.GetNearestChildTaxonRelations(_user).IsNotEmpty())
            {
                foreach (ITaxonRelation taxonRelation in taxon.GetNearestChildTaxonRelations(_user))
                {
                    model.ChildTaxa.Add(new RelatedTaxonViewModel(taxonRelation.ChildTaxon, taxonRelation.ChildTaxon.Category, null));
                }
            }
            if (model.ChildTaxa.Count == 0)
            {
                // TODO cant throw exception
                //throw new Exception("Taxon has no children");
                model.HasChildren = false;
            }

            // Available parents
            model.AvailableParents = new List<RelatedTaxonViewModel>();
            if (model.ChildTaxa.Count != 0)
            {
                ITaxon revisionTaxon = CoreData.TaxonManager.GetTaxon(_user, model.ChildTaxa[0].TaxonId);
                TaxonList list = revisionTaxon.GetTaxaPossibleParents(_user, taxonRevision);
                foreach (ITaxon possibleParentTaxon in list)
                {
                    model.AvailableParents.Add(new RelatedTaxonViewModel(possibleParentTaxon, possibleParentTaxon.Category, null));
                }
            }

            // Taxon categories
            model.TaxonCategories = new List<TaxonCategoryViewModel>();
            // var categories = GetTaxonCategories(taxon);
            // Get all categories within this revision 
            var categories = GetTaxonCategories(taxonRevision.RootTaxon);
            foreach (ITaxonCategory taxonCategory in categories)
            {
                model.TaxonCategories.Add(new TaxonCategoryViewModel(taxonCategory.Id, taxonCategory.Name));
            }
        }

        /// <summary>
        /// Creates a TaxonMove view model
        /// </summary>
        /// <param name="taxon"></param>
        /// <param name="isOkToMove"></param>
        /// <returns></returns>
        public TaxonMoveViewModel CreateTaxonMoveViewModel(ITaxon taxon, ITaxonRevision taxonRevision, bool isOkToMove = true)
        {
            var model = new TaxonMoveViewModel();
            model.TaxonId = taxon.Id;
            model.OldParentTaxonId = taxon.Id;
            model.HasChildren = true;
            model.IsOkToMove = isOkToMove;
           
            // Child taxa            
            model.ChildTaxa = new List<RelatedTaxonViewModel>();
            if (taxon.GetNearestChildTaxonRelations(_user).IsNotEmpty())
            {
                foreach (ITaxonRelation taxonRelation in taxon.GetNearestChildTaxonRelations(_user))
                {
                    model.ChildTaxa.Add(new RelatedTaxonViewModel(taxonRelation.ChildTaxon, taxonRelation.ChildTaxon.Category, null, taxonRelation.IsMainRelation));
                }
            }
            if (model.ChildTaxa.Count == 0)
            {
                // TODO cant throw exception
                //throw new Exception("Taxon has no children");
                model.HasChildren = false;
            }

            // Available parents
            TaxonCategoryList categories2 = new TaxonCategoryList();
            model.AvailableParents = new List<RelatedTaxonViewModel>();
            if (model.ChildTaxa.Count != 0)
            {
                ITaxon revisionTaxon = CoreData.TaxonManager.GetTaxon(_user, model.ChildTaxa[0].TaxonId);
                TaxonList list = revisionTaxon.GetTaxaPossibleParents(_user, taxonRevision);
                foreach (var tx in list)
                {
                    categories2.Merge(tx.Category);
                }
                categories2.Sort(new TaxonCategoryComparer());

                foreach (ITaxon possibleParentTaxon in list)
                {
                    model.AvailableParents.Add(new RelatedTaxonViewModel(possibleParentTaxon, possibleParentTaxon.Category, null));
                }
            }
           
            // Taxon categories
            model.TaxonCategories = new List<TaxonCategoryViewModel>();
            // var categories = GetTaxonCategories(taxon);
            // Get all categories within this revision 
            TaxonCategoryList categories = GetTaxonCategories(taxonRevision.RootTaxon);                        

            foreach (ITaxonCategory taxonCategory in categories2)
            {
                model.TaxonCategories.Add(new TaxonCategoryViewModel(taxonCategory.Id, taxonCategory.Name));
            }

            return model;
        }

        private TaxonCategoryList GetTaxonCategories(ITaxon taxon)
        {
            if (taxon.IsLifeTaxon())
            {
                return CoreData.TaxonManager.GetTaxonCategories(_user);
            }
            else
            {
                return CoreData.TaxonManager.GetTaxonCategories(_user, taxon);
            }            
        }

        /// <summary>
        /// Creates a view model for Taxon Move Change names
        /// </summary>
        /// <param name="oldParentTaxonId"></param>
        /// <param name="newParentTaxonId"></param>
        /// <param name="selectedChildTaxaToMove"></param>
        /// <returns></returns>
        public TaxonMoveChangeNameViewModel CreateTaxonMoveChangeNameViewModel(int oldParentTaxonId, int newParentTaxonId, List<int> selectedChildTaxaToMove)
        {
            TaxonMoveChangeNameViewModel model = new TaxonMoveChangeNameViewModel();

            var newParentTaxon = CoreData.TaxonManager.GetTaxon(_user, newParentTaxonId);
            var oldParentTaxon = CoreData.TaxonManager.GetTaxon(_user, oldParentTaxonId);
            model.SelectedChildTaxaToMove = selectedChildTaxaToMove;
            
            // MovedChildTaxons
            var dicItems = new Dictionary<ITaxon, List<ChangeNameItem>>();
            model.MovedChildTaxons = new List<ChangeNameItem>();
            foreach (int taxonId in selectedChildTaxaToMove)
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);
                dicItems.Add(taxon, new List<ChangeNameItem>());
                ChangeNameItem parentItem = ChangeNameItem.CreateChangeNameItem(taxon, taxon, oldParentTaxon, newParentTaxon, _user);
                model.MovedChildTaxons.Add(parentItem);
                dicItems[taxon].Add(parentItem);
                foreach (var taxonRelation in taxon.GetChildTaxonRelations(_user, true, false))
                {                    
                    ChangeNameItem item = ChangeNameItem.CreateChangeNameItem(taxonRelation.ChildTaxon, taxon, oldParentTaxon, newParentTaxon, _user);
                    model.MovedChildTaxons.Add(item);
                    dicItems[taxon].Add(item);
                }
            }

            model.NewParentTaxonId = newParentTaxon.Id;
            model.OldParentTaxonId = oldParentTaxon.Id;              
            // end constructor

            model.TaxonId = oldParentTaxon.Id;
            model.OldParentDescription = oldParentTaxon.ScientificName;        
            model.NewParentDescription = newParentTaxon.ScientificName;
            model.MovedTaxonsDescription = dicItems.Keys.Select(taxon => taxon.ScientificName).ToList();
         
            return model;
        }

        /// <summary>
        /// Saves the move to db
        /// </summary>
        /// <param name="selectedTaxa"></param>
        /// <param name="oldParentId"></param>
        /// <param name="newParentId"></param>
        /// <param name="taxonRevision"></param>
        public void MoveTaxa(List<int> selectedTaxa, int oldParentId, int newParentId, ITaxonRevision taxonRevision)
        {
            ITaxon oldParent = CoreData.TaxonManager.GetTaxon(_user, oldParentId);
            ITaxon newParent = CoreData.TaxonManager.GetTaxon(_user, newParentId);

            TaxonList taxa = new TaxonList();
            foreach (int taxonId in selectedTaxa)
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);
                taxa.Add(taxon);                
            }

            using (ITransaction transaction = _user.StartTransaction())
            {
                CoreData.TaxonManager.MoveTaxa(_user, taxa, oldParent, newParent, taxonRevision);
                transaction.Commit();
            }
        }  

        /// <summary>
        /// Save name changes to db
        /// </summary>
        /// <param name="nameItems"></param>
        /// <param name="taxonRevision"></param>
        public void SaveNameChanges(List<ChangeNameItem> nameItems, ITaxonRevision taxonRevision)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();            
            
            var taxa = new Dictionary<int, ITaxon>();
            //var dic = new Dictionary<ITaxon, IList<ITaxonName>>();
            TaxonNameList taxonNames = new TaxonNameList();
            foreach (ChangeNameItem changeNameItem in nameItems)
            {
                if (!taxa.ContainsKey(changeNameItem.TaxonId))
                {
                    taxa.Add(changeNameItem.TaxonId, CoreData.TaxonManager.GetTaxon(userContext, changeNameItem.TaxonId));
                }

                ITaxon taxon = taxa[changeNameItem.TaxonId];
                if (changeNameItem.NameId.GetValueOrDefault(-1) == -1) // new name is entered
                {
                    ITaxonName taxonName = taxon.GetScientificName(CoreData.UserManager.GetCurrentUser());
                    if (taxonName == null)
                    {
                        continue;
                    }

                    if (taxonName.Name == changeNameItem.Name && taxonName.Author == changeNameItem.Author)
                    {
                        continue;
                    }

                    // Create new recommended scientific name
                    AddScientificRecommendedTaxonName(changeNameItem.Name, changeNameItem.Author, taxon, taxonRevision);                    
                }
                else // synonym is selected
                {
                    ITaxonName taxonName = taxon.GetTaxonNameByVersion(CoreData.UserManager.GetCurrentUser(), changeNameItem.NameId.Value);
                    if (taxonName == null)
                    {
                        continue;
                    }

                    taxonName.IsRecommended = true;
                    taxonNames.Add(taxonName);                                    
                }
            }

            if (taxonNames.IsNotEmpty())
            {
                using (ITransaction transaction = _user.StartTransaction())
                {
                    CoreData.TaxonManager.UpdateTaxonNames(userContext, taxonRevision, taxonNames);

                    transaction.Commit();
                }
            }
        }

        public void AddScientificRecommendedTaxonName(string name, string author, ITaxon taxon, ITaxonRevision taxonRevision)
        {
            var taxonName = new ArtDatabanken.Data.TaxonName();
            taxonName.DataContext = new DataContext(_user);
            taxonName.Name = name;
            taxonName.Author = author;
            taxonName.Category = CoreData.TaxonManager.GetTaxonNameCategory(_user, (Int32)TaxonNameCategoryId.ScientificName);
            taxonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(_user, TaxonNameStatusId.ApprovedNaming);
            taxonName.NameUsage = CoreData.TaxonManager.GetTaxonNameUsage(_user, TaxonNameUsageId.Accepted);             
            taxonName.IsRecommended = true;
            taxonName.Description = "";
            taxonName.Taxon = taxon;
            taxonName.IsOkForSpeciesObservation = true;
            taxonName.IsOriginalName = false;
            using (ITransaction transaction = _user.StartTransaction())
            {
                CoreData.TaxonManager.CreateTaxonName(_user, taxonRevision, taxonName);
                // Set default reference, set from Revision..
                ReferenceRelationList referencesToAdd = ReferenceHelper.GetDefaultReferences(
                    _user,
                    taxonName,
                    taxonRevision,
                    null);
                var referencesToRemove = new ReferenceRelationList();
                CoreData.ReferenceManager.CreateDeleteReferenceRelations(_user, referencesToAdd, referencesToRemove);
                transaction.Commit();
            }
        }


        /// <summary>
        /// ITaxonCategory comparer.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{ArtDatabanken.Data.ITaxonCategory}" />
        private class TaxonCategoryComparer : IComparer<ITaxonCategory>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
            /// </returns>
            public int Compare(ITaxonCategory x, ITaxonCategory y)
            {
                int sortOrderX = x.SortOrder;
                int sortOrderY = y.SortOrder;
                if (sortOrderX < sortOrderY)
                {
                    return -1;
                }
                if (sortOrderX == sortOrderY)
                {
                    return 0;
                }
                
                return 1;
            }
        }
    }
}
