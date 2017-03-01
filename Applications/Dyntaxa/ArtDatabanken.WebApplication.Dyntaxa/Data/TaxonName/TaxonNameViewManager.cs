using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName
{
    public class TaxonNameViewManager
    {
        private IUserContext _user;

        public TaxonNameViewManager(IUserContext user)
        {
            this._user = user;
        }

        public TaxonNameDeleteViewModel GetViewModel(ITaxon taxon, int nameId)
        {
            ITaxonName taxonName = taxon.GetTaxonNameByVersion(CoreData.UserManager.GetCurrentUser(), nameId);

            TaxonNameDeleteViewModel model = new TaxonNameDeleteViewModel();
            model.TaxonId = taxon.Id;
            model.Version = taxonName.Version;
            model.Name = taxonName.Name;
            model.Comment = taxonName.Description;
            model.Author = taxonName.Author ?? "";            
            model.Category = taxonName.Category.Name;
            model.NameUsage = taxonName.NameUsage.Name;
            model.NameStatus = taxonName.Status.Name;
            model.IsRecommended = taxonName.IsRecommended;    

            return model;
        }

        public bool CanDeleteName(int taxonId, int nameId)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);            
            ITaxonName taxonName = taxon.GetTaxonNameByVersion(CoreData.UserManager.GetCurrentUser(), nameId);
            if (taxonName != null && taxonName.Category.Id == (Int32)TaxonNameCategoryId.ScientificName && taxonName.IsRecommended)
            {
                return false;
            }

            return true;
        }

        public void DeleteName(int taxonId, ITaxonRevision taxonRevision, int nameId)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);            
            ITaxonName taxonName = taxon.GetTaxonNameByVersion(CoreData.UserManager.GetCurrentUser(), nameId);

            using (ITransaction transaction = userContext.StartTransaction())
            {
                CoreData.TaxonManager.DeleteTaxonName(userContext, taxonRevision, taxonName);
                transaction.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxon"></param>
        /// <returns></returns>
        public TaxonNameDetailsViewModel GetTaxonNameDetailsViewModel(ITaxon taxon)
        {            
            var model = new TaxonNameDetailsViewModel();
            model.TaxonId = taxon.Id;            
            InitTaxonNameDetails(model, taxon);
            return model;
        }

        public TaxonNameDetailsViewModel GetTaxonNameDetailsViewModel(ITaxon taxon, string nameId)
        {
            ITaxonName taxonName = taxon.GetTaxonNameByVersion(_user, Int32.Parse(nameId));            
            var model = TaxonNameDetailsViewModel.Create(_user, taxonName); 
            InitTaxonNameDetails(model, taxon);
            return model;
        }       

        public void InitTaxonNameDetails(TaxonNameDetailsViewModel model, ITaxon taxon)
        {            
            model.CategoryList = new List<TaxonDropDownModelHelper>();
            TaxonNameCategoryList nameList = CoreData.TaxonManager.GetTaxonNameCategories(_user);
            foreach (TaxonNameCategory nameCategory in nameList)
            {
                model.CategoryList.Add(new TaxonDropDownModelHelper(nameCategory.Id, nameCategory.Name));
            }
            model.TaxonNameStatusList = new List<TaxonDropDownModelHelper>();
            TaxonNameStatusList nameStatusList = CoreData.TaxonManager.GetTaxonNameStatuses(_user);
            foreach (ITaxonNameStatus nameStatus in nameStatusList.OrderBy(t => t.SortOrder()))
            {
                model.TaxonNameStatusList.Add(new TaxonDropDownModelHelper(nameStatus.Id, nameStatus.Name));
            }
            model.TaxonNameUsageList = new List<TaxonDropDownModelHelper>();
            TaxonNameUsageList nameUsageList = CoreData.TaxonManager.GetTaxonNameUsages(_user);
            foreach (ITaxonNameUsage nameUsage in nameUsageList)
            {
                model.TaxonNameUsageList.Add(new TaxonDropDownModelHelper(nameUsage.Id, nameUsage.Name));
            }
            model.ExistingNames = new List<TaxonNameDetailsViewModel>();            
            TaxonNameList taxonNames = taxon.GetTaxonNames(_user);
            for (int i = 0; i < taxonNames.Count; i++)
            {
                ITaxonName taxonName = taxonNames[i];
                model.ExistingNames.Add(TaxonNameDetailsViewModel.Create(_user, taxonName));
            }
            model.ExistingNames = (from name in model.ExistingNames orderby name.CategoryName select name).ToList();
            for (int i = 0; i < model.ExistingNames.Count; i++)
            {
                if (model.ExistingNames[i].Id == model.Id)
                {
                    model.ExistingNamesCurrentIndex = i;
                    break;
                }                
            }

            // Check name category if Scentific and recommended it is not possible to change status.
            // Unselcet as recommended first and the it is possible to change name usage
            if (model.IsRecommended && (model.SelectedCategoryId == (Int32)TaxonNameCategoryId.ScientificName))
            {
                model.IsPossibleToChangeUsage = false;
            }
            else
            {
                model.IsPossibleToChangeUsage = true;
            }
        }

        public ITaxonName SaveTaxonNameDetailsChanges(TaxonNameDetailsViewModel model, int taxonId, ITaxonRevision taxonRevision)
        {            
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);
            ITaxonName taxonName = taxon.GetTaxonNameByVersion(_user, Int32.Parse(model.Version));

            taxonName.Name = model.Name;
            taxonName.Author = model.Author;
            taxonName.Category = CoreData.TaxonManager.GetTaxonNameCategory(_user, model.SelectedCategoryId);
            taxonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(_user, model.SelectedTaxonNameStatusId);
            taxonName.NameUsage = CoreData.TaxonManager.GetTaxonNameUsage(_user, model.SelectedTaxonNameUsageId);
            //taxonName.IsRecommended = model.IsRecommended;
            taxonName.Description = model.Comment;
            taxonName.Taxon = CoreData.TaxonManager.GetTaxon(_user, model.TaxonId);
            taxonName.IsOkForSpeciesObservation = !model.IsNotOkForObsSystem;
            taxonName.IsOriginalName = model.IsOriginal;

            // at least one scientific name must be recommended
            if (taxonName.Category.Id == (Int32)TaxonNameCategoryId.ScientificName && taxonName.IsRecommended == false)
            {
                var scientificNames = taxon.GetTaxonNames(_user).Where(name => name.Category.Id == (Int32)TaxonNameCategoryId.ScientificName).ToList();
                bool isAnyRecommended = scientificNames.Any(scientificName => scientificName.IsRecommended);
                if (!isAnyRecommended)
                {
                    taxonName.IsRecommended = true;
                }
            }

            using (ITransaction transaction = _user.StartTransaction())
            {
                CoreData.TaxonManager.UpdateTaxonName(_user, taxonRevision, taxonName);

                transaction.Commit();
            }
            return taxonName;
        }

        public ITaxonName AddTaxonName(TaxonNameDetailsViewModel model, int taxonId, ITaxonRevision taxonRevision)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);

            var taxonName = new ArtDatabanken.Data.TaxonName();
            taxonName.DataContext = new DataContext(_user);
            taxonName.Name = model.Name;
            taxonName.Author = model.Author;
            taxonName.Category = CoreData.TaxonManager.GetTaxonNameCategory(_user, model.SelectedCategoryId);
            taxonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(CoreData.UserManager.GetCurrentUser(), model.SelectedTaxonNameStatusId);
            taxonName.NameUsage = CoreData.TaxonManager.GetTaxonNameUsage(CoreData.UserManager.GetCurrentUser(), model.SelectedTaxonNameUsageId); 
            // Todo Not set in GUI only Changed in list
            taxonName.IsRecommended = model.IsRecommended;
            taxonName.Description = model.Comment;
            // taxonName.Taxon = CoreData.TaxonManager.GetTaxon(_user, model.TaxonId);
            taxonName.IsOkForSpeciesObservation = !model.IsNotOkForObsSystem;
            taxonName.IsOriginalName = model.IsOriginal;
            taxonName.Taxon = taxon;
            
            CoreData.TaxonManager.CreateTaxonName(_user, taxonRevision, taxonName);
            //CoreData.TaxonManager.AddTaxonName(_user, taxon, revision, taxonName);
            return taxonName;
        }

        public TaxonNameInfoViewModel GetInfoViewModel(string guid)
        {
            var model = new TaxonNameInfoViewModel();
            ITaxonName taxonName = CoreData.TaxonManager.GetTaxonName(_user, guid);
            model.GUID = taxonName.Guid;
            model.Comment = taxonName.Description;
            model.Name = taxonName.Name;
            model.NameCategory = taxonName.Category.Name; // DyntaxaCachedSettings.Instance.GetTaxonNameCategoryById(_user, taxonName.NameCategory.)
            model.NameUsage = taxonName.NameUsage.Name;
            model.NameStatus = taxonName.Status.Name;
            model.IsOriginal = taxonName.IsOriginalName;
            model.IsRecommended = taxonName.IsRecommended;
            model.IsOkForSpeciesObservation = taxonName.IsOkForSpeciesObservation;
            model.IsUnique = taxonName.IsUnique;
            model.Modified = taxonName.ModifiedDate.ToShortDateString() + " (" + taxonName.ModifiedByPerson + ")";
            model.Author = taxonName.Author;
            return model;
        }
    }
}
