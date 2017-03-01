using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents meta data of a picture.
    /// </summary>
    public class PictureMetaData : IPictureMetaData
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }
        
        /// <summary>
        /// Id for the meta data.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for the meta data.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Value of the meta data.
        /// </summary>
        public String Value { get; set; }
    }
}