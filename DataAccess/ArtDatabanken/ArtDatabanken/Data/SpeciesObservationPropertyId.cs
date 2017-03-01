using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of predefined properties that are associated
    /// with abstract species observations classes.
    /// </summary>
    [DataContract]
    public enum SpeciesObservationPropertyId
    {
        /// <summary>
        /// This is not real property.
        /// This value is used when a variable of type
        /// SpeciesObservationPropertyId does not contain any value.
        /// </summary>
        [EnumMember]
        None,

        /// <summary>
        /// Darwin Core term name: acceptedNameUsage.
        /// The full name, with authorship and date information
        /// if known, of the currently valid (zoological) or
        /// accepted (botanical) taxon.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AcceptedNameUsage,

        /// <summary>
        /// Darwin Core term name: acceptedNameUsageID.
        /// An identifier for the name usage (documented meaning of
        /// the name according to a source) of the currently valid
        /// (zoological) or accepted (botanical) taxon.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AcceptedNameUsageID,

        /// <summary>
        /// Darwin Core term name: dcterms:accessRights.
        /// Information about who can access the resource or
        /// an indication of its security status.
        /// Access Rights may include information regarding
        /// access or restrictions based on privacy, security,
        /// or other policies.
        /// Currently this is a value between 1 to 6.
        /// 1 indicates public access and 6 is the highest security level.
        /// Should in the future be the value 'Public' or 'Protected'.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AccessRights,

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species is the subject
        /// of an action plan ('åtgärdsprogram' in swedish).
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        ActionPlan,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ActivityId,

        /// <summary>
        /// Darwin Core term name: associatedMedia.
        /// A list (concatenated and separated) of identifiers
        /// (publication, global unique identifier, URI) of
        /// media associated with the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AssociatedMedia,

        /// <summary>
        /// Darwin Core term name: associatedOccurrences.
        /// A list (concatenated and separated) of identifiers of
        /// other Occurrence records and their associations to
        /// this Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AssociatedOccurrences,

        /// <summary>
        /// Darwin Core term name: associatedReferences.
        /// A list (concatenated and separated) of identifiers
        /// (publication, bibliographic reference, global unique
        /// identifier, URI) of literature associated with
        /// the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AssociatedReferences,

        /// <summary>
        /// Darwin Core term name: associatedSequences.
        /// A list (concatenated and separated) of identifiers of
        /// other Occurrence records and their associations to
        /// this Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AssociatedSequences,

        /// <summary>
        /// Darwin Core term name: associatedTaxa.
        /// A list (concatenated and separated) of identifiers or
        /// names of taxa and their associations with the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        AssociatedTaxa,

        /// <summary>
        /// Darwin Core term name: basisOfRecord.
        /// The specific nature of the data record -
        /// a subtype of the dcterms:type.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Darwin Core Type Vocabulary
        /// (http://rs.tdwg.org/dwc/terms/type-vocabulary/index.htm).
        /// In Species Gateway this property has the value
        /// HumanObservation.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property according
        /// to the Darwin Core Type Vocabulary.
        /// </summary>
        [EnumMember]
        BasisOfRecord,

        /// <summary>
        /// Darwin Core term name: bed.
        /// The full name of the lithostratigraphic bed from which
        /// the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Bed,

        /// <summary>
        /// Darwin Core term name: behavior.
        /// A description of the behavior shown by the subject at
        /// the time the Occurrence was recorded.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Behavior,

        /// <summary>
        /// Darwin Core term name: dcterms:bibliographicCitation.
        /// A bibliographic reference for the resource as a statement
        /// indicating how this record should be cited (attributed)
        /// when used.
        /// Recommended practice is to include sufficient
        /// bibliographic detail to identify the resource as
        /// unambiguously as possible.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        BibliographicCitation,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        BirdNestActivityId,

        /// <summary>
        /// Darwin Core term name: catalogNumber.
        /// An identifier (preferably unique) for the record
        /// within the data set or collection.
        /// Currently this id does not work as supposed. For example: 
        /// one specific observation may have another id tomorrow.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CatalogNumber,

        /// <summary>
        /// Darwin Core term name: class.
        /// The full scientific name of the class in which
        /// the taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Class,

        /// <summary>
        /// Darwin Core term name: collectionCode.
        /// The name, acronym, coden, or initialism identifying the 
        /// collection or data set from which the record was derived.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CollectionCode,

        /// <summary>
        /// Darwin Core term name: collectionID.
        /// An identifier for the collection or dataset from which
        /// the record was derived.
        /// For physical specimens, the recommended best practice is
        /// to use the identifier in a collections registry such as
        /// the Biodiversity Collections Index
        /// (http://www.biodiversitycollectionsindex.org/).
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CollectionID,

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether a species has been
        /// classified as nature conservation relevant
        /// ('naturvårdsintressant' in swedish).
        /// The concept 'nature conservation relevant' must be defined
        /// before this property can be used.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        ConservationRelevant,

        /// <summary>
        /// Darwin Core term name: continent.
        /// The name of the continent in which the Location occurs.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Getty Thesaurus of Geographi
        /// Names or the ISO 3166 Continent code.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Continent,

        /// <summary>
        /// Not defined in Darwin Core.
        /// M value that is part of a linear reference system.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateM,

        /// <summary>
        /// Darwin Core term name: CoordinatePrecision.
        /// A decimal representation of the precision of the coordinates
        /// given in the DecimalLatitude and DecimalLongitude.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinatePrecision,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Either a complete coordinate system wkt (Well-known text)
        /// as defined by OGC (Open Geospatial Consortium) or a
        /// string representation of an enum value of type
        /// ArtDatabanken.Data.CoordinateSystemId.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property as defined by
        /// OGC or a string representation of an enum value of type
        /// ArtDatabanken.Data.CoordinateSystemId.
        /// </summary>
        [EnumMember]
        CoordinateSystemWkt,

        /// <summary>
        /// Darwin Core term name: coordinateUncertaintyInMeters.
        /// The horizontal distance (in meters) from the given
        /// CoordinateX and CoordinateY describing the
        /// smallest circle containing the whole of the Location.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateUncertaintyInMeters,

        /// <summary>
        /// Not defined in Darwin Core.
        /// East-west value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateX,

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateY,

        /// <summary>
        /// Not defined in Darwin Core.
        /// East-west value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateX_RT90,

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateY_RT90,

        /// <summary>
        /// Not defined in Darwin Core.
        /// East-west value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateX_SWEREF99,

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateY_SWEREF99,

        /// <summary>
        /// Not defined in Darwin Core.
        /// East-west value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateX_ETRS89_LAEA,

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateY_ETRS89_LAEA,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Altitude value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CoordinateZ,

        /// <summary>
        /// Darwin Core term name: country.
        /// The name of the country or major administrative unit
        /// in which the Location occurs.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Getty Thesaurus of Geographic Names.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Country,

        /// <summary>
        /// Darwin Core term name: countryCode.
        /// The standard code for the country in which the
        /// Location occurs.
        /// Recommended best practice is to use ISO 3166-1-alpha-2
        /// country codes.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        CountryCode,

        /// <summary>
        /// Darwin Core term name: county.
        /// The full, unabbreviated name of the next smaller
        /// administrative region than stateProvince(county, shire,
        /// department, etc.) in which the Location occurs
        /// ('län' in swedish).
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        County,

        /// <summary>
        /// Darwin Core term name: dataGeneralizations.
        /// Actions taken to make the shared data less specific or
        /// complete than in its original form.
        /// Suggests that alternative data of higher quality
        /// may be available on request.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        DataGeneralizations,

        /// <summary>
        /// Darwin Core term name: datasetID.
        /// An identifier for the set of data.
        /// May be a global unique identifier or an identifier
        /// specific to a collection or institution.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        DatasetID,

        /// <summary>
        /// Darwin Core term name: datasetName.
        /// The name identifying the data set
        /// from which the record was derived.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        DatasetName,

        /// <summary>
        /// Darwin Core term name: dateIdentified.
        /// The date on which the subject was identified as
        /// representing the Taxon. Recommended best practice is
        /// to use an encoding scheme, such as ISO 8601:2004(E).
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        DateIdentified,

        /// <summary>
        /// Darwin Core term name: day.
        /// The integer day of the month on which the Event occurred
        /// (start date of observation).
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from property
        /// Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        Day,

        /// <summary>
        /// Darwin Core term name: decimalLatitude.
        /// Definition in Darwin Core:
        /// The geographic latitude (in decimal degrees, using
        /// the spatial reference system given in geodeticDatum)
        /// of the geographic center of a Location. Positive values
        /// are north of the Equator, negative values are south of it.
        /// Legal values lie between -90 and 90, inclusive.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        DecimalLatitude,

        /// <summary>
        /// Darwin Core term name: decimalLongitude.
        /// Definition in Darwin Core:
        /// The geographic longitude (in decimal degrees, using
        /// the spatial reference system given in geodeticDatum)
        /// of the geographic center of a Location. Positive
        /// values are east of the Greenwich Meridian, negative
        /// values are west of it. Legal values lie between -180
        /// and 180, inclusive.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        DecimalLongitude,

        /// <summary>
        /// Darwin Core term name: disposition.
        /// The current state of a specimen with respect to the
        /// collection identified in collectionCode or collectionID.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Disposition,

        /// <summary>
        /// Darwin Core term name: dynamicProperties.
        /// A list (concatenated and separated) of additional
        /// measurements, facts, characteristics, or assertions
        /// about the record. Meant to provide a mechanism for
        /// structured content such as key-value pairs.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not use this property. Use flexibly defined species
        /// observation classes and properties instead.
        /// </summary>
        [EnumMember]
        DynamicProperties,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Id (not GUID) value in Dyntaxa.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        DyntaxaTaxonID,

        /// <summary>
        /// Darwin Core term name: earliestAgeOrLowestStage.
        /// The full name of the earliest possible geochronologic
        /// age or lowest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EarliestAgeOrLowestStage,

        /// <summary>
        /// Darwin Core term name: earliestEonOrLowestEonothem.
        /// The full name of the earliest possible geochronologic eon
        /// or lowest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EarliestEonOrLowestEonothem,

        /// <summary>
        /// Darwin Core term name: earliestEpochOrLowestSeries.
        /// The full name of the earliest possible geochronologic
        /// epoch or lowest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EarliestEpochOrLowestSeries,

        /// <summary>
        /// Darwin Core term name: earliestEraOrLowestErathem.
        /// The full name of the earliest possible geochronologic
        /// era or lowest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EarliestEraOrLowestErathem,

        /// <summary>
        /// Darwin Core term name: earliestPeriodOrLowestSystem.
        /// The full name of the earliest possible geochronologic
        /// period or lowest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EarliestPeriodOrLowestSystem,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about date and time when the
        /// species observation ended.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        End,

        /// <summary>
        /// Darwin Core term name: endDayOfYear.
        /// The latest ordinal day of the year on which the Event
        /// occurred (1 for January 1, 365 for December 31,
        /// except in a leap year, in which case it is 366).
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from property
        /// End if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        EndDayOfYear,

        /// <summary>
        /// Darwin Core term name: establishmentMeans.
        /// The process by which the biological individual(s)
        /// represented in the Occurrence became established at the
        /// location.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EstablishmentMeans,

        /// <summary>
        /// Darwin Core term name: eventDate.
        /// The date-time or interval during which an Event occurred.
        /// For occurrences, this is the date-time when the event
        /// was recorded. Not suitable for a time in a geological
        /// context. Recommended best practice is to use an encoding
        /// scheme, such as ISO 8601:2004(E).
        /// For example: ”2007-03-01 13:00:00 - 2008-05-11 15:30:00”
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from properties
        /// End and Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        EventDate,

        /// <summary>
        /// Darwin Core term name: eventID.
        /// A list (concatenated and separated) of identifiers
        /// (publication, global unique identifier, URI) of
        /// media associated with the Occurrence.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EventID,

        /// <summary>
        /// Darwin Core term name: eventRemarks.
        /// Comments or notes about the Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        EventRemarks,

        /// <summary>
        /// Darwin Core term name: eventTime.
        /// The time or interval during which an Event occurred.
        /// Recommended best practice is to use an encoding scheme,
        /// such as ISO 8601:2004(E).
        /// For example: ”13:00:00 - 15:30:00”
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from properties
        /// End and Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        EventTime,

        /// <summary>
        /// Darwin Core term name: family.
        /// The full scientific name of the family in which
        /// the taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Family,

        /// <summary>
        /// Darwin Core term name: fieldNotes.
        /// One of a) an indicator of the existence of, b) a
        /// reference to (publication, URI), or c) the text of
        /// notes taken in the field about the Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        FieldNotes,

        /// <summary>
        /// Darwin Core term name: fieldNumber.
        /// An identifier given to the event in the field. Often 
        /// serves as a link between field notes and the Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        FieldNumber,

        /// <summary>
        /// Darwin Core term name: formation.
        /// The full name of the lithostratigraphic formation from
        /// which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Formation,

        /// <summary>
        /// Darwin Core term name: footprintSpatialFit.
        /// The ratio of the area of the footprint (footprintWKT)
        /// to the area of the true (original, or most specific)
        /// spatial representation of the Location. Legal values are
        /// 0, greater than or equal to 1, or undefined. A value of
        /// 1 is an exact match or 100% overlap. A value of 0 should
        /// be used if the given footprint does not completely contain
        /// the original representation. The footprintSpatialFit is
        /// undefined (and should be left blank) if the original
        /// representation is a point and the given georeference is
        /// not that same point. If both the original and the given
        /// georeference are the same point, the footprintSpatialFit
        /// is 1.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        FootprintSpatialFit,

        /// <summary>
        /// Darwin Core term name: footprintSRS.
        /// A Well-Known Text (WKT) representation of the Spatial
        /// Reference System (SRS) for the footprintWKT of the
        /// Location. Do not use this term to describe the SRS of
        /// the decimalLatitude and decimalLongitude, even if it is
        /// the same as for the footprintWKT - use the geodeticDatum
        /// instead.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        FootprintSRS,

        /// <summary>
        /// Darwin Core term name: footprintWKT.
        /// A Well-Known Text (WKT) representation of the shape
        /// (footprint, geometry) that defines the Location.
        /// A Location may have both a point-radius representation
        /// (see decimalLatitude) and a footprint representation,
        /// and they may differ from each other.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        FootprintWKT,

        /// <summary>
        /// Darwin Core term name: genus.
        /// The full scientific name of the genus in which
        /// the taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Genus,

        /// <summary>
        /// Darwin Core term name: geodeticDatum.
        /// The ellipsoid, geodetic datum, or spatial reference
        /// system (SRS) upon which the geographic coordinates
        /// given in decimalLatitude and decimalLongitude as based.
        /// Recommended best practice is use the EPSG code as a
        /// controlled vocabulary to provide an SRS, if known.
        /// Otherwise use a controlled vocabulary for the name or
        /// code of the geodetic datum, if known. Otherwise use a
        /// controlled vocabulary for the name or code of the
        /// ellipsoid, if known. If none of these is known, use the
        /// value "unknown".
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeodeticDatum,

        /// <summary>
        /// Darwin Core term name: geologicalContextID.
        /// An identifier for the set of information associated
        /// with a GeologicalContext (the location within a geological
        /// context, such as stratigraphy). May be a global unique
        /// identifier or an identifier specific to the data set.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeologicalContextID,

        /// <summary>
        /// Darwin Core term name: georeferencedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations who determined the georeference
        /// (spatial representation) the Location.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferencedBy,

        /// <summary>
        /// Darwin Core term name: georeferencedDate.
        /// The date on which the Location was georeferenced.
        /// Recommended best practice is to use an encoding scheme,
        /// such as ISO 8601:2004(E).
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferencedDate,

        /// <summary>
        /// Darwin Core term name: georeferenceProtocol.
        /// A description or reference to the methods used to
        /// determine the spatial footprint, coordinates, and
        /// uncertainties.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferenceProtocol,

        /// <summary>
        /// Darwin Core term name: georeferenceRemarks.
        /// Notes or comments about the spatial description
        /// determination, explaining assumptions made in addition
        /// or opposition to the those formalized in the method
        /// referred to in georeferenceProtocol.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferenceRemarks,

        /// <summary>
        /// Darwin Core term name: georeferenceSources.
        /// A list (concatenated and separated) of maps, gazetteers,
        /// or other resources used to georeference the Location,
        /// described specifically enough to allow anyone in the
        /// future to use the same resources.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferenceSources,

        /// <summary>
        /// Darwin Core term name: georeferenceVerificationStatus.
        /// A categorical description of the extent to which the
        /// georeference has been verified to represent the best
        /// possible spatial description. Recommended best practice
        /// is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        GeoreferenceVerificationStatus,

        /// <summary>
        /// Darwin Core term name: group.
        /// The full name of the lithostratigraphic group from
        /// which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Group,

        /// <summary>
        /// Darwin Core term name: habitat.
        /// A category or description of the habitat
        /// in which the Event occurred.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Habitat,

        /// <summary>
        /// Darwin Core term name: higherClassification.
        /// A list (concatenated and separated) of taxa names
        /// terminating at the rank immediately superior to the
        /// taxon referenced in the taxon record.
        /// Recommended best practice is to order the list
        /// starting with the highest rank and separating the names
        /// for each rank with a semi-colon (";").
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current values 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        HigherClassification,

        /// <summary>
        /// Darwin Core term name: higherGeography.
        /// A list (concatenated and separated) of geographic
        /// names less specific than the information captured
        /// in the locality term.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        HigherGeography,

        /// <summary>
        /// Darwin Core term name: higherGeographyID.
        /// An identifier for the geographic region within which
        /// the Location occurred.
        /// Recommended best practice is to use an
        /// persistent identifier from a controlled vocabulary
        /// such as the Getty Thesaurus of Geographic Names.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        HigherGeographyID,

        /// <summary>
        /// Darwin Core term name: highestBiostratigraphicZone.
        /// The full name of the highest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        HighestBiostratigraphicZone,

        /// <summary>
        /// Not defined in Darwin Core.
        /// SwedishSpeciesObservationSOAPService specific id
        /// for this species observation.
        /// The id is only used in communication with
        /// SwedishSpeciesObservationSOAPService and has no 
        /// meaning in other contexts.
        /// This id is currently not stable.
        /// The same observation may have another id tomorrow.
        /// In the future this id should be stable.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: 64 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// Darwin Core term name: identificationID.
        /// An identifier for the Identification (the body of
        /// information associated with the assignment of a scientific
        /// name). May be a global unique identifier or an identifier
        /// specific to the data set.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentificationID,

        /// <summary>
        /// Darwin Core term name: identificationQualifier.
        /// A brief phrase or a standard term ("cf.", "aff.") to
        /// express the determiner's doubts about the Identification.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentificationQualifier,

        /// <summary>
        /// Darwin Core term name: identificationReferences.
        /// A list (concatenated and separated) of references
        /// (publication, global unique identifier, URI) used in
        /// the Identification.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentificationReferences,

        /// <summary>
        /// Darwin Core term name: identificationRemarks.
        /// Comments or notes about the Identification.
        /// Contains for example information about that
        /// the observer is uncertain about which species
        /// that has been observed.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentificationRemarks,

        /// <summary>
        /// Darwin Core term name: identificationVerificationStatus.
        /// A categorical indicator of the extent to which the taxonomic
        /// identification has been verified to be correct.
        /// Recommended best practice is to use a controlled vocabulary
        /// such as that used in HISPID/ABCD.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentificationVerificationStatus,

        /// <summary>
        /// Darwin Core term name: identifiedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations who assigned the Taxon to the
        /// subject.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IdentifiedBy,

        /// <summary>
        /// Darwin Core term name: individualCount.
        /// The number of individuals represented present
        /// at the time of the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: 64 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IndividualCount,

        /// <summary>
        /// Darwin Core term name: individualID.
        /// An identifier for an individual or named group of
        /// individual organisms represented in the Occurrence.
        /// Meant to accommodate resampling of the same individual
        /// or group for monitoring purposes. May be a global unique
        /// identifier or an identifier specific to a data set.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IndividualID,

        /// <summary>
        /// Darwin Core term name: informationWithheld.
        /// Additional information that exists, but that has
        /// not been shared in the given record.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        InformationWithheld,

        /// <summary>
        /// Darwin Core term name: infraspecificEpithet.
        /// The name of the lowest or terminal infraspecific epithet
        /// of the scientificName, excluding any rank designation.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        InfraspecificEpithet,

        /// <summary>
        /// Darwin Core term name: institutionCode.
        /// The name (or acronym) in use by the institution
        /// having custody of the object(s) or information
        /// referred to in the record.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        InstitutionCode,

        /// <summary>
        /// Darwin Core term name: institutionID.
        /// An identifier for the institution having custody of 
        /// the object(s) or information referred to in the record.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        InstitutionID,

        /// <summary>
        /// Darwin Core term name: island.
        /// The name of the island on or near which the Location occurs.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Getty Thesaurus of Geographic Names.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Island,

        /// <summary>
        /// Darwin Core term name: islandGroup.
        /// The name of the island group in which the Location occurs.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Getty Thesaurus of Geographic Names.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IslandGroup,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this species occurrence is natural or
        /// if it is a result of human activity.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IsNaturalOccurrence,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a never found observation.
        /// "Never found observation" is an observation that says
        /// that the specified species was not found in a location
        /// deemed appropriate for the species.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        IsNeverFoundObservation,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a 
        /// not rediscovered observation.
        /// "Not rediscovered observation" is an observation that says
        /// that the specified species was not found in a location
        /// where it has previously been observed.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        IsNotRediscoveredObservation,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if this observation is a positive observation.
        /// "Positive observation" is a normal observation indicating
        /// that a species has been seen at a specified location.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        IsPositiveObservation,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if species observations that are reported in
        /// a project are publicly available or not.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        IsPublic,

        /// <summary>
        /// Darwin Core term name: kingdom.
        /// The full scientific name of the kingdom in which the
        /// taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Kingdom,

        /// <summary>
        /// Darwin Core term name: dcterms:language.
        /// A language of the resource.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as RFC 4646 [RFC4646].
        /// 
        /// In flexible species observation format language and culture
        /// are handled by using property Locale in class
        /// WebSpeciesObservationField.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Language,

        /// <summary>
        /// Darwin Core term name: latestAgeOrHighestStage.
        /// The full name of the latest possible geochronologic
        /// age or highest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LatestAgeOrHighestStage,

        /// <summary>
        /// Darwin Core term name: latestEonOrHighestEonothem.
        /// The full name of the latest possible geochronologic eon
        /// or highest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LatestEonOrHighestEonothem,

        /// <summary>
        /// Darwin Core term name: latestEpochOrHighestSeries.
        /// The full name of the latest possible geochronologic
        /// epoch or highest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LatestEpochOrHighestSeries,

        /// <summary>
        /// Darwin Core term name: latestEraOrHighestErathem.
        /// The full name of the latest possible geochronologic
        /// era or highest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LatestEraOrHighestErathem,

        /// <summary>
        /// Darwin Core term name: latestPeriodOrHighestSystem.
        /// The full name of the latest possible geochronologic
        /// period or highest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LatestPeriodOrHighestSystem,

        /// <summary>
        /// Darwin Core term name: lifeStage.
        /// The age class or life stage of the biological individual(s)
        /// at the time the Occurrence was recorded.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LifeStage,

        /// <summary>
        /// Darwin Core term name: lithostratigraphicTerms.
        /// The combination of all litho-stratigraphic names for
        /// the rock from which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LithostratigraphicTerms,

        /// <summary>
        /// Darwin Core term name: locality.
        /// The specific description of the place. Less specific
        /// geographic information can be provided in other
        /// geographic terms (higherGeography, continent, country,
        /// stateProvince, county, municipality, waterBody, island,
        /// islandGroup). This term may contain information modified
        /// from the original to correct perceived errors or
        /// standardize the description.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        Locality,

        /// <summary>
        /// Darwin Core term name: locationAccordingTo.
        /// Information about the source of this Location information.
        /// Could be a publication (gazetteer), institution,
        /// or team of individuals.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LocationAccordingTo,

        /// <summary>
        /// Darwin Core term name: locationID.
        /// An identifier for the set of location information
        /// (data associated with dcterms:Location).
        /// May be a global unique identifier or an identifier
        /// specific to the data set.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        LocationId,

        /// <summary>
        /// Darwin Core term name: locationRemarks.
        /// Comments or notes about the Location.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LocationRemarks,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// location. The information should be accessible
        /// from the most commonly used web browsers.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LocationURL,

        /// <summary>
        /// Darwin Core term name: lowestBiostratigraphicZone.
        /// The full name of the lowest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        LowestBiostratigraphicZone,

        /// <summary>
        /// Darwin Core term name: maximumDepthInMeters.
        /// The greater depth of a range of depth below
        /// the local surface, in meters.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MaximumDepthInMeters,

        /// <summary>
        /// Darwin Core term name: maximumDistanceAboveSurfaceInMeters.
        /// The greater distance in a range of distance from a
        /// reference surface in the vertical direction, in meters.
        /// Use positive values for locations above the surface,
        /// negative values for locations below. If depth measures
        /// are given, the reference surface is the location given
        /// by the depth, otherwise the reference surface is the
        /// location given by the elevation.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MaximumDistanceAboveSurfaceInMeters,

        /// <summary>
        /// Darwin Core term name: maximumElevationInMeters.
        /// The upper limit of the range of elevation (altitude,
        /// usually above sea level), in meters.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MaximumElevationInMeters,

        /// <summary>
        /// Darwin Core term name: measurementAccuracy.
        /// The description of the potential error associated
        /// with the measurementValue.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementAccuracy,

        /// <summary>
        /// Darwin Core term name: measurementDeterminedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations who determined the value of the
        /// MeasurementOrFact.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementDeterminedBy,

        /// <summary>
        /// Darwin Core term name: measurementDeterminedDate.
        /// The date on which the MeasurementOrFact was made.
        /// Recommended best practice is to use an encoding scheme,
        /// such as ISO 8601:2004(E).
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementDeterminedDate,

        /// <summary>
        /// Darwin Core term name: measurementID.
        /// An identifier for the MeasurementOrFact (information
        /// pertaining to measurements, facts, characteristics,
        /// or assertions). May be a global unique identifier or an
        /// identifier specific to the data set.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementID,

        /// <summary>
        /// Darwin Core term name: measurementMethod.
        /// A description of or reference to (publication, URI)
        /// the method or protocol used to determine the measurement,
        /// fact, characteristic, or assertion.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementMethod,

        /// <summary>
        /// Darwin Core term name: measurementRemarks.
        /// Comments or notes accompanying the MeasurementOrFact.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementRemarks,

        /// <summary>
        /// Darwin Core term name: measurementType.
        /// The nature of the measurement, fact, characteristic,
        /// or assertion.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementType,

        /// <summary>
        /// Darwin Core term name: measurementUnit.
        /// The units associated with the measurementValue.
        /// Recommended best practice is to use the
        /// International System of Units (SI).
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementUnit,

        /// <summary>
        /// Darwin Core term name: measurementValue.
        /// The value of the measurement, fact, characteristic,
        /// or assertion.
        ///
        /// Used in abstract species observation class: MeasurementOrFact.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        MeasurementValue,

        /// <summary>
        /// Darwin Core term name: member.
        /// The full name of the lithostratigraphic member from
        /// which the cataloged item was collected.
        ///
        /// Used in abstract species observation class: GeologicalContext.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Member,

        /// <summary>
        /// Darwin Core term name: minimumDepthInMeters.
        /// The lesser depth of a range of depth below the
        /// local surface, in meters.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MinimumDepthInMeters,

        /// <summary>
        /// Darwin Core term name: minimumDistanceAboveSurfaceInMeters.
        /// The lesser distance in a range of distance from a
        /// reference surface in the vertical direction, in meters.
        /// Use positive values for locations above the surface,
        /// negative values for locations below.
        /// If depth measures are given, the reference surface is
        /// the location given by the depth, otherwise the reference
        /// surface is the location given by the elevation.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MinimumDistanceAboveSurfaceInMeters,

        /// <summary>
        /// Darwin Core term name: minimumElevationInMeters.
        /// The lower limit of the range of elevation (altitude,
        /// usually above sea level), in meters.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        MinimumElevationInMeters,

        /// <summary>
        /// Darwin Core term name: dcterms:modified.
        /// The most recent date-time on which the resource was changed.
        /// For Darwin Core, recommended best practice is to use an
        /// encoding scheme, such as ISO 8601:2004(E).
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        Modified,

        /// <summary>
        /// Darwin Core term name: month.
        /// The ordinal month in which the Event occurred.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from property
        /// Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        Month,

        /// <summary>
        /// Darwin Core term name: municipality.
        /// The full, unabbreviated name of the next smaller
        /// administrative region than county (city, municipality, etc.)
        /// in which the Location occurs.
        /// Do not use this term for a nearby named place
        /// that does not contain the actual location.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Municipality,

        /// <summary>
        /// Darwin Core term name: nameAccordingTo.
        /// The reference to the source in which the specific
        /// taxon concept circumscription is defined or implied -
        /// traditionally signified by the Latin "sensu" or "sec."
        /// (from secundum, meaning "according to").
        /// For taxa that result from identifications, a reference
        /// to the keys, monographs, experts and other sources should
        /// be given.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NameAccordingTo,

        /// <summary>
        /// Darwin Core term name: nameAccordingToID.
        /// An identifier for the source in which the specific
        /// taxon concept circumscription is defined or implied.
        /// See nameAccordingTo.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NameAccordingToID,

        /// <summary>
        /// Darwin Core term name: namePublishedIn.
        /// A reference for the publication in which the
        /// scientificName was originally established under the rules
        /// of the associated nomenclaturalCode.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NamePublishedIn,

        /// <summary>
        /// Darwin Core term name: namePublishedInID.
        /// An identifier for the publication in which the
        /// scientificName was originally established under the
        /// rules of the associated nomenclaturalCode.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NamePublishedInID,

        /// <summary>
        /// Darwin Core term name: namePublishedInYear.
        /// The four-digit year in which the scientificName
        /// was published.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NamePublishedInYear,

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether
        /// the species is included in Natura 2000.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        Natura2000,

        /// <summary>
        /// Darwin Core term name: nomenclaturalCode.
        /// The nomenclatural code (or codes in the case of an
        /// ambiregnal name) under which the scientificName is
        /// constructed.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NomenclaturalCode,

        /// <summary>
        /// Darwin Core term name: nomenclaturalStatus.
        /// The status related to the original publication of the
        /// name and its conformance to the relevant rules of
        /// nomenclature. It is based essentially on an algorithm
        /// according to the business rules of the code.
        /// It requires no taxonomic opinion.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        NomenclaturalStatus,

        /// <summary>
        /// Darwin Core term name: occurrenceID.
        /// An identifier for the Occurrence (as opposed to a
        /// particular digital record of the occurrence).
        /// In the absence of a persistent global unique identifier,
        /// construct one from a combination of identifiers in
        /// the record that will most closely make the occurrenceID
        /// globally unique.
        /// The format LSID (Life Science Identifiers) is used as GUID
        /// (Globally unique identifier) for species observations.
        /// Currently known GUIDs:
        /// Species Gateway (Artportalen) 1,
        /// urn:lsid:artportalen.se:Sighting:{reporting system}.{id}
        /// where {reporting system} is one of Bird, Bugs, Fish, 
        /// MarineInvertebrates, PlantAndMushroom or Vertebrate.
        /// Species Gateway (Artportalen) 2,
        /// urn:lsid:artportalen.se:Sighting:{id}
        /// Red list database: urn:lsid:artdata.slu.se:SpeciesObservation:{id}
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        OccurrenceID,

        /// <summary>
        /// Darwin Core term name: occurrenceRemarks.
        /// Comments or notes about the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        OccurrenceRemarks,

        /// <summary>
        /// Darwin Core term name: occurrenceStatus.
        /// A statement about the presence or absence of a Taxon at a
        /// Location.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        OccurrenceStatus,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// occurrence. The information should be accessible
        /// from the most commonly used web browsers.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        OccurrenceURL,

        /// <summary>
        /// Darwin Core term name: order.
        /// The full scientific name of the order in which
        /// the taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Order,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Common name of the organism group that observed species
        /// belongs to. Classification of species groups is the same as
        /// used in latest 'Red List of Swedish Species'.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        OrganismGroup,

        /// <summary>
        /// Darwin Core term name: originalNameUsage.
        /// The taxon name, with authorship and date information
        /// if known, as it originally appeared when first established
        /// under the rules of the associated nomenclaturalCode.
        /// The basionym (botany) or basonym (bacteriology) of the
        /// scientificName or the senior/earlier homonym for replaced
        /// names.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        OriginalNameUsage,

        /// <summary>
        /// Darwin Core term name: originalNameUsageID.
        /// An identifier for the name usage (documented meaning of
        /// the name according to a source) in which the terminal
        /// element of the scientificName was originally established
        /// under the rules of the associated nomenclaturalCode.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        OriginalNameUsageID,

        /// <summary>
        /// Darwin Core term name: otherCatalogNumbers.
        /// A list (concatenated and separated) of previous or
        /// alternate fully qualified catalog numbers or other
        /// human-used identifiers for the same Occurrence,
        /// whether in the current or any other data set or collection.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        OtherCatalogNumbers,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the organization or person that
        /// owns the species observation.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        Owner,

        /// <summary>
        /// Darwin Core term name: ownerInstitutionCode.
        /// The name (or acronym) in use by the institution having
        /// ownership of the object(s) or information referred
        /// to in the record.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        OwnerInstitutionCode,

        /// <summary>
        /// Darwin Core term name: parentNameUsage.
        /// The full name, with authorship and date information
        /// if known, of the direct, most proximate higher-rank
        /// parent taxon (in a classification) of the most specific
        /// element of the scientificName.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        ParentNameUsage,

        /// <summary>
        /// Darwin Core term name: parentNameUsageID.
        /// An identifier for the name usage (documented meaning
        /// of the name according to a source) of the direct,
        /// most proximate higher-rank parent taxon
        /// (in a classification) of the most specific
        /// element of the scientificName.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        ParentNameUsageID,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Parish where the species observation where made.
        /// 'Socken/församling' in swedish.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Parish,

        /// <summary>
        /// Darwin Core term name: phylum.
        /// The full scientific name of the phylum or division
        /// in which the taxon is classified.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Phylum,

        /// <summary>
        /// Darwin Core term name: pointRadiusSpatialFit.
        /// The ratio of the area of the point-radius
        /// (decimalLatitude, decimalLongitude,
        /// coordinateUncertaintyInMeters) to the area of the true
        /// (original, or most specific) spatial representation of
        /// the Location. Legal values are 0, greater than or equal
        /// to 1, or undefined. A value of 1 is an exact match or
        /// 100% overlap. A value of 0 should be used if the given
        /// point-radius does not completely contain the original
        /// representation. The pointRadiusSpatialFit is undefined
        /// (and should be left blank) if the original representation
        /// is a point without uncertainty and the given georeference
        /// is not that same point (without uncertainty). If both the
        /// original and the given georeference are the same point,
        /// the pointRadiusSpatialFit is 1.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        PointRadiusSpatialFit,

        /// <summary>
        /// Darwin Core term name: preparations.
        /// A list (concatenated and separated) of preparations
        /// and preservation methods for a specimen.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Preparations,

        /// <summary>
        /// Darwin Core term name: previousIdentifications.
        /// A list (concatenated and separated) of previous
        /// assignments of names to the Occurrence.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        PreviousIdentifications,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the type of project,
        /// for example 'Environmental monitoring'.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectCategory,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Description of a project.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectDescription,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project ends.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectEndDate,

        /// <summary>
        /// Not defined in Darwin Core.
        /// An identifier for the project.
        /// In the absence of a persistent global unique identifier,
        /// construct one from a combination of identifiers in
        /// the project that will most closely make the ProjectID
        /// globally unique.
        /// The format LSID (Life Science Identifiers) is used as GUID
        /// (Globally unique identifier).
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectID,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the project.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectName,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of person or organization that owns the project.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectOwner,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project starts.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectStartDate,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// project. The information should be accessible
        /// from the most commonly used web browsers.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ProjectURL,

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species 
        /// is protected by the law in Sweden.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        ProtectedByLaw,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about how protected information
        /// about a species is in Sweden.
        /// Currently this is a value between 1 to 6.
        /// 1 indicates public access and 6 is the highest security level.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        ProtectionLevel,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Quantity of observed species, for example distribution area.
        /// Unit is specified in property QuantitiyUnit.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: 64 bits float.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Quantity,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Unit for quantity value of observed species.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        QuantityUnit,

        /// <summary>
        /// Darwin Core term name: recordedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations responsible for recording the
        /// original Occurrence. The primary collector or observer,
        /// especially one who applies a personal identifier
        /// (recordNumber), should be listed first.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        RecordedBy,

        /// <summary>
        /// Darwin Core term name: recordNumber.
        /// An identifier given to the Occurrence at the time it was
        /// recorded. Often serves as a link between field notes and
        /// an Occurrence record, such as a specimen collector's number.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        RecordNumber,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Redlist category for redlisted species. The property also
        /// contains information about which redlist that is referenced.
        /// Example value: CR (Sweden, 2010). Possible redlist values
        /// are DD (Data Deficient), EX (Extinct),
        /// RE (Regionally Extinct), CR (Critically Endangered),
        /// EN (Endangered), VU (Vulnerable), NT (Near Threatened).
        /// Not redlisted species has no value in this property.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        RedlistCategory,

        /// <summary>
        /// Darwin Core term name: dcterms:references.
        /// A related resource that is referenced, cited,
        /// or otherwise pointed to by the described resource.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        References,

        /// <summary>
        /// Darwin Core term name: relatedResourceID.
        /// An identifier for a related resource (the object,
        /// rather than the subject of the relationship).
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        RelatedResourceID,

        /// <summary>
        /// Darwin Core term name: relationshipAccordingTo.
        /// The source (person, organization, publication, reference)
        /// establishing the relationship between the two resources.
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        RelationshipAccordingTo,

        /// <summary>
        /// Darwin Core term name: relationshipEstablishedDate.
        /// The date-time on which the relationship between the
        /// two resources was established. Recommended best practice
        /// is to use an encoding scheme, such as ISO 8601:2004(E).
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        RelationshipEstablishedDate,

        /// <summary>
        /// Darwin Core term name: RelationshipOfResource.
        /// The relationship of the resource identified by
        /// relatedResourceID to the subject
        /// (optionally identified by the resourceID).
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        RelationshipOfResource,

        /// <summary>
        /// Darwin Core term name: relationshipRemarks.
        /// Comments or notes about the relationship between
        /// the two resources.
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        RelationshipRemarks,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the person that reported the species observation.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ReportedBy,

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Not defined in Darwin Core.
        /// Date and time when the species observation was reported.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ReportedDate,
#endif

        /// <summary>
        /// Darwin Core term name: reproductiveCondition.
        /// The reproductive condition of the biological individual(s)
        /// represented in the Occurrence.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ReproductiveCondition,

        /// <summary>
        /// Darwin Core term name: resourceID.
        /// An identifier for the resource that is the subject
        /// of the relationship.
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        ResourceID,

        /// <summary>
        /// Darwin Core term name: resourceRelationshipID.
        /// An identifier for an instance of relationship between
        /// one resource (the subject) and another
        /// (relatedResource, the object).
        ///
        /// Used in abstract species observation class: ResourceRelationship.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property but it is better to
        /// define your own abstract species observation classes and
        /// properties that better indicates the type of data involved.
        /// </summary>
        [EnumMember]
        ResourceRelationshipID,

        /// <summary>
        /// Darwin Core term name: dcterms:rights.
        /// Information about rights held in and over the resource.
        /// Typically, rights information includes a statement
        /// about various property rights associated with the resource,
        /// including intellectual property rights.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Rights,

        /// <summary>
        /// Darwin Core term name: dcterms:rightsHolder.
        /// A person or organization owning or
        /// managing rights over the resource.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        RightsHolder,

        /// <summary>
        /// Darwin Core term name: samplingEffort.
        /// The amount of effort expended during an Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        SamplingEffort,

        /// <summary>
        /// Darwin Core term name: samplingProtocol.
        /// The name of, reference to, or description of the
        /// method or protocol used during an Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        SamplingProtocol,

        /// <summary>
        /// Darwin Core term name: scientificName.
        /// The full scientific name, with authorship and date
        /// information if known. When forming part of an
        /// Identification, this should be the name in lowest level
        /// taxonomic rank that can be determined.
        /// This term should not contain identification qualifications,
        /// which should instead be supplied in the
        /// IdentificationQualifier term.
        /// Currently scientific name without author is provided.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        ScientificName,

        /// <summary>
        /// Darwin Core term name: scientificNameAuthorship.
        /// The authorship information for the scientificName
        /// formatted according to the conventions of the applicable
        /// nomenclaturalCode.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        ScientificNameAuthorship,

        /// <summary>
        /// Darwin Core term name: scientificNameID.
        /// An identifier for the nomenclatural (not taxonomic)
        /// details of a scientific name.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        ScientificNameID,

        /// <summary>
        /// Darwin Core term name: sex.
        /// The sex of the biological individual(s) represented in
        /// the Occurrence.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Sex,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// species observation. The information should be accessible
        /// from the most commonly used web browsers.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        SpeciesObservationURL,

        /// <summary>
        /// Darwin Core term name: specificEpithet.
        /// The name of the first or species epithet of
        /// the scientificName.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        SpecificEpithet,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about date and time when the
        /// species observation started.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: DateTime.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should provide a value for this property.
        /// </summary>
        [EnumMember]
        Start,

        /// <summary>
        /// Darwin Core term name: startDayOfYear.
        /// The earliest ordinal day of the year on which the
        /// Event occurred (1 for January 1, 365 for December 31,
        /// except in a leap year, in which case it is 366).
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from property
        /// Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        StartDayOfYear,

        /// <summary>
        /// Darwin Core term name: stateProvince.
        /// The name of the next smaller administrative region than
        /// country (state, province, canton, department, region, etc.)
        /// in which the Location occurs ('landskap' in swedish).
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        StateProvince,

        /// <summary>
        /// Darwin Core term name: subgenus.
        /// The full scientific name of the subgenus in which
        /// the taxon is classified. Values should include the
        /// genus to avoid homonym confusion.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        Subgenus,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Substrate on which the species was observed.
        ///
        /// Used in abstract species observation class: Occurrence.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Substrate,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Survey method used in a project to
        /// retrieve species observations.
        ///
        /// Used in abstract species observation class: Project.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        SurveyMethod,

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property contains information about the species
        /// immigration history.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        SwedishImmigrationHistory,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the species occurrence in Sweden.
        /// For example information about if the species reproduce
        /// in sweden.
        ///
        /// Used in abstract species observation class: Conservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property.
        /// </summary>
        [EnumMember]
        SwedishOccurrence,

        /// <summary>
        /// Darwin Core term name: taxonConceptID.
        /// An identifier for the taxonomic concept to which the record
        /// refers - not for the nomenclatural details of a taxon.
        /// In SwedishSpeciesObservationSOAPService this property
        /// has the same value as property TaxonID. 
        /// GUID in Dyntaxa is used as value for this property.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonConceptID,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Status of the taxon concept.
        /// Examples of possible values are Valid, Invalid, Lumped
        /// and Splited,
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonConceptStatus,

        /// <summary>
        /// Darwin Core term name: taxonID.
        /// An identifier for the set of taxon information
        /// (data associated with the Taxon class). May be a global
        /// unique identifier or an identifier specific to the data set.
        /// In SwedishSpeciesObservationSOAPService this property
        /// has the same value as property TaxonConceptID. 
        /// GUID in Dyntaxa is used as value for this property.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonID,

        /// <summary>
        /// Darwin Core term name: taxonomicStatus.
        /// The status of the use of the scientificName as a label
        /// for a taxon. Requires taxonomic opinion to define the
        /// scope of a taxon. Rules of priority then are used to
        /// define the taxonomic status of the nomenclature contained
        /// in that scope, combined with the experts opinion.
        /// It must be linked to a specific taxonomic reference that
        /// defines the concept.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonomicStatus,

        /// <summary>
        /// Darwin Core term name: taxonRank.
        /// The taxonomic rank of the most specific name in the
        /// scientificName. Recommended best practice is to use
        /// a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonRank,

        /// <summary>
        /// Darwin Core term name: taxonRemarks.
        /// Comments or notes about the taxon or name.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonRemarks,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Sort order of taxon according to Dyntaxa.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonSortOrder,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// taxon. The information should be accessible
        /// from the most commonly used web browsers.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        TaxonURL,

        /// <summary>
        /// Darwin Core term name: dcterms:type.
        /// The nature or genre of the resource.
        /// For Darwin Core, recommended best practice is
        /// to use the name of the class that defines the
        /// root of the record.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        Type,

        /// <summary>
        /// Darwin Core term name: typeStatus.
        /// A list (concatenated and separated) of nomenclatural
        /// types (type status, typified scientific name, publication)
        /// applied to the subject.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        TypeStatus,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if the species observer himself is
        /// uncertain about the taxon determination.
        ///
        /// Used in abstract species observation class: Identification.
        /// Data type: Boolean.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        UncertainDetermination,

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about current validation status
        /// for the species observation.
        ///
        /// Used in abstract species observation class: SpeciesObservation.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        ValidationStatus,

        /// <summary>
        /// Darwin Core term name: verbatimCoordinates.
        /// The verbatim original spatial coordinates of the Location.
        /// The coordinate ellipsoid, geodeticDatum, or full
        /// Spatial Reference System (SRS) for these coordinates
        /// should be stored in verbatimSRS and the coordinate
        /// system should be stored in verbatimCoordinateSystem.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimCoordinates,

        /// <summary>
        /// Darwin Core term name: verbatimCoordinateSystem.
        /// The spatial coordinate system for the verbatimLatitude
        /// and verbatimLongitude or the verbatimCoordinates of the
        /// Location.
        /// Recommended best practice is to use a controlled vocabulary.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimCoordinateSystem,

        /// <summary>
        /// Darwin Core term name: verbatimDepth.
        /// The original description of the
        /// depth below the local surface.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimDepth,

        /// <summary>
        /// Darwin Core term name: verbatimElevation.
        /// The original description of the elevation (altitude,
        /// usually above sea level) of the Location.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimElevation,

        /// <summary>
        /// Darwin Core term name: verbatimEventDate.
        /// The verbatim original representation of the date
        /// and time information for an Event.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimEventDate,

        /// <summary>
        /// Darwin Core term name: verbatimLatitude.
        /// The verbatim original latitude of the Location.
        /// The coordinate ellipsoid, geodeticDatum, or full
        /// Spatial Reference System (SRS) for these coordinates
        /// should be stored in verbatimSRS and the coordinate
        /// system should be stored in verbatimCoordinateSystem.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimLatitude,

        /// <summary>
        /// Darwin Core term name: verbatimLocality.
        /// The original textual description of the place.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimLocality,

        /// <summary>
        /// Darwin Core term name: verbatimLongitude.
        /// The verbatim original longitude of the Location.
        /// The coordinate ellipsoid, geodeticDatum, or full
        /// Spatial Reference System (SRS) for these coordinates
        /// should be stored in verbatimSRS and the coordinate
        /// system should be stored in verbatimCoordinateSystem.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimLongitude,

        /// <summary>
        /// Darwin Core term name: verbatimSRS.
        /// The ellipsoid, geodetic datum, or spatial reference
        /// system (SRS) upon which coordinates given in
        /// verbatimLatitude and verbatimLongitude, or
        /// verbatimCoordinates are based.
        /// Recommended best practice is use the EPSG code as
        /// a controlled vocabulary to provide an SRS, if known.
        /// Otherwise use a controlled vocabulary for the name or
        /// code of the geodetic datum, if known.
        /// Otherwise use a controlled vocabulary for the name or
        /// code of the ellipsoid, if known. If none of these is
        /// known, use the value "unknown".
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimSRS,

        /// <summary>
        /// Not defined in Darwin Core.
        /// ScientificName as it appears in the original record.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimScientificName,

        /// <summary>
        /// Darwin Core term name: verbatimTaxonRank.
        /// The taxonomic rank of the most specific name in the
        /// scientificName as it appears in the original record.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        VerbatimTaxonRank,

        /// <summary>
        /// Darwin Core term name: vernacularName.
        /// A common or vernacular name.
        ///
        /// Used in abstract species observation class: Taxon.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// Should not provide a value for this property. Current value 
        /// in Dyntaxa will be used (based on property DyntaxaTaxonID).
        /// </summary>
        [EnumMember]
        VernacularName,

        /// <summary>
        /// Darwin Core term name: waterBody.
        /// The name of the water body in which the Location occurs.
        /// Recommended best practice is to use a controlled
        /// vocabulary such as the Getty Thesaurus of Geographic Names.
        ///
        /// Used in abstract species observation class: Location.
        /// Data type: String.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// </summary>
        [EnumMember]
        WaterBody,

        /// <summary>
        /// Darwin Core term name: year.
        /// The four-digit year in which the Event occurred,
        /// according to the Common Era Calendar.
        ///
        /// Used in abstract species observation class: Event.
        /// Data type: 32 bits integer.
        /// Data provider to SwedishSpeciesObservationSOAPService:
        /// May provide a value for this property.
        /// Value for this property will be calculated from property
        /// Start if no value was provided by data provider.
        /// </summary>
        [EnumMember]
        Year
    }
}
