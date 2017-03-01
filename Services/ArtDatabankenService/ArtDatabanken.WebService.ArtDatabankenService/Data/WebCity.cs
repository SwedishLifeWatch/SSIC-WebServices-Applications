using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents information about a city.
    /// </summary>
    [DataContract]
    public class WebCity : WebData
    {
        /// <summary>
        /// Create a WebCity instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebCity(DataReader dataReader)
        {
            County = dataReader.GetString(CityData.COUNTY);
            Municipality = dataReader.GetString(CityData.MUNICIPALITY);
            Name = dataReader.GetString(CityData.NAME);
            Parish = dataReader.GetString(CityData.PARISH);
            XCoordinate = dataReader.GetInt32(CityData.X_COORDINATE);
            YCoordinate = dataReader.GetInt32(CityData.Y_COORDINATE);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// County in which the city is located.
        /// </summary>
        [DataMember]
        public String County
        { get; set; }

        /// <summary>
        /// Municipality in which the city is located.
        /// </summary>
        [DataMember]
        public String Municipality
        { get; set; }

        /// <summary>
        /// Name of the city.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Parish in which the city is located.
        /// </summary>
        [DataMember]
        public String Parish
        { get; set; }

        /// <summary>
        /// XCoordinate where the city is located.
        /// </summary>
        [DataMember]
        public Int32 XCoordinate
        { get; set; }

        /// <summary>
        /// YCoordinate where the city is located.
        /// </summary>
        [DataMember]
        public Int32 YCoordinate
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public void CheckData(WebServiceContext context)
        {
            County = County.CheckSqlInjection();
            County.CheckLength(GetCountyMaxLength(context));
            Municipality = Municipality.CheckSqlInjection();
            Municipality.CheckLength(GetMunicipalityMaxLength(context));
            Name = Name.CheckSqlInjection();
            Name.CheckLength(GetNameMaxLength(context));
            Parish = Parish.CheckSqlInjection();
            Parish.CheckLength(GetParishMaxLength(context));
        }

        /// <summary>
        /// Get max string length for county.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for county.</returns>
        public static Int32 GetCountyMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.User, CityData.TABLE_NAME, CityData.COUNTY_COLUMN);
        }

        /// <summary>
        /// Get max string length for municipality.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for municipality.</returns>
        public static Int32 GetMunicipalityMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.User, CityData.TABLE_NAME, CityData.MUNICIPALITY_COLUMN);
        }

        /// <summary>
        /// Get max string length for name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for name.</returns>
        public static Int32 GetNameMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.User, CityData.TABLE_NAME, CityData.NAME_COLUMN);
        }

        /// <summary>
        /// Get max string length for parish.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for parish.</returns>
        public static Int32 GetParishMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.User, CityData.TABLE_NAME, CityData.PARISH_COLUMN);
        }
    }
}
