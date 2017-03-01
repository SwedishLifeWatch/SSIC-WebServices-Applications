using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a phone number.
    /// </summary>
    [Serializable()]
    public class PhoneNumber : IPhoneNumber
    {
        private ICountry _country;
        private IPhoneNumberType _type;

        /// <summary>
        /// Create an PhoneNumber instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public PhoneNumber(IUserContext userContext)
        {
            Country = CoreData.CountryManager.GetCountry(userContext, CountryId.Sweden);
            DataContext = new DataContext(userContext);
            Id = Int32.MinValue;
            Number = null;
            Type = CoreData.UserManager.GetPhoneNumberType(userContext, PhoneNumberTypeId.Home);
        }

        /// <summary>
        /// Create an PhoneNumber instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='id'>Id for this address type.</param>
        /// <param name='dataContext'>Data context.</param>
        public PhoneNumber(IUserContext userContext,
                           Int32 id,
                           IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            Country = CoreData.CountryManager.GetCountry(userContext, CountryId.Sweden);
            DataContext = dataContext;
            Id = id;
            Number = null;
            Type = CoreData.UserManager.GetPhoneNumberType(userContext, PhoneNumberTypeId.Home);
        }

        /// <summary>
        /// Country. Not null.
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
        /// Id for this phone number.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Phone number. Not empty.
        /// </summary>
        public String Number
        { get; set; }

        /// <summary>
        /// Get country specific phone number prefix.
        /// Not NULL. Is read only.
        /// </summary>
        public Int32 Prefix
        {
            get
            {
                return Country.PhoneNumberPrefix;
            }
        }

        /// <summary>
        /// Type of phone number. Not null.
        /// </summary>
        public IPhoneNumberType Type
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
    }
}
