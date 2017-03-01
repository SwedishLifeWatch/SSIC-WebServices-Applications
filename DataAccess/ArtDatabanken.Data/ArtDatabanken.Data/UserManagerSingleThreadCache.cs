using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of user related information.
    /// </summary>
    public class UserManagerSingleThreadCache : UserManager
    {
        /// <summary>
        /// Create a UserManagerSingleThreadCache instance.
        /// </summary>
        public UserManagerSingleThreadCache()
        {
            AddressTypes = new Hashtable();
            PersonGenders = new Hashtable();
            PhoneNumberTypes = new Hashtable();
            MessageTypes = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Address types cache.
        /// </summary>
        protected Hashtable AddressTypes
        { get; private set; }

        /// <summary>
        /// Message types cache.
        /// </summary>
        protected Hashtable MessageTypes
        { get; private set; }

        /// <summary>
        /// Person genders cache.
        /// </summary>
        protected Hashtable PersonGenders
        { get; private set; }

        /// <summary>
        /// Phone number types cache.
        /// </summary>
        protected Hashtable PhoneNumberTypes
        { get; private set; }

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All address types.</returns>
        public override AddressTypeList GetAddressTypes(IUserContext userContext)
        {
            AddressTypeList addressTypes;

            addressTypes = GetAddressTypes(userContext.Locale);
            if (addressTypes.IsNull())
            {
                addressTypes = base.GetAddressTypes(userContext);
                SetAddressTypes(addressTypes, userContext.Locale);
            }
            return addressTypes;
        }

        /// <summary>
        /// Get address types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Address types for specified locale.</returns>
        protected virtual AddressTypeList GetAddressTypes(ILocale locale)
        {
            AddressTypeList addressTypes = null;

            if (AddressTypes.ContainsKey(locale.ISOCode))
            {
                addressTypes = (AddressTypeList)(AddressTypes[locale.ISOCode]);
            }
            return addressTypes;
        }

        /// <summary>
        /// Get all message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All messgae types.</returns>
        public override MessageTypeList GetMessageTypes(IUserContext userContext)
        {
            MessageTypeList messageTypes;

            messageTypes = GetMessageTypes(userContext.Locale);
            if (messageTypes.IsNull())
            {
                messageTypes = base.GetMessageTypes(userContext);
                SetMessageTypes(messageTypes, userContext.Locale);
            }
            return messageTypes.CloneMessageTypeList();
        }

        /// <summary>
        /// Get message types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Message types for specified locale.</returns>
        protected virtual MessageTypeList GetMessageTypes(ILocale locale)
        {
            MessageTypeList messageTypes = null;

            if (MessageTypes.ContainsKey(locale.ISOCode))
            {
                messageTypes = (MessageTypeList)(MessageTypes[locale.ISOCode]);
            }
            return messageTypes;
        }


        /// <summary>
        /// Get all person genders.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list with all person genders.</returns>
        public override PersonGenderList GetPersonGenders(IUserContext userContext)
        {
            PersonGenderList personGenders;

            personGenders = GetPersonGenders(userContext.Locale);
            if (personGenders.IsNull())
            {
                personGenders = base.GetPersonGenders(userContext);
                SetPersonGenders(personGenders, userContext.Locale);
            }
            return personGenders;
        }

        /// <summary>
        /// Get person genders for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Person genders for specified locale.</returns>
        protected virtual PersonGenderList GetPersonGenders(ILocale locale)
        {
            PersonGenderList personGenders = null;

            if (PersonGenders.ContainsKey(locale.ISOCode))
            {
                personGenders = (PersonGenderList)(PersonGenders[locale.ISOCode]);
            }
            return personGenders;
        }

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list with all phone number types.</returns>
        public override PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext)
        {
            PhoneNumberTypeList phoneNumberTypes;

            phoneNumberTypes = GetPhoneNumberTypes(userContext.Locale);
            if (phoneNumberTypes.IsNull())
            {
                phoneNumberTypes = base.GetPhoneNumberTypes(userContext);
                SetPhoneNumberTypes(phoneNumberTypes, userContext.Locale);
            }
            return phoneNumberTypes;
        }

        /// <summary>
        /// Get Phone number types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Phone number types for specified locale.</returns>
        protected virtual PhoneNumberTypeList GetPhoneNumberTypes(ILocale locale)
        {
            PhoneNumberTypeList phoneNumberTypes = null;

            if (PhoneNumberTypes.ContainsKey(locale.ISOCode))
            {
                phoneNumberTypes = (PhoneNumberTypeList)(PhoneNumberTypes[locale.ISOCode]);
            }
            return phoneNumberTypes;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            AddressTypes.Clear();
            MessageTypes.Clear();
            PersonGenders.Clear();
            PhoneNumberTypes.Clear();
        }

        /// <summary>
        /// Set address types for specified locale.
        /// </summary>
        /// <param name="addressTypes">Address types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetAddressTypes(AddressTypeList addressTypes,
                                               ILocale locale)
        {
            AddressTypes[locale.ISOCode] = addressTypes;
        }

        /// <summary>
        /// Set message types for specified locale.
        /// </summary>
        /// <param name="messageTypes">Message types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetMessageTypes(MessageTypeList messageTypes,
                                               ILocale locale)
        {
            MessageTypes[locale.ISOCode] = messageTypes;
        }

        /// <summary>
        /// Set person genders for specified locale.
        /// </summary>
        /// <param name="personGenders">Person genders.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetPersonGenders(PersonGenderList personGenders,
                                                ILocale locale)
        {
            PersonGenders[locale.ISOCode] = personGenders;
        }

        /// <summary>
        /// Set phone number types for specified locale.
        /// </summary>
        /// <param name="phoneNumberTypes">Phone number types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetPhoneNumberTypes(PhoneNumberTypeList phoneNumberTypes,
                                                   ILocale locale)
        {
            PhoneNumberTypes[locale.ISOCode] = phoneNumberTypes;
        }
    }
}
