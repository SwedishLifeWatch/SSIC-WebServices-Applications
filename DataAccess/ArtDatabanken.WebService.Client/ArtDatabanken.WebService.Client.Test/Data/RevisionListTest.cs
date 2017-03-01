using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RevisionListTest : TestBase
    {
        private TaxonRevisionList _revision;

        public RevisionListTest()
        {
            _revision = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonRevisionList revision;

            revision = new TaxonRevisionList();
            Assert.IsNotNull(revision);
        }

        [TestMethod]
        public void Get()
        {
            GetRevision(true);
            foreach (ITaxonRevision revision in GetRevision())
            {
                Assert.AreEqual(revision, GetRevision().Get(revision.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 revisionId;

            revisionId = Int32.MaxValue;
            GetRevision(true).Get(revisionId);
        }

        private TaxonRevisionList GetRevision()
        {
            return GetRevision(false);
        }

        private TaxonRevisionList GetRevision(Boolean refresh)
        {
            if (_revision.IsNull() || refresh)
            {
                _revision = new TaxonRevisionList();
                _revision.Add(RevisionTest.GetRevision(GetUserContext()));
            }
            return _revision;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            TaxonRevisionList newRevisionList, oldRevisionList;
            Int32 taxonIndex;

            oldRevisionList = GetRevision(true);
            newRevisionList = new TaxonRevisionList();
            for (taxonIndex = 0; taxonIndex < oldRevisionList.Count; taxonIndex++)
            {
                newRevisionList.Add(oldRevisionList[oldRevisionList.Count - taxonIndex - 1]);
            }
            for (taxonIndex = 0; taxonIndex < oldRevisionList.Count; taxonIndex++)
            {
                Assert.AreEqual(newRevisionList[taxonIndex], oldRevisionList[oldRevisionList.Count - taxonIndex - 1]);
            }
        }
    }
}
