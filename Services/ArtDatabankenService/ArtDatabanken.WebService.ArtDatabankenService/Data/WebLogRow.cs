using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
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
        /// Log entry simply states that something has happened.
        /// </summary>
        [EnumMember]
        Information,
        /// <summary>
        /// Log entry related to security.
        /// </summary>
        [EnumMember]
        Security,
        /// <summary>
        /// Log about update of species observation update.
        /// </summary>
        [EnumMember]
        SpeciesObservationUpdate,
        /// <summary>
        /// Log about update of species observation update
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
        /// <summary>
        /// Create a WebLogRow instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebLogRow(DataReader dataReader)
        {
            base.LoadData(dataReader);
        }
    }
}
