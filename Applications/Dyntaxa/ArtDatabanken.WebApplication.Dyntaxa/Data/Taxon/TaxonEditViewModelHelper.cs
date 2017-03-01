using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
// ReSharper restore CheckNamespace

    /// <summary>
    /// The taxon edit view model helper.
    /// </summary>
    public class TaxonEditViewModelHelper
    {
        /// <summary>
        /// The _taxon.
        /// </summary>
        private readonly ITaxon _taxon;

        /// <summary>
        /// The _swedish occourrence fact.
        /// </summary>
        private SpeciesFact _swedishOccourrenceFact;

        /// <summary>
        /// The _swedish history fact.
        /// </summary>
        private SpeciesFact _swedishHistoryFact;

        /// <summary>
        /// The _user.
        /// </summary>
        private IUserContext _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonEditViewModelHelper"/> class.
        /// </summary>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        public TaxonEditViewModelHelper(ITaxon taxon, IUserContext user)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            _user = user;

            // this.revisionId = revisionId;
            _taxon = taxon;
                sp.Stop();
            Debug.WriteLine("TaxonSummary - Retrieving taxon and taxonCategory: {0:N0} milliseconds", sp.ElapsedMilliseconds);
            InitSpeciesFact(taxon);
        }

        /// <summary>
        /// Gets the swedish occurrence.
        /// </summary>
        public string SwedishOccurrence
        {
            get
            {
                if (_swedishOccourrenceFact == null)
                {
                    return "-";
                }

                FactorFieldEnumValue val = _swedishOccourrenceFact.MainField.Value as FactorFieldEnumValue;
                if (val != null)
                {
                    return val.OriginalLabel;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the swedish history.
        /// </summary>
        public string SwedishHistory
        {
            get
            {
                if (_swedishHistoryFact == null)
                {
                    return "-";
                }

                FactorFieldEnumValue val = _swedishHistoryFact.MainField.Value as FactorFieldEnumValue;
                if (val != null)
                {
                    return val.OriginalLabel;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Reads species fact.
        /// </summary>
        private void InitSpeciesFact()
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            try
            {                
                ISpeciesFactSearchCriteria speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
                speciesFactSearchCriteria.EnsureNoListsAreNull();
                speciesFactSearchCriteria.IncludeNotValidHosts = true;
                speciesFactSearchCriteria.IncludeNotValidTaxa = true;
                speciesFactSearchCriteria.Taxa = new TaxonList();
                speciesFactSearchCriteria.Taxa.Add(_taxon);

                var factorIds = new List<Int32> { (int)FactorId.SwedishOccurrence, (int)FactorId.SwedishHistory };
                FactorList factors = CoreData.FactorManager.GetFactors(_user, factorIds);
                speciesFactSearchCriteria.Factors = new FactorList();
                foreach (IFactor factor in factors)
                {
                    speciesFactSearchCriteria.Factors.Add(factor);
                }

                SpeciesFactList speciesFacts = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(_user, speciesFactSearchCriteria);
                foreach (SpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.Factor.Id == (int)FactorId.SwedishOccurrence)
                    {
                        _swedishOccourrenceFact = speciesFact;
                    }

                    if (speciesFact.Factor.Id == (int)FactorId.SwedishHistory)
                    {
                        _swedishHistoryFact = speciesFact;
                    }
                }
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteMessage("Dyntaxa - InitSpeciesFact: " + ex.Message);
            }

            sp.Stop();
            Debug.WriteLine("Retrieving species fact: {0:N0} milliseconds", sp.ElapsedMilliseconds);
        }

        /// <summary>
        /// Reads species fact.
        /// </summary>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        private void InitSpeciesFact(ITaxon taxon)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            try
            {                
                Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetCommonDyntaxaSpeciesFacts(_user, taxon);
                dicSpeciesFacts.TryGetValue(FactorId.SwedishOccurrence, out _swedishOccourrenceFact);
                dicSpeciesFacts.TryGetValue(FactorId.SwedishHistory, out _swedishHistoryFact);                
            }
            catch (Exception)
            {
                // the taxon did not exist in Artfakta
            }

            sp.Stop();
            Debug.WriteLine("Retrieving species fact: {0:N0} milliseconds", sp.ElapsedMilliseconds);
        }
    }
}
