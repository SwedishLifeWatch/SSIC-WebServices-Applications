using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an address.
    /// </summary>
    [Serializable]
    public class Address : IAddress
    {
        private IAddressType _type;
        private ICountry _country;

        /// <summary>
        /// Create an Address instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Address(IUserContext userContext)
        {
            // Set default values.
            City = null;
            Country = CoreData.CountryManager.GetCountry(userContext, CountryId.Sweden);
            DataContext = new DataContext(userContext);
            Id = Int32.MinValue;
            PostalAddress1 = null;
            PostalAddress2 = null;
            Type = CoreData.UserManager.GetAddressType(userContext, AddressTypeId.Home);
            ZipCode = null;
        }

        /// <summary>
        /// City. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        public String City
        { get; set; }

        /// <summary>
        /// Country. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        public ICountry Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (value.IsNotNull())
                {
                    _country = value;
                }
            }
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id for this address.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// First line in the postal address. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        public String PostalAddress1
        { get; set; }

        /// <summary>
        /// Second line in the postal address. Optional.
        /// </summary>
        public String PostalAddress2
        { get; set; }

        /// <summary>
        /// Type of address. Not NULL.
        /// </summary>
        public IAddressType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value.IsNotNull())
                {
                    _type = value;
                }
            }
        }

        /// <summary>
        /// Zip code. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        public String ZipCode
        { get; set; }
    }
}
