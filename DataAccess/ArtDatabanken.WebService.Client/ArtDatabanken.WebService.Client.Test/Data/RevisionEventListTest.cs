using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RevisionEventListTest : TestBase
    {
        private TaxonRevisionEventList _revisionEvent;

        public RevisionEventListTest()
        {
            _revisionEvent = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonRevisionEventList revisionEvent;

            revisionEvent = new TaxonRevisionEventList();
            Assert.IsNotNull(revisionEvent);
        }

        [TestMethod]
        public void Get()
        {
            GetRevisionEvent(true);
            foreach (ITaxonRevisionEvent revisionEvent in GetRevisionEvent())
            {
                Assert.AreEqual(revisionEvent, GetRevisionEvent().Get(revisionEvent.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 revisionEventId;

            revisionEventId = Int32.MaxValue;
            GetRevisionEvent(true).Get(revisionEventId);
        }

        private TaxonRevisionEventList GetRevisionEvent(Boolean refresh = false)
        {
            if (_revisionEvent.IsNull() || refresh)
            {
                _revisionEvent = new TaxonRevisionEventList();
                _revisionEvent.Add(RevisionEventTest.GetRevisionEvent(GetUserContext()));
            }
            return _revisionEvent;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            TaxonRevisionEventList newRevisionEventList, oldRevisionEventList;
            Int32 taxonIndex;

            oldRevisionEventList = GetRevisionEvent(true);
            newRevisionEventList = new TaxonRevisionEventList();
            for (taxonIndex = 0; taxonIndex < oldRevisionEventList.Count; taxonIndex++)
            {
                newRevisionEventList.Add(oldRevisionEventList[oldRevisionEventList.Count - taxonIndex - 1]);
            }
            for (taxonIndex = 0; taxonIndex < oldRevisionEventList.Count; taxonIndex++)
            {
                Assert.AreEqual(newRevisionEventList[taxonIndex], oldRevisionEventList[oldRevisionEventList.Count - taxonIndex - 1]);
            }
        }
    }
}
