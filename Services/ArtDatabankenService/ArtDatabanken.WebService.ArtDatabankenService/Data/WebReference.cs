using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected references should be used.
    /// </summary>
    public enum UserSelectedReferenceUsage
    {
        /// <summary>
        /// References are used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// References are used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    /// This class represents a reference.
    /// </summary>
    [DataContract]
    public class WebReference : WebData
    {
        /// <summary>
        /// Create an instance of reference.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebReference(DataReader dataReader)
        {
            Id = dataReader.GetInt32(ReferenceData.ID);
            Name = dataReader.GetString(ReferenceData.NAME);
            Year = dataReader.GetInt32(ReferenceData.YEAR, -1);
            Text = dataReader.GetString(ReferenceData.TEXT);
        }

        /// <summary>
        /// Id for this reference.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this reference.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Text for this reference.
        /// </summary>
        [DataMember]
        public String Text
        { get; set; }

        /// <summary>
        /// Year for this reference.
        /// </summary>
        [DataMember]
        public Int32 Year
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public void CheckData(WebServiceContext context)
        {
            Name = Name.CheckSqlInjection();
            Name.CheckLength(GetNameMaxLength(context));
            Text = Text.CheckSqlInjection();
            Text.CheckLength(GetTextMaxLength(context));
        }

        /// <summary>
        /// Get max string length for name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for name.</returns>
        public static Int32 GetNameMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, ReferenceData.TABLE_NAME, ReferenceData.NAME_COLUMN);
        }

        /// <summary>
        /// Get max string length for text.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for text.</returns>
        public static Int32 GetTextMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, ReferenceData.TABLE_NAME, ReferenceData.TEXT_COLUMN);
        }
    }
}
