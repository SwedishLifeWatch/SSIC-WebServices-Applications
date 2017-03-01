using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about an application version.
    /// </summary>
    public interface IApplicationVersion : IDataId32
    {
        /// <summary>
        /// ApplicationId
        /// </summary>
        Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// IsRecommended
        /// </summary>
        Boolean IsRecommended
        { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        Boolean IsValid
        { get; set; }    

        /// <summary>
        /// Date application is valid from. Not Null. Is set to date created by default.
        /// </summary>
        DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date application is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        DateTime ValidToDate
        { get; set; }

         /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

         /// <summary>
        /// Information about create/update of organization.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

        /// <summary>
        /// Version
        /// </summary>
        String Version
        { get; set; }
    }
}
