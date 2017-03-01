using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    // TODO implemnt more test if needed
    [TestClass]
    public class RevisionTest : TestBase
    {
        TaxonRevision _taxonRevision;

        public RevisionTest()
        {
            _taxonRevision = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonRevision taxonRevision;

            taxonRevision = new TaxonRevision();
            Assert.IsNotNull(taxonRevision);
        }

        public static TaxonRevision GetRevision(IUserContext userContext)
        {
            return new TaxonRevision();
        }

        private TaxonRevision GetRevision()
        {
            return GetRevision(false);
        }

        private TaxonRevision GetRevision(Boolean refresh)
        {
            if (_taxonRevision.IsNull() || refresh)
            {
                _taxonRevision = new TaxonRevision();
            }
            return _taxonRevision;
        }
    }
}
