using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles factor related information.
    /// </summary>
    public class FactorManager : IFactorManager
    {
        /// <summary>
        /// This property is used to retrieve or update factor information.
        /// </summary>
        public IFactorDataSource DataSource { get; set; }

        /// <summary>
        /// Store two different factor trees in cache. One with only public factors and one with all factors.
        /// </summary>
        public enum FactorTreesPermissionType
        {
            PublicFactorTrees,
            AllFactorTrees
        }

        public enum UserRoleAuthorityIdentifierType
        {
            SpeciesFact
        }

        /// <summary>
        /// A method that retrieves a period object
        /// representing the current public period.
        /// This method only handles the default period type, i.e. 
        /// The Swedish Red List.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>The current public period.</returns>
        public virtual IPeriod GetCurrentPublicPeriod(IUserContext userContext)
        {
            IPeriod currentPublicPeriod;

            currentPublicPeriod = null;
            foreach (IPeriod period in GetPeriods(userContext, PeriodTypeId.SwedishRedList))
            {
                if (period.StopUpdates < DateTime.Today)
                {
                    if (currentPublicPeriod.IsNull() ||
                        // ReSharper disable once PossibleNullReferenceException
                        (currentPublicPeriod.StopUpdates < period.StopUpdates))
                    {
                        currentPublicPeriod = period;
                    }
                }
            }

            return currentPublicPeriod;
        }

        /// <summary>
        /// Get default individual category object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Default individual category.</returns>
        public virtual IIndividualCategory GetDefaultIndividualCategory(IUserContext userContext)
        {
            return GetIndividualCategory(userContext,
                                         IndividualCategoryId.Default);
        }

        /// <summary>
        /// Get information about default organism groups.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Information about default organism groups.</returns>
        public virtual OrganismGroupList GetDefaultOrganismGroups(IUserContext userContext)
        {
            return GetOrganismGroups(userContext,
                                     OrganismGroupType.Standard);
        }

        /// <summary>
        /// Get factor data type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorDataTypeId">Factor data type id.</param>
        /// <returns>Factor data type with specified id.</returns>
        public virtual IFactorDataType GetFactorDataType(IUserContext userContext,
                                                         Int32 factorDataTypeId)
        {
            return GetFactorDataTypes(userContext).Get(factorDataTypeId);
        }

        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor data types.</returns>
        public virtual FactorDataTypeList GetFactorDataTypes(IUserContext userContext)
        {
            return DataSource.GetFactorDataTypes(userContext);
        }

        /// <summary>
        /// Get max number of factor fields that
        /// can be used for one factor.
        /// </summary>
        /// <returns>Max number of factor fields.</returns>
        public virtual Int32 GetFactorFieldMaxCount()
        {
            return Settings.Default.FactorFieldMaxCount;
        }

        /// <summary>
        /// Get factor field enumeration with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldEnumId">Factor field enumeration id.</param>
        /// <returns>Factor field enumeration with specified id.</returns>
        public virtual IFactorFieldEnum GetFactorFieldEnum(IUserContext userContext,
                                                           FactorFieldEnumId factorFieldEnumId)
        {
            return GetFactorFieldEnum(userContext, (Int32)factorFieldEnumId);
        }

        /// <summary>
        /// Get factor field enumeration with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldEnumId">Factor field enumeration id.</param>
        /// <returns>Factor field enumeration with specified id.</returns>
        public virtual IFactorFieldEnum GetFactorFieldEnum(IUserContext userContext,
                                                           Int32 factorFieldEnumId)
        {
            return DataSource.GetFactorFieldEnums(userContext).Get(factorFieldEnumId);
        }

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field enumerations.</returns>
        public virtual FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext)
        {
            return DataSource.GetFactorFieldEnums(userContext);
        }

        /// <summary>
        /// Get factor field type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldTypeId">Id for requested factor field type.</param>
        /// <returns>Factor field type with specified id.</returns>
        public virtual IFactorFieldType GetFactorFieldType(IUserContext userContext,
                                                           Int32 factorFieldTypeId)
        {
            return GetFactorFieldTypes(userContext).Get(factorFieldTypeId);
        }

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field types.</returns>
        public virtual FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext)
        {
            return DataSource.GetFactorFieldTypes(userContext);
        }

        /// <summary>
        /// Get factor origin with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorOriginId">Id for requested factor origin.</param>
        /// <returns>Factor origin with specified id.</returns>
        public virtual IFactorOrigin GetFactorOrigin(IUserContext userContext,
                                                     Int32 factorOriginId)
        {
            return GetFactorOrigins(userContext).Get(factorOriginId);
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor origins.</returns>
        public virtual FactorOriginList GetFactorOrigins(IUserContext userContext)
        {
            return DataSource.GetFactorOrigins(userContext);
        }

        /// <summary>
        /// Get factor update mode with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorUpdateModeId">Id for requested factor update mode.</param>
        /// <returns>All factor update modes.</returns>
        public virtual IFactorUpdateMode GetFactorUpdateMode(IUserContext userContext,
                                                             Int32 factorUpdateModeId)
        {
            return GetFactorUpdateModes(userContext).Get(factorUpdateModeId);
        }

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor update modes.</returns>
        public virtual FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext)
        {
            return DataSource.GetFactorUpdateModes(userContext);
        }

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All individual categories.</returns>
        public virtual IndividualCategoryList GetIndividualCategories(IUserContext userContext)
        {
            return DataSource.GetIndividualCategories(userContext);
        }

        /// <summary>
        /// Get individual category with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="individualCategoryId">Individual category id.</param>
        /// <returns>Individual category with specified id.</returns>
        public virtual IIndividualCategory GetIndividualCategory(IUserContext userContext,
                                                                 IndividualCategoryId individualCategoryId)
        {
            return GetIndividualCategory(userContext, (Int32)individualCategoryId);
        }

        /// <summary>
        /// Get individual category with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="individualCategoryId">Individual category id.</param>
        /// <returns>Individual category with specified id.</returns>
        public virtual IIndividualCategory GetIndividualCategory(IUserContext userContext,
                                                                 Int32 individualCategoryId)
        {
            return GetIndividualCategories(userContext).Get(individualCategoryId);
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factors.</returns>
        public virtual FactorList GetFactors(IUserContext userContext)
        {
            return DataSource.GetFactors(userContext);
        }

        /// <summary>
        /// Get factors with specified ids.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorIds">Factor ids.</param>
        /// <returns>Factors with specified ids.</returns>
        public virtual FactorList GetFactors(IUserContext userContext,
                                             List<Int32> factorIds)
        {
            FactorList allFactors, factors;

            // Check arguments.
            factorIds.CheckNotEmpty("factorIds");

            factors = new FactorList();
            allFactors = GetFactors(userContext);
            foreach (Int32 factorId in factorIds)
            {
                factors.Add(allFactors.Get(factorId));
            }

            return factors;
        }

        /// <summary>
        /// Get factors with specified datatype.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorDataType">Factor datatype.</param>
        /// <returns>Factors with specified datatype.</returns>
        public virtual FactorList GetFactors(IUserContext userContext, IFactorDataType factorDataType)
        {
            FactorList allFactors, factors;

            // Check arguments.
            factorDataType.CheckNotNull("factorDataType");

            allFactors = GetFactors(userContext);
            factors = new FactorList();
            factors.AddRange(allFactors.FindAll(factor => factor.DataType.IsNotNull() && factor.DataType.Id == factorDataType.Id));
            return factors;
        }

        /// <summary>
        /// Get factor with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>Factor with specified id.</returns>
        public virtual IFactor GetFactor(IUserContext userContext,
                                         FactorId factorId)
        {
            return GetFactor(userContext, (Int32)factorId);
        }

        /// <summary>
        /// Get factor with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Id for requested factor.</param>
        /// <returns>Factor with specified id.</returns>
        public virtual IFactor GetFactor(IUserContext userContext,
                                         Int32 factorId)
        {
            return GetFactors(userContext).Get(factorId);
        }

        /// <summary>
        /// Get factors that match the search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>All factors.</returns>
        public virtual FactorList GetFactors(IUserContext userContext,
                                             IFactorSearchCriteria searchCriteria)
        {
            return DataSource.GetFactors(userContext, searchCriteria);
        }

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor trees.</returns>
        public virtual FactorTreeNodeList GetFactorTrees(IUserContext userContext)
        {
            return DataSource.GetFactorTrees(userContext);
        }

        /// <summary>
        /// Get all factor trees that match search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>All factor trees.</returns>
        public virtual FactorTreeNodeList GetFactorTrees(IUserContext userContext,
                                                         IFactorTreeSearchCriteria searchCriteria)
        {
            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            return DataSource.GetFactorTrees(userContext, searchCriteria);
        }

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>All factor trees.</returns>
        public virtual IFactorTreeNode GetFactorTree(IUserContext userContext,
                                                     Int32 factorId)
        {
            FactorTreeNodeList factorTrees;
            IFactorTreeNode factorTree;
            IFactorTreeSearchCriteria searchCriteria;

            searchCriteria = new FactorTreeSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(factorId);
            factorTrees = GetFactorTrees(userContext, searchCriteria);
            factorTree = null;
            if (factorTrees.IsNotEmpty())
            {
                factorTree = factorTrees[0];
            }

            return factorTree;
        }

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>All factor trees.</returns>
        public virtual IFactorTreeNode GetFactorTree(IUserContext userContext,
                                                     FactorId factorId)
        {
            return GetFactorTree(userContext, (Int32)factorId);
        }

        /// <summary>
        /// Get information about organism groups of specified type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        /// <returns>Information about organism groups of specified type.</returns>
        public virtual OrganismGroupList GetOrganismGroups(IUserContext userContext,
                                                           OrganismGroupType organismGroupType)
        {
            IOrganismGroup organismGroup;
            OrganismGroupList organismGroups;

            organismGroups = new OrganismGroupList();
            foreach (FactorFieldEnumValue enumValue in GetFactorFieldEnum(userContext, FactorFieldEnumId.OrganismGroup).Values)
            {
                organismGroup = new OrganismGroup();
                organismGroup.DataContext = enumValue.DataContext;
                organismGroup.Definition = enumValue.Information;
                // ReSharper disable once PossibleInvalidOperationException
                organismGroup.Id = enumValue.KeyInt.Value;
                organismGroup.Name = enumValue.OriginalLabel;
                organismGroup.SortOrder = enumValue.SortOrder;
                organismGroup.Type = organismGroupType;
                organismGroups.Add(organismGroup);
            }

            organismGroups.Sort();
            return organismGroups;
        }

        /// <summary>
        /// Get period with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodId">Id for requested period.</param>
        /// <returns>Period with specified id.</returns>
        public virtual IPeriod GetPeriod(IUserContext userContext,
                                         Int32 periodId)
        {
            return GetPeriods(userContext).Get(periodId);
        }

        /// <summary>
        /// Get period with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodId">Id for requested period.</param>
        /// <returns>Period with specified id.</returns>
        public virtual IPeriod GetPeriod(IUserContext userContext,
                                         PeriodId periodId)
        {
            return GetPeriod(userContext, (Int32)periodId);
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All periods.</returns>
        public virtual PeriodList GetPeriods(IUserContext userContext)
        {
            return DataSource.GetPeriods(userContext);
        }

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        public virtual PeriodList GetPeriods(IUserContext userContext,
                                             Int32 periodTypeId)
        {
            return GetPeriods(userContext).GetPeriods(periodTypeId);
        }

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        public virtual PeriodList GetPeriods(IUserContext userContext,
                                             PeriodTypeId periodTypeId)
        {
            return GetPeriods(userContext).GetPeriods(periodTypeId);
        }

        /// <summary>
        /// Get period type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Id for requested period type.</param>
        /// <returns>Period type with specified id.</returns>
        public virtual IPeriodType GetPeriodType(IUserContext userContext,
                                                 Int32 periodTypeId)
        {
            return GetPeriodTypes(userContext).Get(periodTypeId);
        }

        /// <summary>
        /// Get period type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Id for requested period type.</param>
        /// <returns>Period type with specified id.</returns>
        public virtual IPeriodType GetPeriodType(IUserContext userContext,
                                                 PeriodTypeId periodTypeId)
        {
            return GetPeriodType(userContext, (Int32)periodTypeId);
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All period types.</returns>
        public virtual PeriodTypeList GetPeriodTypes(IUserContext userContext)
        {
            return DataSource.GetPeriodTypes(userContext);
        }

        /// <summary>
        /// Get all public periods.
        /// This method selects all public periods of the
        /// default period type corresponding to Swedish Red List.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All public periods.</returns>
        public virtual PeriodList GetPublicPeriods(IUserContext userContext)
        {
            return GetPublicPeriods(userContext, PeriodTypeId.SwedishRedList);
        }

        /// <summary>
        /// Get all public periods for specified period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All public periods for specified period type.</returns>
        public virtual PeriodList GetPublicPeriods(IUserContext userContext,
                                                   Int32 periodTypeId)
        {
            PeriodList publicPeriods;

            publicPeriods = new PeriodList();
            foreach (IPeriod period in GetPeriods(userContext, periodTypeId))
            {
                if (period.StopUpdates < DateTime.Today)
                {
                    publicPeriods.Add(period);
                }
            }

            return publicPeriods;
        }

        /// <summary>
        /// Get all public periods for specified period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All public periods for specified period type.</returns>
        public virtual PeriodList GetPublicPeriods(IUserContext userContext,
                                                   PeriodTypeId periodTypeId)
        {
            return GetPublicPeriods(userContext, (Int32)periodTypeId);
        }
    }
}
