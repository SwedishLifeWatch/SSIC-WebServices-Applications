using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Definition of how the date parameters are used.
    /// </summary>
    [DataContract]
    public enum WebUseOfDate
    {
        /// <summary>
        /// Don't use date parameter.
        /// </summary>
        [EnumMember]
        NotSet = 0,
        /// <summary>
        /// Use date parameters as absolute values.
        /// </summary>
        [EnumMember]
        Precise = 1,
        /// <summary>
        /// Use the month and day part of the date parameters.
        /// </summary>
        [EnumMember]
        IgnoreYear = 2
    }

    /// <summary>
    /// This class holds parameters used to
    /// search the databases for observations.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebSpeciesObservationSearchCriteria instance.
        /// </summary>
        public WebSpeciesObservationSearchCriteria()
        {
            Counties = new List<WebCounty>();
            DatabaseIds = new List<Int32>();
            IncludeNeverFoundObservations = false;
            IncludeNotRediscoveredObservations = false;
            IncludePositiveObservations = true;
            IsAccuracySpecified = false;
            IsBirdNestActivityLevelSpecified = false;
            IsRectangleSpecified = false;
            LocationSearchString = null;
            Provinces = new List<WebProvince>();
            TaxonIds = new List<Int32>();
            UseOfObservationDate = WebUseOfDate.NotSet;
            UseOfRegistrationDate = WebUseOfDate.NotSet;
        }

        /// <summary>
        /// Gets or sets the wanted accuracy of the coordinates.
        /// </summary>
        [DataMember]
        public Int32 Accuracy
        { get; set; }

        /// <summary>
        /// Gets or sets the bird nest activity level.
        /// </summary>
        [DataMember]
        public Int32 BirdNestActivityLevel
        { get; set; }

        /// <summary>
        /// All counties are used if no county is provided.
        /// List of counties to use for the search.
        /// </summary>
        [DataMember]
        public List<WebCounty> Counties
        { get; set; }

        /// <summary>
        /// All databases are used if no database is provided.
        /// List of databases to use for the search.
        /// </summary>
        [DataMember]
        public List<Int32> DatabaseIds
        { get; set; }

        /// <summary>
        /// Gets or sets the east coordinate of the rectangle.
        /// </summary>
        [DataMember]
        public Int32 EastCoordinate
        { get; set; }

        /// <summary>
        /// Indicates if accuracy has been set
        /// </summary>
        [DataMember]
        public Boolean IsAccuracySpecified
        { get; set; }

        /// <summary>
        /// Indicates if bird nest activity level has been set
        /// </summary>
        [DataMember]
        public Boolean IsBirdNestActivityLevelSpecified
        { get; set; }

        /// <summary>
        /// Indicates if a rectangle has been specified with
        /// NorthCoordinate, EastCoordinate, SouthCoordinate and
        /// WestCoordinate.
        /// Only observations inside this rectangle is returned.
        /// </summary>
        [DataMember]
        public Boolean IsRectangleSpecified
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to search for never found observations.
        /// </summary>
        [DataMember]
        public Boolean IncludeNeverFoundObservations
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to search for not rediscovered observations.
        /// </summary>
        [DataMember]
        public Boolean IncludeNotRediscoveredObservations
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to search for positive observations.
        /// </summary>
        [DataMember]
        public Boolean IncludePositiveObservations
        { get; set; }

        /// <summary>
        /// Include observations where observer is uncertain about taxon determination..
        /// </summary>
        public Boolean IncludeUncertainTaxonDetermination
        {
            get
            {
                if (HasDataField("IncludeUncertainTaxonDetermination"))
                {
                    return GetDataBoolean("IncludeUncertainTaxonDetermination");
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (HasDataField("IncludeUncertainTaxonDetermination"))
                {
                    SetDataBoolean("IncludeUncertainTaxonDetermination", value);
                }
                else
                {
                    WebDataField dataField;

                    if (DataFields.IsNull())
                    {
                        DataFields = new List<WebDataField>();
                    }
                    dataField = new WebDataField();
                    dataField.Name = "IncludeUncertainTaxonDetermination";
                    dataField.Type = WebDataType.Boolean;
                    dataField.Value = value.WebToString();
                    DataFields.Add(dataField);
                }
            }
        }

        /// <summary>
        /// String to match with location names.
        /// Wild cards may be used (% or _).
        /// All locations are used if LocationSearchString is empty.
        /// </summary>
        [DataMember]
        public String LocationSearchString
        { get; set; }

        /// <summary>
        /// Gets or sets the north coordinate of the rectangle.
        /// </summary>
        [DataMember]
        public Int32 NorthCoordinate
        { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [DataMember]
        public DateTime ObservationEndDate
        { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [DataMember]
        public DateTime ObservationStartDate
        { get; set; }

        /// <summary>
        /// String to match with observer names.
        /// Wild cards may be used (% or _).
        /// All observers are used if ObserverSearchString is empty.
        /// </summary>
        [DataMember]
        public String ObserverSearchString
        { get; set; }

        /// <summary>
        /// List of provinces to use for the search.
        /// All provinces are used if no province is provided.
        /// </summary>
        [DataMember]
        public List<WebProvince> Provinces
        { get; set; }

        /// <summary>
        /// Gets or sets the registration end date.
        /// </summary>
        [DataMember]
        public DateTime RegistrationEndDate
        { get; set; }

        /// <summary>
        /// Gets or sets the registration start date.
        /// </summary>
        [DataMember]
        public DateTime RegistrationStartDate
        { get; set; }

        /// <summary>
        /// Gets or sets the south coordinate of the rectangle.
        /// </summary>
        [DataMember]
        public Int32 SouthCoordinate
        { get; set; }

        /// <summary>
        /// List of taxa to use for the search.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Gets or sets how to use selected start- and end-date.
        /// </summary>
        [DataMember]
        public WebUseOfDate UseOfObservationDate
        { get; set; }

        /// <summary>
        /// Gets or sets how to use selected start- and end registrationdate.
        /// </summary>
        [DataMember]
        public WebUseOfDate UseOfRegistrationDate
        { get; set; }

        /// <summary>
        /// User role id.
        /// </summary>
        [DataMember]
        public Int32 UserRoleId
        { get; set; }

        /// <summary>
        /// Gets or sets the west coordinate of the rectangle.
        /// </summary>
        [DataMember]
        public Int32 WestCoordinate
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            LocationSearchString = LocationSearchString.CheckSqlInjection();
            if (Counties.IsNotEmpty())
            {
                foreach (WebCounty county in Counties)
                {
                    county.CheckData();
                }
            }
            if (Provinces.IsNotEmpty())
            {
                foreach (WebProvince province in Provinces)
                {
                    province.CheckData();
                }
            }
        }
    }
}
