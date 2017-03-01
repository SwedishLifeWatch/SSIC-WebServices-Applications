using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

//using RedList.Data.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class handles landscape type information.
    /// </summary>
    public class LandscapeTypeManager
    {
        private static FactorList mLandscapeTypeFactors;
        //private static Hashtable _landscapeTypeOccurrence;

        /// <summary>
        /// Get landscape type factors
        /// </summary>
        public static FactorList LandscapeTypeFactors
        {
            get
            {
                if (mLandscapeTypeFactors.IsNull())
                {
                    LoadLandscapeTypeFactors(CoreData.UserManager.GetApplicationContext());
                }

                return mLandscapeTypeFactors;
            }
        }

        ///// <summary>
        ///// Get all taxa that occurs in specified landscape types.
        ///// </summary>
        ///// <param name="landscapeTypes">Landscape types.</param>
        ///// <param name="isLandscapeTypeImportant">If true, landscape type must be important for returned taxa.</param>
        ///// <param name="dataQueryType">Data query type (only OR and AND is handled in this method)</param>
        ///// <returns>All taxa that occurs in specified landscape types.</returns>
        //public static TaxonList GetTaxa(List<String> landscapeTypes,
        //                                Boolean isLandscapeTypeImportant,
        //                                DataQueryType dataQueryType)
        //{
        //    TaxonList landscapeTypeOccurrenceTaxa, tempTaxa;

        //    landscapeTypeOccurrenceTaxa = null;
        //    if (landscapeTypes.IsNotEmpty())
        //    {
        //        foreach (String landscapeType in landscapeTypes)
        //        {
        //            if (_landscapeTypeOccurrence[landscapeType].IsNotNull())
        //            {
        //                switch (dataQueryType)
        //                {
        //                    case DataQueryType.AndCondition:
        //                        if (landscapeTypeOccurrenceTaxa.IsNull())
        //                        {
        //                            landscapeTypeOccurrenceTaxa = new TaxonList(true);
        //                            landscapeTypeOccurrenceTaxa.AddRange((TaxonList)(_landscapeTypeOccurrence[landscapeType]));
        //                            if (!isLandscapeTypeImportant &&
        //                                (_landscapeTypeOccurrence[landscapeType.ToLower()].IsNotNull()))
        //                            {
        //                                landscapeTypeOccurrenceTaxa.AddRange((TaxonList)(_landscapeTypeOccurrence[landscapeType.ToLower()]));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            tempTaxa = new TaxonList(true);
        //                            tempTaxa.AddRange((TaxonList)(_landscapeTypeOccurrence[landscapeType]));
        //                            if (!isLandscapeTypeImportant &&
        //                                (_landscapeTypeOccurrence[landscapeType.ToLower()].IsNotNull()))
        //                            {
        //                                tempTaxa.AddRange((TaxonList)(_landscapeTypeOccurrence[landscapeType.ToLower()]));
        //                            }
        //                            landscapeTypeOccurrenceTaxa.Subset(tempTaxa);
        //                        }
        //                        break;

        //                    case DataQueryType.OrCondition:
        //                        if (landscapeTypeOccurrenceTaxa.IsNull())
        //                        {
        //                            landscapeTypeOccurrenceTaxa = new TaxonList(true);
        //                        }
        //                        landscapeTypeOccurrenceTaxa.Merge((TaxonList)(_landscapeTypeOccurrence[landscapeType]));
        //                        if (!isLandscapeTypeImportant &&
        //                            (_landscapeTypeOccurrence[landscapeType.ToLower()].IsNotNull()))
        //                        {
        //                            landscapeTypeOccurrenceTaxa.Merge((TaxonList)(_landscapeTypeOccurrence[landscapeType.ToLower()]));
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return landscapeTypeOccurrenceTaxa;
        //}

        /// <summary>
        /// Load information about landscape type
        /// factors into run time memory.
        /// </summary>
        public static void LoadLandscapeTypeFactors(IUserContext userContext)
        {
            mLandscapeTypeFactors = CoreData.FactorManager.GetFactorTree(userContext, FactorId.LandscapeFactors).GetAllLeafFactors();
        }

        ///// <summary>
        ///// Load information about landscape type
        ///// occurrence into run time memory.
        ///// </summary>
        //public static void LoadLandscapeTypeOccurrence()
        //{
        //    Int32 stringIndex;
        //    RedListData redListData;
        //    RedListDataList cachedRedListData;
        //    String landscapeTypeOccurrence;
        //    TaxonList taxa;

        //    cachedRedListData = RedListDataManager.RedListData;
        //    _landscapeTypeOccurrence = new Hashtable();
        //    foreach (Taxon taxon in TaxonManagerAdjusted.Taxa)
        //    {
        //        if (cachedRedListData.Exists(taxon.Id))
        //        {
        //            redListData = cachedRedListData.GetRedListData(taxon.Id);
        //            if (redListData.IsRedListed &&
        //                redListData.LandscapeTypeOccurrence.IsNotEmpty())
        //            {
        //                for (stringIndex = 0; stringIndex < redListData.LandscapeTypeOccurrence.Length; stringIndex++)
        //                {
        //                    landscapeTypeOccurrence = redListData.LandscapeTypeOccurrence.Substring(stringIndex, 1);
        //                    if (_landscapeTypeOccurrence[landscapeTypeOccurrence].IsNull())
        //                    {
        //                        taxa = new TaxonList(true);
        //                        _landscapeTypeOccurrence[landscapeTypeOccurrence] = taxa;
        //                    }
        //                    else
        //                    {
        //                        taxa = (TaxonList)(_landscapeTypeOccurrence[landscapeTypeOccurrence]);
        //                    }
        //                    taxa.Add(taxon);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
