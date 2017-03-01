using System;
using System.Collections.Generic;
using System.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Handle references.
    /// </summary>
    public class ReferenceManager
    {
        /// <summary>
        /// Inserts a list of reference ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceIds">Id for references to insert.</param>
        /// <param name="referenceUsage">How user selected references should be used.</param>
        public static void AddUserSelectedReferences(WebServiceContext context,
                                                     List<Int32> referenceIds,
                                                     UserSelectedReferenceUsage referenceUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable referenceTable;

            if (referenceIds.IsNotEmpty())
            {
                // Delete all references that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedReferences(context);

                // Insert the new list of references.
                referenceTable = new DataTable(UserSelectedReferenceData.TABLE_NAME);
                column = new DataColumn(UserSelectedReferenceData.REQUEST_ID, typeof(Int32));
                referenceTable.Columns.Add(column);
                column = new DataColumn(UserSelectedReferenceData.REFERENCE_ID, typeof(Int32));
                referenceTable.Columns.Add(column);
                column = new DataColumn(UserSelectedReferenceData.REFERENCE_USAGE, typeof(String));
                referenceTable.Columns.Add(column);
                foreach (Int32 referenceId in referenceIds)
                {
                    row = referenceTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = referenceId;
                    row[2] = referenceUsage.ToString();
                    referenceTable.Rows.Add(row);
                }

                DataServer.AddUserSelectedReferences(context, referenceTable);
            }
        }

        /// <summary>
        /// Delete all references that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedReferences(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedReferences(context);
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All references.</returns>
        public static List<WebReference> GetReferences(WebServiceContext context)
        {
            List<WebReference> references;

            references = new List<WebReference>();
            using (DataReader dataReader = DataServer.GetReferences(context))
            {
                while (dataReader.Read())
                {
                    references.Add(new WebReference(dataReader));
                }
            }

            return references;
        }

        /// <summary>
        /// Get all references that matches search string.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchString">Search string.</param>
        /// <returns>All references that matches search string.</returns>
        public static List<WebReference> GetReferencesBySearchString(WebServiceContext context, String searchString)
        {
            List<WebReference> references;

            references = new List<WebReference>();
            using (DataReader dataReader = DataServer.GetReferencesBySearchString(context, searchString))
            {
                while (dataReader.Read())
                {
                    references.Add(new WebReference(dataReader));
                }
            }

            return references;
        }

        /// <summary>
        /// Update reference with specific Id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reference">Existing reference to insert.</param>
        public static void UpdateReference(WebServiceContext context, WebReference reference)
        {
            // Check arguments.
            context.CheckTransaction();
            WebServiceData.AuthorizationManager.CheckAuthorization(context, ApplicationIdentifier.EVA, AuthorityIdentifier.EditSpeciesFacts);
            reference.CheckData(context);

            DataServer.UpdateReference(context,
                                       reference.Id,
                                       reference.Name,
                                       reference.Year,
                                       reference.Text,
                                       WebServiceData.UserManager.GetPerson(context).GetFullName());
        }
        
        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reference">New reference to create.</param>
        public static void CreateReference(WebServiceContext context, WebReference reference)
        {
            // Check arguments.
            context.CheckTransaction();
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditSpeciesFacts);
            reference.CheckData(context);
            string userFullName;
            if (context.GetUser().Type == ArtDatabanken.Data.UserType.Person)
            {
                userFullName = WebServiceData.UserManager.GetPerson(context).GetFullName();
            }
            else
            {
                userFullName = context.GetUser().UserName;
            }

            DataServer.CreateReference(context,
                                       reference.Name,
                                       reference.Year,
                                       reference.Text,
                                       userFullName);
        }
    }
}
