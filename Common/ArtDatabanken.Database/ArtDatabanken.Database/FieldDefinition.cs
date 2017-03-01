using System;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Definition of one field in Elasticsearch.
    /// </summary>
    public class FieldDefinition
    {
        /// <summary>
        /// Data type in Elasticsearch.
        /// </summary>
        public String DataType { get; set; }

        /// <summary>
        /// Information about how the field is indexed.
        /// </summary>
        public String FieldIndex { get; set; }

        /// <summary>
        /// Used format of the specified data type.
        /// </summary>
        public String Format { get; set; }

        /// <summary>
        /// Name of the index that the type is part of.
        /// </summary>
        public String Index { get; set; }

        /// <summary>
        /// Complete field definition in Json format.
        /// </summary>
        public String Json { get; set; }

        /// <summary>
        /// Field name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Indicates how high resolution that is used for 
        /// data of type geo-point or geo-shape.
        /// 1  = 5004km * 5004km
        /// 2  = 1251km * 625km
        /// 3  = 156km  * 156km
        /// 4  = 39km   * 19,5km
        /// 5  = 4,9km  * 4,9km
        /// 6  = 1,2km  * 0,61km
        /// 7  = 152,8m * 152,8m
        /// 8  = 38,2m  * 19,1m
        /// 9  = 4,78m  * 4,78m
        /// 10 = 1,19m  * 0,60m
        /// 11 = 14,9cm * 14,9cm
        /// 12 = 3,7cm  * 1,8cm
        /// </summary>
        public Int32 TreeLevel { get; set; }

        /// <summary>
        /// Name of the type that the field is defined in.
        /// </summary>
        public String Type { get; set; }
    }
}
