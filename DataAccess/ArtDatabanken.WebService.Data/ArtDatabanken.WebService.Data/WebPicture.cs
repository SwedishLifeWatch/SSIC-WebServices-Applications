using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a picture.
    /// </summary>
    [DataContract]
    public class WebPicture : WebData
    {
        /// <summary>
        /// The picture is represented in this format.
        /// E.g. 'jpg', 'bmp' or 'png'.
        /// </summary>
        [DataMember]
        public String Format { get; set; }

        /// <summary>
        /// GUID for this picture.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Picture id.
        /// This value is unique even between different
        /// versions of the same image.
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// The image as binary (unsigned byte) data
        /// that has been Base64 encoded.
        /// </summary>
        [DataMember]
        public String Image { get; set; }

        /// <summary>
        /// Note if picture is archived or not.
        /// </summary>
        [DataMember]
        public Boolean IsArchived { get; set; }

        /// <summary>
        /// Note if picture is public or not.
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Property IsTaxonIdSpecified indicates if  
        /// property TaxonId has a value or not.
        /// Not all pictures are related to taxon.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonIdSpecified { get; set; }

        /// <summary>
        /// Last modified date of picture.
        /// </summary>
        [DataMember]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Last updated date of picture.
        /// </summary>
        [DataMember]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Original size (in bytes) of picture in web service.
        /// </summary>
        [DataMember]
        public Int64 OriginalSize { get; set; }

        /// <summary>
        /// String identifier for this picture.
        /// </summary>
        [DataMember]
        public Int64 PictureStringId { get; set; }

        /// <summary>
        /// Size (in bytes) of picture.
        /// </summary>
        [DataMember]
        public Int64 Size { get; set; }

        /// <summary>
        /// This picture is related to taxon with this id.
        /// Property IsTaxonIdSpecified indicates if  
        /// property TaxonId has a value or not.
        /// Not all pictures are related to taxon.
        /// </summary>
        [DataMember]
        public Int32 TaxonId { get; set; }

        /// <summary>
        /// User who last updated the picture.
        /// </summary>
        [DataMember]
        public String UpdatedBy { get; set; }

        /// <summary>
        /// Version id for picture.
        /// </summary>
        [DataMember]
        public Int64 VersionId { get; set; }
    }
}
