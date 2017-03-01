using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Subscription;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken;
using ArtDatabanken.Security;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace Dyntaxa.Controllers
{
    /// <summary>
    /// Controller that handle Subscriptions.
    /// </summary>
    public class SubscriptionController : DyntaxaBaseController
    {
        /// <summary>
        /// Renders a page where the user can manage its subscriptions.
        /// </summary>
        /// <param name="taxonId">The current taxon identifier.</param>
        /// <returns>View that should be rendered.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult Subscriptions(string taxonId)
        {
            if (taxonId.IsNull())
            {
                taxonId = this.TaxonIdentifier.Id.ToString();
            }

            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;          
            SubscriptionViewManager subscriptionViewManager = new SubscriptionViewManager(GetCurrentUser());
            SubscriptionsViewModel model = subscriptionViewManager.CreateSubscriptionsViewModel(taxon);
            return View(model);
        }

        /// <summary>
        /// Unsubscribe a taxon.
        /// </summary>
        /// <param name="unsubscribeTaxonId">Id of the taxon that the user will unsubscribe.</param>
        /// <returns>View that should be rendered.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult Unsubscribe(int unsubscribeTaxonId)
        {
            SubscriptionViewManager subscriptionViewManager = new SubscriptionViewManager(GetCurrentUser());
            subscriptionViewManager.Unsubscribe(unsubscribeTaxonId);
            return RedirectToAction("Subscriptions");            
        }

        /// <summary>
        /// Subscribes to a taxon.
        /// </summary>
        /// <param name="subscribeTaxonId">Id of the taxon that the user will subscribe to.</param>
        /// <returns>View that should be rendered.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult Subscribe(int subscribeTaxonId)
        {
            SubscriptionViewManager subscriptionViewManager = new SubscriptionViewManager(GetCurrentUser());
            subscriptionViewManager.Subscribe(subscribeTaxonId);
            return RedirectToAction("Subscriptions");
        }
    }
}
