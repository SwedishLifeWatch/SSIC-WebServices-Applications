using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebReference class.
    /// </summary>
    public static class WebReferenceExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="reference">The reference instance.</param>
        /// <param name="context">Web service request context.</param>
        public static void CheckData(this WebReference reference,
                                     WebServiceContext context)
        {
            reference.CheckNotNull("reference");
            reference.Name.CheckNotEmpty("Name");
            reference.Name = reference.Name.CheckSqlInjection();
            reference.Name.CheckLength(GetNameMaxLength(context));
            reference.Title.CheckNotEmpty("Title");
            reference.Title = reference.Title.CheckSqlInjection();
            reference.Title.CheckLength(GetTitleMaxLength(context));
        }

        /// <summary>
        /// Get max string length for name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for name.</returns>
        private static Int32 GetNameMaxLength(WebServiceContext context)
        {
            return context.GetDatabase().GetColumnLength(ReferenceData.TABLE_NAME,
                                                         ReferenceData.NAME_COLUMN_NAME);
        }

        /// <summary>
        /// Get max string length for title.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for title.</returns>
        private static Int32 GetTitleMaxLength(WebServiceContext context)
        {
            return context.GetDatabase().GetColumnLength(ReferenceData.TABLE_NAME,
                                                         ReferenceData.TITLE_COLUMN_NAME);
        }

        /// <summary>
        /// Load data into the WebReference instance.
        /// </summary>
        /// <param name="reference">The reference instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebReference reference,
                                    DataReader dataReader)
        {
            reference.Id = dataReader.GetInt32(ReferenceData.ID);
            reference.IsModifiedDateSpecified = dataReader.IsNotDbNull(ReferenceData.MODIFIED_DATE);
            reference.IsYearSpecified = dataReader.IsNotDbNull(ReferenceData.YEAR);
            reference.ModifiedBy = dataReader.GetString(ReferenceData.MODIFIED_BY);
            if (reference.IsModifiedDateSpecified)
            {
                reference.ModifiedDate = dataReader.GetDateTime(ReferenceData.MODIFIED_DATE);
            }

            reference.Name = dataReader.GetString(ReferenceData.NAME);
            reference.Title = dataReader.GetString(ReferenceData.TITLE);
            if (reference.IsYearSpecified)
            {
                reference.Year = dataReader.GetInt32(ReferenceData.YEAR);
            }
        }
    }
}
