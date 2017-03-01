using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Subscription
{
    /// <summary>
    /// View manager for subscriptions.
    /// </summary>
    public class SubscriptionViewManager
    {
        private IUserContext _userContext;
        private static List<int> _taxonIds;

        static SubscriptionViewManager()
        {
            _taxonIds = new List<int> { 1, 2, 3, 4 };
        }

        public SubscriptionViewManager(IUserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Creates the subscriptions view model.
        /// </summary>
        /// <param name="taxon">The current taxon.</param>
        /// <returns>A view model.</returns>
        public SubscriptionsViewModel CreateSubscriptionsViewModel(ITaxon taxon)
        {
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(_userContext, _taxonIds);
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();
            SubscriptionsViewModel model = new SubscriptionsViewModel();
            model.Subscriptions = taxaList;
            model.CurrentTaxon = TaxonViewModel.CreateFromTaxon(taxon);
            return model;
        }

        /// <summary>
        /// Subscribes to the specified taxon.
        /// </summary>
        /// <param name="taxonId">The taxon identifier.</param>
        public void Subscribe(int taxonId)
        {
            _taxonIds.Add(taxonId);
        }

        /// <summary>
        /// Unsubscribes the specified taxon.
        /// </summary>
        /// <param name="taxonId">The taxon identifier.</param>
        public void Unsubscribe(int taxonId)
        {
            _taxonIds.Remove(taxonId);
        }
    }
}
