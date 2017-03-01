using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents meta data of a picture.
    /// </summary>
    public interface IPictureMetaData : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Name for the meta data.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Value of the meta data.
        /// </summary>
        String Value { get; set; }
    }
}