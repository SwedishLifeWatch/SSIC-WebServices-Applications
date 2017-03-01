using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxon.
    /// </summary>
    public interface ITaxon : IDataId32
    {
        /// <summary>
        /// Alert status for this taxon.
        /// A classification of the need for communication of
        /// problems related to the taxon status and recognition.
        /// Might be used to decide if description
        /// text is displayed as warning.
        /// </summary>
        ITaxonAlertStatus AlertStatus { get; set; }

        /// <summary>
        /// Author of the recommended scientific name.
        /// </summary>
        String Author { get; set; }

        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        ITaxonCategory Category { get; set; }

        /// <summary>
        /// Change status for this taxon.
        /// Indicates if this taxon has been lumped, splited or deleted.
        /// </summary>
        ITaxonChangeStatus ChangeStatus { get; set; }

        /// <summary>
        /// Recommended common name.
        /// Not all taxa has a recommended common name.
        /// </summary>
        String CommonName { get; set; }

        /// <summary>
        /// Id of user that created the taxon.
        /// Mandatory ie always required.
        /// </summary> 
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// Name of the person who created this taxon.
        /// Not required ie could be null.
        /// </summary>
        String CreatedByPerson { get; set; }

        /// <summary>
        /// The taxon was created at this date.
        /// Mandatory ie always required.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Indicates if this taxon is in a checked out revision
        /// and may be updated.
        /// </summary>
        Boolean IsInRevision { get; set; }

        /// <summary>
        /// If this taxon is in a checked out revision then this is the RevisionId; otherwise null.
        /// </summary>        
        Int32? RevisionId { get; set; }

        /// <summary>
        /// Indicates that a specie is a microspecies
        /// </summary>
        Boolean IsMicrospecies { get; set; }

        /// <summary>
        /// Indicates if the information in this taxon instance
        /// has been published or not.
        /// </summary>
        Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates if this taxon is valid or not.
        /// </summary>
        Boolean IsValid { get; set; }

        /// <summary>
        /// Taxon was modified by the user with this id.
        /// </summary>
        Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Name of the person who last modified this taxon.
        /// Not required ie could be null.
        /// </summary>
        String ModifiedByPerson { get; set; }

        /// <summary>
        /// Date taxon was modified.
        /// Set by database revision with taxon in is checked in
        /// </summary>
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        String ScientificName { get; set; }

        /// <summary>
        /// Sorting order for this taxon.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidToDate { get; set; }        

        /// <summary>
        /// Get all valid child taxon relations.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All valid child taxon relations.</returns>
        TaxonRelationList GetAllChildTaxonRelations(IUserContext userContext);

        /// <summary>
        /// Get all parent taxon relations.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="categoryId">The category.</param>
        /// <param name="isTaxonRevisionEditor"></param>        
        /// <param name="includeHistorical"></param>
        /// <param name="isMainRelation"></param>
        /// <returns>
        /// </returns>
        IList<ITaxonRelation> GetAllParentTaxonRelations(IUserContext userContext, int? categoryId, bool isTaxonRevisionEditor, bool includeHistorical, bool isMainRelation = false);

        /// <summary>
        /// Gets the other parent taxa. I.e. all secondary relations.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All other parent taxa.</returns>
        IList<ITaxon> GetOtherParentTaxa(IUserContext userContext);

        /// <summary>
        /// The get non valid parents.
        /// </summary>
        /// <returns>
        /// The <see cref="TaxonRelationList"/>.
        /// </returns>
        TaxonRelationList GetNonValidParents(IUserContext userContext);

        /// <summary>
        /// Get nearest non valid parents.
        /// </summary>
        /// <returns>
        /// The <see cref="TaxonRelationList"/>.
        /// </returns>
        TaxonRelationList GetNearestNonValidParents(IUserContext userContext);

        /// <summary>
        /// Delivers all currently valid direct parent taxa for this taxon while in a revision
        /// </summary>
        List<ITaxonRelation> GetCheckedOutChangesParentTaxa(IUserContext userContext);

        /// <summary>
        /// Gets CheckedOutChangesTaxonName.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>CheckedOutChangesTaxonName.</returns>
        IList<ITaxonName> GetCheckedOutChangesTaxonName(IUserContext userContext);

        /// <summary>
        /// Delivers the current valid TaxonProperty object for a taxon when in a revision
        /// </summary>
        ITaxonProperties GetCheckedOutChangesTaxonProperties(IUserContext userContext);

        /// <summary>
        /// Get child taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is the top of the child taxon tree.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="isValid">
        /// Limit returned taxon trees to valid taxa and taxon relations.
        /// All taxa and taxon relations are included in the child taxon tree
        /// if parameter isValid has the value false.
        /// </param>
        /// <returns>
        /// Child taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is the top of the child taxon tree.
        /// </returns>
        ITaxonTreeNode GetChildTaxonTree(IUserContext userContext,
                                         Boolean isValid);

        /// <summary>
        /// Get recommended common name, ie name of type
        /// SWEDISH_NAME and recommended.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended common name.</returns>
        ITaxonName GetCommonName(IUserContext userContext);

        /// <summary>
        /// Full concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        String GetConceptDefinition(IUserContext userContext);

        /// <summary>
        /// Get currently valid taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Currently valid taxon names.</returns>
        IList<ITaxonName> GetCurrentTaxonNames(IUserContext userContext);

        /// <summary>
        /// Get taxon names where taxon name category type 
        /// equals IDENTIFIER except for recommended GUID.
        /// This includes ITIS_NAME, ITIS_NUMBER, NN_CODE and ERMS:Name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Taxon names where taxon name category type 
        /// equals IDENTIFIER except for recommended GUID.
        /// </returns>
        List<ITaxonName> GetIdentifiers(IUserContext userContext);

        /// <summary>
        /// Gets the Name of the Person that has created this taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Name of the Person that has created this taxon.
        /// </returns>
        string GetCreatedByPersonFullName(IUserContext userContext);

        /// <summary>
        /// Gets the Name of the Person that has modified this taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Name of the Person that has modified this taxon.
        /// </returns>
        string GetModifiedByPersonFullname(IUserContext userContext);

        /// <summary>
        /// Last modified by the person with this name.
        /// </summary>
        IPerson GetModifiedByPerson(IUserContext userContext);

        /// <summary>
        /// Get nearest child taxon relations that are valid.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Nearest child taxon relations that are valid.</returns>
        TaxonRelationList GetNearestChildTaxonRelations(IUserContext userContext);

        /// <summary>
        /// Get nearest parent taxon relations that are valid.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Nearest parent taxon relations that are valid.</returns>
        TaxonRelationList GetNearestParentTaxonRelations(IUserContext userContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="isTaxonRevisionEditor"></param>
        /// <param name="includeHistorical"></param>
        /// <param name="isMainRelation"></param>
        /// <returns></returns>
        IList<ITaxonRelation> GetParentTaxonRelations(IUserContext userContext, bool isTaxonRevisionEditor, bool includeHistorical, bool isMainRelation = false);

        /// <summary>
        /// Get parent taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is the bottom of the parent taxon tree.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="isValid">
        /// Limit returned taxon trees to valid taxa and taxon relations.
        /// All taxa and taxon relations are included in the parent taxon tree
        /// if parameter isValid has the value false.
        /// </param>
        /// <returns>
        /// Parent taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is the bottom of the parent taxon tree.
        /// </returns>
        ITaxonTreeNode GetParentTaxonTree(IUserContext userContext,
                                          Boolean isValid);

        /// <summary>
        /// Get recommended GUID as String, ie name of type GUID and recommended.
        /// This method returns null if taxon has no recommended GUID.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended GUID.</returns>
        String GetRecommendedGuid(IUserContext userContext);

        /// <summary>
        /// Gets or sets References.
        /// </summary>
        ReferenceRelationList GetReferenceRelations(IUserContext userContext);

        /// <summary>
        /// Get recommended scientfic name, ie name of type
        /// SCIENTIFIC_NAME and recommended.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended scientfic name.</returns>
        ITaxonName GetScientificName(IUserContext userContext);

        /// <summary>
        /// Get scientific name and author for this taxon.
        /// </summary>
        /// <returns>Scientific name and author for this taxon.</returns>       
        String GetScientificNameAndAuthor();

        /// <summary>
        /// Get scientific name, author and common name for this taxon.
        /// </summary>
        /// <returns>Scientific name, author and common name for this taxon.</returns>       
        String GetScientificNameAndAuthorAndCommonName();

        /// <summary>
        /// Get all synonyms. proParte synonyms are included in the result.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>        
        /// <returns>
        /// Synonyms.
        /// </returns>
        List<ITaxonName> GetSynonyms(IUserContext userContext);

        /// <summary>
        /// Get synonyms.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="includeProParteSynonyms">
        /// If <c>true</c> proParte synonyms will be included in the result.
        /// </param>
        /// <returns>
        /// Synonyms.
        /// </returns>
        List<ITaxonName> GetSynonyms(IUserContext userContext, bool includeProParteSynonyms);

        /// <summary>
        /// Get proParte synonyms.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>proParte synonyms.</returns>
        List<ITaxonName> GetProParteSynonyms(IUserContext userContext);

        /// <summary>
        /// Get taxon name with specified version.       
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameVersion">Taxon name version.</param>
        /// <returns>Taxon name with specified version.</returns>
        ITaxonName GetTaxonNameByVersion(IUserContext userContext,
                                         Int32 taxonNameVersion);

        /// <summary>
        /// Get all taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All taxon names.</returns>
        TaxonNameList GetTaxonNames(IUserContext userContext);

        /// <summary>
        /// Get all taxon names in a specific category.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="categoryId">Category id.</param>
        /// <returns>All taxon names.</returns>
        List<ITaxonName> GetTaxonNamesByCategoryId(IUserContext userContext, Int32 categoryId);

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameCategoryId">Taxon name category id.</param>
        /// <param name="taxonNameCategoryTypeId">Taxon name category type id.</param>
        /// <param name="taxonNameStatusId">Taxon name usage id.</param>
        /// <param name="isRecommended">Is recommended.</param>
        /// <param name="isTaxonRevisionEditor">Is taxon revision editor.</param>
        /// <param name="includeHistorical">Include historical.</param>
        /// <returns>taxon names that matches search criteria.</returns>
        IList<ITaxonName> GetTaxonNamesBySearchCriteria(IUserContext userContext,
                                                        Int32? taxonNameCategoryId,
                                                        Int32? taxonNameCategoryTypeId,
                                                        Int32? taxonNameStatusId,
                                                        Boolean? isRecommended,
                                                        Boolean isTaxonRevisionEditor,
                                                        Boolean includeHistorical);

        /// <summary>
        /// Gets TaxonPropertieses.
        /// </summary>
        List<ITaxonProperties> GetTaxonProperties(IUserContext userContext);

        /// <summary>
        /// Get taxon tree. This is the combination of both child taxon tree
        /// and parent taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is somewhere inside the taxon tree.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="isValid">
        /// Limit returned taxon trees to valid taxa and taxon relations.
        /// All taxa and taxon relations are included in the taxon tree
        /// if parameter isValid has the value false.
        /// </param>
        /// <returns>
        /// Child and parent taxon tree.
        /// Returned taxon tree node is related to this taxon,
        /// which is somewhere inside the taxon tree.
        /// </returns>
        ITaxonTreeNode GetTaxonTree(IUserContext userContext, Boolean isValid);

        /// <summary>
        /// Sets parent taxa.
        /// </summary>
        /// <param name="parentTaxa">The parent taxa.</param>
        void SetParentTaxa(TaxonRelationList parentTaxa);

        /// <summary>
        /// Sets TaxonPropertieses.
        /// </summary>
        /// <param name="properties"></param>
        void SetTaxonProperties(List<ITaxonProperties> properties);

        //List<ITaxonName> GetNewSynonyms(IUserContext userContext);

        /// <summary>
        /// Get misappplied names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Misapplied names.</returns>
        List<ITaxonName> GetMisappliedNames(IUserContext userContext);

        ///// <summary>
        ///// Get accepted names.
        ///// </summary>
        ///// <param name="userContext">The user context.</param>
        ///// <returns>Accepted names.</returns>
        //List<ITaxonName> GetAcceptedNames(IUserContext userContext);
        
    }
}
