using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Delegate for species fact update event.
    /// </summary>
    /// <param name="speciesFact">Updated species fact.</param>
    public delegate void SpeciesFactUpdateEventHandler(ISpeciesFact speciesFact);

    /// <summary>
    /// Interface that handles species fact information.
    /// </summary>
    public interface ISpeciesFact : IDataId32
    {
        /// <summary>
        /// Event that fires when species facts are updated.
        /// </summary>
        event SpeciesFactUpdateEventHandler UpdateEvent;

        /// <summary>
        /// Test if automatic update is allowed.
        /// </summary>
        Boolean AllowAutomaticUpdate { get; }

        /// <summary>
        /// Test if manual update is allowed.
        /// </summary>
        Boolean AllowManualUpdate { get; }

        /// <summary>
        /// Test if update is allowed.
        /// </summary>
        Boolean AllowUpdate { get; }

        /// <summary>
        /// Get the factor of this species fact object.
        /// </summary>
        IFactor Factor { get; }

        /// <summary>
        /// Get field 1 of this species fact object.
        /// </summary>
        ISpeciesFactField Field1 { get; }

        /// <summary>
        /// Get field 2 of this species fact object.
        /// </summary>
        ISpeciesFactField Field2 { get; }

        /// <summary>
        /// Get field 3 of this species fact object.
        /// </summary>
        ISpeciesFactField Field3 { get; }

        /// <summary>
        /// Get field 4 of this species fact object.
        /// </summary>
        ISpeciesFactField Field4 { get; }

        /// <summary>
        /// Get field 5 of this species fact object.
        /// </summary>
        ISpeciesFactField Field5 { get; }

        /// <summary>
        /// Get the fields of this species fact object.
        /// </summary>
        SpeciesFactFieldList Fields { get; }

        /// <summary>
        /// Indicates whether or not this species fact has changed.
        /// </summary>
        Boolean HasChanged { get; }

        /// <summary>
        /// Test if this species fact uses field 1.
        /// </summary>
        Boolean HasField1 { get; }

        /// <summary>
        /// Test if this species fact uses field 2.
        /// </summary>
        Boolean HasField2 { get; }

        /// <summary>
        /// Test if this species fact uses field 3.
        /// </summary>
        Boolean HasField3 { get; }

        /// <summary>
        /// Test if this species fact uses field 4.
        /// </summary>
        Boolean HasField4 { get; }

        /// <summary>
        /// Test if this species fact uses field 5.
        /// </summary>
        Boolean HasField5 { get; }

        /// <summary>
        /// Indicates whether or not an host object has been
        /// associated with this species fact. 
        /// </summary>
        Boolean HasHost { get; }

        /// <summary>
        /// Indicates whether or not this species fact has an id.
        /// </summary>
        Boolean HasId { get; }

        /// <summary>
        /// Indicates whether or not a update date has
        /// been associated with this species fact.
        /// </summary>
        Boolean HasModifiedDate { get; }

        /// <summary>
        /// Indicates whether or not a period object has
        /// been associated with this species fact.
        /// </summary>
        Boolean HasPeriod { get; }

        /// <summary>
        /// Indicates whether or not a reference id has
        /// been associated with this species fact.
        /// </summary>
        Boolean HasReference { get; }

        /// <summary>
        /// Get the host of this species fact object.
        /// </summary>
        ITaxon Host { get; }

        /// <summary>
        /// Get the unique identifier for this species fact.
        /// </summary>
        String Identifier { get; }

        /// <summary>
        /// Get the individual category of this species fact object.
        /// </summary>
        IIndividualCategory IndividualCategory { get; }

        /// <summary>
        /// Get the main field of this species fact object.
        /// </summary>
        ISpeciesFactField MainField { get; }

        /// <summary>
        /// Get the update user full name of this species fact object.
        /// </summary>
        String ModifiedBy { get; }

        /// <summary>
        /// Get the update date of this species fact object.
        /// </summary>
        DateTime ModifiedDate { get; }

        /// <summary>
        /// Get the Period of this species fact object.
        /// </summary>
        IPeriod Period { get; }

        /// <summary>
        /// Handle the species fact quality of this species fact.
        /// </summary>
        ISpeciesFactQuality Quality { get; set; }

        /// <summary>
        /// Handle the reference of this species fact object.
        /// </summary>
        IReference Reference { get; set; }

        /// <summary>
        /// Indication if this species fact should be deleted
        /// from database.
        /// </summary>
        Boolean ShouldBeDeleted { get; }

        /// <summary>
        /// Get indication if this species fact should be saved to database.
        /// This property is used to handle the
        /// case when a user changes an existing (in database)
        /// species fact to default values and the species fact
        /// should be removed from the database to save space.
        /// </summary>
        Boolean ShouldBeSaved { get; }

        /// <summary>
        /// Get the substantial fields of this species fact object.
        /// </summary>
        SpeciesFactFieldList SubstantialFields { get; }

        /// <summary>
        /// Get the taxon of this species fact object.
        /// </summary>
        ITaxon Taxon { get; }

        /// <summary>
        /// Check that automatic update is allowed.
        /// </summary>
        void CheckAutomaticUpdate();

        /// <summary>
        /// Check that manual update is allowed.
        /// </summary>
        void CheckManualUpdate();

        /// <summary>
        /// Check that update is allowed.
        /// </summary>
        void CheckUpdate();

        /// <summary>
        /// Send information about species fact update.
        /// </summary>
        void FireUpdateEvent();

        /// <summary>
        /// Reset species fact that has been deleted from database.
        /// Set all values to default or null.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        void Reset(IUserContext userContext);

        /// <summary>
        /// Update species fact with new data from database.
        /// </summary>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="reference">Reference of the species fact.</param>
        /// <param name="modifiedDate">Modified date of the species fact.</param>
        /// <param name="modifiedBy">Name of the update user of the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="quality">Quality of the species fact.</param>
        void Update(Int32 id,
                    IReference reference,
                    DateTime modifiedDate,
                    String modifiedBy,
                    Boolean hasFieldValue1,
                    Double fieldValue1,
                    Boolean hasFieldValue2,
                    Double fieldValue2,
                    Boolean hasFieldValue3,
                    Double fieldValue3,
                    Boolean hasFieldValue4,
                    String fieldValue4,
                    Boolean hasFieldValue5,
                    String fieldValue5,
                    ISpeciesFactQuality quality);
    }
}