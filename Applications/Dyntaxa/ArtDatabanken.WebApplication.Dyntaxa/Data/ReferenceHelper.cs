using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using ReferenceList = ArtDatabanken.Data.ReferenceList;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Helper functions used to get information from ArtFakta.
    /// </summary>
    public static class ReferenceHelper
    {
#if DEBUG
        private static readonly int DyntaxaReference = Resources.DyntaxaSettings.Default.DyntaxaDefaultReferenceIdMoneses;
#else
        private static readonly int DyntaxaReference = Resources.DyntaxaSettings.Default.DyntaxaDefaultReferenceId;        
#endif

        /// <summary>
        /// Get default reference from revision
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="newObject"></param>
        /// <param name="taxonRevision"></param>
        /// <param name="referencesToAdd"> </param>
        /// <returns></returns>
        public static ReferenceRelationList GetDefaultReferences(IUserContext userContext, object newObject, ITaxonRevision taxonRevision, ReferenceRelationList referencesToAdd)
        {
            // Check if list is null the create a new one
            var referencesToAddTemp = new ReferenceRelationList();
            if (referencesToAdd.IsNotNull())
            {
                referencesToAddTemp = referencesToAdd;
            }

            if (taxonRevision.GetReferences(userContext).IsNotEmpty())
            {
                // Set all revision references of type Source as references
                foreach (var referenceRelation in taxonRevision.GetReferences(userContext))
                {
                    if (referenceRelation.Type.Id == (int)ReferenceRelationTypeId.Source)
                    {
                        IReference newReference = new ArtDatabanken.Data.Reference();
                        newReference.Id = referenceRelation.ReferenceId;
                        IReferenceRelation newReferenceRelation = GetReferenceRelation(newObject, newReference);
                        if (newReferenceRelation.IsNotNull())
                        {
                            referencesToAddTemp.Add(newReferenceRelation);
                        }
                    }
                }

                // Get first reference of type Used in and set as Source reference.
                if (referencesToAddTemp.Count == 0)
                {
                    IReference newReference = new ArtDatabanken.Data.Reference();
                    newReference.Id = taxonRevision.GetReferences(userContext).First().ReferenceId;                    
                    IReferenceRelation newReferenceRelation = GetReferenceRelation(newObject, newReference);
                    if (newReferenceRelation.IsNotNull())
                    {
                        referencesToAddTemp.Add(newReferenceRelation);
                    }
                }
            }

            // If no references found set dyntaxa as reference
            else
            {
                IReference newReference = new ArtDatabanken.Data.Reference();
                newReference.Id = DyntaxaReference;
                IReferenceRelation newReferenceRelation = GetReferenceRelation(newObject, newReference);
                if (newReferenceRelation.IsNotNull())
                {
                    referencesToAddTemp.Add(newReferenceRelation);
                }
            }

            return referencesToAddTemp;
        }

        private static IReferenceRelation GetReferenceRelation(object newObject, IReference newReference)
        {
            if (newObject is ITaxon)
            {
                IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                ReferenceRelation newReferenceRelation = new ReferenceRelation()
                {
                    RelatedObjectGuid = ((ITaxon)newObject).Guid,
                    Type = CoreData.ReferenceManager.GetReferenceRelationType(userContext, ReferenceRelationTypeId.Source),
                    Reference = null,
                    ReferenceId = newReference.Id
                };
                return newReferenceRelation;
            }

            if (newObject is ITaxonName)
            {
                IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                ReferenceRelation newReferenceRelation = new ReferenceRelation()
                {
                    RelatedObjectGuid = ((ITaxonName)newObject).Guid,
                    Type = CoreData.ReferenceManager.GetReferenceRelationType(userContext, ReferenceRelationTypeId.Source),
                    Reference = null,
                    ReferenceId = newReference.Id
                };
                return newReferenceRelation;
            }

            return null;
        }

        /// <summary>
        /// Get dyntaxa reference relation
        /// </summary>
        /// <param name="taxonRevision"></param>
        /// <returns></returns>
        public static List<IReferenceRelation> GetDyntaxaRevisionReferenceRelation(ITaxonRevision taxonRevision)
        {
            var referencesToAdd = new List<IReferenceRelation>(); 

            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            ReferenceRelation newReferenceRelation = new ReferenceRelation()
            {
                RelatedObjectGuid = taxonRevision.Guid,
                Type = CoreData.ReferenceManager.GetReferenceRelationType(userContext, ReferenceRelationTypeId.Source),
                Reference = null,
                ReferenceId = DyntaxaReference
            };
            referencesToAdd.Add(newReferenceRelation);
            return referencesToAdd;                  
        }

        public static List<IReference> GetReferenceList(IUserContext userContext)
        {
            ReferenceList referenceList = CoreData.ReferenceManager.GetReferences(userContext);
            return referenceList.ToList();            
        }

        public static List<ReferenceViewModel> GetReferences(string guid)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            ReferenceRelationTypeList referenceTypes = CoreData.ReferenceManager.GetReferenceRelationTypes(userContext);
            ReferenceRelationList references = CoreData.ReferenceManager.GetReferenceRelations(userContext, guid);
            
            var list = new List<ReferenceViewModel>();

            foreach (IReferenceRelation referenceRelation in references)
            {
                IReference reference = referenceRelation.GetReference(userContext);
                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                list.Add(new ReferenceViewModel(reference.Id, reference.Name, year, reference.Title, referenceRelation.Type.Description, referenceRelation.Type.Id));
            }

            return list;
        }

        /// <summary>
        /// Remove old source reference for taxon.
        /// </summary>
        /// <param name="taxonRevision"></param>
        /// <param name="replacingTaxon"></param>
        /// <returns></returns>
        public static ReferenceRelationList RemoveTaxonSourceReferences(IUserContext userContext, ITaxonRevision taxonRevision, ITaxon replacingTaxon)
        {
            // Must remove old source reference though
            ReferenceRelationList referencesToRemove = new ReferenceRelationList();
            foreach (IReferenceRelation oldReferenceRelation in replacingTaxon.GetReferenceRelations(userContext))
            {
                if (oldReferenceRelation.Type.Id == (int)ReferenceRelationTypeId.Source)
                {
                    referencesToRemove.Add(oldReferenceRelation);
                }
            }

            return referencesToRemove;
        }
    }
}
