using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents meta data of a picture.
    /// </summary>
    public interface IPictureMetaDataInformation : IDataId64
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }


        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        List<IPictureMetaData> PictureMetaDataList { get; set; }
    }
}