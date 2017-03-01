using System;
using System.Collections.Generic;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class is used to handle species fact related information.
    /// This class is used to optimize performance when species facts
    /// are retrieved from taxon attribute service.
    /// No functionality is added and some functionality are removed.
    /// </summary>
    public class RedListSpeciesFactDataSource : SpeciesFactDataSource
    {
        /// <summary>
        /// Convert a WebSpeciesFact instance into
        /// an ISpeciesFact instance.
        /// </summary>
        /// <param name="webSpeciesFact">A WebSpeciesFact instance.</param>
        /// <param name="factors">List of factors.</param>
        /// <param name="individualCategories">List of individual categories.</param>
        /// <param name="periods">List of periods.</param>
        /// <returns>An ISpeciesFact instance.</returns>
        private ISpeciesFact GetSpeciesFact(
            WebSpeciesFact webSpeciesFact,
            FactorList factors,
            IndividualCategoryList individualCategories,
            PeriodList periods)
        {
            IFactor factor;
            IPeriod period;
            ISpeciesFact speciesFact;
            ITaxon host, taxon;

            factor = factors.Get(webSpeciesFact.FactorId);
            if (webSpeciesFact.IsHostSpecified)
            {
                host = new Data.Taxon();
                host.Id = webSpeciesFact.HostId;
            }
            else
            {
                if (factor.IsTaxonomic)
                {
                    host = new Data.Taxon { Id = (int)TaxonId.Life };
                }
                else
                {
                    host = null;
                }
            }

            if (webSpeciesFact.IsPeriodSpecified)
            {
                period = periods.Get(webSpeciesFact.PeriodId);
            }
            else
            {
                period = null;
            }

            taxon = new Data.Taxon();
            taxon.Id = webSpeciesFact.TaxonId;
            speciesFact = new SpeciesFact(
                webSpeciesFact.Id,
                taxon,
                individualCategories.Get(webSpeciesFact.IndividualCategoryId),
                factor,
                host,
                period,
                webSpeciesFact.FieldValue1,
                webSpeciesFact.IsFieldValue1Specified,
                webSpeciesFact.FieldValue2,
                webSpeciesFact.IsFieldValue2Specified,
                webSpeciesFact.FieldValue3,
                webSpeciesFact.IsFieldValue3Specified,
                webSpeciesFact.FieldValue4,
                webSpeciesFact.IsFieldValue4Specified,
                webSpeciesFact.FieldValue5,
                webSpeciesFact.IsFieldValue5Specified,
                null,
                null,
                webSpeciesFact.ModifiedBy,
                webSpeciesFact.ModifiedDate);

            return speciesFact;
        }

        /// <summary>
        /// Convert a list of WebSpeciesFact instances
        /// to a SpeciesFactList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webSpeciesFacts">List of WebSpeciesFact instances.</param>
        /// <returns>List of SpeciesFact instances.</returns>
        protected override SpeciesFactList GetSpeciesFacts(
            IUserContext userContext,
            List<WebSpeciesFact> webSpeciesFacts)
        {
            SpeciesFactList speciesFacts;
            FactorList factors = CoreData.FactorManager.GetFactors(userContext);
            IndividualCategoryList individualCategories = CoreData.FactorManager.GetIndividualCategories(userContext);
            PeriodList periods = CoreData.FactorManager.GetPeriods(userContext);

            speciesFacts = null;
            if (webSpeciesFacts.IsNotEmpty())
            {
                speciesFacts = new SpeciesFactList();
                foreach (WebSpeciesFact webSpeciesFact in webSpeciesFacts)
                {
                    if (webSpeciesFact.QualityId <= (int)SpeciesFactQualityId.Acceptable)
                    {
                        speciesFacts.Add(GetSpeciesFact(webSpeciesFact, factors, individualCategories, periods));
                    }
                    // else: Bad quality, do not use this species fact.
                }
            }

            return speciesFacts;
        }
    }
}
