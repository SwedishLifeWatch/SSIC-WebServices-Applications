using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains change response and pictureId for picture that has been created, updated 
    /// or deleted.
    /// </summary>
    public interface IPictureResponse : IDataId64
    {

        /// <summary>
        /// Data context information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Number of rows in DB that has beenChanged, used for checking that 
        /// change in de was correct.
        /// </summary>
        Int32 AffectedRows { get; set; }
    }
}