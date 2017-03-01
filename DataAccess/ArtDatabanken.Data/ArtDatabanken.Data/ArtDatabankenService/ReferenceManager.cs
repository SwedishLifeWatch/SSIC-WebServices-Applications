using System;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of reference related objects.
    /// </summary>
    public class ReferenceManager : ManagerBase
    {
        private static ReferenceList _references = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReferenceManager()
        {
            ManagerBase.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _references thread safe.
        /// </summary>
        private static ReferenceList References
        {
            get
            {
                ReferenceList references;

                lock (_lockObject)
                {
                    references = _references;
                }
                return references;
            }
            set
            {
                lock (_lockObject)
                {
                    _references = value;
                }
            }
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        public static void CreateReference(Reference reference)
        {
            WebReference webReference = new WebReference();

            // Check arguments.
            reference.CheckNotNull("reference");

            webReference.Id = 0;
#if DATA_SPECIFIED_EXISTS
            webReference.IdSpecified = true;
#endif
            webReference.Name = reference.Name;
            webReference.Year = reference.Year;
#if DATA_SPECIFIED_EXISTS
            webReference.YearSpecified = true;
#endif
            webReference.Text = reference.Text;

            WebServiceClient.CreateReference(webReference);

            //Make sure that we load new references when we reach LoadReferences
            References = null;
        }

        /// <summary>
        /// Get the requested references.
        /// </summary>
        /// <param name='referenceId'>Id of requested reference.</param>
        /// <returns>Requested reference.</returns>
        /// <exception cref="ArgumentException">Thrown if no reference has the requested id.</exception>
        public static Reference GetReference(Int32 referenceId)
        {
#if (DEBUG)
            // TODO: This code avoids a temporary data bug in the 
            // database. It should be removed as soon as possible.
            Reference reference;

            reference = (Reference)(GetReferences().Find(referenceId));
            if (reference.IsNull())
            {
                reference = GetReferences()[0];
            }
            return reference;
#else
            return GetReferences().Get(referenceId);
#endif
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <returns>All references.</returns>
        public static ReferenceList GetReferences()
        {
            return GetReferences(false);
        }

       
        /// <summary>
        /// Get all references that match a search string.
        /// </summary>
        /// <param name="searchString">Search string.</param>
        /// <returns>All references that matches the search string.</returns>
        public static ReferenceList GetReferences(String searchString)
        {
            ReferenceList references = new ReferenceList();

            // Get data from web service.
            references = new ReferenceList();
            foreach (WebReference webReference in WebServiceClient.GetReferencesBySearchString(searchString))
            {
                references.Add(new Reference(webReference.Id,
                                             webReference.Name,
                                             webReference.Year,
                                             webReference.Text));
            }
            return references;
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name='refresh'>
        /// Indicates if data cache should be updated
        /// with information from the web service.
        /// </param>
        /// <returns>All references.</returns>
        public static ReferenceList GetReferences(Boolean refresh)
        {
            ReferenceList references = null;

            if (refresh)
            {
                References = null;
            }
            for (Int32 getAttempts = 0; (references.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadReferences();
                references = References;
            }
            return references;
        }

        /// <summary>
        /// Get all references from web service.
        /// </summary>
        private static void LoadReferences()
        {
            ReferenceList references;

            if (References.IsNull())
            {
                // Get data from web service.
                references = new ReferenceList(true);
                foreach (WebReference webReference in WebServiceClient.GetReferences())
                {
                    references.Add(new Reference(webReference.Id,
                                                 webReference.Name,
                                                 webReference.Year,
                                                 webReference.Text));
                }
                References = references;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            References = null;
        }

        /// <summary>
        /// Update a referene with specific id in the database.
        /// </summary>
        /// <param name='reference'>Reference to be updated.</param>
        public static void UpdateReference(Reference reference)
        {
            WebReference updatereference = new WebReference();

            // Check arguments.
            reference.CheckNotNull("reference");

            updatereference.Id = reference.Id;
#if DATA_SPECIFIED_EXISTS
            updatereference.IdSpecified = true;
#endif
            updatereference.Name = reference.Name;
            updatereference.Year = reference.Year;
#if DATA_SPECIFIED_EXISTS
            updatereference.YearSpecified = true;
#endif
            updatereference.Text = reference.Text;

            WebServiceClient.UpdateReference(updatereference);
            
            //Make sure that we load new references when we reach LoadReferences
            References = null;
        }
    }
}
