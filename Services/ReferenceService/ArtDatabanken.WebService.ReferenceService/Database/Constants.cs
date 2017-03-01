using System;

namespace ArtDatabanken.WebService.ReferenceService.Database
{
    /// <summary>
    /// Constants used when accessing strings that needs translation.
    /// </summary>
    public struct LocaleData
    {
        /// <summary>
        /// Locale id.
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// ISO code.
        /// </summary>
        public const String ISO_CODE = "ISOCode";

        /// <summary>
        /// Locale name.
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// Native name.
        /// </summary>
        public const String NATIVE_NAME = "NativeName";

        /// <summary>
        /// Locale string.
        /// </summary>
        public const String LOCALE_STRING = "LocaleString";

        /// <summary>
        /// Locale id.
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
    }

    /// <summary>
    /// Constants used when accessing user reference information in database.
    /// </summary>
    public struct ReferenceData
    {
        /// <summary>
        /// Reference id.
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Reference ids.
        /// </summary>
        public const String IDS = "ReferenceIds";

        /// <summary>
        /// Modified by person with this name.
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";

        /// <summary>
        /// Date when the reference was last modified.
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";

        /// <summary>
        /// Reference name.
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// Name of the column with the name value.
        /// </summary>
        public const String NAME_COLUMN_NAME = "namn";

        /// <summary>
        /// Modified by person with this name.
        /// </summary>
        public const String PERSON = "Person";

        /// <summary>
        /// Search string.
        /// </summary>
        public const String SEARCH_STRING = "SearchString";

        /// <summary>
        /// Reference table name.
        /// </summary>
        public const String TABLE_NAME = "dt_referens";

        /// <summary>
        /// Title for the reference.
        /// </summary>
        public const String TITLE = "Text";

        /// <summary>
        /// Text column name.
        /// </summary>
        public const String TITLE_COLUMN_NAME = "text";

        /// <summary>
        /// Where condition.
        /// </summary>
        public const String WHERE_CONDITION = "WhereCondition";

        /// <summary>
        /// Year for the reference.
        /// </summary>
        public const String YEAR = "Year";

        /// <summary>
        /// Year column name.
        /// </summary>
        public const String YEAR_COLUMN_NAME = "ar";
    }

    /// <summary>
    /// Constants used when accessing reference relation information in database.
    /// </summary>
    public struct ReferenceRelationData
    {
        /// <summary>
        /// GUID globally unique identifier.
        /// </summary>
        public const String GUID = "GUID";

        /// <summary>
        /// Id for reference related data.
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Related object guid.
        /// </summary>
        public const String RELATEDOBJECTGUID = "RelatedObjectGUID";

        /// <summary>
        /// Reference id.
        /// </summary>
        public const String REFERENCEID = "ReferenceId";

        /// <summary>
        /// Reference ids.
        /// </summary>
        public const String REFERENCE_IDS = "ReferenceIds";

        /// <summary>
        /// Reference relation id.
        /// </summary>
        public const String REFERENCE_RELATION_ID = "ReferenceRelationId";

        /// <summary>
        /// Reference type.
        /// </summary>
        public const String TYPE = "Type";

        /// <summary>
        /// Reference identifier.
        /// </summary>
        public const String IDENTIFIER = "Identifier";

        /// <summary>
        /// Reference description.
        /// </summary>
        public const string DESCRIPTION = "Description";

        /// <summary>
        /// Object GUID.
        /// </summary>
        public const string OBJECT_GUID = "ObjectGuid";
    }
}
