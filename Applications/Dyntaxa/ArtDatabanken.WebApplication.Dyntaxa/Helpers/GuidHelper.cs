using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Helpers
{
    public enum DyntaxaObjectType
    {
        Unknown,
        Taxon,
        TaxonName,
        Revision
    }

    public static class GuidHelper
    {
        private static string GetTypeFromGuid(string guid)
        {
            string[] stringParts = guid.Split(':');
            if (stringParts.Length == 5)
            {
                return stringParts[3];
            }

            return null;
        }

        public static DyntaxaObjectType GetObjectTypeFromGuid(string guid)
        {
            string strObjectType = GetTypeFromGuid(guid);
            switch (strObjectType)
            {
                case "Taxon":
                    return DyntaxaObjectType.Taxon;
                case "TaxonName":
                    return DyntaxaObjectType.TaxonName;
                case "Revision":
                    return DyntaxaObjectType.Revision;
                default:
                    return DyntaxaObjectType.Unknown;
            }
        }        

        public static string GetObjectDescriptionFromGuid(string guid)
        {
            DyntaxaObjectType type = GetObjectTypeFromGuid(guid);
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            try
            {
                switch (type)
                {
                    case DyntaxaObjectType.Taxon:
                        return GetTaxonDescription(CoreData.TaxonManager.GetTaxon(userContext, guid));
                    case DyntaxaObjectType.TaxonName:
                        return GetTaxonNameDescription(CoreData.TaxonManager.GetTaxonName(userContext, guid));
                    case DyntaxaObjectType.Revision:
                        return GetRevisionDescription(CoreData.TaxonManager.GetTaxonRevision(userContext, guid));
                    case DyntaxaObjectType.Unknown:
                        return "";
                    default:
                        return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string GetRevisionDescription(ITaxonRevision taxonRevision)
        {
            if (taxonRevision == null)
            {
                return "";
            }
            else
            {
                string taxonName;
                if (taxonRevision.RootTaxon.ScientificName.IsNotEmpty())
                {
                    taxonName = taxonRevision.RootTaxon.ScientificName;
                }
                else
                {
                    taxonName = string.Format("TaxonId: {0}", taxonRevision.RootTaxon.Id);
                }

                return string.Format("{0}: {1}", Resources.DyntaxaResource.ReferenceGuidObjectInfoRevisionText, taxonName);
            }
        }

        private static string GetTaxonDescription(ITaxon taxon)
        {
            if (taxon == null)
            {
                return "";
            }

            return taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
        }

        private static string GetTaxonNameDescription(ITaxonName taxonName)
        {
            return taxonName == null ? "" : taxonName.Name;
        }
    }
}
