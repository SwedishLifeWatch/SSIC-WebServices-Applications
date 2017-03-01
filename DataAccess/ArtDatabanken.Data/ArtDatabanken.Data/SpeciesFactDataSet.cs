using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles a collection of
    /// logically coherent species facts.
    /// Scope of the data set is defined by a
    /// species fact data set selection.
    /// Dependent factors are added to the scope.
    /// Empty species facts are added if no exists in the
    /// database for the specified species fact data set selection.
    /// Automatic species facts are calculated.
    /// At least one factor and one taxon must be specified
    /// in the species fact data set selection.
    /// This class is currently not thread safe.
    /// </summary>
    public class SpeciesFactDataSet : ISpeciesFactDataSet
    {
        private ISpeciesFactDataSetSelection _selection;
        private ISpeciesFactDataSetSelection _selectionCopy;

        /// <summary>
        /// Create a species fact data set instance.
        /// </summary>
        public SpeciesFactDataSet()
        {
            Factors = new FactorList(true);
            Hosts = new TaxonList(true);
            IndividualCategories = new IndividualCategoryList(true);
            Periods = new PeriodList(true);
            References = new ReferenceList(true);
            _selection = new SpeciesFactDataSetSelection();
            _selectionCopy = new SpeciesFactDataSetSelection();
            SpeciesFacts = new SpeciesFactList();
            Taxa = new TaxonList(true);
        }
        
        /// <summary>
        /// Factors in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public FactorList Factors { get; private set; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any factors.
        /// </summary>
        public Boolean HasFactors
        {
            get
            {
                return Factors.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any individual categories.
        /// </summary>
        public Boolean HasIndividualCategories
        {
            get
            {
                return IndividualCategories.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any hosts.
        /// </summary>
        public Boolean HasHosts
        {
            get
            {
                return Hosts.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any periods.
        /// </summary>
        public Boolean HasPeriods
        {
            get
            {
                return Periods.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any references.
        /// </summary>
        public Boolean HasReferences
        {
            get
            {
                return References.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any species facts.
        /// </summary>
        public Boolean HasSpeciesFacts
        {
            get
            {
                return SpeciesFacts.IsNotEmpty();
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any taxa.
        /// </summary>
        public Boolean HasTaxa
        {
            get
            {
                return Taxa.IsNotEmpty();
            }
        }

        /// <summary>
        /// Hosts in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Hosts { get; private set; }

        /// <summary>
        /// Individual categories in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public IndividualCategoryList IndividualCategories { get; private set; }

        /// <summary>
        /// Periods in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public PeriodList Periods { get; private set; }

        /// <summary>
        /// References in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public ReferenceList References { get; private set; }

        /// <summary>
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// The data set is initiated or redefined by
        /// setting this property
        /// At least one dimension in the species fact
        /// data set selection must be specified.
        /// </summary>
        public ISpeciesFactDataSetSelection Selection
        {
            get
            {
                return _selectionCopy;
            }
        }

        /// <summary>
        /// List of species facts corresponding to all combinations
        /// of taxa, individual categories, factors, hosts and periods
        /// listed in the selection in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public SpeciesFactList SpeciesFacts { get; private set; }

        /// <summary>
        /// Taxa in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Taxa { get; private set; }

        /// <summary>
        /// Add factors to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         FactorList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Factors.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add factor to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         IFactor selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Factors.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add individual category to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         IIndividualCategory selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.IndividualCategories.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add individual categories to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         IndividualCategoryList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.IndividualCategories.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add period to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         IPeriod selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Periods.Merge(selection);
            AddSelection(userContext, newSelection);
        }
        /// <summary>
        /// Add reference to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="selection"></param>
        public virtual void AddSelection(IUserContext userContext,
                                       IReference selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.References.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add factors, hosts, individual categories, periods or
        /// taxa to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         ISpeciesFactDataSetSelection selection)
        {
            // Check arguments.
            selection.CheckNotNull("selection");

            // Update selection with existing scope.
            selection.Factors.Merge(_selection.Factors);
            selection.Hosts.Merge(_selection.Hosts);
            selection.IndividualCategories.Merge(_selection.IndividualCategories);
            selection.Periods.Merge(_selection.Periods);
            selection.References.Merge(_selection.References);
            selection.Taxa.Merge(_selection.Taxa);
            
            // Update species fact data set.
            UpdateSelection(userContext, selection);
        }

        /// <summary>
        /// Add periods to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelection(IUserContext userContext,
                                         PeriodList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Periods.Merge(selection);
            AddSelection(userContext, newSelection);
        }
        /// <summary>
        /// Add references to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="selection"></param>
        public virtual void AddSelection(IUserContext userContext, ReferenceList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.References.Merge(selection);
            AddSelection(userContext, newSelection);
        }


        /// <summary>
        /// Add host to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelectionHost(IUserContext userContext,
                                             ITaxon selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Hosts.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add hosts to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelectionHosts(IUserContext userContext,
                                              TaxonList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Hosts.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add taxa to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelectionTaxa(IUserContext userContext,
                                             TaxonList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Taxa.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Add taxon to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void AddSelectionTaxon(IUserContext userContext,
                                              ITaxon selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Taxa.Merge(selection);
            AddSelection(userContext, newSelection);
        }

        /// <summary>
        /// Make automated calculations of species facts
        /// that are "automatic" in a species fact list. 
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        public virtual void InitAutomatedCalculations(IUserContext userContext)
        {
            if (SpeciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in SpeciesFacts)
                {
                    switch (speciesFact.Factor.Id)
                    {
                        case (Int32)(FactorId.RedListCategoryAutomatic):
                            ((SpeciesFactRedListCategory)(speciesFact)).Init(userContext, SpeciesFacts);
                            break;
                        case (Int32)(FactorId.RedListCriteriaAutomatic):
                            ((SpeciesFactRedListCriteria)(speciesFact)).Init(userContext, SpeciesFacts);
                            break;
                        case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                            ((SpeciesFactRedListCriteriaDocumentation)(speciesFact)).Init(userContext, SpeciesFacts);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Merge old and new species facts to same list.
        /// </summary>
        /// <param name="speciesFacts">New species facts.</param>
        private void MergeSpeciesFacts(SpeciesFactList speciesFacts)
        {
            if (speciesFacts.IsEmpty())
            {
                return;
            }

            if (SpeciesFacts.IsEmpty())
            {
                SpeciesFacts = speciesFacts;
                return;
            }

            foreach (ISpeciesFact speciesFact in speciesFacts)
            {
                if (!SpeciesFacts.Exists(speciesFact.Identifier))
                {
                    // Add new species fact to old species facts.
                    SpeciesFacts.Add(speciesFact);
                }
            }
        }

        /// <summary>
        /// Remove factors from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            FactorList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Factors.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove factor from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            IFactor selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Factors.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove individual category from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            IIndividualCategory selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.IndividualCategories.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove individual categories from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            IndividualCategoryList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.IndividualCategories.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove period from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            IPeriod selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Periods.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        public virtual void RemoveSelection(IUserContext userContext,
                                           IReference selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.References.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }
        /// <summary>
        /// Remove factors, hosts, individual categories, periods or
        /// taxa from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            ISpeciesFactDataSetSelection selection)
        {
            FactorList factors;
            IndividualCategoryList individualCategories;
            PeriodList periods;
            ReferenceList references;
            TaxonList hosts, taxa;

            // Check arguments.
            selection.CheckNotNull("selection");

            // Update selection with existing scope.
            factors = new FactorList();
            factors.Merge(_selection.Factors);
            factors.Remove(selection.Factors);
            selection.Factors = factors;

            hosts = new TaxonList();
            hosts.Merge(_selection.Hosts);
            hosts.Remove(selection.Hosts);
            selection.Hosts = hosts;

            individualCategories = new IndividualCategoryList();
            individualCategories.Merge(_selection.IndividualCategories);
            individualCategories.Remove(selection.IndividualCategories);
            selection.IndividualCategories = individualCategories;

            periods = new PeriodList();
            periods.Merge(_selection.Periods);
            periods.Remove(selection.Periods);
            selection.Periods = periods;

            references = new ReferenceList();
            references.Merge(_selection.References);
            references.Remove(selection.References);
            selection.References = references;

            taxa = new TaxonList();
            taxa.Merge(_selection.Taxa);
            taxa.Remove(selection.Taxa);
            selection.Taxa = taxa;

            // Update species fact data set.
            UpdateSelection(userContext, selection);
        }

        /// <summary>
        /// Remove periods from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelection(IUserContext userContext,
                                            PeriodList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Periods.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        public virtual void RemoveSelection(IUserContext userContext,
                                            ReferenceList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.References.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove host from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelectionHost(IUserContext userContext,
                                                ITaxon selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Hosts.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove hosts from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelectionHosts(IUserContext userContext,
                                                 TaxonList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Hosts.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove taxa from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelectionTaxa(IUserContext userContext,
                                                TaxonList selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Taxa.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove taxon from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        public virtual void RemoveSelectionTaxon(IUserContext userContext,
                                                 ITaxon selection)
        {
            ISpeciesFactDataSetSelection newSelection;

            newSelection = new SpeciesFactDataSetSelection();
            newSelection.Taxa.Merge(selection);
            RemoveSelection(userContext, newSelection);
        }

        /// <summary>
        /// Remove old species facts that are no longer
        /// included in the species fact scope.
        /// </summary>
        private void RemoveSpeciesFactsNotInScope()
        {
            Int32 index;
            ISpeciesFact speciesFact;

            if (SpeciesFacts.IsNotEmpty())
            {
                for (index = SpeciesFacts.Count - 1; index >= 0; index--)
                {
                    speciesFact = SpeciesFacts[index];
                    if ((!Factors.Contains(speciesFact.Factor)) ||
                        (!IndividualCategories.Contains(speciesFact.IndividualCategory)) ||
                        (!Taxa.Contains(speciesFact.Taxon)) ||
                        (speciesFact.HasHost && !Hosts.Contains(speciesFact.Host)) ||
                        (speciesFact.HasPeriod && !Periods.Contains(speciesFact.Period)))
                    {
                        // This species fact is no longer
                        // included in the species fact scope.
                        SpeciesFacts.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Reset species fact data set to no species facts.
        /// This method is used when species fact data set selection
        /// does not contain both factors and taxa.
        /// </summary>
        /// <param name="selection">
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// </param>
        private void Reset(ISpeciesFactDataSetSelection selection)
        {
            Factors = new FactorList(true);
            Factors.Merge(selection.Factors);
            Hosts = new TaxonList(true);
            Hosts.Merge(selection.Hosts);
            IndividualCategories = new IndividualCategoryList(true);
            IndividualCategories.Merge(selection.IndividualCategories);
            Periods = new PeriodList(true);
            Periods.Merge(selection.Periods);
            References = new ReferenceList(true);
            References.Merge(selection.References);
            SpeciesFacts = new SpeciesFactList(true);
            Taxa = new TaxonList();
            Taxa.Merge(selection.Taxa);

            // Save selection information.
            _selection = (ISpeciesFactDataSetSelection)(selection.Clone());
            _selectionCopy = (ISpeciesFactDataSetSelection)(selection.Clone());
        }

        /// <summary>
        /// Update information about which factors, hosts,
        /// individual categories, periods, references and taxa
        /// that are used in the species facts.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// </param>
        /// <param name="speciesFacts">Species facts.</param>
        private void UpdateScope(IUserContext userContext,
                                 ISpeciesFactDataSetSelection selection,
                                 SpeciesFactList speciesFacts)
        {
            Factors = new FactorList(true);
            Factors.AddRange(selection.Factors);
            Hosts = new TaxonList(true);
            Hosts.AddRange(selection.Hosts);
            IndividualCategories = new IndividualCategoryList(true);
            IndividualCategories.AddRange(selection.IndividualCategories);
            Periods = new PeriodList(true);
            Periods.AddRange(selection.Periods);
            References = new ReferenceList(true);
            Taxa = new TaxonList(true);
            Taxa.AddRange(selection.Taxa);

            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    Factors.Merge(speciesFact.Factor);
                    if (speciesFact.HasHost)
                    {
                        Hosts.Merge(speciesFact.Host);
                    }

                    IndividualCategories.Merge(speciesFact.IndividualCategory);
                    if (speciesFact.HasPeriod)
                    {
                        Periods.Merge(speciesFact.Period);
                    }

                    if (speciesFact.HasReference)
                    {
                        References.Merge(speciesFact.Reference);
                    }

                    Taxa.Merge(speciesFact.Taxon);
                }
            }

            // Set default values if no values are entered.
            if (Hosts.IsEmpty())
            {
                Hosts.Add(CoreData.TaxonManager.GetTaxon(userContext, TaxonId.Life));
            }

            if (IndividualCategories.IsEmpty())
            {
                IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
            }

            if (Periods.IsEmpty())
            {
                Periods.AddRange(CoreData.FactorManager.GetPeriods(userContext));
            }

            // Sort all lists.
            Factors.Sort();
            Hosts.Sort();
            IndividualCategories.Sort();
            Periods.Sort();
            References.Sort();
            Taxa.Sort();
        }

        /// <summary>
        /// The data set is initiated or redefined by
        /// using this method.
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// At least one factor and one taxon must be specified
        /// in the species fact data set selection.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// </param>
        public virtual void UpdateSelection(IUserContext userContext,
                                            ISpeciesFactDataSetSelection selection)
        {
            IFactor factor;
            Int32 factorIndex;
            ISpeciesFactDataSetSelection workingSelection;
            ISpeciesFactSearchCriteria searchCriteria;
            SpeciesFactList speciesFacts;

            // Check arguments.
            selection.CheckNotNull("selection");
            if (selection.Factors.IsEmpty() || selection.Taxa.IsEmpty())
            {
                // Reset species fact data set and end method.
                // Conditions to retrieve species facts are not fulfilled.
                Reset(selection);
                return;
            }

            // Add dependent factors to selection if necessary.
            workingSelection = (ISpeciesFactDataSetSelection)(selection.Clone());
            for (factorIndex = 0; factorIndex < workingSelection.Factors.Count; factorIndex++)
            {
                factor = workingSelection.Factors[factorIndex];
                workingSelection.Factors.Merge(factor.GetDependentFactors(userContext));
            }

            // Get species facts.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = workingSelection.Factors;
            searchCriteria.Hosts = workingSelection.Hosts;
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.IndividualCategories = workingSelection.IndividualCategories;
            searchCriteria.Periods = workingSelection.Periods;
            searchCriteria.References = workingSelection.References;
            searchCriteria.Taxa = workingSelection.Taxa;
            speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, searchCriteria);

            // Update species fact scope with new information.
            UpdateScope(userContext, workingSelection, speciesFacts);

            // Remove old species facts that are no longer
            // included in the species fact scope.
            RemoveSpeciesFactsNotInScope();

            // Merge old and new species facts to same list.
            MergeSpeciesFacts(speciesFacts);

            // Get missing species facts according to expanded
            // combinations of species fact data set selection.
            UpdateWithEmptySpeciesFacts(userContext);

            // Init automated calculation.
            InitAutomatedCalculations(userContext);

            // Save selection information.
            _selection = (ISpeciesFactDataSetSelection)(selection.Clone());
            _selectionCopy = (ISpeciesFactDataSetSelection)(selection.Clone());
        }

        /// <summary>
        /// Expand a species fact list with empty species facts so
        /// that every combination from the species fact data set
        /// selection is represented.
        /// Factors of type header are excluded.
        /// Periodic factors are not expanded to
        /// individual categories other than the default. 
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        private void UpdateWithEmptySpeciesFacts(IUserContext userContext)
        {
            foreach (IFactor factor in Factors)
            {
                if (factor.UpdateMode.IsHeader)
                {
                    // Don't create SpeicesFacts for 'Headers'.
                    continue;
                }

                foreach (IIndividualCategory individualCategory in IndividualCategories)
                {
                    if (factor.IsPeriodic &&
                        (individualCategory.Id != ((Int32)IndividualCategoryId.Default)))
                    {
                        // Periodic factors should only be combined
                        // with default IndividualCategory.
                        continue;
                    }

                    foreach (ITaxon taxon in Taxa)
                    {
                        if (factor.IsPeriodic)
                        {
                            // Factor is periodic
                            foreach (IPeriod period in Periods)
                            {
                                if (factor.IsTaxonomic)
                                {
                                    foreach (ITaxon host in Hosts)
                                    {
                                        SpeciesFacts.Merge(userContext, taxon, individualCategory, factor, host, period);
                                    }
                                }
                                else
                                {
                                    SpeciesFacts.Merge(userContext, taxon, individualCategory, factor, null, period);
                                }
                            }
                            // End factor is periodic
                        }
                        else
                        {
                            // Factor is not periodic
                            if (factor.IsTaxonomic)
                            {
                                foreach (ITaxon host in Hosts)
                                {
                                    SpeciesFacts.Merge(userContext, taxon, individualCategory, factor, host, null);
                                }
                            }
                            else
                            {
                                SpeciesFacts.Merge(userContext, taxon, individualCategory, factor, null, null);
                            }
                            // End factor is not periodic
                        }
                    }
                }
            }
        }
    }
}
