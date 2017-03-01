using System;
using System.Collections.Generic;
using System.Reflection;
using ArtDatabanken.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains support for dynamic adding of data to WebXXX 
    /// data classes without breaking the web service interface.
    /// </summary>
    public static class WebDataExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name='data'>Data object.</param>
        public static void CheckData(this WebData data)
        {
            if (data.IsNotNull() &&
                data.DataFields.IsNotEmpty() &&
                !data.IsDataChecked)
            {
                foreach (WebDataField dataField in data.DataFields)
                {
                    dataField.CheckData();
                }
            }
        }

        /// <summary>
        /// Load all unread columns as WebDataFields into this WebData.
        /// </summary>
        /// <param name='data'>Data object.</param>
        /// <param name='dataReader'>An open DataReader.</param>
        public static void LoadData(this WebData data, DataReader dataReader)
        {
            WebDataField dataField;

            if (dataReader.NextUnreadColumn())
            {
                data.DataFields = new List<WebDataField>();
                do
                {
                    dataField = new WebDataField();
                    dataField.LoadData(dataReader);
                    data.DataFields.Add(dataField);
                }
                while (dataReader.NextUnreadColumn());
            }
        }



        /// <summary>
        /// Checks all properties of type String 
        /// </summary>
        /// <param name='data'>Data object.</param>
        public static void CheckStrings(this WebData data)
        {
            if (!data.IsDataChecked)
            {
                PropertyInfo[] propertyInfo;
                Type type;
                String value;
                // Get the type of the class.
                type = data.GetType();
                propertyInfo = type.GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    if (propertyInfo[i].PropertyType.Equals(typeof(String)))
                    {
                        value = (String)propertyInfo[i].GetValue(data, null);
                        value = value.CheckInjection();
                        propertyInfo[i].SetValue(data, value, null);
                    }
                }
            }
        }
    }
}
