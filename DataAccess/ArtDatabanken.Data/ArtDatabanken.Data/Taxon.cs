using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a taxon.
    /// </summary>
    [Serializable]
    public class Taxon : ITaxon
    {
        private String _conceptDefinition;
        private ReferenceRelationList _referenceRelations;
        private TaxonNameList _taxonNameList;
        private List<ITaxonProperties> _taxonPropertieses;        
        private TaxonRelationList _allValidParentTaxonRelations;
        private TaxonRelationList _allNotValidParentTaxonRelations;
        private TaxonRelationList _nearestNotValidParentTaxonRelations;
        private TaxonRelationList _nearestChildTaxonRelations;
        private TaxonRelationList _nearestParentTaxonRelations;
        private TaxonRelationsTree _parentsTree;

        /// <summary>
        /// Alert status taxon.
        /// A classification of the need for communication of
        /// problems related to the taxon status and recognition.
        /// Might be used to decide if description
        /// text is displayed as warning.
        /// </summary>
        public ITaxonAlertStatus AlertStatus { get; set; }

        /// <summary>
        /// Author of the recommended scientific name.
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        public ITaxonCategory Category { get; set; }

        /// <summary>
        /// ChangeStatus
        /// Indicates taxons lump-split status.
        /// </summary>
        public ITaxonChangeStatus ChangeStatus { get; set; }

        /// <summary>
        /// Recommended common name.
        /// Not all taxa has a recommended common name.
        /// </summary>
        public String CommonName { get; set; }

        /// <summary>
        /// User that created the record.
        ///  Mandatory ie always required.
        /// </summary> 
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date record was created.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Unique identification of a taxon.
        /// Mandatory ie always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if this taxon is in a checked out revision
        /// and may be updated.
        /// </summary>
        public Boolean IsInRevision { get; set; }

        /// <summary>
        /// If this taxon is in a checked out revision then this is the RevisionId; otherwise null.
        /// </summary>
        public int? RevisionId { get; set; }

        /// <summary>
        /// Indicates that a specie is a microspecies
        /// </summary>
        public Boolean IsMicrospecies { get; set; }

        /// <summary>
        /// Gets or sets IsPublished
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// IsValid
        /// true - taxon is valid.
        /// false - taxon is NOT valid.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// Taxon was modified by the user with this id.
        /// Set by database.
        /// </summary>
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Person for a taxon.
        /// This property will return an incorret person. Do not use until it is corrected.
        /// Not required ie could be null.
        /// </summary>
        public String ModifiedByPerson { get; set; }

        /// <summary>
        /// Person for a taxon.
        /// This property will return an incorret person. Do not use until it is corrected.
        /// Not required ie could be null.
        /// </summary>
        public String CreatedByPerson { get; set; }

        /// <summary>
        /// Date taxon was modified.
        /// Set by database revision with taxon in is checked in
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public String PartOfConceptDefinition { get; set; }
    
        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        public String ScientificName { get; set; }

        /// <summary>
        /// SortOrder
        /// Sorting order for this taxon.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Get all valid child taxon relations.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All valid child taxon relations.</returns>
        public TaxonRelationList GetAllChildTaxonRelations(IUserContext userContext)
        {
            ITaxonRelationSearchCriteria searchCriteria;

            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = true;
            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(this);
            return CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
        }

        /// <summary>
        /// Gets the other parent taxa. I.e. all secondary relations.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// All other parent taxa.
        /// </returns>
        public IList<ITaxon> GetOtherParentTaxa(IUserContext userContext)
        {
            TaxonRelationsTree tree;
            tree = IsUserInTaxonRevision(userContext) ? CreateRevisionValidParentsTree(userContext) : GetParentsTree(userContext);
            ITaxonRelationsTreeNode treeNode = tree.GetTreeNode(Id);
            List<ITaxonRelationsTreeNode> otherParents = treeNode.GetAllValidSecondaryParentNodesInRootToNodeOrder();
            return otherParents.Select(x => x.Taxon).ToList();
        }

        /// <summary>
        /// Searches among all levels of parents above the current taxon.
        /// Uses tree search when includeHistorical is false.
        /// Otherwise it uses the collection AllParentTaxa
        /// </summary>
        public IList<ITaxonRelation> GetAllParentTaxonRelations(IUserContext userContext, int? categoryId, bool isTaxonRevisionEditor, bool includeHistorical, bool isMainRelation = false)
        {
            if (includeHistorical == false && this.IsValid == false)
            {
                return new List<ITaxonRelation>();
            }
            else
            {
                if (isTaxonRevisionEditor)
                {
                    return (from taxonRelation in GetAllParentTaxonRelations(userContext, !includeHistorical, isMainRelation)
                            where
                                (!categoryId.HasValue || categoryId.Value == 0 ||
                                 taxonRelation.ParentTaxon.Category.Id == categoryId.Value) &&
                                ((taxonRelation.ValidToDate > DateTime.Now && includeHistorical == false) ||
                                 (taxonRelation.ValidToDate < DateTime.Now && includeHistorical) ||
                                 (IsValid == false && includeHistorical)) &&
                                (taxonRelation.IsMainRelation == true || isMainRelation == false)
                            select taxonRelation).Distinct().ToList();
                }
                // TaxonRevisionEditor is false
                else
                {
                    return (from taxonRelation in GetAllParentTaxonRelations(userContext, !includeHistorical, isMainRelation)
                            where
                                (!categoryId.HasValue || categoryId.Value == 0 || taxonRelation.ParentTaxon.Category.Id == categoryId.Value) &&
                                ((taxonRelation.ValidToDate > DateTime.Now && includeHistorical == false) ||
                                 (taxonRelation.ValidToDate < DateTime.Now && includeHistorical) ||
                                 (IsValid == false && includeHistorical)) &&
                                taxonRelation.IsPublished &&
                                (taxonRelation.IsMainRelation || isMainRelation == false)
                            select taxonRelation).Distinct().ToList();
                }
            }
        }

        /// <summary>
        /// Get all parent taxon relations.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="isValidRequired">Indicates if only valid relations should be returned.</param>
        /// <param name="onlyMainRelations">Only add main relations</param>
        /// <returns>All parent taxon relations.</returns>
        private TaxonRelationList GetAllParentTaxonRelations(IUserContext userContext, Boolean isValidRequired, Boolean onlyMainRelations)
        {
            // If isValidRequired get the parent relations by creating trees. Otherwise use relations.
            if (isValidRequired)
            {
                if (IsUserInTaxonRevision(userContext))
                {
                    return GetRevisionAllValidParentTaxonRelationsByTree(userContext, onlyMainRelations);
                }
                else
                {
                    return GetAllValidParentTaxonRelationsByTree(userContext, onlyMainRelations);
                }
            }
            else
            {
                return GetAllParentTaxonRelationsWhenValidNotRequired(userContext);
            }
        }

        /// <summary>
        /// Gets a parent tree for this taxon using cache.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>A taxon relation tree containing all hierarchical parents to this taxon.</returns>
        private TaxonRelationsTree GetParentsTree(IUserContext userContext)
        {
            if (_parentsTree == null)
            {
                _parentsTree = TaxonRelationsTreeManager.CreateTaxonRelationsParentsTree(userContext, this);
            }

            return _parentsTree;
        }

        /// <summary>
        /// Get all valid parent taxon relations hierarchical top (biota) to bottom (this taxon).
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="onlyMainRelations">
        /// if set to <c>true</c> only main relations are returned, otherwise both main and secondary relations are returned.
        /// </param>
        /// <returns>
        /// A list with all valid parent taxon relations hierarchical top (biota) to bottom (this taxon).
        /// </returns>
        private TaxonRelationList GetAllValidParentTaxonRelationsByTree(IUserContext userContext, Boolean onlyMainRelations)
        {
            TaxonRelationsTree tree = GetParentsTree(userContext);
            ITaxonRelationsTreeNode treeNode = tree.GetTreeNode(Id);
            List<ITaxonRelationsTreeEdge> allValidParentEdgesUpToRootNode =
                treeNode.GetAllValidParentEdgesTopToBottom(onlyMainRelations);
            TaxonRelationList taxonRelationList = allValidParentEdgesUpToRootNode.ToTaxonRelationList();
            return taxonRelationList;
        }

        /// <summary>
        /// Get all valid parent relations hierarchical when in revision mode.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="onlyMainRelations">
        /// if set to <c>true</c> only main relations are returned, otherwise both main and secondary relations are returned.
        /// </param>
        /// <returns>A list with taxon relations.</returns>
        private TaxonRelationList GetRevisionAllValidParentTaxonRelationsByTree(IUserContext userContext, Boolean onlyMainRelations)
        {
            Debug.Assert(this.IsUserInTaxonRevision(userContext), "this.IsUserInTaxonRevision(userContext)");

            TaxonRelationsTree tree = CreateRevisionValidParentsTree(userContext);
            ITaxonRelationsTreeNode treeNode = tree.GetTreeNode(Id);
            List<ITaxonRelationsTreeEdge> allValidParentEdgesUpToRootNode =
                treeNode.GetAllValidParentEdgesTopToBottom(onlyMainRelations);
            TaxonRelationList taxonRelationList = allValidParentEdgesUpToRootNode.ToTaxonRelationList();
            return taxonRelationList;
        }

        /// <summary>
        /// Creates a tree with all valid parents when in revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>A tree with all valid parents.</returns>
        private TaxonRelationsTree CreateRevisionValidParentsTree(IUserContext userContext)
        {
            Debug.Assert(this.IsUserInTaxonRevision(userContext), "this.IsUserInTaxonRevision(userContext)");

            TaxonRelationList replacedRelationsList = new TaxonRelationList();
            TaxonRelationList allValidParentTaxonRelations = new TaxonRelationList();

            TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = true;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            searchCriteria.Taxa = new TaxonList { this };
            TaxonRelationList relations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);            

            foreach (ITaxonRelation relation in relations)
            {
                // If in revision and relation is replaced by another relation - add the relation to the replaced relations list.
                if (relation.ReplacedInTaxonRevisionEventId.IsNotNull())
                {
                    replacedRelationsList.Add(relation);
                }
                else
                {
                    allValidParentTaxonRelations.Add(relation);
                }
            }

            if (replacedRelationsList.Count > 0)
            {
                int index;
                for (index = allValidParentTaxonRelations.Count - 1; index >= 0; index--)
                {
                    int taxonIdToCheck = allValidParentTaxonRelations[index].ChildTaxon.Id;
                    // om valid relationens child finns i listan av non-valid relationens parents
                    if (replacedRelationsList.Any(o => o.ParentTaxon.Id.Equals(taxonIdToCheck)))
                    {
                        // OCH om valid relationens child INTE finns med i listan av valid relationens parents
                        if (!allValidParentTaxonRelations.Any(o => o.ParentTaxon.Id.Equals(taxonIdToCheck)))
                        {
                            replacedRelationsList.Add(allValidParentTaxonRelations[index]);
                            allValidParentTaxonRelations.RemoveAt(index);
                        }
                    }
                }
            }
            // resort in taxon category order if user is in a revision. Parent sort order isn't set when creating new taxa.
            allValidParentTaxonRelations.SortTaxonCategory();

            TaxonRelationsTree taxonRelationsTree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(userContext, allValidParentTaxonRelations, new TaxonList { this });
            return taxonRelationsTree;
        }

        /// <summary>
        /// Get all parent taxon relations when valid is not required.
        /// </summary>
        /// <param name="userContext">The user context.</param>        
        /// <returns>All parent taxon relations.</returns>
        private TaxonRelationList GetAllParentTaxonRelationsWhenValidNotRequired(IUserContext userContext)
        {
            ITaxonRelationSearchCriteria searchCriteria;
            TaxonRelationList helperNonValidRelationList = new TaxonRelationList();

            if (_allValidParentTaxonRelations.IsNull() || this.IsUserInTaxonRevision(userContext))
            {
                this._allValidParentTaxonRelations = new TaxonRelationList();
                this._allNotValidParentTaxonRelations = new TaxonRelationList();
                searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.IsMainRelation = null;
                searchCriteria.IsValid = null;
                searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
                searchCriteria.Taxa = new TaxonList();
                searchCriteria.Taxa.Add(this);
                foreach (var relation in CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria).Distinct())
                {
                    if (relation.ValidToDate >= DateTime.Now)
                    {
                        // If in revision and relation is replaced by another relation - add the relation to the non-valid relations list
                        if (this.IsUserInTaxonRevision(userContext) && relation.ReplacedInTaxonRevisionEventId.IsNotNull())
                        {
                            if (!this._allNotValidParentTaxonRelations.Any(o => o.ParentTaxon.Id.Equals(relation.ParentTaxon.Id)))
                            {
                                this._allNotValidParentTaxonRelations.Add(relation);
                            }
                            helperNonValidRelationList.Add(relation);
                        }
                        else
                        {
                            this._allValidParentTaxonRelations.Add(relation);
                        }
                    }
                    else
                    {
                        if (this.IsUserInTaxonRevision(userContext))
                        {
                            helperNonValidRelationList.Add(relation);
                        }

                        // Save relations where ChildTaxa = this.taxa 
                        // ParentTaxa higher up in the hierarchy are NOT displayed.
                        if (relation.ChildTaxon.Equals(this))
                        {
                            if (!this._allNotValidParentTaxonRelations.Any(o => o.ParentTaxon.Id.Equals(relation.ParentTaxon.Id)))
                            {
                                this._allNotValidParentTaxonRelations.Add(relation);
                            }
                        }
                    }
                }

                if (this.IsUserInTaxonRevision(userContext))
                {
                    int index;
                    for (index = this._allValidParentTaxonRelations.Count - 1; index >= 0; index--)
                    {
                        int taxonIdToCheck = this._allValidParentTaxonRelations[index].ChildTaxon.Id;
                        // om valid relationens child finns i listan av non-valid relationens parents
                        if (helperNonValidRelationList.Any(o => o.ParentTaxon.Id.Equals(taxonIdToCheck)))
                        {
                            // OCH om valid relationens child INTE finns med i listan av valid relationens parents
                            if (!this._allValidParentTaxonRelations.Any(o => o.ParentTaxon.Id.Equals(taxonIdToCheck)))
                            {
                                helperNonValidRelationList.Add(this._allValidParentTaxonRelations[index]);
                                this._allValidParentTaxonRelations.RemoveAt(index);
                            }
                        }
                    }
                    // resort in taxon category order if user is in a revision. Parent sort order isn't set when creating new taxa.
                    this._allValidParentTaxonRelations.SortTaxonCategory();
                }
            }
            return _allValidParentTaxonRelations;
        }

        /// <summary>
        /// The get non valid parents.
        /// </summary>
        /// <returns>
        /// The <see cref="TaxonRelationList"/>.
        /// </returns>
        public TaxonRelationList GetNonValidParents()
        {
            return this._allNotValidParentTaxonRelations;
        }

        /// <summary>
        /// Get non valid parents.
        /// </summary>
        /// <returns>
        /// The <see cref="TaxonRelationList"/>.
        /// </returns>
        public TaxonRelationList GetNonValidParents(IUserContext userContext)
        {
            if (_nearestNotValidParentTaxonRelations == null)
            {
                ITaxonRelationSearchCriteria searchCriteria;
                searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.IsMainRelation = null;
                searchCriteria.IsValid = false;
                searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
                searchCriteria.Taxa = new TaxonList { this };
                _nearestNotValidParentTaxonRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
            }

            return _nearestNotValidParentTaxonRelations;
        }

        /// <summary>
        /// Get nearest non valid parents.
        /// </summary>
        /// <returns>
        /// The <see cref="TaxonRelationList"/>.
        /// </returns>
        public TaxonRelationList GetNearestNonValidParents(IUserContext userContext)
        {            
            // TaxonRelation.ValidToDate is set when the revision is checked in (in database sp: "RevisionCheckIn")
            // so there is not possible to see historical data when in revision mode.
            if (IsUserInTaxonRevision(userContext))
            {
                return new TaxonRelationList();
            }

            if (_nearestNotValidParentTaxonRelations == null)
            {
                ITaxonRelationSearchCriteria searchCriteria;
                searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.IsMainRelation = null;
                searchCriteria.IsValid = false;
                searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
                searchCriteria.Taxa = new TaxonList { this };
                _nearestNotValidParentTaxonRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
            }

            return _nearestNotValidParentTaxonRelations;

            //// Get by tree
            //TaxonRelationsTree tree = GetParentsTree(userContext);
            //TaxonRelationsTreeNode node = tree.GetTreeNode(Id);
            //TaxonRelationList nearestNotValidParents = new TaxonRelationList();
            //foreach (var parentRelation in node.NonvalidMainParents)
            //{
            //    nearestNotValidParents.Add(parentRelation.TaxonRelation);
            //}
            //foreach (var parentRelation in node.NonvalidSecondaryParents)
            //{
            //    nearestNotValidParents.Add(parentRelation.TaxonRelation);
            //}
        }

        /// <summary>
        /// Delivers all currently valid direct parent taxa for this taxon while in a revision
        /// </summary>
        public List<ITaxonRelation> GetCheckedOutChangesParentTaxa(IUserContext userContext)
        {
            return (from prop in GetNearestParentTaxonRelations(userContext) where ((!prop.ReplacedInTaxonRevisionEventId.HasValue) && prop.ValidToDate > DateTime.Now) orderby prop.SortOrder select prop).ToList();
        }

        /// <summary>
        /// Gets CheckedOutChangesTaxonName.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>CheckedOutChangesTaxonName.</returns>
        public IList<ITaxonName> GetCheckedOutChangesTaxonName(IUserContext userContext)
        {
            IEnumerable<ITaxonName> taxonNames;

            taxonNames = from prop in GetTaxonNames(userContext) where prop.GetReplacedInRevisionEvent(userContext) == null || prop.GetReplacedInRevisionEvent(userContext).Id == 0 select prop;
            if (taxonNames.Any())
            {
                return taxonNames.ToList();
            }
            else
            {
                return GetCurrentTaxonNames(userContext);
            }
        }

        /// <summary>
        /// Delivers the current valid TaxonProperty object for a taxon when in a revision
        /// </summary>
        public ITaxonProperties GetCheckedOutChangesTaxonProperties(IUserContext userContext)
        {
            var taxonProperties = (from prop in GetTaxonProperties(userContext) where (prop.ReplacedInTaxonRevisionEvent == null && prop.ValidToDate > DateTime.Now) select prop).ToList();

            if (taxonProperties.Count > 0)
            {
                return taxonProperties.Last();
            }
            else
            {
                return GetTaxonProperties(userContext).Last();
            }
        }

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
        public virtual ITaxonTreeNode GetChildTaxonTree(IUserContext userContext,
                                                        Boolean isValid)
        {
            ITaxonTreeNode taxonTreeNode;
            ITaxonTreeSearchCriteria searchCriteria;
            TaxonTreeNodeList taxonTrees;

            // Get child taxon tree.
            searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.IsValidRequired = isValid;
            searchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Id);
            taxonTrees = CoreData.TaxonManager.GetTaxonTrees(userContext,
                                                                                   searchCriteria);
            // Find taxon tree node for this taxon.
            if (taxonTrees.IsEmpty())
            {
                taxonTreeNode = new TaxonTreeNode();
                taxonTreeNode.Taxon = this;
            }
            else
            {
                // There should be only one root taxon tree which is
                // related to this taxon.
                taxonTreeNode = taxonTrees[0];
            }
            return taxonTreeNode;
        }

        /// <summary>
        /// Get recommended common name, ie name of type
        /// SWEDISH_NAME and recommended.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended common name.</returns>
        public ITaxonName GetCommonName(IUserContext userContext)
        {
            DateTime today;
            ITaxonName commonName;

            commonName = null;
            today = DateTime.Now;
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if (taxonName.IsRecommended &&
                    (taxonName.ValidFromDate <= today) &&
                    (today <= taxonName.ValidToDate) &&
                    (taxonName.Category.Id == (Int32)(TaxonNameCategoryId.SwedishName)))
                {
                    commonName = taxonName;
                    break;
                }
            }
            return commonName;
        }

        /// <summary>
        /// Full concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public String GetConceptDefinition(IUserContext userContext)
        {
            if (_conceptDefinition.IsNull())
            {
                _conceptDefinition = CoreData.TaxonManager.GetTaxonConceptDefinition(userContext, this);
            }
            return _conceptDefinition;
        }

        /// <summary>
        /// Get currently valid taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Currently valid taxon names.</returns>
        public IList<ITaxonName> GetCurrentTaxonNames(IUserContext userContext)
        {
            return (from taxonName in GetTaxonNames(userContext) where (taxonName.ValidToDate > DateTime.Now) && taxonName.IsPublished select taxonName).ToList();
        }

        /// <summary>
        /// Returns the hash code for this taxon.
        /// </summary>
        /// <returns>
        /// A Int32 containing the hash code for this taxon.
        /// </returns>
        public override int GetHashCode()
        {
            return Id;
        }

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
        public List<ITaxonName> GetIdentifiers(IUserContext userContext)
        {
            List<ITaxonName> identifiers;

            identifiers = new List<ITaxonName>();
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if ((taxonName.Category.Type.Id == (Int32)(TaxonNameCategoryTypeId.Identifier)) &&
                    !((taxonName.Category.Id == (Int32)(TaxonNameCategoryId.Guid)) &&
                      taxonName.IsRecommended))
                {
                    identifiers.Add(taxonName);
                }
            }

            return identifiers;
        }

        /// <summary>
        /// Get the full name of the person that made the last modification to this taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// The person that made the last modification to this taxon.
        /// If the person dont exists in the User Admin, an empty string will be retuned.
        /// </returns>
        public string GetModifiedByPersonFullname(IUserContext userContext)
        {
            if (this.ModifiedBy.IsNotNull())
            {
                if (this.ModifiedBy != 0)
                {
                    IPerson person;
                    try
                    {
                        person = CoreData.UserManager.GetPerson(userContext, this.ModifiedBy);
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }

                    if (person.IsNotNull())
                    {
                        return person.FullName;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the person that made the last modification to this taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// The person that made the last modification to this taxon.
        /// If the person dont exists in the User Admin, an empty string will be retuned.
        /// </returns>
        public IPerson GetModifiedByPerson(IUserContext userContext)
        {
            IPerson person;
            try
            {
                person = CoreData.UserManager.GetPerson(userContext, this.ModifiedBy);
            }
            catch (Exception)
            {
                return null;
            }

            return person;
        }

        /// <summary>
        /// Get the full name of the person that created this taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// The fullname of the person that created this taxon.
        /// If the person dont exists in the User Admin, an empty string will be retuned.
        /// </returns>
        public string GetCreatedByPersonFullName(IUserContext userContext)
        {
            string person = string.Empty;
            IUser user;

            try
            {
                user = CoreData.UserManager.GetUser(userContext, this.CreatedBy);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            if (user.IsNotNull() && user.Type == UserType.Person && user.PersonId.HasValue)
            {
                person = CoreData.UserManager.GetPerson(userContext, user.PersonId.Value).FullName;
            }

            return person;
        }

        /// <summary>
        /// Get nearest child taxon relations that are valid.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Nearest child taxon relations that are valid.</returns>
        public virtual TaxonRelationList GetNearestChildTaxonRelations(IUserContext userContext)
        {
            ITaxonRelationSearchCriteria searchCriteria;

            if (_nearestChildTaxonRelations.IsNull() ||
                IsUserInTaxonRevision(userContext))
            {
                searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.IsMainRelation = null;
                searchCriteria.IsValid = true;
                searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
                searchCriteria.Taxa = new TaxonList();
                searchCriteria.Taxa.Add(this);
                _nearestChildTaxonRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
            }
            return _nearestChildTaxonRelations;
        }

        /// <summary>
        /// Get nearest parent taxon relations that are valid.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Nearest parent taxon relations that are valid.</returns>
        public virtual TaxonRelationList GetNearestParentTaxonRelations(IUserContext userContext)
        {
            ITaxonRelationSearchCriteria searchCriteria;

            if (_nearestParentTaxonRelations.IsNull() ||
                IsUserInTaxonRevision(userContext))
            {
                searchCriteria = new TaxonRelationSearchCriteria();
                searchCriteria.IsMainRelation = null;
                searchCriteria.IsValid = true;
                searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
                searchCriteria.Taxa = new TaxonList();
                searchCriteria.Taxa.Add(this);
                _nearestParentTaxonRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
            }
            return _nearestParentTaxonRelations;
        }

        /// <summary>
        /// Searches among the direct parents in the collection ParentTaxa
        /// </summary>
        public IList<ITaxonRelation> GetParentTaxonRelations(IUserContext userContext, bool isTaxonRevisionEditor, bool includeHistorical, bool isMainRelation = false)
        {
            // TaxonRevisionEditor == true (in revision - return published data + data that is changed in the revision)
            if (isTaxonRevisionEditor)
            {
                if (includeHistorical)
                {
                    return (from taxonRelation in this.GetNearestParentTaxonRelations(userContext)
                            where (taxonRelation.IsMainRelation == true || isMainRelation == false)
                            select taxonRelation).ToList();
                }
                // includeHistorical == false
                else
                {
                    return (from taxonRelation in this.GetNearestParentTaxonRelations(userContext)
                            where ((taxonRelation.ValidToDate > DateTime.Now && (!taxonRelation.ReplacedInTaxonRevisionEventId.HasValue)) &&
                                   (taxonRelation.IsMainRelation == true || isMainRelation == false))
                            select taxonRelation).ToList();
                }
            }
            // TaxonRevisionEditor == false (NOT in revision - return public data)
            else
            {
                if (includeHistorical)
                {
                    return (from taxonRelation in this.GetNearestParentTaxonRelations(userContext)
                            where (taxonRelation.IsPublished == true &&
                                  (taxonRelation.IsMainRelation == true || isMainRelation == false))
                            select taxonRelation).ToList();
                }
                // includeHistorical = false
                else
                {
                    return (from taxonRelation in this.GetNearestParentTaxonRelations(userContext)
                            where ((taxonRelation.ValidToDate > DateTime.Now && taxonRelation.IsPublished == true) &&
                                  (taxonRelation.IsMainRelation == true || isMainRelation == false))
                            select taxonRelation).ToList();
                }
            }
        }

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
        public virtual ITaxonTreeNode GetParentTaxonTree(IUserContext userContext,
                                                         Boolean isValid)
        {
            ITaxonTreeNode taxonTreeNode;
            ITaxonTreeSearchCriteria searchCriteria;
            TaxonTreeNodeList taxonTrees;

            // Get parent taxon trees.
            searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.IsValidRequired = isValid;
            searchCriteria.Scope = TaxonTreeSearchScope.AllParentTaxa;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Id);
            taxonTrees = CoreData.TaxonManager.GetTaxonTrees(userContext,
                                                                             searchCriteria);

            // Find taxon tree node for this taxon.
            if (taxonTrees.IsEmpty())
            {
                taxonTreeNode = new TaxonTreeNode();
                taxonTreeNode.Taxon = this;
            }
            else
            {
                // The taxon tree node for this taxon should be at
                // the bottom (leaf) of tree.
                taxonTreeNode = taxonTrees[0];
                while (taxonTreeNode.Taxon.Id != Id)
                {
                    taxonTreeNode = taxonTreeNode.Children[0];
                }
            }
            return taxonTreeNode;
        }

        /// <summary>
        /// Get recommended GUID, ie name of type GUID and recommended.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended GUID.</returns>
        public String GetRecommendedGuid(IUserContext userContext)
        {
            DateTime today;
            String recommendedGuid;

            recommendedGuid = null;
            today = DateTime.Now;
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if ((taxonName.Category.Id == (Int32)(TaxonNameCategoryId.Guid)) &&
                    taxonName.IsRecommended &&
                    (taxonName.ValidFromDate <= today) &&
                    (today <= taxonName.ValidToDate))
                {
                    recommendedGuid = taxonName.Name;
                    break;
                }
            }

            // If no recommended GUID was found among the taxonnames - use taxon GUID.
            if (recommendedGuid.IsNull())
            {
                recommendedGuid = Guid;
            }
            return recommendedGuid;
        }

        /// <summary>
        /// Gets or sets References.
        /// </summary>
        public ReferenceRelationList GetReferenceRelations(IUserContext userContext)
        {
            if (_referenceRelations.IsNull() && Guid.IsNotEmpty())
            {
                _referenceRelations = CoreData.ReferenceManager.GetReferenceRelations(userContext, Guid);
            }
            return _referenceRelations;
        }

        /// <summary>
        /// Get recommended scientfic name, ie name of type
        /// SCIENTIFIC_NAME and recommended.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Recommended scientfic name.</returns>
        public ITaxonName GetScientificName(IUserContext userContext)
        {
            DateTime today;
            ITaxonName scientificName;

            scientificName = null;
            today = DateTime.Now;
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if (taxonName.IsRecommended &&
                    (taxonName.ValidFromDate <= today) &&
                    (today <= taxonName.ValidToDate) &&
                    (taxonName.Category.Id == (Int32)(TaxonNameCategoryId.ScientificName)))
                {
                    scientificName = taxonName;
                    break;
                }
            }
            return scientificName;
        }

        /// <summary>
        /// Get scientific name and author for this taxon.
        /// </summary>
        /// <returns>Scientific name and author for this taxon.</returns>       
        public virtual String GetScientificNameAndAuthor()
        {
            String scientificNameAndAuthor;

            if (Author.IsEmpty())
            {
                scientificNameAndAuthor = ScientificName;
            }
            else
            {
                scientificNameAndAuthor = ScientificName + " " + Author;
            }

            return scientificNameAndAuthor.Trim();
        }

        /// <summary>
        /// Get scientific name, author and common name for this taxon.
        /// </summary>
        /// <returns>Scientific name, author and common name for this taxon.</returns>       
        public virtual String GetScientificNameAndAuthorAndCommonName()
        {
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            stringBuilder.Append(ScientificName);
            if (Author.IsNotEmpty())
            {
                stringBuilder.Append(" " + Author);
            }

            if (CommonName.IsNotEmpty())
            {
                stringBuilder.Append(", " + CommonName);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get all synonyms. proParte synonyms are included in the result.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>        
        /// <returns>
        /// Synonyms.
        /// </returns>
        public List<ITaxonName> GetSynonyms(IUserContext userContext)
        {
            return GetSynonyms(userContext, true);
        }

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
        public List<ITaxonName> GetSynonyms(IUserContext userContext, bool includeProParteSynonyms)
        {
            DateTime today;
            List<ITaxonName> synonyms;

            synonyms = new List<ITaxonName>();
            today = DateTime.Now;
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {                
                if (!(taxonName.ValidFromDate <= today && today <= taxonName.ValidToDate)) 
                {
                    continue;
                }

                if (taxonName.Category.Type.Id == (int)TaxonNameCategoryTypeId.Identifier)
                {
                    continue;
                }

                if (taxonName.Status.Id == (int) TaxonNameStatusId.Removed)
                {
                    continue;
                }

                List<int> nameUsageSynonymIds = new List<int>(4)
                {
                    (int)TaxonNameUsageId.Synonym,
                    (int)TaxonNameUsageId.Heterotypic,
                    (int)TaxonNameUsageId.Homotypic
                };
                if (includeProParteSynonyms)
                {
                    nameUsageSynonymIds.Add((int)TaxonNameUsageId.ProParteSynonym);
                }

                // All names that are accepted but not recommended are synonyms
                // Accepterade namn som inte är rekommenderade
                if (taxonName.NameUsage.Id == (int)TaxonNameUsageId.Accepted
                 && taxonName.IsRecommended == false)                       
                {                                
                    synonyms.Add(taxonName);
                }
                else if (nameUsageSynonymIds.Any(x => x == taxonName.NameUsage.Id))
                {
                    synonyms.Add(taxonName);
                }                           
            }

            return synonyms;
        }
    
        /// <summary>
        /// Get proParte synonyms.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// proParte synonyms.
        /// </returns>
        public List<ITaxonName> GetProParteSynonyms(IUserContext userContext)
        {
            DateTime today;
            List<ITaxonName> proParteSynonyms;

            proParteSynonyms = new List<ITaxonName>();
            today = DateTime.Now;
            
            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {                
                if (taxonName.NameUsage.Id == (int)TaxonNameUsageId.ProParteSynonym                  
                 && taxonName.ValidFromDate <= today 
                 && today <= taxonName.ValidToDate
                 && taxonName.Status.Id != (int)TaxonNameStatusId.Removed)
                {
                    proParteSynonyms.Add(taxonName);
                }
            }
                        
            return proParteSynonyms;
        }

        /// <summary>
        /// Get misapplied names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Misapplied names.
        /// </returns>
        public List<ITaxonName> GetMisappliedNames(IUserContext userContext)
        {
            DateTime today;
            List<ITaxonName> misappliedNames;

            misappliedNames = new List<ITaxonName>();
            today = DateTime.Now;

            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if (taxonName.Category.Type.Id == (int)TaxonNameCategoryTypeId.Identifier)
                {
                    continue;
                }

                if (taxonName.NameUsage.Id == (int)TaxonNameUsageId.MisappliedAuctName                    
                    && taxonName.ValidFromDate <= today 
                    && today <= taxonName.ValidToDate
                    && taxonName.Status.Id != (int)TaxonNameStatusId.Removed)
                {
                    misappliedNames.Add(taxonName);
                }
            }

            return misappliedNames;
        }


        /// <summary>
        /// Get taxon name with specified version.       
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameVersion">Taxon name version.</param>
        /// <returns>Taxon name with specified version.</returns>
        public virtual ITaxonName GetTaxonNameByVersion(IUserContext userContext,
                                                        Int32 taxonNameVersion)
        {
            return GetTaxonNames(userContext).GetByVersion(taxonNameVersion);
        }


        /// <summary>
        /// Get all taxon names in a specific category.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="categoryId">Category id.</param>
        /// <returns>
        /// All taxon names in a specific category.
        /// </returns>
        public List<ITaxonName> GetTaxonNamesByCategoryId(IUserContext userContext, Int32 categoryId)
        {
            DateTime today;
            List<ITaxonName> names;

            names = new List<ITaxonName>();
            today = DateTime.Now;

            foreach (ITaxonName taxonName in GetTaxonNames(userContext))
            {
                if (taxonName.Category.Id == categoryId
                    && taxonName.ValidFromDate <= today
                    && today <= taxonName.ValidToDate)
                {
                    names.Add(taxonName);
                }
            }

            return names;
        }        

        /// <summary>
        /// Get all taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All taxon names.</returns>
        public TaxonNameList GetTaxonNames(IUserContext userContext)
        {
            if (_taxonNameList == null)
            {
                _taxonNameList = CoreData.TaxonManager.GetTaxonNames(userContext, this);
            }
            return _taxonNameList;
        }
        
        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameCategoryId">Taxon name category id.</param>
        /// <param name="taxonNameCategoryTypeId">Taxon name category type id.</param>
        /// <param name="taxonNameStatusId">Taxon name status id.</param>
        /// <param name="isRecommended">Is recommended.</param>
        /// <param name="isTaxonRevisionEditor">Is taxon revision editor.</param>
        /// <param name="includeHistorical">Include historical.</param>
        /// <returns>taxon names that matches search criteria.</returns>
        public IList<ITaxonName> GetTaxonNamesBySearchCriteria(IUserContext userContext,
                                                               Int32? taxonNameCategoryId,
                                                               Int32? taxonNameCategoryTypeId,
                                                               Int32? taxonNameStatusId,
                                                               Boolean? isRecommended,
                                                               Boolean isTaxonRevisionEditor,
                                                               Boolean includeHistorical)
        {
            return (from taxonName in GetTaxonNames(userContext)
                    where
                        (taxonName.Category.Id == taxonNameCategoryId || taxonNameCategoryId.IsNull()) &&
                        (taxonName.Status.Id == taxonNameStatusId || taxonNameStatusId.IsNull()) &&
                        (taxonName.IsRecommended == isRecommended || isRecommended.IsNull()) &&
                        (taxonName.Category.Type.Id == taxonNameCategoryTypeId || taxonNameCategoryTypeId.IsNull()) &&
                        (taxonName.ValidToDate > DateTime.Now || includeHistorical) &&
                        (taxonName.IsPublished || isTaxonRevisionEditor)
                    select taxonName).ToList();
        }

        /// <summary>
        /// Gets TaxonPropertieses.
        /// </summary>
        public List<ITaxonProperties> GetTaxonProperties(IUserContext userContext)
        {
            if (_taxonPropertieses == null)
            {
                _taxonPropertieses = new List<ITaxonProperties>();
                _taxonPropertieses.AddRange(CoreData.TaxonManager.GetTaxonProperties(userContext, this));
            }
            return _taxonPropertieses;
        }

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
        public virtual ITaxonTreeNode GetTaxonTree(IUserContext userContext,
                                                   Boolean isValid)
        {
            ITaxonTreeNode parentTaxonTree, childTaxonTree, taxonTreeNode;
            TaxonTreeNodeList parents;

            // Get parent and child taxon tree.
            childTaxonTree = GetChildTaxonTree(userContext, isValid);
            parentTaxonTree = GetParentTaxonTree(userContext, isValid);

            // Create taxon tree node for this taxon.
            taxonTreeNode = null;
            if (childTaxonTree.IsNotNull() && parentTaxonTree.IsNotNull())
            {
                taxonTreeNode = parentTaxonTree;
                taxonTreeNode.Children = childTaxonTree.Children;
                if (taxonTreeNode.Children.IsNotEmpty())
                {
                    parents = new TaxonTreeNodeList();
                    parents.Add(taxonTreeNode);
                    foreach (ITaxonTreeNode childTaxon in taxonTreeNode.Children)
                    {
                        childTaxon.Parents = parents;
                    }
                }
            }
            else
            {
                if (childTaxonTree.IsNotNull())
                {
                    taxonTreeNode = childTaxonTree;
                }
                if (parentTaxonTree.IsNotNull())
                {
                    taxonTreeNode = parentTaxonTree;
                }
            }
            return taxonTreeNode;
        }

        /// <summary>
        /// Test if user is working in a taxon revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>True, if user is working in a taxon revision.</returns>
        private Boolean IsUserInTaxonRevision(IUserContext userContext)
        {
            return (userContext.CurrentRole.IsNotNull() &&
                    userContext.CurrentRole.Identifier.IsNotEmpty() &&
                    userContext.CurrentRole.Identifier.StartsWith(Settings.Default.DyntaxaRevisionRoleIdentifier));
        }

        /// <summary>
        /// Sets parent taxa.
        /// </summary>
        /// <param name="parentTaxa">The parent taxa.</param>
        public void SetParentTaxa(TaxonRelationList parentTaxa)
        {
            _nearestParentTaxonRelations = parentTaxa;
        }

        /// <summary>
        /// Sets TaxonPropertieses.
        /// </summary>
        /// <param name="properties"></param>
        public void SetTaxonProperties(List<ITaxonProperties> properties)
        {
            _taxonPropertieses = properties;
        }
    }
}
