using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    // TODO implemnt more test if needed
    [TestClass]
    public class RevisionEventTest : TestBase
    {
        TaxonRevisionEvent _taxonRevisionEvent;

        public RevisionEventTest()
        {
            _taxonRevisionEvent = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonRevisionEvent taxonRevisionEvent;

            taxonRevisionEvent = new TaxonRevisionEvent();
            Assert.IsNotNull(taxonRevisionEvent);
        }
        
        public static TaxonRevisionEvent GetRevisionEvent(IUserContext userContext)
        {
            return new TaxonRevisionEvent();
        }

        private TaxonRevisionEvent GetRevisionEvent()
        {
            return GetRevisionEvent(false);
        }

        private TaxonRevisionEvent GetRevisionEvent(Boolean refresh)
        {
            if (_taxonRevisionEvent.IsNull() || refresh)
            {
                _taxonRevisionEvent = new TaxonRevisionEvent();
            }
            return _taxonRevisionEvent;
        }
    }
}
