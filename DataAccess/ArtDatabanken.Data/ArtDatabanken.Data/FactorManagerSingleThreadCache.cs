using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    using System;

    /// <summary>
    /// Class that handles cache of factor information.
    /// </summary>
    public class FactorManagerSingleThreadCache : FactorManager
    {
        /// <summary>
        /// Create a FactorManagerSingleThreadCache instance.
        /// </summary>
        public FactorManagerSingleThreadCache()
        {
            FactorOrigins = new Hashtable();
            FactorUpdateModes = new Hashtable();
            FactorFieldTypes = new Hashtable();
            PeriodTypes = new Hashtable();
            Periods = new Hashtable();
            IndividualCategories = new Hashtable();
            FactorFieldEnums = new Hashtable();
            FactorDataTypes = new Hashtable();
            Factors = new Hashtable();
            FactorTrees = new Hashtable();
            FactorTreeNodes = new Hashtable();
            OrganismGroups = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Factor origins cache.
        /// </summary>
        protected Hashtable FactorOrigins { get; private set; }

        /// <summary>
        /// Factor update modes cache.
        /// </summary>
        protected Hashtable FactorUpdateModes { get; private set; }

        /// <summary>
        /// Factor field types cache.
        /// </summary>
        protected Hashtable FactorFieldTypes { get; private set; }

        /// <summary>
        /// Organism groups cache.
        /// </summary>
        protected Hashtable OrganismGroups { get; private set; }

        /// <summary>
        /// Period types cache.
        /// </summary>
        protected Hashtable PeriodTypes { get; private set; }

        /// <summary>
        /// Periods cache.
        /// </summary>
        protected Hashtable Periods { get; private set; }

        /// <summary>
        /// Individual categories cache.
        /// </summary>
        protected Hashtable IndividualCategories { get; private set; }

        /// <summary>
        /// Factor field enumerations cache.
        /// </summary>
        protected Hashtable FactorFieldEnums { get; private set; }

        /// <summary>
        /// Factor data types cache.
        /// </summary>
        protected Hashtable FactorDataTypes { get; private set; }

        /// <summary>
        /// Factors cache.
        /// </summary>
        protected Hashtable Factors { get; private set; }

        /// <summary>
        /// Factor trees cache.
        /// </summary>
        protected Hashtable FactorTrees { get; private set; }

        /// <summary>
        /// Factor tree nodes cache.
        /// </summary>
        protected Hashtable FactorTreeNodes { get; private set; }

        /// <summary>
        /// Get factor origins for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Factor origins for specified locale.</returns>
        protected virtual FactorOriginList GetFactorOrigins(ILocale locale)
        {
            FactorOriginList factorOrigins = null;

            if (FactorOrigins.ContainsKey(locale.ISOCode))
            {
                factorOrigins = (FactorOriginList)(FactorOrigins[locale.ISOCode]);
            }

            return factorOrigins;
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor origins.</returns>
        public override FactorOriginList GetFactorOrigins(IUserContext userContext)
        {
            FactorOriginList factorOrigins;

            factorOrigins = GetFactorOrigins(userContext.Locale);
            if (factorOrigins.IsNull())
            {
                factorOrigins = base.GetFactorOrigins(userContext);
                SetFactorOrigins(factorOrigins, userContext.Locale);
            }

            return factorOrigins;
        }

        /// <summary>
        /// Get factor update modes for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Factor update modes for specified locale.</returns>
        protected virtual FactorUpdateModeList GetFactorUpdateModes(ILocale locale)
        {
            FactorUpdateModeList factorUpdateModes = null;

            if (FactorUpdateModes.ContainsKey(locale.ISOCode))
            {
                factorUpdateModes = (FactorUpdateModeList)(FactorUpdateModes[locale.ISOCode]);
            }

            return factorUpdateModes;
        }

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor update modes.</returns>
        public override FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext)
        {
            FactorUpdateModeList factorUpdateModes;

            factorUpdateModes = GetFactorUpdateModes(userContext.Locale);
            if (factorUpdateModes.IsNull())
            {
                factorUpdateModes = base.GetFactorUpdateModes(userContext);
                SetFactorUpdateModes(factorUpdateModes, userContext.Locale);
            }

            return factorUpdateModes;
        }

        /// <summary>
        /// Get factor field types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor field types for specified locale.</returns>
        protected virtual FactorFieldTypeList GetFactorFieldTypes(ILocale locale)
        {
            FactorFieldTypeList factorFieldTypes = null;

            if (FactorFieldTypes.ContainsKey(locale.ISOCode))
            {
                factorFieldTypes = (FactorFieldTypeList)(FactorFieldTypes[locale.ISOCode]);
            }
            return factorFieldTypes;
        }

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <returns>All factor field types.</returns>
        public override FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext)
        {
            FactorFieldTypeList factorFieldTypes;

            factorFieldTypes = GetFactorFieldTypes(userContext.Locale);
            if (factorFieldTypes.IsNull())
            {
                factorFieldTypes = base.GetFactorFieldTypes(userContext);
                SetFactorFieldTypes(factorFieldTypes, userContext.Locale);
            }
            return factorFieldTypes;
        }

        /// <summary>
        /// Get period types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Period types for specified locale.</returns>
        protected virtual PeriodTypeList GetPeriodTypes(ILocale locale)
        {
            PeriodTypeList periodTypes = null;

            if (PeriodTypes.ContainsKey(locale.ISOCode))
            {
                periodTypes = (PeriodTypeList)(PeriodTypes[locale.ISOCode]);
            }
            return periodTypes;
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <returns>All period types.</returns>
        public override PeriodTypeList GetPeriodTypes(IUserContext userContext)
        {
            PeriodTypeList periodTypes;

            periodTypes = GetPeriodTypes(userContext.Locale);
            if (periodTypes.IsNull())
            {
                periodTypes = base.GetPeriodTypes(userContext);
                SetPeriodTypes(periodTypes, userContext.Locale);
            }
            return periodTypes;
        }

        /// <summary>
        /// Get periods for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Periods for specified locale.</returns>
        protected virtual PeriodList GetPeriods(ILocale locale)
        {
            PeriodList periods = null;

            if (Periods.ContainsKey(locale.ISOCode))
            {
                periods = (PeriodList)(Periods[locale.ISOCode]);
            }
            return periods;
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <returns>All periods.</returns>
        public override PeriodList GetPeriods(IUserContext userContext)
        {
            PeriodList periods;

            periods = GetPeriods(userContext.Locale);
            if (periods.IsNull())
            {
                periods = base.GetPeriods(userContext);
                SetPeriods(periods, userContext.Locale);
            }
            return periods;
        }

        /// <summary>
        /// Get individual categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Individual categories for specified locale.</returns>
        protected virtual IndividualCategoryList GetIndividualCategories(ILocale locale)
        {
            IndividualCategoryList individualCategories = null;

            if (IndividualCategories.ContainsKey(locale.ISOCode))
            {
                individualCategories = (IndividualCategoryList)(IndividualCategories[locale.ISOCode]);
            }
            return individualCategories;
        }

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <returns>All individual categories.</returns>
        public override IndividualCategoryList GetIndividualCategories(IUserContext userContext)
        {
            IndividualCategoryList individualCategories;

            individualCategories = GetIndividualCategories(userContext.Locale);
            if (individualCategories.IsNull())
            {
                individualCategories = base.GetIndividualCategories(userContext);
                this.SetIndividualCategories(individualCategories, userContext.Locale);
            }
            return individualCategories;
        }

        /// <summary>
        /// Get factor field enums for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor field enums for specified locale.</returns>
        protected virtual FactorFieldEnumList GetFactorFieldEnums(ILocale locale)
        {
            FactorFieldEnumList factorFieldEnums = null;

            if (FactorFieldEnums.ContainsKey(locale.ISOCode))
            {
                factorFieldEnums = (FactorFieldEnumList)(FactorFieldEnums[locale.ISOCode]);
            }
            return factorFieldEnums;
        }

        /// <summary>
        /// Get all factor field enums.
        /// </summary>
        /// <returns>All factor field enums.</returns>
        public override FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext)
        {
            FactorFieldEnumList factorFieldEnums;

            factorFieldEnums = GetFactorFieldEnums(userContext.Locale);
            if (factorFieldEnums.IsNull())
            {
                factorFieldEnums = base.GetFactorFieldEnums(userContext);
                SetFactorFieldEnums(factorFieldEnums, userContext.Locale);
            }
            return factorFieldEnums;
        }

        /// <summary>
        /// Get factor data types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Factor data types for specified locale.</returns>
        protected virtual FactorDataTypeList GetFactorDataTypes(ILocale locale)
        {
            FactorDataTypeList factorDataTypes = null;

            if (FactorDataTypes.ContainsKey(locale.ISOCode))
            {
                factorDataTypes = (FactorDataTypeList)(FactorDataTypes[locale.ISOCode]);
            }
            return factorDataTypes;
        }

        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <returns>All factor data types.</returns>
        public override FactorDataTypeList GetFactorDataTypes(IUserContext userContext)
        {
            FactorDataTypeList factorDataTypes;

            factorDataTypes = GetFactorDataTypes(userContext.Locale);
            if (factorDataTypes.IsNull())
            {
                factorDataTypes = base.GetFactorDataTypes(userContext);
                SetFactorDataTypes(factorDataTypes, userContext.Locale);
            }
            return factorDataTypes;
        }

        /// <summary>
        /// Get factors for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factors or all from cache.</param>
        /// <returns>Factors for specified locale.</returns>
        protected virtual FactorList GetFactors(ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorList factors = null;

            if (Factors.ContainsKey(locale.ISOCode + "#" + factorTreeType))
            {
                factors = (FactorList)(Factors[locale.ISOCode + "#" + factorTreeType]);
            }
            return factors;
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factors.</returns>
        public override FactorList GetFactors(IUserContext userContext)
        {
            FactorList factors;

            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(userContext);

            // Get factors from cache
            factors = GetFactors(userContext.Locale, getOnlyPublicFactors);

            if (factors.IsNull())
            {
                factors = base.GetFactors(userContext);

                // Store factors in cache
                SetFactors(factors, userContext.Locale, getOnlyPublicFactors);
            }

            return factors;
        }

        /// <summary>
        /// Get a factor tree node by the specified factor id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">The specified factor id.</param>
        /// <returns>A factor tree node related to the specified factor id.</returns>
        public override IFactorTreeNode GetFactorTree(IUserContext userContext, int factorId)
        {
            IFactorTreeNode factorTreeNode = null;
            FactorTreeNodeList factorTrees = null;
            Dictionary<int, IFactorTreeNode> factorTreeNodes = new Dictionary<int, IFactorTreeNode>();
            IFactorTreeSearchCriteria searchCriteria;

            searchCriteria = new FactorTreeSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(factorId);

            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(userContext);

            // Get cached factor trees
            factorTrees = GetFactorTrees(userContext.Locale, getOnlyPublicFactors);
            factorTreeNode = GetFactorTreeNode(userContext.Locale, factorId, getOnlyPublicFactors);

            if (factorTrees.IsNull())
            {
                // Get factor trees based on user role and rights (get public factors or all factors)
                factorTrees = base.GetFactorTrees(userContext);

                // Store factor trees in cache
                SetFactorTrees(factorTrees, userContext.Locale, getOnlyPublicFactors);

                foreach (IFactorTreeNode node in factorTrees)
                {
                    if (!factorTreeNodes.ContainsKey(node.Id))
                    {
                        factorTreeNodes.Add(node.Id, node);
                    }

                    AddFactorTreeChildren(factorTreeNodes, node.Children);
                }

                // Store factor tree nodes in cache
                SetFactorTreeNodes(factorTreeNodes, userContext.Locale, getOnlyPublicFactors);
            }

            if (factorTrees.IsNotNull())
            {
                foreach (var factorTree in factorTrees)
                {
                    if (factorTree.Id == factorId)
                    {
                        factorTreeNode = factorTree;
                        break;
                    }
                }
            }

            if (factorTreeNode.IsNull() && factorTreeNodes.IsNotNull())
            {
                foreach (var factorTree in factorTreeNodes)
                {
                    if (factorTree.Key == factorId)
                    {
                        factorTreeNode = factorTree.Value;
                        break;
                    }
                }
            }

            return factorTreeNode;
        }

        /// <summary>
        /// Check each role of current user if any authority has identifier SpeciesFact.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>True means user have right to read non public factors and public factors.</returns>
        private static Boolean IsUserAuthorizedToReadNonPublicFactors(IUserContext context)
        {
            Boolean addNonPublicFactor = false;

            // Check if non-public factors should be returned
            if (context.CurrentRole != null)
            {
                foreach (var webAuthority in context.CurrentRole.Authorities)
                {
                    if (webAuthority.Identifier == UserRoleAuthorityIdentifierType.SpeciesFact.ToString())
                    {
                        if (webAuthority.ReadNonPublicPermission)
                        {
                            addNonPublicFactor = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (IRole role in context.CurrentRoles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (IAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == UserRoleAuthorityIdentifierType.SpeciesFact.ToString())
                            {
                                if (authority.ReadNonPublicPermission)
                                {
                                    addNonPublicFactor = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (addNonPublicFactor)
                    {
                        break;
                    }
                }
            }

            return addNonPublicFactor;
        }

        /// <summary>
        /// Get a factor tree node for specified locale by the spcified factor id.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="factorId">The specified factor id.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor tree node or not from cache.</param>
        /// <returns>A factor tree node related to the specified factor id for specified locale.</returns>
        protected virtual IFactorTreeNode GetFactorTreeNode(ILocale locale, int factorId, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            IFactorTreeNode factorTreeNode = null;

            if (FactorTreeNodes.ContainsKey(locale.ISOCode + "#" + factorTreeType))
            {
                factorTreeNode = ((Dictionary<int, IFactorTreeNode>)(FactorTreeNodes[locale.ISOCode + "#" + factorTreeType]))[factorId];
            }

            return factorTreeNode;
        }

        /// <summary>
        /// Get factor trees for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        /// <returns>Factors for specified locale.</returns>
        protected virtual FactorTreeNodeList GetFactorTrees(ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorTreeNodeList factorTrees = null;

            if (FactorTrees.ContainsKey(locale.ISOCode + "#" + factorTreeType))
            {
                factorTrees = (FactorTreeNodeList)(FactorTrees[locale.ISOCode + "#" + factorTreeType]);
            }
            return factorTrees;
        }

        /// <summary>
        /// Traverse a factor tree and creates a Dictionary for caching.
        /// </summary>
        /// <param name="factorTreeNodes">The Dictionary for caching.</param>
        /// <param name="children">Children factor trees to add to the dictionary.</param>
        private void AddFactorTreeChildren(Dictionary<int, IFactorTreeNode> factorTreeNodes, FactorTreeNodeList children)
        {
            if (children.IsNotEmpty())
            {
                foreach (IFactorTreeNode child in children)
                {
                    if (!factorTreeNodes.ContainsKey(child.Id))
                    {
                        factorTreeNodes.Add(child.Id, child);
                    }

                    AddFactorTreeChildren(factorTreeNodes, child.Children);
                }
            }
        }

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <returns>All factor trees.</returns>
        public override FactorTreeNodeList GetFactorTrees(IUserContext userContext)
        {
            FactorTreeNodeList factorTrees;
            Dictionary<int, IFactorTreeNode> factorTreeNodes = new Dictionary<int, IFactorTreeNode>();

            Boolean getOnlyPublicFactors = !IsUserAuthorizedToReadNonPublicFactors(userContext);

            // Get cached factor trees
            factorTrees = GetFactorTrees(userContext.Locale, getOnlyPublicFactors);

            if (factorTrees.IsNull())
            {
                factorTrees = base.GetFactorTrees(userContext);

                // Store factor trees in cache
                SetFactorTrees(factorTrees, userContext.Locale, getOnlyPublicFactors);

                foreach (IFactorTreeNode factorTreeNode in factorTrees)
                {
                    if (!factorTreeNodes.ContainsKey(factorTreeNode.Id))
                    {
                        factorTreeNodes.Add(factorTreeNode.Id, factorTreeNode);
                    }

                    AddFactorTreeChildren(factorTreeNodes, factorTreeNode.Children);
                }

                // Store factor tree nodes in cache
                SetFactorTreeNodes(factorTreeNodes, userContext.Locale, getOnlyPublicFactors);
            }

            return factorTrees;
        }

        /// <summary>
        /// Get organism groups for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        /// <returns>Organism groups for specified locale.</returns>
        protected virtual OrganismGroupList GetOrganismGroups(ILocale locale,
                                                              OrganismGroupType organismGroupType)
        {
            OrganismGroupList organismGroups = null;
            String cacheKey;

            cacheKey = locale.ISOCode + "#" + organismGroupType;
            if (OrganismGroups.ContainsKey(cacheKey))
            {
                organismGroups = (OrganismGroupList)(OrganismGroups[cacheKey]);
            }

            return organismGroups;
        }

        /// <summary>
        /// Get information about organism groups of specified type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        /// <returns>Information about organism groups of specified type.</returns>
        public override OrganismGroupList GetOrganismGroups(IUserContext userContext,
                                                            OrganismGroupType organismGroupType)
        {
            OrganismGroupList organismGroups;

            organismGroups = GetOrganismGroups(userContext.Locale, organismGroupType);
            if (organismGroups.IsNull())
            {
                organismGroups = base.GetOrganismGroups(userContext, organismGroupType);
                SetOrganismGroups(organismGroups, userContext.Locale, organismGroupType);
            }

            return organismGroups;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            FactorOrigins.Clear();
            FactorUpdateModes.Clear();
            FactorFieldTypes.Clear();
            PeriodTypes.Clear();
            Periods.Clear();
            IndividualCategories.Clear();
            FactorFieldEnums.Clear();
            FactorDataTypes.Clear();
            Factors.Clear();
            FactorTrees.Clear();
            FactorTreeNodes.Clear();
            OrganismGroups.Clear();
        }

        /// <summary>
        /// Set factor origins for specified locale.
        /// </summary>
        /// <param name="factorOrigins">Factor origins.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetFactorOrigins(FactorOriginList factorOrigins,
                                                ILocale locale)
        {
            FactorOrigins[locale.ISOCode] = factorOrigins;
        }

        /// <summary>
        /// Set factor update modes for specified locale.
        /// </summary>
        /// <param name="factorUpdateModes">Factor update modes.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetFactorUpdateModes(FactorUpdateModeList factorUpdateModes,
                                                    ILocale locale)
        {
            FactorUpdateModes[locale.ISOCode] = factorUpdateModes;
        }

        /// <summary>
        /// Set factor field types for specified locale.
        /// </summary>
        /// <param name="factorFieldTypes">Factor field types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetFactorFieldTypes(FactorFieldTypeList factorFieldTypes,
                                                   ILocale locale)
        {
            FactorFieldTypes[locale.ISOCode] = factorFieldTypes;
        }

        /// <summary>
        /// Set organism groups for specified locale.
        /// </summary>
        /// <param name="organismGroups">Organism groups.</param>
        /// <param name="locale">Currently used locale.</param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        protected virtual void SetOrganismGroups(OrganismGroupList organismGroups,
                                                 ILocale locale,
                                                 OrganismGroupType organismGroupType)
        {
            String cacheKey;

            cacheKey = locale.ISOCode + "#" + organismGroupType;
            OrganismGroups[cacheKey] = organismGroups;
        }

        /// <summary>
        /// Set period types for specified locale.
        /// </summary>
        /// <param name="periodTypes">Period types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetPeriodTypes(PeriodTypeList periodTypes,
                                              ILocale locale)
        {
            PeriodTypes[locale.ISOCode] = periodTypes;
        }

        /// <summary>
        /// Set periods for specified locale.
        /// </summary>
        /// <param name="periods">Periods.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetPeriods(PeriodList periods,
                                          ILocale locale)
        {
            Periods[locale.ISOCode] = periods;
        }

        /// <summary>
        /// Set individual categories for specified locale.
        /// </summary>
        /// <param name="individualCategories">Individual categories.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetIndividualCategories(IndividualCategoryList individualCategories,
                                                       ILocale locale)
        {
            IndividualCategories[locale.ISOCode] = individualCategories;
        }

        /// <summary>
        /// Set factor field enums for specified locale.
        /// </summary>
        /// <param name="factorFieldEnums">Factor field enums.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetFactorFieldEnums(FactorFieldEnumList factorFieldEnums,
                                                   ILocale locale)
        {
            FactorFieldEnums[locale.ISOCode] = factorFieldEnums;
        }

        /// <summary>
        /// Set factor data types for specified locale.
        /// </summary>
        /// <param name="factorDataTypes">Factor data types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetFactorDataTypes(FactorDataTypeList factorDataTypes,
                                                  ILocale locale)
        {
            FactorDataTypes[locale.ISOCode] = factorDataTypes;
        }

        /// <summary>
        /// Set factors for specified locale.
        /// </summary>
        /// <param name="factors">Factors.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected virtual void SetFactors(FactorList factors, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            Factors[locale.ISOCode + "#" + factorTreeType] = factors;
        }

        /// <summary>
        /// Set factor trees for specified locale.
        /// </summary>
        /// <param name="factorTrees">Factor trees.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected virtual void SetFactorTrees(FactorTreeNodeList factorTrees, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorTrees[locale.ISOCode + "#" + factorTreeType] = factorTrees;
        }

        /// <summary>
        /// Set factor tree nodes for specified locale.
        /// </summary>
        /// <param name="factorTreeNodes">Factor tree nodes.</param>
        /// <param name="locale">Locale.</param>
        /// <param name="getOnlyPublicFactors">Get only public factor trees or all from cache.</param>
        protected virtual void SetFactorTreeNodes(Dictionary<int, IFactorTreeNode> factorTreeNodes, ILocale locale, Boolean getOnlyPublicFactors)
        {
            String factorTreeType = getOnlyPublicFactors
                                        ? FactorTreesPermissionType.PublicFactorTrees.ToString()
                                        : FactorTreesPermissionType.AllFactorTrees.ToString();

            FactorTreeNodes[locale.ISOCode + "#" + factorTreeType] = factorTreeNodes;
        }
    }
}