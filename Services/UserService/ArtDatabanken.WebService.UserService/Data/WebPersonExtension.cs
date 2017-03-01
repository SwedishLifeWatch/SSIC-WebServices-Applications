using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebPerson class.
    /// </summary>
    public static class WebPersonExtension
    {

        /// <summary>
        /// Load data into the WebPerson instance.
        /// </summary>
        /// <param name='person'>The person.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPerson person,
                                    DataReader dataReader)
        {
            person.Id = dataReader.GetInt32(PersonData.ID);
            person.GUID = dataReader.GetString(PersonData.GUID);
            person.FirstName = dataReader.GetString(PersonData.FIRST_NAME);
            person.MiddleName = dataReader.GetString(PersonData.MIDDLE_NAME);
            person.LastName = dataReader.GetString(PersonData.LAST_NAME);
            person.GenderId = dataReader.GetInt32(PersonData.GENDER_ID);
            person.EmailAddress = dataReader.GetString(EmailData.EMAIL_ADDRESS);
            person.ShowEmailAddress = dataReader.GetBoolean(EmailData.SHOW_EMAIL, false);
            person.ShowAddresses = dataReader.GetBoolean(PersonData.SHOW_ADDRESSES);
            person.ShowPhoneNumbers = dataReader.GetBoolean(PersonData.SHOW_PHONENUMBERS);
            person.LocaleISOCode = dataReader.GetString(LocaleData.LOCALE_STRING);
            person.TaxonNameTypeId = dataReader.GetInt32(PersonData.TAXON_NAME_TYPE_ID);
            person.IsBirthYearSpecified = dataReader.IsNotDbNull(PersonData.BIRTH_YEAR);
            if (person.IsBirthYearSpecified)
            {
                person.BirthYear = dataReader.GetDateTime(PersonData.BIRTH_YEAR);
            }
            person.IsDeathYearSpecified = dataReader.IsNotDbNull(PersonData.DEATH_YEAR);
            if (person.IsDeathYearSpecified)
            {
                person.DeathYear = dataReader.GetDateTime(PersonData.DEATH_YEAR);
            }
            person.IsUserIdSpecified = dataReader.IsNotDbNull(PersonData.USER_ID);
            if (person.IsUserIdSpecified)
            {
                person.UserId =  dataReader.GetInt32(PersonData.USER_ID);
            }
            person.URL = dataReader.GetString(PersonData.URL);
            person.Presentation = dataReader.GetString(PersonData.PRESENTATION);
            person.ShowPresentation = dataReader.GetBoolean(PersonData.SHOW_PRESENTATION);
            person.ShowPersonalInformation = dataReader.GetBoolean(PersonData.SHOW_PERSONALINFORMATION);
            person.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(PersonData.ADMINISTRATION_ROLE_ID);
            if (person.IsAdministrationRoleIdSpecified)
            {
                person.AdministrationRoleId = dataReader.GetInt32(PersonData.ADMINISTRATION_ROLE_ID);
            }
            person.HasSpeciesCollection = dataReader.GetBoolean(PersonData.HAS_COLLECTION);
            person.CreatedDate = dataReader.GetDateTime(PersonData.CREATED_DATE);
            person.CreatedBy = dataReader.GetInt32(PersonData.CREATED_BY, 0);
            person.ModifiedDate = dataReader.GetDateTime(PersonData.MODIFIED_DATE);
            person.ModifiedBy = dataReader.GetInt32(PersonData.MODIFIED_BY, 0);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='person'>The person.</param>
        public static void CheckData(this WebPerson person)
        {
            if (!person.IsDataChecked)
            {
                person.CheckStrings();
                person.IsDataChecked = true;
            }
        }
    }
}
