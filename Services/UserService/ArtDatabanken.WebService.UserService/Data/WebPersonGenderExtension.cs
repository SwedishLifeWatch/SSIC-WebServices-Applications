using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebPersonGender class.
    /// </summary>
    public static class WebPersonGenderExtension
    {
        private static String DEFAULT_NAME = "DEFAULT_NAME";

        /// <summary>
        /// Load data into the WebPersonGender instance.
        /// </summary>
        /// <param name="personGender">This person gender.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPersonGender personGender,
                                    DataReader dataReader)
        {
            personGender.Id = dataReader.GetInt32(PersonGenderData.ID);
            personGender.NameStringId = dataReader.GetInt32(PersonGenderData.NAME_STRING_ID);
            personGender.Name = dataReader.GetString(PersonGenderData.NAME, DEFAULT_NAME);
        }
    }
}
