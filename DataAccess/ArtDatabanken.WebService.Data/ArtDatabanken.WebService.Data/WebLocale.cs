using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a locale.
    /// </summary>
    [DataContract]
    public class WebLocale : WebData
    {
        /// <summary>
        /// Id for this locale.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// ISO code for this locale.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// </summary>
        [DataMember]
        public String ISOCode
        { get; set; }

        /// <summary>
        /// English name of the locale.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Native name or names of the locale.
        /// </summary>
        [DataMember]
        public String NativeName
        { get; set; }

    }
}
