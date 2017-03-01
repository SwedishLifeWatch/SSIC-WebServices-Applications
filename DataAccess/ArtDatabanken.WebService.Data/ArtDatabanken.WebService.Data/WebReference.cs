using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a reference.
    /// </summary>
    [DataContract]
    public class WebReference : WebData
    {
        /// <summary>
        /// Id for this reference.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if property ModifiedDate has a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsModifiedDateSpecified { get; set; }

        /// <summary>
        /// Indicates if property Year has a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsYearSpecified { get; set; }

        /// <summary>
        /// Name of the person that made the last
        /// update of this reference.
        /// </summary>
        [DataMember]
        public String ModifiedBy { get; set; }

        /// <summary>
        /// Date when the reference was last updated.
        /// Property IsModifiedDateSpecified indicates if
        /// this property has a value or not.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Name for this reference.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Title for the reference.
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// Year for this reference.
        /// Property IsYearSpecified indicates if
        /// this property has a value or not.
        /// </summary>
        [DataMember]
        public Int32 Year { get; set; }
    }
}
