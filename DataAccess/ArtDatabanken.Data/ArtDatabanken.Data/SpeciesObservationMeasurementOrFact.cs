﻿using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains measurement or fact information about 
    /// a species observation when a flexible species observation format is required. 
    /// This class also includes all properties available in Darwin Core 1.5 
    /// se class DarwinCoreMeasurementOrFact.
    /// Further information about the Darwin Core 1.5 properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public class SpeciesObservationMeasurementOrFact : ISpeciesObservationMeasurementOrFact
    {
        /// <summary>
        /// Darwin Core term name: measurementAccuracy.
        /// The description of the potential error associated
        /// with the measurementValue.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementAccuracy
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementDeterminedBy.
        /// A list (concatenated and separated) of names of people,
        /// groups, or organizations who determined the value of the
        /// MeasurementOrFact.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementDeterminedBy
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementDeterminedDate.
        /// The date on which the MeasurementOrFact was made.
        /// Recommended best practice is to use an encoding scheme,
        /// such as ISO 8601:2004(E).
        /// This property is currently not used.
        /// </summary>
        public String MeasurementDeterminedDate
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementID.
        /// An identifier for the MeasurementOrFact (information
        /// pertaining to measurements, facts, characteristics,
        /// or assertions). May be a global unique identifier or an
        /// identifier specific to the data set.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementMethod.
        /// A description of or reference to (publication, URI)
        /// the method or protocol used to determine the measurement,
        /// fact, characteristic, or assertion.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementMethod
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementRemarks.
        /// Comments or notes accompanying the MeasurementOrFact.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementRemarks
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementType.
        /// The nature of the measurement, fact, characteristic,
        /// or assertion.
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementType
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementUnit.
        /// The units associated with the measurementValue.
        /// Recommended best practice is to use the
        /// International System of Units (SI).
        /// This property is currently not used.
        /// </summary>
        public String MeasurementUnit
        { get; set; }

        /// <summary>
        /// Darwin Core term name: measurementValue.
        /// The value of the measurement, fact, characteristic,
        /// or assertion.
        /// This property is currently not used.
        /// </summary>
        public String MeasurementValue
        { get; set; }
    }
}
