using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of species fact information.
    /// </summary>
    public class SpeciesFactManager
    {
        /// <summary>
        /// Max number of species facts that are returned
        /// in one call from the client. Exception is thrown
        /// if to many species facts are requested. 
        /// </summary>
        public const Int64 MAX_SPECIES_FACTS = 1000000;

        /// <summary>
        /// Inserts a list of host ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hostIds">Id for hosts to insert.</param>
        /// <param name="hostUsage">How user selected hosts should be used.</param>
        public static void AddUserSelectedHosts(WebServiceContext context,
                                                List<Int32> hostIds,
                                                UserSelectedTaxonUsage hostUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable hostTable;

            if (hostIds.IsNotEmpty())
            {
                // Delete all hosts that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedHosts(context);

                // Insert the new list of hosts.
                hostTable = new DataTable(UserSelectedHostData.TABLE_NAME);
                column = new DataColumn(UserSelectedHostData.REQUEST_ID, typeof(Int32));
                hostTable.Columns.Add(column);
                column = new DataColumn(UserSelectedHostData.HOST_ID, typeof(Int32));
                hostTable.Columns.Add(column);
                column = new DataColumn(UserSelectedHostData.HOST_USAGE, typeof(String));
                hostTable.Columns.Add(column);
                foreach (Int32 hostId in hostIds)
                {
                    row = hostTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = hostId;
                    row[2] = hostUsage.ToString();
                    hostTable.Rows.Add(row);
                }

                DataServer.AddUserSelectedHosts(context, hostTable);
            }
        }

        /// <summary>
        /// Inserts a list of host ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userParameterSelection">All parameters selected by user</param>
        public static void AddUserSelectedParameters(WebServiceContext context,
                                                     WebUserParameterSelection userParameterSelection)
        {
            DataColumn column;
            DataRow row;
            DataTable userSelectedParameter;
            Int32 rowCount, rowIndex;

            // Get max row count.
            rowCount = 0;
            if (userParameterSelection.FactorIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.FactorIds.Count))
            {
                rowCount = userParameterSelection.FactorIds.Count;
            }
            if (userParameterSelection.HostIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.HostIds.Count))
            {
                rowCount = userParameterSelection.HostIds.Count;
            }
            if (userParameterSelection.IndividualCategoryIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.IndividualCategoryIds.Count))
            {
                rowCount = userParameterSelection.IndividualCategoryIds.Count;
            }
            if (userParameterSelection.PeriodIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.PeriodIds.Count))
            {
                rowCount = userParameterSelection.PeriodIds.Count;
            }
            if (userParameterSelection.ReferenceIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.ReferenceIds.Count))
            {
                rowCount = userParameterSelection.ReferenceIds.Count;
            }
            if (userParameterSelection.TaxonIds.IsNotEmpty() &&
                (rowCount < userParameterSelection.TaxonIds.Count))
            {
                rowCount = userParameterSelection.TaxonIds.Count;
            }

            if (rowCount > 0)
            {
                // Delete all user selected parameters that
                // belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedParameters(context);

                // Insert the new list of hosts.
                userSelectedParameter = new DataTable(UserSelectedParameterData.TABLE_NAME);
                column = new DataColumn(UserSelectedParameterData.REQUEST_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.FACTOR_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.HOST_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.INDIVIDUAL_CATEGORY_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.PERIOD_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.REFERENCE_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                column = new DataColumn(UserSelectedParameterData.TAXON_ID, typeof(Int32));
                userSelectedParameter.Columns.Add(column);
                for (rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    row = userSelectedParameter.NewRow();
                    row[0] = context.RequestId;
                    if (userParameterSelection.FactorIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.FactorIds.Count))
                    {
                        row[1] = userParameterSelection.FactorIds[rowIndex];
                    }
                    if (userParameterSelection.HostIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.HostIds.Count))
                    {
                        row[2] = userParameterSelection.HostIds[rowIndex];
                    }
                    if (userParameterSelection.IndividualCategoryIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.IndividualCategoryIds.Count))
                    {
                        row[3] = userParameterSelection.IndividualCategoryIds[rowIndex];
                    }
                    if (userParameterSelection.PeriodIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.PeriodIds.Count))
                    {
                        row[4] = userParameterSelection.PeriodIds[rowIndex];
                    }
                    if (userParameterSelection.ReferenceIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.ReferenceIds.Count))
                    {
                        row[5] = userParameterSelection.ReferenceIds[rowIndex];
                    }
                    if (userParameterSelection.TaxonIds.IsNotEmpty() &&
                        (rowIndex < userParameterSelection.TaxonIds.Count))
                    {
                        row[6] = userParameterSelection.TaxonIds[rowIndex];
                    }
                    userSelectedParameter.Rows.Add(row);
                }
                DataServer.AddUserSelectedParameters(context, userSelectedParameter);
            }
        }

        /// <summary>
        /// Delete all hosts that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedHosts(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedHosts(context);
        }

        /// <summary>
        /// Delete all user selected parameters that belong
        /// to this request from the "temporary" tables.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedParameters(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedParameters(context);
        }

        /// <summary>
        /// Get information about different endangered lists.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about endangered lists.</returns>
        public static List<WebEndangeredList> GetEndangeredLists(WebServiceContext context)
        {
            List<WebEndangeredList> endangeredLists;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllEndangeredLists";
            endangeredLists = (List<WebEndangeredList>)context.GetCachedObject(cacheKey);

            if (endangeredLists.IsNull())
            {
                // Get information from database.
                endangeredLists = new List<WebEndangeredList>();
                using (DataReader dataReader = DataServer.GetEndangeredLists(context))
                {
                    while (dataReader.Read())
                    {
                        endangeredLists.Add(new WebEndangeredList(dataReader));
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, endangeredLists, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.BelowNormal);
            }
            return endangeredLists;
        }

        /// <summary>
        /// Get all categories of species fact qualty.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species fact qualities.</returns>
        public static List<WebSpeciesFactQuality> GetSpeciesFactQualities(WebServiceContext context)
        {
            List<WebSpeciesFactQuality> speciesFactQuality;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllSpeciesFactQualities";
            speciesFactQuality = (List<WebSpeciesFactQuality>)context.GetCachedObject(cacheKey);

            if (speciesFactQuality.IsNull())
            {
                // Get information from database.
                speciesFactQuality = new List<WebSpeciesFactQuality>();
                using (DataReader dataReader = DataServer.GetSpeciesFactQualities(context))
                {
                    while (dataReader.Read())
                    {
                        speciesFactQuality.Add(new WebSpeciesFactQuality(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, speciesFactQuality, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return speciesFactQuality;
        }

        /// <summary>
        /// Inserts a list of speciesFact ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactIds">Id for speciesFacts to insert.</param>
        /// <param name="speciesFactUsage">How user selected speciesFacts should be used.</param>
        public static void AddUserSelectedSpeciesFacts(WebServiceContext context,
                                                       List<Int32> speciesFactIds,
                                                       UserSelectedSpeciesFactUsage speciesFactUsage)
        {
            DataRow row;
            DataTable speciesFactTable;

            if (speciesFactIds.IsNotEmpty())
            {
                // Delete all speciesFact ids that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedSpeciesFacts(context);

                // Insert the new list of speciesFacts.
                speciesFactTable = GetUserSelectedSpeciesFactTable();
                foreach (Int32 speciesFactId in speciesFactIds)
                {
                    row = speciesFactTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = speciesFactId;
                    row[2] = speciesFactUsage.ToString();
                    speciesFactTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedSpeciesFacts(context, speciesFactTable);
            }
        }

        /// <summary>
        /// Inserts a list of species facts by identifier into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFacts">Species facts to insert.</param>
        /// <param name="speciesFactUsage">How user selected SpeciesFacts should be used.</param>
        private static void AddUserSelectedSpeciesFacts(WebServiceContext context,
                                                        List<WebSpeciesFact> speciesFacts,
                                                        UserSelectedSpeciesFactUsage speciesFactUsage)
        {
            DataRow row;
            DataTable speciesFactTable;

            if (speciesFacts.IsNotEmpty())
            {
                // Delete all species facts that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedSpeciesFacts(context);

                // Insert the new list of species facts.
                speciesFactTable = GetUserSelectedSpeciesFactTable();
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    row = speciesFactTable.NewRow();
                    row[0] = context.RequestId;
                    row[2] = speciesFactUsage.ToString();
                    row[3] = speciesFact.FactorId;
                    row[4] = speciesFact.TaxonId;
                    row[5] = speciesFact.IndividualCategoryId;
                    if (speciesFact.IsHostSpecified)
                    {
                        row[6] = speciesFact.HostId;
                    }
                    if (speciesFact.IsPeriodSpecified)
                    {
                        row[7] = speciesFact.PeriodId;
                    }
                    speciesFactTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedSpeciesFacts(context, speciesFactTable);
            }
        }

        /// <summary>
        /// Delete all species facts that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedSpeciesFacts(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedSpeciesFacts(context);
        }

        /// <summary>
        /// Get information about speciesFacts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactIds">Ids for speciesFacts to get information about.</param>
        /// <returns>SpeciesFacts information.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsById(WebServiceContext context, List<Int32> speciesFactIds)
        {
            List<WebSpeciesFact> speciesFacts;

            // Check arguments.
            speciesFactIds.CheckNotEmpty("speciesFactIds");
            if (speciesFactIds.Count > MAX_SPECIES_FACTS)
            {
                // Exceeding max numbers of species facts that
                // can be retrieved in one request.
                throw new ArgumentException("Max " + MAX_SPECIES_FACTS + " species facts can be retrieved in one call.");
            }

            // Get data from database.
            try
            {
                AddUserSelectedSpeciesFacts(context, speciesFactIds, UserSelectedSpeciesFactUsage.Output);
                speciesFacts = new List<WebSpeciesFact>();
                using (DataReader dataReader = DataServer.GetSpeciesFactsById(context))
                {
                    while (dataReader.Read())
                    {
                        speciesFacts.Add(new WebSpeciesFact(dataReader));
                    }
                }
            }
            finally
            {
                DeleteUserSelectedSpeciesFacts(context);
            }

            if (speciesFacts.Count != speciesFactIds.Count)
            {
                // Probably invalid speciesFact ids.
                throw new ArgumentException("Invalid speciesFact ids!");
            }
            return speciesFacts;
        }

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFacts">Species facts to get information about.</param>
        /// <returns>Species facts information.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsByIdentifier(WebServiceContext context,
                                                                       List<WebSpeciesFact> speciesFacts)
        {
            List<WebSpeciesFact> outSpeciesFacts;

            // Check arguments.
            speciesFacts.CheckNotEmpty("speciesFacts");
            if (speciesFacts.Count > MAX_SPECIES_FACTS)
            {
                // Exceeding max numbers of species facts that
                // can be retrieved in one request.
                throw new ArgumentException("Max " + MAX_SPECIES_FACTS + " species facts can be retrieved in one call.");
            }

            // Get data from database.
            try
            {
                AddUserSelectedSpeciesFacts(context, speciesFacts, UserSelectedSpeciesFactUsage.Output);
                outSpeciesFacts = new List<WebSpeciesFact>();
                using (DataReader dataReader = DataServer.GetSpeciesFactsByIdentifier(context))
                {
                    while (dataReader.Read())
                    {
                        outSpeciesFacts.Add(new WebSpeciesFact(dataReader));
                    }
                }
            }
            finally
            {
                DeleteUserSelectedSpeciesFacts(context);
            }

            return outSpeciesFacts;
        }

        /// <summary>
        /// Get species facts that matches user parameter selection.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userParameterSelection">All parameters selected by user</param>
        /// <returns>Species facts.</returns>
        /// <exception cref="ArgumentException">Thrown if user parameter selection is null.</exception>
        public static List<WebSpeciesFact> GetSpeciesFactsByUserParameterSelection(WebServiceContext context,
                                                                                   WebUserParameterSelection userParameterSelection)
        {
            List<WebSpeciesFact> speciesFacts = null;

            // Check arguments.
            userParameterSelection.CheckNotNull("userParameterSelection");
            userParameterSelection.CheckData();

            // Insert species fact search parameters into database.
            AddUserSelectedParameters(context, userParameterSelection);

            // Get information about species facts from database.
            speciesFacts = new List<WebSpeciesFact>();
            using (DataReader dataReader = DataServer.GetSpeciesFactsByUserParameterSelection(context,
                                                                                              userParameterSelection.FactorIds.IsNotEmpty(),
                                                                                              userParameterSelection.TaxonIds.IsNotEmpty(),
                                                                                              userParameterSelection.IndividualCategoryIds.IsNotEmpty(),
                                                                                              userParameterSelection.PeriodIds.IsNotEmpty(),
                                                                                              userParameterSelection.HostIds.IsNotEmpty(),
                                                                                              userParameterSelection.ReferenceIds.IsNotEmpty()))
            {
                while (dataReader.Read())
                {
                    speciesFacts.Add(new WebSpeciesFact(dataReader));
                    if (speciesFacts.Count > MAX_SPECIES_FACTS)
                    {
                        // Exceeding max numbers of species facts that
                        // can be retrieved in one request.
                        throw new ApplicationException("Max " + MAX_SPECIES_FACTS + " species facts can be retrieved in one call.");
                    }
                }
            }

            return speciesFacts;
        }

        /// <summary>
        /// Get table with information about new species facts to create.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFacts">New species facts to create.</param>
        /// <param name="now">Create date and time for the new species facts.</param>
        /// <returns>Table with species fact information.</returns>
        /// <param name='fullName'>The Full name of the editor. Optinal.</param>
        public static DataTable GetSpeciesFactTable(WebServiceContext context,
                                                    List<WebSpeciesFact> speciesFacts,
                                                    DateTime now,
                                                    String fullName)
        {
            DataColumn column;
            DataRow row;
            DataTable speciesFactTable;
            Int32 nextSpeciesFactId;

            nextSpeciesFactId = DataServer.GetMaxSpeciesFactId(context) + 1;

            speciesFactTable = new DataTable(SpeciesFactData.TABLE_NAME);
            column = new DataColumn("idnr", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("faktor", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("taxon", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("individkat", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("referens", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("borttagen", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("datum", typeof(DateTime));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("person", typeof(String));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("tal1", typeof(Double));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("tal2", typeof(Double));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("tal3", typeof(Double));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("text1", typeof(String));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("text2", typeof(String));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("host", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("quality", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("period", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn("IndividualCategoryId", typeof(Int32));
            speciesFactTable.Columns.Add(column);
            foreach (WebSpeciesFact speciesFact in speciesFacts)
            {
                row = speciesFactTable.NewRow();
                row[0] = nextSpeciesFactId++;
                row[1] = speciesFact.FactorId;
                row[2] = speciesFact.TaxonId;
                if (speciesFact.IsPeriodSpecified)
                {
                    if (speciesFact.PeriodId == 3)
                    {
                        // This is period 2010.
                        // An open period has default individual
                        // category (id 0).
                        row[3] = 0;
                    }
                    else
                    {
                        row[3] = speciesFact.PeriodId + 18;
                    }
                }
                else
                {
                    row[3] = speciesFact.IndividualCategoryId;
                }
                row[4] = speciesFact.ReferenceId;
                row[5] = 0;
                row[6] = now.Date;

                if (fullName.IsEmpty())
                {
                    row[7] = WebServiceData.UserManager.GetPerson(context).GetFullName();
                    
                }
                else
                {
                    row[7] = fullName;
                }
                if (speciesFact.IsFieldValue1Specified)
                {
                    row[8] = speciesFact.FieldValue1;
                }
                if (speciesFact.IsFieldValue2Specified)
                {
                    row[9] = speciesFact.FieldValue2;
                }
                if (speciesFact.IsFieldValue3Specified)
                {
                    row[10] = speciesFact.FieldValue3;
                }
                if (speciesFact.IsFieldValue4Specified)
                {
                    row[11] = speciesFact.FieldValue4;
                }
                if (speciesFact.IsFieldValue5Specified)
                {
                    row[12] = speciesFact.FieldValue5;
                }
                if (speciesFact.IsHostSpecified)
                {
                    row[13] = speciesFact.HostId;
                }
                else
                {
                    row[13] = 0;
                }
                row[14] = speciesFact.QualityId;
                if (speciesFact.IsPeriodSpecified)
                {
                    row[15] = speciesFact.PeriodId;
                }
                row[16] = speciesFact.IndividualCategoryId;
                speciesFactTable.Rows.Add(row);
            }

            return speciesFactTable;
        }

        /// <summary>
        /// Get information about occurence in swedish
        /// counties for specified taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Information about occurence in swedish counties for specified taxon.</returns>
        public static List<WebTaxonCountyOccurrence> GetTaxonCountyOccurence(WebServiceContext context,
                                                                               Int32 taxonId)
        {
            List<WebTaxonCountyOccurrence> countyOccurrencies;

            countyOccurrencies = new List<WebTaxonCountyOccurrence>();
            using (DataReader dataReader = DataServer.GetTaxonCountyOccurrence(context, taxonId))
            {
                while (dataReader.Read())
                {
                    countyOccurrencies.Add(new WebTaxonCountyOccurrence(dataReader));
                }
            }
            return countyOccurrencies;
        }

        /// <summary>
        /// Get table structure for table where user
        /// selected species facts are stored.
        /// </summary>
        /// <returns>Table structure.</returns>
        private static DataTable GetUserSelectedSpeciesFactTable()
        {
            DataColumn column;
            DataTable speciesFactTable;

            speciesFactTable = new DataTable(UserSelectedSpeciesFactData.TABLE_NAME);
            column = new DataColumn(UserSelectedSpeciesFactData.REQUEST_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.SPECIES_FACT_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.SPECIES_FACT_USAGE, typeof(String));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.FACTOR_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.TAXON_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.INDIVIDUAL_CATEGORY_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.HOST_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.PERIOD_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);

            return speciesFactTable;
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public static void UpdateSpeciesFacts(WebServiceContext context,
                                              List<WebSpeciesFact> createSpeciesFacts,
                                              List<WebSpeciesFact> deleteSpeciesFacts,
                                              List<WebSpeciesFact> updateSpeciesFacts)
        {
            DataTable speciesFactTable;
            DateTime now = DateTime.Now;
            List<Int32> speciesFactIds;
            String fullName;

            // Check arguments.
            context.CheckTransaction();
            WebServiceData.AuthorizationManager.CheckAuthorization(context, ApplicationIdentifier.EVA, AuthorityIdentifier.EditSpeciesFacts);
            if (createSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in createSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }
            if (deleteSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }
            if (updateSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in updateSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }

            if (createSpeciesFacts.IsNotEmpty())
            {
                // Create species facts.
                speciesFactTable = GetSpeciesFactTable(context, createSpeciesFacts, now, "");
                DataServer.CreateSpeciesFacts(context, speciesFactTable);
            }

            fullName = WebServiceData.UserManager.GetPerson(context).GetFullName();
            if (deleteSpeciesFacts.IsNotEmpty())
            {
                try
                {
                    // Update species facts.
                    foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                    {
                        DataServer.UpdateSpeciesFact(context,
                                                     speciesFact.Id,
                                                     speciesFact.ReferenceId,
                                                     now,
                                                     fullName,
                                                     speciesFact.IsFieldValue1Specified,
                                                     speciesFact.FieldValue1,
                                                     speciesFact.IsFieldValue2Specified,
                                                     speciesFact.FieldValue2,
                                                     speciesFact.IsFieldValue3Specified,
                                                     speciesFact.FieldValue3,
                                                     speciesFact.FieldValue4,
                                                     speciesFact.FieldValue5,
                                                     speciesFact.QualityId);
                    }

                    // Delete species facts.
                    speciesFactIds = new List<Int32>();
                    foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                    {
                        speciesFactIds.Add(speciesFact.Id);
                    }
                    DeleteUserSelectedSpeciesFacts(context);
                    AddUserSelectedSpeciesFacts(context,
                                                speciesFactIds,
                                                UserSelectedSpeciesFactUsage.Output);
                    DataServer.DeleteSpeciesFacts(context);
                }
                finally
                {
                    DeleteUserSelectedSpeciesFacts(context);
                }
            }

            if (updateSpeciesFacts.IsNotEmpty())
            {
                // Update species facts.
                foreach (WebSpeciesFact speciesFact in updateSpeciesFacts)
                {
                    DataServer.UpdateSpeciesFact(context,
                                                 speciesFact.Id,
                                                 speciesFact.ReferenceId,
                                                 now,
                                                 fullName,
                                                 speciesFact.IsFieldValue1Specified,
                                                 speciesFact.FieldValue1,
                                                 speciesFact.IsFieldValue2Specified,
                                                 speciesFact.FieldValue2,
                                                 speciesFact.IsFieldValue3Specified,
                                                 speciesFact.FieldValue3,
                                                 speciesFact.FieldValue4,
                                                 speciesFact.FieldValue5,
                                                 speciesFact.QualityId);
                }
            }
        }

        /// <summary>
        /// Update species facts. This method should only be used by Dyntaxa web application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        /// <param name="fullName">Full Name of editor.</param>
        public static void UpdateDyntaxaSpeciesFacts(WebServiceContext context,
                                              List<WebSpeciesFact> createSpeciesFacts,
                                              List<WebSpeciesFact> deleteSpeciesFacts,
                                              List<WebSpeciesFact> updateSpeciesFacts,
                                              String fullName)
        {
            DataTable speciesFactTable;
            DateTime now = DateTime.Now;
            List<Int32> speciesFactIds;
            
            // Check arguments.
            context.CheckTransaction();
            WebServiceData.AuthorizationManager.CheckAuthorization(context, ApplicationIdentifier.Dyntaxa, AuthorityIdentifier.EditSpeciesFacts);
            if (createSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in createSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }
            if (deleteSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }
            if (updateSpeciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in updateSpeciesFacts)
                {
                    speciesFact.CheckData(context);
                }
            }

            if (createSpeciesFacts.IsNotEmpty())
            {
                // Create species facts.
                speciesFactTable = GetSpeciesFactTable(context, createSpeciesFacts, now, fullName);
                DataServer.CreateSpeciesFacts(context, speciesFactTable);
            }

            if (deleteSpeciesFacts.IsNotEmpty())
            {
                try
                {
                    // Update species facts.
                    foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                    {
                        DataServer.UpdateSpeciesFact(context,
                                                     speciesFact.Id,
                                                     speciesFact.ReferenceId,
                                                     now,
                                                     fullName,
                                                     speciesFact.IsFieldValue1Specified,
                                                     speciesFact.FieldValue1,
                                                     speciesFact.IsFieldValue2Specified,
                                                     speciesFact.FieldValue2,
                                                     speciesFact.IsFieldValue3Specified,
                                                     speciesFact.FieldValue3,
                                                     speciesFact.FieldValue4,
                                                     speciesFact.FieldValue5,
                                                     speciesFact.QualityId);
                    }

                    // Delete species facts.
                    speciesFactIds = new List<Int32>();
                    foreach (WebSpeciesFact speciesFact in deleteSpeciesFacts)
                    {
                        speciesFactIds.Add(speciesFact.Id);
                    }
                    DeleteUserSelectedSpeciesFacts(context);
                    AddUserSelectedSpeciesFacts(context,
                                                speciesFactIds,
                                                UserSelectedSpeciesFactUsage.Output);
                    DataServer.DeleteSpeciesFacts(context);
                }
                finally
                {
                    DeleteUserSelectedSpeciesFacts(context);
                }
            }

            if (updateSpeciesFacts.IsNotEmpty())
            {
                // Update species facts.
                foreach (WebSpeciesFact speciesFact in updateSpeciesFacts)
                {
                    DataServer.UpdateSpeciesFact(context,
                                                 speciesFact.Id,
                                                 speciesFact.ReferenceId,
                                                 now,
                                                 fullName,
                                                 speciesFact.IsFieldValue1Specified,
                                                 speciesFact.FieldValue1,
                                                 speciesFact.IsFieldValue2Specified,
                                                 speciesFact.FieldValue2,
                                                 speciesFact.IsFieldValue3Specified,
                                                 speciesFact.FieldValue3,
                                                 speciesFact.FieldValue4,
                                                 speciesFact.FieldValue5,
                                                 speciesFact.QualityId);
                }
            }
        }
    }
}
