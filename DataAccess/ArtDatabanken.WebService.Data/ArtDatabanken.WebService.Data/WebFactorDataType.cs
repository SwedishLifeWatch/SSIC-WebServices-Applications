using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a factor data type.
    /// </summary>
    [DataContract]
    public class WebFactorDataType : WebData
    {
        /// <summary>
        /// User id that created the record.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// This property is currently not used.
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date record was created.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Definition for this factor data type.
        /// </summary>
        [DataMember]
        public String Definition { get; set; }

        /// <summary>
        /// Fields this factor data type.
        /// </summary>
        [DataMember]
        public List<WebFactorField> Fields { get; set; }

        /// <summary>
        /// Id for this factor data type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Id of the user that made the latest update of the record.
        /// Set by database when record is modified.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Date when the latest update of the record was done.
        /// Set by database when record is modified.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Name for this factor data type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}