using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles a collection of
    /// logically coherent species facts.
    /// Scope of the data set is defined by a
    /// species fact data set selection.
    /// Dependent factors are added to the scope.
    /// Empty species facts are added if no exists in the
    /// database for the specified species fact data set selection.
    /// Automatic species facts are calculated.
    /// At least one factor and one taxon must be specified
    /// in the species fact data set selection.
    /// </summary>
    public interface ISpeciesFactDataSet
    {
        /// <summary>
        /// Factors in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        FactorList Factors { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any factors.
        /// </summary>
        Boolean HasFactors { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any individual categories.
        /// </summary>
        Boolean HasIndividualCategories { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any hosts.
        /// </summary>
        Boolean HasHosts { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any periods.
        /// </summary>
        Boolean HasPeriods { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any references.
        /// </summary>
        Boolean HasReferences { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any species facts.
        /// </summary>
        Boolean HasSpeciesFacts { get; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// contains any taxa.
        /// </summary>
        Boolean HasTaxa { get; }

        /// <summary>
        /// Hosts in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        TaxonList Hosts { get; }

        /// <summary>
        /// Individual categories in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        IndividualCategoryList IndividualCategories { get; }

        /// <summary>
        /// Periods in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        PeriodList Periods { get; }

        /// <summary>
        /// References in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        ReferenceList References { get; }

        /// <summary>
        /// Scope of the data set is defined by this
        /// species fact data set selection.
        /// </summary>
        ISpeciesFactDataSetSelection Selection { get; }

        /// <summary>
        /// List of species facts corresponding to all combinations
        /// of taxa, individual categories, factors, hosts and periods
        /// listed in the selection in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        SpeciesFactList SpeciesFacts { get; }

        /// <summary>
        /// Taxa in the species fact data set.
        /// This list is never set to null.
        /// </summary>
        TaxonList Taxa { get; }

        /// <summary>
        /// Add factors to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          FactorList selection);

        /// <summary>
        /// Add factor to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          IFactor selection);

        /// <summary>
        /// Add individual category to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          IIndividualCategory selection);

        /// <summary>
        /// Add individual categories to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          IndividualCategoryList selection);

        /// <summary>
        /// Add period to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          IPeriod selection);

        void AddSelection(IUserContext userContext,
                          IReference selection);
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
        void AddSelection(IUserContext userContext,
                          ISpeciesFactDataSetSelection selection);

        /// <summary>
        /// Add periods to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelection(IUserContext userContext,
                          PeriodList selection);

        void AddSelection(IUserContext userContext,
                       ReferenceList selection);

        /// <summary>
        /// Add host to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelectionHost(IUserContext userContext,
                              ITaxon selection);

        /// <summary>
        /// Add hosts to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelectionHosts(IUserContext userContext,
                               TaxonList selection);

        /// <summary>
        /// Add taxa to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelectionTaxa(IUserContext userContext,
                              TaxonList selection);

        /// <summary>
        /// Add taxon to current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void AddSelectionTaxon(IUserContext userContext,
                               ITaxon selection);

        /// <summary>
        /// Make automated calculations of species facts
        /// that are "automatic" in a species fact list. 
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        void InitAutomatedCalculations(IUserContext userContext);

        /// <summary>
        /// Remove factors from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             FactorList selection);

        /// <summary>
        /// Remove factor from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             IFactor selection);

        /// <summary>
        /// Remove individual category from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             IIndividualCategory selection);

        /// <summary>
        /// Remove individual categories from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             IndividualCategoryList selection);

        /// <summary>
        /// Remove period from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             IPeriod selection);
        /// <summary>
        /// Remove reference from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="selection"></param>
        void RemoveSelection(IUserContext userContext,
                            IReference selection);

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
        void RemoveSelection(IUserContext userContext,
                             ISpeciesFactDataSetSelection selection);

        /// <summary>
        /// Remove periods from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelection(IUserContext userContext,
                             PeriodList selection);
        /// <summary>
        /// Remove references from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="selection"></param>
        void RemoveSelection(IUserContext userContext,
                            ReferenceList selection);
        /// <summary>
        /// Remove host from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelectionHost(IUserContext userContext,
                                 ITaxon selection);

        /// <summary>
        /// Remove hosts from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelectionHosts(IUserContext userContext,
                                  TaxonList selection);

        /// <summary>
        /// Remove taxa from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelectionTaxa(IUserContext userContext,
                                 TaxonList selection);

        /// <summary>
        /// Remove taxon from current species fact data set scope.
        /// The species facts in the data set are updated
        /// to the new species fact data set scope.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="selection">Changed scope of the data set.</param>
        void RemoveSelectionTaxon(IUserContext userContext,
                                  ITaxon selection);

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
        void UpdateSelection(IUserContext userContext,
                             ISpeciesFactDataSetSelection selection);
    }
}
