using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon
{
    /// <summary>
    /// This class is a manager class for the SwedishOccurrenceSummary view
    /// </summary>
    public class SwedishOccurrenceSummaryManager
    {
        private IUserContext _userContext;
        private ITaxonRevision _taxonRevision;

        public SwedishOccurrenceSummaryManager(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            _userContext = userContext;
            _taxonRevision = taxonRevision;
        }

        /// <summary>
        /// Get red list category as descriptive string.
        /// </summary>
        /// <param name="speciesFact">The red list species fact (FactorId.RedlistCategory).</param>
        /// <returns>Red list category as descriptive string.</returns>
        private String GetRedListCategory(ArtDatabanken.Data.SpeciesFact speciesFact)
        {
            if (speciesFact.IsNotNull() && speciesFact.HasField1)
            {
                return speciesFact.Field1.EnumValue.OriginalLabel;
            }
            else
            {
                return null;
            }
        }

        private int? GetRedListCategoryValue(ArtDatabanken.Data.SpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            int? redlistCategoryValue = null;            

            if (speciesFact.HasField1)
            {
                redlistCategoryValue = speciesFact.Field1.EnumValue.KeyInt;                
            }            

            return redlistCategoryValue;
        }

        /// <summary>
        /// Creates a swedish occurrence summary view model.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns></returns>
        public SwedishOccurrenceSummaryViewModel CreateSwedishOccurrenceSummaryViewModel(ITaxon taxon)
        {
            var model = new SwedishOccurrenceSummaryViewModel();
            int? redListValue = null;            

            // Species fact
            try
            {                
                Dictionary<ArtDatabanken.Data.FactorId, ArtDatabanken.Data.SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetCommonDyntaxaSpeciesFacts(this._userContext, taxon);
                if (dicSpeciesFacts.ContainsKey(FactorId.SwedishHistory))
                {
                    model.SwedishHistory = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, ArtDatabanken.Data.FactorId.SwedishHistory);
                    model.SwedishHistoryFact = dicSpeciesFacts[FactorId.SwedishHistory];
                }

                if (dicSpeciesFacts.ContainsKey(FactorId.SwedishOccurrence))
                {
                    //CoreData.SpeciesFactManager.GetSpeciesFact()
                    model.SwedishOccurrence = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, ArtDatabanken.Data.FactorId.SwedishOccurrence);
                    model.SwedishOccurrenceFact = dicSpeciesFacts[FactorId.SwedishOccurrence];
                }
                                
                if (dicSpeciesFacts.ContainsKey(FactorId.RedlistCategory))
                {
                    model.RedListInfo = GetRedListCategory(dicSpeciesFacts[ArtDatabanken.Data.FactorId.RedlistCategory]);
                    redListValue = GetRedListCategoryValue(dicSpeciesFacts[ArtDatabanken.Data.FactorId.RedlistCategory]);
                }

                // If swedish occurrence or swedish history is changed in the current revision, then show those values instead.
                if (DyntaxaHelper.IsInRevision(_userContext, _taxonRevision))
                {
                    DyntaxaInternalTaxonServiceManager internalTaxonServiceManager =
                        new DyntaxaInternalTaxonServiceManager();

                    // Check if Swedish occurrence is stored in Taxon database in this revision.
                    DyntaxaRevisionSpeciesFact swedishOccurrenceRevisionSpeciesFact =
                        internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                            _userContext,
                            (Int32)FactorId.SwedishOccurrence, 
                            taxon.Id, 
                            _taxonRevision.Id);
                    if (swedishOccurrenceRevisionSpeciesFact != null)
                    {
                        SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, _userContext);
                        TaxonModelManager.UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(_userContext, speciesFactModel.SwedishOccurrenceSpeciesFact, swedishOccurrenceRevisionSpeciesFact);
                        model.SwedishOccurrence = speciesFactModel.SwedishOccurrenceSpeciesFact.GetStatusOriginalLabel();
                        model.SwedishOccurrenceFact = speciesFactModel.SwedishOccurrenceSpeciesFact;
                    }

                    // Check if Swedish history is stored in Taxon database in this revision.
                    DyntaxaRevisionSpeciesFact swedishHistoryRevisionSpeciesFact =
                        internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                            _userContext,
                            (Int32)FactorId.SwedishHistory, 
                            taxon.Id, 
                            _taxonRevision.Id);
                    if (swedishHistoryRevisionSpeciesFact != null)
                    {
                        if (swedishHistoryRevisionSpeciesFact.StatusId.HasValue)
                        {
                            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, _userContext);
                            TaxonModelManager.UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(_userContext, speciesFactModel.SwedishHistorySpeciesFact, swedishHistoryRevisionSpeciesFact);
                            model.SwedishHistory = speciesFactModel.SwedishHistorySpeciesFact.GetStatusOriginalLabel();
                            model.SwedishHistoryFact = speciesFactModel.SwedishHistorySpeciesFact;
                        }
                        else // swedish history is deleted in this revision
                        {
                            model.SwedishHistory = null;
                            model.SwedishHistoryFact = null;
                        }                         
                    }
                }
            }
            catch (Exception)
            {
                // the taxon did not exist in Artfakta
            }

            const int noRedListValue = 6;
            if (!string.IsNullOrEmpty(model.RedListInfo) && redListValue.GetValueOrDefault(0) < noRedListValue)
            {
                var linkManager = new LinkManager();
                string url = linkManager.GetUrlToRedlist(taxon.Id.ToString());
                if (url != "")
                {
                    var item = new LinkItem(
                        LinkType.Url, 
                        LinkQuality.ApprovedByExpert,
                        Resources.DyntaxaResource.LinkToSwedishRedlistLabel, 
                        url);
                    model.RedListLink = item;
                }
            }

            return model;
        }
    }
}
