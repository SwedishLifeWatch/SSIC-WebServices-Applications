using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor.
    /// </summary>
    public interface IFactor : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Factor data type for this factor.
        /// </summary>
        IFactorDataType DataType { get; set; }

        /// <summary>
        /// Taxon id for parent taxon for all potential hosts associated with this factor.
        /// </summary>
        Int32 DefaultHostParentTaxonId { get; set; }

        /// <summary>
        /// Host label for this factor.
        /// </summary>
        String HostLabel { get; set; }

        /// <summary>
        /// Information for this factor.
        /// </summary>
        String Information { get; set; }

        /// <summary>
        /// Indicates if this factor is a leaf in the factor tree.
        /// </summary>
        Boolean IsLeaf { get; set; }

        /// <summary>
        /// Indication about whether or not this factor is periodic.
        /// </summary>
        Boolean IsPeriodic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor should be available for public use.
        /// </summary>
        Boolean IsPublic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor can be associated with a host taxon.
        /// </summary>
        Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Factor origin for this factor.
        /// </summary>
        IFactorOrigin Origin { get; set; }

        /// <summary>
        /// Label for this factor.
        /// </summary>
        String Label { get; set; }

        /// <summary>
        /// Name for this factor.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Sort order for this factor.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Factor update mode for this factor.
        /// </summary>
        IFactorUpdateMode UpdateMode { get; set; }

        /// <summary>
        /// Get all factors that have an impact on this factors value.
        /// Only factors that are automatically calculated
        /// has dependent factors.
        /// </summary>
        /// <param name="userContext">Information about the user that makes this method call.</param>
        /// <returns>Dependent factors.</returns>
        FactorList GetDependentFactors(IUserContext userContext);

        /// <summary>
        /// Get the factor tree node for this factor.
        /// </summary>
        /// <param name="userContext">Information about the user that makes this method call.</param>
        /// <returns>Tree where this factor is top node.</returns>
        IFactorTreeNode GetFactorTree(IUserContext userContext);

        /// <summary>
        /// Get a text string with the basic information about the 
        /// factor that can be presented in applications as a hint text.
        /// </summary>
        /// <returns>The hint text.</returns>
        String GetHint();
    }
}