using System.Collections.Generic;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// This interface is used to handle factor related information.
    /// </summary>
    public interface IFactorDataSource : IDataSource
    {
        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor data types.</returns>
        FactorDataTypeList GetFactorDataTypes(IUserContext userContext);

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field enumerations.</returns>
        FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext);

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field types.</returns>
        FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext);

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor origins.</returns>
        FactorOriginList GetFactorOrigins(IUserContext userContext);

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factors.</returns>
        FactorList GetFactors(IUserContext userContext);

        /// <summary>
        /// Get factors that matches the search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Filtered factors.</returns>
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
        /// Get all factor update modes.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor update modes.</returns>
        FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext);

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All individual categories.</returns>
        IndividualCategoryList GetIndividualCategories(IUserContext userContext);

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All periods.</returns>
        PeriodList GetPeriods(IUserContext userContext);

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All period types.</returns>
        PeriodTypeList GetPeriodTypes(IUserContext userContext);
    }
}
