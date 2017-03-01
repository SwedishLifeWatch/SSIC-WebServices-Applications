// -----------------------------------------------------------------------
// <copyright file="TaxonNameCategoryTest.cs" company="Microsoft">
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
    public class WebTaxonNameCategoryTest : WebDomainTestBase<WebTaxonNameCategory>
    {
        [TestMethod]
        public void Id()
        {
            var id = 1;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void CategoryName()
        {
            var categoryName = "Test";
            GetObject(true).Name = categoryName;
            Assert.AreEqual(GetObject().Name, categoryName);            
        }

        [TestMethod]
        public void ShortName()
        {
            var shortName = "shortName";
            GetObject(true).ShortName = shortName;
            Assert.AreEqual(GetObject().ShortName, shortName);
        }

        [TestMethod]
        public void SortOrder()
        {
            var sortOrder = 1;
            GetObject(true).SortOrder = sortOrder;
            Assert.AreEqual(GetObject().SortOrder, sortOrder);
        }

    }
}
