using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of species observation information.
    /// </summary>
    public class SpeciesObservationManager
    {
        /// <summary>
        /// Max number of species observations that are returned
        /// in one call from the client. Exception is thrown
        /// if to many species observations are requested. 
        /// </summary>
        public const Int64 MAX_SPECIES_OBSERVATIONS = 1000000;

        /// <summary>
        /// Max number of species observations (with information)
        /// that are returned in one call from the client.
        /// If to many species observations are requested then 
        /// only species observations ids are returned and the
        /// client is expected to retrieve the species observation
        /// information in repeated calls to method
        /// GetSpeciesObservationsById.
        /// </summary>
        public const Int64 MAX_SPECIES_OBSERVATIONS_WITH_INFORMATION = 100000;

        /// <summary>
        /// Constructor.
        /// </summary>
        static SpeciesObservationManager()
        {
            SpeciesObservationCoordinateSystem = new WebCoordinateSystem();
            SpeciesObservationCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
        }

        /// <summary>
        /// Coordinate system used for species observations that 
        /// are retrieved from artfakta database.
        /// </summary>
        private static WebCoordinateSystem SpeciesObservationCoordinateSystem
        { get; set; }

        /// <summary>
        /// Inserts a list of species observations ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Id for species observations to insert.</param>
        public static void AddUserSelectedSpeciesObservations(WebServiceContext context,
                                                              List<Int64> speciesObservationIds)
        {
            DataRow row;
            DataTable speciesObservationTable;

            if (speciesObservationIds.IsNotEmpty())
            {
                // Delete all species observations ids that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedSpeciesObservations(context);

                // Insert the new list of species observations.
                speciesObservationTable = GetUserSelectedSpeciesObservationsTable(context);
                foreach (Int64 speciesObservationId in speciesObservationIds)
                {
                    row = speciesObservationTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = speciesObservationId;
                    speciesObservationTable.Rows.Add(row);
                }

                DataServer.AddUserSelectedSpeciesFacts(context, speciesObservationTable);
            }
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webSpeciesObservation">Species observation.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebSpeciesObservation webSpeciesObservation,
                                                 WebAuthority authority)
        {
            Boolean isTaxonFound;
            List<WebRegionGeography> regionsGeography;
            List<WebTaxon> taxa;
            WebPoint point;

            // Test if authority is related to species observations.
            if (authority.Identifier != AuthorityIdentifier.Sighting.ToString())
            {
                return false;
            }

            // Test if authority has enough protection level.
            if (authority.MaxProtectionLevel < webSpeciesObservation.ProtectionLevel)
            {
                return false;
            }

            // Test if species observation is inside regions.
            if (authority.RegionGUIDs.IsNotEmpty())
            {
                point = new WebPoint(webSpeciesObservation.CoordinateX,
                                     webSpeciesObservation.CoordinateY);
                regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                           authority.RegionGUIDs,
                                                                                           SpeciesObservationCoordinateSystem);
                if (!regionsGeography.IsPointInsideGeometry(context,
                                                            SpeciesObservationCoordinateSystem,
                                                            point))
                {
                    return false;
                }
            }

            // Test if species observation belongs to specified taxa.
            if (authority.TaxonGUIDs.IsNotEmpty())
            {
                taxa = GetTaxa(context, authority);
                isTaxonFound = false;
                foreach (WebTaxon webTaxon in taxa)
                {
                    if (webTaxon.Id == webSpeciesObservation.DyntaxaTaxonId)
                    {
                        isTaxonFound = true;
                        break;
                    }
                }

                if (!isTaxonFound)
                {
                    return false;
                }
            }

            // Species observation has passed all tests.
            // User has access right to this species observation.
            return true;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webSpeciesObservation">Species observation.</param>
        /// <param name="role">Check access right in this role.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebSpeciesObservation webSpeciesObservation,
                                                 WebRole role)
        {
            foreach (WebAuthority authority in role.Authorities)
            {
                if (CheckAccessRights(context,
                                      webSpeciesObservation,
                                      authority))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webSpeciesObservation">Species observation.</param>
        /// <param name="roleId">Check access right in this role.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebSpeciesObservation webSpeciesObservation,
                                                 Int32 roleId)
        {
            Boolean roleFound;
            List<WebRole> roles;
            String errorMessage;
            WebRole webRole;

            // Get role.
            webRole = null;
            roleFound = false;
            roles = WebServiceData.UserManager.GetRoles(context);
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Id == roleId)
                    {
                        roleFound = true;
                        webRole = role;
                    }
                }
            }

            if (!roleFound)
            {
                errorMessage = "Du har inte behörighet att genomföra observationsuttaget.";
                errorMessage += "Angiven roll id = " + roleId + ".";
                if (roles.IsNotEmpty())
                {
                    foreach (WebRole role in roles)
                    {
                        errorMessage += "Roll id från UserService = " + role.Id + ".";
                    }
                }

                throw new UnauthorizedAccessException(errorMessage);
            }

            return CheckAccessRights(context, webSpeciesObservation, webRole);
        }

        /// <summary>
        /// Check if user has access rights to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webSpeciesObservation">A species observation.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebSpeciesObservation webSpeciesObservation)
        {
            Boolean hasAccessRight;

            hasAccessRight = false;
            foreach (WebRole role in context.GetRoles())
            {
                if (CheckAccessRights(context,
                                      webSpeciesObservation,
                                      role))
                {
                    hasAccessRight = true;
                    break;
                }
            }

            return hasAccessRight;
        }

        /// <summary>
        /// Check that user has access rights to
        /// retrieved species observations.
        /// Observations are removed from list if user does
        /// not have access rights to them.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webSpeciesObservations">Species observations.</param>
        private static void CheckAccessRights(WebServiceContext context,
                                              List<WebSpeciesObservation> webSpeciesObservations)
        {
            Int32 index;

            if (webSpeciesObservations.IsNotEmpty())
            {
                for (index = webSpeciesObservations.Count - 1; index >= 0; index--)
                {
                    if (!CheckAccessRights(context,
                                           webSpeciesObservations[index]))
                    {
                        webSpeciesObservations.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Check that search criteria are ok.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='searchCriteria'>Species observation search criteria.</param>
        /// <exception cref="ArgumentException">Thrown if information in search criteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        private static void CheckArgument(WebServiceContext context,
                                          WebSpeciesObservationSearchCriteria searchCriteria)
        {
            Boolean roleFound;
            List<WebRole> roles;
            String errorMessage;

            // Check input arguments.
            roleFound = false;
            roles = WebServiceData.UserManager.GetRoles(context);
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Id == searchCriteria.UserRoleId)
                    {
                        roleFound = true;
                        break;
                    }
                }
            }
            if (!roleFound)
            {
                errorMessage = "Du har inte behörighet att genomföra observationsuttaget.";
                errorMessage += "Angiven roll id = " + searchCriteria.UserRoleId + ".";
                if (roles.IsNotEmpty())
                {
                    foreach (WebRole role in roles)
                    {
                        errorMessage += "Roll id från UserService = " + role.Id + ".";
                    }
                }
                throw new UnauthorizedAccessException(errorMessage);
            }
            searchCriteria.LocationSearchString = searchCriteria.LocationSearchString.CheckSqlInjection();
            searchCriteria.ObserverSearchString = searchCriteria.ObserverSearchString.CheckSqlInjection();
            if ((searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet) && (searchCriteria.ObservationStartDate == DateTime.MaxValue))
            {
                throw new ArgumentException("Startdatum för observation måste anges");
            }
            if ((searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet) && (searchCriteria.ObservationEndDate == DateTime.MaxValue))
            {
                throw new ArgumentException("Slutdatum för observation måste anges");
            }
            if ((searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet) && (searchCriteria.RegistrationStartDate == DateTime.MaxValue))
            {
                throw new ArgumentException("Startdatum för registrering måste anges");
            }
            if ((searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet) && (searchCriteria.RegistrationEndDate == DateTime.MaxValue))
            {
                throw new ArgumentException("Slutdatum för registrering måste anges");
            }
        }

        /// <summary>
        /// Check that changed dates are ok.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <exception cref="ArgumentException">Thrown if information in changed dates are invalid.</exception>
        private static void CheckChangedDates(WebServiceContext context,
                                              ref DateTime changedFrom,
                                              ref DateTime changedTo)
        {
            DateTime today, yesterday;
            WebDatabaseUpdate databaseUpdate;

            // Remove hour, minute and second part of DateTime.
            changedFrom = new DateTime(changedFrom.Year, changedFrom.Month, changedFrom.Day, 0, 0, 0);
            changedTo = new DateTime(changedTo.Year, changedTo.Month, changedTo.Day, 0, 0, 0);

            // Check if changed from is newer than changed to.
            if (changedFrom > changedTo)
            {
                throw new ArgumentException("Changed from " + changedFrom.WebToString() + " is larger than changed to " + changedTo.WebToString());
            }

            // Check if changed to is today or in the future.
            today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (today <= changedTo)
            {
                throw new ArgumentException("Changed to " + changedTo.WebToString() + " can not be today or in the future.");
            }

            // Check if changed to is yesterday and that nightly
            // update of species observation has not finished yet.
            yesterday = today - new TimeSpan(1, 0, 0, 0);
            if (yesterday == changedTo)
            {
                // Check if nightly update of species observations has finished.
                databaseUpdate = DatabaseManager.GetDatabaseUpdate(context);
                if ((DateTime.Now.Hour < databaseUpdate.UpdateEnd.Hour) ||
                    ((DateTime.Now.Hour == databaseUpdate.UpdateEnd.Hour) &&
                     (DateTime.Now.Minute <= databaseUpdate.UpdateEnd.Minute)))
                {
                    throw new ArgumentException("Yesterdays changes in species observations has not yet been updated.");
                }
            }

            // Set changedTo to the beginning of the next day.
            // Operator < is used when comparing changedTo
            // in the database.
            changedTo = changedTo + new TimeSpan(1, 0, 0, 0);
        }

        /// <summary>
        /// Delete all species observations that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedSpeciesObservations(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedSpeciesObservations(context);
        }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about bird nest activities.</returns>
        public static List<WebBirdNestActivity> GetBirdNestActivities(WebServiceContext context)
        {
            List<WebBirdNestActivity> birdNestActivities;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllBirdNestActivities";
            birdNestActivities = (List<WebBirdNestActivity>)context.GetCachedObject(cacheKey);

            if (birdNestActivities.IsNull())
            {
                // Get information from database.
                birdNestActivities = new List<WebBirdNestActivity>();
                using (DataReader dataReader = DataServer.GetBirdNestActivities(context))
                {
                    while (dataReader.Read())
                    {
                        birdNestActivities.Add(new WebBirdNestActivity(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, birdNestActivities, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.Low);
                }
            }
            return birdNestActivities;
        }

        /// <summary>
        /// Calculates the sum of all row-values and returns only one row.
        /// </summary>
        /// <param name="outTable">The returning table.</param>
        private static void GetCount(DataTable outTable)
        {
            DataRow newRow;
            Int32 sum = 0;

            foreach (DataRow row in outTable.Rows)
            {
                sum += (Int32)row[0];
            }
            outTable.Rows.Clear();
            newRow = outTable.NewRow();
            newRow[0] = sum;
            outTable.Rows.Add(newRow);
            outTable.Columns[0].ColumnName = "Antal";
        }

        /// <summary>
        /// Get max protection level for species observation that
        /// current user are allowed to.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max protection level.</returns>
        private static Int32 GetProtectionLevel(WebServiceContext context)
        {
            Int32 protectionLevel;
            List<WebRole> roles;

            protectionLevel = 0;
            roles = WebServiceData.UserManager.GetRoles(context);
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if ((authority.Identifier == AuthorityIdentifier.Sighting.ToString()) &&
                                (protectionLevel < authority.MaxProtectionLevel))
                            {
                                protectionLevel = authority.MaxProtectionLevel;
                            }
                        }
                    }
                }
            }

            // Add one to protection level since stored procedure
            // uses '<' and not '<=' when controlling access rights.
            return protectionLevel + 1;
        }

        /// <summary>
        /// Get max protection level for species observation that
        /// current user are allowed to view with selected role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Max protection level.</returns>
        private static Int32 GetProtectionLevel(WebServiceContext context,
                                                WebSpeciesObservationSearchCriteria searchCriteria)
        {
            Int32 protectionLevel;
            List<WebRole> roles;

            protectionLevel = 0;
            roles = WebServiceData.UserManager.GetRoles(context);
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if ((role.Id == searchCriteria.UserRoleId) &&
                        role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if ((authority.Identifier == AuthorityIdentifier.Sighting.ToString()) &&
                                (protectionLevel < authority.MaxProtectionLevel))
                            {
                                protectionLevel = authority.MaxProtectionLevel;
                            }
                        }
                        break;
                    }
                }
            }

            // Add one to protection level since stored procedure
            // uses '<' and not '<=' when controlling access rights.
            return protectionLevel + 1;
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 100000 observations of each change type (new or updated)
        /// with information can be retrieved in one call.
        /// Max 1000000 observations of each change type (deleted, new
        /// or updated), with GUIDs or ids, can be retrieved in one call.
        /// Parameters changedFrom and changedTo may be the same date.
        /// Parameter changedTo must not be today or in the future.
        /// If parameter changedTo is yesterday the method call
        /// must be made after the nightly update of the 
        /// species observations have been performed. 
        /// Currently it is ok to call this method after 05:00
        /// if yesterdays species observations should be retrieved.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <returns>Information about changed species observations.</returns>
        public static WebSpeciesObservationChange GetSpeciesObservationChange(WebServiceContext context,
                                                                              DateTime changedFrom,
                                                                              DateTime changedTo)
        {
            WebSpeciesObservationChange change;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            CheckChangedDates(context, ref changedFrom, ref changedTo);
            DatabaseManager.CheckDatabaseUpdate(context);

            // Cache taxon information for authorities.
            // This is done to avoid problem with two open
            // data readers at the same time.
            if (!context.GetRoles().IsSimpleSpeciesObservationAccessRights())
            {
                foreach (WebRole role in context.GetRoles())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        GetTaxa(context, authority);
                    }
                }
            }

            // Get changed species observation information from database.
            using (DataReader dataReader = DataServer.GetSpeciesObservationChange(context,
                                                                                  GetProtectionLevel(context),
                                                                                  changedFrom,
                                                                                  changedTo))
            {
                change = GetSpeciesObservationChange(context, dataReader);
            }

            return change;
        }

        /// <summary>
        /// Create a WebSpeciesObservationChange instance.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='dataReader'>An open data reader.</param>
        /// <returns>A WebSpeciesObservationChange instance.</returns>
        private static WebSpeciesObservationChange GetSpeciesObservationChange(WebServiceContext context,
                                                                               DataReader dataReader)
        {
            Boolean isSimpleSpeciesObservationAccessRights;
            WebSpeciesObservation speciesObservation;
            WebSpeciesObservationChange speciesObservationChange;

            isSimpleSpeciesObservationAccessRights = context.GetRoles().IsSimpleSpeciesObservationAccessRights();
            speciesObservationChange = new WebSpeciesObservationChange();
            speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();
            speciesObservationChange.DeletedSpeciesObservationCount = 0;
            speciesObservationChange.MaxSpeciesObservationCount = Settings.Default.MaxSpeciesObservationWithInformation;
            speciesObservationChange.NewSpeciesObservationCount = 0;
            speciesObservationChange.NewSpeciesObservationIds = null;
            speciesObservationChange.NewSpeciesObservations = new List<WebSpeciesObservation>();
            speciesObservationChange.UpdatedSpeciesObservationCount = 0;
            speciesObservationChange.UpdatedSpeciesObservationIds = null;
            speciesObservationChange.UpdatedSpeciesObservations = new List<WebSpeciesObservation>();

            // Get new species observations.
            while (dataReader.Read())
            {
                speciesObservationChange.NewSpeciesObservationCount++;
                if (speciesObservationChange.NewSpeciesObservationCount <= speciesObservationChange.MaxSpeciesObservationCount)
                {
                    // Add species observation with information.
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationChange.NewSpeciesObservations.Add(speciesObservation);
                    }
                    else
                    {
                        if (CheckAccessRights(context, speciesObservation))
                        {
                            speciesObservationChange.NewSpeciesObservations.Add(speciesObservation);
                        }
                    }
                }
                else
                {
                    if (speciesObservationChange.NewSpeciesObservationCount == (speciesObservationChange.MaxSpeciesObservationCount + 1))
                    {
                        // To many species observations.
                        // Return only species observation ids.
                        // Move species observation ids from 
                        // SpeciesObservations to SpeciesObservationIds.
                        speciesObservationChange.NewSpeciesObservationIds = new List<Int64>();
                        foreach (WebSpeciesObservation speciesObservationTemp in speciesObservationChange.NewSpeciesObservations)
                        {
                            speciesObservationChange.NewSpeciesObservationIds.Add(speciesObservationTemp.Id);
                        }

                        speciesObservationChange.NewSpeciesObservations = null;
                    }

                    // Add only species observation id.
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationChange.NewSpeciesObservationIds.Add(dataReader.GetInt64(SpeciesObservationData.ID));
                    }
                    else
                    {
                        speciesObservation = new WebSpeciesObservation(dataReader);
                        if (CheckAccessRights(context, speciesObservation))
                        {
                            speciesObservationChange.NewSpeciesObservationIds.Add(speciesObservation.Id);
                        }
                    }
                }

                if (speciesObservationChange.NewSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many new species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            if (!dataReader.NextResultSet())
            {
                throw new ApplicationException("Failed to read updated species observation.");
            }

            // Get updated species observations.
            while (dataReader.Read())
            {
                speciesObservationChange.UpdatedSpeciesObservationCount++;
                if (speciesObservationChange.UpdatedSpeciesObservationCount <= speciesObservationChange.MaxSpeciesObservationCount)
                {
                    // Add species observation with information.
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationChange.UpdatedSpeciesObservations.Add(speciesObservation);
                    }
                    else
                    {
                        if (CheckAccessRights(context, speciesObservation))
                        {
                            speciesObservationChange.UpdatedSpeciesObservations.Add(speciesObservation);
                        }
                    }
                }
                else
                {
                    if (speciesObservationChange.UpdatedSpeciesObservationCount == (speciesObservationChange.MaxSpeciesObservationCount + 1))
                    {
                        // To many species observations.
                        // Return only species observation ids.
                        // Move species observation ids from 
                        // SpeciesObservations to SpeciesObservationIds.
                        speciesObservationChange.UpdatedSpeciesObservationIds = new List<Int64>();
                        foreach (WebSpeciesObservation speciesObservationTemp in speciesObservationChange.NewSpeciesObservations)
                        {
                            speciesObservationChange.UpdatedSpeciesObservationIds.Add(speciesObservationTemp.Id);
                        }

                        speciesObservationChange.UpdatedSpeciesObservations = null;
                    }

                    // Add only species observation id.
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationChange.UpdatedSpeciesObservationIds.Add(dataReader.GetInt64(SpeciesObservationData.ID));
                    }
                    else
                    {
                        speciesObservation = new WebSpeciesObservation(dataReader);
                        if (CheckAccessRights(context, speciesObservation))
                        {
                            speciesObservationChange.UpdatedSpeciesObservationIds.Add(speciesObservation.Id);
                        }
                    }
                }

                if (speciesObservationChange.UpdatedSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many updated species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            if (!dataReader.NextResultSet())
            {
                throw new ApplicationException("Failed to read deleted species observation.");
            }

            // Get deleted species observations.
            while (dataReader.Read())
            {
                speciesObservationChange.DeletedSpeciesObservationCount++;

                // Add species observation GUID.
                speciesObservationChange.DeletedSpeciesObservationGuids.Add(WebSpeciesObservation.GetGuid(dataReader));

                if (speciesObservationChange.DeletedSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many deleted species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            return speciesObservationChange;
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public static Int32 GetSpeciesObservationCountBySearchCriteria(WebServiceContext context,
                                                                       WebSpeciesObservationSearchCriteria searchCriteria)
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            CheckArgument(context, searchCriteria);
            DatabaseManager.CheckDatabaseUpdate(context);
            searchCriteria.CheckData();

            if (context.GetRole(searchCriteria.UserRoleId).IsSimpleSpeciesObservationAccessRights())
            {
                try
                {
                    // Add information about user selected taxa to the database.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.AddUserSelectedTaxa(context, searchCriteria.TaxonIds, UserSelectedTaxonUsage.Input);
                        DataServer.UpdateUserSelecedTaxa(context);
                    }

                    return GetSpeciesObservationCountBySearchCriteria(context,
                                                                      searchCriteria,
                                                                      GetProtectionLevel(context, searchCriteria));
                }
                finally
                {
                    // Clean up.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.DeleteUserSelectedTaxa(context);
                    }
                }
            }
            else
            {
                speciesObservationInformation = GetSpeciesObservations(context, searchCriteria);
                return (Int32)(speciesObservationInformation.SpeciesObservationCount);
            }
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="protectionLevel">Protection level.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        private static Int32 GetSpeciesObservationCountBySearchCriteria(WebServiceContext context,
                                                                        WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        Int32 protectionLevel)
        {
            Boolean hasCountyId, hasCountyPartId;
            Boolean hasProvinceId, hasProvincePartId;
            Int32 countyId, countyPartId;
            Int32 provinceId, provincePartId;
            Int32 databaseId;
            WebCounty county;
            WebProvince province;

            // Set values.
            if (searchCriteria.DatabaseIds.IsNotEmpty() &&
                (searchCriteria.DatabaseIds.Count == 1))
            {
                databaseId = searchCriteria.DatabaseIds[0];
            }
            else
            {
                databaseId = -1;
            }

            if (searchCriteria.Counties.IsNotEmpty() &&
                (searchCriteria.Counties.Count == 1))
            {
                county = searchCriteria.Counties[0];
                if (county.IsCountyPart)
                {
                    hasCountyId = false;
                    countyId = -1;
                    hasCountyPartId = true;
                    countyPartId = county.Id;
                }
                else
                {
                    hasCountyId = true;
                    countyId = county.Id;
                    hasCountyPartId = false;
                    countyPartId = -1;
                }
            }
            else
            {
                hasCountyId = false;
                countyId = -1;
                hasCountyPartId = false;
                countyPartId = -1;
            }

            if (searchCriteria.Provinces.IsNotEmpty() &&
                (searchCriteria.Provinces.Count == 1))
            {
                province = searchCriteria.Provinces[0];
                if (province.IsProvincePart)
                {
                    hasProvinceId = false;
                    provinceId = -1;
                    hasProvincePartId = true;
                    provincePartId = province.Id;
                }
                else
                {
                    hasProvinceId = true;
                    provinceId = province.Id;
                    hasProvincePartId = false;
                    provincePartId = -1;
                }
            }
            else
            {
                hasProvinceId = false;
                provinceId = -1;
                hasProvincePartId = false;
                provincePartId = -1;
            }

            // Get information from database.
            return DataServer.GetSpeciesObservationCountBySearchCriteria(context,
                                                                         0,
                                                                         protectionLevel, //securityParameter.Power,
                                                                         false, //securityParameter.HasTaxonId(),
                                                                         0, //securityParameter.TaxonId,
                                                                         false, //securityParameter.HasRegionId(),
                                                                         0, //securityParameter.RegionId,
                                                                         0, //securityParameter.Distorsion,
                                                                         false, //securityParameter.HideAttributes,
                                                                         searchCriteria.TaxonIds.IsNotEmpty(),
                                                                         (Int32)(searchCriteria.UseOfObservationDate),
                                                                         searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet,
                                                                         searchCriteria.ObservationStartDate,
                                                                         searchCriteria.ObservationEndDate,
                                                                         (Int32)(searchCriteria.UseOfRegistrationDate),
                                                                         searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet,
                                                                         searchCriteria.RegistrationStartDate,
                                                                         searchCriteria.RegistrationEndDate,
                                                                         searchCriteria.IsRectangleSpecified,
                                                                         searchCriteria.NorthCoordinate,
                                                                         searchCriteria.SouthCoordinate,
                                                                         searchCriteria.EastCoordinate,
                                                                         searchCriteria.WestCoordinate,
                                                                         searchCriteria.IsAccuracySpecified,
                                                                         searchCriteria.Accuracy,
                                                                         searchCriteria.LocationSearchString,
                                                                         searchCriteria.IncludePositiveObservations,
                                                                         searchCriteria.IncludeNeverFoundObservations,
                                                                         searchCriteria.IncludeNotRediscoveredObservations,
                                                                         (databaseId != -1),
                                                                         databaseId,
                                                                         hasProvinceId,
                                                                         provinceId,
                                                                         hasProvincePartId,
                                                                         provincePartId,
                                                                         hasCountyId,
                                                                         countyId,
                                                                         hasCountyPartId,
                                                                         countyPartId,
                                                                         searchCriteria.IsBirdNestActivityLevelSpecified,
                                                                         searchCriteria.BirdNestActivityLevel,
                                                                         searchCriteria.ObserverSearchString,
                                                                         searchCriteria.IncludeUncertainTaxonDetermination);
        }

        /// <summary>
        /// Get species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public static WebSpeciesObservationInformation GetSpeciesObservations(WebServiceContext context,
                                                                              WebSpeciesObservationSearchCriteria searchCriteria)
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            CheckArgument(context, searchCriteria);
            DatabaseManager.CheckDatabaseUpdate(context);
            searchCriteria.CheckData();

            // Cache taxon information for authorities.
            // This is done to avoid problem with two open
            // data readers at the same time.
            if (!context.GetRole(searchCriteria.UserRoleId).IsSimpleSpeciesObservationAccessRights())
            {
                foreach (WebAuthority authority in context.GetRole(searchCriteria.UserRoleId).Authorities)
                {
                    GetTaxa(context, authority);
                }
            }

            // Get observation information from the database.
            try
            {
                // Add information about user selected taxa to the database.
                if (searchCriteria.TaxonIds.IsNotEmpty())
                {
                    TaxonManager.AddUserSelectedTaxa(context, searchCriteria.TaxonIds, UserSelectedTaxonUsage.Input);
                    DataServer.UpdateUserSelecedTaxa(context);
                }

                speciesObservationInformation = new WebSpeciesObservationInformation();
                using (DataReader dataReader = GetSpeciesObservations(context,
                                                                      searchCriteria,
                                                                      GetProtectionLevel(context, searchCriteria)))
                {
                    LoadSpeciesObservations(context, dataReader, speciesObservationInformation, searchCriteria.UserRoleId);
                }
            }
            finally
            {
                // Clean up.
                if (searchCriteria.TaxonIds.IsNotEmpty())
                {
                    TaxonManager.DeleteUserSelectedTaxa(context);
                }
            }

            return speciesObservationInformation;
        }

        /// <summary>
        /// Get species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="protectionLevel">Protection level.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        private static DataReader GetSpeciesObservations(WebServiceContext context,
                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                         Int32 protectionLevel)
        {
            Boolean hasCountyId, hasCountyPartId;
            Boolean hasProvinceId, hasProvincePartId;
            Int32 countyId, countyPartId;
            Int32 provinceId, provincePartId;
            Int32 databaseId;
            WebCounty county;
            WebProvince province;

            // Set values.
            if (searchCriteria.DatabaseIds.IsNotEmpty() &&
                (searchCriteria.DatabaseIds.Count == 1))
            {
                databaseId = searchCriteria.DatabaseIds[0];
            }
            else
            {
                databaseId = -1;
            }

            if (searchCriteria.Counties.IsNotEmpty() &&
                (searchCriteria.Counties.Count == 1))
            {
                county = searchCriteria.Counties[0];
                if (county.IsCountyPart)
                {
                    hasCountyId = false;
                    countyId = -1;
                    hasCountyPartId = true;
                    countyPartId = county.Id;
                }
                else
                {
                    hasCountyId = true;
                    countyId = county.Id;
                    hasCountyPartId = false;
                    countyPartId = -1;
                }
            }
            else
            {
                hasCountyId = false;
                countyId = -1;
                hasCountyPartId = false;
                countyPartId = -1;
            }

            if (searchCriteria.Provinces.IsNotEmpty() &&
                (searchCriteria.Provinces.Count == 1))
            {
                province = searchCriteria.Provinces[0];
                if (province.IsProvincePart)
                {
                    hasProvinceId = false;
                    provinceId = -1;
                    hasProvincePartId = true;
                    provincePartId = province.Id;
                }
                else
                {
                    hasProvinceId = true;
                    provinceId = province.Id;
                    hasProvincePartId = false;
                    provincePartId = -1;
                }
            }
            else
            {
                hasProvinceId = false;
                provinceId = -1;
                hasProvincePartId = false;
                provincePartId = -1;
            }

            // Get information from database.
            return DataServer.GetSpeciesObservations(context,
                                                     0,
                                                     protectionLevel, //securityParameter.Power,
                                                     false, //securityParameter.HasTaxonId(),
                                                     0, //securityParameter.TaxonId,
                                                     false, //securityParameter.HasRegionId(),
                                                     0, //securityParameter.RegionId,
                                                     0, //securityParameter.Distorsion,
                                                     false, //securityParameter.HideAttributes,
                                                     searchCriteria.TaxonIds.IsNotEmpty(),
                                                     (Int32)(searchCriteria.UseOfObservationDate),
                                                     searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet,
                                                     searchCriteria.ObservationStartDate,
                                                     searchCriteria.ObservationEndDate,
                                                     (Int32)(searchCriteria.UseOfRegistrationDate),
                                                     searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet,
                                                     searchCriteria.RegistrationStartDate,
                                                     searchCriteria.RegistrationEndDate,
                                                     searchCriteria.IsRectangleSpecified,
                                                     searchCriteria.NorthCoordinate,
                                                     searchCriteria.SouthCoordinate,
                                                     searchCriteria.EastCoordinate,
                                                     searchCriteria.WestCoordinate,
                                                     searchCriteria.IsAccuracySpecified,
                                                     searchCriteria.Accuracy,
                                                     searchCriteria.LocationSearchString,
                                                     searchCriteria.IncludePositiveObservations,
                                                     searchCriteria.IncludeNeverFoundObservations,
                                                     searchCriteria.IncludeNotRediscoveredObservations,
                                                     (databaseId != -1),
                                                     databaseId,
                                                     hasProvinceId,
                                                     provinceId,
                                                     hasProvincePartId,
                                                     provincePartId,
                                                     hasCountyId,
                                                     countyId,
                                                     hasCountyPartId,
                                                     countyPartId,
                                                     searchCriteria.IsBirdNestActivityLevelSpecified,
                                                     searchCriteria.BirdNestActivityLevel,
                                                     SpeciesObservationInformationType.All.ToString(),
                                                     searchCriteria.ObserverSearchString,
                                                     searchCriteria.IncludeUncertainTaxonDetermination);
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 10000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="userRoleId">In which role is the user retrieving species observations.</param>
        /// <returns>Species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if to many species observation ids has been given.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public static WebSpeciesObservationInformation GetSpeciesObservationsById(WebServiceContext context,
                                                                                  List<Int64> speciesObservationIds,
                                                                                  Int32 userRoleId)
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");
            if (speciesObservationIds.Count > MAX_SPECIES_OBSERVATIONS_WITH_INFORMATION)
            {
                throw new ApplicationException("Max " + MAX_SPECIES_OBSERVATIONS_WITH_INFORMATION + " species observations can be retrieved in one call");
            }
            DatabaseManager.CheckDatabaseUpdate(context);

            // Get observation information from the database.
            try
            {
                // Add information about user selected species observatios to the database.
                AddUserSelectedSpeciesObservations(context, speciesObservationIds);

                speciesObservationInformation = new WebSpeciesObservationInformation();
                using (DataReader dataReader = DataServer.GetSpeciesObservationsById(context,
                                                                                     0, //previousPower,
                                                                                     GetProtectionLevel(context), //securityParameter.Power,
                                                                                     false, //securityParameter.HasTaxonId(),
                                                                                     0, //securityParameter.TaxonId,
                                                                                     false, //securityParameter.HasRegionId(),
                                                                                     0, //securityParameter.RegionId,
                                                                                     0, //securityParameter.Distorsion,
                                                                                     false, //securityParameter.HideAttributes,
                                                                                     SpeciesObservationInformationType.All.ToString()))
                {
                    LoadSpeciesObservations(context, dataReader, speciesObservationInformation, userRoleId);
                }
            }
            finally
            {
                // Clean up.
                DeleteUserSelectedSpeciesObservations(context);
            }

            return speciesObservationInformation;
        }

        /// <summary>
        /// Get taxa that belongs to authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>Taxa that belongs to authority.</returns>
        private static List<WebTaxon> GetTaxa(WebServiceContext context,
                                              WebAuthority authority)
        {
            String authorityTaxaCacheKey;
            List<WebTaxon> taxa;

            taxa = null;
            if (authority.TaxonGUIDs.IsNotEmpty())
            {
                // Get cached information.
                authorityTaxaCacheKey = "AuthorityTaxaCacheKey" +
                                        "#" +
                                        authority.Id;
                taxa = (List<WebTaxon>)(context.GetCachedObject(authorityTaxaCacheKey));

                // Data not in cache - store it in the cache.
                if (taxa.IsNull())
                {
                    taxa = TaxonManager.GetChildTaxa(context,
                                                     authority.TaxonGUIDs,
                                                     TaxonInformationType.Basic);

                    // Add information to cache.
                    context.AddCachedObject(authorityTaxaCacheKey,
                                            taxa,
                                            DateTime.Now + new TimeSpan(0, 1, 0, 0),
                                            CacheItemPriority.BelowNormal);
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get all taxa for the species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public static List<WebTaxon> GetTaxaBySpeciesObservations(WebServiceContext context,
                                                                  WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<Int32> taxonIds;
            List<WebTaxon> taxa;
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            CheckArgument(context, searchCriteria);
            DatabaseManager.CheckDatabaseUpdate(context);
            searchCriteria.CheckData();

            if (context.GetRole(searchCriteria.UserRoleId).IsSimpleSpeciesObservationAccessRights())
            {
                try
                {
                    // Add information about user selected taxa to the database.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.AddUserSelectedTaxa(context, searchCriteria.TaxonIds, UserSelectedTaxonUsage.Input);
                    }

                    using (DataReader dataReader = GetTaxaBySpeciesObservations(context,
                                                                                searchCriteria,
                                                                                GetProtectionLevel(context, searchCriteria)))
                    {
                        taxa = TaxonManager.GetTaxa(dataReader);
                    }

                    return taxa;
                }
                finally
                {
                    // Clean up.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.DeleteUserSelectedTaxa(context);
                    }
                }
            }
            else
            {
                taxa = null;
                speciesObservationInformation = GetSpeciesObservations(context, searchCriteria);
                taxonIds = new List<Int32>();
                if (speciesObservationInformation.SpeciesObservations.IsNotEmpty())
                {
                    foreach (WebSpeciesObservation speciesObservation in speciesObservationInformation.SpeciesObservations)
                    {
                        if (!taxonIds.Contains(speciesObservation.DyntaxaTaxonId))
                        {
                            taxonIds.Add(speciesObservation.DyntaxaTaxonId);
                        }
                    }

                    taxa = WebServiceData.TaxonManager.GetTaxaByIds(context, taxonIds);
                }

                return taxa;
            }
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="protectionLevel">Protection level.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        private static DataReader GetTaxaBySpeciesObservations(WebServiceContext context,
                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                               Int32 protectionLevel)
        {
            Boolean hasCountyId, hasCountyPartId;
            Boolean hasProvinceId, hasProvincePartId;
            Int32 countyId, countyPartId;
            Int32 provinceId, provincePartId;
            Int32 databaseId;
            WebCounty county;
            WebProvince province;

            // Set values.
            if (searchCriteria.DatabaseIds.IsNotEmpty() &&
                (searchCriteria.DatabaseIds.Count == 1))
            {
                databaseId = searchCriteria.DatabaseIds[0];
            }
            else
            {
                databaseId = -1;
            }

            if (searchCriteria.Counties.IsNotEmpty() &&
                (searchCriteria.Counties.Count == 1))
            {
                county = searchCriteria.Counties[0];
                if (county.IsCountyPart)
                {
                    hasCountyId = false;
                    countyId = -1;
                    hasCountyPartId = true;
                    countyPartId = county.Id;
                }
                else
                {
                    hasCountyId = true;
                    countyId = county.Id;
                    hasCountyPartId = false;
                    countyPartId = -1;
                }
            }
            else
            {
                hasCountyId = false;
                countyId = -1;
                hasCountyPartId = false;
                countyPartId = -1;
            }

            if (searchCriteria.Provinces.IsNotEmpty() &&
                (searchCriteria.Provinces.Count == 1))
            {
                province = searchCriteria.Provinces[0];
                if (province.IsProvincePart)
                {
                    hasProvinceId = false;
                    provinceId = -1;
                    hasProvincePartId = true;
                    provincePartId = province.Id;
                }
                else
                {
                    hasProvinceId = true;
                    provinceId = province.Id;
                    hasProvincePartId = false;
                    provincePartId = -1;
                }
            }
            else
            {
                hasProvinceId = false;
                provinceId = -1;
                hasProvincePartId = false;
                provincePartId = -1;
            }

            // Get information from database.
            return DataServer.GetTaxaBySpeciesObservations(context,
                                                           0, //minPower,
                                                           protectionLevel, //maxPower,
                                                           false, //securityParameter.HasTaxonId(),
                                                           0, // securityParameter.TaxonId,
                                                           false, //securityParameter.HasRegionId(),
                                                           0, //securityParameter.RegionId,
                                                           0, //securityParameter.Distorsion,
                                                           false, //securityParameter.HideAttributes,
                                                           searchCriteria.TaxonIds.IsNotEmpty(),
                                                           (Int32)(searchCriteria.UseOfObservationDate),
                                                           searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet,
                                                           searchCriteria.ObservationStartDate,
                                                           searchCriteria.ObservationEndDate,
                                                           (Int32)(searchCriteria.UseOfRegistrationDate),
                                                           searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet,
                                                           searchCriteria.RegistrationStartDate,
                                                           searchCriteria.RegistrationEndDate,
                                                           searchCriteria.IsRectangleSpecified,
                                                           searchCriteria.NorthCoordinate,
                                                           searchCriteria.SouthCoordinate,
                                                           searchCriteria.EastCoordinate,
                                                           searchCriteria.WestCoordinate,
                                                           searchCriteria.IsAccuracySpecified,
                                                           searchCriteria.Accuracy,
                                                           searchCriteria.LocationSearchString,
                                                           searchCriteria.IncludePositiveObservations,
                                                           searchCriteria.IncludeNeverFoundObservations,
                                                           searchCriteria.IncludeNotRediscoveredObservations,
                                                           (databaseId != -1),
                                                           databaseId,
                                                           hasProvinceId,
                                                           provinceId,
                                                           hasProvincePartId,
                                                           provincePartId,
                                                           hasCountyId,
                                                           countyId,
                                                           hasCountyPartId,
                                                           countyPartId,
                                                           searchCriteria.IsBirdNestActivityLevelSpecified,
                                                           searchCriteria.BirdNestActivityLevel,
                                                           searchCriteria.ObserverSearchString,
                                                           searchCriteria.IncludeUncertainTaxonDetermination);
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        public static Int32 GetTaxaCountBySpeciesObservations(WebServiceContext context,
                                                              WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebTaxon> taxa;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context,
                                                                   AuthorityIdentifier.Sighting);

            // Check input arguments.
            CheckArgument(context, searchCriteria);
            DatabaseManager.CheckDatabaseUpdate(context);
            searchCriteria.CheckData();

            if (context.GetRole(searchCriteria.UserRoleId).IsSimpleSpeciesObservationAccessRights())
            {
                try
                {
                    // Add information about user selected taxa to the database.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.AddUserSelectedTaxa(context, searchCriteria.TaxonIds, UserSelectedTaxonUsage.Input);
                        DataServer.UpdateUserSelecedTaxa(context);
                    }

                    // Get information from database.
                    return GetTaxaCountBySpeciesObservations(context,
                                                             searchCriteria,
                                                             GetProtectionLevel(context, searchCriteria));
                }
                finally
                {
                    // Clean up.
                    if (searchCriteria.TaxonIds.IsNotEmpty())
                    {
                        TaxonManager.DeleteUserSelectedTaxa(context);
                    }
                }
            }
            else
            {
                taxa = GetTaxaBySpeciesObservations(context, searchCriteria);
                if (taxa.IsEmpty())
                {
                    return 0;
                }
                else
                {
                    return taxa.Count;
                }
            }
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="protectionLevel">Protection level.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        private static Int32 GetTaxaCountBySpeciesObservations(WebServiceContext context,
                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                               Int32 protectionLevel)
        {
            Boolean hasCountyId, hasCountyPartId;
            Boolean hasProvinceId, hasProvincePartId;
            Int32 countyId, countyPartId;
            Int32 provinceId, provincePartId;
            Int32 databaseId;
            WebCounty county;
            WebProvince province;

            // Set values.
            if (searchCriteria.DatabaseIds.IsNotEmpty() &&
                (searchCriteria.DatabaseIds.Count == 1))
            {
                databaseId = searchCriteria.DatabaseIds[0];
            }
            else
            {
                databaseId = -1;
            }

            if (searchCriteria.Counties.IsNotEmpty() &&
                (searchCriteria.Counties.Count == 1))
            {
                county = searchCriteria.Counties[0];
                if (county.IsCountyPart)
                {
                    hasCountyId = false;
                    countyId = -1;
                    hasCountyPartId = true;
                    countyPartId = county.Id;
                }
                else
                {
                    hasCountyId = true;
                    countyId = county.Id;
                    hasCountyPartId = false;
                    countyPartId = -1;
                }
            }
            else
            {
                hasCountyId = false;
                countyId = -1;
                hasCountyPartId = false;
                countyPartId = -1;
            }

            if (searchCriteria.Provinces.IsNotEmpty() &&
                (searchCriteria.Provinces.Count == 1))
            {
                province = searchCriteria.Provinces[0];
                if (province.IsProvincePart)
                {
                    hasProvinceId = false;
                    provinceId = -1;
                    hasProvincePartId = true;
                    provincePartId = province.Id;
                }
                else
                {
                    hasProvinceId = true;
                    provinceId = province.Id;
                    hasProvincePartId = false;
                    provincePartId = -1;
                }
            }
            else
            {
                hasProvinceId = false;
                provinceId = -1;
                hasProvincePartId = false;
                provincePartId = -1;
            }

            // Get information from database.
            return DataServer.GetTaxaCountBySpeciesObservations(context,
                                                                0, //minPower,
                                                                protectionLevel, //maxPower,
                                                                false, //securityParameter.HasTaxonId(),
                                                                0, //securityParameter.TaxonId,
                                                                false, //securityParameter.HasRegionId(),
                                                                0, //securityParameter.RegionId,
                                                                0, //securityParameter.Distorsion,
                                                                false, //securityParameter.HideAttributes,
                                                                searchCriteria.TaxonIds.IsNotEmpty(),
                                                                (Int32)(searchCriteria.UseOfObservationDate),
                                                                searchCriteria.UseOfObservationDate != WebUseOfDate.NotSet,
                                                                searchCriteria.ObservationStartDate,
                                                                searchCriteria.ObservationEndDate,
                                                                (Int32)(searchCriteria.UseOfRegistrationDate),
                                                                searchCriteria.UseOfRegistrationDate != WebUseOfDate.NotSet,
                                                                searchCriteria.RegistrationStartDate,
                                                                searchCriteria.RegistrationEndDate,
                                                                searchCriteria.IsRectangleSpecified,
                                                                searchCriteria.NorthCoordinate,
                                                                searchCriteria.SouthCoordinate,
                                                                searchCriteria.EastCoordinate,
                                                                searchCriteria.WestCoordinate,
                                                                searchCriteria.IsAccuracySpecified,
                                                                searchCriteria.Accuracy,
                                                                searchCriteria.LocationSearchString,
                                                                searchCriteria.IncludePositiveObservations,
                                                                searchCriteria.IncludeNeverFoundObservations,
                                                                searchCriteria.IncludeNotRediscoveredObservations,
                                                                (databaseId != -1),
                                                                databaseId,
                                                                hasProvinceId,
                                                                provinceId,
                                                                hasProvincePartId,
                                                                provincePartId,
                                                                hasCountyId,
                                                                countyId,
                                                                hasCountyPartId,
                                                                countyPartId,
                                                                searchCriteria.IsBirdNestActivityLevelSpecified,
                                                                searchCriteria.BirdNestActivityLevel,
                                                                searchCriteria.ObserverSearchString,
                                                                searchCriteria.IncludeUncertainTaxonDetermination);
        }

        /// <summary>
        /// Get table structure that is used to insert user
        /// selected species observations into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>User selected obervation table structure.</returns>
        public static DataTable GetUserSelectedSpeciesObservationsTable(WebServiceContext context)
        {
            DataColumn column;
            DataTable speciesObservationTable;

            speciesObservationTable = new DataTable(UserSelectedSpeciesObservationsData.TABLE_NAME);
            column = new DataColumn(UserSelectedSpeciesObservationsData.REQUEST_ID, typeof(Int32));
            speciesObservationTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesObservationsData.SPECIES_OBSERVATION_ID, typeof(Int64));
            speciesObservationTable.Columns.Add(column);
            return speciesObservationTable;
        }

        /// <summary>
        /// Load species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='dataReader'>An open data reader.</param>
        /// <param name='speciesObservationInformation'>Load species observation information into this instance.</param>
        /// <param name='roleId'>Role id.</param>
        private static void LoadSpeciesObservations(WebServiceContext context,
                                                    DataReader dataReader,
                                                    WebSpeciesObservationInformation speciesObservationInformation,
                                                    Int32 roleId)
        {
            Boolean isSimpleSpeciesObservationAccessRights;
            Int64 speciesObservationCount;
            WebSpeciesObservation speciesObservation;

            speciesObservationCount = 0;
            isSimpleSpeciesObservationAccessRights = context.GetRole(roleId).IsSimpleSpeciesObservationAccessRights();
            while (dataReader.Read())
            {
                speciesObservationCount++;
                if (speciesObservationCount <= speciesObservationInformation.MaxSpeciesObservationCount)
                {
                    // Add species observation with information.
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationInformation.SpeciesObservationCount++;
                        speciesObservationInformation.SpeciesObservations.Add(speciesObservation);
                    }
                    else
                    {
                        if (CheckAccessRights(context, speciesObservation, roleId))
                        {
                            speciesObservationInformation.SpeciesObservationCount++;
                            speciesObservationInformation.SpeciesObservations.Add(speciesObservation);
                        }
                    }
                }
                else
                {
                    if (speciesObservationCount == (speciesObservationInformation.MaxSpeciesObservationCount + 1))
                    {
                        // To many species observations.
                        // Return only species observation ids.
                        // Move species observation ids from 
                        // SpeciesObservations to SpeciesObservationIds.
                        speciesObservationInformation.SpeciesObservationIds = new List<Int64>();
                        foreach (WebSpeciesObservation webSpeciesObservation in speciesObservationInformation.SpeciesObservations)
                        {
                            speciesObservationInformation.SpeciesObservationIds.Add(webSpeciesObservation.Id);
                        }

                        speciesObservationInformation.SpeciesObservations = null;
                    }

                    // Add only species observation id.
                    if (isSimpleSpeciesObservationAccessRights)
                    {
                        speciesObservationInformation.SpeciesObservationCount++;
                        speciesObservationInformation.SpeciesObservationIds.Add(dataReader.GetInt64(SpeciesObservationData.ID));
                    }
                    else
                    {
                        speciesObservation = new WebSpeciesObservation(dataReader);
                        if (CheckAccessRights(context, speciesObservation, roleId))
                        {
                            speciesObservationInformation.SpeciesObservationCount++;
                            speciesObservationInformation.SpeciesObservationIds.Add(speciesObservation.Id);
                        }
                    }
                }

                if (speciesObservationInformation.SpeciesObservationCount > MAX_SPECIES_OBSERVATIONS)
                {
                    throw new ApplicationException("To many species observations was retrieved!, Limit is set to " + MAX_SPECIES_OBSERVATIONS + " observations.");
                }
            }
        }
    }
}
