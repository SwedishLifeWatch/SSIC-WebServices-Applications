using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName
{
    /// <summary>
    /// View model for adding or editing a taxon name
    /// </summary>
    public class TaxonNameDetailsViewModel
    {
        public int TaxonId { get; set; }

        public string TaxonNameGuid { get; set; }        

        [LocalizedDisplayName("TaxonNameAddEditNameLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        [Required]
        public string Name { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditAuthorLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string Author { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditNameCategoryLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public List<TaxonDropDownModelHelper> CategoryList { get; set; }

        //[Required]
        [LocalizedDisplayName("TaxonNameAddEditNameCategoryLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SelectedCategoryId { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditUsageLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public List<TaxonDropDownModelHelper> TaxonNameStatusList { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditNameUsageLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public List<TaxonDropDownModelHelper> TaxonNameUsageList { get; set; }

        //[Required]
        [LocalizedDisplayName("TaxonNameAddEditUsageLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SelectedTaxonNameStatusId { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditNameUsageLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SelectedTaxonNameUsageId { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditIsRecommendedLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool IsRecommended { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditIsOriginal", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool IsOriginal { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditNotOkForObs", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool IsNotOkForObsSystem { get; set; }

        [LocalizedDisplayName("TaxonNameAddEditCommentLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string Comment { get; set; }

        /// <summary>
        /// List of all taxonName referenses
        /// </summary>
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public IList<int> TaxonNameReferencesList { get; set; }

        /// <summary>
        /// Indication of number of taxon name references must be at least one.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedPickReferenceText")]
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int NoOfTaxonNameReferences { get; set; }

        public string Version { get; set; }

        public string CategoryName { get; set; }

        public string NameStatusText { get; set; }

        public string NameUsageText { get; set; }

        // Indicates if it´s possible to change usage
        public bool IsPossibleToChangeUsage { get; set; }

        public List<TaxonNameDetailsViewModel> ExistingNames { get; set; }

        public int? ExistingNamesCurrentIndex { get; set; }

        public int Id { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public static TaxonNameDetailsViewModel Create(IUserContext userContext, ITaxonName taxonName)
        {            
            var model = new TaxonNameDetailsViewModel();
            model.TaxonId = taxonName.Taxon.Id;
            if (taxonName.Id == 0)
            {
                model.Id = taxonName.Version;
            }
            else
            {
                model.Id = taxonName.Id;
            }
            model.Name = taxonName.Name;
            model.Comment = taxonName.Description;
            model.Author = taxonName.Author;
            model.SelectedCategoryId = taxonName.Category.Id;
            model.SelectedTaxonNameStatusId = taxonName.Status.Id;
            model.SelectedTaxonNameUsageId = taxonName.NameUsage.Id;
            model.NameStatusText = taxonName.Status.Name;
            model.NameUsageText = taxonName.NameUsage.Name;
            model.IsRecommended = taxonName.IsRecommended;
            model.IsNotOkForObsSystem = !taxonName.IsOkForSpeciesObservation;
            model.Version = taxonName.Version.ToString();
            model.IsOriginal = taxonName.IsOriginalName;
            model.CategoryName = taxonName.Category.Name;
            model.LastUpdated = taxonName.ModifiedDate;

            //// Set "modified by user". CoreData.UserManager.GetUser is a slow operation, so we store each entry in a dictionary.
            //IUser modifiedByUser;
            //if (dicLoadedUsers == null)
            //{
            //    dicLoadedUsers = new Dictionary<int, IUser>();
            //}
            //if (!dicLoadedUsers.ContainsKey(taxonName.ModifiedBy))
            //{
            //    modifiedByUser = CoreData.UserManager.GetUser(CoreData.UserManager.GetCurrentUser(), taxonName.ModifiedBy);
            //    dicLoadedUsers.Add(taxonName.ModifiedBy, modifiedByUser);
            //}
            //else
            //{
            //    modifiedByUser = dicLoadedUsers[taxonName.ModifiedBy];
            //}
            model.UpdatedBy = taxonName.ModifiedByPerson;

            if (taxonName.Guid.IsNotNull())
            {
                model.TaxonNameGuid = taxonName.Guid;
            }

            model.TaxonNameReferencesList = new List<int>();
            model.NoOfTaxonNameReferences = 0;
            if (taxonName.GetReferences(userContext).IsNotEmpty())
            {
                foreach (ReferenceRelation referenceRelation in taxonName.GetReferences(userContext))
                {
                    model.NoOfTaxonNameReferences++;
                }
            }
            
            return model;            
        }
    }
}
