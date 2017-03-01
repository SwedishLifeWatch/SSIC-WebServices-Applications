using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Handles species fact information.
    /// </summary>
    public static class SpeciesFactManager
    {
        /// <summary>
        /// Indicates if species fact related information has been changed.
        /// </summary>
        private static Boolean _isSpeciesFactInformationUpdated;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SpeciesFactManager()
        {
            _isSpeciesFactInformationUpdated = false;
            WebServiceContext.CommitTransactionEvent += RemoveCachedObjects;
        }

        /// <summary>
        /// Create species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        public static void CreateSpeciesFacts(WebServiceContext context,
                                              List<WebSpeciesFact> createSpeciesFacts)
        {
            DataTable speciesFactTable;
            DateTime now = DateTime.Now;

            // Check transaction.
            context.CheckTransaction();

            // Check authorization.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditSpeciesFacts);

            // Check arguments.
            createSpeciesFacts.CheckData();

            if (createSpeciesFacts.IsNotEmpty())
            {
                // Create species facts.
                speciesFactTable = GetSpeciesFactCreateTable(context, createSpeciesFacts, now, String.Empty);
                context.GetTaxonAttributeDatabase().CreateSpeciesFacts(speciesFactTable);
            }
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        public static void DeleteSpeciesFacts(WebServiceContext context,
                                              List<WebSpeciesFact> deleteSpeciesFacts)
        {
            DataTable speciesFactTable;
            List<Int32> speciesFactIds;

            // Check transaction.
            context.CheckTransaction();

            // Check authorization.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditSpeciesFacts);

            // Check arguments.
            deleteSpeciesFacts.CheckData();

            if (deleteSpeciesFacts.IsNotEmpty())
            {
                // Update species facts.
                speciesFactTable = GetSpeciesFactUpdateTable(context, deleteSpeciesFacts);
                context.GetTaxonAttributeDatabase().UpdateSpeciesFacts(speciesFactTable);

                // Delete species facts.
                speciesFactIds = deleteSpeciesFacts.GetIds();
                context.GetTaxonAttributeDatabase().DeleteSpeciesFactsByIds(speciesFactIds);
            }
        }

        /// <summary>
        /// Get table with information about new species facts to create.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFacts">New species facts to create.</param>
        /// <param name="now">Create date and time for the new species facts.</param>
        /// <param name="fullName">The Full name of the editor. Optional.</param>
        /// <returns>Table with species fact information.</returns>
        public static DataTable GetSpeciesFactCreateTable(WebServiceContext context,
                                                          List<WebSpeciesFact> speciesFacts,
                                                          DateTime now,
                                                          String fullName)
        {
            DataTable speciesFactTable = GetSpeciesFactTable();

            if (speciesFacts.IsNotEmpty())
            {
                int nextSpeciesFactId = context.GetTaxonAttributeDatabase().GetMaxSpeciesFactId() + 1;
                DataRow row;
                WebPerson person = null;

                if (fullName.IsEmpty())
                {
                    person = WebServiceData.UserManager.GetPerson(context);
                }

                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    row = speciesFactTable.NewRow();
                    row[0] = nextSpeciesFactId++;
                    row[1] = speciesFact.FactorId;
                    row[2] = speciesFact.TaxonId;
                    row[3] = speciesFact.IsPeriodSpecified
                                            ? (speciesFact.PeriodId == 3 ? 0 : speciesFact.PeriodId + 18)
                                            : speciesFact.IndividualCategoryId;
                    row[4] = speciesFact.ReferenceId;
                    row[5] = 0;
                    row[6] = now.Date;
                    // ReSharper disable once PossibleNullReferenceException
                    row[7] = person.IsNotNull() ? person.FirstName + " " + person.LastName : fullName;
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

                    row[13] = speciesFact.IsHostSpecified ? speciesFact.HostId : 0;
                    row[14] = speciesFact.QualityId;
                    if (speciesFact.IsPeriodSpecified)
                    {
                        row[15] = speciesFact.PeriodId;
                    }

                    row[16] = speciesFact.IndividualCategoryId;
                    speciesFactTable.Rows.Add(row);
                }
            }

            return speciesFactTable;
        }

        /// <summary>
        /// Get species fact identifier DataTable.
        /// </summary>
        /// <returns>A species fact identifier DataTable.</returns>
        public static DataTable GetSpeciesFactIdentifiersTable()
        {
            DataTable speciesFactIdentifiers = new DataTable("SpeciesFactIdentifiers");

            speciesFactIdentifiers.Columns.Add("TaxonId", typeof(int)).AllowDBNull = false;
            speciesFactIdentifiers.Columns.Add("FactorId", typeof(int)).AllowDBNull = false;
            speciesFactIdentifiers.Columns.Add("IndividualCategoryId", typeof(int)).AllowDBNull = false;
            speciesFactIdentifiers.Columns.Add("HostId", typeof(int)).AllowDBNull = true;
            speciesFactIdentifiers.Columns.Add("PeriodId", typeof(int)).AllowDBNull = true;
            return speciesFactIdentifiers;
        }

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species fact qualities.</returns>
        public static List<WebSpeciesFactQuality> GetSpeciesFactQualities(WebServiceContext context)
        {
            List<WebSpeciesFactQuality> speciesFactQualities;
            String cacheKey;
            WebSpeciesFactQuality speciesFactQuality;

            // Get cached information.
            speciesFactQualities = null;
            cacheKey = Settings.Default.SpeciesFactQualityCacheKey;
            if (!context.IsInTransaction())
            {
                speciesFactQualities = (List<WebSpeciesFactQuality>)context.GetCachedObject(cacheKey);
            }

            if (speciesFactQualities.IsNull())
            {
                // Get information from database.
                speciesFactQualities = new List<WebSpeciesFactQuality>();
                using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactQualities())
                {
                    while (dataReader.Read())
                    {
                        speciesFactQuality = new WebSpeciesFactQuality();
                        speciesFactQuality.LoadData(dataReader);
                        speciesFactQualities.Add(speciesFactQuality);
                    }

                    if (!context.IsInTransaction())
                    {
                        // Add information to cache.
                        context.AddCachedObject(cacheKey, speciesFactQualities, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                    }
                }
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Get species facts with specified identifiers.
        /// Only existing species facts are returned,
        /// e.g. species fact identifiers that does not
        /// match existing species fact does not affect
        /// the returned species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactIdentifiers">
        /// Species facts identifiers. E.g. WebSpeciesFacts
        /// instances where id for requested combination of
        /// factor, host, individual category, period and taxon
        /// has been set.
        /// Host id is only used together with taxonomic factors.
        /// Period id is only used together with periodic factors.
        /// </param>
        /// <returns>
        /// Existing species facts among the
        /// requested species facts.
        /// </returns>
        public static List<WebSpeciesFact> GetSpeciesFactsByIdentifiers(WebServiceContext context,
                                                                        List<WebSpeciesFact> speciesFactIdentifiers)
        {
            DataTable speciesFactIdentifiersTable;
            List<WebSpeciesFact> speciesFacts;
            Object host, period;
            WebSpeciesFact speciesFact;

            // Check arguments.
            speciesFactIdentifiers.CheckNotEmpty("speciesFacts");
            if (speciesFactIdentifiers.Count > Settings.Default.MaxSpeciesFacts)
            {
                // Exceeding max numbers of species facts that
                // can be retrieved in one request.
                throw new ArgumentException("Max " + Settings.Default.MaxSpeciesFacts + " species facts can be retrieved in one call.");
            }

            // Get data from database.
            speciesFactIdentifiersTable = GetSpeciesFactIdentifiersTable();
            foreach (WebSpeciesFact webSpeciesFact in speciesFactIdentifiers)
            {
                if (webSpeciesFact.IsHostSpecified)
                {
                    host = webSpeciesFact.HostId;
                }
                else
                {
                    host = DBNull.Value;
                }

                if (webSpeciesFact.IsPeriodSpecified)
                {
                    period = webSpeciesFact.PeriodId;
                }
                else
                {
                    period = DBNull.Value;
                }

                speciesFactIdentifiersTable.Rows.Add(webSpeciesFact.TaxonId, webSpeciesFact.FactorId, webSpeciesFact.IndividualCategoryId, host, period);
            }

            speciesFacts = new List<WebSpeciesFact>();
            using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactsByIdentifiers(speciesFactIdentifiersTable))
            {
                while (dataReader.Read())
                {
                    speciesFact = new WebSpeciesFact();
                    speciesFact.LoadData(dataReader);
                    speciesFacts.Add(speciesFact);
                }
            }

            return speciesFacts;
        }

        /// <summary>
        /// Get all species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactIds">Ids for speciesFacts to get information about.</param>
        /// <returns>SpeciesFacts information.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsByIds(WebServiceContext context, List<int> speciesFactIds)
        {
            List<WebSpeciesFact> speciesFacts;
            WebSpeciesFact speciesFact;

            // Check arguments.
            speciesFactIds.CheckNotEmpty("speciesFactIds");
            if (speciesFactIds.Count > Settings.Default.MaxSpeciesFacts)
            {
                // Exceeding max numbers of species facts that
                // can be retrieved in one request.
                throw new ArgumentException("Max " + Settings.Default.MaxSpeciesFacts + " species facts can be retrieved in one call.");
            }

            // Get data from database.
            speciesFacts = new List<WebSpeciesFact>();
            using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactsByIds(speciesFactIds))
            {
                while (dataReader.Read())
                {
                    speciesFact = new WebSpeciesFact();
                    speciesFact.LoadData(dataReader);
                    speciesFacts.Add(speciesFact);
                }
            }

            if (speciesFacts.Count != speciesFactIds.Count)
            {
                // Probably invalid speciesFact ids.
                throw new ArgumentException("Invalid speciesFact ids!");
            }

            return speciesFacts;
        }

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebServiceContext context,
                                                                           WebSpeciesFactSearchCriteria searchCriteria)
        {
            List<WebSpeciesFact> speciesFacts;
            WebSpeciesFact speciesFact;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get data from database.
            speciesFacts = new List<WebSpeciesFact>();
            using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactsBySearchCriteria(searchCriteria.GetQuery(WebSpeciesFactSearchCriteriaExtension.QuerySelectPart.QueryDefault),
                                                                                                               searchCriteria.FactorDataTypeIds,
                                                                                                               searchCriteria.FactorIds,
                                                                                                               searchCriteria.HostIds,
                                                                                                               searchCriteria.TaxonIds))
            {
                while (dataReader.Read())
                {
                    speciesFact = new WebSpeciesFact();
                    speciesFact.LoadData(dataReader);
                    speciesFacts.Add(speciesFact);

                    if (speciesFacts.Count > Settings.Default.MaxSpeciesFacts)
                    {
                        // Exceeding max numbers of species facts that
                        // can be retrieved in one request.
                        throw new ArgumentException("Max " + Settings.Default.MaxSpeciesFacts + " species facts can be retrieved in one call.");
                    }
                }
            }

            return speciesFacts;
        }

        /// <summary>
        /// Creates a species fact DataTable.
        /// </summary>
        /// <returns>A species fact DataTable.</returns>
        private static DataTable GetSpeciesFactTable()
        {
            DataTable speciesFactsTable = new DataTable(SpeciesFactData.TABLE_NAME);

            speciesFactsTable.Columns.Add("idnr", typeof(int));
            speciesFactsTable.Columns.Add("faktor", typeof(int));
            speciesFactsTable.Columns.Add("taxon", typeof(int));
            speciesFactsTable.Columns.Add("individkat", typeof(int));
            speciesFactsTable.Columns.Add("referens", typeof(int));
            speciesFactsTable.Columns.Add("borttagen", typeof(int));
            speciesFactsTable.Columns.Add("datum", typeof(DateTime));
            speciesFactsTable.Columns.Add("person", typeof(string));
            speciesFactsTable.Columns.Add("tal1", typeof(double));
            speciesFactsTable.Columns.Add("tal2", typeof(double));
            speciesFactsTable.Columns.Add("tal3", typeof(double));
            speciesFactsTable.Columns.Add("text1", typeof(string));
            speciesFactsTable.Columns.Add("text2", typeof(string));
            speciesFactsTable.Columns.Add("host", typeof(int));
            speciesFactsTable.Columns.Add("quality", typeof(int));
            speciesFactsTable.Columns.Add("period", typeof(int));
            speciesFactsTable.Columns.Add("IndividualCategoryId", typeof(int));
            return speciesFactsTable;
        }

        /// <summary>
        /// Get species fact DataTable for updating.
        /// </summary>
        /// <returns>A species fact DataTable for updating.</returns>
        private static DataTable GetSpeciesFactUpdateTable()
        {
            DataTable updateSpeciesFactTable = new DataTable("UpdateSpeciesFactTable");

            updateSpeciesFactTable.Columns.Add("Id", typeof(int)).AllowDBNull = false;
            updateSpeciesFactTable.Columns.Add("ReferenceId", typeof(int)).AllowDBNull = false;
            updateSpeciesFactTable.Columns.Add("UpdateUserFullName", typeof(string)).AllowDBNull = false;
            updateSpeciesFactTable.Columns["UpdateUserFullName"].MaxLength = 250;
            updateSpeciesFactTable.Columns.Add("FieldValue1", typeof(double)).AllowDBNull = true;
            updateSpeciesFactTable.Columns.Add("FieldValue2", typeof(double)).AllowDBNull = true;
            updateSpeciesFactTable.Columns.Add("FieldValue3", typeof(double)).AllowDBNull = true;
            updateSpeciesFactTable.Columns.Add("FieldValue4", typeof(string)).AllowDBNull = true;
            updateSpeciesFactTable.Columns["FieldValue4"].MaxLength = 250;
            updateSpeciesFactTable.Columns.Add("FieldValue5", typeof(string)).AllowDBNull = true;
            updateSpeciesFactTable.Columns["FieldValue5"].MaxLength = 7000;
            updateSpeciesFactTable.Columns.Add("QualityId", typeof(int)).AllowDBNull = false;
            return updateSpeciesFactTable;
        }

        /// <summary>
        /// Get table with information about new species facts to update.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFacts">Species facts to update.</param>
        /// <returns>Table with species fact information.</returns>
        public static DataTable GetSpeciesFactUpdateTable(WebServiceContext context,
                                                          List<WebSpeciesFact> speciesFacts)
        {
            DataTable speciesFactTable = GetSpeciesFactUpdateTable();

            if (speciesFacts.IsNotEmpty())
            {
                DataRow row;
                WebPerson person = WebServiceData.UserManager.GetPerson(context);
                String fullName = person.IsNotNull() ? person.FirstName + " " + person.LastName : String.Empty;

                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    row = speciesFactTable.NewRow();
                    row[0] = speciesFact.Id;
                    row[1] = speciesFact.ReferenceId;
                    row[2] = fullName;
                    if (speciesFact.IsFieldValue1Specified)
                    {
                        row[3] = speciesFact.FieldValue1;
                    }
                    else
                    {
                        row[3] = DBNull.Value;
                    }

                    if (speciesFact.IsFieldValue2Specified)
                    {
                        row[4] = speciesFact.FieldValue2;
                    }
                    else
                    {
                        row[4] = DBNull.Value;
                    }

                    if (speciesFact.IsFieldValue3Specified)
                    {
                        row[5] = speciesFact.FieldValue3;
                    }
                    else
                    {
                        row[5] = DBNull.Value;
                    }

                    if (speciesFact.IsFieldValue4Specified)
                    {
                        row[6] = speciesFact.FieldValue4;
                    }
                    else
                    {
                        row[6] = DBNull.Value;
                    }

                    if (speciesFact.IsFieldValue5Specified)
                    {
                        row[7] = speciesFact.FieldValue5;
                    }
                    else
                    {
                        row[7] = DBNull.Value;
                    }

                    row[8] = speciesFact.QualityId;
                    speciesFactTable.Rows.Add(row);
                }
            }

            return speciesFactTable;
        }

        /// <summary>
        /// Remove information objects from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void RemoveCachedObjects(WebServiceContext context)
        {
            String cacheKey;

            if (_isSpeciesFactInformationUpdated)
            {
                _isSpeciesFactInformationUpdated = false;
                cacheKey = Settings.Default.SpeciesFactQualityCacheKey;
                context.RemoveCachedObject(cacheKey);
            }
        }

        /// <summary>
        /// Get taxon count of the taxa that matches fact search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxon count of the taxa that matches fact search criteria.</returns>
        public static Int32 GetTaxaCountBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria)
        {
            Int32 taxonCount = 0;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get data from database.
            using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactsBySearchCriteria(searchCriteria.GetQuery(WebSpeciesFactSearchCriteriaExtension.QuerySelectPart.QueryTaxonCount),
                                                                                                               searchCriteria.FactorDataTypeIds,
                                                                                                               searchCriteria.FactorIds,
                                                                                                               searchCriteria.HostIds,
                                                                                                               searchCriteria.TaxonIds))
            {
                while (dataReader.Read())
                {
                    taxonCount = dataReader.GetInt32(SpeciesFactData.TAXON_COUNT);

                    if (taxonCount > Settings.Default.MaxSpeciesFacts)
                    {
                        // Exceeding max numbers of taxa that
                        // can be retrieved in one request.
                        throw new ArgumentException("Max " + Settings.Default.MaxSpeciesFacts + " taxa can be retrieved in one call.");
                    }
                }
            }

            return taxonCount;
        }

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        public static List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria)
        {
            List<Int32> taxonIds = new List<int>();

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData();

            // Get list of taxon ids from database.
            using (DataReader dataReader = context.GetTaxonAttributeDatabase().GetSpeciesFactsBySearchCriteria(searchCriteria.GetQuery(WebSpeciesFactSearchCriteriaExtension.QuerySelectPart.QueryTaxonIds),
                                                                                                               searchCriteria.FactorDataTypeIds,
                                                                                                               searchCriteria.FactorIds,
                                                                                                               searchCriteria.HostIds,
                                                                                                               searchCriteria.TaxonIds))
            {
                while (dataReader.Read())
                {
                    // Gather distinct list of taxon ids
                    if (!taxonIds.Contains(dataReader.GetInt32(SpeciesFactData.TAXON_ID)))
                    {
                        taxonIds.Add(dataReader.GetInt32(SpeciesFactData.TAXON_ID));
                    }

                    if (taxonIds.Count > Settings.Default.MaxSpeciesFacts)
                    {
                        // Exceeding max numbers of taxa that
                        // can be retrieved in one request.
                        throw new ArgumentException("Max " + Settings.Default.MaxSpeciesFacts + " taxa can be retrieved in one call.");
                    }
                }
            }

            // Get list of taxa by list of taxon ids
            return WebServiceData.TaxonManager.GetTaxaByIds(context, taxonIds);
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public static void UpdateSpeciesFacts(WebServiceContext context,
                                              List<WebSpeciesFact> updateSpeciesFacts)
        {
            DataTable speciesFactTable;

            // Check transaction.
            context.CheckTransaction();

            // Check authorization.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditSpeciesFacts);

            // Check arguments.
            updateSpeciesFacts.CheckData();

            if (updateSpeciesFacts.IsNotEmpty())
            {
                // Update species facts.
                speciesFactTable = GetSpeciesFactUpdateTable(context, updateSpeciesFacts);
                context.GetTaxonAttributeDatabase().UpdateSpeciesFacts(speciesFactTable);
            }
        }
    }
}