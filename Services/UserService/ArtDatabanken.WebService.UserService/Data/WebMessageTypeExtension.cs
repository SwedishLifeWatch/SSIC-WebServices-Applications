using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebMessageType class.
    /// </summary>
    public static class WebMessageTypeExtension
    {
        private const String DEFAULT_NAME = "DEFAULT_NAME";

        /// <summary>
        /// Load data into the WebMessageType instance.
        /// </summary>
        /// <param name="messageType">This message type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebMessageType messageType,
                                    DataReader dataReader)
        {
            messageType.Id = dataReader.GetInt32(MessageTypeData.ID);
            messageType.NameStringId = dataReader.GetInt32(MessageTypeData.NAME_STRING_ID);
            messageType.Name = dataReader.GetString(MessageTypeData.NAME, DEFAULT_NAME);
        }
    }
}
