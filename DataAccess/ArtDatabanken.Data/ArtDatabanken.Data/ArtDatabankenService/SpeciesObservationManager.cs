using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Manager of species observation information.
    /// </summary>
    public class SpeciesObservationManager : ManagerBase
    {
        /// <summary>
        /// The _bird nest activities.
        /// </summary>
        private static BirdNestActivityList _birdNestActivities;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SpeciesObservationManager()
        {
            RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _birdNestActivities thread safe.
        /// </summary>
        private static BirdNestActivityList BirdNestActivities
        {
            get
            {
                BirdNestActivityList birdNestActivities;

                lock (_lockObject)
                {
                    birdNestActivities = _birdNestActivities;
                }

                return birdNestActivities;
            }

            set
            {
                lock (_lockObject)
                {
                    _birdNestActivities = value;
                }
            }
        }

        /// <summary>
        /// Check that changed dates are ok.
        /// </summary>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <exception cref="ArgumentException">Thrown if information in changed dates are invalid.</exception>
        private static void CheckChangedDates(DateTime changedFrom,
                                              DateTime changedTo)
        {
            DateTime today;

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
        }

        /// <summary>
        /// Get all bird nest activities objects.
        /// </summary>
        /// <returns>All bird nest activities.</returns>
        public static BirdNestActivityList GetBirdNestActivities()
        {
            BirdNestActivityList birdNestActivities = null;

            for (Int32 getAttempts = 0; (birdNestActivities.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadBirdNestActivities();
                birdNestActivities = BirdNestActivities;
            }

            return birdNestActivities;
        }

        /// <summary>
        /// Convert a WebSpeciesObservation to a SpeciesObservation.
        /// </summary>
        /// <param name="webSpeciesObservation">The WebSpeciesObservation.</param>
        /// <returns>The SpeciesObservation.</returns>
        public static SpeciesObservation GetSpeciesObservation(WebSpeciesObservation webSpeciesObservation)
        {
            DataFieldList dataFields;
            Int32? accuracy, speciesActivityId;
            SpeciesObservation speciesObservation;

            dataFields = new DataFieldList(webSpeciesObservation.DataFields);
            accuracy = null;
            if (dataFields.IsValueSpecified(SpeciesObservation.ACCURACY_DATA_FIELD))
            {
                accuracy = dataFields.GetInt32(SpeciesObservation.ACCURACY_DATA_FIELD);
            }

            speciesActivityId = null;
            if (dataFields.IsValueSpecified(SpeciesObservation.SPECIES_ACTIVITY_ID_DATA_FIELD))
            {
                speciesActivityId = dataFields.GetInt32(SpeciesObservation.SPECIES_ACTIVITY_ID_DATA_FIELD);
            }

            speciesObservation = new SpeciesObservation(webSpeciesObservation.Id,
                                                        dataFields.GetString(SpeciesObservation.ORGANISM_GROUP_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.SCIENTIFIC_NAME_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.TAXON_UNCERTAINTY_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.COMMON_NAME_DATA_FIELD),
                                                        dataFields.GetDateTime(SpeciesObservation.START_DATE_DATA_FIELD),
                                                        dataFields.GetDateTime(SpeciesObservation.END_DATE_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.LOCALITY_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.PARISH_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.MUNICIPALITY_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.COUNTY_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.PROVINCE_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.NORTH_COORDINATE_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.EAST_COORDINATE_DATA_FIELD),
                                                        accuracy,
                                                        dataFields.GetString(SpeciesObservation.OBSERVERS_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.ORIGIN_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.QUANTITY_OR_AREA_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.UNIT_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.LIFE_STAGE_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.ACTIVITY_OR_SUBSTRATE_COUNT_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.BIOTOPE_OR_SUBSTRATE_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.COMMENT_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.DETERMINATOR_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.COLLECTION_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.ACCESSION_ID_DATA_FIELD),
                                                        dataFields.GetBoolean(SpeciesObservation.NOT_REDISCOVERED_DATA_FIELD),
                                                        dataFields.GetBoolean(SpeciesObservation.NEVER_FOUND_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.DATABASE_OBSERVATION_ID_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.DATABASE_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.TAXON_SORT_ORDER_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.TAXON_ID_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.ORGANISM_GROUP_SORT_ORDER_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.PROTECTION_LEVEL_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.SCI_CODE_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.SCI_NAME_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.REDLIST_CATEGORY_DATA_FIELD),
                                                        dataFields.GetString(SpeciesObservation.GUID_DATA_FIELD),
                                                        dataFields.GetInt32(SpeciesObservation.DATABASE_ID_DATA_FIELD),
                                                        dataFields.GetDateTime(SpeciesObservation.REPORTED_DATE_DATA_FIELD),
                                                        speciesActivityId,
                                                        dataFields.GetDateTime(SpeciesObservation.MODIFIED_DATA_FIELD));
            return speciesObservation;
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
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
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <returns>Information about changed species observations.</returns>
        public static SpeciesObservationChange GetSpeciesObservationChange(DateTime changedFrom, DateTime changedTo)
        {
            Int32 index, speciesObservationIdsIndex;
            List<Int64> speciesObservationIds;
            SpeciesObservationChange change;
            WebSpeciesObservationChange webChange;
            WebSpeciesObservationInformation webInformation;

            // Check arguments.
            CheckChangedDates(changedFrom, changedTo);

            // Get data from web service.
            webChange = WebServiceClient.GetSpeciesObservationChange(changedFrom, changedTo);

            change = new SpeciesObservationChange();

            if (webChange.IsNotNull())
            {
                // Handle deleted species observations.
                change.DeletedSpeciesObservationGuids = webChange.DeletedSpeciesObservationGuids;

                // Handle new species observations.
                change.NewSpeciesObservations = new SpeciesObservationList();
                if (webChange.NewSpeciesObservations.IsEmpty() && webChange.NewSpeciesObservationIds.IsNotEmpty())
                {
                    // Get species observations in parts.
                    speciesObservationIds = new List<Int64>();
                    index = 0;
                    if (Configuration.Debug)
                    {
                        webChange.MaxSpeciesObservationCount = Math.Min(20000, webChange.MaxSpeciesObservationCount);
                    }

                    for (speciesObservationIdsIndex = 0;
                         speciesObservationIdsIndex < webChange.NewSpeciesObservationIds.Count;
                         speciesObservationIdsIndex++)
                    {
                        speciesObservationIds.Add(webChange.NewSpeciesObservationIds[speciesObservationIdsIndex]);
                        if (++index == webChange.MaxSpeciesObservationCount)
                        {
                            // Get one part of species observations.
                            webInformation = WebServiceClient.GetSpeciesObservationsById(
                                speciesObservationIds,
                                UserManager.GetUser().Roles[0].Id);
                            foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                            {
                                change.NewSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                            }

                            if ((webChange.NewSpeciesObservationIds.Count - 1 - speciesObservationIdsIndex)
                                < webChange.MaxSpeciesObservationCount)
                            {
                                speciesObservationIds = new List<Int64>();
                            }

                            index = 0;
                            speciesObservationIds.Clear();
                        }
                    }

                    if (speciesObservationIds.IsNotEmpty())
                    {
                        webInformation = WebServiceClient.GetSpeciesObservationsById(
                            speciesObservationIds,
                            UserManager.GetUser().Roles[0].Id);
                        foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                        {
                            change.NewSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                        }
                    }
                }
                else if (webChange.NewSpeciesObservations.IsNotEmpty())
                {
                    foreach (WebSpeciesObservation webSpeciesObservation in webChange.NewSpeciesObservations)
                    {
                        change.NewSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                    }
                }

                // Handle updated species observations.
                change.UpdatedSpeciesObservations = new SpeciesObservationList();
                if (webChange.UpdatedSpeciesObservations.IsEmpty()
                    && webChange.UpdatedSpeciesObservationIds.IsNotEmpty())
                {
                    // Get species observations in parts.
                    speciesObservationIds = new List<Int64>();
                    index = 0;
                    for (speciesObservationIdsIndex = 0;
                         speciesObservationIdsIndex < webChange.UpdatedSpeciesObservationIds.Count;
                         speciesObservationIdsIndex++)
                    {
                        speciesObservationIds.Add(webChange.UpdatedSpeciesObservationIds[speciesObservationIdsIndex]);
                        if (++index == webChange.MaxSpeciesObservationCount)
                        {
                            // Get one part of species observations.
                            webInformation = WebServiceClient.GetSpeciesObservationsById(
                                speciesObservationIds,
                                UserManager.GetUser().Roles[0].Id);
                            foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                            {
                                change.UpdatedSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                            }

                            if ((webChange.UpdatedSpeciesObservationIds.Count - 1 - speciesObservationIdsIndex)
                                < webChange.MaxSpeciesObservationCount)
                            {
                                speciesObservationIds = new List<Int64>();
                            }

                            index = 0;
                            speciesObservationIds.Clear();
                        }
                    }

                    if (speciesObservationIds.IsNotEmpty())
                    {
                        webInformation = WebServiceClient.GetSpeciesObservationsById(
                            speciesObservationIds,
                            UserManager.GetUser().Roles[0].Id);
                        foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                        {
                            change.UpdatedSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                        }
                    }
                }
                else if (webChange.UpdatedSpeciesObservations.IsNotEmpty())
                {
                    foreach (WebSpeciesObservation webSpeciesObservation in webChange.UpdatedSpeciesObservations)
                    {
                        change.UpdatedSpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                    }
                }
            }

            return change;
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria for the species observations.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        public static Int32 GetSpeciesObservationCount(SpeciesObservationSearchCriteria searchCriteria)
        {
            WebSpeciesObservationSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            webSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            return WebServiceClient.GetSpeciesObservationCount(webSearchCriteria);
        }

        /// <summary>
        /// Get SpeciesObservations that fulfill the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria for the species observations.</param>
        /// <returns>Information about species observations that fulfill the search criteria.</returns>
        public static SpeciesObservationInformation GetSpeciesObservations(SpeciesObservationSearchCriteria searchCriteria)
        {
            Int32 index, speciesObservationIdsIndex;
            List<Int64> speciesObservationIds;
            SpeciesObservationInformation information;
            WebSpeciesObservationInformation tempWebInformation, webInformation;
            WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            speciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            webInformation = WebServiceClient.GetSpeciesObservations(speciesObservationSearchCriteria);

            information = new SpeciesObservationInformation();
            if (webInformation.SpeciesObservations.IsEmpty() &&
                webInformation.SpeciesObservationIds.IsNotEmpty())
            {
                // Get species observations in parts.
                speciesObservationIds = new List<Int64>();
                index = 0;
                for (speciesObservationIdsIndex = 0; speciesObservationIdsIndex < webInformation.SpeciesObservationIds.Count; speciesObservationIdsIndex++)
                {
                    speciesObservationIds.Add(webInformation.SpeciesObservationIds[speciesObservationIdsIndex]);
                    if (++index == webInformation.MaxSpeciesObservationCount)
                    {
                        // Get one part of species observations.
                        tempWebInformation = WebServiceClient.GetSpeciesObservationsById(speciesObservationIds,
                                                                                         speciesObservationSearchCriteria.UserRoleId);
                        foreach (WebSpeciesObservation webSpeciesObservation in tempWebInformation.SpeciesObservations)
                        {
                            information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                        }

                        if ((webInformation.SpeciesObservationIds.Count - 1 - speciesObservationIdsIndex) <
                            webInformation.MaxSpeciesObservationCount)
                        {
                            speciesObservationIds = new List<Int64>();
                        }

                        index = 0;
                        speciesObservationIds.Clear();
                    }
                }

                if (speciesObservationIds.IsNotEmpty())
                {
                    tempWebInformation = WebServiceClient.GetSpeciesObservationsById(speciesObservationIds,
                                                                                     speciesObservationSearchCriteria.UserRoleId);
                    foreach (WebSpeciesObservation webSpeciesObservation in tempWebInformation.SpeciesObservations)
                    {
                        information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                    }
                }
            }
            else if (webInformation.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                {
                    information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                }
            }

            return information;
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 1000000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="userRoleId">In which role is the user retrieving species observations.</param>
        /// <returns>Species observations.</returns>
        public static SpeciesObservationInformation GetSpeciesObservations(List<Int64> speciesObservationIds,
                                                                           Int32 userRoleId)
        {
            Int32 index, speciesObservationIdsIndex;
            SpeciesObservationInformation information;
            WebSpeciesObservationInformation tempWebInformation, webInformation;

            // Check arguments.
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");

            // Get data from web service.
            webInformation = WebServiceClient.GetSpeciesObservationsById(speciesObservationIds,
                                                                         userRoleId);

            information = new SpeciesObservationInformation();
            if (webInformation.SpeciesObservations.IsEmpty() &&
                webInformation.SpeciesObservationIds.IsNotEmpty())
            {
                // Get species observations in parts.
                speciesObservationIds = new List<Int64>();
                index = 0;
                for (speciesObservationIdsIndex = 0; speciesObservationIdsIndex < webInformation.SpeciesObservationIds.Count; speciesObservationIdsIndex++)
                {
                    speciesObservationIds.Add(webInformation.SpeciesObservationIds[speciesObservationIdsIndex]);
                    if (++index == webInformation.MaxSpeciesObservationCount)
                    {
                        // Get one part of species observations.
                        tempWebInformation = WebServiceClient.GetSpeciesObservationsById(speciesObservationIds,
                                                                                         userRoleId);
                        foreach (WebSpeciesObservation webSpeciesObservation in tempWebInformation.SpeciesObservations)
                        {
                            information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                        }

                        if ((webInformation.SpeciesObservationIds.Count - 1 - speciesObservationIdsIndex) <
                            webInformation.MaxSpeciesObservationCount)
                        {
                            speciesObservationIds = new List<Int64>();
                        }

                        index = 0;
                        speciesObservationIds.Clear();
                    }
                }

                if (speciesObservationIds.IsNotEmpty())
                {
                    tempWebInformation = WebServiceClient.GetSpeciesObservationsById(speciesObservationIds,
                                                                                     userRoleId);
                    foreach (WebSpeciesObservation webSpeciesObservation in tempWebInformation.SpeciesObservations)
                    {
                        information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                    }
                }
            }
            else if (webInformation.SpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation webSpeciesObservation in webInformation.SpeciesObservations)
                {
                    information.SpeciesObservations.Add(GetSpeciesObservation(webSpeciesObservation));
                }
            }

            return information;
        }

        /// <summary>
        /// Convert a SpeciesObservationSearchCriteria to a
        /// WebSpeciesObservationSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">The SpeciesObservationSearchCriteria.</param>
        /// <returns>The WebSpeciesObservationSearchCriteria.</returns>
        private static WebSpeciesObservationSearchCriteria GetSpeciesObservationSearchCriteria(SpeciesObservationSearchCriteria searchCriteria)
        {
            WebDataField dataField;
            WebSpeciesObservationSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebSpeciesObservationSearchCriteria();
            webSearchCriteria.IsAccuracySpecified = searchCriteria.HasAccuracy;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IsAccuracySpecifiedSpecified = true;
#endif
            if (webSearchCriteria.IsAccuracySpecified)
            {
                webSearchCriteria.Accuracy = searchCriteria.Accuracy;
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.AccuracySpecified = true;
#endif
            }

            webSearchCriteria.IsBirdNestActivityLevelSpecified = searchCriteria.HasBirdNestActivityLevel;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IsBirdNestActivityLevelSpecifiedSpecified = true;
#endif
            if (webSearchCriteria.IsBirdNestActivityLevelSpecified)
            {
                webSearchCriteria.BirdNestActivityLevel = searchCriteria.BirdNestActivityLevel;
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.BirdNestActivityLevelSpecified = true;
#endif
            }

            webSearchCriteria.Counties = GeographicManager.GetCounties(searchCriteria.Counties);

            webSearchCriteria.DatabaseIds = null;
            if (searchCriteria.DatabaseIds.IsNotEmpty())
            {
                webSearchCriteria.DatabaseIds = searchCriteria.DatabaseIds;
            }

            webSearchCriteria.IncludeNeverFoundObservations = searchCriteria.IncludeNeverFoundObservations;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IncludeNeverFoundObservationsSpecified = true;
#endif
            webSearchCriteria.IncludeNotRediscoveredObservations = searchCriteria.IncludeNotRediscoveredObservations;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IncludeNotRediscoveredObservationsSpecified = true;
#endif
            webSearchCriteria.IncludePositiveObservations = searchCriteria.IncludePositiveObservations;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IncludePositiveObservationsSpecified = true;
#endif

            if (webSearchCriteria.DataFields.IsNull())
            {
                webSearchCriteria.DataFields = new List<WebDataField>();
            }

            dataField = new WebDataField();
            dataField.Name = "IncludeUncertainTaxonDetermination";
            dataField.Type = WebDataType.Boolean;
            dataField.Value = searchCriteria.IncludeUncertainTaxonDetermination.WebToString();
            webSearchCriteria.DataFields.Add(dataField);
 
            webSearchCriteria.LocationSearchString = searchCriteria.LocalitySearchString;

            webSearchCriteria.IsRectangleSpecified = searchCriteria.HasBoundingBox;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.IsRectangleSpecifiedSpecified = true;
#endif
            if (webSearchCriteria.IsRectangleSpecified)
            {
                webSearchCriteria.EastCoordinate = searchCriteria.MaxEastCoordinate;
                webSearchCriteria.NorthCoordinate = searchCriteria.MaxNorthCoordinate;
                webSearchCriteria.WestCoordinate = searchCriteria.MaxWestCoordinate;
                webSearchCriteria.SouthCoordinate = searchCriteria.MaxSouthCoordinate;
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.EastCoordinateSpecified = true;
#endif
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.NorthCoordinateSpecified = true;
#endif
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.WestCoordinateSpecified = true;
#endif
#if DATA_SPECIFIED_EXISTS
                webSearchCriteria.SouthCoordinateSpecified = true;
#endif
            }

            webSearchCriteria.ObservationEndDate = searchCriteria.ObservationEndDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.ObservationEndDateSpecified = true;
#endif
            webSearchCriteria.ObservationStartDate = searchCriteria.ObservationStartDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.ObservationStartDateSpecified = true;
#endif
            webSearchCriteria.ObserverSearchString = searchCriteria.ObserverSearchString;
            webSearchCriteria.UseOfObservationDate = searchCriteria.UseOfObservationDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.UseOfObservationDateSpecified = true;
#endif

            webSearchCriteria.RegistrationEndDate = searchCriteria.RegistrationEndDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RegistrationEndDateSpecified = true;
#endif
            webSearchCriteria.RegistrationStartDate = searchCriteria.RegistrationStartDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.RegistrationStartDateSpecified = true;
#endif
            webSearchCriteria.UseOfRegistrationDate = searchCriteria.UseOfRegistrationDate;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.UseOfRegistrationDateSpecified = true;
#endif

            webSearchCriteria.Provinces = GeographicManager.GetProvinces(searchCriteria.Provinces);

            webSearchCriteria.TaxonIds = null;
            if (searchCriteria.TaxonIds.IsNotEmpty())
            {
                webSearchCriteria.TaxonIds = searchCriteria.TaxonIds;
            }

            webSearchCriteria.UserRoleId = searchCriteria.UserRoleId;
#if DATA_SPECIFIED_EXISTS
            webSearchCriteria.UserRoleIdSpecified = true;
#endif

            return webSearchCriteria;
        }

        /// <summary>
        /// Get all taxa for the species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria for the species observations.</param>
        /// <returns>Taxa information.</returns>
        public static TaxonList GetTaxaBySpeciesObservations(SpeciesObservationSearchCriteria searchCriteria)
        {
            WebSpeciesObservationSearchCriteria webSearchCriteria;
            List<WebTaxon> webTaxa;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            webSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            webTaxa = WebServiceClient.GetTaxaBySpeciesObservations(webSearchCriteria);
            return TaxonManager.GetTaxa(webTaxa);
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria for the species observations.</param>
        /// <returns>Taxa count.</returns>
        public static Int32 GetTaxaCountBySpeciesObservations(SpeciesObservationSearchCriteria searchCriteria)
        {
            WebSpeciesObservationSearchCriteria webSearchCriteria;

            // Check arguments.
            searchCriteria.CheckNotNull("searchCriteria");

            // Get data from web service.
            webSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            return WebServiceClient.GetTaxaCountBySpeciesObservations(webSearchCriteria);
        }
 
        /// <summary>
        /// Get bird nest activities from web service.
        /// </summary>
        private static void LoadBirdNestActivities()
        {
            BirdNestActivityList birdNestActivities;

            if (BirdNestActivities.IsNull())
            {
                // Get data from web service.
                birdNestActivities = new BirdNestActivityList();
                foreach (WebBirdNestActivity webBirdNestActivity in WebServiceClient.GetBirdNestActivities())
                {
                    birdNestActivities.Add(new BirdNestActivity(webBirdNestActivity.Id,
                                                                webBirdNestActivity.Name));
                }

                BirdNestActivities = birdNestActivities;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            BirdNestActivities = null;
        }
    }
}
