using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an adress.
    /// </summary>
    [DataContract]
    public class WebAddress : WebData
    {
        /// <summary>
        /// City.
        /// </summary>
        [DataMember]
        public String City
        { get; set; }

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
        /// Address id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Organization id that this address relates to.
        /// </summary>
        public Int32 OrganizationId
        { get; set; }

        /// <summary>
        /// Person id that this address relates to.
        /// </summary>
        public Int32 PersonId
        { get; set; }

        /// <summary>
        /// First line in the postal address.
        /// </summary>
        [DataMember]
        public String PostalAddress1
        { get; set; }

        /// <summary>
        /// Second line in the postal address.
        /// </summary>
        [DataMember]
        public String PostalAddress2
        { get; set; }

        /// <summary>
        /// Type of address.
        /// </summary>
        [DataMember]
        public WebAddressType Type
        { get; set; }

        /// <summary>
        /// Address type id.
        /// </summary>
        public Int32 TypeId
        { get; set; }

        /// <summary>
        /// Zip code.
        /// </summary>
        [DataMember]
        public String ZipCode
        { get; set; }
    }
}
