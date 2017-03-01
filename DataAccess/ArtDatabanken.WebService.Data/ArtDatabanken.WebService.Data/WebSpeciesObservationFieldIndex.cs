using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains index information related
    /// to a species observation field instance.
    /// This class i currently not used.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationFieldIndex : WebData
    {
        /// <summary>
        /// Defines array index if class may have multiple instances.
        /// This property i currently not used.
        /// </summary>
        [DataMember]
        public Int64 ClassIndex { get; set; }

        /// <summary>
        /// Specifies if property ClassIndex has a value or not.
        /// This property i currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsClassIndexSpecified { get; set; }

        /// <summary>
        /// Specifies if property PropertyIndex has a value or not.
        /// This property i currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPropertyIndexSpecified { get; set; }

        /// <summary>
        /// Defines array index if property may have multiple values.
        /// This property i currently not used.
        /// </summary>
        [DataMember]
        public Int64 PropertyIndex { get; set; }
    }
}
