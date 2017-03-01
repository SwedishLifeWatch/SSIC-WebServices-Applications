using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents change response/affected rows in database and pictureId for picture that has been created, updated 
    /// or deleted.
    /// </summary>
    public class PictureResponse : IPictureResponse
    {
        /// <summary>
        /// Data context information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for this picture.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Number of rows in DB that has beenChanged, used for checking that 
        /// change in de was correct.
        /// </summary>
        public Int32 AffectedRows { get; set; }
    }
}