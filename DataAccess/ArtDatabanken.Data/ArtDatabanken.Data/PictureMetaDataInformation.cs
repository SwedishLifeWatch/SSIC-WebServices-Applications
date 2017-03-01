using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about metadata set to a picture for a list of pictures.
    /// </summary>
    public class PictureMetaDataInformation : IPictureMetaDataInformation
    {

        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Picture id.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        public List<IPictureMetaData> PictureMetaDataList { get; set; }
    }
}