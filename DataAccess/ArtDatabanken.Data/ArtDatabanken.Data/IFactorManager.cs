using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles factor related information.
    /// </summary>
    public interface IFactorManager
    {
        /// <summary>
        /// This property is used to retrieve or update factor information.
        /// </summary>
        IFactorDataSource DataSource { get; set; }

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
        IPeriod GetCurrentPublicPeriod(IUserContext userContext);

        /// <summary>
        /// Get information about default organism groups.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Information about default organism groups.</returns>
        OrganismGroupList GetDefaultOrganismGroups(IUserContext userContext);

        /// <summary>
        /// Get default individual category object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Default individual category.</returns>
        IIndividualCategory GetDefaultIndividualCategory(IUserContext userContext);

        /// <summary>
        /// Get factor data type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorDataTypeId">Factor data type id.</param>
        /// <returns>Factor data type with specified id.</returns>
        IFactorDataType GetFactorDataType(IUserContext userContext,
                                          Int32 factorDataTypeId);

        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor data types.</returns>
        FactorDataTypeList GetFactorDataTypes(IUserContext userContext);

        /// <summary>
        /// Get max number of factor fields that
        /// can be used for one factor.
        /// </summary>
        /// <returns>Max number of factor fields.</returns>
        Int32 GetFactorFieldMaxCount();

        /// <summary>
        /// Get factor field enumeration with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldEnumId">Factor field enumeration id.</param>
        /// <returns>Factor field enumeration with specified id.</returns>
        IFactorFieldEnum GetFactorFieldEnum(IUserContext userContext,
                                            FactorFieldEnumId factorFieldEnumId);

        /// <summary>
        /// Get factor field enumeration with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldEnumId">Factor field enumeration id.</param>
        /// <returns>Factor field enumeration with specified id.</returns>
        IFactorFieldEnum GetFactorFieldEnum(IUserContext userContext,
                                            Int32 factorFieldEnumId);

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field enumerations.</returns>
        FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext);

        /// <summary>
        /// Get factor field type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldTypeId">Id for requested factor field type.</param>
        /// <returns>Factor field type with specified id.</returns>
        IFactorFieldType GetFactorFieldType(IUserContext userContext,
                                            Int32 factorFieldTypeId);

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field types.</returns>
        FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext);

        /// <summary>
        /// Get factor origin with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorOriginId">Id for requested factor origin.</param>
        /// <returns>Factor origin with specified id.</returns>
        IFactorOrigin GetFactorOrigin(IUserContext userContext,
                                      Int32 factorOriginId);

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor origins.</returns>
        FactorOriginList GetFactorOrigins(IUserContext userContext);

        /// <summary>
        /// Get factor update mode with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorUpdateModeId">Id for requested factor update mode.</param>
        /// <returns>Factor update mode with specified id.</returns>
        IFactorUpdateMode GetFactorUpdateMode(IUserContext userContext,
                                              Int32 factorUpdateModeId);

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor update modes.</returns>
        FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext);

        /// <summary>
        /// Get information about organism groups of specified type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name='organismGroupType'>Type of organism groups.</param>
        /// <returns>Information about organism groups of specified type.</returns>
        OrganismGroupList GetOrganismGroups(IUserContext userContext,
                                            OrganismGroupType organismGroupType);

        /// <summary>
        /// Get period with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodId">Id for requested period.</param>
        /// <returns>Period with specified id.</returns>
        IPeriod GetPeriod(IUserContext userContext, Int32 periodId);

        /// <summary>
        /// Get period with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodId">Id for requested period.</param>
        /// <returns>Period with specified id.</returns>
        IPeriod GetPeriod(IUserContext userContext, PeriodId periodId);

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All periods.</returns>
        PeriodList GetPeriods(IUserContext userContext);

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        PeriodList GetPeriods(IUserContext userContext,
                              Int32 periodTypeId);

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        PeriodList GetPeriods(IUserContext userContext,
                              PeriodTypeId periodTypeId);

        /// <summary>
        /// Get period type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Id for requested period type.</param>
        /// <returns>Period type with specified id.</returns>
        IPeriodType GetPeriodType(IUserContext userContext,
                                  Int32 periodTypeId);

        /// <summary>
        /// Get period type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Id for requested period type.</param>
        /// <returns>Period type with specified id.</returns>
        IPeriodType GetPeriodType(IUserContext userContext,
                                  PeriodTypeId periodTypeId);

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All period types.</returns>
        PeriodTypeList GetPeriodTypes(IUserContext userContext);

        /// <summary>
        /// Get all public periods.
        /// This method selects all public periods of the
        /// default period type corresponding to Swedish Red List.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All public periods.</returns>
        PeriodList GetPublicPeriods(IUserContext userContext);

        /// <summary>
        /// Get all public periods for specified period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All public periods for specified period type.</returns>
        PeriodList GetPublicPeriods(IUserContext userContext,
                                    Int32 periodTypeId);

        /// <summary>
        /// Get all public periods for specified period type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All public periods for specified period type.</returns>
        PeriodList GetPublicPeriods(IUserContext userContext,
                                    PeriodTypeId periodTypeId);

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All individual categories.</returns>
        IndividualCategoryList GetIndividualCategories(IUserContext userContext);

        /// <summary>
        /// Get individual category with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="individualCategoryId">Individual category id.</param>
        /// <returns>Individual category with specified id.</returns>
        IIndividualCategory GetIndividualCategory(IUserContext userContext,
                                                  IndividualCategoryId individualCategoryId);

        /// <summary>
        /// Get individual category with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="individualCategoryId">Individual category id.</param>
        /// <returns>Individual category with specified id.</returns>
        IIndividualCategory GetIndividualCategory(IUserContext userContext,
                                                  Int32 individualCategoryId);

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factors.</returns>
        FactorList GetFactors(IUserContext userContext);

        /// <summary>
        /// Get factors with specified ids.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorIds">Factor ids.</param>
        /// <returns>Factors with specified ids.</returns>
        FactorList GetFactors(IUserContext userContext,
                              List<Int32> factorIds);

        /// <summary>
        /// Get factors with specified datatype.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorDataType">Factor datatype.</param>
        /// <returns>Factors with specified datatype.</returns>
        FactorList GetFactors(IUserContext userContext,
                              IFactorDataType factorDataType);

        /// <summary>
        /// Get factor with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>Factor with specified id.</returns>
        IFactor GetFactor(IUserContext userContext, FactorId factorId);

        /// <summary>
        /// Get factor with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>Factor with specified id.</returns>
        IFactor GetFactor(IUserContext userContext, Int32 factorId);

        /// <summary>
        /// Get factors that match the search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>All factors.</returns>
        FactorList GetFactors(IUserContext userContext,
                              IFactorSearchCriteria searchCriteria);

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor trees.</returns>
        FactorTreeNodeList GetFactorTrees(IUserContext userContext);

        /// <summary>
        /// Get all factor trees that match search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>All factor trees.</returns>
        FactorTreeNodeList GetFactorTrees(IUserContext userContext,
                                          IFactorTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>All factor trees.</returns>
        IFactorTreeNode GetFactorTree(IUserContext userContext,
                                      Int32 factorId);

        /// <summary>
        /// Get specified factor tree node.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorId">Factor id.</param>
        /// <returns>All factor trees.</returns>
        IFactorTreeNode GetFactorTree(IUserContext userContext,
                                      FactorId factorId);
    }
}
