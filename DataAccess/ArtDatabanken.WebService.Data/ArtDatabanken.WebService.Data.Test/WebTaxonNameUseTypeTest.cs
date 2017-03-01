// -----------------------------------------------------------------------
// <copyright file="WebTaxonNameUseTypeTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebNameUsageTest : WebDomainTestBase<WebTaxonNameUsage>
    {
        [TestMethod]
        public void Id()
        {
            var id = 1;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void NameString()
        {
            var nameString = "name";
            GetObject(true).NameString = nameString;
            Assert.AreEqual(GetObject().NameString, nameString);
        }

        [TestMethod]
        public void DescriptionString()
        {
            var descriptionString = "description";
            GetObject(true).DescriptionString = descriptionString;
            Assert.AreEqual(GetObject().DescriptionString, descriptionString);
        }
    }
}
