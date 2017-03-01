using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a time span.
    /// </summary>
    [DataContract]
    public class WebTimeSpan : WebData
    {
        /// <summary>
        /// Days component of the time interval represented by the
        /// current TimeSpan class.
        /// </summary>
        [DataMember]
        public Int32 Days { get; set; }

        /// <summary>
        /// Hours component of the time interval represented by the
        /// current TimeSpan class.
        /// </summary>
        [DataMember]
        public Int32 Hours { get; set; }

        /// <summary>
        /// Indicates if property Days has been set.
        /// </summary>
        [DataMember]
        public Boolean IsDaysSpecified { get; set; }

        /// <summary>
        /// Indicates if property Hours has been set.
        /// </summary>
        [DataMember]
        public Boolean IsHoursSpecified { get; set; }

        /// <summary>
        /// Indicates if property Minutes has been set.
        /// </summary>
        [DataMember]
        public Boolean IsMinutesSpecified { get; set; }

        /// <summary>
        /// Indicates if property Seconds has been set.
        /// </summary>
        [DataMember]
        public Boolean IsSecondsSpecified { get; set; }

        /// <summary>
        /// Indicates if property NanoSeconds has been set.
        /// </summary>
        [DataMember]
        public Boolean IsNanoSecondsSpecified { get; set; }

        /// <summary>
        /// Minutes component of the time interval represented by the
        /// current TimeSpan class.
        /// </summary>
        [DataMember]
        public Int32 Minutes { get; set; }

        /// <summary>
        /// NanoSeconds component of the time interval represented by the
        /// current TimeSpan class.
        /// </summary>
        [DataMember]
        public Int64 NanoSeconds { get; set; }

        /// <summary>
        /// Seconds component of the time interval represented by the
        /// current TimeSpan class.
        /// </summary>
        [DataMember]
        public Int32 Seconds { get; set; }
    }
}
