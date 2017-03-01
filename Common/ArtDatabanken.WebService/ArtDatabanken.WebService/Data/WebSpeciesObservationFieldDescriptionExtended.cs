using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains metadata about a specific data field used internally in service
    /// </summary>
    public class WebSpeciesObservationFieldDescriptionExtended: WebSpeciesObservationFieldDescription
    {
        /// <summary>
        /// if observation is public.
        /// </summary>
        
        public Boolean IsPublic
        { get; set; }

        /// <summary>
        /// if observation is Darwin Core.
        /// </summary>
        
        public Boolean IsDarwinCoreProperty
        { get; set; }

        public String ShortTableName 
        {
            get
            {
                switch (PersistedInTable)
                {
                    case "Taxon":
                    case "Conservation":
                        return "T";
                    case "Position":
                        return "O"; //Same table since View_DCO_POS is used.
                }
                return "O";
            } 
        }
        /// <summary>
        /// Load data into a Species Observation Field Description object from database.
        /// </summary>
        /// <param name="dataReader">An open data reader.</param>
        public void Load(DataReader dataReader)
        {
            String classIdentifier;
            SpeciesObservationClassId classId;
            SpeciesObservationPropertyId propertyId;

            classIdentifier = dataReader.GetString(SpeciesObservationFieldDescriptionData.CLASS);
            Class = new WebSpeciesObservationClass();
            if (Enum.TryParse(classIdentifier, true, out classId))
            {
                Class.Id = classId;
            }
            else
            {
                Class.Id = SpeciesObservationClassId.None;
                Class.Identifier = classIdentifier;
            }

            Definition = dataReader.GetString(SpeciesObservationFieldDescriptionData.DEFINITION);
            DefinitionUrl = dataReader.GetString(SpeciesObservationFieldDescriptionData.DEFINITION_URL);
            Documentation = dataReader.GetString(SpeciesObservationFieldDescriptionData.DOCUMENTATION);
            DocumentationUrl = dataReader.GetString(SpeciesObservationFieldDescriptionData.DOCUMENTATION_URL);
            Guid = dataReader.GetString(SpeciesObservationFieldDescriptionData.GUID);
            Id = dataReader.GetInt32(SpeciesObservationFieldDescriptionData.ID);
            if (dataReader.IsNotDbNull(SpeciesObservationFieldDescriptionData.IMPORTANCE))
            {
                Importance = dataReader.GetInt32(SpeciesObservationFieldDescriptionData.IMPORTANCE);
            }
            IsAcceptedByTdwg = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_ACCEPTED_BY_TDWG);
            IsClass = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_CLASS_NAME);
            IsDarwinCoreProperty = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_DARWINCORE);
            IsImplemented = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_IMPLEMENTED);
            IsMandatory = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_MANDATORY);
            IsMandatoryFromProvider = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_MANDATORY_FROM_PROVIDER);
            IsObtainedFromProvider = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_OBTAINED_FROM_PROVIDER);
            IsPlanned = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_PLANNED);
            IsPublic = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_PUBLIC);
            IsSearchable = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_SEARCHABLEFIELD);
            IsSortable = dataReader.GetBoolean(SpeciesObservationFieldDescriptionData.IS_SORTABLE);
            Label = dataReader.GetString(SpeciesObservationFieldDescriptionData.LABEL);
            Name = dataReader.GetString(SpeciesObservationFieldDescriptionData.NAME);
            PersistedInTable = dataReader.GetString(SpeciesObservationFieldDescriptionData.PERSISTED_IN_TABLE);
            Remarks = dataReader.GetString(SpeciesObservationFieldDescriptionData.REMARKS);
            if (dataReader.IsNotDbNull(SpeciesObservationFieldDescriptionData.SORT_ORDER))
            {
                SortOrder = dataReader.GetInt32(SpeciesObservationFieldDescriptionData.SORT_ORDER);
            }
            String type;
            type = dataReader.GetString(SpeciesObservationFieldDescriptionData.TYPE);
            switch (type)
            {
                case "Boolean":
                    Type = WebDataType.Boolean;
                    break;
                case "DateTime":
                    Type = WebDataType.DateTime;
                    break;
                case "Double":
                case "Single":
                case "Float64":
                    Type = WebDataType.Float64;
                    break;
                case "Byte":
                case "SByte":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                    Type = WebDataType.Int32;
                    break;
                case "Int64":
                case "UInt64":
                    Type = WebDataType.Int64;
                    break;
                default:
                    Type = WebDataType.String;
                    break;
            }
            Uuid = dataReader.GetString(SpeciesObservationFieldDescriptionData.UUID);

            if (!IsClass)
            {
                Property = new WebSpeciesObservationProperty();
                if (Enum.TryParse(Name, true, out propertyId))
                {
                    Property.Id = propertyId;
                }
                else
                {
                    Property.Id = SpeciesObservationPropertyId.None;
                    Property.Identifier = Name;
                }
            }
        }
    }
}
