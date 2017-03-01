using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains support for dynamic adding of data to WebXXX 
    /// data classes without breaking the web service interface.
    /// </summary>
    [DataContract]
    public class WebData
    {
        /// <summary>
        /// This property can be used to handle an arbitrary set of data.
        /// Example on usage of this property:
        /// Data type may handle a flexible set of data fields or
        /// additional new data should be handled by a data type but you
        /// may want to avoid breaking the web service contract.
        /// </summary>
        [DataMember]
        public List<WebDataField> DataFields
        { get; set; }

        ///  <summary>
        ///  Indicates if data has been checked for validity or not.
        ///  This property is not part of the web service interface.
        ///  </summary>
        public Boolean IsDataChecked
        { get; set; }
    }
}
