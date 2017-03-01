using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Identifies one instance of data.
    /// This class is used in WebDataQuery handling.
    /// String is used to contain the data id, instead of
    /// Int32, since string is more generic and can handle 
    /// more cases.
    /// </summary>
    [DataContract]
    public class WebDataIdentifier : WebData
    {
        /// <summary>
        /// Create a WebDataIdentifier instance.
        /// </summary>
        public WebDataIdentifier()
        {
            DataType = DataTypeId.NoDataType;
            Identifier = null;
        }

        /// <summary>Type of data that the identifier references.</summary>
        [DataMember]
        public DataTypeId DataType
        { get; set; }

        /// <summary>
        /// Identifier for the data.
        /// String is used to contain the data id, instead of Int32,
        /// since string is more generic and can handle more cases.
        ///</summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            Identifier = Identifier.CheckSqlInjection();
        }
    }
}
