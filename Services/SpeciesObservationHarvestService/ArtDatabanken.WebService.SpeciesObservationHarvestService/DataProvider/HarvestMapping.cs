using System;
using System.Data;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// Contains mapping properties used by harvest service.
    /// </summary>
    public class HarvestMapping
    {
        public String Property { get; set; }

        public Boolean Mandatory { get; set; }

        public String Class { get; set; }

        public String Type { get; set; }

        public String Name { get; set; }

        public String Method { get; set; }

        public String Default { get; set; }

        public Boolean IsSearchable { get; set; }

        public Boolean IsDarwinCore { get; set; }

        public Boolean IsMandatoryFromProvider { get; set; }

        public Boolean IsObtainedFromProvider { get; set; }

        public String PersistedInTable { get; set; }

        public String GUID { get; set; }

        public String PropertyIdentifier { get; set; }

        /// <summary>
        /// Compares this instance of HarvestMapping to a data row and returns a boolean indicating if there are any differences
        /// </summary>
        /// <param name="descriptionRow">A DataRow containing the description</param>
        /// <param name="mappingRow">A DataRow containing the mapping</param>
        /// <returns>A boolean, true if there are any differences and false if all properties of the both instances are equal</returns>
        public bool HasDifferences(DataRow descriptionRow, DataRow mappingRow)
        {
            return
                !this.IsEqual(this.Class, descriptionRow[SpeciesObservationFieldDescriptionData.CLASS]) ||
                !this.IsEqual(this.Property, descriptionRow[SpeciesObservationFieldDescriptionData.NAME]) ||
                !this.IsEqual(this.Name, mappingRow[SpeciesObservationFieldMappingData.PROVIDER_FIELD_NAME]) ||
                !this.IsEqual(this.Mandatory, descriptionRow[SpeciesObservationFieldDescriptionData.IS_MANDATORY]) ||
                !this.IsEqual(this.Type, descriptionRow[SpeciesObservationFieldDescriptionData.TYPE]) ||
                !this.IsEqual(this.Method, mappingRow[SpeciesObservationFieldMappingData.METHOD]) ||
                !this.IsEqual(this.Default, mappingRow[SpeciesObservationFieldMappingData.DEFAULT_VALUE]) ||
                !this.IsEqual(this.IsSearchable, descriptionRow[SpeciesObservationFieldDescriptionData.IS_SEARCHABLEFIELD]) ||
                !this.IsEqual(this.IsDarwinCore, descriptionRow[SpeciesObservationFieldDescriptionData.IS_DARWINCORE]) ||
                !this.IsEqual(this.IsMandatoryFromProvider, descriptionRow[SpeciesObservationFieldDescriptionData.IS_MANDATORY_FROM_PROVIDER]) ||
                !this.IsEqual(this.IsObtainedFromProvider, descriptionRow[SpeciesObservationFieldDescriptionData.IS_OBTAINED_FROM_PROVIDER]) ||
                !this.IsEqual(this.PersistedInTable, descriptionRow[SpeciesObservationFieldDescriptionData.PERSISTED_IN_TABLE]) ||
                !this.IsEqual(this.GUID, descriptionRow[SpeciesObservationFieldDescriptionData.GUID]);
        }

        /// <summary>
        /// Compares two objects and returns true if they are equal or if both objects are null
        /// </summary>
        /// <param name="objA">The first object</param>
        /// <param name="objB">The second object</param>
        /// <returns>A boolean indicating whether the two objects are equal or if both objects are null</returns>
        private bool IsEqual(object objA, object objB)
        {
            if (objA == null)
            {
                return objB == null || objB is DBNull;
            }

            return objA.Equals(objB);
        }

        /// <summary>
        /// Test if mapping is a project parameter field.
        /// </summary>
        /// <returns>True, if mapping is a project parameter field.</returns>
        public Boolean IsProjectParameter()
        {
            return PropertyIdentifier.IsNotEmpty() &&
                   PropertyIdentifier.Contains("ProjectParameter");
        }
    }
}
