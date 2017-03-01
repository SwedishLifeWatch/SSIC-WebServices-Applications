using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an phone number.
    /// </summary>
    [DataContract]
    public class WebPhoneNumber : WebData
    {
        /// <summary>
        /// Country.
        /// </summary>
        [DataMember]
        public WebCountry Country
        { get; set; }

        /// <summary>
        /// Country id.
        /// </summary>
        public Int32 CountryId
        { get; set; }

        /// <summary>
        /// Id for this phonenumber
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        [DataMember]
        public String Number
        { get; set; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public Int32 OrganizationId
        { get; set; }

        /// <summary>
        /// Person id.
        /// </summary>
        public Int32 PersonId
        { get; set; }

        /// <summary>
        /// Type of phone number.
        /// </summary>
        [DataMember]
        public WebPhoneNumberType Type
        { get; set; }

        /// <summary>
        /// Phone number type id.
        /// </summary>
        public Int32 TypeId
        { get; set; }
   }
}
