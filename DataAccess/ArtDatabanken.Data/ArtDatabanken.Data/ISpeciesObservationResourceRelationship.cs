using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains resource relationship information about 
    /// a species observation when a flexible species observation format is required. 
    /// This interface also includes all properties available in Darwin Core 1.5 
    /// se interface IDarwinCoreResourceRelationship.
    /// Further information about the Darwin Core 1.5 properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public interface ISpeciesObservationResourceRelationship
    {
        /// <summary>
        /// Darwin Core term name: relatedResourceID.
        /// An identifier for a related resource (the object,
        /// rather than the subject of the relationship).
        /// This property is currently not used.
        /// </summary>
        String RelatedResourceID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: relationshipAccordingTo.
        /// The source (person, organization, publication, reference)
        /// establishing the relationship between the two resources.
        /// This property is currently not used.
        /// </summary>
        String RelationshipAccordingTo
        { get; set; }

        /// <summary>
        /// Darwin Core term name: relationshipEstablishedDate.
        /// The date-time on which the relationship between the
        /// two resources was established. Recommended best practice
        /// is to use an encoding scheme, such as ISO 8601:2004(E).
        /// This property is currently not used.
        /// </summary>
        String RelationshipEstablishedDate
        { get; set; }

        /// <summary>
        /// Darwin Core term name: RelationshipOfResource.
        /// The relationship of the resource identified by
        /// relatedResourceID to the subject
        /// (optionally identified by the resourceID).
        /// Recommended best practice is to use a controlled vocabulary.
        /// This property is currently not used.
        /// </summary>
        String RelationshipOfResource
        { get; set; }

        /// <summary>
        /// Darwin Core term name: relationshipRemarks.
        /// Comments or notes about the relationship between
        /// the two resources.
        /// This property is currently not used.
        /// </summary>
        String RelationshipRemarks
        { get; set; }

        /// <summary>
        /// Darwin Core term name: resourceID.
        /// An identifier for the resource that is the subject
        /// of the relationship.
        /// This property is currently not used.
        /// </summary>
        String ResourceID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: resourceRelationshipID.
        /// An identifier for an instance of relationship between
        /// one resource (the subject) and another
        /// (relatedResource, the object).
        /// This property is currently not used.
        /// </summary>
        String ResourceRelationshipID
        { get; set; }
    }
}
