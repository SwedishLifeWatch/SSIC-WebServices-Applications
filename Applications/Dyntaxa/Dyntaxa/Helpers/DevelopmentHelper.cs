using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace Dyntaxa.Helpers
{
    /// <summary>
    /// Helper methods that should be used in development.
    /// Should not be used in release
    /// </summary>
    public static class DevelopmentHelper
    {
        public static ITaxonRevision GetDefaultRevision()
        {
            IUserContext user = CoreData.UserManager.GetCurrentUser();
            return CoreData.TaxonManager.GetTaxonRevision(user, DefaultRevisionId);
        }

        public static int DefaultRevisionId
        {
            get { return 1; }
        }
    }
}