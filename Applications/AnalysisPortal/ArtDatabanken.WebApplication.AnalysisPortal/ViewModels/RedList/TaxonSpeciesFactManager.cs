using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

using ArtDatabanken;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class is used to present information about a taxon.
    /// </summary>
    public class TaxonSpeciesFactManager //: ITaxonSpeciesFactManager
    {
        /// <summary>
        /// Information about the user.
        /// </summary>
        private readonly IUserContext mUserContext;

        /// <summary>
        /// Taxon.
        /// </summary>
        private ITaxon mTaxon;

        /// <summary>
        /// Taxon species fact view model.
        /// </summary>
        private TaxonSpeciesFactViewModel mTaxonSpeciesFact;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSpeciesFactManager" /> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public TaxonSpeciesFactManager(IUserContext userContext)
        {
            mUserContext = userContext;
        }

        /// <summary>
        /// Retrieves information about the specified taxon.
        /// </summary>
        /// <param name="taxonId">Id of the taxon.</param>
        /// <returns>Information about the specified taxon.</returns>
        public TaxonSpeciesFactViewModel GetTaxonSpeciesFact(int taxonId)
        {
            mTaxon = CoreData.TaxonManager.GetTaxon(mUserContext, taxonId);
            mTaxonSpeciesFact = new TaxonSpeciesFactViewModel(mUserContext, mTaxon);

            IIndividualCategory individualCategory = CoreData.FactorManager.GetDefaultIndividualCategory(mUserContext);
            IPeriod period = CoreData.FactorManager.GetCurrentRedListPeriod(mUserContext);
            PeriodList periods = CoreData.FactorManager.GetPublicPeriods(mUserContext);
            IFactor categoryFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.RedlistCategory);
            IFactor criteriaDocumentationFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.RedlistCriteriaDocumentation);
            IFactor criteriaFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.RedlistCriteriaString);
            IFactor globalCategoryFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.GlobalRedlistCategory);
            //FactorList landscapeTypesFactors = LandscapeTypeCache.GetFactors(mUserContext);
            //FactorList lifeFormFactors = LifeFormCache.GetFactors(mUserContext);
            FactorList conventionFactors = CoreData.FactorManager.GetFactorTree(mUserContext, FactorId.Conventions).GetAllLeafFactors();
            IFactor organismGroup1Factor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.Redlist_OrganismLabel1);
            IFactor organismGroup2Factor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.Redlist_OrganismLabel2);
            //FactorList countyOccurrenceFactors = CountyOccurrenceCache.GetFactors(mUserContext);
            //FactorList biotopeFactors = BiotopeCache.GetFactors(mUserContext);
            //FactorList substrateFactors = SubstrateCache.GetFactors(mUserContext);
            //FactorList impactFactors = ImpactCache.GetFactors(mUserContext);
            IFactor actionPlanFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.ActionPlan);
            IFactor protectedByLawFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.ProtectedByLaw);
            IFactor swedishOccurrenceFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SwedishOccurrence);
            //FactorList hostFactors = HostCache.GetFactors(mUserContext);

            // Get species facts.
            var countyOccurrenceSpeciesFacts = new SpeciesFactList();
            var landscapeTypeSpeciesFacts = new SpeciesFactList();
            var biotopeSpeciesFacts = new SpeciesFactList();
            var lifeFormSpeciesFacts = new SpeciesFactList();
            var conventionSpeciesFacts = new SpeciesFactList();
            var previouslyCategorySpeciesFact = new SpeciesFactList();
            ISpeciesFact categorySpeciesFact = null;
            ISpeciesFact criteriaDocumentationSpeciesFact = null;
            ISpeciesFact criteriaSpeciesFact = null;
            ISpeciesFact globalCategorySpeciesFact = null;
            ISpeciesFact organismGroup1SpeciesFact = null;
            ISpeciesFact organismGroup2SpeciesFact = null;
            var substrateSpeciesFacts = new SpeciesFactList();
            var impactSpeciesFacts = new SpeciesFactList();
            ISpeciesFact actionPlanSpeciesFact = null;
            ISpeciesFact protectedByLawSpeciesFact = null;
            ISpeciesFact swedishOccurrenceSpeciesFact = null;
            var hostSpeciesFacts = new SpeciesFactList();
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria { Factors = new FactorList() };

            searchCriteria.AddTaxon(mTaxon);
            searchCriteria.Add(individualCategory);
            foreach (IPeriod tempPeriod in periods)
            {
                searchCriteria.Add(tempPeriod);
            }

            // Add current period if not exist.
            bool currentPeriodExist = periods.Any(tempPeriod => tempPeriod.Id == period.Id);
            if (!currentPeriodExist)
            {
                searchCriteria.Add(period);
            }

            searchCriteria.Add(categoryFactor);
            searchCriteria.Add(criteriaDocumentationFactor);
            searchCriteria.Add(criteriaFactor);
            searchCriteria.Add(globalCategoryFactor);
            //searchCriteria.Factors.AddRange(landscapeTypesFactors);
            //searchCriteria.Factors.AddRange(lifeFormFactors);
            searchCriteria.Factors.AddRange(conventionFactors);
            searchCriteria.Add(organismGroup1Factor);
            searchCriteria.Add(organismGroup2Factor);
            //searchCriteria.Factors.AddRange(countyOccurrenceFactors);
            //searchCriteria.Factors.AddRange(biotopeFactors);
            //searchCriteria.Factors.AddRange(substrateFactors);
            //searchCriteria.Factors.AddRange(impactFactors);
            searchCriteria.Add(actionPlanFactor);
            searchCriteria.Add(protectedByLawFactor);
            searchCriteria.Add(swedishOccurrenceFactor);
            //searchCriteria.Factors.AddRange(hostFactors);
            SpeciesFactList speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(mUserContext, searchCriteria);

            // Split species facts into information groups.
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    //if (countyOccurrenceFactors.Exists(speciesFact.Factor))
                    //{
                    //    countyOccurrenceSpeciesFacts.Add(speciesFact);
                    //}
                    //else if (landscapeTypesFactors.Exists(speciesFact.Factor))
                    //{
                    //    landscapeTypeSpeciesFacts.Add(speciesFact);
                    //}
                    //else if (biotopeFactors.Exists(speciesFact.Factor))
                    //{
                    //    biotopeSpeciesFacts.Add(speciesFact);
                    //}
                    //else if (substrateFactors.Exists(speciesFact.Factor))
                    //{
                    //    substrateSpeciesFacts.Add(speciesFact);
                    //    if (hostFactors.Exists(speciesFact.Factor))
                    //    {
                    //        hostSpeciesFacts.Add(speciesFact);
                    //    }
                    //}
                    //else if (impactFactors.Exists(speciesFact.Factor))
                    //{
                    //    impactSpeciesFacts.Add(speciesFact);
                    //}
                    //else if (lifeFormFactors.Exists(speciesFact.Factor))
                    //{
                    //    lifeFormSpeciesFacts.Add(speciesFact);
                    //}
                    if (conventionFactors.Exists(speciesFact.Factor))
                    {
                        conventionSpeciesFacts.Add(speciesFact);
                    }
                    else if (speciesFact.Factor.Id == (int)FactorId.RedlistCategory && speciesFact.Period.Id != period.Id)
                    {
                        previouslyCategorySpeciesFact.Add(speciesFact);
                    }
                    else
                    {
                        switch (speciesFact.Factor.Id)
                        {
                            case (int)FactorId.ActionPlan:
                                actionPlanSpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.GlobalRedlistCategory:
                                globalCategorySpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.ProtectedByLaw:
                                protectedByLawSpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.RedlistCategory:
                                categorySpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.RedlistCriteriaDocumentation:
                                criteriaDocumentationSpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.RedlistCriteriaString:
                                criteriaSpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.Redlist_OrganismLabel1:
                                organismGroup1SpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.Redlist_OrganismLabel2:
                                organismGroup2SpeciesFact = speciesFact;
                                break;
                            case (int)FactorId.SwedishOccurrence:
                                swedishOccurrenceSpeciesFact = speciesFact;
                                break;
                        }
                    }
                }
            }

            //mTaxonSpeciesFact.InitCountyOccurrenceInformation(countyOccurrenceSpeciesFacts);
            //mTaxonSpeciesFact.InitLandscapeTypeInformation(landscapeTypeSpeciesFacts);
            //mTaxonSpeciesFact.InitBiotopeInformation(biotopeSpeciesFacts);
            //mTaxonSpeciesFact.InitLifeFormInformation(lifeFormSpeciesFacts);
            //mTaxonSpeciesFact.InitConventionInformation(conventionSpeciesFacts);
            InitRedListInformation(
                mTaxonSpeciesFact, 
                categorySpeciesFact,
                criteriaSpeciesFact,
                criteriaDocumentationSpeciesFact,
                globalCategorySpeciesFact,
                period);
            //mTaxonSpeciesFact.InitOrganismGroupInformation(organismGroup1SpeciesFact,
            //                                              organismGroup2SpeciesFact);
            //mTaxonSpeciesFact.InitSubstrateInformation(substrateSpeciesFacts);
            //mTaxonSpeciesFact.InitImpactInformation(impactSpeciesFacts);
            //mTaxonSpeciesFact.InitActionPlanInformation(actionPlanSpeciesFact);
            //mTaxonSpeciesFact.InitProtectedByLawInformation(protectedByLawSpeciesFact);
            //mTaxonSpeciesFact.InitHostInformation(hostSpeciesFacts, mUserContext);
            //mTaxonSpeciesFact.InitSwedishOccurrenceInformation(swedishOccurrenceSpeciesFact);
            //mTaxonSpeciesFact.InitPreviouslyRedListedCategories(previouslyCategorySpeciesFact);
            //GetSpeciesInformationDocument();
            //long pictureId;
            //mTaxonSpeciesFact.HasImage = !string.IsNullOrWhiteSpace(GetPictureByTaxon(mTaxon,
            //                                                       AppSettings.Default.PictureXtraLargeHeight,
            //                                                       AppSettings.Default.PictureXtraLargeWidth,
            //                                                       AppSettings.Default.PictureLargeSize,
            //                                                       true,
            //                                                       string.Empty,
            //                                                       out pictureId));
            //mTaxonSpeciesFact.ImageMetaData = GetPictureMetadataByPictureId(pictureId);
            //mTaxonSpeciesFact.HasCountyMap = GetCountyMap(mTaxon) != null;
            //mTaxonSpeciesFact.HasObservationMap = SpeciesFilter.IsTaxaSpeciesOrBelow(mTaxon) &&
            //    SpeciesFactCacheManager.Instance.GetObservationMap(mTaxon.Id.ToString(CultureInfo.InvariantCulture)) != null;

            mTaxonSpeciesFact.HasSpeciesFacts = HasSpeciesFacts(speciesFacts);
            mTaxonSpeciesFact.IsValid = CheckIfTaxonValid();

            //mTaxonSpeciesFact.Synonyms = mTaxon.GetSynonymsViewModel(mUserContext);

            // Identify higher level taxa
            //mTaxonSpeciesFact.CategoryNameHighLevel = SpeciesFilter.IsTaxaSpeciesOrBelow(mTaxon) ? "" : mTaxon.Category.Name + " - ";

            //// Ignore if not higher taxa
            //if ((mTaxonSpeciesFact.IsHigherTaxa = !SpeciesFilter.IsTaxaSpeciesOrBelow(mTaxon)) == false)
            //{
            //    return mTaxonSpeciesFact;
            //}

            // Count all occurences for each category in underlying taxa
            //var taxonNameSearchInformation = TaxonNameSearchManager.Instance.GetInformation(this.mTaxon.Id);
            //foreach (var category in RedListedHelper.GetAllRedListCategories())
            //{
            //    mTaxonSpeciesFact.RedListCategoryTaxa[category] = new List<int>();
            //}

            //foreach (var taxonInfoType in taxonNameSearchInformation.AllChildTaxaIdsInScope)
            //{
            //    if (SpeciesFilter.IsTaxaSpeciesOrBelow(taxonInfoType.CategoryId, taxonInfoType.ParentCategoryId) && taxonInfoType.HasValidRedListCategory)
            //    {
            //        mTaxonSpeciesFact.RedListCategoryTaxa[taxonInfoType.RedListCategoryId].Add(taxonInfoType.TaxonId);
            //    }
            //}

            return mTaxonSpeciesFact;
        }

        /// <summary>
        /// Init red list information.
        /// </summary>
        /// <param name="taxonSpeciesFact">Taxon species fact view model.</param>
        /// <param name="categorySpeciesFact">Red list category species fact.</param>
        /// <param name="criteriaSpeciesFact">Red list criteria species fact.</param>
        /// <param name="criteriaDocumentationSpeciesFact">Red list criteria documentation species fact.</param>
        /// <param name="globalCategorySpeciesFact">Global red list category species fact.</param>
        /// <param name="period">Red list period.</param>
        public static void InitRedListInformation(
            TaxonSpeciesFactViewModel taxonSpeciesFact,
            ISpeciesFact categorySpeciesFact,
            ISpeciesFact criteriaSpeciesFact,
            ISpeciesFact criteriaDocumentationSpeciesFact,
            ISpeciesFact globalCategorySpeciesFact,
            IPeriod period)
        {
            if (categorySpeciesFact.IsRedlistCategorySpecified())
            {
                taxonSpeciesFact.RedListCategory = categorySpeciesFact.Field1.EnumValue.OriginalLabel.Substring(0, categorySpeciesFact.Field1.EnumValue.OriginalLabel.Length - 4) + "(" + categorySpeciesFact.MainField.StringValue + ")";
                taxonSpeciesFact.IsRedListed = categorySpeciesFact.Field1.EnumValue.KeyInt >= (int)RedListCategory.DD && categorySpeciesFact.Field1.EnumValue.KeyInt < (int)RedListCategory.LC;
                taxonSpeciesFact.IsRedListCriteriaAvailable = (categorySpeciesFact.Field1.EnumValue.KeyInt >= (int)RedListCategory.CR) && (categorySpeciesFact.Field1.EnumValue.KeyInt <= (int)RedListCategory.VU);
            }

            if (taxonSpeciesFact.IsRedListCriteriaAvailable && criteriaSpeciesFact.IsRedlistCriteriaSpecified())
            {
                taxonSpeciesFact.RedListCriteria = criteriaSpeciesFact.MainField.StringValue;
            }

            if (globalCategorySpeciesFact.IsGlobalRedlistCategorySpecified())
            {
                taxonSpeciesFact.GlobalRedListCategory = globalCategorySpeciesFact.MainField.StringValue;
            }

            if (criteriaDocumentationSpeciesFact.IsRedlistDocumentationSpecified())
            {
                taxonSpeciesFact.RedListDocumentationQuality = criteriaDocumentationSpeciesFact.Quality.Id;
                taxonSpeciesFact.RedListDocumentationText = criteriaDocumentationSpeciesFact.MainField.StringValue;
            }

            taxonSpeciesFact.Period = period.Year;
        }

        ///// <summary>
        ///// Gets a picture from the pictureservice by using the pictureid
        ///// </summary>
        ///// <param name="pictureId"></param>
        ///// <param name="pictureHeight">Height of returned picture.</param>
        ///// <param name="pictureWidth">Width of returned picture.</param>
        ///// <param name="requestedPictureSize">Requested size of returned picture.</param>
        ///// <param name="isRequestedPictureSizeSpecified">Is requested size of returned picture specified.</param>
        ///// <param name="requestedFormat">Requested format of returned picture.</param>
        ///// <returns></returns>
        //public string GetPictureByPictureId(int pictureId,
        //    int pictureHeight,
        //    int pictureWidth,
        //    long requestedPictureSize,
        //    bool isRequestedPictureSizeSpecified,
        //    string requestedFormat)
        //{
        //    IPicture picture;
        //    try
        //    {
        //        picture = CoreData.PictureManager.GetPicture(mUserContext,
        //                    pictureId,
        //                    pictureHeight,
        //                    pictureWidth,
        //                    requestedPictureSize,
        //                    isRequestedPictureSizeSpecified,
        //                    requestedFormat);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TaxonSpeciesFactManagerException(RedListResource.FailedToGetPictureFromPictureService, ex);
        //    }

        //    return picture == null ? null : picture.Image;
        //}

        ///// <summary>
        ///// Gets a picture by the given taxon id.
        ///// </summary>
        ///// <param name="taxon">The taxon to use to retrieve a recommended picture.</param>
        ///// <returns>The picture object.</returns>
        //public string GetPictureByTaxon(ITaxon taxon)
        //{
        //    long pictureId;
        //    return GetPictureByTaxon(taxon,
        //                             AppSettings.Default.PictureXtraLargeHeight,
        //                             AppSettings.Default.PictureXtraLargeWidth,
        //                             0,
        //                             false,
        //                             string.Empty,
        //                             out pictureId);
        //}

        /// <summary>
        /// Get picture by taxon and requested picture related properties.
        /// </summary>
        /// <param name="taxon">Taxon that expects to be related to picture.</param>
        /// <param name="pictureHeight">Height of returned picture.</param>
        /// <param name="pictureWidth">Width of returned picture.</param>
        /// <param name="requestedPictureSize">Requested size of returned picture.</param>
        /// <param name="isRequestedPictureSizeSpecified">Is requested size of returned picture specified.</param>
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <param name="pictureId">Id for uploded image. Will also be returned from this method.</param>
        /// <returns>Picture as binary string.</returns>
        public string GetPictureByTaxon(
            ITaxon taxon,
            int pictureHeight,
            int pictureWidth,
            long requestedPictureSize,
            bool isRequestedPictureSizeSpecified,
            string requestedFormat,
            out long pictureId)
        {
            IPictureRelationType pictureRelationType = CoreData.PictureManager.GetPictureRelationType(mUserContext, PictureRelationTypeIdentifier.TaxonRedList);
            PictureRelationList pictureRelations = CoreData.PictureManager.GetPictureRelations(mUserContext, taxon, pictureRelationType);
            pictureId = 0;
            if (pictureRelations.IsNotNull())
            {
                IPictureRelation pictureRelation = pictureRelations.FirstOrDefault(r => r.IsRecommended);

                if (pictureRelation.IsNotNull())
                {
                    if (pictureRelation != null)
                    {
                        IPicture taxonPicture = CoreData.PictureManager.GetPicture(
                            mUserContext,
                            pictureRelation.PictureId,
                            pictureHeight,
                            pictureWidth,
                            requestedPictureSize,
                            isRequestedPictureSizeSpecified,
                            requestedFormat);

                        if (taxonPicture.IsNotNull())
                        {
                            pictureId = taxonPicture.Id;
                        }
                        return taxonPicture.Image;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get picture by taxon and requested picture related properties.
        /// </summary>
        /// <param name="pictureId">Taxon that expects to be related to picture.</param>
        /// <returns>String of metadata</returns>
        public string GetPictureMetadataByPictureId(long pictureId)
        {
            var metadataIds = new List<int>();
            var pictureText = new StringBuilder();

            if (pictureId > 0)
            {
                PictureMetaDataList metadatList = CoreData.PictureManager.GetPictureMetaData(mUserContext, pictureId, metadataIds);

                if (metadatList.IsNotNull())
                {
                    foreach (IPictureMetaData metaData in metadatList)
                    {
                        if (metaData.Id == (int)PictureMetaDataDescriptionId.Copyright)
                        {
                            // This is the only field used in redlist application and it is the Photogafer.
                            pictureText.Append(metaData.Value);
                        }
                    }
                }
                return pictureText.ToString();
            }

            return string.Empty;
        }

        ///// <summary>
        ///// Gets a list of metadata for a specific picture 
        ///// </summary>
        ///// <param name="pictureId"></param>
        ///// <param name="metaDataIds">A list of meta data ids</param>
        ///// <returns></returns>
        //public PictureMetaDataList GetPictureMetaDataListByPictureId(long pictureId, List<int> metaDataIds)
        //{
        //    if (pictureId <= 0)
        //    {
        //        throw new TaxonSpeciesFactManagerException();
        //    }

        //    try
        //    {
        //        return CoreData.PictureManager.GetPictureMetaData(mUserContext, pictureId, metaDataIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TaxonSpeciesFactManagerException(RedListResource.FailedToGetPictureMetaDataFromPictureManager, ex);
        //    }
        //}

        /// <summary>
        /// Gets the picture relations from a a taxon and a given relation type
        /// </summary>
        /// <param name="taxon"></param>
        /// <returns></returns>
        public PictureRelationList GetPictureRelations(ITaxon taxon)
        {
            IPictureRelationType pictureRelationType = CoreData.PictureManager.GetPictureRelationType(mUserContext, PictureRelationTypeIdentifier.TaxonRedList);
            return CoreData.PictureManager.GetPictureRelations(mUserContext, taxon, pictureRelationType);
        }

        /// <summary>
        /// Checs whether or not SpeciesFacts exist.
        /// </summary>
        /// <param name="speciesFactList">
        /// SpeciesFact lists.
        /// </param>
        public bool HasSpeciesFacts(SpeciesFactList speciesFactList)
        {
            return speciesFactList.IsNotEmpty();
        }

        /// <summary>
        /// Indicates whether or not the taxon is valid.
        /// </summary>
        public bool CheckIfTaxonValid()
        {
            return mTaxon.IsValid;
        }

        ///// <summary>
        ///// Get factors that are used in Alva.
        ///// </summary>
        //private void GetSpeciesInformationDocument()
        //{
        //    IFactor speciesInformationDocumentIsPublishableFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentIsPublishable);
        //    ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria
        //    {
        //        Factors = new FactorList { speciesInformationDocumentIsPublishableFactor },
        //        Taxa = new TaxonList { mTaxon }
        //    };
        //    SpeciesFactList speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(mUserContext, searchCriteria);

        //    if (speciesFacts.IsNotEmpty())
        //    {
        //        ISpeciesFact speciesInformationDocumentIsPublishableSpeciesFact = speciesFacts[0];

        //        if (speciesInformationDocumentIsPublishableSpeciesFact.IsSpeciesInformationDocumentIsPublishableSpecified())
        //        {
        //            mTaxonSpeciesFact.IsSpeciesInformationDocumentPublishable = speciesInformationDocumentIsPublishableSpeciesFact.MainField.BooleanValue;

        //            if (mTaxonSpeciesFact.IsSpeciesInformationDocumentPublishable)
        //            {
        //                IFactor speciesInformationDocumentPreambleFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentPreamble);
        //                IFactor speciesInformationDocumentDescriptionFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentDescription);
        //                IFactor speciesInformationDocumentDistributionFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentDistribution);
        //                IFactor speciesInformationDocumentEcologyFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentEcology);
        //                IFactor speciesInformationDocumentThreatsFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentThreats);
        //                IFactor speciesInformationDocumentMeasuresFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentMeasures);
        //                IFactor speciesInformationDocumentExtraFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentExtra);
        //                IFactor speciesInformationDocumentReferencesFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentReferences);
        //                IFactor speciesInformationDocumentAuthorAndYearFactor = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentAuthorAndYear);
        //                IFactor speciesInformationDocumentItalicsInReferences = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentItalicsInReferences);
        //                IFactor speciesInformationDocumentItalicsInText = CoreData.FactorManager.GetFactor(mUserContext, FactorId.SpeciesInformationDocumentItalicsInText);
        //                ISpeciesFact speciesInformationDocumentPreambleSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentDescriptionSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentDistributionSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentEcologySpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentThreatsSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentMeasuresSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentExtraSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentReferencesSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentAuthorAndYearSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentItalicsInReferencesSpeciesFact = null;
        //                ISpeciesFact speciesInformationDocumentItalicsInTextSpeciesFact = null;

        //                searchCriteria.Factors.Remove(speciesInformationDocumentIsPublishableFactor);
        //                searchCriteria.Add(speciesInformationDocumentPreambleFactor);
        //                searchCriteria.Add(speciesInformationDocumentDescriptionFactor);
        //                searchCriteria.Add(speciesInformationDocumentDistributionFactor);
        //                searchCriteria.Add(speciesInformationDocumentEcologyFactor);
        //                searchCriteria.Add(speciesInformationDocumentThreatsFactor);
        //                searchCriteria.Add(speciesInformationDocumentMeasuresFactor);
        //                searchCriteria.Add(speciesInformationDocumentExtraFactor);
        //                searchCriteria.Add(speciesInformationDocumentReferencesFactor);
        //                searchCriteria.Add(speciesInformationDocumentAuthorAndYearFactor);
        //                searchCriteria.Add(speciesInformationDocumentItalicsInReferences);
        //                searchCriteria.Add(speciesInformationDocumentItalicsInText);
        //                speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(mUserContext, searchCriteria);
        //                if (speciesFacts.IsNotEmpty())
        //                {
        //                    foreach (ISpeciesFact speciesFact in speciesFacts)
        //                    {
        //                        switch (speciesFact.Factor.Id)
        //                        {
        //                            case (int)FactorId.SpeciesInformationDocumentPreamble:
        //                                speciesInformationDocumentPreambleSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentDescription:
        //                                speciesInformationDocumentDescriptionSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentDistribution:
        //                                speciesInformationDocumentDistributionSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentEcology:
        //                                speciesInformationDocumentEcologySpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentThreats:
        //                                speciesInformationDocumentThreatsSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentMeasures:
        //                                speciesInformationDocumentMeasuresSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentExtra:
        //                                speciesInformationDocumentExtraSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentReferences:
        //                                speciesInformationDocumentReferencesSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentAuthorAndYear:
        //                                speciesInformationDocumentAuthorAndYearSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentItalicsInReferences:
        //                                speciesInformationDocumentItalicsInReferencesSpeciesFact = speciesFact;
        //                                break;
        //                            case (int)FactorId.SpeciesInformationDocumentItalicsInText:
        //                                speciesInformationDocumentItalicsInTextSpeciesFact = speciesFact;
        //                                break;
        //                        }
        //                    }
        //                }

        //                mTaxonSpeciesFact.InitSpeciesInformationDocumentInformation(speciesInformationDocumentPreambleSpeciesFact,
        //                                                                           speciesInformationDocumentDescriptionSpeciesFact,
        //                                                                           speciesInformationDocumentDistributionSpeciesFact,
        //                                                                           speciesInformationDocumentEcologySpeciesFact,
        //                                                                           speciesInformationDocumentThreatsSpeciesFact,
        //                                                                           speciesInformationDocumentMeasuresSpeciesFact,
        //                                                                           speciesInformationDocumentExtraSpeciesFact,
        //                                                                           speciesInformationDocumentReferencesSpeciesFact,
        //                                                                           speciesInformationDocumentAuthorAndYearSpeciesFact,
        //                                                                           speciesInformationDocumentItalicsInReferencesSpeciesFact,
        //                                                                           speciesInformationDocumentItalicsInTextSpeciesFact);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Creates a list of Link items for all links recommended for a certain taxon.
        ///// </summary>
        ///// <param name="taxonId">The taxon.</param>
        ///// <returns>A list of link items.</returns>
        //public static List<LinkItem> GetRecommendedLinks(int taxonId)
        //{
        //    var sp = new Stopwatch();
        //    sp.Start();

        //    var links = new List<LinkItem>();
        //    var linkManager = new LinkManager();
        //    LinkItem item;
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
        //    ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);

        //    // Add link to Photos from Artportalen
        //    string url = linkManager.GetUrlToMediaAp(taxon.Id.ToString(CultureInfo.InvariantCulture));

        //    if (url.IsNotEmpty())
        //    {
        //        item = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, RedListResource.LinkToPhotosAPLabel, url);
        //        links.Add(item);
        //    }

        //    // Add links to search in Dyntaxa
        //    url = linkManager.GetUrlToDyntaxa(taxon.Id.ToString(CultureInfo.InvariantCulture));

        //    if (url.IsNotEmpty())
        //    {
        //        item = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, RedListResource.LinkToDyntaxaLabel, url);
        //        links.Add(item);
        //    }

        //    // Add link to Google images.
        //    url = linkManager.GetUrlToGoogleImagesResults(taxon.ScientificName);
        //    if (url.IsNotEmpty())
        //    {
        //        item = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToGoogleImagesLabel, url);
        //        links.Add(item);
        //    }

        //    if (taxon.ScientificName.IsNotEmpty())
        //    {
        //        // Add link to GBIF
        //        url = linkManager.GetUrlToGbif(taxon.ScientificName);
        //        if (url.IsNotEmpty())
        //        {
        //            item = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToGbifLabel, url);
        //            links.Add(item);
        //        }

        //        // Add link to EoL
        //        url = linkManager.GetUrlToEoL(taxon.ScientificName);
        //        if (url.IsNotEmpty())
        //        {
        //            item = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToEoLLabel, url);
        //            links.Add(item);
        //        }

        //        // Add link to Biodiversity Heritage Library.
        //        url = linkManager.GetUrlToBiodiversityHeritageLibrary(taxon.ScientificName);
        //        if (url.IsNotEmpty())
        //        {
        //            item = new LinkItem(LinkType.Url, LinkQuality.Automatic, "Biodiversity Heritage Library", url);
        //            links.Add(item);
        //        }
        //    }

        //    // Add a link to ITIS Taxon information.
        //    var itisNumberNames = taxon.GetTaxonNamesBySearchCriteria(userContext, (int)TaxonNameCategoryId.ItisNumber, null, null, true, false, false);
        //    foreach (var name in itisNumberNames)
        //    {
        //        url = AppSettings.Default.UrlToGetITISTaxonInformation.Replace("[Id]", name.Name);
        //        var linkItem = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, RedListResource.LinkToItisLabel, url);
        //        links.Add(linkItem);
        //    }

        //    var guidNames = taxon.GetTaxonNamesBySearchCriteria(userContext, (int)TaxonNameCategoryId.Guid, null, null, null, false, false);
        //    foreach (var name in guidNames)
        //    {
        //        LinkItem linkItem;

        //        // Add a link to PESI Taxon information.
        //        url = AppSettings.Default.UrlToGetPESITaxonInformation.Replace("[GUID]", name.Name);
        //        if (name.IsRecommended)
        //        {
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, RedListResource.LinkToPesiLabel, url);
        //            links.Add(linkItem);
        //        }
        //        else
        //        {
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToPesiLabel, url);
        //            links.Add(linkItem);
        //        }

        //        // Add a link to Fauna Europea Taxon information.
        //        url = linkManager.GetUrlToFaunaEuropeaTaxonInformation(name.Name);
        //        if (url.IsNotEmpty())
        //        {
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToFaunaEuropeaLabel, url);
        //            links.Add(linkItem);
        //        }

        //        // Add a link to Marbef (ERMS) Taxon information.
        //        url = linkManager.GetUrlToMarbefTaxonInformation(name.Name);
        //        if (url.IsNotEmpty())
        //        {
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToMarbefLabel, url);
        //            links.Add(linkItem);

        //            url = linkManager.GetUrlToNordicMicroalgaeTaxonInformation(taxon.ScientificName);
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToNordicMicroalgaeLabel, url);
        //            links.Add(linkItem);
        //        }

        //        // Add a link to Algaebase Taxon information.
        //        url = linkManager.GetUrlToAlgaebaseTaxonInformation(name.Name);
        //        if (url.IsNotEmpty())
        //        {
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToAlgaebaseLabel, url);
        //            links.Add(linkItem);

        //            url = linkManager.GetUrlToNordicMicroalgaeTaxonInformation(taxon.ScientificName);
        //            linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToNordicMicroalgaeLabel, url);
        //            links.Add(linkItem);
        //        }
        //    }

        //    // Add link to Naturforskaren taxon information
        //    url = linkManager.GetUrlToNaturforskaren(taxon.ScientificName);
        //    if (url.IsNotEmpty())
        //    {
        //        item = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToNaturforskarenLabel, url);
        //        links.Add(item);
        //    }

        //    // Add link to WIKI taxon information
        //    url = linkManager.GetUrlToWikipedia(taxon.ScientificName);
        //    if (url.IsNotEmpty())
        //    {
        //        item = new LinkItem(LinkType.Url, LinkQuality.Automatic, RedListResource.LinkToWikipediaLabel, url);
        //        links.Add(item);
        //    }

        //    sp.Stop();
        //    Debug.WriteLine("TaxonInfo - Retrieving links: {0:N0} milliseconds", sp.ElapsedMilliseconds);
        //    return links;
        //}

        //public byte[] GetCountyMap(ITaxon taxon)
        //{
        //    // Set swedish locale
        //    SetSwedishLangugage();

        //    // Check so that we have a valid SpeciesFactList containing elements
        //    if (IsSpeciesFactFactListEmpty(taxon))
        //    {
        //        return null;
        //    }

        //    byte[] buffer = null;
        //    var returnStream = new MemoryStream();

        //    try
        //    {
        //        var countyMapProvider = new RedListCountyOccurrenceMap(mUserContext, taxon);

        //        if (countyMapProvider.CountyInformationExist)
        //        {
        //            countyMapProvider.Height = 800;
        //            countyMapProvider.UpdateInformation = RedListResource.TaxonInfoDistributionInSwedenCountyOccurrence;

        //            var map = countyMapProvider.Bitmap;
        //            map.Save(returnStream, ImageFormat.Png);

        //            buffer = returnStream.GetBuffer();
        //            returnStream.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to update county map cache", ex);
        //    }

        //    return buffer;
        //}

        private bool IsSpeciesFactFactListEmpty(ITaxon taxon)
        {
            IFactorSearchCriteria factorSearchCrieteria = new FactorSearchCriteria();
            var countyIds = new List<int> { (int)FactorId.CountyOccurrence };
            factorSearchCrieteria.RestrictSearchToFactorIds = countyIds;
            factorSearchCrieteria.RestrictReturnToScope = FactorSearchScope.LeafFactors;
            FactorList counties = CoreData.FactorManager.GetFactors(mUserContext, factorSearchCrieteria);

            ISpeciesFactSearchCriteria parameters = new SpeciesFactSearchCriteria();
            parameters.Taxa = new TaxonList { taxon };
            parameters.Factors = new FactorList();
            parameters.Factors = counties;
            parameters.IncludeNotValidHosts = true;
            parameters.IncludeNotValidTaxa = true;

            SpeciesFactList sList = CoreData.SpeciesFactManager.GetSpeciesFacts(mUserContext, parameters);

            return sList.IsEmpty();
        }

        /// <summary>
        /// Sets the language of the thread to swedish
        /// </summary>
        private void SetSwedishLangugage()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "sv-SE" ||
                Thread.CurrentThread.CurrentUICulture.Name != "sv-SE")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            }
        }

        //public byte[] GetPdf(string id)
        //{
        //    // Get taxa
        //    var taxon = TaxonHelper.Instance.GetTaxon(id);

        //    // Set swedish locale
        //    SetSwedishLangugage();

        //    string createPdfUrl;
        //    switch (Environment.MachineName)
        //    {
        //        case "MONESES-DEV":
        //            {
        //                createPdfUrl = AppSettings.Default.SpeciesFactCachePdfServiceUrlPrefixMonesesDev + @"taxon/" + taxon.Id +
        //                               @"/artfaktabladsompdf";
        //                break;
        //            }
        //        case "MONESES2-1":
        //            {
        //                createPdfUrl = AppSettings.Default.SpeciesFactCachePdfServiceUrlPrefixMoneses2 + @"taxon/" + taxon.Id +
        //                               @"/artfaktabladsompdf";
        //                break;
        //            }
        //        case "LAMPETRA2-1":
        //            {
        //                createPdfUrl = AppSettings.Default.SpeciesFactCachePdfServiceUrlPrefixLampetra1 + @"taxon/" + taxon.Id +
        //                               @"/artfaktabladsompdf";
        //                break;
        //            }
        //        case "LAMPETRA2-2":
        //            {
        //                createPdfUrl = AppSettings.Default.SpeciesFactCachePdfServiceUrlPrefixLampetra2 + @"taxon/" + taxon.Id +
        //                               @"/artfaktabladsompdf";
        //                break;
        //            }
        //        default:
        //            {
        //                createPdfUrl = AppSettings.Default.SpeciesFactCachePdfServiceUrlPrefixDev + @"taxon/" + taxon.Id +
        //                               @"/artfaktabladsompdf";
        //                break;
        //            }
        //    }

        //    byte[] buffer;
        //    try
        //    {
        //        // Create the request
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(createPdfUrl);

        //        // Set the the timeout for the call to the pdf service
        //        httpWebRequest.Timeout = 50000;

        //        // Fetch the response from the pdf service
        //        var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        //        if (httpWebResponse.ContentLength == 0)
        //        {
        //            throw new Exception(string.Format("Failed to get a response from the pdf service with taxonid: {0}", taxon.Id));
        //        }

        //        Stream responseStream = httpWebResponse.GetResponseStream();

        //        if (responseStream == null)
        //        {
        //            throw new Exception(string.Format("Failed to get a response from the pdf service with taxonid: {0}, stream is null", taxon.Id));
        //        }

        //        using (var binaryReader = new BinaryReader(responseStream))
        //        {
        //            var length = (int)httpWebResponse.ContentLength;
        //            buffer = binaryReader.ReadBytes(length);
        //        }

        //        if (responseStream.IsNotNull())
        //        {
        //            responseStream.Close();
        //        }

        //        if (httpWebResponse.IsNotNull())
        //        {
        //            httpWebResponse.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("Failed to update pdf cache for taxon {0}", taxon.Id), ex);
        //    }

        //    return buffer;
        //}
    }
}
