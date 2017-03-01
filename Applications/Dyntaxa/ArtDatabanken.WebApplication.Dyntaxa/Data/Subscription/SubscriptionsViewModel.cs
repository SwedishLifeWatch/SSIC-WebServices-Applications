using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Subscription
{
    /// <summary>
    /// View model for subscriptions.
    /// </summary>
    public class SubscriptionsViewModel
    {
        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>        
        public List<TaxonViewModel> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the current taxon.
        /// </summary>        
        public TaxonViewModel CurrentTaxon { get; set; }
    }
}
