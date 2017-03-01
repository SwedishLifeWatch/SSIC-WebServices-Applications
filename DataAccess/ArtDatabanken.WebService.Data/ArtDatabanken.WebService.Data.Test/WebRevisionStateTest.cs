// -----------------------------------------------------------------------
// <copyright file="WebRevisionStateTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebRevisionStateTest : WebDomainTestBase<WebTaxonRevisionState>
    {
        [TestMethod]
        public void Id()
        {
            var value = 1;
            GetObject(true).Id = value;
            Assert.AreEqual(GetObject().Id, value);
        }

        [TestMethod]
        public void StateString()
        {
            var value = "CheckedOut";
            GetObject(true).Description = value;
            Assert.AreEqual(GetObject().Description, value);
        }
    }
}
