using System;

namespace ArtDatabanken.WebService.GeoReferenceService.Database
{
    /// <summary>
    /// Constants used when accessing city information in the database
    /// </summary>
    public struct CityData
    {
        /// <summary>
        /// the search string to match with the beginning city names
        /// </summary>
        public const string SEARCH_STRING = "SearchString";

        /// <summary>
        /// City Name
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// The X Coordinate
        /// </summary>
        public const string COORDINATE_X = "XCoordinate";

        /// <summary>
        /// The Y Coordinate
        /// </summary>
        public const string COORDINATE_Y = "YCoordinate";

        /// <summary>
        /// The name of the parish
        /// </summary>
        public const string PARISH = "Parish";

        /// <summary>
        /// The name of the Municipality
        /// </summary>
        public const string MUNICIPALITY = "Municipality";

        /// <summary>
        /// The name of the county
        /// </summary>
        public const string COUNTY = "County";

        /// <summary>
        /// The name of the province
        /// </summary>
        public const string PROVINCE = "Province";
    }

    /// <summary>
    /// Constants used when accessing region categories in database.
    /// </summary>
    public struct RegionCategoryData
    {
        /// <summary>
        /// CountryISOCode
        /// </summary>
        public const String COUNTRY_ISO_CODE = "CountryIsoCode";

        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Level
        /// </summary>
        public const String LEVEL = "Level";

        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORT_ORDER = "SortOrder";

        /// <summary>
        /// TypeId
        /// </summary>
        public const String TYPE_ID = "TypeId";

    }

    /// <summary>
    /// Constants used when accessing region information in database.
    /// </summary>
    public struct RegionData
    {
        /// <summary>
        /// BoundingBox
        /// </summary>
        public const String BOUNDING_BOX = "BoundingBox";

        /// <summary>
        /// CategoryId
        /// </summary>
        public const String CATEGORY_ID = "CategoryId";

        /// <summary>
        /// CategoryIds
        /// </summary>
        public const String CATEGORY_IDS = "CategoryIds";

        /// <summary>
        /// CountryISOCodes
        /// </summary>
        public const String COUNTRY_ISO_CODES = "CountryISOCodes";

        /// <summary>
        /// GUIDs
        /// </summary>
        public const String GUIDS = "GUIDs";

        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Region ids
        /// </summary>
        public const String IDS = "Ids";

        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// NativeId
        /// </summary>
        public const String NATIVE_ID = "NativeId";

        /// <summary>
        /// Polygon
        /// </summary>
        public const String POLYGON = "Polygon";

        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";

        /// <summary>
        /// TypeId
        /// </summary>
        public const String TYPE_ID = "Type";

        /// <summary>
        /// Polygon WGS84
        /// </summary>
        public const String POLYGON_WGS84 = "Polygon_WGS84";
    }

    /// <summary>
    /// Constants used when accessing region types in database.
    /// </summary>
    public struct RegionTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }
}
