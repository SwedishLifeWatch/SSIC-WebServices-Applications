using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of factor information.
    /// </summary>
    public class FactorManagerMultiThreadCache : FactorManagerSingleThreadCache
    {
        /// <summary>
        /// Get factor origins for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor origins for specified locale.</returns>
        protected override FactorOriginList GetFactorOrigins(ILocale locale)
        {
            FactorOriginList factorOrigins = null;

            lock (FactorOrigins)
            {
                if (FactorOrigins.ContainsKey(locale.ISOCode))
                {
                    factorOrigins = (FactorOriginList)(FactorOrigins[locale.ISOCode]);
                }
            }
            return factorOrigins;
        }

        /// <summary>
        /// Get factor update modes for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor update modes for specified locale.</returns>
        protected override FactorUpdateModeList GetFactorUpdateModes(ILocale locale)
        {
            FactorUpdateModeList factorUpdateModes = null;

            lock (FactorUpdateModes)
            {
                if (FactorUpdateModes.ContainsKey(locale.ISOCode))
                {
                    factorUpdateModes = (FactorUpdateModeList)(FactorUpdateModes[locale.ISOCode]);
                }
            }
            return factorUpdateModes;
        }

        /// <summary>
        /// Get factor field types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor field types for specified locale.</returns>
        protected override FactorFieldTypeList GetFactorFieldTypes(ILocale locale)
        {
            FactorFieldTypeList factorFieldTypes = null;

            lock (FactorFieldTypes)
            {
                if (FactorFieldTypes.ContainsKey(locale.ISOCode))
                {
                    factorFieldTypes = (FactorFieldTypeList)(FactorFieldTypes[locale.ISOCode]);
                }
            }
            return factorFieldTypes;
        }

        /// <summary>
        /// Get period types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Period types for specified locale.</returns>
        protected override PeriodTypeList GetPeriodTypes(ILocale locale)
        {
            PeriodTypeList periodTypes = null;

            lock (PeriodTypes)
            {
                if (PeriodTypes.ContainsKey(locale.ISOCode))
                {
                    periodTypes = (PeriodTypeList)(PeriodTypes[locale.ISOCode]);
                }
            }
            return periodTypes;
        }

        /// <summary>
        /// Get organism groups for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        /// <returns>Organism groups for specified locale.</returns>
        protected override OrganismGroupList GetOrganismGroups(ILocale locale,
                                                               OrganismGroupType organismGroupType)
        {
            OrganismGroupList organismGroups = null;
            String cacheKey;

            cacheKey = locale.ISOCode + "#" + organismGroupType;
            lock (OrganismGroups)
            {
                if (OrganismGroups.ContainsKey(cacheKey))
                {
                    organismGroups = (OrganismGroupList)(OrganismGroups[cacheKey]);
                }
            }

            return organismGroups;
        }

        /// <summary>
        /// Get periods for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Periods for specified locale.</returns>
        protected override PeriodList GetPeriods(ILocale locale)
        {
            PeriodList periods = null;

            lock (Periods)
            {
                if (Periods.ContainsKey(locale.ISOCode))
                {
                    periods = (PeriodList)(Periods[locale.ISOCode]);
                }
            }
            return periods;
        }

        /// <summary>
        /// Get individual categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Individual categories for specified locale.</returns>
        protected override IndividualCategoryList GetIndividualCategories(ILocale locale)
        {
            IndividualCategoryList individualCategories = null;

            lock (IndividualCategories)
            {
                if (IndividualCategories.ContainsKey(locale.ISOCode))
                {
                    individualCategories = (IndividualCategoryList)(IndividualCategories[locale.ISOCode]);
                }
            }
            return individualCategories;
        }

        /// <summary>
        /// Get factor field enums for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor field enums for specified locale.</returns>
        protected override FactorFieldEnumList GetFactorFieldEnums(ILocale locale)
        {
            FactorFieldEnumList factorFieldEnums = null;

            lock (FactorFieldEnums)
            {
                if (FactorFieldEnums.ContainsKey(locale.ISOCode))
                {
                    factorFieldEnums = (FactorFieldEnumList)(FactorFieldEnums[locale.ISOCode]);
                }
            }
            return factorFieldEnums;
        }

        /// <summary>
        /// Get factor data types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor data types for specified locale.</returns>
        protected override FactorDataTypeList GetFactorDataTypes(ILocale locale)
        {
            FactorDataTypeList factorDataTypes = null;

            lock (FactorDataTypes)
            {
                if (FactorDataTypes.ContainsKey(locale.ISOCode))
                {
                    factorDataTypes = (FactorDataTypeList)(FactorDataTypes[locale.ISOCode]);
                }
            }
            return factorDataTypes;
        }

        /// <summary>
        /// Get factors for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor or all from cache.</param>
        /// <returns>Factors for specified locale.</returns>
        protected override FactorList GetFactors(ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorList factors = null;

            lock (Factors)
            {
                if (Factors.ContainsKey(locale.ISOCode + "#" + factorTreeType))
                {
                    factors = (FactorList)(Factors[locale.ISOCode + "#" + factorTreeType]);
                }
            }
            return factors;
        }

        /// <summary>
        /// Get a factor tree node for specified locale by the spcified factor id.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="factorId">The specified factor id.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        /// <returns>A factor tree node related to the specified factor id for specified locale.</returns>
        protected override IFactorTreeNode GetFactorTreeNode(ILocale locale, int factorId, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            IFactorTreeNode factorTreeNode = null;

            lock (FactorTreeNodes)
            {
                if (FactorTreeNodes.ContainsKey(locale.ISOCode + "#" + factorTreeType))
                {
                    factorTreeNode = ((Dictionary<int, IFactorTreeNode>)(FactorTreeNodes[locale.ISOCode + "#" + factorTreeType]))[factorId];
                }
            }

            return factorTreeNode;
        }

        /// <summary>
        /// Get factor trees for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        /// <returns>Factor trees for specified locale.</returns>
        protected override FactorTreeNodeList GetFactorTrees(ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorTreeNodeList factorTrees = null;

            lock (FactorTrees)
            {
                if (FactorTrees.ContainsKey(locale.ISOCode + "#" + factorTreeType))
                {
                    factorTrees = (FactorTreeNodeList)(FactorTrees[locale.ISOCode + "#" + factorTreeType]);
                }
            }
            return factorTrees;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (FactorOrigins)
            {
                FactorOrigins.Clear();
            }

            lock (FactorUpdateModes)
            {
                FactorUpdateModes.Clear();
            }

            lock (FactorFieldTypes)
            {
                FactorFieldTypes.Clear();
            }

            lock (PeriodTypes)
            {
                PeriodTypes.Clear();
            }

            lock (Periods)
            {
                Periods.Clear();
            }
            lock (IndividualCategories)
            {
                IndividualCategories.Clear();
            }
            lock (FactorFieldEnums)
            {
                FactorFieldEnums.Clear();
            }

            lock (FactorDataTypes)
            {
                FactorDataTypes.Clear();
            }
            lock (Factors)
            {
                Factors.Clear();
            }

            lock (FactorTrees)
            {
                FactorTrees.Clear();
            }

            lock (FactorTreeNodes)
            {
                FactorTreeNodes.Clear();
            }

            lock (OrganismGroups)
            {
                OrganismGroups.Clear();
            }
        }

        /// <summary>
        /// Set factor origins for specified locale.
        /// </summary>
        /// <param name="factorOrigins">Factor origins.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetFactorOrigins(FactorOriginList factorOrigins,
                                                 ILocale locale)
        {
            lock (FactorOrigins)
            {
                FactorOrigins[locale.ISOCode] = factorOrigins;
            }
        }

        /// <summary>
        /// Set factor update modes for specified locale.
        /// </summary>
        /// <param name="factorUpdateModes">Factor update modes.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetFactorUpdateModes(FactorUpdateModeList factorUpdateModes,
                                                     ILocale locale)
        {
            lock (FactorUpdateModes)
            {
                FactorUpdateModes[locale.ISOCode] = factorUpdateModes;
            }
        }

        /// <summary>
        /// Set factor field types for specified locale.
        /// </summary>
        /// <param name="factorFieldTypes">Factor field types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetFactorFieldTypes(FactorFieldTypeList factorFieldTypes,
                                                    ILocale locale)
        {
            lock (FactorFieldTypes)
            {
                FactorFieldTypes[locale.ISOCode] = factorFieldTypes;
            }
        }

        /// <summary>
        /// Set organism groups for specified locale.
        /// </summary>
        /// <param name="organismGroups">Organism groups.</param>
        /// <param name="locale">Currently used locale.</param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        protected override void SetOrganismGroups(OrganismGroupList organismGroups,
                                                  ILocale locale,
                                                  OrganismGroupType organismGroupType)
        {
            String cacheKey;

            cacheKey = locale.ISOCode + "#" + organismGroupType;
            lock (OrganismGroups)
            {
                OrganismGroups[cacheKey] = organismGroups;
            }
        }

        /// <summary>
        /// Set period types for specified locale.
        /// </summary>
        /// <param name="periodTypes">Period types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetPeriodTypes(PeriodTypeList periodTypes,
                                               ILocale locale)
        {
            lock (PeriodTypes)
            {
                PeriodTypes[locale.ISOCode] = periodTypes;
            }
        }

        /// <summary>
        /// Set periods for specified locale.
        /// </summary>
        /// <param name="periods">Periods.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetPeriods(PeriodList periods,
                                           ILocale locale)
        {
            lock (Periods)
            {
                Periods[locale.ISOCode] = periods;
            }
        }

        /// <summary>
        /// Set individual categories for specified locale.
        /// </summary>
        /// <param name="individualCategories">Individual categories.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetIndividualCategories(IndividualCategoryList individualCategories,
                                                        ILocale locale)
        {
            lock (IndividualCategories)
            {
                IndividualCategories[locale.ISOCode] = individualCategories;
            }
        }

        /// <summary>
        /// Set factor field enums for specified locale.
        /// </summary>
        /// <param name="factorFieldEnums">Factor field enums.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetFactorFieldEnums(FactorFieldEnumList factorFieldEnums,
                                                    ILocale locale)
        {
            lock (FactorFieldEnums)
            {
                FactorFieldEnums[locale.ISOCode] = factorFieldEnums;
            }
        }

        /// <summary>
        /// Set factor data types for specified locale.
        /// </summary>
        /// <param name="factorDataTypes">Factor data types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetFactorDataTypes(FactorDataTypeList factorDataTypes,
                                                   ILocale locale)
        {
            lock (FactorDataTypes)
            {
                FactorDataTypes[locale.ISOCode] = factorDataTypes;
            }
        }

        /// <summary>
        /// Set factors for specified locale.
        /// </summary>
        /// <param name="factors">Factors.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected override void SetFactors(FactorList factors, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            lock (Factors)
            {
                Factors[locale.ISOCode + "#" + factorTreeType] = factors;
            }
        }

        /// <summary>
        /// Set factor trees for specified locale.
        /// </summary>
        /// <param name="factorTrees">Factor trees.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected override void SetFactorTrees(FactorTreeNodeList factorTrees, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            lock (FactorTrees)
            {
                FactorTrees[locale.ISOCode + "#" + factorTreeType] = factorTrees;
            }
        }

        /// <summary>
        /// Set factor tree nodes for specified locale.
        /// </summary>
        /// <param name="factorTreeNodes">Factor tree nodes.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected override void SetFactorTreeNodes(Dictionary<int, IFactorTreeNode> factorTreeNodes, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            lock (FactorTreeNodes)
            {
                FactorTreeNodes[locale.ISOCode + "#" + factorTreeType] = factorTreeNodes;
            }
        }
    }
}