using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebCityInformation class.
    /// </summary>
    public static class WebCityInformationExtension
    {
        /// <summary>
        /// Load data into the WebCityInformation instance.
        /// </summary>
        /// <param name="cityInformation">This WebCityInformation.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebCityInformation cityInformation, DataReader dataReader)
        {
            cityInformation.Name = dataReader.GetString(CityData.NAME);
            cityInformation.CoordinateX = Convert.ToDouble(dataReader.GetInt32(CityData.COORDINATE_X));
            cityInformation.CoordinateY = Convert.ToDouble(dataReader.GetInt32(CityData.COORDINATE_Y));
            cityInformation.Province = dataReader.GetString(CityData.PROVINCE);
            cityInformation.County = dataReader.GetString(CityData.COUNTY);
            cityInformation.Municipality = dataReader.GetString(CityData.MUNICIPALITY);
            cityInformation.Parish = dataReader.GetString(CityData.PARISH);
        }
    }
}
