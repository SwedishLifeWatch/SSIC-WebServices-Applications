using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Converts data from one data type to another.
    /// Not all combinations of conversions are implemented.
    /// This class is used in WebDataQuery handling.
    /// </summary>
    [DataContract]
    public class WebDataConversion : WebData
    {
        /// <summary>
        /// Create a WebDataConversion instance.
        /// </summary>
        public WebDataConversion()
        {
            ConvertToDataType = DataTypeId.NoDataType;
            DataQuery = null;
        }

        /// <summary>Data type to which data is converted.</summary>
        [DataMember]
        public DataTypeId ConvertToDataType
        { get; set; }

        /// <summary>
        /// Data that should be converted
        /// (after the query has been executed). 
        /// </summary>
        [DataMember]
        public WebDataQuery DataQuery
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            DataQuery.CheckNotNull("DataQuery");
            DataQuery.CheckData();
        }
    }
}
