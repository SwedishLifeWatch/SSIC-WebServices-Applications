using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using Dyntaxa.Helpers;

namespace Dyntaxa.Test.TestModels
{
    class ReferenceDataSource : IReferenceDataSource

    {
        public IDataSourceInformation GetDataSourceInformation()
        {
            throw new NotImplementedException();
        }

        public void CreateReference(IUserContext userContext, IReference reference)
        {
            throw new NotImplementedException();
        }

        public ReferenceList GetReferences(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public ReferenceList GetReferences(IUserContext userContext, List<int> referenceIds)
        {
            throw new NotImplementedException();
        }

        public ReferenceList GetReferences(IUserContext userContext, IReferenceSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public void UpdateReference(IUserContext userContext, IReference reference)
        {
            throw new NotImplementedException();
        }

        public void CreateReferenceRelation(IUserContext userContext, IReferenceRelation referenceRelation)
        {
            throw new NotImplementedException();
        }

        public void DeleteReferenceRelation(IUserContext userContext, IReferenceRelation referenceRelation)
        {
            throw new NotImplementedException();
        }

        public IReferenceRelation GetReferenceRelation(IUserContext userContext, int referenceRelationId)
        {
            throw new NotImplementedException();
        }

        public ReferenceRelationList GetReferenceRelations(IUserContext userContext, string relatedObjectGuid)
        {
            ReferenceRelationList list = new ReferenceRelationList();
            list.Add(new ReferenceRelation() { });
            return list;

        }

        public ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }
    }
}
