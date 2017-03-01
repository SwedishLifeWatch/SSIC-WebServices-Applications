using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public static class DyntaxaHelper
    {
        private static ITaxonRevision GetRevisionFromSession()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session["Revision"] as ITaxonRevision;                
            }
            return null;
        }

        public static bool IsInRevision()
        {
            ITaxonRevision taxonRevision = GetRevisionFromSession();
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            if (taxonRevision != null)
            {
                return IsInRevision(userContext, taxonRevision);
            }

            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return false;
            }

            var revisionId = HttpContext.Current.Session["RevisionId"] as int?;
            return IsInRevision(userContext, revisionId);
        }
   
        public static bool IsInRevision(IUserContext user, int? revisionId)
        {
            ITaxonRevision rev = GetRevisionFromSession();            
            if (rev != null)
            {
                return IsInRevision(user, rev);
            }

            if (!revisionId.HasValue)
            {
                return false;
            }

            ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(user, revisionId.Value);
            return user.IsTaxonRevisionEditor(taxonRevision);
        }

        public static bool IsInRevision(IUserContext user, ITaxonRevision taxonRevision)
        {
            if (taxonRevision == null)
            {
                return false;
            }

            return user.IsTaxonRevisionEditor(taxonRevision);
        }
    }
}
