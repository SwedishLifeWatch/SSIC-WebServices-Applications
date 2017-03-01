using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of different types of log messages.
    /// </summary>
    [DataContract]
    public enum LogType
    {
        /// <summary>
        /// No specific log type.
        /// </summary>
        [EnumMember]
        None,
        /// <summary>
        /// An error log row.
        /// </summary>
        [EnumMember]
        Error,
        /// <summary>
        /// Log entry that simply states that something has happened.
        /// </summary>
        [EnumMember]
        Information,
        /// <summary>
        /// Log entry related to security.
        /// </summary>
        [EnumMember]
        Security,
        /// <summary>
        /// Log about update of species observation table.
        /// </summary>
        [EnumMember]
        SpeciesObservationStatistic,
        /// <summary>
        /// Log about nightly update of species observation table.
        /// </summary>
        [EnumMember]
        SpeciesObservationUpdate,
        /// <summary>
        /// Log about nightly update of species observation table.
        /// in the species gateway.
        /// </summary>
        [EnumMember]
        SpeciesObservationArtportalenUpdate,
        /// <summary>
        /// Trace entry.
        /// </summary>
        [EnumMember]
        Trace
    }

    /// <summary>
    /// This class represents an entry in the web service log.
    /// </summary>
    [DataContract]
    public class WebLogRow : WebData
    {
    }
}
