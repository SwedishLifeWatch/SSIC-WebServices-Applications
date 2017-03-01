using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of picture metadata descriptions.
    /// </summary>
    public enum PictureMetaDataDescriptionId
    {
        /// <summary>
        /// No metadata.
        /// </summary>
        None = 0,
        /// <summary>
        /// Copyright notic.
        /// </summary>
        Copyright = 1,
        /// <summary>
        /// Name of the person who originally created the photograph.
        /// </summary>
        PictureCreatedBy = 2,
        /// <summary>
        /// Date and time when the photograph was created.
        /// </summary>
        PictureCreatedDate = 3,
        /// <summary>
        /// A short decription of what the document contains. For pictures, this should be shown 
        /// as a neutrally kept photograph text which can function no matter of the context in 
        /// which the photograph shall be shown.
        /// </summary>
        Description = 4,
        /// <summary>
        /// Comma separated words or short phrases.
        /// </summary>
        Keywords = 5,
        /// <summary>
        /// Date when the photograph was modified.
        /// </summary>
        PictureModifiedBy = 6,
        /// <summary>
        /// The Rights Usage Terms field should include free-text 
        /// instructions on how this photograph can be legally used.
        /// </summary>
        UsageTerms = 7,
        /// <summary>
        /// Indication if an image may be displayed publicly. If false, the image will 
        /// only appear for users who have permission to manage ArtDatabankens images.
        /// </summary>
        IsPublic = 8,
        /// <summary>
        /// Indicates whether the image is the latest version
        /// </summary>
        IsLatestVersion = 9,
        /// <summary>
        /// Unique string based on filename and date when the image was created. 
        /// This value is the same for all versions of an image.
        /// </summary>
        PictureStringId = 10,
        /// <summary>
        /// Globally unique identifier in LSID format.
        /// </summary>
        Guid = 11,
        /// <summary>
        /// GUID of the image as the current image originates. If the image is a partial 
        /// enlargement of a different image such.
        /// </summary>
        OriginalGuid = 12,
        /// <summary>
        /// Filename that the image has when it is uploaded.
        /// </summary>
        FileName = 13,
        /// <summary>
        /// URL to the original source for the image. Can be a URL to the image in the 
        /// Species Observations System gallery.
        /// </summary>
        SourceUrl = 14,
        /// <summary>
        /// Picture format such as jpeg, bmp, png or tiff. Apparent from the file extension
        /// and can be loaded automatically on upload or changed on download.
        /// </summary>
        FileFormat = 15,
        /// <summary>
        /// Controls whether the image file should be stored on fast 
        /// expensive disks or archived on servers with lower storage cost. 
        /// Archived original is not meant to be shown publicly.
        /// </summary>
        IsArchived = 16,
        /// <summary>
        /// Date and time when the image was last edited.
        /// </summary>
        PictureModifiedDate = 17,
        /// <summary>
        /// Unique ID on the image that the metadata belongs. 
        /// This value is unique for each version of an image.
        /// </summary>
        PictureVersionId = 18,
        /// <summary>
        /// Specifies the life stage that the species in the picture is in. 
        /// Eg egg, pupa or adult.
        /// </summary>
        LifeStage = 19,
        /// <summary>
        /// Indicates the gender of the species in the image.
        /// </summary>
        Sex = 20,
        /// <summary>
        /// Name of the place where the photo was taken.
        /// </summary>
        Location = 21,
        /// <summary>
        /// Name of community close to where the picture was taken.
        /// </summary>
        City = 22,
        /// <summary>
        /// Province where the picture was taken.
        /// </summary>
        Province = 23,
        /// <summary>
        /// Country code according to the ISO standard. SE if the image is from Sweden.
        /// </summary>
        CountryCode = 24,
        /// <summary>
        /// Id (according to Dyntaxa) for the taxon in the picture.
        /// </summary>
        TaxonId = 25,
        /// <summary>
        /// Scientific name of the taxon that the picture is related to. 
        /// No value if the picture is not related to any taxon.
        /// </summary>
        ScientificName = 26,
        /// <summary>
        /// Swedish name for the taxon that the image is related to. 
        /// No value if the picture is not related to any taxon or 
        /// if there are no Swedish name for the current taxon.
        /// </summary>
        CommonName = 27,
        /// <summary>
        /// Specifies the relations that the image has to other types of data. 
        /// Eg to taxon, species fact or factor.
        /// </summary>
        PictureRelations = 28,
        /// <summary>
        /// Name of the person who have validated the species identification.
        /// </summary>
        TaxonIdentificationValidatedBy = 29,
        /// <summary>
        /// Date when the species identification was validated.
        /// </summary>
        TaxonIdentificationValidatedDate = 30,
        /// <summary>
        /// Name of the competent person who approved the determination of the species.
        /// </summary>
        TaxonIdentificationApproved = 31,
        /// <summary>
        /// Specifies the status that the image has the imaging process handling.
        /// </summary>
        PictureLifeCycleStatus = 32,
        /// <summary>
        /// Reference that indicates where in a published document 
        /// (or similar) that an image is included.
        /// </summary>
        ReferenceNumber = 33,
        /// <summary>
        /// Date and time when the image was uploaded to the picture administration system.
        /// </summary>
        PictureInformationCreatedDate = 34,
        /// <summary>
        /// Indicates who saved image in the picture administration system.
        /// </summary>
        PictureInformationCreatedBy = 35,
        /// <summary>
        /// Specifies who last modified information about the image in 
        /// the picture administration system.
        /// </summary>
        PictureInformationModifiedBy = 36,
        /// <summary>
        /// Date and time at which information about the image was last changed in 
        /// the picture administration system.
        /// </summary>
        PictureInformationModifiedDate = 37
    }
}