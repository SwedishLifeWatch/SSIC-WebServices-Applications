using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class contains information related to a country.
    /// </summary>
    [DataContract]
    public class WebCountry : WebData
    {
        /// <summary>
        /// Id for this country.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// A two character code representing the country according to ISO-3166.
        /// </summary>
        [DataMember]
        public String ISOAlpha2Code
        { get; set; }

        /// <summary>
        /// A three character code representing the country according to ISO-3166.
        /// </summary>
        [DataMember]
        public String ISOAlpha3Code
        { get; set; }

        /// <summary>
        /// A integer code representing the country according to ISO-3166.
        /// </summary>
        [DataMember]
        public Int32 ISOCode
        { get; set; }

        /// <summary>
        /// ISO Name of the country.
        /// </summary>
        [DataMember]
        public String ISOName
        { get; set; }

        /// <summary>
        /// English name of the country.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Native name or names of the country.
        /// </summary>
        [DataMember]
        public String NativeName
        { get; set; }

        /// <summary>
        /// Native short name or names of the country.
        /// </summary>
        [DataMember]
        public String NativeShortName
        { get; set; }

        /// <summary>
        /// Phone number prefix.
        /// </summary>
        [DataMember]
        public Int32 PhoneNumberPrefix
        { get; set; }

        /// <summary>
        /// Short english name of the country.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }
    }
}
