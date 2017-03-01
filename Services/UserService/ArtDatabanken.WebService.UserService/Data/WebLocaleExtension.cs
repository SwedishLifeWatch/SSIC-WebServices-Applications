using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebLocale class.
    /// </summary>
    public static class WebLocaleExtension
    {
        private static String DEFAULT_NAME = "MISSING_NAME";

        /// <summary>
        /// Load data into the WebLocale instance.
        /// </summary>
        /// <param name="locale">This locale.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebLocale locale,
                                    DataReader dataReader)
        {
            locale.Id = dataReader.GetInt32(LocaleData.ID);
            locale.ISOCode = dataReader.GetString(LocaleData.ISO_CODE);
            locale.Name = dataReader.GetString(LocaleData.NAME);
            locale.NativeName = dataReader.GetString(LocaleData.NATIVE_NAME, DEFAULT_NAME);
        }
    }
}
