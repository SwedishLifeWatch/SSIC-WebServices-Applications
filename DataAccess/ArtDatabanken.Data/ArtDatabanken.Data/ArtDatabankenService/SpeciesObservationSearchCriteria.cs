using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds parameters used to
    /// search for species observations.
    /// </summary>
    public class SpeciesObservationSearchCriteria
    {
        private List<Int32> _databaseIds;
        private List<Int32> _taxonIds;
        private CountyList _counties;
        private ProvinceList _provinces;

        /// <summary>
        /// Create a SpeciesObservationSearchCriteria instance.
        /// </summary>
        public SpeciesObservationSearchCriteria()
        {
            Accuracy = Int32.MinValue;
            BirdNestActivityLevel = Int32.MinValue;
            Counties = new CountyList();
            DatabaseIds = new List<Int32>();
            HasAccuracy = false;
            HasBirdNestActivityLevel = false;
            HasBoundingBox = false;
            IncludeNeverFoundObservations = false;
            IncludeNotRediscoveredObservations = false;
            IncludePositiveObservations = true;
            IncludeUncertainTaxonDetermination = true;
            LocalitySearchString = null;
            MaxEastCoordinate = Int32.MinValue;
            MaxWestCoordinate = Int32.MinValue;
            MaxNorthCoordinate = Int32.MinValue;
            MaxSouthCoordinate = Int32.MinValue;
            Provinces = new ProvinceList();
            TaxonIds = new List<Int32>();

            if (UserManager.IsUserLoggedIn())
            {
                UserRoleId = UserManager.GetUser().Roles[0].Id;
            }
            else
            {
                UserRoleId = Int32.MinValue;
            }

            ObservationEndDate = DateTime.MinValue;
            ObservationStartDate = DateTime.MinValue;
            ObserverSearchString = null;
            UseOfObservationDate = WebUseOfDate.NotSet;
            RegistrationEndDate = DateTime.MinValue;
            RegistrationStartDate = DateTime.MinValue;
            UseOfRegistrationDate = WebUseOfDate.NotSet;
        }

        /// <summary>
        /// Search for observations with a accuracy value
        /// for the coordinates that is not larger than
        /// the specified accuray value.
        /// HasAccuracy must be set to true if Accuracy is used.
        /// </summary>
        public Int32 Accuracy
        { get; set; }

        /// <summary>
        /// Search for observations with a bird nest activity level
        /// that is equal or stronger than the specified value.
        /// HasBirdNestActivityLevel must be set to true if
        /// BirdNestActivityLevel is used.
        /// </summary>
        public Int32 BirdNestActivityLevel
        { get; set; }

        /// <summary>
        /// Counties to search in.
        /// All counties are used if no county is provided.
        /// </summary>
        public CountyList Counties
        {
            get
            {
                if (_counties.IsNull())
                {
                    _counties = new CountyList();
                }
                return _counties;
            }
            set { _counties = value; }
        }

        /// <summary>
        /// Databases to search in.
        /// All databases are used if no database is provided.
        /// </summary>
        public List<Int32> DatabaseIds
        {
            get
            {
                if (_databaseIds.IsNull())
                {
                    _databaseIds = new List<Int32>();
                }
                return _databaseIds;
            }
            set { _databaseIds = value; }
        }

        /// <summary>
        /// Handle if accuracy has been set.
        /// </summary>
        public Boolean HasAccuracy
        { get; set; }

        /// <summary>
        /// Handle if bird nest activity level has been set.
        /// </summary>
        public Boolean HasBirdNestActivityLevel
        { get; set; }

        /// <summary>
        /// Handle if bounding box has been set.
        /// </summary>
        public Boolean HasBoundingBox
        { get; set; }

        /// <summary>
        /// Handle indication whether to search for never found observations.
        /// </summary>
        public Boolean IncludeNeverFoundObservations
        { get; set; }

        /// <summary>
        /// Handle indication whether to search for not rediscovered observations.
        /// </summary>
        public Boolean IncludeNotRediscoveredObservations
        { get; set; }

        /// <summary>
        /// Handle indication whether to search for positive observations.
        /// </summary>
        public Boolean IncludePositiveObservations
        { get; set; }

        /// <summary>
        /// Include observations where observer is uncertain about taxon determination..
        /// </summary>
        public Boolean IncludeUncertainTaxonDetermination
        { get; set; }

        /// <summary>
        /// String to match with locality names.
        /// SQL Server wildcard characters can be used.
        /// All localities are used if LocalitySearchString is empty.
        /// </summary>
        public String LocalitySearchString
        { get; set; }

        /// <summary>
        /// Search for observations that are located west of this coordinate.
        /// HasBoundingBox must be set to true if MaxEastCoordinate is used.
        /// </summary>
        public Int32 MaxEastCoordinate
        { get; set; }

        /// <summary>
        /// Search for observations that are located south of this coordinate.
        /// HasBoundingBox must be set to true if MaxNorthCoordinate is used.
        /// </summary>
        public Int32 MaxNorthCoordinate
        { get; set; }

        /// <summary>
        /// Search for observations that are located north of this coordinate.
        /// HasBoundingBox must be set to true if MaxSouthCoordinate is used.
        /// </summary>
        public Int32 MaxSouthCoordinate
        { get; set; }

        /// <summary>
        /// Search for observations that are located east of this coordinate.
        /// HasBoundingBox must be set to true if MaxWestCoordinate is used.
        /// </summary>
        public Int32 MaxWestCoordinate
        { get; set; }

        /// <summary>
        /// Handle the observation end date.
        /// </summary>
        public DateTime ObservationEndDate
        { get; set; }

        /// <summary>
        /// Handle the observation start date.
        /// </summary>
        public DateTime ObservationStartDate
        { get; set; }

        /// <summary>
        /// String to match with observer names. Wild cards may be used (% or _).
        /// All observers are used if ObserverSearchString is empty.
        /// </summary>
        public String ObserverSearchString
        { get; set; }

        /// <summary>
        /// Provinces to use for the search.
        /// All provinces are used if no province is provided.
        /// </summary>
        public ProvinceList Provinces
        {
            get
            {
                if (_provinces.IsNull())
                {
                    _provinces = new ProvinceList();
                }
                return _provinces;
            }
            set { _provinces = value; }
        }

        /// <summary>
        /// Handle the registration end date.
        /// </summary>
        public DateTime RegistrationEndDate
        { get; set; }

        /// <summary>
        /// Handle the registration start date.
        /// </summary>
        public DateTime RegistrationStartDate
        { get; set; }

        /// <summary>
        /// Search for observations of the specified taxa.
        /// </summary>
        public List<Int32> TaxonIds
        {
            get
            {
                if (_taxonIds.IsNull())
                {
                    _taxonIds = new List<Int32>();
                }
                return _taxonIds;
            }
            set { _taxonIds = value; }
        }

        /// <summary>
        /// Handle how to use selected observation start- and enddate.
        /// </summary>
        public WebUseOfDate UseOfObservationDate
        { get; set; }

        /// <summary>
        /// Handle how to use selected registration start- and end registrationdate.
        /// </summary>
        public WebUseOfDate UseOfRegistrationDate
        { get; set; }

        /// <summary>
        /// In which user role for (the currently logged in user)
        /// should the observation search be made.
        /// </summary>
        public Int32 UserRoleId
        { get; set; }
    }
}
