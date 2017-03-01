using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains event information about a species
    /// observation in Darwin Core 1.5 compatible format.
    /// Further information about the properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public class DarwinCoreEvent : IDarwinCoreEvent
    {
        /// <summary>
        /// Darwin Core term name: day.
        /// The integer day of the month on which the Event occurred
        /// (start date of observation).
        /// This property is currently not used.
        /// </summary>
        public Int32 Day
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about date and time when the
        /// species observation ended.
        /// </summary>
        public DateTime End
        { get; set; }

        /// <summary>
        /// Darwin Core term name: endDayOfYear.
        /// The latest ordinal day of the year on which the Event
        /// occurred (1 for January 1, 365 for December 31,
        /// except in a leap year, in which case it is 366).
        /// This property is currently not used.
        /// </summary>
        public Int32 EndDayOfYear
        { get; set; }

        /// <summary>
        /// Darwin Core term name: eventDate.
        /// The date-time or interval during which an Event occurred.
        /// For occurrences, this is the date-time when the event
        /// was recorded. Not suitable for a time in a geological
        /// context. Recommended best practice is to use an encoding
        /// scheme, such as ISO 8601:2004(E).
        /// For example: ”2007-03-01 13:00:00 - 2008-05-11 15:30:00”
        /// This property is currently not used.
        /// </summary>
        public String EventDate
        { get; set; }

        /// <summary>
        /// Darwin Core term name: eventID.
        /// A list (concatenated and separated) of identifiers
        /// (publication, global unique identifier, URI) of
        /// media associated with the Occurrence.
        /// This property is currently not used.
        /// </summary>
        public String EventID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: eventRemarks.
        /// Comments or notes about the Event.
        /// This property is currently not used.
        /// </summary>
        public String EventRemarks
        { get; set; }

        /// <summary>
        /// Darwin Core term name: eventTime.
        /// The time or interval during which an Event occurred.
        /// Recommended best practice is to use an encoding scheme,
        /// such as ISO 8601:2004(E).
        /// For example: ”13:00:00 - 15:30:00”
        /// This property is currently not used.
        /// </summary>
        public String EventTime
        { get; set; }

        /// <summary>
        /// Darwin Core term name: fieldNotes.
        /// One of a) an indicator of the existence of, b) a
        /// reference to (publication, URI), or c) the text of
        /// notes taken in the field about the Event.
        /// This property is currently not used.
        /// </summary>
        public String FieldNotes
        { get; set; }

        /// <summary>
        /// Darwin Core term name: fieldNumber.
        /// An identifier given to the event in the field. Often 
        /// serves as a link between field notes and the Event.
        /// This property is currently not used.
        /// </summary>
        public String FieldNumber
        { get; set; }

        /// <summary>
        /// Darwin Core term name: habitat.
        /// A category or description of the habitat
        /// in which the Event occurred.
        /// This property is currently not used.
        /// </summary>
        public String Habitat
        { get; set; }

        /// <summary>
        /// Id for this DarwinCoreEvent.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Darwin Core term name: month.
        /// The ordinal month in which the Event occurred.
        /// This property is currently not used.
        /// </summary>
        public Int32 Month
        { get; set; }

        /// <summary>
        /// Darwin Core term name: samplingEffort.
        /// The amount of effort expended during an Event.
        /// This property is currently not used.
        /// </summary>
        public String SamplingEffort
        { get; set; }

        /// <summary>
        /// Darwin Core term name: samplingProtocol.
        /// The name of, reference to, or description of the
        /// method or protocol used during an Event.
        /// This property is currently not used.
        /// </summary>
        public String SamplingProtocol
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about date and time when the
        /// species observation started.
        /// </summary>
        public DateTime Start
        { get; set; }

        /// <summary>
        /// Darwin Core term name: startDayOfYear.
        /// The earliest ordinal day of the year on which the
        /// Event occurred (1 for January 1, 365 for December 31,
        /// except in a leap year, in which case it is 366).
        /// This property is currently not used.
        /// </summary>
        public Int32 StartDayOfYear
        { get; set; }

        /// <summary>
        /// Darwin Core term name: verbatimEventDate.
        /// The verbatim original representation of the date
        /// and time information for an Event.
        /// This property is currently not used.
        /// </summary>
        public String VerbatimEventDate
        { get; set; }

        /// <summary>
        /// Darwin Core term name: year.
        /// The four-digit year in which the Event occurred,
        /// according to the Common Era Calendar.
        /// This property is currently not used.
        /// </summary>
        public Int32 Year
        { get; set; }
    }
}
