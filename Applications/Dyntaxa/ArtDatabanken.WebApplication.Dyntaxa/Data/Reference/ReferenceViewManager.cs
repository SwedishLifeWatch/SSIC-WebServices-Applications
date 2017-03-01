using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Resources;
using ReferenceList = ArtDatabanken.Data.ReferenceList;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    /// <summary>
    /// View manager for references.
    /// </summary>
    public class ReferenceViewManager
    {
        /// <summary>
        /// The user.
        /// </summary>
        private IUserContext user;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceViewManager"/> class.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public ReferenceViewManager(IUserContext user)
        {
            this.user = user;
        }

        /// <summary>
        /// The create reference list view model.
        /// </summary>
        /// <returns>
        /// The <see cref="ReferenceListViewModel"/>.
        /// </returns>
        public ReferenceListViewModel CreateReferenceListViewModel()
        {            
            var model = new ReferenceListViewModel();            
            model.References = new List<ReferenceViewModel>();
            var referenceList = ReferenceHelper.GetReferenceList(user);
            foreach (IReference reference in referenceList)
            {
                model.References.Add(new ReferenceViewModel(reference.Id, reference.Name, reference.Year.GetValueOrDefault(-1), reference.Title));
            }

            return model;
        }

        /// <summary>
        /// Search for a reference.
        /// </summary>
        /// <param name="searchString">
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReferenceViewModel> SearchReference(string searchString)
        {            
            IReferenceSearchCriteria searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.LogicalOperator = LogicalOperator.Or;
            
            int year;
            if (int.TryParse(searchString, out year))
            {
                searchCriteria.Years = new List<int>();
                searchCriteria.Years.Add(year);                
            }

            searchCriteria.NameSearchString = new StringSearchCriteria
            {
                SearchString = searchString, 
                CompareOperators = new List<StringCompareOperator> { StringCompareOperator.Contains }
            };
            searchCriteria.TitleSearchString = new StringSearchCriteria
            {
                SearchString = searchString, 
                CompareOperators = new List<StringCompareOperator> { StringCompareOperator.Contains }
            };
            ReferenceList list = CoreData.ReferenceManager.GetReferences(user, searchCriteria);
            List<ReferenceViewModel> references = new List<ReferenceViewModel>();
            foreach (IReference reference in list)
            {   
                // todo - handle Year null value
                references.Add(new ReferenceViewModel(reference.Id, reference.Name, reference.Year.GetValueOrDefault(), reference.Title));
            }

            return references;
        }

        /// <summary>
        /// The get reference list.
        /// </summary>
        /// <param name="guid">
        /// The guid.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<ReferenceViewModel> GetReferenceList(string guid)
        {
            ReferenceRelationTypeList referenceTypes = CoreData.ReferenceManager.GetReferenceRelationTypes(user);
            ReferenceRelationList references = CoreData.ReferenceManager.GetReferenceRelations(user, guid);
            var list = new List<ReferenceViewModel>();

            foreach (IReferenceRelation referenceRelation in references)
            {
                var reference = referenceRelation.GetReference(user);

                IReferenceRelationType referenceType;
                referenceType = null;
                foreach (IReferenceRelationType tempReferenceRelationType in referenceTypes)
                {
                    if (tempReferenceRelationType.Id == referenceRelation.Type.Id)
                    {
                        referenceType = tempReferenceRelationType;
                    }
                }                

                if (referenceType.IsNull())
                {
                    referenceType = referenceTypes[0];
                }

                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                list.Add(new ReferenceViewModel(reference.Id, reference.Name, year, reference.Title, referenceType.Description, referenceType.Id));
            }

            return list;
        }

        /// <summary>
        /// The create reference info view model.
        /// </summary>
        /// <param name="guid">
        /// The guid.
        /// </param>
        /// <returns>
        /// The <see cref="ReferenceInfoViewModel"/>.
        /// </returns>
        public ReferenceInfoViewModel CreateReferenceInfoViewModel(string guid)
        {            
            ReferenceInfoViewModel model = new ReferenceInfoViewModel();
            model.Guid = guid;
            model.References = GetReferenceList(guid);
            return model;
        }

        /// <summary>
        /// Creates a reference information view model.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>The <see cref="ReferenceInfoViewModel"/>.</returns>
        public ReferenceInfoViewModel CreateReferenceInfoViewModel(IReference reference)
        {
            ReferenceInfoViewModel model = new ReferenceInfoViewModel();            
            model.References = new List<ReferenceViewModel>();
            ReferenceViewModel referenceViewModel = new ReferenceViewModel();
            referenceViewModel.Name = reference.Name;
            referenceViewModel.Year = reference.Year;
            referenceViewModel.Text = reference.Title;
            referenceViewModel.Usage = "";
            model.References.Add(referenceViewModel);
            return model;
        }

        /// <summary>
        /// The create reference add view model.
        /// </summary>
        /// <param name="guid">
        ///     The guid.
        /// </param>
        /// <param name="showReferenceApplyMode"></param>
        /// <returns>
        /// The <see cref="ReferenceAddViewModel"/>.
        /// </returns>
        public ReferenceAddViewModel CreateReferenceAddViewModel(string guid, bool showReferenceApplyMode)
        {            
            var model = new ReferenceAddViewModel();
            model.Guid = guid;
            model.ReferenceTypes = new List<IReferenceRelationType>();
            model.ReferenceTypes.AddRange(CoreData.ReferenceManager.GetReferenceRelationTypes(user));
            model.ReferenceTypesSelectBoxString = CreateReferenceTypesSelectBoxString();
            model.SelectedReferences = GetReferenceList(guid);
            model.ShowReferenceApplyMode = showReferenceApplyMode;
            return model;
        }

        /// <summary>
        /// Creates a html-string for a select box where the user can
        /// select what type of reference this is.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string CreateReferenceTypesSelectBoxString()
        {
            ReferenceRelationTypeList referenceTypes = CoreData.ReferenceManager.GetReferenceRelationTypes(user);
                        
            StringBuilder sb = new StringBuilder();
            sb.Append("<select name=\"usageType\" style=\"width: 100%\">");
            sb.Append("<option value=-1 selected=\"selected\">" + HttpUtility.HtmlEncode(DyntaxaResource.ReferenceAddChooseType) + "</option>");
            foreach (IReferenceRelationType referenceRelationType in referenceTypes)
            {                
                sb.AppendFormat(
                    "<option value=\"{0}\">{1}</option>", 
                    referenceRelationType.Id, 
                    HttpUtility.HtmlEncode(referenceRelationType.Description));                
            }

            sb.Append("</select>");
            return sb.ToString();            
        }

        /// <summary>
        /// Updates the reference relations.
        /// </summary>        
        /// <param name="taxon">The source taxon.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="newReferenceList">The new reference list.</param>
        /// <param name="applyMode">The apply mode.</param>
        public void UpdateReferenceRelations(
            ITaxon taxon, 
            string guid, 
            ReferenceViewModel[] newReferenceList, 
            ReferenceApplyMode applyMode)
        {
            ReferenceRelationList allReferenceRelationItemsToCreate = new ReferenceRelationList();
            ReferenceRelationList allReferenceRelationItemsToDelete = new ReferenceRelationList();
            ReferenceRelationList createReferenceRelationItems;
            ReferenceRelationList deleteReferenceRelationItems;

            // Handle source taxon reference relations
            createReferenceRelationItems = GetReferenceRelationsThatWillBeCreated(guid, newReferenceList, ReferenceApplyMode.OnlySelected);
            deleteReferenceRelationItems = GetReferenceRelationsThatWillBeDeleted(guid, newReferenceList, ReferenceApplyMode.OnlySelected);
            allReferenceRelationItemsToCreate.AddRange(createReferenceRelationItems);
            allReferenceRelationItemsToDelete.AddRange(deleteReferenceRelationItems);

            if (applyMode != ReferenceApplyMode.OnlySelected)
            {
                // Get all child taxa
                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                taxonSearchCriteria.TaxonIds = new List<int>();
                taxonSearchCriteria.TaxonIds.Add(taxon.Id);
                taxonSearchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
                TaxonList taxonList = CoreData.TaxonManager.GetTaxa(user, taxonSearchCriteria);
                taxonList.Remove(taxon);
            
                foreach (ITaxon childTaxon in taxonList)
                {
                    createReferenceRelationItems = GetReferenceRelationsThatWillBeCreated(childTaxon.Guid, newReferenceList, applyMode);
                    deleteReferenceRelationItems = GetReferenceRelationsThatWillBeDeleted(childTaxon.Guid, newReferenceList, applyMode);
                    allReferenceRelationItemsToCreate.AddRange(createReferenceRelationItems);
                    allReferenceRelationItemsToDelete.AddRange(deleteReferenceRelationItems);
                }
            }

            if (allReferenceRelationItemsToCreate.Count > 0 || allReferenceRelationItemsToDelete.Count > 0)
            {
                using (ITransaction transaction = user.StartTransaction())
                {
                    CoreData.ReferenceManager.DeleteReferenceRelations(user, allReferenceRelationItemsToDelete);
                    CoreData.ReferenceManager.CreateReferenceRelations(user, allReferenceRelationItemsToCreate);                    
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Updates the reference relations.
        /// </summary>        
        /// <param name="guid">The unique identifier.</param>
        /// <param name="newReferenceList">The new reference list.</param>
        public void UpdateReferenceRelations(string guid, ReferenceViewModel[] newReferenceList)
        {
            UpdateReferenceRelations(null, guid, newReferenceList, ReferenceApplyMode.OnlySelected);
        }

        /// <summary>
        /// Gets the reference relations that will be deleted.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="newReferences">The new references.</param>
        /// <param name="applyMode">The apply mode.</param>
        /// <returns>A list with all reference relations that will be deleted.</returns>
        private ReferenceRelationList GetReferenceRelationsThatWillBeDeleted(string guid, ReferenceViewModel[] newReferences, ReferenceApplyMode applyMode)
        {
            ReferenceRelationList referenceRelationsToDelete = new ReferenceRelationList();
            IEnumerable<IReferenceRelation> existingReferenceRelations = CoreData.ReferenceManager.GetReferenceRelations(user, guid);
            if (applyMode == ReferenceApplyMode.ReplaceOnlySourceInUnderlyingTaxa)
            {
                existingReferenceRelations = existingReferenceRelations.Where(referenceRelation => referenceRelation.Type.Id == (int)ReferenceRelationTypeId.Source);
            }

            // Don't delete any reference relations in the these modes.
            if (applyMode == ReferenceApplyMode.AddToUnderlyingTaxa)
            {
                return referenceRelationsToDelete;
            }

            foreach (IReferenceRelation existingReferenceRelation in existingReferenceRelations)
            {
                bool found = newReferences.Any(newReference => newReference.Id == existingReferenceRelation.ReferenceId && 
                                                               newReference.UsageTypeId == existingReferenceRelation.Type.Id);
                if (!found)
                {
                    referenceRelationsToDelete.Add(existingReferenceRelation);
                }
            }

            return referenceRelationsToDelete;
        }

        /// <summary>
        /// Gets the reference relations that will be created.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="newReferences">The new references.</param>
        /// <param name="applyMode">The apply mode.</param>
        /// <returns>A list with all reference relations that will be created.</returns>
        private ReferenceRelationList GetReferenceRelationsThatWillBeCreated(string guid, ReferenceViewModel[] newReferences, ReferenceApplyMode applyMode)
        {
            ReferenceRelationList referencesToAdd = new ReferenceRelationList();

            IEnumerable<IReferenceRelation> existingReferenceRelations = CoreData.ReferenceManager.GetReferenceRelations(user, guid);
            if (applyMode == ReferenceApplyMode.ReplaceOnlySourceInUnderlyingTaxa)
            {
                existingReferenceRelations = existingReferenceRelations.Where(referenceRelation => referenceRelation.Type.Id == (int)ReferenceRelationTypeId.Source);
            }

            foreach (ReferenceViewModel newReference in newReferences)
            {
                // If the new reference isn't source and we are in Replace only source in underlying mode, don't add this reference.
                if (applyMode == ReferenceApplyMode.ReplaceOnlySourceInUnderlyingTaxa && newReference.UsageTypeId != (int)ReferenceRelationTypeId.Source)
                {
                    continue;
                }

                bool referenceAlreadyExists = existingReferenceRelations.Any(existingReferenceRelation => newReference.Id == existingReferenceRelation.ReferenceId && newReference.UsageTypeId == existingReferenceRelation.Type.Id);

                if (!referenceAlreadyExists)
                {
                    IReference reference = new ArtDatabanken.Data.Reference();
                    reference.Id = newReference.Id;

                    ReferenceRelation newReferenceRelation = new ReferenceRelation()
                    {
                        RelatedObjectGuid = guid,
                        Type = CoreData.ReferenceManager.GetReferenceRelationType(user, newReference.UsageTypeId),
                        Reference = null,
                        ReferenceId = newReference.Id
                    };
                    referencesToAdd.Add(newReferenceRelation);
                }
            }

            return referencesToAdd;
        }

        /// <summary>
        /// Creates a Guid object view model.
        /// </summary>
        /// <param name="guid">
        /// </param>
        /// <returns>
        /// The <see cref="GuidObjectViewModel"/>.
        /// </returns>
        public GuidObjectViewModel CreateGuidObjectViewModel(string guid)
        {
            var model = new GuidObjectViewModel();
            model.GUID = guid;
            model.TypeDescription = GuidHelper.GetObjectTypeFromGuid(guid).ToString();
            model.Description = GuidHelper.GetObjectDescriptionFromGuid(guid);
            return model;
        }

        /// <summary>
        /// The create new reference view model.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object CreateNewReferenceViewModel()
        {
            var model = new CreateNewReferenceViewModel();
            model.Reference = new ReferenceViewModel();
            return model;
        }

        /// <summary>
        /// The create new reference.
        /// </summary>
        /// <param name="refModel">
        /// The ref model.
        /// </param>
        public void CreateNewReference(ReferenceViewModel refModel)
        {            
            IReference reference = new ArtDatabanken.Data.Reference();
            reference.Id = refModel.Id;
            reference.Name = refModel.Name;
            reference.Year = refModel.Year;
            reference.Title = refModel.Text;
                        
            using (ITransaction transaction = user.StartTransaction(30))
            {
                CoreData.ReferenceManager.CreateReference(user, reference);                               
                transaction.Commit();
            }
        }
    }
}
