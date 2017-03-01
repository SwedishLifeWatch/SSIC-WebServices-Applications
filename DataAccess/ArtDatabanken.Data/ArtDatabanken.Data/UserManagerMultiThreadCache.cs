using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of user related information.
    /// </summary>
    public class UserManagerMultiThreadCache : UserManagerSingleThreadCache
    {
        /// <summary>
        /// Get address types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Address types for specified locale.</returns>
        protected override AddressTypeList GetAddressTypes(ILocale locale)
        {
            AddressTypeList addressTypes = null;

            lock (AddressTypes)
            {
                if (AddressTypes.ContainsKey(locale.ISOCode))
                {
                    addressTypes = (AddressTypeList)(AddressTypes[locale.ISOCode]);
                }
            }
            return addressTypes;
        }

        /// <summary>
        /// Get message types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Message types for specified locale.</returns>
        protected override MessageTypeList GetMessageTypes(ILocale locale)
        {
            MessageTypeList messageTypes = null;

            lock (MessageTypes)
            {
                if (MessageTypes.ContainsKey(locale.ISOCode))
                {
                    messageTypes = (MessageTypeList)(MessageTypes[locale.ISOCode]);
                }
            }
            if (messageTypes.IsNotNull())
            {
                return messageTypes.CloneMessageTypeList();
            }
            else
            {
                return messageTypes;
            }
        }

        
        /// <summary>
        /// Get person genders for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Person genders for specified locale.</returns>
        protected override PersonGenderList GetPersonGenders(ILocale locale)
        {
            PersonGenderList personGenders = null;

            lock (PersonGenders)
            {
                if (PersonGenders.ContainsKey(locale.ISOCode))
                {
                    personGenders = (PersonGenderList)(PersonGenders[locale.ISOCode]);
                }
            }
            return personGenders;
        }

        /// <summary>
        /// Get Phone number types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Phone number types for specified locale.</returns>
        protected override PhoneNumberTypeList GetPhoneNumberTypes(ILocale locale)
        {
            PhoneNumberTypeList phoneNumberTypes = null;

            lock (PhoneNumberTypes)
            {
                if (PhoneNumberTypes.ContainsKey(locale.ISOCode))
                {
                    phoneNumberTypes = (PhoneNumberTypeList)(PhoneNumberTypes[locale.ISOCode]);
                }
            }
            return phoneNumberTypes;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (AddressTypes)
            {
                AddressTypes.Clear();
            }
            lock (MessageTypes)
            {
                MessageTypes.Clear();
            }
            lock (PersonGenders)
            {
                PersonGenders.Clear();
            }
            lock (PhoneNumberTypes)
            {
                PhoneNumberTypes.Clear();
            }
        }

        /// <summary>
        /// Set address types for specified locale.
        /// </summary>
        /// <param name="addressTypes">Address types.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Address types for specified locale.</returns>
        protected override void SetAddressTypes(AddressTypeList addressTypes,
                                                ILocale locale)
        {
            lock (AddressTypes)
            {
                AddressTypes[locale.ISOCode] = addressTypes;
            }
        }

        /// <summary>
        /// Set message types for specified locale.
        /// </summary>
        /// <param name="messageTypes">Message types.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Message types for specified locale.</returns>
        protected override void SetMessageTypes(MessageTypeList messageTypes,
                                                ILocale locale)
        {
            lock (MessageTypes)
            {
                MessageTypes[locale.ISOCode] = messageTypes;
            }
        }

        /// <summary>
        /// Set person genders for specified locale.
        /// </summary>
        /// <param name="personGenders">Person genders.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Person genders for specified locale.</returns>
        protected override void SetPersonGenders(PersonGenderList personGenders,
                                                 ILocale locale)
        {
            lock (PhoneNumberTypes)
            {
                PersonGenders[locale.ISOCode] = personGenders;
            }
        }

        /// <summary>
        /// Set phone number types for specified locale.
        /// </summary>
        /// <param name="phoneNumberTypes">Phone number types.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Phone number types for specified locale.</returns>
        protected override void SetPhoneNumberTypes(PhoneNumberTypeList phoneNumberTypes,
                                                    ILocale locale)
        {
            lock (PhoneNumberTypes)
            {
                PhoneNumberTypes[locale.ISOCode] = phoneNumberTypes;
            }
        }
    }
}
