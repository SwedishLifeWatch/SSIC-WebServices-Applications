using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonReplaceModelManager
    {
        /// <summary>
        /// Creates the model for lumping 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxon"></param>
        /// <param name="revisionId"></param>
        /// <param name="replacingTaxonId"></param>
        /// <param name="lumpTaxonIdList"></param>
        /// <returns></returns>
        public TaxonLumpViewModel GetTaxonLumpViewModel(IUserContext userContext, ITaxon taxon, int revisionId, int? replacingTaxonId, List<int?> lumpTaxonIdList, bool isOkToLump, bool isReloaded)
        {
            TaxonLumpViewModel model = new TaxonLumpViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (revisionId.IsNotNull())
                    {
                        model.RevisionErrorId = revisionId.ToString();
                        model.RevisionId = revisionId.ToString();
                        model.TaxonId = taxon.Id.ToString();
                        model.IsSelectedTaxonSetAsReplacingTaxon = false;
                        model.IsSelectedTaxonAlreadyInLumpList = false;
                        model.IsReplacingTaxonSet = false;
                        model.IsSelectedTaxonChildless = true;
                        model.IsAnyLumpTaxonSet = false;
                        model.IsOkToLump = isOkToLump;
                        model.IsReloaded = isReloaded;
                        model.IsReplacingTaxonValid = true;
                        model.IsLumpTaxonValid = true;
                        model.IsLumpTaxonCategoryValid = true;

                        // 1. first we check selected taxon if it has any children
                        if (taxon.GetNearestChildTaxonRelations(userContext).IsNotEmpty())
                        {
                            model.IsSelectedTaxonChildless = false;
                        }
                        //model.SelectedTaxon = new List<TaxonParentViewModelHelper>();
                        //// Get selected taxon ie taxon selected in GUI
                        //TaxonParentViewModelHelper selectedTaxon = new TaxonParentViewModelHelper();
                        //selectedTaxon.Category = CoreData.TaxonManager.GetTaxonCategoryById(loggedInUser, taxon.Category).Name;
                        //selectedTaxon.CommonName = taxon.RecommendedCommonName.IsNotNull() ? taxon.RecommendedCommonName.Name : string.Empty;
                        //selectedTaxon.ScientificName = taxon.RecommendedScentificName.IsNotNull() ? taxon.RecommendedScentificName.Name : string.Empty;
                        //selectedTaxon.TaxonId = taxon.Id.ToString();
                        //model.SelectedTaxon.Add(selectedTaxon);
                        // we now check if selected taxon exist in list to lump
                        if (lumpTaxonIdList.IsNotNull() && !lumpTaxonIdList.Contains(null) && lumpTaxonIdList.Contains(taxon.Id))
                        {
                            model.IsSelectedTaxonAlreadyInLumpList = true;
                        }

                        //2. Check if replacing taxon is set, if so show it.
                        if (replacingTaxonId.IsNotNull())
                        {
                            model.IsReplacingTaxonSet = true;
                            ITaxon repTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)replacingTaxonId);
                            model.ReplacingTaxon = new List<TaxonParentViewModelHelper>();
                            TaxonParentViewModelHelper replacingTaxon = new TaxonParentViewModelHelper();
                            replacingTaxon.Category = repTaxon.Category.Name;
                            replacingTaxon.CommonName = repTaxon.CommonName.IsNotEmpty() ? repTaxon.CommonName : string.Empty;
                            replacingTaxon.ScientificName = repTaxon.ScientificName.IsNotEmpty() ? repTaxon.ScientificName : string.Empty;
                            replacingTaxon.TaxonId = repTaxon.Id.ToString();
                            model.ReplacingTaxon.Add(replacingTaxon);
                            
                            model.ReplacingTaxonId = repTaxon.Id.ToString();
                            // Now we must check if selected taxon equals replacing taxon
                            if (repTaxon.Id == taxon.Id)
                            {
                                model.IsSelectedTaxonSetAsReplacingTaxon = true;
                            }
                            if (!model.IsOkToLump)
                            {
                                if (repTaxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                                {
                                    model.IsReplacingTaxonValid = false;
                                    model.TaxonErrorName = repTaxon.ScientificName;
                                  //  model.IsOkToLump = true;
                                }
                            }
                        }

                        // 3. Check and populate list of taxon to lump
                        model.LumpTaxonList = new List<TaxonParentViewModelHelper>();
                        if (taxon.IsNotNull())
                        {
                            if (lumpTaxonIdList.IsNotNull() && !lumpTaxonIdList.Contains(null))
                            {
                                var lumpIdList = lumpTaxonIdList.Cast<int>().ToList();
                                TaxonList taxa = CoreData.TaxonManager.GetTaxa(loggedInUser, lumpIdList);
                                if (taxa.IsNotNull() && taxa.Count > 0)
                                {
                                    var category = taxa[0].GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;
                                    foreach (ITaxon lumpTaxon in taxa)
                                    {
                                        var lumpTaxonModel = new TaxonParentViewModelHelper();
                                        lumpTaxonModel.Category = lumpTaxon.Category.Name;
                                        lumpTaxonModel.SortOrder = lumpTaxon.Category.SortOrder;
                                        lumpTaxonModel.CommonName = lumpTaxon.CommonName.IsNotEmpty() ? lumpTaxon.CommonName : string.Empty;
                                        lumpTaxonModel.ScientificName = lumpTaxon.ScientificName.IsNotEmpty() ? lumpTaxon.ScientificName : string.Empty;
                                        lumpTaxonModel.TaxonId = lumpTaxon.Id.ToString();
                                        model.LumpTaxonList.Add(lumpTaxonModel);
                                         // Check why it is not ok to lump
                                        if (!model.IsOkToLump)
                                        {
                                            if (lumpTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != category.Id)
                                            {
                                                model.IsLumpTaxonCategoryValid = false;
                                                model.TaxonErrorName = lumpTaxon.ScientificName;
                                               // model.IsOkToLump = true;
                                            }

                                            if (lumpTaxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                                            {
                                                model.IsLumpTaxonValid = false;
                                                model.TaxonErrorName = lumpTaxon.ScientificName;
                                               // model.IsOkToLump = true;
                                            }
                                        }
                                    }
                                }

                                model.IsAnyLumpTaxonSet = true;
                            }
                        }
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        /// <summary>
        /// Create view model for split
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxon"></param>
        /// <param name="revisionId"></param>
        /// <param name="replaceTaxonIdList"></param>
        /// <param name="splitTaxonId"></param>
        /// <returns></returns>
        public TaxonSplitViewModel GetTaxonSplitViewModel(IUserContext userContext, ITaxon taxon, int revisionId, List<int?> replaceTaxonIdList, int? splitTaxonId, bool isOkToSplit, bool isReloaded)
        {
            TaxonSplitViewModel model = new TaxonSplitViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (revisionId.IsNotNull())
                    {
                        model.RevisionErrorId = revisionId.ToString();
                        model.RevisionId = revisionId.ToString();
                        model.TaxonId = taxon.Id.ToString();
                        model.IsSelectedTaxonSetAsSplitTaxon = false;
                        model.IsSelectedTaxonAlreadyInReplacingList = false;
                        model.IsSelectedTaxonChildless = true;
                        model.IsAnyReplacingTaxonSet = false;
                        model.IsSplitTaxonSet = false;
                        model.IsOkToSplit = isOkToSplit;
                        model.IsReloaded = isReloaded;
                        model.IsSplitTaxonValid = true;
                        model.IsReplacingTaxonValid = true;
                        model.IsReplacingTaxonCategoryValid = true;

                        // 1. first we check selected taxon if it has any children
                        if (taxon.GetNearestChildTaxonRelations(userContext).IsNotEmpty())
                        {
                            model.IsSelectedTaxonChildless = false;
                        }
                        model.SelectedTaxon = new List<TaxonParentViewModelHelper>();
                        // Set selected taxon
                        TaxonParentViewModelHelper selectedTaxon = new TaxonParentViewModelHelper();
                        selectedTaxon.Category = taxon.Category.Name;
                        selectedTaxon.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                        selectedTaxon.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : string.Empty;
                        selectedTaxon.TaxonId = taxon.Id.ToString();
                        model.SelectedTaxon.Add(selectedTaxon);
                        // We now check if selected taxon exist in list of replacing taxa
                        if (replaceTaxonIdList.IsNotNull() && !replaceTaxonIdList.Contains(null) && replaceTaxonIdList.Contains(taxon.Id))
                        {
                            model.IsSelectedTaxonAlreadyInReplacingList = true;
                        }

                        // 2. Check and populate list of replacing taxa
                        model.ReplacingTaxonList = new List<TaxonParentViewModelHelper>();
                        if (taxon.IsNotNull())
                        {
                            if (replaceTaxonIdList.IsNotNull() && !replaceTaxonIdList.Contains(null))
                            {
                                var replaceIdList = replaceTaxonIdList.Cast<int>().ToList();
                                TaxonList taxa = CoreData.TaxonManager.GetTaxa(loggedInUser, replaceIdList);
                                var category = taxa[0].GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;
                                   
                                if (taxa.IsNotNull() && taxa.Count > 0)
                                {
                                    foreach (ITaxon replaceTaxon in taxa)
                                    {
                                        var splitTaxonModel = new TaxonParentViewModelHelper();
                                        splitTaxonModel.Category = replaceTaxon.Category.Name;
                                        splitTaxonModel.SortOrder = replaceTaxon.Category.SortOrder;
                                        splitTaxonModel.CommonName = replaceTaxon.CommonName.IsNotEmpty() ? replaceTaxon.CommonName : string.Empty;
                                        splitTaxonModel.ScientificName = replaceTaxon.ScientificName.IsNotEmpty() ? replaceTaxon.ScientificName : string.Empty;
                                        splitTaxonModel.TaxonId = replaceTaxon.Id.ToString();
                                        model.ReplacingTaxonList.Add(splitTaxonModel);

                                        // Check why it is not ok to split
                                        if (!model.IsOkToSplit)
                                        {
                                            if (replaceTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != category.Id)
                                            {
                                                model.IsReplacingTaxonCategoryValid = false;
                                                model.TaxonErrorName = replaceTaxon.ScientificName;
                                               // model.IsOkToSplit = true;
                                            }

                                            if (replaceTaxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                                            {
                                                model.IsReplacingTaxonValid = false;
                                                model.TaxonErrorName = replaceTaxon.ScientificName;
                                               // model.IsOkToSplit = true;
                                            }
                                        }
                                    }
                                }
                                model.IsAnyReplacingTaxonSet = true;
                            }
                         }
                        
                        //3. Check if split taxon is set, if so show it.
                        if (splitTaxonId.IsNotNull())
                        {
                            ITaxon repTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)splitTaxonId);
                            model.SplitTaxon = new List<TaxonParentViewModelHelper>();
                            TaxonParentViewModelHelper replacingTaxon = new TaxonParentViewModelHelper();
                            replacingTaxon.Category = repTaxon.Category.Name;
                            replacingTaxon.CommonName = repTaxon.CommonName.IsNotEmpty() ? repTaxon.CommonName : string.Empty;
                            replacingTaxon.ScientificName = repTaxon.ScientificName.IsNotEmpty() ? repTaxon.ScientificName : string.Empty;
                            replacingTaxon.TaxonId = repTaxon.Id.ToString();
                            model.SplitTaxon.Add(replacingTaxon);
                            model.IsSplitTaxonSet = true; 
                            model.ReplacingTaxonId = repTaxon.Id.ToString();
                            // Now we must check if selected taxon equals replacing taxon
                            if (repTaxon.Id == taxon.Id)
                            {
                                model.IsSelectedTaxonSetAsSplitTaxon = true;
                            }
                            if (!model.IsOkToSplit)
                            {
                                if (repTaxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                                {
                                    model.IsSplitTaxonValid = false;
                                    model.TaxonErrorName = repTaxon.ScientificName;
                                   // model.IsOkToSplit = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        /// <summary>
        /// Lump taxa
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="replacingTaxon"></param>
        /// <param name="taxonRevision"></param>
        /// <param name="ctrLumpTaxonIdList"></param>
        public void LumpTaxa(IUserContext userContext, int? replacingTaxonId, ITaxonRevision taxonRevision, List<int?> ctrLumpTaxonIdList)
        {
            ITaxon replacingTaxon = null;
            if (replacingTaxonId.IsNotNull())
            {
                replacingTaxon = CoreData.TaxonManager.GetTaxon(userContext, (int)replacingTaxonId);
            }
            TaxonList taxaToLump = new TaxonList();
            foreach (var taxonId in ctrLumpTaxonIdList)
            {
                if (taxonId.IsNotNull())
                {
                    taxaToLump.Add(CoreData.TaxonManager.GetTaxon(userContext, (int)taxonId));
                }
            }

            using (ITransaction transaction = userContext.StartTransaction())
            {
                CoreData.TaxonManager.LumpTaxon(userContext, taxaToLump, replacingTaxon, taxonRevision);

                // Update reference for taxon, set revision reference as Source reference and remove old reference.
                ReferenceRelationList referencesToAdd = ReferenceHelper.GetDefaultReferences(userContext, replacingTaxon, taxonRevision, null);
                ReferenceRelationList referencesToRemove = ReferenceHelper.RemoveTaxonSourceReferences(userContext, taxonRevision, replacingTaxon);
                CoreData.ReferenceManager.CreateDeleteReferenceRelations(userContext, referencesToAdd, referencesToRemove);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Split taxa
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="splitTaxonId"></param>
        /// <param name="taxonRevision"></param>
        /// <param name="ctrReplaceTaxonIdList"></param>
        public void SplitTaxon(IUserContext loggedInUser, int? splitTaxonId, ITaxonRevision taxonRevision, List<int?> ctrReplaceTaxonIdList)
        {
            ITaxon splitTaxon = null;
            if (splitTaxonId.IsNotNull())
            {
                splitTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)splitTaxonId);
            }
            TaxonList replacingTaxa = new TaxonList();
            foreach (var taxonId in ctrReplaceTaxonIdList)
            {
                if (taxonId.IsNotNull())
                {
                    replacingTaxa.Add(CoreData.TaxonManager.GetTaxon(loggedInUser, (int)taxonId));
                }
            }

            using (ITransaction transaction = loggedInUser.StartTransaction())
            {
                CoreData.TaxonManager.SplitTaxon(loggedInUser, splitTaxon, replacingTaxa, taxonRevision);
                transaction.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxonIdList"></param>
        /// <returns></returns>
        public IList<TaxonParentViewModelHelper> GetTaxonList(IUserContext userContext, List<int?> taxonIdList)
        {
            IList<TaxonParentViewModelHelper> list = new List<TaxonParentViewModelHelper>();
            IUserContext loggedInUser = userContext;

            if (taxonIdList.IsNotNull() && !taxonIdList.Contains(null))
            {
                var idList = taxonIdList.Cast<int>().ToList();
                TaxonList taxa = CoreData.TaxonManager.GetTaxa(loggedInUser, idList);
                if (taxa.IsNotNull() && taxa.Count > 0)
                {
                    foreach (ITaxon taxon in taxa)
                    {
                        var taxonModel = new TaxonParentViewModelHelper();
                        taxonModel.Category = taxon.Category.Name;
                        taxonModel.SortOrder = taxon.Category.SortOrder;
                        taxonModel.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                        taxonModel.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : string.Empty;
                        taxonModel.TaxonId = taxon.Id.ToString();
                        list.Add(taxonModel);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        public IList<TaxonParentViewModelHelper> GetTaxon(IUserContext userContext, int? taxonId)
        {
             IUserContext loggedInUser = userContext;
             ITaxon taxon = null;
             IList<TaxonParentViewModelHelper> list = new List<TaxonParentViewModelHelper>();
             var taxonModel = new TaxonParentViewModelHelper();
             if (taxonId.IsNotNull())
             {
                taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)taxonId);

                taxonModel.Category = taxon.Category.Name;
                taxonModel.SortOrder = taxon.Category.SortOrder;
                taxonModel.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                taxonModel.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : string.Empty;
                taxonModel.TaxonId = taxon.Id.ToString();  
                list.Add(taxonModel);
            }
             return list;
        }
    }
}
